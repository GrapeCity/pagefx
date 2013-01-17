using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    [PageFX.AbcInstance(269)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public class GraphicsStroke : Avm.Object, flash.display.IGraphicsStroke, flash.display.IGraphicsData
    {
        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public double thickness;

        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public bool pixelHinting;

        [PageFX.AbcInstanceTrait(8)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public double miterLimit;

        [PageFX.AbcInstanceTrait(12)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public flash.display.IGraphicsFill fill;

        public extern virtual Avm.String caps
        {
            [PageFX.AbcInstanceTrait(2)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(3)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual Avm.String joints
        {
            [PageFX.AbcInstanceTrait(5)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(6)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual Avm.String scaleMode
        {
            [PageFX.AbcInstanceTrait(9)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(10)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsStroke(double thickness, bool pixelHinting, Avm.String scaleMode, Avm.String caps, Avm.String joints, double miterLimit, flash.display.IGraphicsFill fill);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsStroke(double thickness, bool pixelHinting, Avm.String scaleMode, Avm.String caps, Avm.String joints, double miterLimit);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsStroke(double thickness, bool pixelHinting, Avm.String scaleMode, Avm.String caps, Avm.String joints);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsStroke(double thickness, bool pixelHinting, Avm.String scaleMode, Avm.String caps);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsStroke(double thickness, bool pixelHinting, Avm.String scaleMode);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsStroke(double thickness, bool pixelHinting);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsStroke(double thickness);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsStroke();


    }
}
