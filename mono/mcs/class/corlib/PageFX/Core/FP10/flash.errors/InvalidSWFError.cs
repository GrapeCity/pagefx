using System;
using System.Runtime.CompilerServices;

namespace flash.errors
{
    [PageFX.ABC]
    [PageFX.FP9]
    public class InvalidSWFError : Avm.Error
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern InvalidSWFError(Avm.String arg0, int arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern InvalidSWFError(Avm.String arg0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern InvalidSWFError();
    }
}
