using System;
using System.Runtime.CompilerServices;

namespace flash.text
{
    /// <summary>
    /// The TextFormat class represents character formatting information.  Use the TextFormat class
    /// to create specific text formatting for text fields. You can apply text formatting
    /// to both static and dynamic text fields. The properties of the TextFormat class apply to device and
    /// embedded fonts. However, for embedded fonts, bold and italic text actually require specific fonts. If you
    /// want to display bold or italic text with an embedded font, you need to embed the bold and italic variations
    /// of that font.
    /// </summary>
    [PageFX.AbcInstance(194)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class TextFormat : Avm.Object
    {
        /// <summary>Indicates the alignment of the paragraph. Valid values are TextFormatAlign constants.</summary>
        public extern virtual Avm.String align
        {
            [PageFX.AbcInstanceTrait(0)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(1)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// Indicates the block indentation in pixels. Block indentation is applied to
        /// an entire block of text; that is, to all lines of the text. In contrast, normal indentation
        /// (TextFormat.indent) affects only the first line of each paragraph.
        /// If this property is null, the TextFormat object does not specify block indentation
        /// (block indentation is 0).
        /// </summary>
        public extern virtual object blockIndent
        {
            [PageFX.AbcInstanceTrait(2)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(3)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// Specifies whether the text is boldface. The default value is null,
        /// which means no boldface is used.
        /// If the value is true, then
        /// the text is boldface.
        /// </summary>
        public extern virtual object bold
        {
            [PageFX.AbcInstanceTrait(4)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(5)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// Indicates that the text is part of a bulleted list. In a bulleted
        /// list, each paragraph of text is indented. To the left of the first line of each paragraph, a bullet
        /// symbol is displayed. The default value is null, which means no bulleted list
        /// is used.
        /// </summary>
        public extern virtual object bullet
        {
            [PageFX.AbcInstanceTrait(6)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(7)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// Indicates the color of the text. A number containing three 8-bit RGB components; for example,
        /// 0xFF0000 is red, and 0x00FF00 is green. The default value is null,
        /// which means that Flash Player uses the color black (0x000000).
        /// </summary>
        public extern virtual object color
        {
            [PageFX.AbcInstanceTrait(8)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(9)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual Avm.String display
        {
            [PageFX.AbcInstanceTrait(10)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(11)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// The name of the font for text in this text format, as a string. The default value is
        /// null, which means that Flash Player uses Times New Roman font for the text.
        /// </summary>
        public extern virtual Avm.String font
        {
            [PageFX.AbcInstanceTrait(12)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(13)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// Indicates the indentation from the left
        /// margin to the first character in the paragraph. The default value is null, which
        /// indicates that no indentation is used.
        /// </summary>
        public extern virtual object indent
        {
            [PageFX.AbcInstanceTrait(14)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(15)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// Indicates whether text in this text format is italicized. The default
        /// value is null, which means no italics are used.
        /// </summary>
        public extern virtual object italic
        {
            [PageFX.AbcInstanceTrait(16)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(17)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// A Boolean value that indicates whether kerning is enabled (true)
        /// or disabled (false). Kerning adjusts the pixels between certain character pairs to improve readability, and
        /// should be used only when necessary, such as with headings in large fonts. Kerning is
        /// supported for embedded fonts only.
        /// Certain fonts such as Verdana and monospaced fonts,
        /// such as Courier New,  do not support kerning.
        /// The default value is null, which means that kerning is not enabled.
        /// </summary>
        public extern virtual object kerning
        {
            [PageFX.AbcInstanceTrait(18)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(19)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// An integer representing the amount of vertical space (called leading)
        /// between lines. The default value is null, which indicates that the
        /// amount of leading used is 0.
        /// </summary>
        public extern virtual object leading
        {
            [PageFX.AbcInstanceTrait(20)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(21)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// The left margin of the paragraph, in pixels. The default value is null, which
        /// indicates that the left margin is 0 pixels.
        /// </summary>
        public extern virtual object leftMargin
        {
            [PageFX.AbcInstanceTrait(22)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(23)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// An integer representing the amount of space that is uniformly distributed between all characters.
        /// The value specifies the number of pixels that are added to the advance after each character.
        /// The default value is null, which means that 0 pixels of letter spacing is used.
        /// </summary>
        public extern virtual object letterSpacing
        {
            [PageFX.AbcInstanceTrait(24)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(25)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// The right margin of the paragraph, in pixels. The default value is null,
        /// which indicates that the right margin is 0 pixels.
        /// </summary>
        public extern virtual object rightMargin
        {
            [PageFX.AbcInstanceTrait(26)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(27)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// The point size of text in this text format. The default value is null, which
        /// means that a point size of 12 is used.
        /// </summary>
        public extern virtual object size
        {
            [PageFX.AbcInstanceTrait(28)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(29)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// Specifies custom tab stops as an array of non-negative integers. Each tab stop is
        /// specified in pixels. If custom tab stops are not specified (null), the default tab
        /// stop is 4 (average character width).
        /// </summary>
        public extern virtual Avm.Array tabStops
        {
            [PageFX.AbcInstanceTrait(30)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(31)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// Indicates the target window where the hyperlink is displayed. If the target window is an
        /// empty string, the text is displayed in the default target window _self. You can choose
        /// a custom name or one of the following four names: _self specifies the current frame in
        /// the current window, _blank specifies a new window, _parent specifies the
        /// parent of the current frame, and _top specifies the top-level frame in the current
        /// window. If the TextFormat.url property is an empty string or null,
        /// you can get or set this property, but the property will have no effect.
        /// </summary>
        public extern virtual Avm.String target
        {
            [PageFX.AbcInstanceTrait(32)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(33)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// Indicates whether the text that uses this text format is underlined (true)
        /// or not (false). This underlining is similar to that produced by the
        /// &lt;U&gt; tag, but the latter is not true underlining, because it does not skip
        /// descenders correctly. The default value is null, which indicates that underlining
        /// is not used.
        /// </summary>
        public extern virtual object underline
        {
            [PageFX.AbcInstanceTrait(34)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(35)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// Indicates the target URL for the text in this text format. If the url
        /// property is an empty string, the text does not have a hyperlink. The default value is null,
        /// which indicates that the text does not have a hyperlink.
        /// Note: The text with the assigned text format must be set with the htmlText
        /// property for the hyperlink to work.
        /// </summary>
        public extern virtual Avm.String url
        {
            [PageFX.AbcInstanceTrait(36)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(37)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextFormat(Avm.String font, object size, object color, object bold, object italic, object underline, Avm.String url, Avm.String target, Avm.String align, object leftMargin, object rightMargin, object indent, object leading);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextFormat(Avm.String font, object size, object color, object bold, object italic, object underline, Avm.String url, Avm.String target, Avm.String align, object leftMargin, object rightMargin, object indent);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextFormat(Avm.String font, object size, object color, object bold, object italic, object underline, Avm.String url, Avm.String target, Avm.String align, object leftMargin, object rightMargin);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextFormat(Avm.String font, object size, object color, object bold, object italic, object underline, Avm.String url, Avm.String target, Avm.String align, object leftMargin);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextFormat(Avm.String font, object size, object color, object bold, object italic, object underline, Avm.String url, Avm.String target, Avm.String align);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextFormat(Avm.String font, object size, object color, object bold, object italic, object underline, Avm.String url, Avm.String target);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextFormat(Avm.String font, object size, object color, object bold, object italic, object underline, Avm.String url);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextFormat(Avm.String font, object size, object color, object bold, object italic, object underline);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextFormat(Avm.String font, object size, object color, object bold, object italic);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextFormat(Avm.String font, object size, object color, object bold);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextFormat(Avm.String font, object size, object color);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextFormat(Avm.String font, object size);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextFormat(Avm.String font);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextFormat();


    }
}
