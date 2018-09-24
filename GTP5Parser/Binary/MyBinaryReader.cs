using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GTP5Parser.Binary
{
    public class MyBinaryReader : System.IO.BinaryReader
    {
        readonly Encoding utf8 = Encoding.GetEncoding("UTF-8");
        readonly Encoding win1251 = Encoding.GetEncoding("Windows-1251");

        public ByteArrayMemoryBlock lastSkipped;

        public MyBinaryReader(Stream input) : base(input)
        {
        }

        public new StringMemoryBlock ReadString()
        {
            var offset = BaseStream.Position;
            var strLength = ReadByte();
            var bytes = ReadBytes(strLength.Value);
            // byte[] win1251Bytes = Encoding.Convert(utf8, win1251, bytes.Value.ToArray());
            var result = win1251.GetString(bytes.Value);
            return new StringMemoryBlock()
            {
                Value = result,
                Offset = offset,
                Size = strLength.Value + sizeof(byte)
            };
        }

        public StringMemoryBlock ReadString(int maxBytes)
        {
            var result = ReadString();
            ReadBytes(maxBytes - result.Value.Length);
            return result;
        }

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

        public new CharMemoryBlock ReadChar()
        {
            var offset = BaseStream.Position;
            var result = base.ReadChar();
            return new CharMemoryBlock()
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

        public StringMemoryBlock ReadLongString()
        {
            var offset = BaseStream.Position;
            int stringLength = base.ReadInt32();
            var bytes = ReadBytes(stringLength);
            byte[] win1251Bytes = Encoding.Convert(utf8, win1251, bytes.Value.ToArray());
            var result = win1251.GetString(win1251Bytes);
            return new StringMemoryBlock()
            {
                Value = result,
                Offset = offset,
                Size = stringLength + sizeof(int)
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

        public void Skip(int count)
        {
            lastSkipped = ReadBytes(count);
        }

        public void SkipWhile(Func<bool> callback)
        {
            while (true)
            {
                var lastPosition = BaseStream.Position;
                if (!callback())
                {
                    BaseStream.Seek(lastPosition, SeekOrigin.Begin);
                    break;
                }
            }
        }

        public void SkipWhile(byte b)
        {
            SkipWhile(() => ReadByte().Value == b);
        }

        public void SkipWhile(int b)
        {
            SkipWhile(() => ReadInt32().Value == b);
        }

        public void SkipWhile(float b)
        {
            SkipWhile(() => ReadSingle().Value == b);
        }

        public void SkipWhile(double b)
        {
            SkipWhile(() => ReadDouble().Value == b);
        }

        public void SkipWhile(bool b)
        {
            SkipWhile(() => ReadBoolean().Value == b);
        }

        public void SkipWhile(char b)
        {
            SkipWhile(() => ReadChar().Value == b);
        }

        public void Back(int step = 1)
        {
            BaseStream.Seek(BaseStream.Position - step, SeekOrigin.Begin);
        }

        public ByteArrayMemoryBlock ReadToEnd()
        {
            long offset = BaseStream.Position;
            var result = new List<byte>();

            while (BaseStream.Length > BaseStream.Position)
            {
                result.Add(ReadByte().Value);
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
