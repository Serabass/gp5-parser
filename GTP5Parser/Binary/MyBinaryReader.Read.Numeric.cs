using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace GTP5Parser.Binary
{
    public partial class MyBinaryReader : BinaryReader
    {
        public new FloatMemoryBlock ReadSingle()
        {
            var offset = BaseStream.Position;
            var result = base.ReadSingle();
            return new FloatMemoryBlock()
            {
                Offset = offset,
                Value = result
            };
        }

        public new DoubleMemoryBlock ReadDouble()
        {
            var offset = BaseStream.Position;
            var result = base.ReadDouble();
            return new DoubleMemoryBlock()
            {
                Offset = offset,
                Value = result
            };
        }

        public new Int32MemoryBlock ReadInt32()
        {
            var offset = BaseStream.Position;
            var result = base.ReadInt32();
            return new Int32MemoryBlock()
            {
                Offset = offset,
                Value = result
            };
        }

        public new ShortMemoryBlock ReadInt16()
        {
            var offset = BaseStream.Position;
            var result = base.ReadInt16();
            return new ShortMemoryBlock()
            {
                Offset = offset,
                Value = result
            };
        }

        public new SByteMemoryBlock ReadSByte()
        {
            var offset = BaseStream.Position;
            var result = base.ReadSByte();
            return new SByteMemoryBlock()
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

        public new ByteMemoryBlock ReadByte()
        {
            var offset = BaseStream.Position;
            var result = base.ReadByte();
            return new ByteMemoryBlock()
            {
                Offset = offset,
                Value = result
            };
        }

    }
}
