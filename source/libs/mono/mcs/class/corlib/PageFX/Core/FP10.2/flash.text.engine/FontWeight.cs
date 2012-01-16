using System;
using System.Runtime.CompilerServices;

namespace flash.text.engine
{
    [PageFX.AbcInstance(356)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public class FontWeight : Avm.Object
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String NORMAL;

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String BOLD;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern FontWeight();
    }
}
