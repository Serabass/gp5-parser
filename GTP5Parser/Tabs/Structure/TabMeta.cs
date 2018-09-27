using GTP5Parser.Binary;
using System;

namespace GTP5Parser.Tabs.Structure
{
    public class TabMeta : IDisposable
    {
        public Int32MemoryBlock _Title;
        public StringMemoryBlock Title;
        public Int32MemoryBlock _Subtitle;
        public StringMemoryBlock Subtitle;
        public Int32MemoryBlock _Artist;
        public StringMemoryBlock Artist;
        public Int32MemoryBlock _Album;
        public StringMemoryBlock Album;
        public Int32MemoryBlock _LyricsBy;
        public StringMemoryBlock LyricsBy;
        public Int32MemoryBlock _Music;
        public StringMemoryBlock Music;
        public Int32MemoryBlock _Copy;
        public StringMemoryBlock Copy;
        public Int32MemoryBlock _TabAuthor;
        public StringMemoryBlock TabAuthor;
        public Int32MemoryBlock _Instructions;
        public StringMemoryBlock Instructions;
        public Int32MemoryBlock _Notice;
        public StringMemoryBlock Notice;

        public void Dispose()
        {
            Title = null;
            Subtitle = null;
            Artist = null;
            Album = null;
            LyricsBy = null;
            Music = null;
            Copy = null;
            TabAuthor = null;
            Instructions = null;
            Notice = null;
        }
    }
}
