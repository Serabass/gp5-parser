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

    public class StructMemoryBlock<T> : MemoryBlock<T>
    {
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

        public int Length => Value.Length;
    }

    public class ShortMemoryBlock : MemoryBlock<short>
    {
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
    }

    public class BooleanMemoryBlock : MemoryBlock<bool>
    {
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
    }

    public class ByteMemoryBlock : MemoryBlock<byte>
    {
        public static implicit operator byte(ByteMemoryBlock block)
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
    }

    public class FloatMemoryBlock : MemoryBlock<float>
    {

        public static bool operator ==(FloatMemoryBlock block, float value)
        {
            return block.Value == value;
        }

        public static bool operator !=(FloatMemoryBlock block, float value)
        {
            return block.Value != value;
        }
    }

    public class DoubleMemoryBlock : MemoryBlock<double>
    {
        public static bool operator ==(DoubleMemoryBlock block, double value)
        {
            return block.Value == value;
        }

        public static bool operator !=(DoubleMemoryBlock block, double value)
        {
            return block.Value != value;
        }
    }

    public class CharMemoryBlock : MemoryBlock<char>
    {
        public static bool operator ==(CharMemoryBlock block, char value)
        {
            return block.Value == value;
        }

        public static bool operator !=(CharMemoryBlock block, char value)
        {
            return block.Value != value;
        }
    }

    public class SByteMemoryBlock : MemoryBlock<sbyte>
    {
        public static bool operator ==(SByteMemoryBlock block, sbyte value)
        {
            return block.Value == value;
        }

        public static bool operator !=(SByteMemoryBlock block, sbyte value)
        {
            return block.Value != value;
        }
    }
}
