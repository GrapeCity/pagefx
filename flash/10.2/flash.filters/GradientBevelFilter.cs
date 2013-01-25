using System;
using System.Runtime.CompilerServices;

namespace flash.filters
{
    /// <summary>
    /// The GradientBevelFilter class lets you apply a gradient bevel effect to
    /// display objects. A gradient bevel is a beveled edge, enhanced with gradient color,
    /// on the outside, inside, or top of an object. Beveled edges make objects look
    /// three-dimensional.
    /// You can apply the filter to any display object (that is, objects that inherit from the DisplayObject class),
    /// such as MovieClip, SimpleButton, TextField, and Video objects, as well as to BitmapData objects.
    /// </summary>
    [PageFX.AbcInstance(297)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class GradientBevelFilter : flash.filters.BitmapFilter
    {
        /// <summary>The offset distance. Valid values are 0 to 8. The default value is 4.0.</summary>
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
        /// The angle, in degrees. Valid values are 0 to 360. The default is 45.
        /// The angle value represents the angle of the theoretical light source falling on the object.
        /// The value determines the angle at which the gradient colors are applied to the object:
        /// where the highlight and the shadow appear, or where the first color in the array appears.
        /// The colors are then applied in the order in which they appear in the array.
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
        /// An array of RGB hexadecimal color values to use in the gradient.
        /// For example, red is 0xFF0000, blue is 0x0000FF, and so on.
        /// The colors property cannot be changed by directly modifying its values.
        /// Instead, you must get a reference to colors, make the change to the
        /// reference, and then set colors to the reference.The colors, alphas, and ratios properties are related.
        /// The first element in the colors array
        /// corresponds to the first element in the alphas array
        /// and in the ratios array, and so on.
        /// </summary>
        public extern virtual Avm.Array colors
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
        /// An array of alpha transparency values for the corresponding colors in the
        /// colors array. Valid values for each element
        /// in the array are 0 to 1. For example, .25 sets a transparency value of 25%.
        /// The alphas property cannot be changed by directly modifying its values.
        /// Instead, you must get a reference to alphas, make the change to the
        /// reference, and then set alphas to the reference.The colors, alphas, and ratios properties are related.
        /// The first element in the colors array
        /// corresponds to the first element in the alphas array
        /// and in the ratios array, and so on.
        /// </summary>
        public extern virtual Avm.Array alphas
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
        /// An array of color distribution ratios for the corresponding colors in the
        /// colors array. Valid values for each element
        /// in the array are 0 to 255.
        /// The ratios property cannot be changed by directly modifying its values.
        /// Instead, you must get a reference to ratios, make the change to the
        /// reference, and then set ratios to the reference. The colors, alphas, and ratios properties are related.
        /// The first element in the colors array
        /// corresponds to the first element in the alphas array
        /// and in the ratios array, and so on. To understand how the colors in a gradient bevel are distributed, think first of the colors
        /// that you want in your gradient bevel. Consider that a simple bevel has a highlight color and shadow
        /// color; a gradient bevel has a highlight gradient and a shadow gradient. Assume that the highlight
        /// appears on the top-left corner, and the shadow appears on the bottom-right corner. Assume that one
        /// possible usage of the filter has four colors in the highlight and four in the shadow. In addition
        /// to the highlight and shadow, the filter uses a base fill color that appears where the edges of the
        /// highlight and shadow meet. Therefore the total number of colors is nine, and the corresponding number
        /// of elements in the ratios array is nine. If you think of a gradient as composed of stripes of various colors, blending into each other,
        /// each ratio value sets the position of the color on the radius of the gradient, where 0 represents
        /// the outermost point of the gradient and 255 represents the innermost point of the gradient.
        /// For a typical usage,
        /// the middle value is 128, and that is the base fill value. To get the bevel effect shown in the
        /// image below, assign the
        /// ratio values as follows, using the example of nine colors: The first four colors range from 0-127, increasing in value so that each value is greater than
        /// or equal to the previous one. This is the highlight bevel edge. The fifth color (the middle color) is the base fill, set to 128. The pixel value of 128
        /// sets the base fill, which appears either outside the shape (and around the bevel edges) if the type
        /// is set to outer; or inside the shape, effectively covering the object&apos;s own fill, if the type
        /// is set to inner. The last four colors range from 129-255, increasing in value so that each value
        /// is greater than or equal to the previous one. This is the shadow bevel edge. If you want an equal distribution of colors for each edge, use an odd number of colors,
        /// where the middle color is the base fill. Distribute the values between 0-127 and 129-255
        /// equally among your colors, then adjust the value to change the width of each stripe of color
        /// in the gradient. For a gradient bevel with nine colors, a possible array is
        /// [16, 32, 64, 96, 128, 160, 192, 224, 235]. The following image depicts the gradient bevel
        /// as described:Keep in mind that the spread of the colors in the gradient varies based on the values
        /// of the blurX, blurY, strength, and quality
        /// properties, as well as the ratios values.
        /// </summary>
        public extern virtual Avm.Array ratios
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
        /// The amount of horizontal blur. Valid values are 0 to 255. A blur of 1 or
        /// less means that the original image is copied as is. The default value
        /// is 4. Values that are a power of 2 (such as 2, 4, 8, 16 and 32) are optimized
        /// to render more quickly than other values.
        /// </summary>
        public extern virtual double blurX
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
        /// The amount of vertical blur. Valid values are 0 to 255. A blur of 1 or less
        /// means that the original image is copied as is. The default value is
        /// 4. Values that are a power of 2 (such as 2, 4, 8, 16 and 32) are optimized
        /// to render more quickly than other values.
        /// </summary>
        public extern virtual double blurY
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
        /// Specifies whether the object has a knockout effect. A knockout effect
        /// makes the object&apos;s fill transparent and reveals the background color of the document.
        /// The value true specifies a knockout effect;
        /// the default is false (no knockout effect).
        /// </summary>
        public extern virtual bool knockout
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
        /// The strength of the imprint or spread. The higher the value, the more color is imprinted
        /// and the stronger the contrast between the bevel and the background.
        /// Valid values are 0 to 255.
        /// A value of 0 means that the filter is not applied. The default value is 1.
        /// </summary>
        public extern virtual double strength
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
        /// The placement of the bevel effect. Possible values are BitmapFilterType constants:
        /// BitmapFilterType.OUTER — Bevel on the outer edge of the objectBitmapFilterType.INNER — Bevel on the inner edge of the objectBitmapFilterType.FULL — Bevel on top of the object
        /// </summary>
        public extern virtual Avm.String type
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
        public extern GradientBevelFilter(double distance, double angle, Avm.Array colors, Avm.Array alphas, Avm.Array ratios, double blurX, double blurY, double strength, int quality, Avm.String type, bool knockout);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GradientBevelFilter(double distance, double angle, Avm.Array colors, Avm.Array alphas, Avm.Array ratios, double blurX, double blurY, double strength, int quality, Avm.String type);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GradientBevelFilter(double distance, double angle, Avm.Array colors, Avm.Array alphas, Avm.Array ratios, double blurX, double blurY, double strength, int quality);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GradientBevelFilter(double distance, double angle, Avm.Array colors, Avm.Array alphas, Avm.Array ratios, double blurX, double blurY, double strength);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GradientBevelFilter(double distance, double angle, Avm.Array colors, Avm.Array alphas, Avm.Array ratios, double blurX, double blurY);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GradientBevelFilter(double distance, double angle, Avm.Array colors, Avm.Array alphas, Avm.Array ratios, double blurX);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GradientBevelFilter(double distance, double angle, Avm.Array colors, Avm.Array alphas, Avm.Array ratios);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GradientBevelFilter(double distance, double angle, Avm.Array colors, Avm.Array alphas);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GradientBevelFilter(double distance, double angle, Avm.Array colors);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GradientBevelFilter(double distance, double angle);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GradientBevelFilter(double distance);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GradientBevelFilter();

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override flash.filters.BitmapFilter clone();
    }
}
