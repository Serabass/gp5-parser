using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace GTP5Parser.Binary
{
    public partial class MyBinaryReader : BinaryReader
    {
        public ByteArrayMemoryBlock lastSkipped;

        public void Skip(int count)
        {
            lastSkipped = this << count;
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
            SkipWhile(() => ReadByte() == b);
        }

        public void SkipWhile(int b)
        {
            SkipWhile(() => ReadInt32() == b);
        }

        public void SkipWhile(float b)
        {
            SkipWhile(() => ReadSingle() == b);
        }

        public void SkipWhile(double b)
        {
            SkipWhile(() => ReadDouble() == b);
        }

        public void SkipWhile(bool b)
        {
            SkipWhile(() => ReadBoolean() == b);
        }

        public void SkipWhile(char b)
        {
            SkipWhile(() => ReadChar() == b);
        }

        public void Back(int step = 1)
        {
            BaseStream.Seek(BaseStream.Position - step, SeekOrigin.Begin);
        }

    }
}
