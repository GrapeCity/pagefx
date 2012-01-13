using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class StageColorCorrectionMode : Avm.Object
    {
        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String DEFAULT;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String ALWAYS_ON;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String ALWAYS_OFF;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern StageColorCorrectionMode();
    }
}
