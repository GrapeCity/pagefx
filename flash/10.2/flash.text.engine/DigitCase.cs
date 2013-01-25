using System;
using System.Runtime.CompilerServices;

namespace flash.text.engine
{
    [PageFX.AbcInstance(365)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public partial class DigitCase : Avm.Object
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String DEFAULT;

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String LINING;

        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String OLD_STYLE;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DigitCase();
    }
}
