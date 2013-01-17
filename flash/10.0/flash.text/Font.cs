using System;
using System.Runtime.CompilerServices;

namespace flash.text
{
    /// <summary>
    /// The Font class is used to manage embedded fonts in SWF files. Embedded fonts
    /// are represented as a subclass of the Font class. The Font class is currently useful only to
    /// find out information about embedded fonts; you cannot alter a font by
    /// using this class.
    /// You cannot use the Font class to load external fonts, or to create an instance
    /// of a Font object by itself. Use the Font class as an abstract base class.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class Font : Avm.Object
    {
        public extern virtual Avm.String fontType
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual Avm.String fontStyle
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual Avm.String fontName
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Font();

        /// <summary>Specifies whether a provided string can be displayed using the currently assigned font.</summary>
        /// <param name="arg0">The string to test against the current font.</param>
        /// <returns>A value of true if the specified string can be fully displayed using this font.</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool hasGlyphs(Avm.String arg0);

        /// <summary>Specifies whether to provide a list of the currently available embedded fonts.</summary>
        /// <param name="arg0">
        /// (default = false)  Indicates whether you want to limit the list to only the currently available embedded fonts.
        /// If this is set to true then a list of all fonts, both device fonts and embedded fonts, is returned.
        /// If this is set to false then only a list of embedded fonts is returned.
        /// </param>
        /// <returns>A list of available fonts as an array of Font objects.</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static Avm.Array enumerateFonts(bool arg0);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static Avm.Array enumerateFonts();

        /// <summary>Registers a font class in the global font list.</summary>
        /// <param name="arg0">The class you want to add to the global font list.</param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void registerFont(Avm.Class arg0);
    }
}
