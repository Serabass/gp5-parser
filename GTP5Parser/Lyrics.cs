using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTP5Parser
{
    public class Lyrics : IDisposable
    {
        public MemoryBlock<int> Start;
        public MemoryBlock<string> Content;

        public void Dispose()
        {
            Start = null;
            Content = null;
        }
    }
}
