using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTP5Parser
{
    class Tab
    {
        public string Version;
        public string Title;
        public string Subtitle;
        public string Artist;
        public string Album;
        public string LyricsBy;
        public string Music;
        public string Copy;
        public string Nabor;
        public string Instr;
        public string Notes;
        public int LyricsTrack;
        public List<Lyrics> LyricsArray = new List<Lyrics>();

        public Template Template;

        public List<Track> Tracks = new List<Track>();

        public int Moderate;
    
        public static Tab FromFile(string path = "test2.gp5")
        {
            var tab = new Tab();
            var stream = File.OpenRead(path);
            var reader = new MyBinaryReader(stream);

            tab.Version = reader.ReadString(30);
            int unk = reader.ReadInt32();
            tab.Title = reader.ReadString();
            int unk2 = reader.ReadInt32();
            tab.Subtitle = reader.ReadString();
            byte[] unk3 = reader.ReadBytes(4);
            tab.Artist = reader.ReadString();
            byte[] unk4 = reader.ReadBytes(4);
            tab.Album = reader.ReadString();
            byte[] unk5 = reader.ReadBytes(4);
            tab.LyricsBy = reader.ReadString();
            byte[] unk6 = reader.ReadBytes(4);
            tab.Music = reader.ReadString();
            byte[] unk7 = reader.ReadBytes(4);
            tab.Copy = reader.ReadString();
            byte[] unk8 = reader.ReadBytes(4);
            tab.Nabor = reader.ReadString();
            byte[] unk9 = reader.ReadBytes(4);
            tab.Instr = reader.ReadString();
            byte[] unk10 = reader.ReadBytes(8);
            tab.Notes = reader.ReadString();
            tab.LyricsTrack = reader.ReadInt32();

            for (int i = 0; i< 5; i++)
            {
                var lyrics = new Lyrics()
                {
                    Start = reader.ReadInt32(),
                    Content = reader.ReadLongString()
                };

                tab.LyricsArray.Add(lyrics);
            }

            reader.BaseStream.Seek(0x21E, SeekOrigin.Begin);

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
            tab.Moderate = reader.ReadInt32();
            var s = reader.ReadString();





            reader.BaseStream.Seek(0x600, SeekOrigin.Begin);
            var barCount = reader.ReadInt32();
            var tracksCount = reader.ReadInt32();
            byte unk12 = reader.ReadByte();
            byte up = reader.ReadByte();
            byte down = reader.ReadByte();
            byte[] unk13 = reader.ReadBytes(2);
            byte[] link8notes = reader.ReadBytes(4);
            for (var i = 0; i < tracksCount; i++)
            {
                var track = new Track();
                track.Title = reader.ReadString();
                byte[] unk15 = reader.ReadBytes(0x28 - track.Title.Length);
                track.StringsCount = reader.ReadInt32();

                track.Tuning = new int[track.StringsCount];
                for (var i2 = 0; i2 < track.StringsCount; i2++)
                {
                    var str = reader.ReadInt32();
                    track.Tuning[i2] = str;
                }

                byte[] unk16 = reader.ReadBytes((7 - track.StringsCount) * 4);

                Debugger.Break();

                reader.BaseStream.Seek(0x55, SeekOrigin.Begin);
                track.BarsCount = reader.ReadInt32();
                track.Capo = reader.ReadInt32();
                reader.BaseStream.Seek(0x5D, SeekOrigin.Begin);
                var color = reader.ReadBytes(3);
                track.Color = new TrackColor()
                {
                    Red = color[0],
                    Green = color[1],
                    Blue = color[2],
                };
                tab.AddTrack(track);
            }

            reader.Close();
            stream.Close();
            stream.Dispose();
            reader.Dispose();
            Debugger.Break();
            return tab;
        }

        public Tab AddTrack(Track track)
        {
            Tracks.Add(track);
            return this;
        }
    }
}
