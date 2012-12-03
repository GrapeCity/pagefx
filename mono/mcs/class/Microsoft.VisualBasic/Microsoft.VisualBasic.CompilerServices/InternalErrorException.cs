namespace Microsoft.VisualBasic.CompilerServices
{
    using System;

    public sealed class InternalErrorException : Exception
    {
        public InternalErrorException() : base(Utils.GetResourceString("InternalError"))
        {
        }

        public InternalErrorException(string message) : base(message)
        {
        }

        public InternalErrorException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

