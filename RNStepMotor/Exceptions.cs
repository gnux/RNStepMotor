using System;

namespace RNStepMotor
{
    /// <summary>
    /// RNCrcException is mapped to return value 44
    /// </summary>
    public sealed class RNCrcException : Exception
    {
        public RNCrcException(string message) : base(message) { }
        public RNCrcException() : base() { }
        public RNCrcException(string message, Exception innerException) : base(message, innerException) { }
    }

    public sealed class RNSlaveIDException : Exception
    {
        public RNSlaveIDException(string message) : base(message) { }
        public RNSlaveIDException() : base() { }
        public RNSlaveIDException(string message, Exception innerException) : base(message, innerException) { }
    }

    public sealed class RNUnknownCommandException : Exception
    {
        public RNUnknownCommandException(string message) : base(message) { }
        public RNUnknownCommandException() : base() { }
        public RNUnknownCommandException(string message, Exception innerException) : base(message, innerException) { }
    }

    public sealed class RNUnknownReturnValueException : Exception
    {
        public RNUnknownReturnValueException(string message) : base(message) { }
        public RNUnknownReturnValueException() : base() { }
        public RNUnknownReturnValueException(string message, Exception innerException) : base(message, innerException) { }
    }

    public sealed class RNConnectionTimeOutException : Exception
    {
        public RNConnectionTimeOutException(string message) : base(message) { }
        public RNConnectionTimeOutException() : base() { }
        public RNConnectionTimeOutException(string message, Exception innerException) : base(message, innerException) { }
    }
}