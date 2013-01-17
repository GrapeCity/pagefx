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
    [PageFX.AbcInstance(184)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class BevelFilter : flash.filters.BitmapFilter
    {
        /// <summary>The offset distance of the bevel. Valid values are in pixels (floating point). The default is 4.</summary>
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
        /// The angle of the bevel. Valid values are from 0 to 360°. The default value is 45°.
        /// The angle value represents the angle of the theoretical light source falling on the object
        /// and determines the placement of the effect relative to the object. If the distance
        /// property is set to 0, the effect is not offset from the object and, therefore,
        /// the angle property has no effect.
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
        /// The highlight color of the bevel. Valid values are in hexadecimal format,
        /// 0xRRGGBB. The default is 0xFFFFFF.
        /// </summary>
        public extern virtual uint highlightColor
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
        /// The alpha transparency value of the highlight color. The value is specified as a normalized
        /// value from 0 to 1. For example,
        /// .25 sets a transparency value of 25%. The default value is 1.
        /// </summary>
        public extern virtual double highlightAlpha
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
        /// The shadow color of the bevel. Valid values are in hexadecimal format, 0xRRGGBB. The default
        /// is 0x000000.
        /// </summary>
        public extern virtual uint shadowColor
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
        /// The alpha transparency value of the shadow color. This value is specified as a normalized
        /// value from 0 to 1. For example,
        /// .25 sets a transparency value of 25%. The default is 1.
        /// </summary>
        public extern virtual double shadowAlpha
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
        /// The amount of horizontal blur, in pixels. Valid values are from 0 to 255 (floating point).
        /// The default value is 4. Values that are a power of 2 (such as 2, 4, 8, 16, and 32) are optimized
        /// to render more quickly than other values.
        /// </summary>
        public extern virtual double blurX
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
        /// The amount of vertical blur, in pixels. Valid values are from 0 to 255 (floating point).
        /// The default value is 4. Values that are a power of 2 (such as 2, 4, 8, 16, and 32) are optimized
        /// to render more quickly than other values.
        /// </summary>
        public extern virtual double blurY
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
        /// default value is false (no knockout).
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
        /// The number of times to apply the filter. The default value is BitmapFilterQuality.LOW,
        /// which is equivalent to applying the filter once. The value BitmapFilterQuality.MEDIUM
        /// applies the filter twice; the value BitmapFilterQuality.HIGH applies it three times.
        /// Filters with lower values are rendered more quickly.
        /// For most applications, a quality value of low, medium, or high is sufficient.
        /// Although you can use additional numeric values up to 15 to achieve different effects,
        /// higher values are rendered more slowly. Instead of increasing the value of quality,
        /// you can often get a similar effect, and with faster rendering, by simply increasing the values
        /// of the blurX and blurY properties.You can use the following BitmapFilterQuality constants to specify values of the quality property:
        /// BitmapFilterQuality.LOWBitmapFilterQuality.MEDIUMBitmapFilterQuality.HIGH
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
        /// The strength of the imprint or spread. Valid values are from 0 to 255. The larger the value,
        /// the more color is imprinted and the stronger the contrast between the bevel and the background.
        /// The default value is 1.
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

        /// <summary>
        /// The placement of the bevel on the object. Inner and outer bevels are placed on
        /// the inner or outer edge; a full bevel is placed on the entire object.
        /// Valid values are the BitmapFilterType constants:
        /// BitmapFilterType.INNERBitmapFilterType.OUTERBitmapFilterType.FULL
        /// </summary>
        public extern virtual Avm.String type
        {
            [PageFX.AbcInstanceTrait(22)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(23)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern BevelFilter(double distance, double angle, uint highlightColor, double highlightAlpha, uint shadowColor, double shadowAlpha, double blurX, double blurY, double strength, int quality, Avm.String type, bool knockout);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern BevelFilter(double distance, double angle, uint highlightColor, double highlightAlpha, uint shadowColor, double shadowAlpha, double blurX, double blurY, double strength, int quality, Avm.String type);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern BevelFilter(double distance, double angle, uint highlightColor, double highlightAlpha, uint shadowColor, double shadowAlpha, double blurX, double blurY, double strength, int quality);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern BevelFilter(double distance, double angle, uint highlightColor, double highlightAlpha, uint shadowColor, double shadowAlpha, double blurX, double blurY, double strength);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern BevelFilter(double distance, double angle, uint highlightColor, double highlightAlpha, uint shadowColor, double shadowAlpha, double blurX, double blurY);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern BevelFilter(double distance, double angle, uint highlightColor, double highlightAlpha, uint shadowColor, double shadowAlpha, double blurX);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern BevelFilter(double distance, double angle, uint highlightColor, double highlightAlpha, uint shadowColor, double shadowAlpha);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern BevelFilter(double distance, double angle, uint highlightColor, double highlightAlpha, uint shadowColor);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern BevelFilter(double distance, double angle, uint highlightColor, double highlightAlpha);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern BevelFilter(double distance, double angle, uint highlightColor);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern BevelFilter(double distance, double angle);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern BevelFilter(double distance);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern BevelFilter();

        [PageFX.AbcInstanceTrait(24)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override flash.filters.BitmapFilter clone();
    }
}
