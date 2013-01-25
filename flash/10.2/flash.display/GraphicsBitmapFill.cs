using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    [PageFX.AbcInstance(196)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public partial class GraphicsBitmapFill : Avm.Object, flash.display.IGraphicsFill, flash.display.IGraphicsData
    {
        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public flash.display.BitmapData bitmapData;

        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public flash.geom.Matrix matrix;

        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public bool repeat;

        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public bool smooth;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsBitmapFill(flash.display.BitmapData bitmapData, flash.geom.Matrix matrix, bool repeat, bool smooth);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsBitmapFill(flash.display.BitmapData bitmapData, flash.geom.Matrix matrix, bool repeat);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsBitmapFill(flash.display.BitmapData bitmapData, flash.geom.Matrix matrix);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsBitmapFill(flash.display.BitmapData bitmapData);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsBitmapFill();
    }
}
