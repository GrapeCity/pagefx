using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    /// <summary>
    /// The InterpolationMethod class provides values for the interpolationMethod
    /// parameter in the Graphics.beginGradientFill()  and
    /// Graphics.lineGradientStyle()  methods. This parameter determines
    /// the RGB space that Flash Player uses when rendering the gradient.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class InterpolationMethod : Avm.Object
    {
        /// <summary>
        /// Specifies that the linear RGB interpolation method should be used. This means that Flash Player
        /// uses an RGB color space based on a linear RGB color model.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String LINEAR_RGB;

        /// <summary>
        /// Specifies that the RGB interpolation method should be used. This means that
        /// Flash Player uses the exponential sRGB (standard RGB) space when rendering the gradient.
        /// The sRGB space is a W3C-endorsed standard that defines a non-linear conversion between
        /// red, green, and blue component values and the actual intensity of the visible component color.
        /// For example, consider a simple linear gradient between two colors (with the spreadMethod
        /// parameter set to SpreadMethod.REFLECT). The different interpolation methods affect
        /// the appearance as follows: InterpolationMethod.LINEAR_RGBInterpolationMethod.RGB
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String RGB;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern InterpolationMethod();
    }
}
