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
    [PageFX.AbcInstance(114)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class DropShadowFilter : flash.filters.BitmapFilter
    {
        /// <summary>
        /// The offset distance for the shadow, in pixels. The default
        /// value is 4.0 (floating point).
        /// </summary>
        public extern virtual double distance
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
        /// The angle of the shadow. Valid values are 0 to 360 degrees (floating point). The
        /// default value is 45.
        /// </summary>
        public extern virtual double angle
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
        /// The color of the shadow. Valid values are in hexadecimal format 0xRRGGBB. The
        /// default value is 0x000000.
        /// </summary>
        public extern virtual uint color
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
        /// The alpha transparency value for the shadow color. Valid values are 0.0 to 1.0.
        /// For example,
        /// .25 sets a transparency value of 25%. The default value is 1.0.
        /// </summary>
        public extern virtual double alpha
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
        /// The amount of horizontal blur. Valid values are 0 to 255.0 (floating point). The
        /// default value is 4.0.
        /// </summary>
        public extern virtual double blurX
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
        /// The amount of vertical blur. Valid values are 0 to 255.0 (floating point). The
        /// default value is 4.0.
        /// </summary>
        public extern virtual double blurY
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
        /// Indicates whether or not the object is hidden. The value true
        /// indicates that the object itself is not drawn; only the shadow is visible.
        /// The default is false (the object is shown).
        /// </summary>
        public extern virtual bool hideObject
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
        /// Indicates whether or not the shadow is an inner shadow. The value true indicates
        /// an inner shadow. The default is false, an outer shadow (a
        /// shadow around the outer edges of the object).
        /// </summary>
        public extern virtual bool inner
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
        /// Applies a knockout effect (true), which effectively
        /// makes the object&apos;s fill transparent and reveals the background color of the document. The
        /// default is false (no knockout).
        /// </summary>
        public extern virtual bool knockout
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

        /// <summary>
        /// The number of times to apply the filter.
        /// The default value is BitmapFilterQuality.LOW, which is equivalent to applying
        /// the filter once. The value BitmapFilterQuality.MEDIUM applies the filter twice;
        /// the value BitmapFilterQuality.HIGH applies it three times. Filters with lower values
        /// are rendered more quickly.
        /// For most applications, a quality value of low, medium, or high is sufficient.
        /// Although you can use additional numeric values up to 15 to achieve different effects,
        /// higher values are rendered more slowly. Instead of increasing the value of quality,
        /// you can often get a similar effect, and with faster rendering, by simply increasing
        /// the values of the blurX and blurY properties.
        /// </summary>
        public extern virtual int quality
        {
            [PageFX.AbcInstanceTrait(18)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(19)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// The strength of the imprint or spread. The higher the value,
        /// the more color is imprinted and the stronger the contrast between the shadow and the background.
        /// Valid values are from 0 to 255.0. The default is 1.0.
        /// </summary>
        public extern virtual double strength
        {
            [PageFX.AbcInstanceTrait(20)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(21)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DropShadowFilter(double distance, double angle, uint color, double alpha, double blurX, double blurY, double strength, int quality, bool inner, bool knockout, bool hideObject);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DropShadowFilter(double distance, double angle, uint color, double alpha, double blurX, double blurY, double strength, int quality, bool inner, bool knockout);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DropShadowFilter(double distance, double angle, uint color, double alpha, double blurX, double blurY, double strength, int quality, bool inner);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DropShadowFilter(double distance, double angle, uint color, double alpha, double blurX, double blurY, double strength, int quality);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DropShadowFilter(double distance, double angle, uint color, double alpha, double blurX, double blurY, double strength);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DropShadowFilter(double distance, double angle, uint color, double alpha, double blurX, double blurY);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DropShadowFilter(double distance, double angle, uint color, double alpha, double blurX);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DropShadowFilter(double distance, double angle, uint color, double alpha);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DropShadowFilter(double distance, double angle, uint color);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DropShadowFilter(double distance, double angle);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DropShadowFilter(double distance);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DropShadowFilter();

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override flash.filters.BitmapFilter clone();
    }
}
