using System;
using System.Runtime.CompilerServices;

namespace flash.text.engine
{
    [PageFX.AbcInstance(311)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public class RenderingMode : Avm.Object
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String NORMAL;

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String CFF;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern RenderingMode();
    }
}
