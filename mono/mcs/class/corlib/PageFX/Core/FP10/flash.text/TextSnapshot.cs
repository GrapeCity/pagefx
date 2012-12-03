using System;
using System.Runtime.CompilerServices;

namespace flash.text
{
    /// <summary>
    /// TextSnapshot objects let you work with static text in a movie clip. You can use them, for example,
    /// to lay out text with greater precision than that allowed by dynamic text, but still access the text
    /// in a read-only way.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class TextSnapshot : Avm.Object
    {
        public extern virtual int charCount
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextSnapshot();

        /// <summary>
        /// Returns a Boolean value that specifies whether a TextSnapshot object contains selected text in
        /// the specified range.
        /// To search all characters, pass a value of 0 for start, and
        /// charCount (or any very large number) for end.
        /// To search a single character, pass the end parameter a value that is one greater
        /// than the start parameter.
        /// </summary>
        /// <param name="arg0">
        /// Indicates the position of the first character to be examined.
        /// Valid values for beginIndex are 0 through
        /// TextSnapshot.charCount - 1. If beginIndex is a negative value,
        /// 0 is used.
        /// </param>
        /// <param name="arg1">
        /// A value that is one greater than the index of the last character to be examined. Valid values
        /// for endIndex are 0 through charCount.
        /// The character indexed by the endIndex parameter is not included in the extracted
        /// string. If this parameter is omitted, charCount is used. If this value is less than
        /// or equal to the value of beginIndex, beginIndex + 1 is used.
        /// </param>
        /// <returns>
        /// A Boolean value that indicates whether at least one character in the given range has been
        /// selected by the corresponding setSelected() method (true); otherwise,
        /// false.
        /// </returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool getSelected(int arg0, int arg1);

        /// <summary>
        /// Returns a string that contains all the characters specified by the beginIndex
        /// and endIndex parameters. If no characters are selected, an empty string is
        /// returned.
        /// To return all characters, pass a value of 0 for beginIndex and
        /// charCount (or any very large number) for endIndex.
        /// To return a single character, pass a value of beginIndex + 1 for endIndex. If you pass a value of true for includeLineEndings,
        /// newline characters are inserted in the string returned where deemed appropriate. In this case,
        /// the return string might be longer than the input range. If includeLineEndings
        /// is false or omitted, the selected text is returned without any characters added.
        /// </summary>
        /// <param name="arg0">
        /// Indicates the position of the first character to be included in the
        /// returned string. Valid values for beginIndex are0 through
        /// charCount - 1. If beginIndex is a negative value,
        /// 0 is used.
        /// </param>
        /// <param name="arg1">
        /// A value that is one greater than the index of the last character to be examined. Valid values
        /// for endIndex are 0 through charCount. The character
        /// indexed by the endIndex parameter is not included in the extracted string. If this
        /// parameter is omitted, charCount is used. If this value is less than or
        /// equal to the value of beginIndex, beginIndex + 1 is used.
        /// </param>
        /// <param name="arg2">
        /// (default = false)  An optional Boolean value that specifies whether newline characters
        /// are inserted (true) or are not inserted (false) into the returned string.
        /// The default value is false.
        /// </param>
        /// <returns>
        /// A string containing the characters in the specified range, or an empty string if no
        /// characters are found in the specified range.
        /// </returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String getText(int arg0, int arg1, bool arg2);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.String getText(int arg0, int arg1);

        /// <summary>
        /// Specifies a range of characters in a TextSnapshot object to be selected or deselected.
        /// Characters that are selected are drawn with a colored rectangle behind them, matching the
        /// bounding box of the character. The color of the bounding box is defined by
        /// setSelectColor().
        /// To select or deselect all characters, pass a value of 0 for beginIndex and
        /// charCount (or any very large number) for endIndex. To
        /// specify a single character, pass a value of start + 1 for endIndex. Because characters are individually marked as selected, you can call this method multiple
        /// times to select multiple characters; that is, using this method does not deselect other
        /// characters that have been set by this method.The colored rectangle that indicates a selection is displayed only for fonts that include
        /// character metric information; by default, Flash does not include this information for static
        /// text fields. In some cases, this behavior means that text that is selected won&apos;t appear to be
        /// selected onscreen. To ensure that all selected text appears to be
        /// selected, you can force the Flash authoring tool to include the character metric information
        /// for a font. To do this, add a dynamic text field that uses that font, select Character Options
        /// for that dynamic text field, and then specify that font outlines should be embedded for at least
        /// one character. It doesn&apos;t matter which characters you specify, nor even if they are the
        /// characters used in the static text fields in question.
        /// </summary>
        /// <param name="arg0">
        /// Indicates the position of the first character to select.
        /// Valid values for beginIndex are 0 through charCount - 1.
        /// If beginIndex is a negative value, 0 is used.
        /// </param>
        /// <param name="arg1">
        /// An integer that is 1+ the index of the last character to be
        /// examined. Valid values for end are 0 through charCount.
        /// The character indexed by the end parameter is not included in the extracted
        /// string. If you omit this parameter, TextSnapshot.charCount is used. If the
        /// value of beginIndex is less than or equal to the value of endIndex,
        /// beginIndex + 1 is used.
        /// </param>
        /// <param name="arg2">
        /// A Boolean value that specifies whether the text should be selected (true)
        /// or deselected (false).
        /// </param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void setSelected(int arg0, int arg1, bool arg2);

        /// <summary>
        /// Specifies the color to use when highlighting characters that have been selected with the
        /// setSelected() method. The color is always opaque; you can&apos;t specify a
        /// transparency value.
        /// This method works correctly only with fonts that include character metric information; however,
        /// by default, the Flash authoring tool does not include this information for static text fields.
        /// Therefore, the method might return -1 instead of an index value. To
        /// ensure that an index value is returned, you can force the Flash authoring tool to include the
        /// character metric information for a font. To do this, add a dynamic text field that uses that
        /// font, select Character Options for that dynamic text field, and then specify that font outlines
        /// should be embedded for at least one character. (It doesn&apos;t matter which characters you
        /// specify, nor if they are the characters used in the static text fields.)
        /// </summary>
        /// <param name="arg0">
        /// (default = 0xFFFF00)  The color used for the border placed around characters that have been selected by the
        /// corresponding setSelected() command, expressed in hexadecimal
        /// format (0xRRGGBB).
        /// </param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void setSelectColor(uint arg0);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void setSelectColor();

        /// <summary>
        /// Searches the specified TextSnapshot object and returns the position of the first
        /// occurrence of textToFind found at or after beginIndex. If
        /// textToFind is not found, the method returns -1.
        /// </summary>
        /// <param name="arg0">Specifies the starting point to search for the specified text.</param>
        /// <param name="arg1">
        /// Specifies the text to search for. If you specify a string literal instead of a
        /// variable of type String, enclose the string in quotation marks.
        /// </param>
        /// <param name="arg2">
        /// Specifies whether the text must match the case of the string in
        /// textToFind.
        /// </param>
        /// <returns>The zero-based index position of the first occurrence of the specified text, or -1.</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int findText(int arg0, Avm.String arg1, bool arg2);

        /// <summary>
        /// Lets you determine which character within a TextSnapshot object is on or near the specified
        /// x, y coordinates of the movie clip containing the text in the TextSnapshot object.
        /// If you omit or pass a value of 0 for maxDistance, the location specified
        /// by the x, y coordinates must lie inside the bounding box of the TextSnapshot object.
        /// This method works correctly only with fonts that include character metric information; however,
        /// by default, the Flash authoring tool does not include this information for static text fields.
        /// Therefore,
        /// the method might return -1 instead of an index value. To ensure that an index
        /// value is returned, you can force the Flash authoring tool to include the character metric
        /// information for a font. To do this, add a dynamic text field that uses that font, select
        /// Character Options for that dynamic text field, and then specify that font outlines should be
        /// embedded for at least one character. (It doesn&apos;t matter which characters you specify, nor
        /// whether they are the characters used in the static text fields.)
        /// </summary>
        /// <param name="arg0">
        /// A number that represents the x coordinate of the movie clip containing the
        /// text.
        /// </param>
        /// <param name="arg1">
        /// A number that represents the y coordinate of the movie clip containing the
        /// text.
        /// </param>
        /// <param name="arg2">
        /// (default = 0)  An optional number that represents the maximum distance from
        /// x, y that can be searched for
        /// text. The distance is measured from the center point of each character. The
        /// default value is 0.
        /// </param>
        /// <returns>
        /// A number representing the index value of the character that is nearest to the specified
        /// x, y coordinate. Returns
        /// -1 if no character is found, or if the font doesn&apos;t contain character metric information.
        /// </returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual double hitTestTextNearPos(double arg0, double arg1, double arg2);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern double hitTestTextNearPos(double arg0, double arg1);

        /// <summary>
        /// Returns an array of objects that contains information about a run of text. Each object corresponds
        /// to one character in the range of characters specified by the two method parameters.
        /// Note:  Using the getTextRunInfo() method for a large range of text can
        /// return a large object. Adobe recommends limiting the text range defined by the
        /// beginIndex and endIndex parameters.
        /// </summary>
        /// <param name="arg0">
        /// The index value of the first character in a range of characters in a TextSnapshot
        /// object.
        /// </param>
        /// <param name="arg1">
        /// The index value of the last character in a range of characters in a TextSnapshot
        /// object.
        /// </param>
        /// <returns>
        /// An array of objects in which each object contains information about a specific character
        /// in the range of characters specified by the beginIndex and endIndex parameters.
        /// Each object contains the following eleven properties:
        /// indexInRunA zero-based integer index of the character
        /// (relative to the entire string rather than the selected run of text).selectedA Boolean value that indicates whether the character is selected
        /// true; false otherwise.fontThe name of the character&apos;s font.colorThe combined alpha and color value of the character.
        /// The first two hexadecimal digits represent the alpha value, and the remaining digits
        /// represent the color value.heightThe height of the character, in pixels.matrix_a, matrix_b, matrix_c,
        /// matrix_d, matrix_tx, and matrix_ty
        /// The values of a matrix that define the geometric transformation on the character.
        /// Normal, upright text always has a matrix of the form
        /// [1 0 0 1 x y], where x and y
        /// are the position of the character within the parent movie clip, regardless of the height of
        /// the text. The matrix is in the parent movie clip coordinate system, and
        /// does not include any transformations that may be on that movie clip itself (or its parent). corner0x, corner0y, corner1x, corner1y,
        /// corner2x, corner2y, corner3x,
        /// and corner3yThe corners of the bounding box of
        /// the character, based on the coordinate system of the parent movie clip.
        /// These values are only available if the font used by the character is embedded in the
        /// SWF file.
        /// </returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.Array getTextRunInfo(int arg0, int arg1);

        /// <summary>
        /// Returns a string that contains all the characters specified by the corresponding
        /// setSelected() method. If no characters are specified (by the
        /// setSelected() method), an empty string is returned.
        /// If you pass true for includeLineEndings,
        /// newline characters are inserted in the return string, and
        /// the return string might be longer than the input range. If
        /// includeLineEndings is false or omitted, the method returns
        /// the selected text without adding any characters.
        /// </summary>
        /// <param name="arg0">
        /// (default = false)  An optional Boolean value that specifies
        /// whether newline characters are inserted into the returned string where
        /// appropriate. The default value is false.
        /// </param>
        /// <returns>
        /// A string that contains all the characters specified by the
        /// corresponding setSelected() command.
        /// </returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String getSelectedText(bool arg0);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.String getSelectedText();
    }
}
