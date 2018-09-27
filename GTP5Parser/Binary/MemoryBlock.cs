using System;
using System.Linq;

namespace GTP5Parser.Binary
{
    public class MemoryBlock<T>
    {
        public T Value;
        public long Offset;
        public long Size;

        public MemoryBlock(T data)
        {
            Value = data;
        }

        public MemoryBlock(T data, long offset, long size) : this(data)
        {
            Offset = offset;
            Size = size;
        }

        public MemoryBlock()
        {
        }

        public new string ToString()
        {
            var result = Value.ToString().Replace("\0", "\\0");
            return string.Format("{0} @ {1:X}", result, Offset);
        }
    }

    public class StructMemoryBlock<T> : MemoryBlock<T>, IDisposable where T : IDisposable
    {
        public void Dispose()
        {
            Value.Dispose();
        }
    }

    public class StringMemoryBlock : MemoryBlock<string>
    {
        public static implicit operator string(StringMemoryBlock block)
        {
            return block.Value;
        }

        public static bool operator ==(StringMemoryBlock block, string value)
        {
            return block.Value == value;
        }

        public static bool operator !=(StringMemoryBlock block, string value)
        {
            return block.Value != value;
        }

        public char this[int index] => Value[index];

        public override bool Equals(object obj)
        {
            return Value == obj;
        }

        public int Length => Value.Length;
    }

    public class ShortMemoryBlock : MemoryBlock<short>
    {
        public new long Size = sizeof(short);
        public static implicit operator short(ShortMemoryBlock block)
        {
            return block.Value;
        }

        public static bool operator ==(ShortMemoryBlock block, short value)
        {
            return block.Value == value;
        }

        public static bool operator !=(ShortMemoryBlock block, short value)
        {
            return block.Value != value;
        }
        
        public override bool Equals(object obj)
        {
            return Value.ToString() == obj?.ToString();
        }

    }

    public class BooleanMemoryBlock : MemoryBlock<bool>
    {
        public new long Size = sizeof(bool);
        public static implicit operator bool(BooleanMemoryBlock block)
        {
            return block.Value;
        }

        public static bool operator ==(BooleanMemoryBlock block, bool value)
        {
            return block.Value == value;
        }

        public static bool operator !=(BooleanMemoryBlock block, bool value)
        {
            return block.Value != value;
        }
    }

    public class Int32MemoryBlock : MemoryBlock<int>
    {
        public new long Size = sizeof(int);
        public static implicit operator int(Int32MemoryBlock block)
        {
            return block.Value;
        }

        public static bool operator ==(Int32MemoryBlock block, int value)
        {
            return block.Value == value;
        }

        public static bool operator !=(Int32MemoryBlock block, int value)
        {
            return block.Value != value;
        }
        
        public override bool Equals(object obj)
        {
            return Value.ToString() == obj?.ToString();
        }

    }

    public class ByteMemoryBlock : MemoryBlock<byte>
    {
        public new long Size = sizeof(byte);
        public static implicit operator byte(ByteMemoryBlock block)
        {
            return block.Value;
        }

        public static implicit operator int(ByteMemoryBlock block)
        {
            return block.Value;
        }

        public static bool operator ==(ByteMemoryBlock block, byte value)
        {
            return block.Value == value;
        }

        public static bool operator !=(ByteMemoryBlock block, byte value)
        {
            return block.Value != value;
        }

        public bool IsOnAt(int position)
        {
            return (Value & (1 << position - 1)) != 0;
        }
    }

    public class ByteArrayMemoryBlock : MemoryBlock<byte[]>
    {
        public static implicit operator byte[] (ByteArrayMemoryBlock block)
        {
            return block.Value;
        }

        public byte this[int index]
        {
            get
            {
                return Value[index];
            }
            set
            {
                Value[index] = value;
            }
        }

        public string ToHexString(string separator = "  ")
        {
            return string.Join(separator, Value.Select(b => b.ToString("X2")));
        }

        public string ToDecString(string separator = "\t")
        {
            return string.Join(separator, Value.Select(b => b.ToString()));
        }
    }

    public class FloatMemoryBlock : MemoryBlock<float>
    {
        public new long Size = sizeof(float);
        public static bool operator ==(FloatMemoryBlock block, float value)
        {
            return block.Value == value;
        }

        public static bool operator !=(FloatMemoryBlock block, float value)
        {
            return block.Value != value;
        }
        
        public override bool Equals(object obj)
        {
            return Value.ToString() == obj?.ToString();
        }

    }

    public class DoubleMemoryBlock : MemoryBlock<double>
    {
        public new long Size = sizeof(double);
        public static bool operator ==(DoubleMemoryBlock block, double value)
        {
            return block.Value == value;
        }

        public static bool operator !=(DoubleMemoryBlock block, double value)
        {
            return block.Value != value;
        }
        
        public override bool Equals(object obj)
        {
            return Value.ToString() == obj?.ToString();
        }

    }

    public class CharMemoryBlock : MemoryBlock<char>
    {
        public new long Size = sizeof(char);
        public static bool operator ==(CharMemoryBlock block, char value)
        {
            return block.Value == value;
        }

        public static bool operator !=(CharMemoryBlock block, char value)
        {
            return block.Value != value;
        }
        
        public override bool Equals(object obj)
        {
            return Value.ToString() == obj?.ToString();
        }

    }

    public class SByteMemoryBlock : MemoryBlock<sbyte>
    {
        public new long Size = sizeof(sbyte);
        public static bool operator ==(SByteMemoryBlock block, sbyte value)
        {
            return block.Value == value;
        }

        public static bool operator !=(SByteMemoryBlock block, sbyte value)
        {
            return block.Value != value;
        }
        
        public override bool Equals(object obj)
        {
            return Value.ToString() == obj?.ToString();
        }
    }
}
