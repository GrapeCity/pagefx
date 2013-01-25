using System;
using System.Runtime.CompilerServices;

namespace flash.text
{
    /// <summary>
    /// The TextField class is used to create display objects for text display and input.
    /// All dynamic and input text fields in a SWF file are instances of the TextField class.
    /// You can give a text field an instance name in the Property inspector and
    /// use the methods and properties of the TextField class to manipulate it with ActionScript.
    /// TextField instance names are displayed in the Movie Explorer and in the Insert Target Path dialog box
    /// in the Actions panel.
    /// </summary>
    [PageFX.AbcInstance(218)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class TextField : flash.display.InteractiveObject
    {
        /// <summary>
        /// When set to true and the text field is not in focus, Flash Player highlights the
        /// selection in the text field in gray. When set to false and the text field is not in
        /// focus, Flash Player does not highlight the selection in the text field.
        /// </summary>
        public extern virtual bool alwaysShowSelection
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
        /// The type of anti-aliasing used for this text field. Use flash.text.AntiAliasType
        /// constants for this property. You can control this setting only if the font is
        /// embedded (with the embedFonts property set to true).
        /// The default setting is flash.text.AntiAliasType.ADVANCED.
        /// To set values for this property, use the following string values:String valueDescriptionflash.text.AntiAliasType.NORMALApplies the regular text anti-aliasing. This matches the type of anti-aliasing that
        /// Flash Player 7 and earlier versions used.flash.text.AntiAliasType.ADVANCEDApplies advanced anti-aliasing, which makes text more legible. (This feature became
        /// available in Flash Player 8.) Advanced anti-aliasing allows for high-quality rendering
        /// of font faces at small sizes. It is best used with applications
        /// that have a lot of small text. Advanced anti-aliasing is not recommended for
        /// fonts that are larger than 48 points.
        /// </summary>
        public extern virtual Avm.String antiAliasType
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
        /// Controls automatic sizing and alignment of text fields.
        /// Acceptable values for the TextFieldAutoSize constants: TextFieldAutoSize.NONE (the default),
        /// TextFieldAutoSize.LEFT, TextFieldAutoSize.RIGHT, and TextFieldAutoSize.CENTER.
        /// If autoSize is set to TextFieldAutoSize.NONE (the default) no resizing occurs.If autoSize is set to TextFieldAutoSize.LEFT, the text is
        /// treated as left-justified text, meaning that the left margin of the text field remains fixed and any
        /// resizing of a single line of the text field is on the right margin. If the text includes a line break
        /// (for example, &quot;\n&quot; or &quot;\r&quot;), the bottom is also resized to fit the next
        /// line of text. If wordWrap is also set to true, only the bottom
        /// of the text field is resized and the right side remains fixed.If autoSize is set to TextFieldAutoSize.RIGHT, the text is treated as
        /// right-justified text, meaning that the right margin of the text field remains fixed and any resizing
        /// of a single line of the text field is on the left margin. If the text includes a line break
        /// (for example, &quot;\n&quot; or &quot;\r&quot;), the bottom is also resized to fit the next
        /// line of text. If wordWrap is also set to true, only the bottom
        /// of the text field is resized and the left side remains fixed.If autoSize is set to TextFieldAutoSize.CENTER, the text is treated as
        /// center-justified text, meaning that any resizing of a single line of the text field is equally distributed
        /// to both the right and left margins. If the text includes a line break (for example, &quot;\n&quot; or
        /// &quot;\r&quot;), the bottom is also resized to fit the next line of text. If wordWrap is also
        /// set to true, only the bottom of the text field is resized and the left and
        /// right sides remain fixed.
        /// </summary>
        public extern virtual Avm.String autoSize
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
        /// Specifies whether the text field has a background fill. If true, the text field has a
        /// background fill. If false, the text field has no background fill.
        /// Use the backgroundColor property to set the background color of a text field.
        /// </summary>
        public extern virtual bool background
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
        /// The color of the text field background. The default value is 0xFFFFFF (white).
        /// This property can be retrieved or set, even if there currently is no background, but the
        /// color is visible only if the text field has the background property set to
        /// true.
        /// </summary>
        public extern virtual uint backgroundColor
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

        /// <summary>
        /// Specifies whether the text field has a border. If true, the text field has a border.
        /// If false, the text field has no border. Use the borderColor property
        /// to set the border color.
        /// </summary>
        public extern virtual bool border
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
        /// The color of the text field border. The default value is 0x000000 (black).
        /// This property can be retrieved or set, even if there currently is no border, but the
        /// color is visible only if the text field has the border property set to
        /// true.
        /// </summary>
        public extern virtual uint borderColor
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
        /// An integer (1-based index) that indicates the bottommost line that is currently visible in
        /// the specified text field. Think of the text field as a window onto a block of text.
        /// The scrollV property is the 1-based index of the topmost visible line
        /// in the window.
        /// All the text between the lines indicated by scrollV and bottomScrollV
        /// is currently visible in the text field.
        /// </summary>
        public extern virtual int bottomScrollV
        {
            [PageFX.AbcInstanceTrait(14)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// The index of the insertion point (caret) position. If no insertion point is displayed,
        /// the value is the position the insertion point would be if you restored focus to the field (typically where the
        /// insertion point last was, or 0 if the field has not had focus).
        /// Selection span indexes are zero-based (for example, the first position is 0,
        /// the second position is 1, and so on).
        /// </summary>
        public extern virtual int caretIndex
        {
            [PageFX.AbcInstanceTrait(15)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// A Boolean value that specifies whether extra white space (spaces, line breaks, and so on)
        /// in a text field with HTML text should be removed. The default value is false.
        /// The condenseWhite property only affects text set with
        /// the htmlText property, not the text property. If you set
        /// text with the text property, condenseWhite is ignored.
        /// If you set condenseWhite to true, you must use standard HTML commands such as
        /// &lt;BR&gt; and &lt;P&gt; to place line breaks in the text field.Set the condenseWhite property before setting the htmlText property.
        /// </summary>
        public extern virtual bool condenseWhite
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
        /// Specifies the format applied to newly inserted text, such as text inserted with the
        /// replaceSelectedText() method or text entered by a user.
        /// When you access the defaultTextFormat property, the returned TextFormat object has all
        /// of its properties defined. No property is null.Note: You can&apos;t set this property if a style sheet is applied to the text field.
        /// </summary>
        public extern virtual flash.text.TextFormat defaultTextFormat
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
        /// Specifies whether to render by using embedded font outlines.
        /// If false, Flash Player renders the text field by using
        /// device fonts.
        /// If you set the embedFonts property to true for a text field,
        /// you must specify a font for that text by using the font property of
        /// a TextFormat object applied to the text field.
        /// If the specified font is not embedded in the SWF file, the text is not displayed.
        /// </summary>
        public extern virtual bool embedFonts
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
        /// The type of grid fitting used for this text field. This property applies only if the
        /// flash.text.AntiAliasType property of the text field is set to flash.text.AntiAliasType.ADVANCED.
        /// The type of grid fitting used determines whether Flash Player forces strong horizontal and
        /// vertical lines to fit to a pixel or subpixel grid, or not at all.For the flash.text.GridFitType property, you can use the following string values:String valueDescriptionflash.text.GridFitType.NONESpecifies no grid fitting. Horizontal and vertical lines in the glyphs are not
        /// forced to the pixel grid. This setting is usually good for animation or
        /// for large font sizes.flash.text.GridFitType.PIXELSpecifies that strong horizontal and vertical lines are fit to the
        /// pixel grid. This setting works only for left-aligned text fields.
        /// To use this setting, the flash.dispaly.AntiAliasType property of the text field
        /// must be set to flash.text.AntiAliasType.ADVANCED.
        /// This setting generally provides the best legibility for
        /// left-aligned text.flash.text.GridFitType.SUBPIXELSpecifies that strong horizontal and vertical lines are fit to the subpixel grid on
        /// an LCD monitor. To use this setting, the
        /// flash.text.AntiAliasType property of the text field must be set to
        /// flash.text.AntiAliasType.ADVANCED. The flash.text.GridFitType.SUBPIXEL setting is often good
        /// for right-aligned or centered
        /// dynamic text, and it is sometimes a useful trade-off for animation versus text quality.
        /// </summary>
        public extern virtual Avm.String gridFitType
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
        /// Contains the HTML representation of the text field&apos;s contents.
        /// Flash Player supports the following HTML tags:
        /// Tag
        /// Description
        /// Anchor tag
        /// The &lt;a&gt; tag creates a hypertext link and supports the following attributes:
        /// hrefThe URL can be either absolute or relative to the location of the SWF file that
        /// is loading the page. An example of an absolute reference to a URL is http://www.adobe.com;
        /// an example of a relative reference is /index.html. Absolute URLs must be prefixed with
        /// http://; otherwise, Flash treats them as relative URLs.
        /// eventUse the event attribute to specify the
        /// text property of the link TextEvent that is dispatched when the
        /// user clicks the hypertext link. An example is event:myEvent;  when the user clicks
        /// this hypertext link, the text field dispatches a link TextEvent with its
        /// text property set to &quot;myEvent&quot;.
        /// targetSpecifies the name of the target window where you load the page.
        /// Options include _self, _blank, _parent, and
        /// _top. The _self option specifies the current frame in the current window,
        /// _blank specifies a new window, _parent specifies the parent of the
        /// current frame, and _top specifies the top-level frame in the current window.
        /// You can use the link event to cause the link to execute an ActionScript function in a SWF file instead
        /// of opening a URL. You can also define a:link, a:hover, and a:active
        /// styles for anchor tags by using style sheets.
        /// Bold tag
        /// The &lt;b&gt; tag renders text as bold. A bold typeface must be available for the font used.
        /// Break tag
        /// The &lt;br&gt; tag creates a line break in the text field. You must set the text field to
        /// be a multiline text field to use this tag.
        /// Font tag
        /// The &lt;font&gt; tag specifies a font or list of fonts to display the text.The font tag
        /// supports the following attributes:
        /// colorOnly hexadecimal color (#FFFFFF) values are supported.
        /// faceSpecifies the name of the font to use. As shown in the following example,
        /// you can specify a list of comma-delimited font names, in which case Flash Player selects the first available
        /// font. If the specified font is not installed on the user&apos;s computer system or isn&apos;t embedded in the SWF file,
        /// Flash Player selects a substitute font.
        /// sizeSpecifies the size of the font. You can use absolute pixel sizes, such as 16 or 18,
        /// or relative point sizes, such as +2 or -4.
        /// Image tag
        /// The &lt;img&gt; tag lets you embed external image files (JPEG, GIF, PNG), SWF files, and
        /// movie clips inside text fields. Text automatically flows around images you embed in text fields. To use
        /// this tag, you must set the text field to be multiline and to wrap text.
        /// The &lt;img&gt; tag supports the following attributes: srcSpecifies the URL to an image or SWF file, or the linkage identifier for a movie clip
        /// symbol in the library. This attribute is required; all other attributes are optional. External files (JPEG, GIF, PNG,
        /// and SWF files) do not show until they are downloaded completely.
        /// widthThe width of the image, SWF file, or movie clip being inserted, in pixels.
        /// heightThe height of the image, SWF file, or movie clip being inserted, in pixels.
        /// alignSpecifies the horizontal alignment of the embedded image within the text field.
        /// Valid values are left and right. The default value is left.
        /// hspaceSpecifies the amount of horizontal space that surrounds the image where
        /// no text appears. The default value is 8.
        /// vspaceSpecifies the amount of vertical space that surrounds the image where no
        /// text appears. The default value is 8.
        /// idSpecifies the name for the movie clip instance (created by Flash Player) that contains
        /// the embedded image file, SWF file, or movie clip. This is useful if you want to control the embedded content with
        /// ActionScript.
        /// checkPolicyFileSpecifies that Flash Player will check for a cross-domain policy file
        /// on the server associated with the image&apos;s domain. If a cross-domain policy file exists, SWF files in the domains
        /// listed in the file can access the data of the loaded image, for instance by calling the
        /// BitmapData.draw() method with this image as the source parameter. For more information,
        /// see the &quot;Flash Player Security&quot; chapter in Programming ActionScript 3.0.
        /// Flash displays media embedded in a text field at full size. To specify the dimensions of the media
        /// you are embedding, use the &lt;img&gt; tag&apos;s height and width
        /// attributes. In general, an image embedded in a text field appears on the line following the
        /// &lt;img&gt; tag. However, when the &lt;img&gt; tag
        /// is the first character in the text field, the image appears on the first line of the text field.
        /// Italic tag
        /// The &lt;i&gt; tag displays the tagged text in italics. An italic typeface must be available
        /// for the font used.
        /// List item tag
        /// The &lt;li&gt; tag places a bullet in front of the text that it encloses.
        /// Note: Because Flash Player does not recognize ordered and unordered list tags (&lt;ol&gt;
        /// and &lt;ul&gt;, they do not modify how your list is rendered. All lists are unordered and all
        /// list items use bullets.
        /// Paragraph tag
        /// The &lt;p&gt; tag creates a new paragraph. You must set the text field to be a multiline
        /// text field to use this tag.
        /// The &lt;p&gt; tag supports the following attributes:
        /// alignSpecifies alignment of text within the paragraph; valid values are left, right, justify, and center.
        /// classSpecifies a CSS style class defined by a flash.text.StyleSheet object.
        /// Span tag
        /// The &lt;span&gt; tag is available only for use with CSS text styles. It supports the
        /// following attribute:
        /// classSpecifies a CSS style class defined by a flash.text.StyleSheet object.
        /// Text format tag
        /// The &lt;textformat&gt; tag lets you use a subset of paragraph formatting
        /// properties of the TextFormat class within text fields, including line leading, indentation,
        /// margins, and tab stops. You can combine &lt;textformat&gt; tags with the
        /// built-in HTML tags. The &lt;textformat&gt; tag has the following attributes: blockindentSpecifies the block indentation in points; corresponds to
        /// TextFormat.blockIndent.
        /// indentSpecifies the indentation from the left margin to the first character
        /// in the paragraph; corresponds to TextFormat.indent. Both positive and negative
        /// numbers are acceptable.
        /// leadingSpecifies the amount of leading (vertical space) between lines;
        /// corresponds to TextFormat.leading. Both positive and negative numbers are acceptable.
        /// leftmarginSpecifies the left margin of the paragraph, in points; corresponds
        /// to TextFormat.leftMargin.
        /// rightmarginSpecifies the right margin of the paragraph, in points; corresponds
        /// to TextFormat.rightMargin.
        /// tabstopsSpecifies custom tab stops as an array of non-negative integers;
        /// corresponds to TextFormat.tabStops.
        /// Underline tag
        /// The &lt;u&gt; tag underlines the tagged text.
        /// Flash Player supports the following HTML entities:
        /// Entity
        /// Description
        /// &amp;lt;
        /// &lt; (less than)
        /// &amp;gt;
        /// &gt; (greater than)
        /// &amp;amp;
        /// &amp; (ampersand)
        /// &amp;quot;
        /// &quot; (double quotes)
        /// &amp;apos;
        /// &apos; (apostrophe, single quote)
        /// Flash also supports explicit character codes, such as
        /// &amp;#38; (ASCII ampersand) and &amp;#x20AC; (Unicode € symbol).
        /// </summary>
        public extern virtual Avm.String htmlText
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
        /// The number of characters in a text field. A character such as tab (\t) counts as one
        /// character.
        /// </summary>
        public extern virtual int length
        {
            [PageFX.AbcInstanceTrait(26)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual Avm.String textInteractionMode
        {
            [PageFX.AbcInstanceTrait(27)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// The maximum number of characters that the text field can contain, as entered by a user.
        /// A script can insert more text than maxChars allows; the maxChars property
        /// indicates only how much text a user can enter. If the value of this property is 0,
        /// a user can enter an unlimited amount of text.
        /// </summary>
        public extern virtual int maxChars
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

        /// <summary>The maximum value of scrollH.</summary>
        public extern virtual int maxScrollH
        {
            [PageFX.AbcInstanceTrait(30)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>The maximum value of scrollV.</summary>
        public extern virtual int maxScrollV
        {
            [PageFX.AbcInstanceTrait(31)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// A Boolean value that indicates whether Flash Player should automatically scroll multiline
        /// text fields when the user clicks a text field and rolls the mouse wheel.
        /// By default, this value is true. This property is useful if you want to prevent
        /// mouse wheel scrolling of text fields, or implement your own text field scrolling.
        /// </summary>
        public extern virtual bool mouseWheelEnabled
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
        /// Indicates whether the text field is a multiline text field. If the value is true,
        /// the text field is multiline; if the value is false, the text field is a single-line
        /// text field.
        /// </summary>
        public extern virtual bool multiline
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
        /// Defines the number of text lines in a multiline text field.
        /// If wordWrap property is set to true,
        /// the number of lines increases when text wraps.
        /// </summary>
        public extern virtual int numLines
        {
            [PageFX.AbcInstanceTrait(36)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Specifies whether the text field is a password text field. If the value of this property is true,
        /// the text field is treated as a password text field and hides the input characters using asterisks instead of the
        /// actual characters. If false, the text field is not treated as a password text field. When password mode
        /// is enabled, the Cut and Copy commands and their corresponding keyboard shortcuts will
        /// not function.  This security mechanism prevents an unscrupulous user from using the shortcuts to discover
        /// a password on an unattended computer.
        /// </summary>
        public extern virtual bool displayAsPassword
        {
            [PageFX.AbcInstanceTrait(37)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(38)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// Indicates the set of characters that a user can enter into the text field. If the value of the
        /// restrict property is null, you can enter any character. If the value of
        /// the restrict property is an empty string, you cannot enter any character. If the value
        /// of the restrict property is a string of characters, you can enter only characters in
        /// the string into the text field. The string is scanned from left to right. You can specify a range by
        /// using the hyphen (-) character. This only restricts user interaction; a script may put any text into the
        /// text field. This property does not synchronize with the Embed font options
        /// in the Property inspector.If the string begins with a caret (^) character, all characters are initially accepted and
        /// succeeding characters in the string are excluded from the set of accepted characters. If the string does
        /// not begin with a caret (^) character, no characters are initially accepted and succeeding characters in the
        /// string are included in the set of accepted characters.The following example allows only uppercase characters, spaces, and numbers to be entered into
        /// a text field:
        /// my_txt.restrict = &quot;A-Z 0-9&quot;;
        /// The following example includes all characters, but excludes lowercase letters:
        /// my_txt.restrict = &quot;^a-z&quot;;
        /// You can use a backslash to enter a ^ or - verbatim. The accepted backslash sequences are \-, \^ or \\.
        /// The backslash must be an actual character in the string, so when specified in ActionScript, a double backslash
        /// must be used. For example, the following code includes only the dash (-) and caret (^):
        /// my_txt.restrict = &quot;\\-\\^&quot;;
        /// The ^ may be used anywhere in the string to toggle between including characters and excluding characters.
        /// The following code includes only uppercase letters, but excludes the uppercase letter Q:
        /// my_txt.restrict = &quot;A-Z^Q&quot;;
        /// You can use the u escape sequence to construct restrict strings.
        /// The following code includes only the characters from ASCII 32 (space) to ASCII 126 (tilde).
        /// my_txt.restrict = &quot;\u0020-&amp;#92u007E&quot;;
        /// </summary>
        public extern virtual Avm.String restrict
        {
            [PageFX.AbcInstanceTrait(39)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(40)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// The current horizontal scrolling position. If the scrollH property is 0, the text
        /// is not horizontally scrolled. This property value is an integer that represents the horizontal
        /// position in pixels.
        /// The units of horizontal scrolling are pixels, whereas the units of vertical scrolling are lines.
        /// Horizontal scrolling is measured in pixels because most fonts you typically use are proportionally
        /// spaced; that is, the characters can have different widths. Flash Player performs vertical scrolling by
        /// line because users usually want to see a complete line of text rather than a
        /// partial line. Even if a line uses multiple fonts, the height of the line adjusts to fit
        /// the largest font in use.Note: The scrollH property is zero-based, not 1-based like
        /// the scrollV vertical scrolling property.
        /// </summary>
        public extern virtual int scrollH
        {
            [PageFX.AbcInstanceTrait(41)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(42)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// The vertical position of text in a text field. The scrollV property is useful for
        /// directing users to a specific paragraph in a long passage, or creating scrolling text fields.
        /// The units of vertical scrolling are lines, whereas the units of horizontal scrolling are pixels.
        /// If the first line displayed is the first line in the text field, scrollV is set to 1 (not 0).
        /// Horizontal scrolling is measured in pixels because most fonts are proportionally
        /// spaced; that is, the characters can have different widths. Flash performs vertical scrolling by line
        /// because users usually want to see a complete line of text rather than a partial line.
        /// Even if there are multiple fonts on a line, the height of the line adjusts to fit the largest font in
        /// use.
        /// </summary>
        public extern virtual int scrollV
        {
            [PageFX.AbcInstanceTrait(43)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(44)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// A Boolean value that indicates whether the text field is selectable. The value true
        /// indicates that the text is selectable. The selectable property controls whether
        /// a text field is selectable, not whether a text field is editable. A dynamic text field can
        /// be selectable even if it is not editable. If a dynamic text field is not selectable, the user
        /// cannot select its text.
        /// If selectable is set to false, the text in the text field does not
        /// respond to selection commands from the mouse or keyboard, and the text cannot be copied with the
        /// Copy command. If selectable is set to true, the text in the text field
        /// can be selected with the mouse or keyboard, and the text can be copied with the Copy command.
        /// You can select text this way even if the text field is a dynamic text field instead of an input text field.
        /// </summary>
        public extern virtual bool selectable
        {
            [PageFX.AbcInstanceTrait(45)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(46)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual Avm.String selectedText
        {
            [PageFX.AbcInstanceTrait(47)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// The zero-based character index value of the first character in the current selection.
        /// For example, the first character is 0, the second character is 1, and so on. If no
        /// text is selected, this property is the value of caretIndex.
        /// </summary>
        public extern virtual int selectionBeginIndex
        {
            [PageFX.AbcInstanceTrait(48)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// The zero-based character index value of the last character in the current selection.
        /// For example, the first character is 0, the second character is 1, and so on. If no
        /// text is selected, this property is the value of caretIndex.
        /// </summary>
        public extern virtual int selectionEndIndex
        {
            [PageFX.AbcInstanceTrait(49)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// The sharpness of the glyph edges in this text field. This property applies
        /// only if the flash.text.AntiAliasType property of the text field is set to
        /// flash.text.AntiAliasType.ADVANCED. The range for
        /// sharpness is a number from -400 to 400. If you attempt to set
        /// sharpness to a value outside that range, Flash sets the property to
        /// the nearest value in the range (either -400 or 400).
        /// </summary>
        public extern virtual double sharpness
        {
            [PageFX.AbcInstanceTrait(50)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(51)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// Attaches a style sheet to the text field. For information on creating style sheets, see the StyleSheet class
        /// and Programming ActionScript 3.0.
        /// You can change the style sheet associated with a text field at any time. If you change
        /// the style sheet in use, the text field is redrawn with the new style sheet.
        /// You can set the style sheet to null or undefined
        /// to remove the style sheet. If the style sheet in use is removed, the text field is redrawn without a style sheet. Note: If the style sheet is removed, the contents of both TextField.text and
        /// TextField.htmlText change to incorporate the formatting previously applied by the style sheet. To preserve
        /// the original TextField.htmlText contents without the formatting, save the value in a variable before
        /// removing the style sheet.
        /// </summary>
        public extern virtual flash.text.StyleSheet styleSheet
        {
            [PageFX.AbcInstanceTrait(52)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(53)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// A string that is the current text in the text field. Lines are separated by the carriage
        /// return character (&apos;\r&apos;, ASCII 13). This property contains unformatted text in the text
        /// field, without HTML tags.
        /// To get the text in HTML form, use the htmlText property.
        /// </summary>
        public extern virtual Avm.String text
        {
            [PageFX.AbcInstanceTrait(54)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(55)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// The color of the text in a text field, in hexadecimal format.
        /// The hexadecimal color system uses six digits to represent
        /// color values. Each digit has sixteen possible values or characters. The characters range from
        /// 0 to 9 and then A to F. For example, black is 0x000000; white is
        /// 0xFFFFFF.
        /// </summary>
        public extern virtual uint textColor
        {
            [PageFX.AbcInstanceTrait(56)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(57)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>The height of the text in pixels.</summary>
        public extern virtual double textHeight
        {
            [PageFX.AbcInstanceTrait(58)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>The width of the text in pixels.</summary>
        public extern virtual double textWidth
        {
            [PageFX.AbcInstanceTrait(59)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// The thickness of the glyph edges in this text field. This property applies only
        /// when flash.text.AntiAliasType is set to flash.text.AntiAliasType.ADVANCED.
        /// The range for thickness is a number from -200 to 200. If you attempt to
        /// set thickness to a value outside that range, the property is set to the
        /// nearest value in the range (either -200 or 200).
        /// </summary>
        public extern virtual double thickness
        {
            [PageFX.AbcInstanceTrait(60)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(61)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// The type of the text field.
        /// Either one of the following TextFieldType constants: TextFieldType.DYNAMIC,
        /// which specifies a dynamic text field, which a user cannot edit, or TextFieldType.INPUT,
        /// which specifies an input text field, which a user can edit.
        /// </summary>
        public extern virtual Avm.String type
        {
            [PageFX.AbcInstanceTrait(62)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(63)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// A Boolean value that indicates whether the text field has word wrap. If the value of
        /// wordWrap is true, the text field has word wrap;
        /// if the value is false, the text field does not have word wrap. The default
        /// value is false.
        /// </summary>
        public extern virtual bool wordWrap
        {
            [PageFX.AbcInstanceTrait(64)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(65)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// Specifies whether to copy and paste the text formatting along with the text. When set to true,
        /// Flash Player will also copy and paste formatting (such as alignment, bold, and italics) when you copy and paste between text fields. Both the origin and destination text fields for the copy and paste procedure must have
        /// useRichTextClipboard set to true. The default value
        /// is false.
        /// </summary>
        public extern virtual bool useRichTextClipboard
        {
            [PageFX.AbcInstanceTrait(90)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(91)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [PageFX.Event("textInteractionModeChange")]
        public event flash.events.EventHandler textInteractionModeChange
        {
            add { }
            remove { }
        }

        [PageFX.Event("textInput")]
        public event flash.events.TextEventHandler textInput
        {
            add { }
            remove { }
        }

        [PageFX.Event("scroll")]
        public event flash.events.EventHandler scroll
        {
            add { }
            remove { }
        }

        [PageFX.Event("link")]
        public event flash.events.TextEventHandler link
        {
            add { }
            remove { }
        }

        [PageFX.Event("change")]
        public event flash.events.EventHandler change
        {
            add { }
            remove { }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextField();

        /// <summary>
        /// Appends the string specified by the newText parameter to the end of the text
        /// of the text field. This method is more efficient than an addition assignment (+=) on
        /// a text property (such as someTextField.text += moreText),
        /// particularly for a text field that contains a significant amount of content.
        /// </summary>
        /// <param name="newText">The string to append to the existing text.</param>
        [PageFX.AbcInstanceTrait(66)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void appendText(Avm.String newText);

        /// <summary>Returns a rectangle that is the bounding box of the character.</summary>
        /// <param name="charIndex">
        /// The zero-based index value for the character (for example, the first
        /// position is 0, the second position is 1, and so on).
        /// </param>
        /// <returns>
        /// A rectangle with x and y minimum and maximum values
        /// defining the bounding box of the character.
        /// </returns>
        [PageFX.AbcInstanceTrait(68)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.geom.Rectangle getCharBoundaries(int charIndex);

        /// <summary>
        /// Returns the zero-based index value of the character at the point specified by the x
        /// and y parameters.
        /// </summary>
        /// <param name="x">The x coordinate of the character.</param>
        /// <param name="y">The y coordinate of the character.</param>
        /// <returns>
        /// The zero-based index value of the character (for example, the first position is 0,
        /// the second position is 1, and so on).  Returns -1 if the point is not over any character.
        /// </returns>
        [PageFX.AbcInstanceTrait(69)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int getCharIndexAtPoint(double x, double y);

        /// <summary>Given a character index, returns the index of the first character in the same paragraph.</summary>
        /// <param name="charIndex">
        /// The zero-based index value of the character (for example, the first character is 0,
        /// the second character is 1, and so on).
        /// </param>
        /// <returns>The zero-based index value of the first character in the same paragraph.</returns>
        [PageFX.AbcInstanceTrait(71)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int getFirstCharInParagraph(int charIndex);

        /// <summary>
        /// Returns the zero-based index value of the line at the point specified by the x
        /// and y parameters.
        /// </summary>
        /// <param name="x">The x coordinate of the line.</param>
        /// <param name="y">The y coordinate of the line.</param>
        /// <returns>
        /// The zero-based index value of the line (for example, the first line is 0, the
        /// second line is 1, and so on).  Returns -1 if the point is not over any line.
        /// </returns>
        [PageFX.AbcInstanceTrait(72)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int getLineIndexAtPoint(double x, double y);

        /// <summary>
        /// Returns the zero-based index value of the line containing the character specified
        /// by the charIndex parameter.
        /// </summary>
        /// <param name="charIndex">
        /// The zero-based index value of the character (for example, the first character is 0,
        /// the second character is 1, and so on).
        /// </param>
        /// <returns>The zero-based index value of the line.</returns>
        [PageFX.AbcInstanceTrait(73)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int getLineIndexOfChar(int charIndex);

        /// <summary>Returns the number of characters in a specific text line.</summary>
        /// <param name="lineIndex">The line number for which you want the length.</param>
        /// <returns>The number of characters in the line.</returns>
        [PageFX.AbcInstanceTrait(74)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int getLineLength(int lineIndex);

        /// <summary>Returns metrics information about a given text line.</summary>
        /// <param name="lineIndex">The line number for which you want metrics information.</param>
        /// <returns>A TextLineMetrics object.</returns>
        [PageFX.AbcInstanceTrait(75)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.text.TextLineMetrics getLineMetrics(int lineIndex);

        /// <summary>
        /// Returns the character index of the first character in the line that
        /// the lineIndex parameter specifies.
        /// </summary>
        /// <param name="lineIndex">
        /// The zero-based index value of the line (for example, the first line is 0,
        /// the second line is 1, and so on).
        /// </param>
        /// <returns>The zero-based index value of the first character in the line.</returns>
        [PageFX.AbcInstanceTrait(76)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int getLineOffset(int lineIndex);

        /// <summary>Returns the text of the line specified by the lineIndex parameter.</summary>
        /// <param name="lineIndex">
        /// The zero-based index value of the line (for example, the first line is 0,
        /// the second line is 1, and so on).
        /// </param>
        /// <returns>The text string contained in the specified line.</returns>
        [PageFX.AbcInstanceTrait(77)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String getLineText(int lineIndex);

        /// <summary>
        /// Given a character index, returns the length of the paragraph containing the given character.
        /// The length is relative to the first character in the paragraph (as returned by
        /// getFirstCharInParagraph()), not to the character index passed in.
        /// </summary>
        /// <param name="charIndex">
        /// The zero-based index value of the character (for example, the first character is 0,
        /// the second character is 1, and so on).
        /// </param>
        /// <returns>Returns the number of characters in the paragraph.</returns>
        [PageFX.AbcInstanceTrait(78)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int getParagraphLength(int charIndex);

        /// <summary>
        /// Returns a TextFormat object that contains formatting information for the range of text that the
        /// beginIndex and endIndex parameters specify. Only properties
        /// that are common to the entire text specified are set in the resulting TextFormat object.
        /// Any property that is mixed, meaning that it has different values
        /// at different points in the text, has a value of null.
        /// If you do not specify
        /// values for these parameters, this method is applied to all the text in the text field.  The following table describes three possible usages:UsageDescriptionmy_textField.getTextFormat()Returns a TextFormat object containing formatting information for all text in a text field.
        /// Only properties that are common to all text in the text field are set in the resulting TextFormat
        /// object. Any property that is mixed, meaning that it has different values at different
        /// points in the text, has a value of null.my_textField.getTextFormat(beginIndex:Number)Returns a TextFormat object containing a copy of the text format of the character at the
        /// beginIndex position.my_textField.getTextFormat(beginIndex:Number,endIndex:Number)Returns a TextFormat object containing formatting information for the span of
        /// text from beginIndex to endIndex. Only properties that are common
        /// to all of the text in the specified range are set in the resulting TextFormat object. Any property
        /// that is mixed (that is, has different values at different points in the range) has its value set to null.
        /// </summary>
        /// <param name="beginIndex">(default = -1)  Optional; an integer that specifies the starting location of a range of text within the text field.</param>
        /// <param name="endIndex">(default = -1)  Optional; an integer that specifies the ending location of a range of text within the text field.</param>
        /// <returns>The TextFormat object that represents the formatting properties for the specified text.</returns>
        [PageFX.AbcInstanceTrait(79)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.text.TextFormat getTextFormat(int beginIndex, int endIndex);

        [PageFX.AbcInstanceTrait(79)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern flash.text.TextFormat getTextFormat(int beginIndex);

        [PageFX.AbcInstanceTrait(79)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern flash.text.TextFormat getTextFormat();

        [PageFX.AbcInstanceTrait(80)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.Array getTextRuns(int beginIndex, int endIndex);

        [PageFX.AbcInstanceTrait(80)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.Array getTextRuns(int beginIndex);

        [PageFX.AbcInstanceTrait(80)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.Array getTextRuns();

        [PageFX.AbcInstanceTrait(81)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String getRawText();

        [PageFX.AbcInstanceTrait(82)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String getXMLText(int beginIndex, int endIndex);

        [PageFX.AbcInstanceTrait(82)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.String getXMLText(int beginIndex);

        [PageFX.AbcInstanceTrait(82)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.String getXMLText();

        [PageFX.AbcInstanceTrait(83)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void insertXMLText(int beginIndex, int endIndex, Avm.String richText, bool pasting);

        [PageFX.AbcInstanceTrait(83)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void insertXMLText(int beginIndex, int endIndex, Avm.String richText);

        /// <summary>
        /// Replaces the current selection with the contents of the value parameter.
        /// The text is inserted at the position of the current selection, using the current default character
        /// format and default paragraph format. The text is not treated as HTML.
        /// You can use the replaceSelectedText() method to insert and delete text without disrupting
        /// the character and paragraph formatting of the rest of the text.Note: This method will not work if a style sheet is applied to the text field.
        /// </summary>
        /// <param name="value">The string to replace the currently selected text.</param>
        [PageFX.AbcInstanceTrait(85)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void replaceSelectedText(Avm.String value);

        /// <summary>
        /// Replaces the range of characters that the beginIndex and
        /// endIndex parameters specify with the contents
        /// of the newText parameter.
        /// Note: This method will not work if a style sheet is applied to the text field.
        /// </summary>
        /// <param name="beginIndex">The zero-based index value for the start position of the replacement range.</param>
        /// <param name="endIndex">The zero-based index value for end position of the replacement range.</param>
        /// <param name="newText">The text to use to replace the specified range of characters.</param>
        [PageFX.AbcInstanceTrait(86)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void replaceText(int beginIndex, int endIndex, Avm.String newText);

        /// <summary>
        /// Sets as selected the text designated by the index values of the
        /// first and last characters, which are specified with the beginIndex
        /// and endIndex parameters. If the two parameter values are the same,
        /// this method sets the insertion point, just as if you set the
        /// caretIndex property.
        /// </summary>
        /// <param name="beginIndex">
        /// The zero-based index value of the first character in the selection
        /// (for example, the first character is 0, the second character is 1, and so on).
        /// </param>
        /// <param name="endIndex">The zero-based index value of the last character in the selection.</param>
        [PageFX.AbcInstanceTrait(87)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void setSelection(int beginIndex, int endIndex);

        /// <summary>
        /// Applies the text formatting that the format parameter specifies to the specified text in a text field.
        /// The value of format must be a TextFormat object that specifies the
        /// desired text formatting changes. Only the non-null properties of format are applied
        /// to the text field. Any property of format that is set to null is not
        /// applied. By default, all of the properties of a newly created TextFormat object are set to null.
        /// Note: This method will not work if a style sheet is applied to the text field.The setTextFormat() method changes the text formatting applied to a range of
        /// characters or to the entire body of text in a text field. To apply the properties of format to all text in the text
        /// field, do not specify values for beginIndex and endIndex. To apply the
        /// properties of the format to a range of text, specify values for the beginIndex and
        /// the endIndex parameters. You can use the length property to determine
        /// the index values.The two types of formatting information in a TextFormat object are
        /// character level formatting and paragraph level formatting.
        /// Each character in a text field might have its own character formatting
        /// settings, such as font name, font size, bold, and italic.For paragraphs, the first character of the paragraph is examined for the paragraph formatting
        /// settings for the entire paragraph. Examples of paragraph formatting settings are left margin,
        /// right margin, and indentation.Any text inserted manually by the user, or replaced by means of the
        /// replaceSelectedText() method, receives the text field&apos;s default formatting for new text,
        /// and not the formatting specified for the text insertion point. To set a text field&apos;s default
        /// formatting for new text, use defaultTextFormat.
        /// </summary>
        /// <param name="format">A TextFormat object that contains character and paragraph formatting information.</param>
        /// <param name="beginIndex">
        /// (default = -1)  The zero-based index position specifying the first character of the
        /// desired range of text.
        /// </param>
        /// <param name="endIndex">
        /// (default = -1)  The zero-based index position specifying the last character of the desired
        /// range of text.
        /// UsageDescriptionmy_textField.setTextFormat(textFormat:TextFormat)Applies the properties of textFormat to all text in the text
        /// field.my_textField.setTextFormat(textFormat:TextFormat, beginIndex:int)Applies the properties of textFormat to the text starting with the
        /// beginIndex position.my_textField.setTextFormat(textFormat:TextFormat, beginIndex:int,
        /// endIndex:int)Applies the properties of the textFormat parameter to the span of
        /// text from the beginIndex position to the endIndex position.Notice that any text inserted manually by the user, or replaced by means of the
        /// replaceSelectedText() method, receives the text field&apos;s default formatting for new
        /// text, and not the formatting specified for the text insertion point. To set a text field&apos;s
        /// default formatting for new text, use the defaultTextFormat property.
        /// </param>
        [PageFX.AbcInstanceTrait(88)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void setTextFormat(flash.text.TextFormat format, int beginIndex, int endIndex);

        [PageFX.AbcInstanceTrait(88)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void setTextFormat(flash.text.TextFormat format, int beginIndex);

        [PageFX.AbcInstanceTrait(88)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void setTextFormat(flash.text.TextFormat format);

        /// <summary>
        /// Returns a DisplayObject reference for the given id, for an image or SWF file
        /// that has been added to an HTML-formatted text field by using an &lt;img&gt; tag.
        /// The &lt;img&gt; tag is in the following format:
        /// &lt;img src = &apos;filename.jpg&apos; id = &apos;instanceName&apos; &gt;
        /// </summary>
        /// <param name="id">
        /// The id to match (in the id attribute of the
        /// &lt;img&gt; tag).
        /// </param>
        /// <returns>
        /// The display object corresponding to the image or SWF file with the matching id
        /// attribute in the &lt;img&gt; tag of the text field. For media loaded from an external source,
        /// this object is a Loader object, and, once loaded, the media object is a child of that Loader object. For media
        /// embedded in the SWF file, this is the loaded object. If there is no &lt;img&gt; tag with
        /// the matching id, the method returns null.
        /// </returns>
        [PageFX.AbcInstanceTrait(89)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.display.DisplayObject getImageReference(Avm.String id);

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static bool isFontCompatible(Avm.String fontName, Avm.String fontStyle);
    }
}
