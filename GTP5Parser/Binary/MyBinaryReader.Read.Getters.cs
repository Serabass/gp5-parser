
namespace GTP5Parser.Binary
{
    public partial class MyBinaryReader
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
