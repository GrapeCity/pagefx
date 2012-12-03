using System;
using System.Runtime.CompilerServices;

namespace flash.filters
{
    /// <summary>
    /// The DropShadowFilter class lets you add a drop shadow to display objects.
    /// The shadow algorithm is based on the same box filter that the blur filter uses. You have
    /// several options for the style of the drop shadow, including inner or outer shadow and knockout mode.
    /// You can apply the filter to any display object (that is, objects that inherit from the DisplayObject class),
    /// such as MovieClip, SimpleButton, TextField, and Video objects, as well as to BitmapData objects.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class DropShadowFilter : BitmapFilter
    {
        public extern virtual bool hideObject
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
        public extern DropShadowFilter(double arg0, double arg1, uint arg2, double arg3, double arg4, double arg5, double arg6, int arg7, bool arg8, bool arg9, bool arg10);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DropShadowFilter(double arg0, double arg1, uint arg2, double arg3, double arg4, double arg5, double arg6, int arg7, bool arg8, bool arg9);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DropShadowFilter(double arg0, double arg1, uint arg2, double arg3, double arg4, double arg5, double arg6, int arg7, bool arg8);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DropShadowFilter(double arg0, double arg1, uint arg2, double arg3, double arg4, double arg5, double arg6, int arg7);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DropShadowFilter(double arg0, double arg1, uint arg2, double arg3, double arg4, double arg5, double arg6);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DropShadowFilter(double arg0, double arg1, uint arg2, double arg3, double arg4, double arg5);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DropShadowFilter(double arg0, double arg1, uint arg2, double arg3, double arg4);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DropShadowFilter(double arg0, double arg1, uint arg2, double arg3);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DropShadowFilter(double arg0, double arg1, uint arg2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DropShadowFilter(double arg0, double arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DropShadowFilter(double arg0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DropShadowFilter();

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override BitmapFilter clone();


    }
}
