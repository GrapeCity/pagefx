using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    /// <summary>The StageQuality class provides values for the Stage.quality  property.</summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class StageQuality : Avm.Object
    {
        /// <summary>
        /// Specifies very high rendering quality: graphics are anti-aliased using a 4 x 4 pixel
        /// grid and bitmaps are always smoothed.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String BEST;

        /// <summary>Specifies low rendering quality: graphics are not anti-aliased, and bitmaps are not smoothed.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String LOW;

        /// <summary>
        /// Specifies medium rendering quality: graphics are anti-aliased using a 2 x 2 pixel grid,
        /// but bitmaps are not smoothed. This setting is suitable for movies that do not contain text.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String MEDIUM;

        /// <summary>
        /// Specifies high rendering quality: graphics are anti-aliased using a 4 x 4 pixel grid,
        /// and bitmaps are smoothed if the movie is static.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String HIGH;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern StageQuality();
    }
}
