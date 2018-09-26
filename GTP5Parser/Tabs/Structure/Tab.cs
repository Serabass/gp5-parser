using System;
using System.Collections.Generic;
using System.IO;
using GTP5Parser.Binary;

namespace GTP5Parser.Tabs.Structure
{
    public class Tab : IDisposable
    {
        public Version Version = new Version();
        public TabMeta Meta;

        public ShortMemoryBlock Moderate;
        public BooleanMemoryBlock HideTempo;
        public Int32MemoryBlock BarCount;
        public Int32MemoryBlock TracksCount;
        public ByteMemoryBlock Up;
        public ByteMemoryBlock Down;
        public ByteArrayMemoryBlock KeySigns;
        public ByteArrayMemoryBlock Link8Notes;
        public Int32MemoryBlock LyricsTrack;

        public List<Lyrics> LyricsArray = new List<Lyrics>();

        public Template Template;

        public List<Track> Tracks = new List<Track>();

        public List<MidiInstruments> Instruments = new List<MidiInstruments>();
        public List<Bookmark> Bookmarks = new List<Bookmark>();

        public List<Chord> Chords = new List<Chord>();

        public static Tab FromStream(Stream stream, string path)
        {
            return new TabReader(stream, path).ReadTab();
        }

        public static Tab FromFile(string path = "test2.gp5")
        {
            var stream = File.OpenRead(path);
            var tab = FromStream(stream, path);
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