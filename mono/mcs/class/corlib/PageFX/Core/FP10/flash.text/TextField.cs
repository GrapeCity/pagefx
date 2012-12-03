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
    [PageFX.ABC]
    [PageFX.FP9]
    public class TextField : flash.display.InteractiveObject
    {
        public extern virtual bool alwaysShowSelection
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

        public extern virtual double sharpness
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

        public extern virtual bool wordWrap
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

        public extern virtual Avm.String gridFitType
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

        public extern virtual int caretIndex
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual uint borderColor
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

        public extern virtual bool condenseWhite
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

        public extern virtual int numLines
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual int scrollH
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

        public extern virtual int maxScrollH
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual Avm.String autoSize
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

        public extern virtual TextFormat defaultTextFormat
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

        public extern virtual double textWidth
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual int scrollV
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

        public extern virtual uint backgroundColor
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

        public extern virtual bool embedFonts
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

        public extern virtual bool border
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

        public extern virtual bool multiline
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

        public extern virtual bool background
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

        public extern virtual int maxChars
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

        public extern virtual bool selectable
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

        public extern virtual int maxScrollV
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual bool displayAsPassword
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

        public extern virtual bool mouseWheelEnabled
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

        public extern virtual double textHeight
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual Avm.String restrict
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

        public extern virtual StyleSheet styleSheet
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

        public extern virtual bool useRichTextClipboard
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

        public extern virtual Avm.String type
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

        public extern virtual uint textColor
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

        public extern virtual Avm.String antiAliasType
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

        public extern virtual Avm.String text
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

        public extern virtual int selectionEndIndex
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual int selectionBeginIndex
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual int length
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual Avm.String selectedText
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual Avm.String htmlText
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

        public extern virtual double thickness
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

        public extern virtual int bottomScrollV
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
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
        /// Replaces the range of characters that the beginIndex and
        /// endIndex parameters specify with the contents
        /// of the newText parameter.
        /// Note: This method will not work if a style sheet is applied to the text field.
        /// </summary>
        /// <param name="arg0">The zero-based index value for the start position of the replacement range.</param>
        /// <param name="arg1">The zero-based index value for end position of the replacement range.</param>
        /// <param name="arg2">The text to use to replace the specified range of characters.</param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void replaceText(int arg0, int arg1, Avm.String arg2);

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
        /// <param name="arg0">A TextFormat object that contains character and paragraph formatting information.</param>
        /// <param name="arg1">
        /// (default = -1)  The zero-based index position specifying the first character of the
        /// desired range of text.
        /// </param>
        /// <param name="arg2">
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
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void setTextFormat(TextFormat arg0, int arg1, int arg2);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void setTextFormat(TextFormat arg0, int arg1);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void setTextFormat(TextFormat arg0);

        /// <summary>Returns the number of characters in a specific text line.</summary>
        /// <param name="arg0">The line number for which you want the length.</param>
        /// <returns>The number of characters in the line.</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int getLineLength(int arg0);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.Array getTextRuns(int arg0, int arg1);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.Array getTextRuns(int arg0);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.Array getTextRuns();

        /// <summary>
        /// Returns the character index of the first character in the line that
        /// the lineIndex parameter specifies.
        /// </summary>
        /// <param name="arg0">
        /// The zero-based index value of the line (for example, the first line is 0,
        /// the second line is 1, and so on).
        /// </param>
        /// <returns>The zero-based index value of the first character in the line.</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int getLineOffset(int arg0);

        /// <summary>
        /// Returns a DisplayObject reference for the given id, for an image or SWF file
        /// that has been added to an HTML-formatted text field by using an &lt;img&gt; tag.
        /// The &lt;img&gt; tag is in the following format:
        /// &lt;img src = &apos;filename.jpg&apos; id = &apos;instanceName&apos; &gt;
        /// </summary>
        /// <param name="arg0">
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
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.display.DisplayObject getImageReference(Avm.String arg0);

        /// <summary>Returns the text of the line specified by the lineIndex parameter.</summary>
        /// <param name="arg0">
        /// The zero-based index value of the line (for example, the first line is 0,
        /// the second line is 1, and so on).
        /// </param>
        /// <returns>The text string contained in the specified line.</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String getLineText(int arg0);

        /// <summary>Given a character index, returns the index of the first character in the same paragraph.</summary>
        /// <param name="arg0">
        /// The zero-based index value of the character (for example, the first character is 0,
        /// the second character is 1, and so on).
        /// </param>
        /// <returns>The zero-based index value of the first character in the same paragraph.</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int getFirstCharInParagraph(int arg0);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String getRawText();

        /// <summary>Returns a rectangle that is the bounding box of the character.</summary>
        /// <param name="arg0">
        /// The zero-based index value for the character (for example, the first
        /// position is 0, the second position is 1, and so on).
        /// </param>
        /// <returns>
        /// A rectangle with x and y minimum and maximum values
        /// defining the bounding box of the character.
        /// </returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.geom.Rectangle getCharBoundaries(int arg0);

        /// <summary>
        /// Replaces the current selection with the contents of the value parameter.
        /// The text is inserted at the position of the current selection, using the current default character
        /// format and default paragraph format. The text is not treated as HTML.
        /// You can use the replaceSelectedText() method to insert and delete text without disrupting
        /// the character and paragraph formatting of the rest of the text.Note: This method will not work if a style sheet is applied to the text field.
        /// </summary>
        /// <param name="arg0">The string to replace the currently selected text.</param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void replaceSelectedText(Avm.String arg0);

        /// <summary>
        /// Given a character index, returns the length of the paragraph containing the given character.
        /// The length is relative to the first character in the paragraph (as returned by
        /// getFirstCharInParagraph()), not to the character index passed in.
        /// </summary>
        /// <param name="arg0">
        /// The zero-based index value of the character (for example, the first character is 0,
        /// the second character is 1, and so on).
        /// </param>
        /// <returns>Returns the number of characters in the paragraph.</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int getParagraphLength(int arg0);

        /// <summary>
        /// Sets as selected the text designated by the index values of the
        /// first and last characters, which are specified with the beginIndex
        /// and endIndex parameters. If the two parameter values are the same,
        /// this method sets the insertion point, just as if you set the
        /// caretIndex property.
        /// </summary>
        /// <param name="arg0">
        /// The zero-based index value of the first character in the selection
        /// (for example, the first character is 0, the second character is 1, and so on).
        /// </param>
        /// <param name="arg1">The zero-based index value of the last character in the selection.</param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void setSelection(int arg0, int arg1);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String getXMLText(int arg0, int arg1);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.String getXMLText(int arg0);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.String getXMLText();

        /// <summary>
        /// Returns the zero-based index value of the character at the point specified by the x
        /// and y parameters.
        /// </summary>
        /// <param name="arg0">The x coordinate of the character.</param>
        /// <param name="arg1">The y coordinate of the character.</param>
        /// <returns>
        /// The zero-based index value of the character (for example, the first position is 0,
        /// the second position is 1, and so on).  Returns -1 if the point is not over any character.
        /// </returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int getCharIndexAtPoint(double arg0, double arg1);

        /// <summary>
        /// Appends the string specified by the newText parameter to the end of the text
        /// of the text field. This method is more efficient than an addition assignment (+=) on
        /// a text property (such as someTextField.text += moreText),
        /// particularly for a text field that contains a significant amount of content.
        /// </summary>
        /// <param name="arg0">The string to append to the existing text.</param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void appendText(Avm.String arg0);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void insertXMLText(int arg0, int arg1, Avm.String arg2, bool arg3);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void insertXMLText(int arg0, int arg1, Avm.String arg2);

        /// <summary>
        /// Returns the zero-based index value of the line at the point specified by the x
        /// and y parameters.
        /// </summary>
        /// <param name="arg0">The x coordinate of the line.</param>
        /// <param name="arg1">The y coordinate of the line.</param>
        /// <returns>
        /// The zero-based index value of the line (for example, the first line is 0, the
        /// second line is 1, and so on).  Returns -1 if the point is not over any line.
        /// </returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int getLineIndexAtPoint(double arg0, double arg1);

        /// <summary>Returns metrics information about a given text line.</summary>
        /// <param name="arg0">The line number for which you want metrics information.</param>
        /// <returns>A TextLineMetrics object.</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual TextLineMetrics getLineMetrics(int arg0);

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
        /// <param name="arg0">(default = -1)  Optional; an integer that specifies the starting location of a range of text within the text field.</param>
        /// <param name="arg1">(default = -1)  Optional; an integer that specifies the ending location of a range of text within the text field.</param>
        /// <returns>The TextFormat object that represents the formatting properties for the specified text.</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual TextFormat getTextFormat(int arg0, int arg1);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextFormat getTextFormat(int arg0);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextFormat getTextFormat();

        /// <summary>
        /// Returns the zero-based index value of the line containing the character specified
        /// by the charIndex parameter.
        /// </summary>
        /// <param name="arg0">
        /// The zero-based index value of the character (for example, the first character is 0,
        /// the second character is 1, and so on).
        /// </param>
        /// <returns>The zero-based index value of the line.</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int getLineIndexOfChar(int arg0);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static bool isFontCompatible(Avm.String arg0, Avm.String arg1);
    }
}
