using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GTP5Parser.Binary
{
    public partial class MyBinaryReader : BinaryReader
    {
        readonly Encoding utf8 = Encoding.GetEncoding("UTF-8");
        readonly Encoding win1251 = Encoding.GetEncoding("Windows-1251");

        public MyBinaryReader(Stream input) : base(input)
        {
        }

        public new BooleanMemoryBlock ReadBoolean()
        {
            var offset = BaseStream.Position;
            var result = base.ReadBoolean();
            return new BooleanMemoryBlock()
            {
                Offset = offset,
                Value = result
            };
        }

        public new ByteArrayMemoryBlock ReadBytes(int count)
        {
            var offset = BaseStream.Position;
            var result = base.ReadBytes(count);
            return new ByteArrayMemoryBlock()
            {
                Offset = offset,
                Value = result
            };
        }

        public MemoryBlock<TStruct> ReadStruct<TStruct>(Action<TStruct> action) where TStruct : IDisposable, new()
        {
            var offset = BaseStream.Position;
            var structObject = new TStruct();
            action(structObject);

            return new MemoryBlock<TStruct>
            {
                Value = structObject,
                Offset = offset,
                Size = BaseStream.Position - offset
            };
        }

        public ByteArrayMemoryBlock ReadToEnd()
        {
            long offset = BaseStream.Position;
            var result = new List<byte>();

            while (BaseStream.Length > BaseStream.Position)
            {
                result.Add(ReadByte());
            }

            return new ByteArrayMemoryBlock
            {
                Offset = offset,
                Value = result.ToArray(),
                Size = result.Count
            };
        }
    }
}
