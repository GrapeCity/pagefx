using System;
using System.Runtime.CompilerServices;

namespace flash.filters
{
    /// <summary>
    /// The BlurFilter class lets you apply a blur visual effect to display objects.
    /// A blur effect softens the details of an image. You can produce blurs that
    /// range from a softly unfocused look to a Gaussian blur, a hazy
    /// appearance like viewing an image through semi-opaque glass. When the quality  property
    /// of this filter is set to low, the result is a softly unfocused look.
    /// When the quality  property is set to high, it approximates a Gaussian blur
    /// filter.  You can apply the filter to any display object (that is, objects that inherit
    /// from the DisplayObject class),
    /// such as MovieClip, SimpleButton, TextField, and Video objects, as well as to BitmapData objects.
    /// </summary>
    [PageFX.AbcInstance(262)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class BlurFilter : flash.filters.BitmapFilter
    {
        /// <summary>
        /// The amount of horizontal blur. Valid values are from 0 to 255 (floating point). The
        /// default value is 4. Values that are a power of 2 (such as 2, 4, 8, 16 and 32) are optimized
        /// to render more quickly than other values.
        /// </summary>
        public extern virtual double blurX
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
        /// The amount of vertical blur. Valid values are from 0 to 255 (floating point). The
        /// default value is 4. Values that are a power of 2 (such as 2, 4, 8, 16 and 32) are optimized
        /// to render more quickly than other values.
        /// </summary>
        public extern virtual double blurY
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
        /// The number of times to perform the blur. The default value is BitmapFilterQuality.LOW,
        /// which is equivalent to applying the filter once. The value BitmapFilterQuality.MEDIUM
        /// applies the filter twice; the value BitmapFilterQuality.HIGH applies it three times
        /// and approximates a Gaussian blur. Filters with lower values are rendered more quickly.
        /// For most applications, a quality value of low, medium, or high is sufficient.
        /// Although you can use additional numeric values up to 15 to increase the number of times the blur
        /// is applied,
        /// higher values are rendered more slowly. Instead of increasing the value of quality,
        /// you can often get a similar effect, and with faster rendering, by simply increasing the values
        /// of the blurX and blurY properties.You can use the following BitmapFilterQuality constants to specify values of the
        /// quality property:BitmapFilterQuality.LOWBitmapFilterQuality.MEDIUMBitmapFilterQuality.HIGH
        /// </summary>
        public extern virtual int quality
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

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern BlurFilter(double blurX, double blurY, int quality);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern BlurFilter(double blurX, double blurY);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern BlurFilter(double blurX);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern BlurFilter();

        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override flash.filters.BitmapFilter clone();
    }
}
