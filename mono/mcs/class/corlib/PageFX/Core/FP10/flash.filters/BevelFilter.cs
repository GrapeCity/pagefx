using System;
using System.Runtime.CompilerServices;

namespace flash.filters
{
    /// <summary>
    /// The BevelFilter class lets you add a bevel effect to display objects.
    /// A bevel effect gives objects such as buttons a three-dimensional look. You can customize
    /// the look of the bevel with different highlight and shadow colors, the amount
    /// of blur on the bevel, the angle of the bevel, the placement of the bevel,
    /// and a knockout effect.
    /// You can apply the filter to any display object (that is, objects that inherit from the
    /// DisplayObject class), such as MovieClip, SimpleButton, TextField, and Video objects,
    /// as well as to BitmapData objects.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class BevelFilter : BitmapFilter
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

        public extern virtual uint shadowColor
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

        public extern virtual double highlightAlpha
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

        public extern virtual uint highlightColor
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

        public extern virtual double shadowAlpha
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
        public extern BevelFilter(double arg0, double arg1, uint arg2, double arg3, uint arg4, double arg5, double arg6, double arg7, double arg8, int arg9, Avm.String arg10, bool arg11);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern BevelFilter(double arg0, double arg1, uint arg2, double arg3, uint arg4, double arg5, double arg6, double arg7, double arg8, int arg9, Avm.String arg10);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern BevelFilter(double arg0, double arg1, uint arg2, double arg3, uint arg4, double arg5, double arg6, double arg7, double arg8, int arg9);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern BevelFilter(double arg0, double arg1, uint arg2, double arg3, uint arg4, double arg5, double arg6, double arg7, double arg8);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern BevelFilter(double arg0, double arg1, uint arg2, double arg3, uint arg4, double arg5, double arg6, double arg7);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern BevelFilter(double arg0, double arg1, uint arg2, double arg3, uint arg4, double arg5, double arg6);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern BevelFilter(double arg0, double arg1, uint arg2, double arg3, uint arg4, double arg5);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern BevelFilter(double arg0, double arg1, uint arg2, double arg3, uint arg4);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern BevelFilter(double arg0, double arg1, uint arg2, double arg3);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern BevelFilter(double arg0, double arg1, uint arg2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern BevelFilter(double arg0, double arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern BevelFilter(double arg0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern BevelFilter();

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override BitmapFilter clone();


    }
}
