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

            RNCommandLibrary rn = new RNCommandLibrary();
            try
            {
              //  rn.ResetStepCounter(MotorSelection.Motor1 | MotorSelection.Motor2);
                rn.Connect("COM2");
            }
            catch (Exception e)
            {
            
            }
            //rn.EnableCRC();
            Console.WriteLine(DateTime.Now.Second +" "+ DateTime.Now.Millisecond);
            string str = rn.GetVersionAndStateInformation();
            Console.WriteLine(str);
            Console.WriteLine(DateTime.Now.Second +" "+ DateTime.Now.Millisecond);
            //rn.ResetStepCounter(MotorSelection.Motor1 | MotorSelection.Motor2);
           // rn.RotateSteps(MotorSelection.Motor1, 300);
           // rn.RotateSteps(MotorSelection.Motor2, 300);
            rn.Disconnect();
            //byte crc = rn.TestCrc(val);
            //Console.WriteLine(crc + " 0x" + Convert.ToString(crc, 16));
            //Console.Read();

        }
    }
}
