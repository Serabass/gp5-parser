using System;
using System.IO;

namespace GTP5Parser.Binary
{
    public partial class MyBinaryReader : BinaryReader
    {
        public static ByteArrayMemoryBlock operator <<(MyBinaryReader reader, int count)
        {
            return reader.ReadBytes(count);
        }

        public static bool operator +(MyBinaryReader reader, Func<bool> callback)
        {
            reader.SkipWhile(callback);
            return true;
        }

        public static StringMemoryBlock operator ~(MyBinaryReader reader)
        {
            return reader.ReadString();
        }

        public static StringMemoryBlock operator %(MyBinaryReader reader, int count)
        {
            return reader.ReadString(count);
        }

        public static bool operator >>(MyBinaryReader reader, int count)
        {
            reader.Skip(count);
            return true;
        }
    }
}
