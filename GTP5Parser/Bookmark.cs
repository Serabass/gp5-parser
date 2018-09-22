using System;

namespace GTP5Parser
{
    public class Bookmark : IDisposable
    {
        public MemoryBlock<string> Title;
        public Color Color;

        public void Dispose()
        {
            Title = null;
            Color = null;
        }
    }
}
