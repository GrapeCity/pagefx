using System;
using System.Runtime.CompilerServices;

namespace flash.ui
{
    [PageFX.AbcInstance(352)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public class KeyboardType : Avm.Object
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String ALPHANUMERIC;

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYPAD;

        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String NONE;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern KeyboardType();
    }
}
