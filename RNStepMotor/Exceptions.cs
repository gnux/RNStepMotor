using System;

namespace RNStepMotor
{
    public sealed class RNCrcException : Exception
    {
        public RNCrcException(string message) : base(message) { }
        public RNCrcException() : base() { }
        public RNCrcException(string message, Exception innerException) : base(message, innerException) { }
    }

    public sealed class RNConnectionTimeOutException : Exception
    {
        public RNConnectionTimeOutException(string message) : base(message) { }
        public RNConnectionTimeOutException() : base() { }
        public RNConnectionTimeOutException(string message, Exception innerException) : base(message, innerException) { }
    }
}