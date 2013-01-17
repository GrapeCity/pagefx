using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class GraphicsPathCommand : Avm.Object
    {
        [PageFX.ABC]
        [PageFX.FP10]
        public static int LINE_TO;

        [PageFX.ABC]
        [PageFX.FP10]
        public static int MOVE_TO;

        [PageFX.ABC]
        [PageFX.FP10]
        public static int CURVE_TO;

        [PageFX.ABC]
        [PageFX.FP10]
        public static int WIDE_LINE_TO;

        [PageFX.ABC]
        [PageFX.FP10]
        public static int WIDE_MOVE_TO;

        [PageFX.ABC]
        [PageFX.FP10]
        public static int NO_OP;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsPathCommand();
    }
}
