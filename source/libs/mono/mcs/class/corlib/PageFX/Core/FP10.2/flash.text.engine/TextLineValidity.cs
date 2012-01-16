using System;
using System.Runtime.CompilerServices;

namespace flash.text.engine
{
    [PageFX.AbcInstance(338)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public class TextLineValidity : Avm.Object
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String VALID;

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String POSSIBLY_INVALID;

        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String INVALID;

        [PageFX.AbcClassTrait(3)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STATIC;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextLineValidity();
    }
}
