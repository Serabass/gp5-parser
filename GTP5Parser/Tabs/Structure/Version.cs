using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTP5Parser.Tabs.Structure
{
    public struct Version : IDisposable
    {
        public int Major;
        public int Minor;

        public void Dispose()
        {
            
        }

        public new string ToString()
        {
            return $"v{Major}.{Minor}";
        }
    }
}
