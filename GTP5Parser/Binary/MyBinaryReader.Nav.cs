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
            SkipWhile(() => Byte == b);
        }

        public void SkipWhile(int i)
        {
            SkipWhile(() => Int32 == i);
        }

        public void SkipWhile(float f)
        {
            SkipWhile(() => Single == f);
        }

        public void SkipWhile(double d)
        {
            SkipWhile(() => Double == d);
        }

        public void SkipWhile(bool b)
        {
            SkipWhile(() => Boolean == b);
        }

        public void SkipWhile(char c)
        {
            SkipWhile(() => Char == c);
        }

        public void Back(int step = 1)
        {
            BaseStream.Seek(BaseStream.Position - step, SeekOrigin.Begin);
        }

    }
}
