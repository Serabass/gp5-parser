using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using GTP5Parser.Binary;
using GTP5Parser.Tabs.Structure;

namespace GTP5Parser.Tabs
{
    partial class TabReader : MyBinaryReader
    {
        private void ReadStructTab(Tab tab)
        {
            tab.Version = ReadStruct<Version>(ReadStructVersion).Value;
            tab.Meta = ReadStruct<TabMeta>(ReadStructTabMeta).Value;
            tab.LyricsTrack = ReadInt32();

            for (int i = 0; i < 5; i++)
            {
                var lyrics = ReadStruct<Lyrics>(ReadStructLyrics).Value;
                tab.LyricsArray.Add(lyrics);
            }

            Debugger.Break();
            switch (tab.Version.ToString())
            {
                case "v5.00":
                    Skip(0x1E);
                    break;
                case "v5.10":
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

        public void ReadStructVersion(Version version)
        {
            var VersionString = ReadString();
            var match = Regex.Match(VersionString.Value, @"v(?<major>\d+)\.(?<minor>\d+)$", RegexOptions.Singleline);

            if (!SupportedVersions.Contains(match.Value))
            {
                throw new VersionNotSupportedException(VersionString.Value);
            }

            Debugger.Break();

            version.Major = int.Parse(match.Groups["major"].Value);
            version.Minor = int.Parse(match.Groups["minor"].Value);
        }

        public void ReadStructTabMeta(TabMeta meta)
        {
            meta.Title = (ReadBytes(10), ReadString()).Item2;
            meta.Subtitle = (ReadInt32(), ReadString()).Item2;
            meta.Artist = (ReadInt32(), ReadString()).Item2;
            meta.Album = (ReadInt32(), ReadString()).Item2;
            meta.LyricsBy = (ReadInt32(), ReadString()).Item2;
            meta.Music = (ReadInt32(), ReadString()).Item2;
            meta.Copy = (ReadInt32(), ReadString()).Item2;
            meta.TabAuthor = (ReadInt32(), ReadString()).Item2;
            meta.Instructions = (ReadInt32(), ReadString()).Item2;
            meta.Notes = (ReadInt32(), ReadInt32(), ReadString()).Item3;
        }

        private void ReadStructLyrics(Lyrics lyrics)
        {
            lyrics.Start = ReadInt32();
            lyrics.Content = ReadLongString();
        }

        private void ReadStructTemplate(Template template)
        {
            template.Title = (ReadInt32(), ReadString()).Item2;
            template.Subtitle = (ReadInt32(), ReadString()).Item2;
            template.Artist = (ReadInt32(), ReadString()).Item2;
            template.Album = (ReadInt32(), ReadString()).Item2;
            template.WordsBy = (ReadInt32(), ReadString()).Item2;
            template.MusicBy = (ReadInt32(), ReadString()).Item2;
            template.WordsAndMusicBy = (ReadInt32(), ReadString()).Item2;
            template.Copyright = (ReadInt32(), ReadString()).Item2;
            template.Rights = (ReadInt32(), ReadString()).Item2;
            template.Page = (ReadInt32(), ReadString()).Item2;
            template.Moderate = (ReadInt32(), ReadString()).Item2;
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
            track.Tuning = new Note.MemoryBlock[track.StringsCount.Value];

            for (var i = 0; i < track.StringsCount.Value; i++)
            {
                var str = ReadInt32();
                track.Tuning[i] = new Note.MemoryBlock(new Note(str.Value));
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
