using System;
using System.Runtime.CompilerServices;

namespace flash.text
{
    /// <summary>
    /// The FontType class contains the enumerated constants &quot;embedded&quot;
    /// and &quot;device&quot;  for the fontType  property of the Font class.
    /// </summary>
    [PageFX.AbcInstance(178)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class FontType : Avm.Object
    {
        /// <summary>
        /// Indicates that this is an embedded font.
        /// Font outlines are embedded in the published SWF file.
        /// Text fields that use embedded fonts are always displayed
        /// in the chosen font, whether or not that font is installed
        /// on the playback system. Also, text fields that use embedded fonts
        /// are always anti-aliased (smoothed) by Flash Player. You
        /// can select the amount of anti-aliasing you want by using the
        /// TextField.antiAliasType property.One drawback to embedded fonts is that they increase the size of the SWF file.
        /// </summary>
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String EMBEDDED;

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String EMBEDDED_CFF;

        /// <summary>
        /// Indicates that this is a device font.
        /// Flash Player uses the fonts installed on the system that is running the SWF file.
        /// Using device fonts results in a smaller movie size, because font data
        /// is not included in the file. Device fonts are often a good choice for
        /// displaying text at small point sizes, because anti-aliased text can be blurry
        /// at small sizes. Device fonts are also a good choice for large blocks of text,
        /// such as scrolling text.Text fields that use device fonts may not be displayed the same across different
        /// systems and platforms, because Flash Player uses the fonts that are installed on the system.
        /// For the same reason, device fonts are not anti-aliased and may appear jagged at
        /// large point sizes.
        /// </summary>
        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String DEVICE;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern FontType();
    }
}
