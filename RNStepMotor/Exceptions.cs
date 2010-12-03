﻿using System;

namespace RNStepMotor
{
    public sealed class RNCrcException : Exception
    {
        public RNCrcException(string message) : base(message) { }
        public RNCrcException() : base() { }
        public RNCrcException(string message, Exception innerException) : base(message, innerException) { }
    }
}