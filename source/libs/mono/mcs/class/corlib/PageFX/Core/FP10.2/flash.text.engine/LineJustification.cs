using System;
using System.Runtime.CompilerServices;

namespace flash.text.engine
{
    [PageFX.AbcInstance(321)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public class LineJustification : Avm.Object
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String UNJUSTIFIED;

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String ALL_BUT_LAST;

        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String ALL_INCLUDING_LAST;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern LineJustification();
    }
}
