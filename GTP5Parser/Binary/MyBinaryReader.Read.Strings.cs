using System.Text;

namespace GTP5Parser.Binary
{
    public partial class MyBinaryReader
    {
        public new StringMemoryBlock ReadString()
        {
            var offset = BaseStream.Position;
            var strLength = Byte;
            var bytes = this << strLength.Value;
            // byte[] win1251Bytes = Encoding.Convert(utf8, win1251, bytes.Value.ToArray());
            var result = _win1251.GetString(bytes);
            return new StringMemoryBlock()
            {
                Value = result,
                Offset = offset,
                Size = strLength + sizeof(byte)
            };
        }

        public StringMemoryBlock ReadIntByteString()
        {
            var offset = BaseStream.Position;
            var intLength = Int32;
            var strLength = Byte;
            var bytes = this << strLength.Value;
            // byte[] win1251Bytes = Encoding.Convert(utf8, win1251, bytes.Value.ToArray());
            var result = _win1251.GetString(bytes);
            return new StringMemoryBlock
            {
                Value = result,
                Offset = offset,
                Size = strLength + sizeof(byte)
            };
        }

        public StringMemoryBlock ReadString(int maxBytes)
        {
            var result = ~this;
            Skip(maxBytes - result.Length);
            return result;
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

        public StringMemoryBlock ReadLongString()
        {
            var offset = BaseStream.Position;
            int stringLength = base.ReadInt32();
            var bytes = this << stringLength;
            byte[] win1251Bytes = Encoding.Convert(_utf8, _win1251, bytes);
            var result = _win1251.GetString(win1251Bytes);
            return new StringMemoryBlock()
            {
                Value = result,
                Offset = offset,
                Size = stringLength + sizeof(int)
            };
        }

    }
}
