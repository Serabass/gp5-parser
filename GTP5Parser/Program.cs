using GTP5Parser.Tabs;
using GTP5Parser.Tabs.Structure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace GTP5Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            var x = Directory.EnumerateFiles(".\\gtptabs.com", "*.gp5", SearchOption.AllDirectories);

            foreach (string file in x)
            {
                try
                {
                    Tab tab = Tab.FromFile("re.gp5");
                    Debugger.Break();
                }
                catch (VersionNotSupportedException e)
                {
                    continue;
                }
            }

            return;
        }
    }

}
