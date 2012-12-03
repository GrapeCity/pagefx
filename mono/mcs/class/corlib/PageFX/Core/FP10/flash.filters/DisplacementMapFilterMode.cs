using System;
using System.Runtime.CompilerServices;

namespace flash.filters
{
    /// <summary>
    /// The DisplacementMapFilterMode class provides values for the mode  property
    /// of the DisplacementMapFilter class.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class DisplacementMapFilterMode : Avm.Object
    {
        /// <summary>
        /// If the displacement value is outside the image, substitutes the values in
        /// the color and alpha properties.
        /// Use with the DisplacementMapFilter.mode property.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String COLOR;

        /// <summary>
        /// If the displacement value is out of range, ignores the displacement and uses the source pixel.
        /// Use with the DisplacementMapFilter.mode property.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String IGNORE;

        /// <summary>
        /// Wraps the displacement value to the other side of the source image.
        /// Use with the DisplacementMapFilter.mode property.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String WRAP;

        /// <summary>
        /// Clamps the displacement value to the edge of the source image.
        /// Use with the DisplacementMapFilter.mode property.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String CLAMP;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DisplacementMapFilterMode();
    }
}
