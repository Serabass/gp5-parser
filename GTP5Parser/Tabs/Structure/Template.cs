using GTP5Parser.Binary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTP5Parser.Tabs.Structure
{
    public class Template : IDisposable
    {
        public StringMemoryBlock Title;
        public StringMemoryBlock Subtitle;
        public StringMemoryBlock Artist;
        public StringMemoryBlock Album;
        public StringMemoryBlock WordsBy;
        public StringMemoryBlock MusicBy;
        public StringMemoryBlock WordsAndMusicBy;
        public StringMemoryBlock Copyright;
        public StringMemoryBlock Rights;
        public StringMemoryBlock Page;
        public StringMemoryBlock Moderate;

        public void Dispose()
        {
            Title = null;
            Subtitle = null;
            Artist = null;
            Album = null;
            WordsBy = null;
            MusicBy = null;
            WordsAndMusicBy = null;
            Copyright = null;
            Rights = null;
            Page = null;
            Moderate = null;
        }
    }
}
