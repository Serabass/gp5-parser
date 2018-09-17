
namespace GTP5Parser
{
    public class Color
    {
        public static Color FromBytes(byte[] bytes)
        {
            return new Color()
            {
                Red = bytes[0],
                Green = bytes[1],
                Blue = bytes[2],
            }; 
        }

        public byte Red;
        public byte Green;
        public byte Blue;
    }
}
