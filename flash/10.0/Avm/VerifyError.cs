using System;
using System.Runtime.CompilerServices;

namespace Avm
{
    /// <summary>
    /// The VerifyError class represents an error that occurs when a malformed
    /// or corrupted SWF file is encountered.
    /// </summary>
    [PageFX.ABC]
    [PageFX.QName("VerifyError")]
    [PageFX.FP9]
    public class VerifyError : Error
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern VerifyError(String message, int id);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern VerifyError(String message);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern VerifyError();
    }
}
