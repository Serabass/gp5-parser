using GTP5Parser.Binary;
using System;
using System.Collections.Generic;
using GTP5Parser.Tabs.Structure.Meta;

namespace GTP5Parser.Tabs.Structure
{
    public class TabMeta : IDisposable
    {
        public StringMemoryBlock Title;
        public StringMemoryBlock Subtitle;
        public StringMemoryBlock Artist;
        public StringMemoryBlock Album;
        public StringMemoryBlock LyricsBy;
        public StringMemoryBlock Music;
        public StringMemoryBlock Copy;
        public StringMemoryBlock TabAuthor;
        public StringMemoryBlock Instructions;
        public Int32MemoryBlock NoticeLineCount;
        public List<TabMetaNoticeLine> Notice = new List<TabMetaNoticeLine>();

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
