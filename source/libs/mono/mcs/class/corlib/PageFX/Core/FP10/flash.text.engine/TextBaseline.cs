using System;
using System.Runtime.CompilerServices;

namespace flash.text.engine
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class TextBaseline : Avm.Object
    {
        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String DESCENT;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String IDEOGRAPHIC_BOTTOM;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String USE_DOMINANT_BASELINE;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String IDEOGRAPHIC_CENTER;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String IDEOGRAPHIC_TOP;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String ASCENT;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String ROMAN;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextBaseline();
    }
}
