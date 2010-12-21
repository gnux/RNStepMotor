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

using System.ComponentModel;

namespace gnux.RNStepMotor.Definitions
{
    /// <summary>
    /// Choose motors assigned for command.
    /// Up to eight motors are selectable.
    /// </summary>
    public enum MotorSelection : byte
    {
        [Description("None")]
        None = 0,
        [Description("Motor 1")]
        Motor1 = 1,
        [Description("Motor 2")]
        Motor2 = 2,
        [Description("Motor 3")]
        Motor3 = 4,
        [Description("Motor 4")]
        Motor4 = 8,
        [Description("Motor 5")]
        Motor5 = 16,
        [Description("Motor 6")]
        Motor6 = 32,
        [Description("Motor 7")]
        Motor7 = 64,
        [Description("Motor 8")]
        Motor8 = 128,
        [Description("All")]
        All = 255
    }

    public enum MotorState : byte
    {
        [Description("OFF")]
        Off = 0,
        [Description("Stopped")]
        Stopped = 1,
        [Description("Turning")]
        Turning = 2,
        [Description("Continous rotation")]
        ContinousRotation = 3
    }

    public enum SpeedSetting : byte
    {
        [Description("1000")]
        _1000 = 0,
        [Description("500")]
        _500 = 1,
        [Description("333")]
        _333 = 2,
        [Description("250")]
        _250 = 3,
        [Description("200")]
        _200 = 4,
        [Description("150")]
        _150 = 6,
        [Description("125")]
        _125 = 7,
        [Description("100")]
        _100 = 9,
        [Description("90")]
        _90 = 10,
        [Description("80")]
        _80 = 12,
        [Description("70")]
        _70 = 13,
        [Description("60")]
        _59 = 16,
        [Description("50")]
        _50 = 19,
        [Description("45")]
        _45 = 21,
        [Description("40")]
        _40 = 24,
        [Description("35")]
        _35 = 28,
        [Description("30")]
        _30 = 32,
        [Description("25")]
        _25 = 39,
        [Description("20")]
        _20 = 49,
        [Description("15")]
        _15 = 66,
        [Description("10")]
        _10 = 99,
        [Description("8")]
        _8 = 124,
        [Description("6")]
        _6 = 166,
        [Description("5")]
        _5 = 199,
        [Description("4")]
        _4 = 249
    }

    public enum Acceleration : byte
    {
        [Description("Immediate")]
        immediate = 0,
        [Description("Very Fast")]
        vfast = 5,
        [Description("Fast")]
        fast = 15,
        [Description("Normal")]
        normal = 25,
        [Description("Slow")]
        slow = 35,
        [Description("Very Slow")]
        vslow = 50
    }

    public enum EndSwitchState : byte
    {
        [Description("OFF")]
        Off = 0,
        [Description("ON")]
        On = 1
    }

    /// <summary>
    /// Choose rotation direction.
    /// </summary>
    public enum RotatingDirection : byte
    {
        [Description("Left")]
        Left = 0,
        [Description("Right")]
        Right = 1
    }

    /// <summary>
    /// Choose how long a setting should least.
    /// </summary>
    public enum SettingsDuration : byte
    {
        [Description("Until Reset")]
        UntilReset = 0,
        [Description("Store in EEPRom")]
        StoreInEeprom = 1
    }

    public enum CurrentSelection : byte
    {
        [Description("Motor Current")]
        MotorCurrent = 10,
        [Description("Start Current")]
        StartCurrent = 11,
        HoldCurrent = 12
    }

    public enum StepWidth : byte
    {
        [Description("Full Step")]
        FullStep = 0,
        [Description("Half Step")]
        HalfStep = 1
    }

    public enum CRCMode : byte
    {
        [Description("Disabled")]
        Disabled = 0,
        [Description("Enabled")]
        Enabled = 1
    }

    public enum InterfaceMode : byte
    {
        [Description("RS232 or IC2")]
        RS232orIC2 = 0,
        [Description("External")]
        External = 2
    }

    /// <summary>
    /// Definition of command sets
    /// </summary>
    public enum RNCommands : byte
    {
        SetStepWidth = 13,
        ResetStepCounter = 14,
        ActivateOrHoldMotor = 50,
        TurnOffMotor = 51,
        SetRotationDirection = 52,
        SetSpeedAndAcceleration = 53,
        StartContinuousRotation = 54,
        RotateNumberOfSteps = 55,
        Reserved = 100,
        GetMotorState = 101,
        GetStepCounter = 102,
        GetLastIC2Confirmation = 103,
        GetEndSwitchStatus = 104,
        SetConnectionMode = 200,
        SetCRCMode = 201,
        SetIC2SlaveID = 202,
        ResetBoard = 203,
        GetEepromContent = 254,
        GetFirmwareVersionAndState = 255
    }

    public enum RNReturnValues : byte
    {
        OK = 42,
        UnknownCommand = 43,
        WrongCRC = 44,
        WrongSlaveID = 45
    }
}
