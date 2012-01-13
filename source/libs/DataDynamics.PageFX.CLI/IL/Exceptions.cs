using System;

namespace DataDynamics.PageFX.CLI.IL
{
    [Serializable]
    public class DecompileException : Exception
    {
        public DecompileException() { }
        public DecompileException(string message) : base(message) { }
        public DecompileException(string message, Exception inner) : base(message, inner) { }
    }

    [Serializable]
    public class ILTranslatorException : Exception
    {
        public ILTranslatorException() { }
        public ILTranslatorException(string message) : base(message) { }
        public ILTranslatorException(string message, Exception inner) : base(message, inner) { }
    }
}