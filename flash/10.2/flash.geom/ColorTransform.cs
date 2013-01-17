using System;
using System.Runtime.CompilerServices;

namespace flash.geom
{
    /// <summary>
    /// The ColorTransform class lets you adjust the color values in a display object.
    /// The color adjustment or color transformation  can be applied to all four channels:
    /// red, green, blue, and alpha transparency.
    /// </summary>
    [PageFX.AbcInstance(360)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class ColorTransform : Avm.Object
    {
        /// <summary>A decimal value that is multiplied with the red channel value.</summary>
        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public double redMultiplier;

        /// <summary>A decimal value that is multiplied with the green channel value.</summary>
        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        public double greenMultiplier;

        /// <summary>A decimal value that is multiplied with the blue channel value.</summary>
        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        public double blueMultiplier;

        /// <summary>
        /// A decimal value that is multiplied with the alpha transparency channel value.
        /// If you set the alpha transparency value of a display object directly by using the
        /// alpha property of the DisplayObject instance, it affects the value of the
        /// alphaMultiplier property of that display object&apos;s transform.colorTransform
        /// property.
        /// </summary>
        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        public double alphaMultiplier;

        /// <summary>
        /// A number from -255 to 255 that is added to the red channel value after it has been
        /// multiplied by the redMultiplier value.
        /// </summary>
        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.FP9]
        public double redOffset;

        /// <summary>
        /// A number from -255 to 255 that is added to the green channel value after it has
        /// been multiplied by the greenMultiplier value.
        /// </summary>
        [PageFX.AbcInstanceTrait(5)]
        [PageFX.ABC]
        [PageFX.FP9]
        public double greenOffset;

        /// <summary>
        /// A number from -255 to 255 that is added to the blue channel value after it has
        /// been multiplied by the blueMultiplier value.
        /// </summary>
        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.FP9]
        public double blueOffset;

        /// <summary>
        /// A number from -255 to 255 that is added to the alpha transparency channel value after it has
        /// been multiplied by the alphaMultiplier value.
        /// </summary>
        [PageFX.AbcInstanceTrait(7)]
        [PageFX.ABC]
        [PageFX.FP9]
        public double alphaOffset;

        /// <summary>
        /// The RGB color value for a ColorTransform object.
        /// When you set this property, it changes the three color offset values (redOffset,
        /// greenOffset, and blueOffset)
        /// accordingly, and it sets the three color multiplier values (redMultiplier,
        /// greenMultiplier, and blueMultiplier) to 0.
        /// The alpha transparency multiplier and offset values do not change.When you pass a value for this property, use the format 0xRRGGBB.
        /// RR, GG, and BB each consist
        /// of two hexadecimal digits that specify the offset of each color component. The 0x
        /// tells the ActionScript compiler that the number is a hexadecimal value.
        /// </summary>
        public extern virtual uint color
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

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ColorTransform(double redMultiplier, double greenMultiplier, double blueMultiplier, double alphaMultiplier, double redOffset, double greenOffset, double blueOffset, double alphaOffset);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ColorTransform(double redMultiplier, double greenMultiplier, double blueMultiplier, double alphaMultiplier, double redOffset, double greenOffset, double blueOffset);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ColorTransform(double redMultiplier, double greenMultiplier, double blueMultiplier, double alphaMultiplier, double redOffset, double greenOffset);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ColorTransform(double redMultiplier, double greenMultiplier, double blueMultiplier, double alphaMultiplier, double redOffset);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ColorTransform(double redMultiplier, double greenMultiplier, double blueMultiplier, double alphaMultiplier);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ColorTransform(double redMultiplier, double greenMultiplier, double blueMultiplier);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ColorTransform(double redMultiplier, double greenMultiplier);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ColorTransform(double redMultiplier);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ColorTransform();

        /// <summary>
        /// Concatenates the ColorTranform object specified by the second parameter
        /// with the current ColorTransform object and sets the
        /// current object as the result, which is an additive combination of the two color transformations.
        /// When you apply the concatenated ColorTransform object, the effect is the same as applying the
        /// second color transformation after the original color transformation.
        /// </summary>
        /// <param name="second">The ColorTransform object to be combined with the current ColorTransform object.</param>
        [PageFX.AbcInstanceTrait(10)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void concat(flash.geom.ColorTransform second);

        /// <summary>
        /// Formats and returns a string that describes all of the properties of the
        /// ColorTransform object.
        /// </summary>
        /// <returns>A string that lists all of the properties of the ColorTransform object.</returns>
        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String toString();
    }
}
