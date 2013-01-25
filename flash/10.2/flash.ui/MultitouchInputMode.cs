using System;
using System.Runtime.CompilerServices;

namespace flash.ui
{
    [PageFX.AbcInstance(160)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public partial class MultitouchInputMode : Avm.Object
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String NONE;

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String GESTURE;

        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String TOUCH_POINT;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern MultitouchInputMode();
    }
}
