using System;
using System.Runtime.CompilerServices;

namespace flash.text.engine
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class TextLineCreationResult : Avm.Object
    {
        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String EMERGENCY;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String SUCCESS;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String COMPLETE;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String INSUFFICIENT_WIDTH;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextLineCreationResult();
    }
}
