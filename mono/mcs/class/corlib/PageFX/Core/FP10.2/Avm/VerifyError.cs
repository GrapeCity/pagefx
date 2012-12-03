using System;
using System.Runtime.CompilerServices;

namespace Avm
{
    /// <summary>
    /// The VerifyError class represents an error that occurs when a malformed
    /// or corrupted SWF file is encountered.
    /// </summary>
    [PageFX.AbcInstance(44)]
    [PageFX.ABC]
    [PageFX.QName("VerifyError")]
    [PageFX.FP9]
    public class VerifyError : Avm.Error
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern VerifyError(Avm.String message, int id);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern VerifyError(Avm.String message);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern VerifyError();
    }
}
