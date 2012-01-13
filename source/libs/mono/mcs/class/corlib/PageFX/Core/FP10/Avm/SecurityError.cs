using System;
using System.Runtime.CompilerServices;

namespace Avm
{
    /// <summary>
    /// The SecurityError  exception is thrown when some type of security violation
    /// takes place.
    /// </summary>
    [PageFX.ABC]
    [PageFX.QName("SecurityError")]
    [PageFX.FP9]
    public class SecurityError : Error
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern SecurityError(String message, int id);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern SecurityError(String message);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern SecurityError();
    }
}
