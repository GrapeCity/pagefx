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
    [PageFX.AbcInstance(139)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class ConvolutionFilter : flash.filters.BitmapFilter
    {
        /// <summary>
        /// An array of values used for matrix transformation. The number of items
        /// in the array must equal matrixX * matrixY.
        /// A matrix convolution is based on an n x m matrix, which describes how a given pixel value in the
        /// input image is combined with its neighboring pixel values to produce a resulting pixel value. Each
        /// result pixel is determined by applying the matrix to the corresponding source pixel and its
        /// neighboring pixels. For a 3 x 3 matrix convolution, the following formula is used for each independent color channel:
        /// dst (x, y) = ((src (x-1, y-1) * a0 + src(x, y-1) * a1....
        /// src(x, y+1) * a7 + src (x+1,y+1) * a8) / divisor) + bias
        /// Certain filter specifications perform faster when run by a processor
        /// that offers SSE (Streaming SIMD Extensions). The following are criteria
        /// for faster convolution operations:The filter must be a 3x3 filter.All the filter terms must be integers between -127 and +127.The sum of all the filter terms must not have an absolute value greater than 127.If any filter term is negative, the divisor must be between 2.00001 and 256.If all filter terms are positive, the divisor must be between 1.1 and 256.The bias must be an integer.
        /// </summary>
        public extern virtual Avm.Array matrix
        {
            [PageFX.AbcInstanceTrait(0)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(1)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// The x dimension of the matrix (the number of columns in the matrix). The default
        /// value is 0.
        /// </summary>
        public extern virtual double matrixX
        {
            [PageFX.AbcInstanceTrait(2)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(3)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// The y dimension of the matrix (the number of rows in the matrix). The default value
        /// is 0.
        /// </summary>
        public extern virtual double matrixY
        {
            [PageFX.AbcInstanceTrait(4)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(5)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// The divisor used during matrix transformation. The default value is 1.
        /// A divisor that is the sum of all the matrix values smooths out the overall color intensity of the
        /// result. A value of 0 is ignored and the default is used instead.
        /// </summary>
        public extern virtual double divisor
        {
            [PageFX.AbcInstanceTrait(6)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(7)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// The amount of bias to add to the result of the matrix transformation.
        /// The bias increases the color value of each channel, so that dark colors
        /// appear brighter. The default value is 0.
        /// </summary>
        public extern virtual double bias
        {
            [PageFX.AbcInstanceTrait(8)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(9)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// Indicates if the alpha channel is preserved without the filter effect
        /// or if the convolution filter is applied
        /// to the alpha channel as well as the color channels.
        /// A value of false indicates that the
        /// convolution applies to all channels, including the
        /// alpha channel. A value of true indicates that the convolution applies only to the
        /// color channels. The default value is true.
        /// </summary>
        public extern virtual bool preserveAlpha
        {
            [PageFX.AbcInstanceTrait(10)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(11)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// Indicates whether the image should be clamped. For pixels off the source image, a value of
        /// true indicates that the input
        /// image is extended along each of its borders as necessary by duplicating the color values at each
        /// respective edge of the input image. A value of false indicates that another color should
        /// be used, as specified in the color and alpha properties.
        /// The default is true.
        /// </summary>
        public extern virtual bool clamp
        {
            [PageFX.AbcInstanceTrait(12)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(13)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// The hexadecimal color to substitute for pixels that are off the source image.
        /// It is an RGB value with no alpha component. The default is 0.
        /// </summary>
        public extern virtual uint color
        {
            [PageFX.AbcInstanceTrait(14)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(15)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// The alpha transparency value of the substitute color. Valid values are 0 to 1.0. The default is 0. For example,
        /// .25 sets a transparency value of 25%.
        /// </summary>
        public extern virtual double alpha
        {
            [PageFX.AbcInstanceTrait(16)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(17)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ConvolutionFilter(double matrixX, double matrixY, Avm.Array matrix, double divisor, double bias, bool preserveAlpha, bool clamp, uint color, double alpha);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ConvolutionFilter(double matrixX, double matrixY, Avm.Array matrix, double divisor, double bias, bool preserveAlpha, bool clamp, uint color);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ConvolutionFilter(double matrixX, double matrixY, Avm.Array matrix, double divisor, double bias, bool preserveAlpha, bool clamp);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ConvolutionFilter(double matrixX, double matrixY, Avm.Array matrix, double divisor, double bias, bool preserveAlpha);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ConvolutionFilter(double matrixX, double matrixY, Avm.Array matrix, double divisor, double bias);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ConvolutionFilter(double matrixX, double matrixY, Avm.Array matrix, double divisor);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ConvolutionFilter(double matrixX, double matrixY, Avm.Array matrix);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ConvolutionFilter(double matrixX, double matrixY);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ConvolutionFilter(double matrixX);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ConvolutionFilter();

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override flash.filters.BitmapFilter clone();
    }
}
