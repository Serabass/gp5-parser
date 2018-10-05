using System;
using System.Collections.Generic;

namespace GTP5Parser.Tabs.Structure
{
    public class LyricsList : IDisposable
    {
        public List<Lyrics> List = new List<Lyrics>();

        public void Dispose()
        {
            List.Clear();
        }
    }
}