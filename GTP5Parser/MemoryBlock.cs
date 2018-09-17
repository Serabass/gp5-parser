﻿
namespace GTP5Parser
{
    public class MemoryBlock<T>
    {
        public T Value;
        public long Offset;
        public long Size;

        public new string ToString()
        {
            return string.Format("{0} @ {1:X}", Value.ToString(), Offset);
        }
    }
}