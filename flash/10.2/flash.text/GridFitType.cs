using System;
using System.Runtime.CompilerServices;

namespace flash.text
{
    /// <summary>The GridFitType class defines values for grid fitting in the TextField class.</summary>
    [PageFX.AbcInstance(96)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class GridFitType : Avm.Object
    {
        /// <summary>
        /// Doesn&apos;t set grid fitting. Horizontal and vertical lines
        /// in the glyphs are not forced to the pixel grid.
        /// This constant is used in setting the gridFitType property of the
        /// TextField class. This is often a good setting for animation
        /// or for large font sizes.
        /// Use the syntax GridFitType.NONE.
        /// </summary>
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String NONE;

        /// <summary>
        /// Fits strong horizontal and vertical lines to the pixel grid.
        /// This constant is used in setting the gridFitType property of the
        /// TextField class. This setting only works for left-justified text
        /// fields and acts like the GridFitType.SUBPIXEL constant in static
        /// text. This setting generally provides the best readability for left-aligned text.
        /// Use the syntax GridFitType.PIXEL.
        /// </summary>
        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String PIXEL;

        /// <summary>
        /// Fits strong horizontal and vertical lines to the sub-pixel
        /// grid on LCD monitors. (Red, green, and blue are actual pixels on an LCD screen.)
        /// This is often a good setting for right-aligned or center-aligned dynamic
        /// text, and it is sometimes a useful tradeoff for animation vs. text quality.
        /// This constant is used in setting the gridFitType property of the
        /// TextField class.
        /// Use the syntax GridFitType.SUBPIXEL.
        /// </summary>
        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String SUBPIXEL;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GridFitType();
    }
}
