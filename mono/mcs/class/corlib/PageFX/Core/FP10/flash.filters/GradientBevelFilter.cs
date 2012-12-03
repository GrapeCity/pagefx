using System;
using System.Runtime.CompilerServices;

namespace flash.filters
{
    /// <summary>
    /// The GradientBevelFilter class lets you apply a gradient bevel effect to
    /// display objects. A gradient bevel is a beveled edge, enhanced with gradient color,
    /// on the outside, inside, or top of an object. Beveled edges make objects look
    /// three-dimensional.
    /// You can apply the filter to any display object (that is, objects that inherit from the DisplayObject class),
    /// such as MovieClip, SimpleButton, TextField, and Video objects, as well as to BitmapData objects.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class GradientBevelFilter : BitmapFilter
    {
        public extern virtual Avm.Array colors
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual double strength
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual double blurX
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual double blurY
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual double angle
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual Avm.String type
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual Avm.Array ratios
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual Avm.Array alphas
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual bool knockout
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual double distance
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual int quality
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GradientBevelFilter(double arg0, double arg1, Avm.Array arg2, Avm.Array arg3, Avm.Array arg4, double arg5, double arg6, double arg7, int arg8, Avm.String arg9, bool arg10);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GradientBevelFilter(double arg0, double arg1, Avm.Array arg2, Avm.Array arg3, Avm.Array arg4, double arg5, double arg6, double arg7, int arg8, Avm.String arg9);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GradientBevelFilter(double arg0, double arg1, Avm.Array arg2, Avm.Array arg3, Avm.Array arg4, double arg5, double arg6, double arg7, int arg8);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GradientBevelFilter(double arg0, double arg1, Avm.Array arg2, Avm.Array arg3, Avm.Array arg4, double arg5, double arg6, double arg7);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GradientBevelFilter(double arg0, double arg1, Avm.Array arg2, Avm.Array arg3, Avm.Array arg4, double arg5, double arg6);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GradientBevelFilter(double arg0, double arg1, Avm.Array arg2, Avm.Array arg3, Avm.Array arg4, double arg5);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GradientBevelFilter(double arg0, double arg1, Avm.Array arg2, Avm.Array arg3, Avm.Array arg4);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GradientBevelFilter(double arg0, double arg1, Avm.Array arg2, Avm.Array arg3);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GradientBevelFilter(double arg0, double arg1, Avm.Array arg2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GradientBevelFilter(double arg0, double arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GradientBevelFilter(double arg0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GradientBevelFilter();

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override BitmapFilter clone();


    }
}
