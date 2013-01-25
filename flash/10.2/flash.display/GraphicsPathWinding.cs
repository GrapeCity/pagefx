using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    [PageFX.AbcInstance(361)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public partial class GraphicsPathWinding : Avm.Object
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String EVEN_ODD;

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String NON_ZERO;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsPathWinding();
    }
}
