using System;
using RNStepMotor;
//using RNStepMotor.Utils;
//using EnumString;
using System.ComponentModel;
using System.Reflection;
using gnux.Extensions.extEnums;

namespace Test
{
    class testAttribute : Attribute { }

    public enum LaLa : byte
    {
        [DescriptionAttribute("Enum a")]
        [test]
        a = 45,
        [DescriptionAttribute("Enum b")]
        b,
        [DescriptionAttribute("Enum c")]
        c
    }

    //public class LaLaExtensions
    //{
    //    public static string ConvertToString(this LaLa value)
    //    {
    //        if (value == null)
    //            throw new ArgumentNullException("value");
    //        Type type = value.GetType();
    //        FieldInfo fieldInfo = type.GetField(Enum.GetName(type, value));
    //        var descriptionAttribute =
    //            (DescriptionAttribute)Attribute.GetCustomAttribute(
    //                                       fieldInfo, typeof(DescriptionAttribute));

    //        if (descriptionAttribute != null)
    //            return descriptionAttribute.Description;
    //        return value.ToString();
    //    }
    //}

    //public static class ExtensionMethods
    //{

    //    public static string ToDescriptionString(this byte val)
    //    {
    //        return null;
    //    }
    //}

    class Program
    {
        static void Main(string[] args)
        {


            // Console.WriteLine((double)EnumStringHelper.GetAssociatedValue<LaLa>("wE"));
            //foreach(string str in EnumStringHelper.GetAssociatedString(lala.a))
            // Console.WriteLine(EnumStringHelper.GetAssociatedString(LaLa.c));
            // foreach (string str in EnumStringHelper.GetEnumStrings(typeof(LaLa)))
            //     Console.WriteLine(str);
            // foreach (string str in EnumStringHelper.GetEnumStringsArray(typeof(SpeedSetting)))


            LaLa c;
            c = LaLa.c;
            byte x;
            Console.WriteLine(c.ToDescriptionString());
            Console.WriteLine();
            Console.WriteLine(c.FromDescriptionString("enum a", true));
            Console.WriteLine();
            foreach (string str in c.GetDescriptionStrings())
                Console.WriteLine(str);
            Console.WriteLine();
            foreach (string str in c.GetDescriptionStringsList())
                Console.WriteLine(str);
            Console.ReadKey();

            //byte[] val = { 0x0e, 0x01, 0x00, 0x00, 0x00, 0x00};

            //byte test = 9;
            //RNReturnValues val = (RNReturnValues)test;

            //RNCommandLibrary rn = new RNCommandLibrary();
            //try
            //{
            //    //  rn.ResetStepCounter(MotorSelection.Motor1 | MotorSelection.Motor2);
            //    rn.Connect("COM2");


            //    rn.SetCRCMode(CRCMode.Enabled);
            //    rn.ResetStepCounter(MotorSelection.All);
            //    Console.WriteLine(DateTime.Now.Second + " " + DateTime.Now.Millisecond);
            //    string str = rn.GetVersionAndStateInformation();
            //    Console.WriteLine(str);
            //    Console.WriteLine(DateTime.Now.Second + " " + DateTime.Now.Millisecond);
            //    EndSwitchState[] states =  rn.GetEndSwitchStates();
            //    rn.SetRotatingDirection(MotorSelection.Motor2, RotatingDirection.Right);
            //    rn.SetSpeedAndAcceleration(MotorSelection.Motor2, 10, 10);
            //    rn.SetCurrent(CurrentSelection.StartCurrent, MotorSelection.Motor1 | MotorSelection.Motor2, 300);
            //    rn.SetCurrent(CurrentSelection.MotorCurrent, MotorSelection.Motor1 | MotorSelection.Motor2, 300);
            //    rn.RotateSteps(MotorSelection.Motor2, 200);
            //    //rn.RotateSteps(, 200);
            //    //rn.ResetStepCounter(MotorSelection.Motor1 | MotorSelection.Motor2);
            //    // rn.RotateSteps(MotorSelection.Motor1, 300);
            //    // rn.RotateSteps(MotorSelection.Motor2, 300);
            //}
            //catch (Exception e)
            //{

            //}
            //finally
            //{
            //    rn.Disconnect();
            //    rn.Dispose();
            //}
            ////byte crc = rn.TestCrc(val);
            ////Console.WriteLine(crc + " 0x" + Convert.ToString(crc, 16));
            ////Console.Read();

        }
    }
}
