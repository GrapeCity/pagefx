using System;
using System.Runtime.CompilerServices;

namespace flash.filters
{
    /// <summary>
    /// The DisplacementMapFilter class uses the pixel values from the specified BitmapData object
    /// (called the displacement map image ) to perform a displacement of an object.
    /// You can use this filter to apply a warped
    /// or mottled effect to any object that inherits from the DisplayObject class,
    /// such as MovieClip, SimpleButton, TextField, and Video objects, as well as to BitmapData objects.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class DisplacementMapFilter : BitmapFilter
    {
        public extern virtual uint componentY
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

        public extern virtual Avm.String mode
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

        public extern virtual flash.geom.Point mapPoint
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

        public extern virtual flash.display.BitmapData mapBitmap
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

        public extern virtual double scaleX
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

        public extern virtual double scaleY
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

        public extern virtual uint componentX
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
        public extern DisplacementMapFilter(flash.display.BitmapData arg0, flash.geom.Point arg1, uint arg2, uint arg3, double arg4, double arg5, Avm.String arg6, uint arg7, double arg8);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DisplacementMapFilter(flash.display.BitmapData arg0, flash.geom.Point arg1, uint arg2, uint arg3, double arg4, double arg5, Avm.String arg6, uint arg7);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DisplacementMapFilter(flash.display.BitmapData arg0, flash.geom.Point arg1, uint arg2, uint arg3, double arg4, double arg5, Avm.String arg6);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DisplacementMapFilter(flash.display.BitmapData arg0, flash.geom.Point arg1, uint arg2, uint arg3, double arg4, double arg5);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DisplacementMapFilter(flash.display.BitmapData arg0, flash.geom.Point arg1, uint arg2, uint arg3, double arg4);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DisplacementMapFilter(flash.display.BitmapData arg0, flash.geom.Point arg1, uint arg2, uint arg3);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DisplacementMapFilter(flash.display.BitmapData arg0, flash.geom.Point arg1, uint arg2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DisplacementMapFilter(flash.display.BitmapData arg0, flash.geom.Point arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DisplacementMapFilter(flash.display.BitmapData arg0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DisplacementMapFilter();

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override BitmapFilter clone();


    }
}
