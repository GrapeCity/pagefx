using System;
using System.Runtime.CompilerServices;

namespace flash.text.engine
{
    [PageFX.AbcInstance(140)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public partial class TextLineCreationResult : Avm.Object
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String SUCCESS;

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String EMERGENCY;

        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String COMPLETE;

        [PageFX.AbcClassTrait(3)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String INSUFFICIENT_WIDTH;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextLineCreationResult();
    }
}
