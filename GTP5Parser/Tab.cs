using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GTP5Parser
{
    public class Tab
    {
        public Version Version = new Version();

        public MemoryBlock<string> Title;
        public MemoryBlock<string> Subtitle;
        public MemoryBlock<string> Artist;
        public MemoryBlock<string> Album;
        public MemoryBlock<string> LyricsBy;
        public MemoryBlock<string> Music;
        public MemoryBlock<string> Copy;
        public MemoryBlock<string> Nabor;
        public MemoryBlock<string> Instr;
        public MemoryBlock<string> Notes;

        public MemoryBlock<int> LyricsTrack;
        public List<Lyrics> LyricsArray = new List<Lyrics>();

        public Template Template;

        public List<Track> Tracks = new List<Track>();

        public MemoryBlock<short> Moderate;
        public MemoryBlock<bool> HideTempo;

        public MemoryBlock<int> BarCount;
        public MemoryBlock<int> TracksCount;
        public MemoryBlock<byte> Up;
        public MemoryBlock<byte> Down;
        public MemoryBlock<byte[]> KeySigns;
        public MemoryBlock<byte[]> Link8Notes;

        public List<MidiInstruments> Instruments = new List<MidiInstruments>();
        public List<Bookmark> Bookmarks = new List<Bookmark>();
        

        public static Tab FromStream(Stream stream)
        {
            var tab = new Tab();
            using (var reader = new MyBinaryReader(stream))
            {
                var VersionString = reader.ReadString();
                var match = Regex.Match(VersionString.Value, @"v(?<major>\d+)\.(?<minor>\d+)$", RegexOptions.Singleline);
                tab.Version.Major = int.Parse(match.Groups["major"].Value);
                tab.Version.Minor = int.Parse(match.Groups["minor"].Value);
                var unk = reader.ReadBytes(10);
                tab.Title = reader.ReadString();
                var unk2 = reader.ReadInt32();
                tab.Subtitle = reader.ReadString();
                var unk3 = reader.ReadInt32();
                tab.Artist = reader.ReadString();
                var unk4 = reader.ReadInt32();
                tab.Album = reader.ReadString();
                var unk5 = reader.ReadInt32();
                tab.LyricsBy = reader.ReadString();
                var unk6 = reader.ReadInt32();
                tab.Music = reader.ReadString();
                var unk7 = reader.ReadInt32();
                tab.Copy = reader.ReadString();
                var unk8 = reader.ReadInt32();
                tab.Nabor = reader.ReadString();
                var unk9 = reader.ReadInt32();
                tab.Instr = reader.ReadString();
                var unk10 = reader.ReadInt32();
                var unk10_1 = reader.ReadInt32();
                tab.Notes = reader.ReadString();
                tab.LyricsTrack = reader.ReadInt32();

                for (int i = 0; i < 5; i++)
                {
                    var lyrics = new Lyrics()
                    {
                        Start = reader.ReadInt32(),
                        Content = reader.ReadLongString()
                    };

                    tab.LyricsArray.Add(lyrics);
                }

                MemoryBlock<byte[]> a_0 = reader.ReadBytes(21);
                MemoryBlock<byte[]> a_1 = reader.ReadBytes(16);
                MemoryBlock<byte[]> a_2 = reader.ReadBytes(16);

                tab.Template = new Template();

                tab.Template.Title = reader.ReadString();
                reader.Skip(4);
                tab.Template.Subtitle = reader.ReadString();
                reader.Skip(4);
                tab.Template.Artist = reader.ReadString();
                reader.Skip(4);
                tab.Template.Album = reader.ReadString();
                reader.Skip(4);
                tab.Template.WordsBy = reader.ReadString();
                reader.Skip(4);
                tab.Template.MusicBy = reader.ReadString();
                reader.Skip(4);
                tab.Template.WordsAndMusicBy = reader.ReadString();
                reader.Skip(4);
                tab.Template.Copyright = reader.ReadString();
                reader.Skip(4);
                tab.Template.Rights = reader.ReadString();
                reader.Skip(4);
                tab.Template.Page = reader.ReadString();
                reader.Skip(4);
                tab.Template.Moderate = reader.ReadString();
                tab.Moderate = reader.ReadInt16();
                reader.Skip(2);
                tab.HideTempo = reader.ReadBoolean();
                reader.Skip(5);

                List<TrackMeta> TrackMeta = new List<TrackMeta>();

                for (var i = 0; i < 55; i++)
                {
                    var offset = reader.BaseStream.Position;

                    var trackMeta = new TrackMeta()
                    {
                        Offset = offset,
                        Instrument = reader.ReadEnum<MidiInstruments>(),
                        Volume = reader.ReadByte(),
                        Pan = reader.ReadByte(),
                        Chorus = reader.ReadByte(),
                        Reverb = reader.ReadByte(),
                        Phaser = reader.ReadByte(),
                        Tremolo = reader.ReadByte(),
                    };
                    reader.Skip(2);
                    TrackMeta.Add(trackMeta);
                }

                reader.Skip(54);
                tab.BarCount = reader.ReadInt32();
                tab.TracksCount = reader.ReadInt32();
                var unk12 = reader.ReadByte();
                tab.Up = reader.ReadByte();
                tab.Down = reader.ReadByte();
                tab.KeySigns = reader.ReadBytes(2);
                tab.Link8Notes = reader.ReadBytes(4);

                // reader.Skip(16);

                while (true)
                {
                    reader.SkipWhile(() => reader.ReadByte().Value == 0x00);
                    if (reader.ReadByte().Value == 0x08)
                    {
                        reader.Back(1);
                        break;
                    }
                    reader.Back(1);
                    var i80 = reader.ReadInt32();
                    reader.Skip(5);
                    var bookmark = new Bookmark();
                    bookmark.Title = reader.ReadString();
                    bookmark.Color = reader.ReadColor();
                    tab.Bookmarks.Add(bookmark);
                }
                
                for (var i = 0; i < tab.TracksCount.Value; i++)
                {
                    var track = new Track();
                    track.Flags = reader.ReadByte();
                    track.Meta = TrackMeta[i];
                    track.Title = reader.ReadString(0x28);
                    track.StringsCount = reader.ReadInt32();
                    track.Tuning = new MemoryBlock<Note>[track.StringsCount.Value];

                    for (var o = 0; o < track.StringsCount.Value; o++)
                    {
                        var str = reader.ReadInt32();
                        track.Tuning[o] = new MemoryBlock<Note>(new Note(str.Value));
                    }
                    
                    var remainingStringTuningData = reader.ReadBytes((7 - track.StringsCount.Value) * 4);
                    track.Port = reader.ReadInt32();
                    track.MainChannel = reader.ReadInt32();
                    track.EffectChannel = reader.ReadInt32();
                    track.FretCount = reader.ReadInt32();
                    track.Capo = reader.ReadInt32();
                    track.Color = reader.ReadColor();
                    reader.Skip(0x36);
                    var o0 = reader.ReadString();
                    var o1 = reader.ReadInt32();
                    var o2 = reader.ReadString();
                    tab.AddTrack(track);
                }

                reader.Skip(6);

                var length = reader.ReadSByte();

                var stringsBits = reader.ReadByte();

                List<bool> bits = new List<bool>();

                for (var i = 0; i < 8; i++) // 00100100
                {
                    var bit = (stringsBits.Value & (1 << i - 1)) != 0;
                    bits.Add(bit);
                }

                var notesCount = bits.Count(b => b == true);

                var frets = new List<byte>();

                for (var i = 0; i < notesCount; i++)
                {
                    var flags = reader.ReadByte();
                    var unk__2 = reader.ReadByte();
                    var flags2 = reader.ReadByte();
                    var fret = reader.ReadByte();
                    var unk__4 = reader.ReadByte();
                    frets.Add(fret.Value);
                }
                
                var a = reader.ReadToEnd();

                reader.Close();
                reader.Dispose();
                Debugger.Break();
            }
            return tab;
        }

        public static Tab FromFile(string path = "test2.gp5")
        {
            var stream = File.OpenRead(path);
            var tab = FromStream(stream);
            stream.Close();
            stream.Dispose();
            return tab;
        }

        public Tab AddTrack(Track track)
        {
            Tracks.Add(track);
            return this;
        }
    }
}
