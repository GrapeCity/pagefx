using System;
using System.Runtime.CompilerServices;

namespace Avm
{
    [PageFX.ABC]
    [PageFX.QName("UninitializedError")]
    [PageFX.FP9]
    public class UninitializedError : Error
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern UninitializedError(String message, int id);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern UninitializedError(String message);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern UninitializedError();
    }
}
