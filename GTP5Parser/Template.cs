using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTP5Parser
{
    public class Template : IDisposable
    {
        public MemoryBlock<string> Title;
        public MemoryBlock<string> Subtitle;
        public MemoryBlock<string> Artist;
        public MemoryBlock<string> Album;
        public MemoryBlock<string> WordsBy;
        public MemoryBlock<string> MusicBy;
        public MemoryBlock<string> WordsAndMusicBy;
        public MemoryBlock<string> Copyright;
        public MemoryBlock<string> Rights;
        public MemoryBlock<string> Page;
        public MemoryBlock<string> Moderate;

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
