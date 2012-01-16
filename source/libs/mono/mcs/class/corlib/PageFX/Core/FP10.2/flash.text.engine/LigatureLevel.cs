using System;
using System.Runtime.CompilerServices;

namespace flash.text.engine
{
    [PageFX.AbcInstance(349)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public class LigatureLevel : Avm.Object
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String NONE;

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String MINIMUM;

        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String COMMON;

        [PageFX.AbcClassTrait(3)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String UNCOMMON;

        [PageFX.AbcClassTrait(4)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String EXOTIC;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern LigatureLevel();
    }
}
