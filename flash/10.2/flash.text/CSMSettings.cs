using System;
using System.Runtime.CompilerServices;

namespace flash.text
{
    /// <summary>
    /// The CSMSettings class contains properties for use with the
    /// TextRenderer.setAdvancedAntiAliasingTable()  method
    /// to provide continuous stroke modulation (CSM). CSM is the continuous
    /// modulation of both stroke weight and edge sharpness.
    /// </summary>
    [PageFX.AbcInstance(224)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class CSMSettings : Avm.Object
    {
        /// <summary>
        /// The size, in pixels, for which the settings apply.
        /// The advancedAntiAliasingTable array passed to the
        /// setAdvancedAntiAliasingTable() method can contain multiple
        /// entries that specify CSM settings for different font sizes. Using this
        /// property, you can specify the font size to which the other settings apply.
        /// </summary>
        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public double fontSize;

        /// <summary>
        /// The inside cutoff value, above which densities are set to a maximum density
        /// value (such as 255).
        /// </summary>
        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        public double insideCutoff;

        /// <summary>The outside cutoff value, below which densities are set to zero.</summary>
        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        public double outsideCutoff;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern CSMSettings(double fontSize, double insideCutoff, double outsideCutoff);
    }
}
