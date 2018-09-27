
using GTP5Parser.Binary;
using System;
using System.IO;

namespace GTP5Parser.Tabs.Structure
{
    public struct BarSize
    {
        public byte Up;
        public byte Down;
    }

    public class Track : IDisposable
    {
        public const int TrackBlockLength = 0xBD;

        public ByteMemoryBlock Flags;
        public ByteMemoryBlock Flags2;
        public StringMemoryBlock Title;
        public Int32MemoryBlock StringsCount;
        public Note.MemoryBlock[] Tuning;
        public Int32MemoryBlock FretCount;
        public Int32MemoryBlock Capo;
        public Int32MemoryBlock Port;
        public Int32MemoryBlock MainChannel;
        public Int32MemoryBlock EffectChannel;
        public ByteMemoryBlock MidiBank;

        public Color Color;

        public TrackMeta Meta;

        public static Track FromReader(BinaryReader reader)
        {
            return new Track();
        }

        public void Dispose()
        {
            Meta.Dispose();

            Flags = null;
            Flags2 = null;
            Title = null;
            StringsCount = null;
            Tuning = null;
            FretCount = null;
            Capo = null;
            Port = null;
            MainChannel = null;
            EffectChannel = null;
            MidiBank = null;
            Color = null;
        }
    }
}
