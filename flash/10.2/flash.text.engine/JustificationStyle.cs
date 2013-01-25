using System;
using System.Runtime.CompilerServices;

namespace flash.text.engine
{
    [PageFX.AbcInstance(351)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public partial class JustificationStyle : Avm.Object
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String PUSH_IN_KINSOKU;

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String PUSH_OUT_ONLY;

        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String PRIORITIZE_LEAST_ADJUSTMENT;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern JustificationStyle();
    }
}
