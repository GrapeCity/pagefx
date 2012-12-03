using System;
using System.Runtime.CompilerServices;

namespace flash.text.engine
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class FontWeight : Avm.Object
    {
        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String BOLD;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String NORMAL;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern FontWeight();
    }
}
