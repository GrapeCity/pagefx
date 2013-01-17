using System;
using System.Runtime.CompilerServices;

namespace flash.text.engine
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class CFFHinting : Avm.Object
    {
        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String NONE;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String HORIZONTAL_STEM;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern CFFHinting();
    }
}
