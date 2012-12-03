using System;
using System.Runtime.CompilerServices;

namespace flash.filters
{
    /// <summary>
    /// The ConvolutionFilter class applies a matrix convolution filter effect. A convolution combines pixels
    /// in the input image with neighboring pixels to produce an image. A wide variety of image
    /// effects can be achieved through convolutions, including blurring, edge detection, sharpening,
    /// embossing, and beveling. You can apply the filter to any display object (that is, objects that
    /// inherit from the DisplayObject class),
    /// such as MovieClip, SimpleButton, TextField, and Video objects, as well as to BitmapData objects.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class ConvolutionFilter : BitmapFilter
    {
        public extern virtual Avm.Array matrix
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

        public extern virtual bool preserveAlpha
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

        public extern virtual double bias
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

        public extern virtual double matrixX
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

        public extern virtual double matrixY
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

        public extern virtual bool clamp
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

        public extern virtual double divisor
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
        public extern ConvolutionFilter(double arg0, double arg1, Avm.Array arg2, double arg3, double arg4, bool arg5, bool arg6, uint arg7, double arg8);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ConvolutionFilter(double arg0, double arg1, Avm.Array arg2, double arg3, double arg4, bool arg5, bool arg6, uint arg7);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ConvolutionFilter(double arg0, double arg1, Avm.Array arg2, double arg3, double arg4, bool arg5, bool arg6);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ConvolutionFilter(double arg0, double arg1, Avm.Array arg2, double arg3, double arg4, bool arg5);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ConvolutionFilter(double arg0, double arg1, Avm.Array arg2, double arg3, double arg4);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ConvolutionFilter(double arg0, double arg1, Avm.Array arg2, double arg3);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ConvolutionFilter(double arg0, double arg1, Avm.Array arg2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ConvolutionFilter(double arg0, double arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ConvolutionFilter(double arg0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ConvolutionFilter();

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override BitmapFilter clone();


    }
}
