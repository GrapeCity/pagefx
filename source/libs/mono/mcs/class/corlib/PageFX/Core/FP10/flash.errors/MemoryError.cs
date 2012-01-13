using System;
using System.Runtime.CompilerServices;

namespace flash.errors
{
    /// <summary>The MemoryError exception is thrown when a memory allocation request fails.</summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class MemoryError : Avm.Error
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern MemoryError(Avm.String arg0, int arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern MemoryError(Avm.String arg0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern MemoryError();
    }
}
