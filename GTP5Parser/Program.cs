using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTP5Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            Tab.FromFile("test.gp5");
        }
    }

    class MyBinaryReader : BinaryReader
    {
        Encoding utf8 = Encoding.GetEncoding("UTF-8");
        Encoding win1251 = Encoding.GetEncoding("Windows-1251");

        public MyBinaryReader(Stream input) : base(input)
        {
        }

        public string ReadString(int maxBytes)
        {
            var result = ReadString();
            ReadBytes(maxBytes - result.Length);
            return result;
        }

        public string ReadLongString()
        {
            int stringLength = ReadInt32();
            var bytes = ReadBytes(stringLength);
            byte[] win1251Bytes = Encoding.Convert(utf8, win1251, bytes.ToArray());
            var result = win1251.GetString(win1251Bytes);
            return result;
        }

        public void Skip(int count)
        {
            ReadBytes(count);
        }
    }
}
