using System;
using System.Runtime.CompilerServices;

namespace Avm
{
    [PageFX.AbcInstance(45)]
    [PageFX.ABC]
    [PageFX.QName("UninitializedError")]
    [PageFX.FP9]
    public class UninitializedError : Avm.Error
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern UninitializedError(Avm.String message, int id);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern UninitializedError(Avm.String message);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern UninitializedError();
    }
}
