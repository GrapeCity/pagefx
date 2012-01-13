using System;
using System.Runtime.CompilerServices;

namespace flash.text.engine
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class Kerning : Avm.Object
    {
        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String AUTO;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String ON;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String OFF;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Kerning();
    }
}
