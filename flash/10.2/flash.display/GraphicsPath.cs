using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    [PageFX.AbcInstance(258)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public class GraphicsPath : Avm.Object, flash.display.IGraphicsPath, flash.display.IGraphicsData
    {
        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public Avm.Vector<Avm.String> commands;

        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public Avm.Vector<Avm.String> data;

        public extern virtual Avm.String winding
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

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsPath(Avm.Vector<Avm.String> commands, Avm.Vector<Avm.String> data, Avm.String winding);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsPath(Avm.Vector<Avm.String> commands, Avm.Vector<Avm.String> data);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsPath(Avm.Vector<Avm.String> commands);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsPath();

        [PageFX.AbcInstanceTrait(5)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void moveTo(double x, double y);

        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void lineTo(double x, double y);

        [PageFX.AbcInstanceTrait(7)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void curveTo(double controlX, double controlY, double anchorX, double anchorY);

        [PageFX.AbcInstanceTrait(8)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void wideLineTo(double x, double y);

        [PageFX.AbcInstanceTrait(9)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void wideMoveTo(double x, double y);
    }
}
