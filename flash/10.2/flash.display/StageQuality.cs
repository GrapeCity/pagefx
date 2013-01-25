using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    /// <summary>The StageQuality class provides values for the Stage.quality  property.</summary>
    [PageFX.AbcInstance(110)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class StageQuality : Avm.Object
    {
        /// <summary>Specifies low rendering quality: graphics are not anti-aliased, and bitmaps are not smoothed.</summary>
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String LOW;

        /// <summary>
        /// Specifies medium rendering quality: graphics are anti-aliased using a 2 x 2 pixel grid,
        /// but bitmaps are not smoothed. This setting is suitable for movies that do not contain text.
        /// </summary>
        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String MEDIUM;

        /// <summary>
        /// Specifies high rendering quality: graphics are anti-aliased using a 4 x 4 pixel grid,
        /// and bitmaps are smoothed if the movie is static.
        /// </summary>
        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String HIGH;

        /// <summary>
        /// Specifies very high rendering quality: graphics are anti-aliased using a 4 x 4 pixel
        /// grid and bitmaps are always smoothed.
        /// </summary>
        [PageFX.AbcClassTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String BEST;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern StageQuality();
    }
}
