using System;
using System.Runtime.CompilerServices;

namespace flash.errors
{
    /// <summary>
    /// The IllegalOperationError exception is thrown when a method is not implemented or the
    /// implementation doesn&apos;t cover the current usage.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class IllegalOperationError : Avm.Error
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern IllegalOperationError(Avm.String arg0, int arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern IllegalOperationError(Avm.String arg0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern IllegalOperationError();
    }
}
