using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    /// <summary>The StageScaleMode class provides values for the Stage.scaleMode  property.</summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class StageScaleMode : Avm.Object
    {
        /// <summary>
        /// Specifies that the entire Adobe® Flash® application be visible in the specified area without trying to preserve
        /// the original aspect ratio. Distortion can occur.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String EXACT_FIT;

        /// <summary>
        /// Specifies that the entire Flash application fill the specified area, without distortion but possibly with
        /// some cropping, while maintaining the original aspect ratio of the application.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String NO_BORDER;

        /// <summary>
        /// Specifies that the size of the Flash application be fixed, so that it remains unchanged even as the size
        /// of the player window changes. Cropping might occur if the player window is smaller than the content.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String NO_SCALE;

        /// <summary>
        /// Specifies that the entire Flash application be visible in the specified area without distortion while
        /// maintaining the original aspect ratio of the application. Borders can appear on two sides of the application.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String SHOW_ALL;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern StageScaleMode();
    }
}
