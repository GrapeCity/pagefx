using System;
using System.Runtime.CompilerServices;

namespace flash.ui
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class MouseCursor : Avm.Object
    {
        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String BUTTON;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String HAND;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String IBEAM;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String ARROW;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String AUTO;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern MouseCursor();
    }
}
