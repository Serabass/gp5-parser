
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace GTP5Parser
{
    public struct BarSize
    {
        public byte Up;
        public byte Down;
    }

    public class Track
    {
        public const int TRACK_BLOCK_LENGTH = 0xBD;
        
        public MemoryBlock<byte> Flags;
        public MemoryBlock<byte> Flags2;
        public MemoryBlock<string> Title;

        public Color Color;

        public MemoryBlock<int> StringsCount;
        public MemoryBlock<Note>[] Tuning;
        public MemoryBlock<int> FretCount;
        public MemoryBlock<int> Capo;
        public MemoryBlock<int> Port;
        public MemoryBlock<int> MainChannel;
        public MemoryBlock<int> EffectChannel;
        public MemoryBlock<byte> MidiBank;

        public TrackMeta Meta;



        public static Track FromReader(BinaryReader reader)
        {
            return new Track();
        }
    }
}
