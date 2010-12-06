using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RNStepMotor
{
    /// <summary>
    /// Choose motors assigned for command.
    /// Up to eight motors are selectable.
    /// </summary>
    public enum MotorSelection : byte
    {
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
}
