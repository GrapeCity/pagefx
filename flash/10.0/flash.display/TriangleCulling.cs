using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class TriangleCulling : Avm.Object
    {
        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String NEGATIVE;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String NONE;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String POSITIVE;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TriangleCulling();
    }
}
