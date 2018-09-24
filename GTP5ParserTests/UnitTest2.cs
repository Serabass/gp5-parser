using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GTP5Parser;
using GTP5Parser.Tabs;

namespace GTP5ParserTests
{
    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public void TestMethod1()
        {
            Assert.AreEqual("E5", Note.From(0x40).ToString()); // E5
            Assert.AreEqual("B4", Note.From(0x3B).ToString()); // B4
            Assert.AreEqual("G4", Note.From(0x37).ToString()); // G4
            Assert.AreEqual("D4", Note.From(0x32).ToString()); // D4
            Assert.AreEqual("A3", Note.From(0x2D).ToString()); // A3
            Assert.AreEqual("E3", Note.From(0x28).ToString()); // E3
        }
    }
}
