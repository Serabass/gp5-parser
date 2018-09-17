using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTP5Parser
{
    public enum KeySignsType {
        Abm = 0xF9,
        Ebm = 0xFA,
        Bbm = 0xFB,
        Fm = 0xFC,
        Cm = 0xFD,
        Gm = 0xFE,
        Dm = 0xFF,
        Am = 0x00,
        Em = 0x01,
        Bm = 0x02,
        F_m = 0x03,
        C_m = 0x04,
        G_m = 0x05,
        D_m = 0x06,
        A_m = 0x07,
    }

    public class KeySigns
    {
        public byte Value;
        public KeySignsType Type;
    }
}
