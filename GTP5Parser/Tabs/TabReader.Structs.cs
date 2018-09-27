using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using GTP5Parser.Tabs.Structure;
using GTP5Parser.Tabs.Structure.Meta;
using Version = GTP5Parser.Tabs.Structure.Version;

namespace GTP5Parser.Tabs
{
    partial class TabReader
    {
        private Tab ReadStructTab(Tab tab)
        {
            tab.Version = ReadStruct<Version>(ReadStructVersion).Value;
            Skip(0x06); // TODO Learn
            tab.Meta = ReadStruct<TabMeta>(ReadStructTabMeta).Value;
            tab.LyricsTrack = Int32;
            for (int i = 0; i < 5; i++)
            {
                var lyrics = ReadStruct<Lyrics>(ReadStructLyrics).Value;
                tab.LyricsArray.Add(lyrics);
            }


            switch (tab.Version.ToString())
            {
                case "v5.00":
                    Skip(0x1E);
                    break;
                case "v5.10":
                    Skip(0x35); // TODO Learn it!
                    break;
                default:
                    Skip(0x35);
                    break;
            }

            tab.Template = ReadStruct<Template>(ReadStructTemplate).Value;
            tab.Moderate = Short;
            Skip(0x02); // Always 00 00
            tab.HideTempo = Boolean;
            Skip(0x05); // TODO Learn
            for (_trackMetaIterator = 0; _trackMetaIterator < 64; _trackMetaIterator++)
            {
                TrackMetaArray.Add(ReadStruct<TrackMeta>(ReadStructTrackMeta).Value);
            }

            Skip(0x26); // TODO Learn
            var unk13 = Int32; // TODO Learn
            tab.BarCount = Int32;
            tab.TracksCount = Int32;
            var unk12 = Byte; // TODO Learn
            tab.Up = Byte;
            tab.Down = Byte;

            tab.KeySigns = this << 2;
            tab.Link8Notes = this << 4;

            Console.WriteLine("{0}, {1}", BaseStream.Position.ToString("X5"), Path);
            return tab;

            switch (Path)
            {
                case "re.gp5":
                    Skip(0x14);
                    break;
                default:
                    Debugger.Break();
                    break;
            }

            tab.Bookmarks.Add(ReadStruct<Bookmark>(ReadStructBookmark).Value);

            return tab;
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

        public Version ReadStructVersion(Version version)
        {
            var versionString = String;
            const string pattern = @"FICHIER GUITAR PRO (?<version>v(?<major>\d+)\.(?<minor>\d+))$";

            if (!Regex.IsMatch(versionString, pattern))
            {
                _stream.Close();
                throw new UnknownTabHeaderException();
            }

            var match = Regex.Match(versionString, pattern, RegexOptions.Singleline);

            version.Major = int.Parse(match.Groups["major"].Value);
            version.Minor = int.Parse(match.Groups["minor"].Value);
            return version;
        }

        public TabMeta ReadStructTabMeta(TabMeta meta)
        {
            meta._Title = Int32;
            meta.Title = String;

            if (meta._Title.Value != meta.Title.Value.Length + 1)
            {
                Debugger.Break();
                Process.GetCurrentProcess().Kill();
            }

            meta._Subtitle = Int32;
            meta.Subtitle = String;

            meta._Artist = Int32;
            meta.Artist = String;

            meta._Album = Int32;
            meta.Album = String;

            meta._LyricsBy = Int32;
            meta.LyricsBy = String;

            meta._Music = Int32;
            meta.Music = String;

            meta._Copy = Int32;
            meta.Copy = String;

            meta._TabAuthor = Int32;
            meta.TabAuthor = String;

            meta._Instructions = Int32;
            meta.Instructions = String;

            meta.NoticeLineCount = Int32;

            for (var i = 0; i < meta.NoticeLineCount; i++)
            {
                meta.Notice.Add(new TabMetaNoticeLine
                {
                    Unknown = Int32,
                    Content = String
                });
            }

            return meta;
        }

        private Lyrics ReadStructLyrics(Lyrics lyrics)
        {
            lyrics.Start = Int32;
            lyrics.Content = LongString;
            return lyrics;
        }

        private Template ReadStructTemplate(Template template)
        {
            template.Title = String;
            template.Subtitle = (this << 4, String).Item2;
            template.Artist = (this << 4, String).Item2;
            template.Album = (this << 4, String).Item2;
            template.WordsBy = (this << 4, String).Item2;
            template.MusicBy = (this << 4, String).Item2;
            template.WordsAndMusicBy = (this << 4, String).Item2;
            template.Copyright = (this << 4, String).Item2;
            template.Rights = (this << 4, String).Item2;
            template.Page = (this << 4, String).Item2;
            template.Moderate = (this << 4, String).Item2;
            return template;
        }

        private TrackMeta ReadStructTrackMeta(TrackMeta trackMeta)
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
            return trackMeta;
        }

        private Track ReadStructTrack(Track track)
        {
            track.Flags = Byte;
            track.Meta = TrackMetaArray[_trackMetaIterator];
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
            var o0 = String;
            var o1 = Int32;
            var o2 = String;
            return track;
        }

        private Chord ReadStructChord(Chord chord)
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

            return chord;
        }

        private Bookmark ReadStructBookmark(Bookmark bookmark)
        {
            bookmark.Title = String;
            bookmark.Color = ReadColor();
            return bookmark;
        }
    }
}