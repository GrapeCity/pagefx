using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    [PageFX.AbcInstance(116)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public class GraphicsSolidFill : Avm.Object, flash.display.IGraphicsFill, flash.display.IGraphicsData
    {
        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public uint color;

        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public double alpha;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsSolidFill(uint color, double alpha);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsSolidFill(uint color);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsSolidFill();
    }
}
