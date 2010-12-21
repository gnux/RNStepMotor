/***********************************************************************
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 * 
 * (c) 2010, gnux
 */

using System;
using System.Text;

namespace gnux.RNStepMotor.Utils
{
    static class RNStepBoard
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

        internal static byte[] PadCommand(byte[] data)
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
                str.AppendFormat("{0:X2}", b);
            }
            return str.ToString();
        }

        internal static byte[] StringToByteArray(string str)
        {
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            return enc.GetBytes(str);
        }

        internal static string ByteArrayToString(byte[] arr)
        {
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            return enc.GetString(arr);
        }
    }

    
}
