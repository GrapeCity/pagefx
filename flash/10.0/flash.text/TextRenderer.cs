using System;
using System.Runtime.CompilerServices;

namespace flash.text
{
    /// <summary>
    /// The TextRenderer class provides functionality for the advanced anti-aliasing capability of
    /// embedded fonts. Advanced anti-aliasing allows font faces to render at very high quality at
    /// small sizes. Use advanced anti-aliasing with applications that have a lot of small text. Adobe does not recommend using advanced
    /// anti-aliasing for very large fonts (larger than 48 points).
    /// Advanced anti-aliasing is available in Flash Player 8 and later only.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class TextRenderer : Avm.Object
    {
        public extern static int maxLevel
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

        public extern static Avm.String displayMode
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

        public extern static Avm.String antiAliasType
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
        public extern TextRenderer();

        /// <summary>
        /// Sets a custom continuous stroke modulation (CSM) lookup table for a font.
        /// Flash Player attempts to detect the best CSM for your font. If you are not
        /// satisfied with the CSM that the Flash Player provides, you can customize
        /// your own CSM by using the setAdvancedAntiAliasingTable() method.
        /// </summary>
        /// <param name="arg0">The name of the font for which you are applying settings.</param>
        /// <param name="arg1">
        /// The font style indicated by using one of the values from
        /// the flash.text.FontStyle class.
        /// </param>
        /// <param name="arg2">
        /// This value determines whether the stroke is dark or whether it is light.
        /// Use one of the values from the flash.text.TextColorType class.
        /// </param>
        /// <param name="arg3">
        /// An array of one or more CSMSettings objects
        /// for the specified font. Each object contains the following properties:
        /// fontSizeinsideCutOffoutsideCutOffThe advancedAntiAliasingTable array can contain multiple entries
        /// that specify CSM settings for different font sizes.The fontSize is the size, in pixels, for which the settings apply.Advanced anti-aliasing uses adaptively sampled distance fields (ADFs) to
        /// represent the outlines that determine a glyph. Flash Player uses an outside cutoff value
        /// (outsideCutOff),
        /// below which densities are set to zero, and an inside cutoff value (insideCutOff),
        /// above which densities
        /// are set to a maximum density value (such as 255). Between these two cutoff values,
        /// the mapping function is a linear curve ranging from zero at the outside cutoff
        /// to the maximum density at the inside cutoff.Adjusting the outside and inside cutoff values affects stroke weight and
        /// edge sharpness. The spacing between these two parameters is comparable to twice the
        /// filter radius of classic anti-aliasing methods; a narrow spacing provides a sharper edge,
        /// while a wider spacing provides a softer, more filtered edge. When
        /// the spacing is zero, the resulting density image is a bi-level bitmap. When the
        /// spacing is very wide, the resulting density image has a watercolor-like edge.Typically, users prefer sharp, high-contrast edges at small point sizes, and
        /// softer edges for animated text and larger point sizes. The outside cutoff typically has a negative value, and the inside cutoff typically
        /// has a positive value, and their midpoint typically lies near zero. Adjusting these
        /// parameters to shift the midpoint toward negative infinity increases the stroke
        /// weight; shifting the midpoint toward positive infinity decreases the stroke weight.
        /// Make sure that the outside cutoff value is always less than or equal to the inside cutoff value.
        /// </param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void setAdvancedAntiAliasingTable(Avm.String arg0, Avm.String arg1, Avm.String arg2, Avm.Array arg3);


    }
}
