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
    [PageFX.AbcInstance(369)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class GlowFilter : flash.filters.BitmapFilter
    {
        /// <summary>
        /// The color of the glow. Valid values are in the hexadecimal format
        /// 0xRRGGBB. The default value is 0xFF0000.
        /// </summary>
        public extern virtual uint color
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
        /// The alpha transparency value for the color. Valid values are 0 to 1.
        /// For example,
        /// .25 sets a transparency value of 25%. The default value is 1.
        /// </summary>
        public extern virtual double alpha
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
        /// The amount of horizontal blur. Valid values are 0 to 255 (floating point). The
        /// default value is 6. Values that are a power of 2 (such as 2, 4,
        /// 8, 16, and 32) are optimized
        /// to render more quickly than other values.
        /// </summary>
        public extern virtual double blurX
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
        /// The amount of vertical blur. Valid values are 0 to 255 (floating point). The
        /// default value is 6. Values that are a power of 2 (such as 2, 4,
        /// 8, 16, and 32) are optimized
        /// to render more quickly than other values.
        /// </summary>
        public extern virtual double blurY
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
        /// Specifies whether the glow is an inner glow. The value true indicates
        /// an inner glow. The default is false, an outer glow (a glow
        /// around the outer edges of the object).
        /// </summary>
        public extern virtual bool inner
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
        /// Specifies whether the object has a knockout effect. A value of true
        /// makes the object&apos;s fill transparent and reveals the background color of the document. The
        /// default value is false (no knockout effect).
        /// </summary>
        public extern virtual bool knockout
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
        /// The number of times to apply the filter. The default value is BitmapFilterQuality.LOW,
        /// which is equivalent to applying the filter once. The value BitmapFilterQuality.MEDIUM
        /// applies the filter twice; the value BitmapFilterQuality.HIGH applies it three times.
        /// Filters with lower values are rendered more quickly.
        /// For most applications, a quality value of low, medium, or high is sufficient.
        /// Although you can use additional numeric values up to 15 to achieve different effects,
        /// higher values are rendered more slowly. Instead of increasing the value of quality,
        /// you can often get a similar effect, and with faster rendering, by simply increasing the values
        /// of the blurX and blurY properties.
        /// </summary>
        public extern virtual int quality
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
        /// The strength of the imprint or spread. The higher the value,
        /// the more color is imprinted and the stronger the contrast between the glow and the background.
        /// Valid values are 0 to 255. The default is 2.
        /// </summary>
        public extern virtual double strength
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

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GlowFilter(uint color, double alpha, double blurX, double blurY, double strength, int quality, bool inner, bool knockout);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GlowFilter(uint color, double alpha, double blurX, double blurY, double strength, int quality, bool inner);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GlowFilter(uint color, double alpha, double blurX, double blurY, double strength, int quality);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GlowFilter(uint color, double alpha, double blurX, double blurY, double strength);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GlowFilter(uint color, double alpha, double blurX, double blurY);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GlowFilter(uint color, double alpha, double blurX);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GlowFilter(uint color, double alpha);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GlowFilter(uint color);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GlowFilter();

        [PageFX.AbcInstanceTrait(16)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override flash.filters.BitmapFilter clone();
    }
}
