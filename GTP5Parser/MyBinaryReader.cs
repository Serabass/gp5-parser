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
            // byte[] win1251Bytes = Encoding.Convert(utf8, win1251, bytes.Value.ToArray());
            var result = win1251.GetString(bytes.Value);
            return new MemoryBlock<string>()
            {
                Value = result,
                Offset = offset,
                Size = strLength.Value + sizeof(byte)
            };
        }

        public MemoryBlock<string> ReadString(int maxBytes)
        {
            var result = ReadString();
            ReadBytes(maxBytes - result.Value.Length);
            return result;
        }

        public new MemoryBlock<float> ReadSingle()
        {
            var offset = BaseStream.Position;
            var result = base.ReadSingle();
            return new MemoryBlock<float>()
            {
                Offset = offset,
                Value = result
            };
        }

        public new MemoryBlock<double> ReadDouble()
        {
            var offset = BaseStream.Position;
            var result = base.ReadDouble();
            return new MemoryBlock<double>()
            {
                Offset = offset,
                Value = result
            };
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

        public new MemoryBlock<char> ReadChar()
        {
            var offset = BaseStream.Position;
            var result = base.ReadChar();
            return new MemoryBlock<char>()
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
                Offset = offset,
                Size = stringLength + sizeof(int)
            };
        }

        [Obsolete]
        public MemoryBlock<TStruct> ReadStruct<TStruct>(Action<MyBinaryReader, TStruct> action) where TStruct : IDisposable, new()
        {
            var offset = BaseStream.Position;
            var structObject = new TStruct();
            action(this, structObject);
            return new MemoryBlock<TStruct>
            {
                Value = structObject,
                Offset = offset,
                Size = BaseStream.Position - offset
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

        public Color ReadColor()
        {
            var color = ReadBytes(3);
            return Color.FromBytes(color.Value);
        }

        [Obsolete]
        public void Jump(long offset)
        {
            BaseStream.Seek(offset, SeekOrigin.Begin);
        }

        public MemoryBlock<byte[]> ReadToEnd()
        {
            long offset = BaseStream.Position;
            var result = new List<byte>();

            while (BaseStream.Length > BaseStream.Position)
            {
                result.Add(ReadByte().Value);
            }

            return new MemoryBlock<byte[]>
            {
                Offset = offset,
                Value = result.ToArray(),
                Size = result.Count
            };
        }
    }
}
