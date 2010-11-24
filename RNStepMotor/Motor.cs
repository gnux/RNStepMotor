using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RNStepMotor
{
    class Motor
    {
        private uint _motCurrent;
        private uint _holdCurrent;
        private uint _startCurrent;

        public Motor() { }


        public uint MotCurrent
        {
            get { return _motCurrent; }
            set
            {
                if (value >= 100 && value <= 2000) { _motCurrent = value; }
                else { throw new ArgumentException("Current must be in interval 100mA - 2000ma"); }
            }
        }
    }
}
