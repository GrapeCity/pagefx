using System;
using System.Runtime.CompilerServices;

namespace flash.globalization
{
    [PageFX.AbcInstance(97)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public class CollatorMode : Avm.Object
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String SORTING;

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String MATCHING;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern CollatorMode();
    }
}
