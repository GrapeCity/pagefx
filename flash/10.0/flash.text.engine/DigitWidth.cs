using System;
using System.Runtime.CompilerServices;

namespace flash.text.engine
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class DigitWidth : Avm.Object
    {
        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String DEFAULT;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String PROPORTIONAL;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String TABULAR;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DigitWidth();
    }
}
