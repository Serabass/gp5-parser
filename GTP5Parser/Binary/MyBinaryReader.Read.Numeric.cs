﻿using System;
using System.Linq;

namespace GTP5Parser.Binary
{
    public partial class MyBinaryReader
    {
        private new FloatMemoryBlock ReadSingle()
        {
            var offset = BaseStream.Position;
            var result = base.ReadSingle();
            return new FloatMemoryBlock
            {
                Offset = offset,
                Value = result
            };
        }

        private new DoubleMemoryBlock ReadDouble()
        {
            var offset = BaseStream.Position;
            var result = base.ReadDouble();
            return new DoubleMemoryBlock()
            {
                Offset = offset,
                Value = result
            };
        }

        private new Int32MemoryBlock ReadInt32()
        {
            var offset = BaseStream.Position;
            var result = base.ReadInt32();
            return new Int32MemoryBlock()
            {
                Offset = offset,
                Value = result
            };
        }

        private new ShortMemoryBlock ReadInt16()
        {
            var offset = BaseStream.Position;
            var result = base.ReadInt16();
            return new ShortMemoryBlock()
            {
                Offset = offset,
                Value = result
            };
        }

        private new SByteMemoryBlock ReadSByte()
        {
            var offset = BaseStream.Position;
            var result = base.ReadSByte();
            return new SByteMemoryBlock()
            {
                Offset = offset,
                Value = result
            };
        }

        protected SBytesMemoryBlock ReadSBytes(int count)
        {
            var offset = BaseStream.Position;
            var result = base.ReadBytes(count).Select(b => (sbyte)b).ToArray();
            return new SBytesMemoryBlock
            {
                Offset = offset,
                Value = result
            };
        }

        public MemoryBlock<T> ReadEnum<T>()
        {
            var offset = BaseStream.Position;
            var result = (T)Enum.ToObject(typeof(T), Int32);
            return new MemoryBlock<T>()
            {
                Offset = offset,
                Value = result
            };
        }

        public MemoryBlock<T> ReadSByteEnum<T>()
        {
            var offset = BaseStream.Position;
            var result = (T)Enum.ToObject(typeof(T), SByte);
            return new MemoryBlock<T>()
            {
                Offset = offset,
                Value = result
            };
        }

        private new ByteMemoryBlock ReadByte()
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
