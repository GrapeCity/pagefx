using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class GraphicsStroke : Avm.Object, IGraphicsStroke, IGraphicsData
    {
        [PageFX.ABC]
        [PageFX.FP10]
        public IGraphicsFill fill;

        [PageFX.ABC]
        [PageFX.FP10]
        public double thickness;

        [PageFX.ABC]
        [PageFX.FP10]
        public bool pixelHinting;

        [PageFX.ABC]
        [PageFX.FP10]
        public double miterLimit;

        public extern virtual Avm.String caps
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

        public extern virtual Avm.String joints
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

        public extern virtual Avm.String scaleMode
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
        public extern GraphicsStroke(double arg0, bool arg1, Avm.String arg2, Avm.String arg3, Avm.String arg4, double arg5, IGraphicsFill arg6);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsStroke(double arg0, bool arg1, Avm.String arg2, Avm.String arg3, Avm.String arg4, double arg5);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsStroke(double arg0, bool arg1, Avm.String arg2, Avm.String arg3, Avm.String arg4);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsStroke(double arg0, bool arg1, Avm.String arg2, Avm.String arg3);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsStroke(double arg0, bool arg1, Avm.String arg2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsStroke(double arg0, bool arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsStroke(double arg0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsStroke();


    }
}
