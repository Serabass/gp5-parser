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
            tab.KeySigns = this << 2;
            tab.Link8Notes = this << 4;

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
            var VersionString = ~this;
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
            meta.Title = (this << 10, ~this).Item2;
            meta.Subtitle = (this << 4, ~this).Item2;
            meta.Artist = (this << 4, ~this).Item2;
            meta.Album = (this << 4, ~this).Item2;
            meta.LyricsBy = (this << 4, ~this).Item2;
            meta.Music = (this << 4, ~this).Item2;
            meta.Copy = (this << 4, ~this).Item2;
            meta.TabAuthor = (this << 4, ~this).Item2;
            meta.Instructions = (this << 4, ~this).Item2;
            meta.Notes = (this << 4, ReadInt32(), ~this).Item3;
        }

        private void ReadStructLyrics(Lyrics lyrics)
        {
            lyrics.Start = ReadInt32();
            lyrics.Content = ReadLongString();
        }

        private void ReadStructTemplate(Template template)
        {
            template.Title = (this << 4, ~this).Item2;
            template.Subtitle = (this << 4, ~this).Item2;
            template.Artist = (this << 4, ~this).Item2;
            template.Album = (this << 4, ~this).Item2;
            template.WordsBy = (this << 4, ~this).Item2;
            template.MusicBy = (this << 4, ~this).Item2;
            template.WordsAndMusicBy = (this << 4, ~this).Item2;
            template.Copyright = (this << 4, ~this).Item2;
            template.Rights = (this << 4, ~this).Item2;
            template.Page = (this << 4, ~this).Item2;
            template.Moderate = (this << 4, ~this).Item2;
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
            track.Title = this % 0x28;
            track.StringsCount = ReadInt32();
            track.Tuning = new Note.MemoryBlock[track.StringsCount.Value];

            for (var i = 0; i < track.StringsCount.Value; i++)
            {
                var str = ReadInt32();
                track.Tuning[i] = new Note.MemoryBlock(new Note(str.Value));
            }

            var remainingStringTuningData = this << (7 - track.StringsCount.Value) * 4;
            track.Port = ReadInt32();
            track.MainChannel = ReadInt32();
            track.EffectChannel = ReadInt32();
            track.FretCount = ReadInt32();
            track.Capo = ReadInt32();
            track.Color = ReadColor();
            Skip(0x36);
            var o0 = ~this;
            var o1 = ReadInt32();
            var o2 = ~this;
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
                var bit = stringsBits.IsOnAt(i);

                if (!bit) continue;

                var flags = ReadByte();
                var bit2 = flags.IsOnAt(5);
                if (bit2)
                {
                    var unk211 = ReadByte();
                }
                var flags2 = ReadByte();
                var fret = ReadByte();
                var unk41 = ReadByte();
                chord.notes[i] = fret;
                Skip(2);
            }
        }

        private void ReadStructBookmark(Bookmark bookmark)
        {
            bookmark.Title = ~this;
            bookmark.Color = ReadColor();
        }
    }
}
