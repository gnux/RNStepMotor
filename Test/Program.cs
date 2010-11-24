using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RNStepMotor;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //byte[] val = { 0x0e, 0x01, 0x00, 0x00, 0x00, 0x00};

            RNBoard rn = new RNBoard();
            try
            {
                rn.Connect("COM1");
            }
            catch (Exception e)
            {
            
            }
            rn.EnableCRC();
            rn.ResetStepCounter(MotorSelection.Motor1 | MotorSelection.Motor2);
            rn.Disconnect();
            //byte crc = rn.TestCrc(val);
            //Console.WriteLine(crc + " 0x" + Convert.ToString(crc, 16));
            //Console.Read();

        }
    }
}
