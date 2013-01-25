using System;
using System.Runtime.CompilerServices;

namespace flash.errors
{
    /// <summary>
    /// The IllegalOperationError exception is thrown when a method is not implemented or the
    /// implementation doesn&apos;t cover the current usage.
    /// </summary>
    [PageFX.AbcInstance(83)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class IllegalOperationError : Avm.Error
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern IllegalOperationError(Avm.String message, int id);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern IllegalOperationError(Avm.String message);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern IllegalOperationError();
    }
}
