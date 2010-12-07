using System;
using RNStepMotor;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //byte[] val = { 0x0e, 0x01, 0x00, 0x00, 0x00, 0x00};

            byte test = 9;
            RNReturnValues val = (RNReturnValues)test;

            RNCommandLibrary rn = new RNCommandLibrary();
            try
            {
                //  rn.ResetStepCounter(MotorSelection.Motor1 | MotorSelection.Motor2);
                rn.Connect("COM2");


                rn.SetCRCMode(CRCMode.Enabled);
                rn.ResetStepCounter(MotorSelection.All);
                Console.WriteLine(DateTime.Now.Second + " " + DateTime.Now.Millisecond);
                string str = rn.GetVersionAndStateInformation();
                Console.WriteLine(str);
                Console.WriteLine(DateTime.Now.Second + " " + DateTime.Now.Millisecond);
                EndSwitchState[] states =  rn.GetEndSwitchStates();
                rn.SetRotatingDirection(MotorSelection.Motor2, RotatingDirection.Right);
                rn.SetSpeedAndAcceleration(MotorSelection.Motor2, 10, 10);
                rn.SetCurrent(CurrentSelection.StartCurrent, MotorSelection.Motor1 | MotorSelection.Motor2, 300);
                rn.SetCurrent(CurrentSelection.MotorCurrent, MotorSelection.Motor1 | MotorSelection.Motor2, 300);
                rn.RotateSteps(MotorSelection.Motor2, 200);
                //rn.RotateSteps(, 200);
                //rn.ResetStepCounter(MotorSelection.Motor1 | MotorSelection.Motor2);
                // rn.RotateSteps(MotorSelection.Motor1, 300);
                // rn.RotateSteps(MotorSelection.Motor2, 300);
            }
            catch (Exception e)
            {

            }
            finally
            {
                rn.Disconnect();
                rn.Dispose();
            }
            //byte crc = rn.TestCrc(val);
            //Console.WriteLine(crc + " 0x" + Convert.ToString(crc, 16));
            //Console.Read();

        }
    }
}
