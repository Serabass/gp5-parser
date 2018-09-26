using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GTP5Parser.Binary
{
    public partial class MyBinaryReader : BinaryReader
    {
        private readonly Encoding _utf8 = Encoding.GetEncoding("UTF-8");
        private readonly Encoding _win1251 = Encoding.GetEncoding("Windows-1251");

        protected MyBinaryReader(Stream input) : base(input)
        {
            
        }

        private new BooleanMemoryBlock ReadBoolean()
        {
            var offset = BaseStream.Position;
            var result = base.ReadBoolean();
            return new BooleanMemoryBlock()
            {
                Offset = offset,
                Value = result
            };
        }

        private new ByteArrayMemoryBlock ReadBytes(int count)
        {
            var offset = BaseStream.Position;
            var result = base.ReadBytes(count);
            return new ByteArrayMemoryBlock()
            {
                Offset = offset,
                Value = result
            };
        }

        protected StructMemoryBlock<T> ReadStruct<T>(Action<T> action)
            where T : IDisposable, new()
        {
            var offset = BaseStream.Position;
            var structObject = new T();
            action(structObject);

            return new StructMemoryBlock<T>
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
                result.Add(Byte);
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
