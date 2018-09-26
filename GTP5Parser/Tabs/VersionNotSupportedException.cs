using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTP5Parser.Tabs
{
    public class VersionNotSupportedException : Exception
    {
        public string Version;

        public VersionNotSupportedException(string Version)
            : base($"Version {Version} is not supported yet")
        {
            this.Version = Version;
        }
    }
}