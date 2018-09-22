using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTP5Parser
{
    public struct TrackMeta : IDisposable
    {
        public long Offset;
        public MemoryBlock<MidiInstruments> Instrument;
        public MemoryBlock<byte> Volume;
        public MemoryBlock<byte> Pan;
        public MemoryBlock<byte> Chorus;
        public MemoryBlock<byte> Reverb;
        public MemoryBlock<byte> Phaser;
        public MemoryBlock<byte> Tremolo;

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
