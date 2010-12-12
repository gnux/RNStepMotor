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

using EnumString;

namespace RNStepMotor
{
    /// <summary>
    /// Choose motors assigned for command.
    /// Up to eight motors are selectable.
    /// </summary>
    public enum MotorSelection : byte
    {
        None = 0,
        Motor1 = 1,
        Motor2 = 2,
        Motor3 = 4,
        Motor4 = 8,
        Motor5 = 16,
        Motor6 = 32,
        Motor7 = 64,
        Motor8 = 128,
        All = 255
    }

    public enum MotorState : byte
    {
        Off = 0,
        Stopped = 1,
        Turning = 2,
        ContinousRotation = 3
    }

    public enum SpeedSetting : byte
    {
        [StringValue("1000")]
        s1000 = 0,
        [StringValue("500")]
        s500 = 1,
        [StringValue("333")]
        s333 = 2,
        [StringValue("250")]
        s250 = 3,
        [StringValue("200")]
        s200 = 4,
        [StringValue("166")]
        s166 = 5,
        [StringValue("143")]
        s143 = 6,
        [StringValue("125")]
        s125 = 7,
        [StringValue("111")]
        s111 = 8,
        [StringValue("100")]
        s100 = 9,
        [StringValue("90")]
        s90 = 10,
        [StringValue("83")]
        s83 = 11,
        [StringValue("77")]
        s77 = 12,
        [StringValue("71")]
        s71 = 13,
        [StringValue("67")]
        s67 = 14,
        [StringValue("63")]
        s63 = 15,
        [StringValue("59")]
        s59 = 16,
        [StringValue("56")]
        s56 = 17,
        [StringValue("53")]
        s53 = 18,
        [StringValue("50")]
        s50 = 19,
        [StringValue("48")]
        s48 = 20,
        [StringValue("45")]
        s45 = 21

    }

    public enum EndSwitchState : byte
    {
        Off = 0,
        On = 1
    }

    /// <summary>
    /// Choose rotation direction.
    /// </summary>
    public enum RotatingDirection : byte
    {
        Left = 0,
        Right = 1
    }

    /// <summary>
    /// Choose how long a setting should least.
    /// </summary>
    public enum SettingsDuration : byte
    {
        UntilReset = 0,
        StoreInEeprom = 1
    }

    public enum CurrentSelection : byte
    {
        MotorCurrent = 10,
        StartCurrent = 11,
        HoldCurrent = 12
    }

    public enum StepWidth : byte
    {
        FullStep = 0,
        HalfStep = 1
    }

    public enum CRCMode : byte
    {
        Disabled = 0,
        Enabled = 1
    }

    public enum InterfaceMode : byte
    {
        RS232orIC2 = 0,
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
