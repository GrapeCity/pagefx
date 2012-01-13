using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class GraphicsPath : Avm.Object, IGraphicsPath, IGraphicsData
    {
        [PageFX.ABC]
        [PageFX.FP10]
        public Avm.Vector<int> commands;

        [PageFX.ABC]
        [PageFX.FP10]
        public Avm.Vector<double> data;

        public extern virtual Avm.String winding
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsPath(Avm.Vector<int> arg0, Avm.Vector<double> arg1, Avm.String arg2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsPath(Avm.Vector<int> arg0, Avm.Vector<double> arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsPath(Avm.Vector<int> arg0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsPath();

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void wideLineTo(double arg0, double arg1);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void moveTo(double arg0, double arg1);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void lineTo(double arg0, double arg1);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void curveTo(double arg0, double arg1, double arg2, double arg3);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void wideMoveTo(double arg0, double arg1);
    }
}
