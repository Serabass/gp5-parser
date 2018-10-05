using System;
using GTP5Parser.Binary;

namespace GTP5Parser.Tabs.Structure
{
    public class RSEMasterEffect : IDisposable
    {
        public int Volume;
        public int Reverb;
        public StructMemoryBlock<RSEEqualizer> Equalizer;
        
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}