using System;
using System.Runtime.CompilerServices;

namespace flash.geom
{
    /// <summary>
    /// The ColorTransform class lets you adjust the color values in a display object.
    /// The color adjustment or color transformation  can be applied to all four channels:
    /// red, green, blue, and alpha transparency.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class ColorTransform : Avm.Object
    {
        /// <summary>
        /// A number from -255 to 255 that is added to the red channel value after it has been
        /// multiplied by the redMultiplier value.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public double redOffset;

        /// <summary>A decimal value that is multiplied with the green channel value.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public double greenMultiplier;

        /// <summary>
        /// A number from -255 to 255 that is added to the blue channel value after it has
        /// been multiplied by the blueMultiplier value.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public double blueOffset;

        /// <summary>
        /// A number from -255 to 255 that is added to the alpha transparency channel value after it has
        /// been multiplied by the alphaMultiplier value.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public double alphaOffset;

        /// <summary>A decimal value that is multiplied with the red channel value.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public double redMultiplier;

        /// <summary>A decimal value that is multiplied with the blue channel value.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public double blueMultiplier;

        /// <summary>
        /// A number from -255 to 255 that is added to the green channel value after it has
        /// been multiplied by the greenMultiplier value.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public double greenOffset;

        /// <summary>
        /// A decimal value that is multiplied with the alpha transparency channel value.
        /// If you set the alpha transparency value of a display object directly by using the
        /// alpha property of the DisplayObject instance, it affects the value of the
        /// alphaMultiplier property of that display object&apos;s transform.colorTransform
        /// property.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public double alphaMultiplier;

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

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ColorTransform(double arg0, double arg1, double arg2, double arg3, double arg4, double arg5, double arg6, double arg7);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ColorTransform(double arg0, double arg1, double arg2, double arg3, double arg4, double arg5, double arg6);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ColorTransform(double arg0, double arg1, double arg2, double arg3, double arg4, double arg5);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ColorTransform(double arg0, double arg1, double arg2, double arg3, double arg4);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ColorTransform(double arg0, double arg1, double arg2, double arg3);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ColorTransform(double arg0, double arg1, double arg2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ColorTransform(double arg0, double arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ColorTransform(double arg0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ColorTransform();

        /// <summary>
        /// Formats and returns a string that describes all of the properties of the
        /// ColorTransform object.
        /// </summary>
        /// <returns>A string that lists all of the properties of the ColorTransform object.</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String toString();

        /// <summary>
        /// Concatenates the ColorTranform object specified by the second parameter
        /// with the current ColorTransform object and sets the
        /// current object as the result, which is an additive combination of the two color transformations.
        /// When you apply the concatenated ColorTransform object, the effect is the same as applying the
        /// second color transformation after the original color transformation.
        /// </summary>
        /// <param name="arg0">The ColorTransform object to be combined with the current ColorTransform object.</param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void concat(ColorTransform arg0);
    }
}
