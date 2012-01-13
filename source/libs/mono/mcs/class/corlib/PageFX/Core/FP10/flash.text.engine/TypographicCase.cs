using System;
using System.Runtime.CompilerServices;

namespace flash.text.engine
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class TypographicCase : Avm.Object
    {
        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String LOWERCASE;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String CAPS;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String DEFAULT;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String UPPERCASE;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String TITLE;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String SMALL_CAPS;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String CAPS_AND_SMALL_CAPS;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TypographicCase();
    }
}
