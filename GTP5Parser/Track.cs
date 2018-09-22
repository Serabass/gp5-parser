
using System;
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

    public class Track : IDisposable
    {
        public const int TRACK_BLOCK_LENGTH = 0xBD;

        public MemoryBlock<byte> Flags;
        public MemoryBlock<byte> Flags2;
        public MemoryBlock<string> Title;
        public MemoryBlock<int> StringsCount;
        public MemoryBlock<Note>[] Tuning;
        public MemoryBlock<int> FretCount;
        public MemoryBlock<int> Capo;
        public MemoryBlock<int> Port;
        public MemoryBlock<int> MainChannel;
        public MemoryBlock<int> EffectChannel;
        public MemoryBlock<byte> MidiBank;

        public Color Color;

        public TrackMeta Meta;

        public static Track FromReader(System.IO.BinaryReader reader)
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
