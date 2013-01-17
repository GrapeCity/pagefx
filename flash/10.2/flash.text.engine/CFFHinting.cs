using System;
using System.Runtime.CompilerServices;

namespace flash.text.engine
{
    [PageFX.AbcInstance(22)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public class CFFHinting : Avm.Object
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String NONE;

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String HORIZONTAL_STEM;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern CFFHinting();
    }
}
