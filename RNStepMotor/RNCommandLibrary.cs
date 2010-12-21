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
using System.IO.Ports;
using System.Threading;
using gnux.RNStepMotor.Utils;
using gnux.RNStepMotor.Definitions;
using gnux.RNStepMotor.Exceptions;

namespace gnux.RNStepMotor
{
    public class RNCommandLibrary : IDisposable
    {
        #region members
        byte[] _answer = null;
        AutoResetEvent _dataAvailable = new AutoResetEvent(false);
        int _timeoutAnswer = 1000;
        #endregion

        #region Constructors
        public RNCommandLibrary()
        {
            _conn.DataReceived += new SerialDataReceivedEventHandler(_conn_DataReceived);
            _conn.ErrorReceived += new SerialErrorReceivedEventHandler(_conn_ErrorReceived);
            _conn.ReadTimeout = 500;
            _conn.WriteTimeout = 500;
            _conn.StopBits = StopBits.One;
            _conn.Parity = Parity.None;
            _conn.DataBits = 8;
            _conn.BaudRate = 9600;
        }
        #endregion

        #region Get Set
        public int TimeoutAnswer
        {
            get { return _timeoutAnswer; }
            set { _timeoutAnswer = value; }
        }

        public int WriteTimeout
        {
            get { return _conn.WriteTimeout; }
            set { _conn.WriteTimeout = value; }
        }

        public int ReadTimeout
        {
            get { return _conn.ReadTimeout; }
            set { _conn.ReadTimeout = value; }
        }

        public string[] AvailablePorts
        {
            get { return SerialPort.GetPortNames(); }
        }
        #endregion

        #region Connection
        private SerialPort _conn = new SerialPort();

        public void Connect(string serPort)
        {
            lock (this)
            {
                if (_conn.IsOpen)
                    Disconnect();
                _conn.PortName = serPort;
                _conn.Open();
            }
        }

        public void Disconnect()
        {
            lock (this)
            {
                lock (_conn)
                {
                    if (!_conn.IsOpen)
                        return;
                    else
                        _conn.Close();
                }
            }
        }

        public string CurrentPort
        {
            get { return _conn.PortName; }
        }

        private byte[] SendCommand(byte[] data, bool waitResponse = true)
        {
            if (data.Length > 6)
                throw new ArgumentException("Commandpart max. lenght is 6!");
            if (data.Length < 6)
                data = RNStepBoard.PadCommand(data);
            byte[] command = new byte[9];
            command[0] = (byte)'!';
            command[1] = (byte)'#';
            for (int i = 0; i < 6; i++)
                command[i + 2] = data[i];
            command[8] = RNStepBoard.CalculateCRC8(data);
            lock (_conn)
            {
                _conn.Write(command, 0, 9);
            }

            if (!waitResponse)
                return null;

            _dataAvailable.Reset();

            if (!(_dataAvailable.WaitOne(_timeoutAnswer) && _answer != null))
                throw new RNConnectionTimeOutException("Timeout occured while waiting for response\n" + "Message was: " + RNStepBoard.ByteArrayToHexString(command));

            _dataAvailable.Reset();

            lock (_answer)
            {
                CheckReturnValue(_answer);
                Array.Resize(ref _answer, _answer.Length - 1);
                return _answer;
            }
        }

        private void CheckReturnValue(byte[] _answer)
        {
            switch ((RNReturnValues)_answer[_answer.Length - 1])
            {
                case RNReturnValues.OK: return;
                case RNReturnValues.UnknownCommand: throw new RNUnknownCommandException();
                case RNReturnValues.WrongCRC: throw new RNCrcException();
                case RNReturnValues.WrongSlaveID: throw new RNSlaveIDException();
                default: throw new RNUnknownReturnValueException();
            }
        }

        private void _conn_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort conn = (SerialPort)sender;
            lock (conn)
            {
                int size = 0;
                byte[] answer = new byte[size];
                while (conn.BytesToRead != 0)
                {
                    size += conn.BytesToRead;
                    int index = answer.Length;
                    Array.Resize(ref answer, size);
                    conn.Read(answer, index, size - index);
                    // Give some Time, because the board is slow! May this is a hack!
                    Thread.Sleep(20);
                }
                _answer = answer;
            }
            _dataAvailable.Set();
            return;
        }

        private void _conn_ErrorReceived(Object sender, SerialErrorReceivedEventArgs e)
        {
            //TODO:

            throw new NotImplementedException();
        }
        #endregion

        #region Commands
        public void SetCurrent(CurrentSelection current, MotorSelection motors, uint mA, SettingsDuration duration = SettingsDuration.UntilReset)
        {
            lock (this)
            {
                SendCommand(new byte[] { (byte)current, (byte)motors, (byte)(mA & 0x00FF), (byte)((mA & 0xFF00) >> 8), (byte)duration });
            }
        }

        public void SetStepWidth(StepWidth width, SettingsDuration duration = SettingsDuration.UntilReset)
        {
            lock (this)
            {
                SendCommand(new byte[] { (byte)RNCommands.SetStepWidth, (byte)MotorSelection.All, (byte)width, (byte)duration });
            }
        }

        public void ResetStepCounter(MotorSelection motors)
        {
            lock (this)
            {
                SendCommand(new byte[] { (byte)RNCommands.ResetStepCounter, (byte)motors });
            }
        }

        public void ActivateOrHold(MotorSelection motors)
        {
            lock (this)
            {
                SendCommand(new byte[] { (byte)RNCommands.ActivateOrHoldMotor, (byte)motors });
            }
        }

        public void TurnOff(MotorSelection motors)
        {
            lock (this)
            {
                SendCommand(new byte[] { (byte)RNCommands.TurnOffMotor, (byte)motors });
            }
        }

        public void SetRotatingDirection(MotorSelection motors, RotatingDirection direction)
        {
            lock (this)
            {
                SendCommand(new byte[] { (byte)RNCommands.SetRotationDirection, (byte)motors, (byte)direction });
            }
        }

        public void SetSpeedAndAcceleration(MotorSelection motors, SpeedSetting speed, Acceleration acceleration)
        {
            lock (this)
            {
                SendCommand(new byte[] { (byte)RNCommands.SetSpeedAndAcceleration, (byte)motors, (byte)speed, (byte)acceleration });
            }
        }

        public void StartContinuousRotation(MotorSelection motors)
        {
            lock (this)
            {
                SendCommand(new byte[] { (byte)RNCommands.StartContinuousRotation, (byte)motors });
            }
        }

        public void RotateSteps(MotorSelection motors, uint steps)
        {
            lock (this)
            {
                SendCommand(new byte[] { (byte)RNCommands.RotateNumberOfSteps, (byte)motors, (byte)(steps & 0x00FF), (byte)((steps & 0xFF00) >> 8) });
            }
        }

        public MotorState[] GetMotorState(MotorSelection motors)
        {
            lock (this)
            {
                byte[] answer = SendCommand(new byte[] { (byte)RNCommands.GetMotorState });
                MotorState[] states = new MotorState[answer.Length];
                for (int i = 0; i < answer.Length; ++i)
                    states[i] = (MotorState)answer[i];
                return states;
            }
        }

        public uint[] GetStepCounter(MotorSelection motors)
        {
            lock (this)
            {
                byte[] answer = SendCommand(new byte[] { (byte)RNCommands.GetStepCounter, (byte)motors });
                uint[] steps = new uint[answer.Length / 4];
                for (int i = 0; i < answer.Length / 4; ++i)
                {
                    steps[i] = 0;
                    for (int j = 0; j < 4; ++j)
                        steps[i] += answer[i * 4 + j] * (uint)Math.Pow(256, j);
                }
                return steps;
            }
        }

        public byte GetLastIC2Confirmation()
        {
            lock (this)
            {
                byte[] answer = SendCommand(new byte[] { (byte)RNCommands.GetLastIC2Confirmation });
                return answer[0];
            }
        }

        public EndSwitchState[] GetEndSwitchStates()
        {
            lock (this)
            {
                byte[] answer = SendCommand(new byte[] { (byte)RNCommands.GetEndSwitchStatus });
                EndSwitchState[] switches = new EndSwitchState[answer.Length * 8];
                for (int i = 0; i < answer.Length; ++i)
                    for (int j = 0; j < 8; ++j)
                        switches[i * 8 + j] = (((answer[i] >> j) & 0x01) == 0x01) ? EndSwitchState.On : EndSwitchState.Off;
                return switches;
            }
        }

        public void SetInterfaceMode(InterfaceMode mode)
        {
            lock (this)
            {
                SendCommand(new byte[] { (byte)RNCommands.SetConnectionMode, (byte)mode });
            }
        }

        public void SetCRCMode(CRCMode mode)
        {
            lock (this)
            {
                SendCommand(new byte[] { (byte)RNCommands.SetCRCMode, (byte)mode });
            }
        }

        public void SetIC2SlaveID(byte id)
        {
            lock (this)
            {
                SendCommand(new byte[] { (byte)RNCommands.SetIC2SlaveID, id });
            }
        }

        public void ResetBoard()
        {
            lock (this)
            {
                SendCommand(new byte[] { (byte)RNCommands.ResetBoard }, false);
            }
        }

        public byte[] GetEEPromValues(byte num)
        {
            lock (this)
            {
                byte[] answer = SendCommand(new byte[] { (byte)RNCommands.GetEepromContent, num });
                return answer;
            }
        }

        public string GetVersionAndStateInformation()
        {
            lock (this)
            {
                return RNStepBoard.ByteArrayToString(SendCommand(new byte[] { (byte)RNCommands.GetFirmwareVersionAndState }));
            }
        }
        #endregion

        #region IDisposable Member
        public void Dispose()
        {
            Disconnect();
            _conn.Dispose();
        }
        #endregion
    }
}
