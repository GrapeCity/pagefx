using System;
using System.Runtime.CompilerServices;

namespace flash.ui
{
    [PageFX.AbcInstance(126)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public class MouseCursor : Avm.Object
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String AUTO;

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String ARROW;

        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String BUTTON;

        [PageFX.AbcClassTrait(3)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String HAND;

        [PageFX.AbcClassTrait(4)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String IBEAM;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern MouseCursor();
    }
}
