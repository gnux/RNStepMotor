using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RNStepMotor
{
    static class Utils
    {
        internal static byte CalculateCRC8(byte[] value)
        {
            byte crc8 = 0;

            foreach (byte b in value)
            {
                byte c = b;
                for (byte i = 0; i <= 7; i++)
                {
                    byte j = (byte)((byte)1 & (byte)(c ^ crc8));
                    crc8 = (byte) (crc8 / 2);
                    c = (byte)(c / 2);
                    if (j != 0)
                        crc8 = (byte)(crc8 ^ (byte) 0x8C);
                }
            }
            return crc8;
        }

        internal static byte[] Pad(byte[] data)
        {
            byte[] pad = new byte[6];
            for (int i = 0; i < 6; i++)
                pad[i] = (data.Length > i) ? ((byte) data[i]) : (byte) 0;
            return pad;
        }

        internal static string ByteArrayToHexString(byte[] array)
        {
            StringBuilder str = new StringBuilder("0x");
            foreach (byte b in array)
            {
                str.Append(Convert.ToString(b, 16));
            }
            return str.ToString();
        }
    }
}
