namespace GTP5Parser.Binary
{
    public partial class MyBinaryReader
    {
        protected BooleanMemoryBlock Boolean => ReadBoolean();
        protected ShortMemoryBlock Short => ReadInt16();
        protected Int32MemoryBlock Int32 => ReadInt32();
        protected StringMemoryBlock String => ReadString();
        protected StringMemoryBlock IntByteString => ReadIntByteString();
        protected StringMemoryBlock LongString => ReadLongString();
        protected ByteMemoryBlock Byte => ReadByte();
        protected SByteMemoryBlock SByte => ReadSByte();
        protected CharMemoryBlock Char => ReadChar();
        protected FloatMemoryBlock Single => ReadSingle();
        protected DoubleMemoryBlock Double => ReadDouble();
    }
}