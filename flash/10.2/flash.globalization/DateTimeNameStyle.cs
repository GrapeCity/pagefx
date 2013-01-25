using System;
using System.Runtime.CompilerServices;

namespace flash.globalization
{
    [PageFX.AbcInstance(277)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public partial class DateTimeNameStyle : Avm.Object
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String FULL;

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String LONG_ABBREVIATION;

        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String SHORT_ABBREVIATION;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DateTimeNameStyle();
    }
}
