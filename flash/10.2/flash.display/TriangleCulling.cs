using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    [PageFX.AbcInstance(129)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public partial class TriangleCulling : Avm.Object
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String NONE;

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String POSITIVE;

        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String NEGATIVE;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TriangleCulling();
    }
}
