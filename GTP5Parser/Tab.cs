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
    class Tab
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

        public List<MidiInstruments> Instruments = new List<MidiInstruments>();

        

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

                for (var i = 0; i < 64; i++)
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

                reader.Skip(42);
                var barCount = reader.ReadInt32();
                var tracksCount = reader.ReadInt32();
                var unk12 = reader.ReadByte();
                var up = reader.ReadByte();
                var down = reader.ReadByte();
                var keySigns = reader.ReadBytes(2);
                var link8notes = reader.ReadBytes(2);
                
                for (var i = 0; i < 4; i++)
                {
                    var bookmark = new Bookmark();
                    bookmark.Title = reader.ReadString();
                    var col = reader.ReadBytes(3);
                    bookmark.Color = Color.FromBytes(col.Value);
                    var data = reader.ReadBytes(0x13);
                }
                
                for (var i = 0; i < tracksCount.Value; i++)
                {
                    var track = new Track();
                    track.Flags = reader.ReadByte();
                    track.Meta = TrackMeta[i];
                    track.Title = reader.ReadString(0x28);
                    track.StringsCount = reader.ReadInt32();
                    track.Tuning = new MemoryBlock<int>[track.StringsCount.Value];

                    for (var o = 0; o < track.StringsCount.Value; o++)
                    {
                        var str = reader.ReadInt32();
                        track.Tuning[o] = str;
                    }
                    
                    var remainingStringTuningData = reader.ReadBytes((7 - track.StringsCount.Value) * 4);
                    track.Port = reader.ReadInt32();
                    track.MainChannel = reader.ReadInt32();
                    track.EffectChannel = reader.ReadInt32();
                    track.BarsCount = reader.ReadInt32();
                    track.Capo = reader.ReadInt32();
                    var color = reader.ReadBytes(3);
                    track.Color = Color.FromBytes(color.Value);
                    reader.Skip(2);
                    track.Flags2 = reader.ReadByte();
                    reader.Skip(1);
                    track.MidiBank = reader.ReadByte();
                    var o1 = reader.ReadBytes(4);
                    var o2 = reader.ReadBytes(5);
                    var o3 = reader.ReadBytes(4);
                    var o4 = reader.ReadBytes(10);
                    var o5 = reader.ReadBytes(1);
                    var xxxx = reader.ReadByte();
                    var o5_1 = reader.ReadBytes(16);
                    var o6 = reader.ReadBytes(14);
                    tab.AddTrack(track);
                }

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
