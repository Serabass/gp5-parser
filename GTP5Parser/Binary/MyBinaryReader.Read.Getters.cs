using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace GTP5Parser.Binary
{
    public partial class MyBinaryReader : BinaryReader
    {
        public BooleanMemoryBlock Boolean => ReadBoolean();
        public ShortMemoryBlock Short => ReadInt16();
        public Int32MemoryBlock Int32 => ReadInt32();
        public StringMemoryBlock String => ReadString();
        public StringMemoryBlock LongString => ReadLongString();
        public ByteMemoryBlock Byte => ReadByte();
        public SByteMemoryBlock SByte => ReadSByte();
        public CharMemoryBlock Char => ReadChar();
        public FloatMemoryBlock Single => ReadSingle();
        public DoubleMemoryBlock Double => ReadDouble();
    }
}
