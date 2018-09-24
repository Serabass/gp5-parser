using System;
using System.Collections.Generic;

namespace GTP5Parser.Tabs.Structure
{
    public enum ChordLength : sbyte
    {
        Full = -2,
        Half = -1,
        Quarter = 0,
        Eight = 1,
        OneSix = 2,
        ThreeTwo = 3,
        SixFour = 4,
    }

    public class Chord : IDisposable
    {
        public ChordLength length;
        public byte Flags;

        public Dictionary<int, byte> notes = new Dictionary<int, byte>();

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
