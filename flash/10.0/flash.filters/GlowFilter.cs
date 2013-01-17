using System;
using System.Runtime.CompilerServices;

namespace flash.filters
{
    /// <summary>
    /// The GlowFilter class lets you apply a glow effect to display objects.
    /// You have several options for the style of the
    /// glow, including inner or outer glow and knockout mode.
    /// The glow filter is similar to the drop shadow filter with the distance
    /// and angle  properties of the drop shadow filter set to 0.
    /// You can apply the filter to any display object (that is, objects that inherit from the DisplayObject class),
    /// such as MovieClip, SimpleButton, TextField, and Video objects, as well as to BitmapData objects.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class GlowFilter : BitmapFilter
    {
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

        public extern virtual uint color
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

        public extern virtual bool inner
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

        public extern virtual double alpha
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
        public extern GlowFilter(uint arg0, double arg1, double arg2, double arg3, double arg4, int arg5, bool arg6, bool arg7);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GlowFilter(uint arg0, double arg1, double arg2, double arg3, double arg4, int arg5, bool arg6);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GlowFilter(uint arg0, double arg1, double arg2, double arg3, double arg4, int arg5);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GlowFilter(uint arg0, double arg1, double arg2, double arg3, double arg4);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GlowFilter(uint arg0, double arg1, double arg2, double arg3);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GlowFilter(uint arg0, double arg1, double arg2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GlowFilter(uint arg0, double arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GlowFilter(uint arg0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GlowFilter();

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override BitmapFilter clone();


    }
}
