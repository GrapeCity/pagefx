using System;

namespace DataDynamics.PageFX.CLI.Metadata
{
    /// <summary>
    /// Occured when metadata within PE file is corrupted
    /// </summary>
    [Serializable]
    public sealed class BadMetadataException : Exception
    {
        public BadMetadataException()
        {
        }

        public BadMetadataException(string message) : base(message)
        {
        }
    }

    [Serializable]
    public class BadSignatureException : Exception
    {
        public BadSignatureException()
        {
        }

        public BadSignatureException(string message) : base(message)
        {
        }
    }

    [Serializable]
    public class BadTokenException : Exception
    {
        public BadTokenException(int token) : base(string.Format("Unable to resolve token {0}", (SimpleIndex)token))
        {
            Token = token;
        }

	    public int Token { get; private set; }
    }
}