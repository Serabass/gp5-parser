using GTP5Parser.Binary;
using System;

namespace GTP5Parser.Tabs.Structure
{
    public class Bookmark : IDisposable
    {
        public StringMemoryBlock Title;
        public Color Color;

        public void Dispose()
        {
            Title = null;
            Color = null;
        }
    }
}
