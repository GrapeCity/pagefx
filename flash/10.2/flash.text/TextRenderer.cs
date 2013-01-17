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
    [PageFX.AbcInstance(357)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class TextRenderer : Avm.Object
    {
        public extern static Avm.String antiAliasType
        {
            [PageFX.AbcClassTrait(0)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcClassTrait(1)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// The adaptively sampled distance fields (ADFs) quality level for advanced anti-aliasing. The only acceptable values are
        /// 3, 4, and 7.
        /// Advanced anti-aliasing uses ADFs to
        /// represent the outlines that determine a glyph. The higher the quality, the more
        /// cache space is required for ADF structures. A value of 3 takes the least amount
        /// of memory and provides the lowest quality. Larger fonts require more cache space;
        /// at a font size of 64 pixels, the quality level increases from 3 to 4 or
        /// from 4 to 7 unless, the level is already set to 7.
        /// </summary>
        public extern static int maxLevel
        {
            [PageFX.AbcClassTrait(3)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcClassTrait(4)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// Controls the rendering of advanced anti-aliasing text. The visual quality of text is very subjective, and while
        /// Flash Player tries to use the best settings for various conditions, designers may choose a different
        /// look or feel for their text. Also, using displayMode allows a designer to override Flash
        /// Player&apos;s subpixel choice and create visual consistency independent of the user&apos;s hardware. Use the values in the TextDisplayMode class to set this property.
        /// </summary>
        public extern static Avm.String displayMode
        {
            [PageFX.AbcClassTrait(5)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcClassTrait(6)]
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
        /// <param name="fontName">The name of the font for which you are applying settings.</param>
        /// <param name="fontStyle">
        /// The font style indicated by using one of the values from
        /// the flash.text.FontStyle class.
        /// </param>
        /// <param name="colorType">
        /// This value determines whether the stroke is dark or whether it is light.
        /// Use one of the values from the flash.text.TextColorType class.
        /// </param>
        /// <param name="advancedAntiAliasingTable">
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
        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void setAdvancedAntiAliasingTable(Avm.String fontName, Avm.String fontStyle, Avm.String colorType, Avm.Array advancedAntiAliasingTable);


    }
}
