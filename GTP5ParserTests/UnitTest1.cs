using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.IO;
using GTP5Parser.Tabs;
using GTP5Parser.Tabs.Structure;

namespace GTP5ParserTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
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

            // Assert.Equals(tab.BarCount, 10);
            // Assert.Equals(tab.Up, 6);
            // Assert.Equals(tab.Down, 4);
            // Assert.Equals(tab.TracksCount, 3);
            // Assert.Equals(tab.Tracks.Count, 3);
            // Assert.Equals(tab.Bookmarks.Count, 2);
            // Assert.Equals(tab.Bookmarks[0].Title, "BM1");
            // Assert.Equals(tab.Bookmarks[1].Title, "BM2");
            // Assert.Equals(tab.Bookmarks[0].Color.Red, 255);
            // Assert.Equals(tab.Bookmarks[0].Color.Green, 0);
            // Assert.Equals(tab.Bookmarks[0].Color.Blue, 0);
            // Assert.Equals(tab.Bookmarks[1].Color.Red, 255);
            // Assert.Equals(tab.Bookmarks[1].Color.Green, 0);
            // Assert.Equals(tab.Bookmarks[1].Color.Blue, 0);
            // 
            // 
            // Assert.Equals(tab.Tracks[0].Title, "Track Number One");
            // Assert.Equals(tab.Tracks[1].Title, "Track Number Two");
        }
    }
}
