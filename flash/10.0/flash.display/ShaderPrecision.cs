using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class ShaderPrecision : Avm.Object
    {
        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String FAST;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String FULL;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ShaderPrecision();
    }
}
