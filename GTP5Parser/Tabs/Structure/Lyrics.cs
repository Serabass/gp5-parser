using GTP5Parser.Binary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTP5Parser.Tabs.Structure
{
    public class Lyrics : IDisposable
    {
        public Int32MemoryBlock Start;
        public StringMemoryBlock Content;

        public void Dispose()
        {
            Start = null;
            Content = null;
        }
    }
}
