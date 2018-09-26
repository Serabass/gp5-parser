using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using GTP5Parser.Binary;
using GTP5Parser.Tabs.Structure;

namespace GTP5Parser.Tabs
{
    partial class TabReader
    {
        private void ReadStructTab(Tab tab)
        {
            tab.Version = ReadStruct<Version>(ReadStructVersion).Value;
            tab.Meta = ReadStruct<TabMeta>(ReadStructTabMeta).Value;
            tab.LyricsTrack = Int32;

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

            tab.Moderate = Short;
            Skip(0x02);
            tab.HideTempo = Boolean;
            Skip(0x05);

            for (TrackMetaIterator = 0; TrackMetaIterator < 64; TrackMetaIterator++)
            {
                TrackMetaArray.Add(ReadStruct<TrackMeta>(ReadStructTrackMeta).Value);
            }

            Skip(0x26);
            var unk13 = Int32;
            tab.BarCount = Int32;
            tab.TracksCount = Int32;
            var unk12 = Byte;
            tab.Up = Byte;
            tab.Down = Byte;
            tab.KeySigns = this << 2;
            tab.Link8Notes = this << 4;

            // Skip(16);
            while (true)
            {
                SkipWhile(() => Byte == 0x00);
                if (Byte == 0x08)
                {
                    Back();
                    break;
                }
                Back();
                var i80 = Int32;
                tab.Bookmarks.Add(ReadStruct<Bookmark>(ReadStructBookmark).Value);
            }

            for (var i = 0; i < tab.TracksCount; i++)
            {
                tab.AddTrack(ReadStruct<Track>(ReadStructTrack).Value);
            }

            Skip(5);

            var xx = 0;

            while (!AtEnd)
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
            var match = Regex.Match(VersionString, @"v(?<major>\d+)\.(?<minor>\d+)$", RegexOptions.Singleline);

            if (!SupportedVersions.Contains(match.Value))
            {
                throw new VersionNotSupportedException(VersionString);
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
            lyrics.Start = Int32;
            lyrics.Content = LongString;
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
            trackMeta.Volume = Byte;
            trackMeta.Pan = Byte;
            trackMeta.Chorus = Byte;
            trackMeta.Reverb = Byte;
            trackMeta.Phaser = Byte;
            trackMeta.Tremolo = Byte;
            Skip(2);
        }

        private void ReadStructTrack(Track track)
        {
            track.Flags = Byte;
            track.Meta = TrackMetaArray[TrackMetaIterator];
            track.Title = this % 0x28;
            track.StringsCount = Int32;
            track.Tuning = new Note.MemoryBlock[track.StringsCount];

            for (var i = 0; i < track.StringsCount; i++)
            {
                var str = Int32;
                track.Tuning[i] = new Note.MemoryBlock(new Note(str));
            }

            var remainingStringTuningData = this << (7 - track.StringsCount) * 4;
            track.Port = Int32;
            track.MainChannel = Int32;
            track.EffectChannel = Int32;
            track.FretCount = Int32;
            track.Capo = Int32;
            track.Color = ReadColor();
            Skip(0x36);
            var o0 = ~this;
            var o1 = Int32;
            var o2 = ~this;
        }

        private void ReadStructChord(Chord chord)
        {
            chord.Flags = Byte;
            chord.length = ReadSByteEnum<ChordLength>().Value;

            var stringsBits = Byte;

            List<bool> bits = new List<bool>();

            for (var i = 0; i < 8; i++)
            {
                var bit = stringsBits.IsOnAt(i);

                if (!bit) continue;

                var flags = Byte;
                var bit2 = flags.IsOnAt(5);
                if (bit2)
                {
                    var unk211 = Byte;
                }
                var flags2 = Byte;
                var fret = Byte;
                var unk41 = Byte;
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
