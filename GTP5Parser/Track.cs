
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace GTP5Parser
{
    public struct TrackColor
    {
        public byte Red;
        public byte Green;
        public byte Blue;
    }

    public struct BarSize
    {
        public byte Up;
        public byte Down;
    }

    class Track
    {
        public const int TRACK_BLOCK_LENGTH = 0xBD;

        public string Title;

        public TrackColor Color;

        public int StringsCount;
        public int[] Tuning;
        public int BarsCount;
        public int Capo;

        public static Track FromReader(BinaryReader reader)
        {
            return new Track();
        }
    }
}
