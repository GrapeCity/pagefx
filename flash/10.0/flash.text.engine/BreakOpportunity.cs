using System;
using System.Runtime.CompilerServices;

namespace flash.text.engine
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class BreakOpportunity : Avm.Object
    {
        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String ALL;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String AUTO;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String ANY;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String NONE;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern BreakOpportunity();
    }
}
