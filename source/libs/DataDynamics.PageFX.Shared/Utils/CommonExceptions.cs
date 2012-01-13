using System;

namespace DataDynamics
{
    [Serializable]
    public sealed class BadFormatException : Exception
    {
        public BadFormatException()
        {
        }

        public BadFormatException(string message)
            : base(message)
        {
        }

        public BadFormatException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    [Serializable]
    public sealed class BadInstructionException : Exception
    {
        public BadInstructionException(int code)
        {
            _code = code;
        }

        public int Code
        {
            get { return _code; }
        }
        private readonly int _code;

        public override string Message
        {
            get
            {
                return string.Format("The format of instruction with code {0} is invalid", _code);
            }
        }
    }
}