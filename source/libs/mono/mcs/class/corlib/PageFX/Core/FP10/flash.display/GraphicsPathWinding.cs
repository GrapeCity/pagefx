using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class GraphicsPathWinding : Avm.Object
    {
        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String EVEN_ODD;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String NON_ZERO;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsPathWinding();
    }
}
