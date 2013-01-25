using System;
using System.Runtime.CompilerServices;

namespace flash.errors
{
    /// <summary>The MemoryError exception is thrown when a memory allocation request fails.</summary>
    [PageFX.AbcInstance(49)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class MemoryError : Avm.Error
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern MemoryError(Avm.String message, int id);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern MemoryError(Avm.String message);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern MemoryError();
    }
}
