using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;

namespace RNStepMotor
{
    #region DEFINITIONS
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
        Motor8 = 128
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


    #endregion

    public class RNCommandLibrary : IDisposable
    {
        /// <summary>
        /// Definition of command sets
        /// </summary>
        private enum RNCommands : byte
        {
            SetMotorCurrent = 10,
            SetStartCurrent = 11,
            SetHoldCurrent = 12,
            SetStepWidth = 13,
            ResetStepCounter = 14,
            ActivateOrHoldMotor = 50,
            TurnOffMotor = 51,
            SetRotationDirection = 52,
            SetSpeedAndAcceleration = 53,
            StartContinuousRotation = 54,
            RotateNumberOfSteps = 55,
            Reserved = 100,
            GetMotorStatus = 101,
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

        byte[] _answer = null;
        AutoResetEvent _dataAvailable = new AutoResetEvent(false);
        bool _crc = false;


        public RNCommandLibrary() { }
        public void SetCurrent() { }

        #region CONNECTION
        SerialPort _conn = null;

        public void Connect(string serPort)
        {
            lock (this)
            {
                if (_conn != null)
                    Disconnect();
                _conn = new SerialPort(serPort, 9600, Parity.None, 8, StopBits.One);
                _conn.DataReceived += new SerialDataReceivedEventHandler(ReceiveHandler);
                _conn.ReadTimeout = 500;
                _conn.WriteTimeout = 500;
                _conn.Open();
            }
        }

        public void Disconnect()
        {
            lock (this)
            {
                if (_conn == null)
                    return;
                lock (_conn)
                {
                    if (_conn.IsOpen)
                        _conn.Close();
                    _conn.Dispose();
                }
                _conn = null;
            }
        }

        public string CurrentPort
        {
            get
            {
                if (_conn != null)
                    return _conn.PortName;
                else
                    return "";
            }
        }

        private byte[] SendCommand(byte[] data)
        {
            if (data.Length > 6)
                throw new ArgumentException("Commandpart max. lenght is 6!");
            if (data.Length < 6)
                data = Utils.Pad(data);
            byte[] command = new byte[9];
            command[0] = (byte)'!';
            command[1] = (byte)'#';
            for (int i = 0; i < 6; i++)
                command[i + 2] = data[i];
            if (_crc == true)
                command[8] = Utils.CalculateCRC8(data);
            else
                command[8] = 0;

            lock (_conn)
            {
                _conn.Write(command, 0, 9);
            }
            if (!(_dataAvailable.WaitOne(1000) && _answer != null))
                throw new TimeoutException("Timeout occured while waiting for response\n" + "Message was: " + Utils.ByteArrayToHexString(command));

            _dataAvailable.Reset();

            lock (_answer)
            {
                CheckReturnValue(_answer);
            }
            return _answer;
        }

        private void CheckReturnValue(byte[] _answer)
        {
            switch (_answer[_answer.Length - 1])
            {
                case 42: return;
                case 45: throw new NotImplementedException();
                case 44: throw new NotImplementedException();
                case 43: throw new NotImplementedException();
                default: throw new NotImplementedException();
            }
        }

        private void ReceiveHandler(object sender, SerialDataReceivedEventArgs args)
        {
            //wait 20ms
            Thread.Sleep(20);
            lock (_conn)
            {
                _answer = Utils.StringToByteArray(_conn.ReadExisting());
            }
            _dataAvailable.Set();
            return;
        }
        #endregion

        #region CommandsSetup
        public void SetMotorCurrent(MotorSelection motors, uint cur, SettingsDuration duration)
        {
            lock (this)
            {
                SendCommand(new byte[] { (byte)RNCommands.SetMotorCurrent, (byte)motors, (byte)(cur & 0x00FF), (byte)((cur & 0xFF00) >> 8), (byte)duration });
            }
        }

        public void SetStartCurrent(MotorSelection motors, uint cur, SettingsDuration duration)
        {
            lock (this)
            {
                SendCommand(new byte[] { (byte)RNCommands.SetStartCurrent, (byte)motors, (byte)(cur & 0x00FF), (byte)((cur & 0xFF00) >> 8), (byte)duration });
            }
        }

        public void SetHoldingCurrent(MotorSelection motors, uint cur, SettingsDuration duration)
        {
            lock (this)
            {
                SendCommand(new byte[] { (byte)RNCommands.SetHoldCurrent, (byte)motors, (byte)(cur & 0x00FF), (byte)((cur & 0xFF00) >> 8), (byte)duration });
            }
        }

        public void RotateSteps(MotorSelection motors, uint steps)
        {
            lock (this)
            {
                SendCommand(new byte[] { (byte)RNCommands.RotateNumberOfSteps, (byte)motors, (byte)(steps & 0x00FF), (byte)((steps & 0xFF00) >> 8) });
            }
        }

        public void ResetStepCounter(MotorSelection motors)
        {
            lock (this)
            {
                SendCommand(new byte[] { (byte)RNCommands.ResetStepCounter, (byte)motors });
            }
        }


        #endregion


        

        public void SetFullStep(MotorSelection motors)
        {
            lock (this)
            {
                SendCommand(new byte[] { 13, (byte)motors, 0 });
            }
        }

        public void SetHalfStep(MotorSelection motors)
        {
            SendCommand(new byte[] { 13, (byte)motors, 1 });
        }

        public void ActivateOrHold(MotorSelection motors)
        {
            SendCommand(new byte[] { 50, (byte)motors });
        }

        public void TurnOff(MotorSelection motors)
        {
            SendCommand(new byte[] { 51, (byte)motors });
        }

        public void SetRotatingDirection(MotorSelection motors, RotatingDirection direction)
        {
            SendCommand(new byte[] { 52, (byte)motors, (byte)direction });
        }

        public void EnableCRC() { _crc = true; }
        public void DisableCRC() { _crc = false; }

        #region IDisposable Member

        public void Dispose()
        {
            Disconnect();
        }

        #endregion
    }
}
