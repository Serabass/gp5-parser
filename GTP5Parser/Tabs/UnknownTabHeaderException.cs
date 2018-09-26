using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTP5Parser.Tabs
{
    public class UnknownTabHeaderException : Exception
    {
        public UnknownTabHeaderException() : base($"Unknown tab header")
        {
        }
    }
}
