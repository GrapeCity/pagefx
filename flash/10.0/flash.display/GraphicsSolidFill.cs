using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class GraphicsSolidFill : Avm.Object, IGraphicsFill, IGraphicsData
    {
        [PageFX.ABC]
        [PageFX.FP10]
        public uint color;

        [PageFX.ABC]
        [PageFX.FP10]
        public double alpha;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsSolidFill(uint arg0, double arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsSolidFill(uint arg0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsSolidFill();
    }
}
