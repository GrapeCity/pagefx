using System;
using System.Runtime.CompilerServices;

namespace flash.filters
{
    /// <summary>
    /// The GradientGlowFilter class lets you apply a gradient glow effect to display objects.
    /// A gradient glow is a realistic-looking glow with a color gradient that
    /// you can control. You can apply a gradient glow around
    /// the inner or outer edge of an object or on top of an object.
    /// You can apply the filter to any display object (objects that inherit from the DisplayObject class),
    /// such as MovieClip, SimpleButton, TextField, and Video objects, as well as to BitmapData objects.
    /// </summary>
    [PageFX.AbcInstance(118)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class GradientGlowFilter : flash.filters.BitmapFilter
    {
        /// <summary>
        /// The angle, in degrees. Valid values are 0 to 360. The default is 45.
        /// The angle value represents the angle of the theoretical light source falling on the object and
        /// determines the placement of the effect relative to the object. If distance is set to 0, the effect
        /// is not offset from the object, and therefore the angle property has no effect.
        /// </summary>
        public extern virtual double angle
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
        /// An array of alpha transparency values for the corresponding colors in
        /// the colors array. Valid values for each element in the array are 0 to 1.
        /// For example, .25 sets the alpha transparency value to 25%.
        /// The alphas property cannot be changed by directly modifying its values.
        /// Instead, you must get a reference to alphas, make the change to the
        /// reference, and then set alphas to the reference.The colors, alphas, and ratios properties are related.
        /// The first element in the colors array
        /// corresponds to the first element in the alphas array
        /// and in the ratios array, and so on.
        /// </summary>
        public extern virtual Avm.Array alphas
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
        /// The amount of horizontal blur. Valid values are 0 to 255. A blur of 1 or
        /// less means that the original image is copied as is. The default value
        /// is 4. Values that are a power of 2 (such as 2, 4, 8, 16, and 32) are optimized
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
        /// The amount of vertical blur. Valid values are 0 to 255. A blur of 1 or less
        /// means that the original image is copied as is. The default value is
        /// 4. Values that are a power of 2 (such as 2, 4, 8, 16, and 32) are optimized
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
        /// An array of colors that defines a gradient.
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

        /// <summary>The offset distance of the glow. The default value is 4.</summary>
        public extern virtual double distance
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
        /// Specifies whether the object has a knockout effect. A knockout effect
        /// makes the object&apos;s fill transparent and reveals the background color of the document.
        /// The value true specifies a knockout effect;
        /// the default value is false (no knockout effect).
        /// </summary>
        public extern virtual bool knockout
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
        /// An array of color distribution ratios for the corresponding colors in the
        /// colors array. Valid values are
        /// 0 to 255.
        /// The ratios property cannot be changed by directly modifying its values.
        /// Instead, you must get a reference to ratios, make the change to the
        /// reference, and then set ratios to the reference.The colors, alphas, and ratios properties are related.
        /// The first element in the colors array
        /// corresponds to the first element in the alphas array
        /// and in the ratios array, and so on.Think of the gradient glow filter as a glow that emanates from
        /// the center of the object (if the distance value is set to 0),
        /// with gradients that are stripes of color blending into each other. The first color
        /// in the colors array is the outermost color of the glow.
        /// The last color is the innermost color of the glow.Each value in the ratios array sets
        /// the position of the color on the radius of the gradient, where 0 represents
        /// the outermost point of the gradient and 255 represents the innermost point of
        /// the gradient. The ratio values can range from 0 to 255 pixels,
        /// in increasing value; for example [0, 64, 128, 200, 255]. Values from 0 to 128
        /// appear on the outer edges of the glow. Values from 129 to 255 appear in the inner
        /// area of the glow. Depending on the ratio values of the colors and the type
        /// value of the filter, the filter colors might be obscured by the object to which
        /// the filter is applied.In the following code and image, a filter is applied to a black circle movie
        /// clip, with the type set to &quot;full&quot;. For instructional purposes, the first color
        /// in the colors array, pink, has an alpha value of 1,
        /// so it shows against the white document background. (In practice, you probably would
        /// not want the first color showing in this way.) The last color in the
        /// array, yellow, obscures the black circle to which the filter is applied:
        /// var colors:Array = [0xFFCCFF, 0x0000FF, 0x9900FF, 0xFF0000, 0xFFFF00];
        /// var alphas:Array = [1, 1, 1, 1, 1];
        /// var ratios:Array = [0, 32, 64, 128, 225];
        /// var myGGF:GradientGlowFilter = new GradientGlowFilter(0, 0, colors, alphas, ratios, 50, 50, 1, 2, &quot;full&quot;, false);
        /// To achieve a seamless effect with your document background when you set the type
        /// value to &quot;outer&quot; or &quot;full&quot;, set the first color in the
        /// array to the same color as the document background, or set the
        /// alpha value of the first color to 0; either technique makes the filter blend in with the background.If you make two small changes in the code, the effect of the glow can be very
        /// different, even with the same ratios and colors arrays. Set
        /// the alpha value of the first
        /// color in the array to 0, to make the filter blend in with the document&apos;s
        /// white background; and set the type property to
        /// &quot;outer&quot; or &quot;inner&quot;.
        /// Observe the results, as shown in the following images.Keep in mind that the spread of the colors in the gradient varies based on the values
        /// of the blurX, blurY, strength, and quality
        /// properties, as well as the ratios values.
        /// </summary>
        public extern virtual Avm.Array ratios
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
        /// and the stronger the contrast between the glow and the background. Valid values are 0 to 255.
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
        /// The placement of the filter effect. Possible values are flash.filters.BitmapFilterType constants:
        /// BitmapFilterType.OUTER — Glow on the outer edge of the objectBitmapFilterType.INNER — Glow on the inner edge of the object; the default.BitmapFilterType.FULL — Glow on top of the object
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
        public extern GradientGlowFilter(double distance, double angle, Avm.Array colors, Avm.Array alphas, Avm.Array ratios, double blurX, double blurY, double strength, int quality, Avm.String type, bool knockout);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GradientGlowFilter(double distance, double angle, Avm.Array colors, Avm.Array alphas, Avm.Array ratios, double blurX, double blurY, double strength, int quality, Avm.String type);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GradientGlowFilter(double distance, double angle, Avm.Array colors, Avm.Array alphas, Avm.Array ratios, double blurX, double blurY, double strength, int quality);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GradientGlowFilter(double distance, double angle, Avm.Array colors, Avm.Array alphas, Avm.Array ratios, double blurX, double blurY, double strength);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GradientGlowFilter(double distance, double angle, Avm.Array colors, Avm.Array alphas, Avm.Array ratios, double blurX, double blurY);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GradientGlowFilter(double distance, double angle, Avm.Array colors, Avm.Array alphas, Avm.Array ratios, double blurX);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GradientGlowFilter(double distance, double angle, Avm.Array colors, Avm.Array alphas, Avm.Array ratios);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GradientGlowFilter(double distance, double angle, Avm.Array colors, Avm.Array alphas);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GradientGlowFilter(double distance, double angle, Avm.Array colors);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GradientGlowFilter(double distance, double angle);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GradientGlowFilter(double distance);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GradientGlowFilter();

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override flash.filters.BitmapFilter clone();
    }
}
