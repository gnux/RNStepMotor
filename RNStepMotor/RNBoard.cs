using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;

namespace RNStepMotor
{
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

    public enum RotatingDirection : byte
    {
        Left = 0,
        Right = 1
    }

    public class RNCommandLibrary : IDisposable
    {
        byte[] _answer = null;
        AutoResetEvent _dataAvailable = new AutoResetEvent(false);
        bool _crc = false;

        //        Motor[] _motors = new Motor[2];

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
                Thread.Sleep(20);
                if (_conn.BytesToWrite > 0)
                    Thread.Sleep(50);
                if (_conn.BytesToWrite > 0)
                    throw new TimeoutException("while write!");
            }
            if (!(_dataAvailable.WaitOne(5000) && _answer != null))
                throw new TimeoutException("Timeout occured while waiting for response\n" + "Message was: " + Utils.ByteArrayToHexString(command));

            lock (_answer)
            {
                //CheckReturnValue(_answer);
            }

            return null;
        }

        private void ReceiveHandler(object sender, SerialDataReceivedEventArgs args)
        {
            Console.Write("HALLO");
            return;
        }
        #endregion

        public void ResetStepCounter(MotorSelection motors)
        {
            lock (this)
            {
                SendCommand(new byte[] { 14, (byte)motors });
            }
        }

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
