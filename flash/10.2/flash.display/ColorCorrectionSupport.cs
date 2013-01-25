using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    [PageFX.AbcInstance(159)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public partial class ColorCorrectionSupport : Avm.Object
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String UNSUPPORTED;

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String DEFAULT_ON;

        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String DEFAULT_OFF;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ColorCorrectionSupport();
    }
}
