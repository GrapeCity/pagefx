namespace Microsoft.VisualBasic.CompilerServices
{
    using System;

    public sealed class IncompleteInitialization : Exception
    {
        public IncompleteInitialization()
        {
        }

        public IncompleteInitialization(string message) : base(message)
        {
        }

        public IncompleteInitialization(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

