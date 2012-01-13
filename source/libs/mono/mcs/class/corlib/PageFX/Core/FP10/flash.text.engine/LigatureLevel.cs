using System;
using System.Runtime.CompilerServices;

namespace flash.text.engine
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class LigatureLevel : Avm.Object
    {
        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String UNCOMMON;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String NONE;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String EXOTIC;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String COMMON;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String MINIMUM;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern LigatureLevel();
    }
}
