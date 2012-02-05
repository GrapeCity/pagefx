using System;

namespace DataDynamics.PageFX
{
    public class CompilerException : ApplicationException
    {
        public CompilerException(string code, string message)
            : base(string.Format("error {0}: {1}", code, message))
        {
            _errorCode = code;
        }

        public CompilerException(string code, string message, Exception innerException)
            : base(string.Format("error {0}: {1}", code, message), innerException)
        {
            _errorCode = code;
        }

        public CompilerException(Error err, string message)
            : this(err.Code, message)
        {
        }

        public CompilerException(Error err, string message, Exception innerException)
            : this(err.Code, message, innerException)
        {
        }

        public string ErrorCode
        {
            get { return _errorCode; }
            set { _errorCode = value; }
        }
        private string _errorCode;
    }
}