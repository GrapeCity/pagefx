using System;
using System.Runtime.CompilerServices;

namespace flash.text.engine
{
    [PageFX.AbcInstance(359)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public class DigitWidth : Avm.Object
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String DEFAULT;

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String PROPORTIONAL;

        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String TABULAR;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DigitWidth();
    }
}
