using System;
using System.Runtime.CompilerServices;

namespace flash.text.engine
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class JustificationStyle : Avm.Object
    {
        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String PUSH_IN_KINSOKU;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String PRIORITIZE_LEAST_ADJUSTMENT;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String PUSH_OUT_ONLY;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern JustificationStyle();
    }
}
