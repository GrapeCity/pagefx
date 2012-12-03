using System;
using System.Runtime.CompilerServices;

namespace flash.text.engine
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class DigitCase : Avm.Object
    {
        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String LINING;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String DEFAULT;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String OLD_STYLE;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DigitCase();
    }
}
