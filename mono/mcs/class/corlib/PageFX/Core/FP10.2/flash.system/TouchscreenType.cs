using System;
using System.Runtime.CompilerServices;

namespace flash.system
{
    [PageFX.AbcInstance(328)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public class TouchscreenType : Avm.Object
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String FINGER;

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STYLUS;

        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String NONE;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TouchscreenType();
    }
}
