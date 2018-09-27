using GTP5Parser.Tabs;
using GTP5Parser.Tabs.Structure;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace GTP5Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            var x = Directory.EnumerateFiles(".\\sorted\\v5.10", "*.gp5", SearchOption.AllDirectories);

            foreach (string file in x)
            {
                Tab tab;
                try
                {
                    tab = Tab.FromFile(file);
                }
                catch (VersionNotSupportedException e)
                {
                    Debugger.Break();
                }
                catch (UnknownTabHeaderException e)
                {
                    Debugger.Break();
                }
            }
        }
    }
}