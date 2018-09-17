using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTP5Parser
{
    class MyBinaryReader : BinaryReader
    {
        Encoding utf8 = Encoding.GetEncoding("UTF-8");
        Encoding win1251 = Encoding.GetEncoding("Windows-1251");

        public MemoryBlock<byte[]> lastSkipped;

        public MyBinaryReader(Stream input) : base(input)
        {
        }

        public new MemoryBlock<string> ReadString()
        {
            var offset = BaseStream.Position;
            var strLength = ReadByte();
            var bytes = ReadBytes(strLength.Value);
            byte[] win1251Bytes = Encoding.Convert(utf8, win1251, bytes.Value.ToArray());
            var result = win1251.GetString(win1251Bytes);
            return new MemoryBlock<string>()
            {
                Value = result,
                Offset = offset,
                Size = strLength.Value + 1
            };
        }

        public MemoryBlock<string> ReadString(int maxBytes)
        {
            var result = ReadString();
            ReadBytes(maxBytes - result.Value.Length);
            return result;
        }

        public new MemoryBlock<int> ReadInt32()
        {
            var offset = BaseStream.Position;
            var result = base.ReadInt32();
            return new MemoryBlock<int>()
            {
                Offset = offset,
                Value = result
            };
        }

        public new MemoryBlock<short> ReadInt16()
        {
            var offset = BaseStream.Position;
            var result = base.ReadInt16();
            return new MemoryBlock<short>()
            {
                Offset = offset,
                Value = result
            };
        }

        public new MemoryBlock<bool> ReadBoolean()
        {
            var offset = BaseStream.Position;
            var result = base.ReadBoolean();
            return new MemoryBlock<bool>()
            {
                Offset = offset,
                Value = result
            };
        }

        public new MemoryBlock<byte[]> ReadBytes(int count)
        {
            var offset = BaseStream.Position;
            var result = base.ReadBytes(count);
            return new MemoryBlock<byte[]>()
            {
                Offset = offset,
                Value = result
            };
        }

        public new MemoryBlock<byte> ReadByte()
        {
            var offset = BaseStream.Position;
            var result = base.ReadByte();
            return new MemoryBlock<byte>()
            {
                Offset = offset,
                Value = result
            };
        }

        public new MemoryBlock<sbyte> ReadSByte()
        {
            var offset = BaseStream.Position;
            var result = base.ReadSByte();
            return new MemoryBlock<sbyte>()
            {
                Offset = offset,
                Value = result
            };
        }

        public MemoryBlock<T> ReadEnum<T>()
        {
            var offset = BaseStream.Position;
            var result = (T)Enum.ToObject(typeof(T), base.ReadInt32());
            return new MemoryBlock<T>()
            {
                Offset = offset,
                Value = result
            };
        }

        public MemoryBlock<string> ReadLongString()
        {
            var offset = BaseStream.Position;
            int stringLength = base.ReadInt32();
            var bytes = ReadBytes(stringLength);
            byte[] win1251Bytes = Encoding.Convert(utf8, win1251, bytes.Value.ToArray());
            var result = win1251.GetString(win1251Bytes);
            return new MemoryBlock<string>()
            {
                Value = result,
                Offset = offset
            };
        }

        public void Skip(int count)
        {
            lastSkipped = ReadBytes(count);
        }

        [Obsolete]
        public void Jump(long offset)
        {
            BaseStream.Seek(offset, SeekOrigin.Begin);
        }
    }
}
