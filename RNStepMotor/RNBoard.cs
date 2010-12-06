using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Threading;

namespace RNStepMotor
{
    public class RNCommandLibrary : IDisposable
    {
        byte[] _answer = null;
        AutoResetEvent _dataAvailable = new AutoResetEvent(false);
        public RNCommandLibrary() { }

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
            command[8] = Utils.CalculateCRC8(data);
            lock (_conn)
            {
                _conn.Write(command, 0, 9);
            }
            //TODO: Replace with chooseable timeout!
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
                    //TODO: introduce exceptions!
                case 42: return;
                case 45: throw new NotImplementedException();
                case 44: throw new NotImplementedException();
                case 43: throw new NotImplementedException();
                default: throw new NotImplementedException();
            }
        }

        private void ReceiveHandler(object sender, SerialDataReceivedEventArgs args)
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
                    // Give some Time, because the board is slow! Maybe a hack!
                    Thread.Sleep(20);
                }
                _answer = answer;
            }
            _dataAvailable.Set();
            return;
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

        public void SetSpeedAndAcceleration(MotorSelection motors, byte speed, byte acceleration)
        {
            lock (this)
            {
                SendCommand(new byte[] { (byte)RNCommands.SetSpeedAndAcceleration, speed, acceleration });
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

        public short GetStepCounter(MotorSelection motors)
        {
            lock (this)
            {
                byte[] answer = SendCommand(new byte[] { (byte)RNCommands.GetStepCounter });
                //TODO!!!

                // MotorState[] states = new MotorState[answer.Length];
                // for (int i = 0; i < answer.Length; ++i)
                //     states[i] = (MotorState)answer[i];
                return 0;
            }
        }

        public byte GetLastIC2Confirmation()
        {
            lock (this)
            {
                byte[] answer = SendCommand(new byte[] { (byte)RNCommands.GetLastIC2Confirmation });
                //TODO!!!

                // MotorState[] states = new MotorState[answer.Length];
                // for (int i = 0; i < answer.Length; ++i)
                //     states[i] = (MotorState)answer[i];
                return 0;
            }
        }

        public EndSwitchState[] GetEndSwitchStates()
        {
            throw new NotImplementedException();
            //return null;
        }

        public void SetInterfaceMode(InterfaceMode mode) { throw new NotImplementedException(); }

        public void SetCRCMode(CRCMode mode)
        {
            lock (this)
            {
                SendCommand(new byte[] { (byte)RNCommands.SetCRCMode, (byte)mode });
            }
        }

        public void SetIC2SlaveID(byte id)
        {
            if (id % 2 != 0)
                throw new ArgumentException("SlaveID must be even");
            lock (this)
            {
                SendCommand(new byte[] { (byte)RNCommands.SetIC2SlaveID, id });
            }
        }

        public void ResetBoard()
        {
            lock (this)
            {
                SendCommand(new byte[] { (byte)RNCommands.ResetBoard });
            }
        }

        public byte[] GetEEPromValues(byte num)
        {
            throw new NotImplementedException();
        }

        public string GetVersionAndStateInformation()
        {
            lock (this)
            {
                return Utils.ByteArrayToString(SendCommand(new byte[] { (byte)RNCommands.GetFirmwareVersionAndState }));
            }
        }



        #endregion

        #region IDisposable Member

        public void Dispose()
        {
            Disconnect();
        }

        #endregion
    }
}
