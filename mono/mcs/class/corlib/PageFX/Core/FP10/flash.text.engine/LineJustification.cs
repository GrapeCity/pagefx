using System;
using System.Runtime.CompilerServices;

namespace flash.text.engine
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class LineJustification : Avm.Object
    {
        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String ALL_BUT_LAST;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String UNJUSTIFIED;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String ALL_INCLUDING_LAST;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern LineJustification();
    }
}
