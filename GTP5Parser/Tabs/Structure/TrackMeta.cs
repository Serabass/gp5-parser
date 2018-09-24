using GTP5Parser.Binary;
using System;

namespace GTP5Parser.Tabs.Structure
{
    public struct TrackMeta : IDisposable
    {
        public long Offset;
        public MemoryBlock<MidiInstruments> Instrument;
        public ByteMemoryBlock Volume;
        public ByteMemoryBlock Pan;
        public ByteMemoryBlock Chorus;
        public ByteMemoryBlock Reverb;
        public ByteMemoryBlock Phaser;
        public ByteMemoryBlock Tremolo;

        public void Dispose()
        {
            Instrument = null;
            Volume = null;
            Pan = null;
            Chorus = null;
            Reverb = null;
            Phaser = null;
            Tremolo = null;
        }
    }
}
