using System;
using System.Runtime.CompilerServices;

namespace flash.globalization
{
    [PageFX.AbcInstance(270)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public class DateTimeStyle : Avm.Object
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String CUSTOM;

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String LONG;

        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String MEDIUM;

        [PageFX.AbcClassTrait(3)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String SHORT;

        [PageFX.AbcClassTrait(4)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String NONE;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DateTimeStyle();
    }
}
