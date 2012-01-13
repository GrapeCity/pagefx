using System;
using System.Runtime.CompilerServices;

namespace flash.text.engine
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class TextLineValidity : Avm.Object
    {
        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String STATIC;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String INVALID;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String VALID;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String POSSIBLY_INVALID;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextLineValidity();
    }
}
