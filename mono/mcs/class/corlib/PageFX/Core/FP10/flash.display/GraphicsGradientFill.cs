using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class GraphicsGradientFill : Avm.Object, IGraphicsFill, IGraphicsData
    {
        [PageFX.ABC]
        [PageFX.FP10]
        public flash.geom.Matrix matrix;

        [PageFX.ABC]
        [PageFX.FP10]
        public double focalPointRatio;

        [PageFX.ABC]
        [PageFX.FP10]
        public Avm.Array ratios;

        [PageFX.ABC]
        [PageFX.FP10]
        public Avm.Array colors;

        [PageFX.ABC]
        [PageFX.FP10]
        public Avm.Array alphas;

        public extern virtual Avm.String interpolationMethod
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

        public extern virtual Avm.String spreadMethod
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

        public extern virtual Avm.String type
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
        public extern GraphicsGradientFill(Avm.String arg0, Avm.Array arg1, Avm.Array arg2, Avm.Array arg3, object arg4, object arg5, Avm.String arg6, double arg7);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsGradientFill(Avm.String arg0, Avm.Array arg1, Avm.Array arg2, Avm.Array arg3, object arg4, object arg5, Avm.String arg6);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsGradientFill(Avm.String arg0, Avm.Array arg1, Avm.Array arg2, Avm.Array arg3, object arg4, object arg5);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsGradientFill(Avm.String arg0, Avm.Array arg1, Avm.Array arg2, Avm.Array arg3, object arg4);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsGradientFill(Avm.String arg0, Avm.Array arg1, Avm.Array arg2, Avm.Array arg3);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsGradientFill(Avm.String arg0, Avm.Array arg1, Avm.Array arg2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsGradientFill(Avm.String arg0, Avm.Array arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsGradientFill(Avm.String arg0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsGradientFill();


    }
}
