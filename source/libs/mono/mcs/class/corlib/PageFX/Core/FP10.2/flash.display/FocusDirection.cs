using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    [PageFX.AbcInstance(89)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public class FocusDirection : Avm.Object
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String TOP;

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String BOTTOM;

        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String NONE;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern FocusDirection();
    }
}
