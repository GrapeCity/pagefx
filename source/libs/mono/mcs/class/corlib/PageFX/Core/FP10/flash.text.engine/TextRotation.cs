using System;
using System.Runtime.CompilerServices;

namespace flash.text.engine
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class TextRotation : Avm.Object
    {
        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String ROTATE_180;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String ROTATE_270;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String ROTATE_90;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String ROTATE_0;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String AUTO;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextRotation();
    }
}
