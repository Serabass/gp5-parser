using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace GTP5Parser
{
    public class Tab : IDisposable
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

        public List<Chord> Chords = new List<Chord>();

        public static Tab FromStream(Stream stream)
        {
            return new TabReader(stream).ReadTab();
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

        public void Dispose()
        {
            LyricsArray.ForEach(lyrics => lyrics.Dispose());
            Template.Dispose();
            Chords.ForEach(chord => chord.Dispose());
            Bookmarks.ForEach(bookmark => bookmark.Dispose());
        }
    }
}