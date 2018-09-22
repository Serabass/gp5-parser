using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace GTP5Parser
{
    class TabReader : BinaryReader
    {
        readonly Stream stream;

        public static string[] SupportedVersions = {
            "v5.10"
        };

        private List<TrackMeta> TrackMetaArray = new List<TrackMeta>();
        private int TrackMetaIterator;

        private bool atEnd => BaseStream.Position >= BaseStream.Length;

        public static Tab ReadTabFromStream(Stream stream)
        {
            return new TabReader(stream).ReadTab();
        }

        public TabReader(Stream stream) : base(stream)
        {
            this.stream = stream;
        }

        public Tab ReadTab()
        {
            using (var tab2 = ReadStruct<Tab>(ReadStructTab).Value)
            {
                return tab2;
            }
        }

        private void ReadStructTab(Tab tab)
        {
            var VersionString = ReadString();
            var match = Regex.Match(VersionString.Value, @"v(?<major>\d+)\.(?<minor>\d+)$", RegexOptions.Singleline);

            if (!SupportedVersions.Contains(match.Value))
            {
                throw new Exception($"Version {VersionString.Value} is not supported yet");
            }

            tab.Version.Major = int.Parse(match.Groups["major"].Value);
            tab.Version.Minor = int.Parse(match.Groups["minor"].Value);
            var unk = ReadBytes(10);
            tab.Title = ReadString();
            var unk2 = ReadInt32();
            tab.Subtitle = ReadString();
            var unk3 = ReadInt32();
            tab.Artist = ReadString();
            var unk4 = ReadInt32();
            tab.Album = ReadString();
            var unk5 = ReadInt32();
            tab.LyricsBy = ReadString();
            var unk6 = ReadInt32();
            tab.Music = ReadString();
            var unk7 = ReadInt32();
            tab.Copy = ReadString();
            var unk8 = ReadInt32();
            tab.TabAuthor = ReadString();
            var unk9 = ReadInt32();
            tab.Instructions = ReadString();
            var unk10 = ReadInt32();
            var unk10_1 = ReadInt32();
            tab.Notes = ReadString();
            tab.LyricsTrack = ReadInt32();

            for (int i = 0; i < 5; i++)
            {
                var lyrics = ReadStruct<Lyrics>(ReadStructLyrics).Value;
                tab.LyricsArray.Add(lyrics);
            }

            Debugger.Break();
            switch (tab.Version.Minor)
            {
                case 0:
                    Skip(0x1E);
                    break;
                case 10:
                    Skip(0x35);
                    break;
            }

            tab.Template = ReadStruct<Template>(ReadStructTemplate).Value;

            tab.Moderate = ReadInt16();
            Skip(0x02);
            tab.HideTempo = ReadBoolean();
            Skip(0x05);

            for (TrackMetaIterator = 0; TrackMetaIterator < 64; TrackMetaIterator++)
            {
                TrackMetaArray.Add(ReadStruct<TrackMeta>(ReadStructTrackMeta).Value);
            }

            Skip(0x26);
            var unk13 = ReadInt32();
            tab.BarCount = ReadInt32();
            tab.TracksCount = ReadInt32();
            var unk12 = ReadByte();
            tab.Up = ReadByte();
            tab.Down = ReadByte();
            tab.KeySigns = ReadBytes(2);
            tab.Link8Notes = ReadBytes(4);

            // Skip(16);
            while (true)
            {
                SkipWhile(() => ReadByte().Value == 0x00);
                if (ReadByte().Value == 0x08)
                {
                    Back();
                    break;
                }
                Back();
                var i80 = ReadInt32();
                Skip(5);
                tab.Bookmarks.Add(ReadStruct<Bookmark>(ReadStructBookmark).Value);
            }

            for (var i = 0; i < tab.TracksCount.Value; i++)
            {
                tab.AddTrack(ReadStruct<Track>(ReadStructTrack).Value);
            }

            Skip(5);

            var xx = 0;

            while (!atEnd)
            {
                xx++;
                tab.Chords.Add(ReadStruct<Chord>(ReadStructChord).Value);
            }

            Close();
            Dispose();
            Debugger.Break();
        }

        private void ReadStructLyrics(Lyrics lyrics)
        {
            lyrics.Start = ReadInt32();
            lyrics.Content = ReadLongString();
        }

        private void ReadStructTemplate(Template template)
        {
            template.Title = ReadString();
            Skip(4);
            template.Subtitle = ReadString();
            Skip(4);
            template.Artist = ReadString();
            Skip(4);
            template.Album = ReadString();
            Skip(4);
            template.WordsBy = ReadString();
            Skip(4);
            template.MusicBy = ReadString();
            Skip(4);
            template.WordsAndMusicBy = ReadString();
            Skip(4);
            template.Copyright = ReadString();
            Skip(4);
            template.Rights = ReadString();
            Skip(4);
            template.Page = ReadString();
            Skip(4);
            template.Moderate = ReadString();
        }

        private void ReadStructTrackMeta(TrackMeta trackMeta)
        {
            var offset = BaseStream.Position;
            trackMeta.Offset = offset;
            trackMeta.Instrument = ReadEnum<MidiInstruments>();
            trackMeta.Volume = ReadByte();
            trackMeta.Pan = ReadByte();
            trackMeta.Chorus = ReadByte();
            trackMeta.Reverb = ReadByte();
            trackMeta.Phaser = ReadByte();
            trackMeta.Tremolo = ReadByte();
            Skip(2);
        }

        private void ReadStructTrack(Track track)
        {
            track.Flags = ReadByte();
            track.Meta = TrackMetaArray[TrackMetaIterator];
            track.Title = ReadString(0x28);
            track.StringsCount = ReadInt32();
            track.Tuning = new MemoryBlock<Note>[track.StringsCount.Value];

            for (var o = 0; o < track.StringsCount.Value; o++)
            {
                var str = ReadInt32();
                track.Tuning[o] = new MemoryBlock<Note>(new Note(str.Value));
            }

            var remainingStringTuningData = ReadBytes((7 - track.StringsCount.Value) * 4);
            track.Port = ReadInt32();
            track.MainChannel = ReadInt32();
            track.EffectChannel = ReadInt32();
            track.FretCount = ReadInt32();
            track.Capo = ReadInt32();
            track.Color = ReadColor();
            Skip(0x36);
            var o0 = ReadString();
            var o1 = ReadInt32();
            var o2 = ReadString();
        }

        private void ReadStructChord(Chord chord)
        {
            chord.Flags = ReadByte().Value;
            var b = ReadSByte();
            chord.length = (ChordLength)b.Value;

            var stringsBits = ReadByte();

            List<bool> bits = new List<bool>();

            for (var i = 0; i < 8; i++)
            {
                var bit = (stringsBits.Value & (1 << i - 1)) != 0;

                if (!bit) continue;

                var flags = ReadByte();
                var bit2 = (flags.Value & (1 << 5 - 1)) != 0;
                if (bit2)
                {
                    var unk211 = ReadByte();
                }
                var flags2 = ReadByte();
                var fret = ReadByte();
                var unk41 = ReadByte();
                chord.notes[i] = fret.Value;
                Skip(2);
            }
        }

        private void ReadStructBookmark(Bookmark bookmark)
        {
            bookmark.Title = ReadString();
            bookmark.Color = ReadColor();
        }

    }
}
