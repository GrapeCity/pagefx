using System;
using System.Runtime.CompilerServices;

namespace flash.text
{
    /// <summary>The TextDisplayMode class contains values that control the subpixel anti-aliasing of the advanced anti-aliasing system.</summary>
    [PageFX.AbcInstance(199)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class TextDisplayMode : Avm.Object
    {
        /// <summary>
        /// Forces Flash Player to use LCD subpixel anti-aliasing. Depending on the font and
        /// the hardware, this setting can result in much higher resolution text or text coloring.
        /// </summary>
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String LCD;

        /// <summary>
        /// Forces Flash Player to display grayscale anti-aliasing. While this setting
        /// avoids text coloring, some users may think it appears blurry.
        /// </summary>
        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String CRT;

        /// <summary>Allows Flash Player to choose LCD or CRT mode.</summary>
        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String DEFAULT;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextDisplayMode();
    }
}
