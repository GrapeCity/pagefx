using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    [PageFX.AbcInstance(362)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public class GraphicsPathCommand : Avm.Object
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static int NO_OP;

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static int MOVE_TO;

        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static int LINE_TO;

        [PageFX.AbcClassTrait(3)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static int CURVE_TO;

        [PageFX.AbcClassTrait(4)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static int WIDE_MOVE_TO;

        [PageFX.AbcClassTrait(5)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static int WIDE_LINE_TO;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsPathCommand();
    }
}
