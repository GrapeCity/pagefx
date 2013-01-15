using System;

namespace DataDynamics.PageFX.Core.IL
{
	[Serializable]
    public class ILTranslatorException : Exception
    {
        public ILTranslatorException() { }
        public ILTranslatorException(string message) : base(message) { }
        public ILTranslatorException(string message, Exception inner) : base(message, inner) { }
    }
}