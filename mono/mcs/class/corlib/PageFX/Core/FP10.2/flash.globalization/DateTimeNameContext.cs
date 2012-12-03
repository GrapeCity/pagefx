using System;
using System.Runtime.CompilerServices;

namespace flash.globalization
{
    [PageFX.AbcInstance(115)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public class DateTimeNameContext : Avm.Object
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String FORMAT;

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STANDALONE;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DateTimeNameContext();
    }
}
