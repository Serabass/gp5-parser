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

        public MemoryBlock()
        {
        }

        public new string ToString()
        {
            var result = Value.ToString().Replace("\0", "\\0");
            return string.Format("{0} @ {1:X}", result, Offset);
        }
    }

    public class StringMemoryBlock : MemoryBlock<string>
    {
        public char this[int index] => Value[index];
    }

    public class ShortMemoryBlock : MemoryBlock<short> { }
    public class BooleanMemoryBlock : MemoryBlock<bool> { }
    public class Int32MemoryBlock : MemoryBlock<int> { }
    public class ByteMemoryBlock : MemoryBlock<byte> { }
    public class ByteArrayMemoryBlock : MemoryBlock<byte[]>
    {
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
    public class FloatMemoryBlock : MemoryBlock<float> { }
    public class DoubleMemoryBlock : MemoryBlock<double> { }
    public class CharMemoryBlock : MemoryBlock<char> { }
    public class SByteMemoryBlock : MemoryBlock<sbyte> { }
}
