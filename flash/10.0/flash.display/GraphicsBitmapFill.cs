using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class GraphicsBitmapFill : Avm.Object, IGraphicsFill, IGraphicsData
    {
        [PageFX.ABC]
        [PageFX.FP10]
        public flash.geom.Matrix matrix;

        [PageFX.ABC]
        [PageFX.FP10]
        public BitmapData bitmapData;

        [PageFX.ABC]
        [PageFX.FP10]
        public bool repeat;

        [PageFX.ABC]
        [PageFX.FP10]
        public bool smooth;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsBitmapFill(BitmapData arg0, flash.geom.Matrix arg1, bool arg2, bool arg3);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsBitmapFill(BitmapData arg0, flash.geom.Matrix arg1, bool arg2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsBitmapFill(BitmapData arg0, flash.geom.Matrix arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsBitmapFill(BitmapData arg0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsBitmapFill();
    }
}
