using System;
using System.Runtime.CompilerServices;

namespace flash.errors
{
    [PageFX.AbcInstance(86)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class InvalidSWFError : Avm.Error
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern InvalidSWFError(Avm.String message, int id);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern InvalidSWFError(Avm.String message);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern InvalidSWFError();
    }
}
