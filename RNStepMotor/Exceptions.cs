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

namespace gnux.RNStepMotor.Exceptions
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