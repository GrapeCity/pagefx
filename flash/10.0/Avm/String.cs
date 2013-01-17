using System;
using System.Runtime.CompilerServices;

namespace Avm
{
    /// <summary>
    /// The String class is a data type that represents a string of characters. The String
    /// class provides methods and properties that let you manipulate primitive string value
    /// types. You can convert the value of any object into a String data type object using
    /// the String()  function.
    /// </summary>
    [PageFX.ABC]
    [PageFX.QName("String")]
    [PageFX.FP9]
    public class String : Object
    {
        public extern virtual int length
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern String(object arg0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern String();

        /// <summary>Matches the specifed pattern against the string.</summary>
        /// <param name="arg0">
        /// The pattern to match, which can be any type of object, but it is typically
        /// either a string or a regular expression. If the pattern is not a regular
        /// expression or a string, then the method converts it to a string before executing.
        /// </param>
        /// <returns>
        /// An array of strings consisting
        /// of all substrings in the string that match the specified pattern.
        /// If pattern is a regular expression, in order to return an array with
        /// more than one matching substring, the g (global) flag must be set in
        /// the regular expression:
        /// If the g (global) flag is not set, the return array will contain
        /// no more than one match, and the lastIndex property of the regular expression
        /// remains unchanged.If the g (global) flag is set, the method starts the search
        /// at the beginning of the string (index position 0). If a matching substring is an
        /// empty string (which can occur with a regular expression such as /x*/),
        /// the method adds that empty string to the array of matches, and then continues searching
        /// at the next index position. The lastIndex property of the regular expression
        /// is set to 0 after the method completes.
        /// If no match is found, the method returns null. If you pass no value
        /// (or an undefined value) as the pattern parameter, the method returns
        /// null.
        /// </returns>
        [PageFX.ABC]
        [PageFX.QName("match", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Array match(object arg0);

        [PageFX.ABC]
        [PageFX.QName("match", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Array match();

        /// <summary>
        /// Searches the string and returns the position of the first occurrence of val
        /// found at or after startIndex within the calling string. This index
        /// is zero-based, meaning that the first character in a string is considered to be
        /// at index 0--not index 1. If val is not found, the method returns -1.
        /// </summary>
        /// <param name="val">The substring for which to search.</param>
        /// <param name="startIndex">
        /// (default = 0)  An optional integer specifying the
        /// starting index of the search.
        /// </param>
        /// <returns>
        /// The index of the first occurrence
        /// of the specified substring or -1.
        /// </returns>
        [PageFX.ABC]
        [PageFX.QName("indexOf", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int indexOf(String val, int startIndex);

        [PageFX.ABC]
        [PageFX.QName("indexOf", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern int indexOf(String val);

        [PageFX.ABC]
        [PageFX.QName("indexOf", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern int indexOf();

        /// <summary>
        /// Returns a string consisting of the character specified by startIndex
        /// and all characters up to endIndex - 1. If endIndex is
        /// not specified, String.length is used. If the value of startIndex
        /// equals the value of endIndex, the method returns an empty string. If
        /// the value of startIndex is greater than the value of endIndex,
        /// the parameters are automatically swapped before the function executes. The original
        /// string is unmodified.
        /// </summary>
        /// <param name="startIndex">
        /// (default = 0)  An integer specifying the index
        /// of the first character used to create the substring. Valid values for startIndex
        /// are 0 through String.length. If startIndex
        /// is a negative value, 0 is used.
        /// </param>
        /// <param name="endIndex">
        /// (default = 0x7fffffff)  An integer that is one
        /// greater than the index of the last character in the extracted substring. Valid values
        /// for endIndex are 0 through String.length.
        /// The character at endIndex is not included in the substring. The default
        /// is the maximum value allowed for an index. If this parameter is omitted, String.length
        /// is used. If this parameter is a negative value, 0 is used.
        /// </param>
        /// <returns>
        /// A substring based on the specified
        /// parameters.
        /// </returns>
        [PageFX.ABC]
        [PageFX.QName("substring", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual String substring(int startIndex, int endIndex);

        [PageFX.ABC]
        [PageFX.QName("substring", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern String substring(int startIndex);

        [PageFX.ABC]
        [PageFX.QName("substring", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern String substring();

        /// <summary>
        /// Returns a string that includes the startIndex character and all characters
        /// up to, but not including, the endIndex character. The original String
        /// object is not modified. If the endIndex parameter is not specified,
        /// then the end of the substring is the end of the string. If the character indexed
        /// by startIndex is the same as or to the right of the character indexed
        /// by endIndex, the method returns an empty string.
        /// </summary>
        /// <param name="startIndex">
        /// (default = 0)  The zero-based index of the starting
        /// point for the slice. If startIndex is a negative number, the slice
        /// is created from right-to-left, where -1 is the last character.
        /// </param>
        /// <param name="endIndex">
        /// (default = 0x7fffffff)  An integer that is one
        /// greater than the index of the ending point for the slice. The character indexed
        /// by the endIndex parameter is not included in the extracted string.
        /// If endIndex is a negative number, the ending point is determined by
        /// counting back from the end of the string, where -1 is the last character. The default
        /// is the maximum value allowed for an index. If this parameter is omitted, String.length
        /// is used.
        /// </param>
        /// <returns>
        /// A substring based on the specified
        /// indices.
        /// </returns>
        [PageFX.ABC]
        [PageFX.QName("slice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual String slice(int startIndex, int endIndex);

        [PageFX.ABC]
        [PageFX.QName("slice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern String slice(int startIndex);

        [PageFX.ABC]
        [PageFX.QName("slice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern String slice();

        /// <summary>
        /// Searches the string from right to left and returns the index of the last occurrence
        /// of val found before startIndex. The index is zero-based,
        /// meaning that the first character is at index 0, and the last is at string.length
        /// - 1. If val is not found, the method returns -1.
        /// </summary>
        /// <param name="val">The string for which to search.</param>
        /// <param name="startIndex">
        /// (default = 0x7FFFFFFF)  An optional integer specifying
        /// the starting index from which to search for val. The default is the
        /// maximum value allowed for an index. If startIndex is not specified,
        /// the search starts at the last item in the string.
        /// </param>
        /// <returns>
        /// The position of the last occurrence
        /// of the specified substring or -1 if not found.
        /// </returns>
        [PageFX.ABC]
        [PageFX.QName("lastIndexOf", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int lastIndexOf(String val, int startIndex);

        [PageFX.ABC]
        [PageFX.QName("lastIndexOf", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern int lastIndexOf(String val);

        [PageFX.ABC]
        [PageFX.QName("lastIndexOf", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern int lastIndexOf();

        /// <summary>
        /// Returns a copy of this string, with all uppercase characters converted to lowercase.
        /// The original string is unmodified.
        /// This method converts all characters (not simply A-Z) for which Unicode lowercase
        /// equivalents exist:  var str:String = &quot; JOSÉ BARÇA&quot;;
        /// trace(str.toLowerCase()); // josé barça
        /// These case mappings are defined in the UnicodeData.txt file and the SpecialCasings.txt file, as defined in the Unicode Character Database specification.
        /// </summary>
        /// <returns>
        /// A copy of this string with
        /// all uppercase characters converted to lowercase.
        /// </returns>
        [PageFX.ABC]
        [PageFX.QName("toLowerCase", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual String toLowerCase();

        /// <summary>
        /// Splits a String object into an array of substrings by dividing it wherever the specified
        /// delimiter parameter occurs.
        /// If the delimiter parameter is a regular expression, only the first
        /// match at a given position of the string is considered, even if backtracking could
        /// find a nonempty substring match at that position. For example:  var str:String = &quot;ab&quot;;
        /// var results:Array = str.split(/a~~?/); // results == [&quot;&quot;,&quot;b&quot;]
        /// results = str.split(/a~~/); // results == [&quot;&quot;,&quot;b&quot;].)
        /// If the delimiter parameter is a regular expression containing grouping
        /// parentheses, then each time the delimiter is matched, the results (including
        /// any undefined results) of the grouping parentheses are spliced into the output array.
        /// For example  var str:String = &quot;Thi5 is a tricky-66 example.&quot;;
        /// var re:RegExp = /(\d+)/;
        /// var results:Array = str.split(re);
        /// // results == [&quot;Thi&quot;,&quot;5&quot;,&quot; is a tricky-&quot;,&quot;66&quot;,&quot; example.&quot;]
        /// If the limit parameter is specified, then the returned array will have
        /// no more than the specified number of elements.
        /// If the delimiter is an empty string, an empty regular expression, or
        /// a regular expression that can match an empty string, each single character in the
        /// string is output as an element in the array.
        /// If the delimiter parameter is undefined, the entire string is placed
        /// into the first element of the returned array.
        /// </summary>
        /// <param name="delimiter">
        /// The pattern that specifies where to split this string. This can be any type
        /// of object but is typically either a string or a regular expression. If the delimiter
        /// is not a regular expression or string, then the method converts it to a string before
        /// executing.
        /// </param>
        /// <param name="limit">
        /// (default
        /// = 0x7fffffff)  The maximum number of items to place
        /// into the array. The default is the maximum value allowed.
        /// </param>
        /// <returns>An array of substrings.</returns>
        [PageFX.ABC]
        [PageFX.QName("split", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Array split(object delimiter, int limit);

        [PageFX.ABC]
        [PageFX.QName("split", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Array split(object delimiter);

        [PageFX.ABC]
        [PageFX.QName("split", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Array split();

        /// <summary>
        /// Appends the supplied arguments to the end of the String object, converting them
        /// to strings if necessary, and returns the resulting string. The original value of
        /// the source String object remains unchanged.
        /// </summary>
        /// <returns>
        /// A new string consisting of
        /// this string concatenated with the specified parameters.
        /// </returns>
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual String concat();

        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern String concat(object rest0);

        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern String concat(object rest0, object rest1);

        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern String concat(object rest0, object rest1, object rest2);

        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern String concat(object rest0, object rest1, object rest2, object rest3);

        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern String concat(object rest0, object rest1, object rest2, object rest3, object rest4);

        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern String concat(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5);

        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern String concat(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6);

        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern String concat(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7);

        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern String concat(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8);

        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern String concat(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8, object rest9);

        /// <summary>
        /// Returns a copy of this string, with all uppercase characters converted to lowercase.
        /// The original string is unmodified. While this method is intended to handle the conversion
        /// in a locale-specific way, the ActionScript 3.0 implementation does not produce a
        /// different result from the toLowerCase() method.
        /// </summary>
        /// <returns>
        /// A copy of this string with
        /// all uppercase characters converted to lowercase.
        /// </returns>
        [PageFX.ABC]
        [PageFX.QName("toLocaleLowerCase", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual String toLocaleLowerCase();

        /// <summary>
        /// Searches for the specifed pattern and returns the index of the first
        /// matching substring. If there is no matching substring, the method returns -1.
        /// </summary>
        /// <param name="pattern">
        /// The pattern to match, which can be any type of object but is typically either
        /// a string or a regular expression.. If the pattern is not a regular
        /// expression or a string, then the method converts it to a string before executing.
        /// Note that if you specify a regular expression, the method ignores the global flag
        /// (&quot;g&quot;) of the regular expression, and it ignores the lastIndex property
        /// of the regular expression (and leaves it unmodified). If you pass an undefined value
        /// (or no value), the method returns -1.
        /// </param>
        /// <returns>
        /// The index of the first matching
        /// substring, or -1 if there is no match. Note that the string is zero-indexed;
        /// the first character of the string is at index 0, the last is at string.length
        /// - 1.
        /// </returns>
        [PageFX.ABC]
        [PageFX.QName("search", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int search(object pattern);

        [PageFX.ABC]
        [PageFX.QName("search", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern int search();

        /// <summary>
        /// Returns the character in the position specified by the index parameter.
        /// If index is not a number from 0 to string.length - 1,
        /// an empty string is returned.
        /// This method is similar to String.charCodeAt() except that the returned
        /// value is a character, not a 16-bit integer character code.
        /// </summary>
        /// <param name="index">
        /// (default
        /// = 0)  An integer specifying the position of a character
        /// in the string. The first character is indicated by 0, and the last
        /// character is indicated by my_str.length - 1.
        /// </param>
        /// <returns>
        /// The character at the specified
        /// index. Or an empty string if the specified index is outside the range of this string&apos;s
        /// indices.
        /// </returns>
        [PageFX.ABC]
        [PageFX.QName("charAt", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual String charAt(int index);

        [PageFX.ABC]
        [PageFX.QName("charAt", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern String charAt();

        /// <summary>
        /// Compares the sort order of two or more strings and returns the result of the comparison
        /// as an integer. While this method is intended to handle the comparison in a locale-specific
        /// way, the ActionScript 3.0 implementation does not produce a different result from
        /// other string comparisons such as the equality (==) or inequality (!=)
        /// operators. If the strings are equivalent, the return value is 0. If the original
        /// string value precedes the string value specified by other, the return
        /// value is a negative integer, the absolute value of which represents the number of
        /// characters that separates the two string values. If the original string value comes
        /// after other, the return value is a positive integer, the absolute value
        /// of which represents the number of characters that separates the two string values.
        /// </summary>
        /// <param name="arg0">A string value to compare.</param>
        /// <returns>
        /// The value 0 if the strings are equal.
        /// Otherwise, a negative integer if the original string precedes the string argument
        /// and a positive integer if the string argument precedes the original string. In both
        /// cases the absolute value of the number represents the difference between the two
        /// strings.
        /// </returns>
        [PageFX.ABC]
        [PageFX.QName("localeCompare", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int localeCompare(String arg0);

        [PageFX.ABC]
        [PageFX.QName("localeCompare", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern int localeCompare();

        [PageFX.ABC]
        [PageFX.QName("toString", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual String toString();

        /// <summary>
        /// Returns the primitive value of a String instance. This method is designed to convert
        /// a String object into a primitive string value. Because Flash Player automatically
        /// calls valueOf() when necessary, you rarely need to explicitly call
        /// this method.
        /// </summary>
        /// <returns>The value of the string.</returns>
        [PageFX.ABC]
        [PageFX.QName("valueOf", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual String valueOf();

        /// <summary>
        /// Returns a substring consisting of the characters that start at the specified startIndex
        /// and with a length specified by len. The original string is unmodified.
        /// </summary>
        /// <param name="startIndex">
        /// (default = 0)  An integer that specified the index
        /// of the first character to be used to create the substring. If startIndex
        /// is a negative number, the starting index is determined from the end of the string,
        /// where -1 is the last character.
        /// </param>
        /// <param name="len">
        /// (default
        /// = 0x7fffffff)  The number of characters in the
        /// substring being created. The default value is the maximum value allowed. If len
        /// is not specified, the substring includes all the characters from startIndex
        /// to the end of the string.
        /// </param>
        /// <returns>
        /// A substring based on the specified
        /// parameters.
        /// </returns>
        [PageFX.ABC]
        [PageFX.QName("substr", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual String substr(int startIndex, int len);

        [PageFX.ABC]
        [PageFX.QName("substr", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern String substr(int startIndex);

        [PageFX.ABC]
        [PageFX.QName("substr", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern String substr();

        /// <summary>
        /// Matches the specifed pattern against the string and returns a new string
        /// in which the first match of pattern is replaced with the content specified
        /// by repl. The pattern parameter can be a string or a regular
        /// expression. The repl parameter can be a string or a function; if it
        /// is a function, the string returned by the function is inserted in place of the match.
        /// The original string is not modified.
        /// In the following example, only the first instance of &quot;sh&quot; (case-sensitive) is replaced:
        /// var myPattern:RegExp = /sh/;
        /// var str:String = &quot;She sells seashells by the seashore.&quot;;
        /// trace(str.replace(myPattern, &quot;sch&quot;));
        /// // She sells seaschells by the seashore.
        /// In the following example, all instances of &quot;sh&quot; (case-sensitive) are replaced because
        /// the g (global) flag is set in the regular expression:
        /// var myPattern:RegExp = /sh/g;
        /// var str:String = &quot;She sells seashells by the seashore.&quot;;
        /// trace(str.replace(myPattern, &quot;sch&quot;));
        /// // She sells seaschells by the seaschore.
        /// In the following example, all instance of &quot;sh&quot; are replaced because the g
        /// (global) flag is set in the regular expression and the matches are not case-sensitive
        /// because the i (ignoreCase) flag is set: var myPattern:RegExp = /sh/gi;
        /// var str:String = &quot;She sells seashells by the seashore.&quot;;
        /// trace(str.replace(myPattern, &quot;sch&quot;));
        /// // sche sells seaschells by the seaschore.
        /// </summary>
        /// <param name="pattern">
        /// The pattern to match, which can be any type of object, but it is typically
        /// either a string or a regular expression. If you specify a pattern parameter
        /// that is any object other than a string or a regular expression, the toString()
        /// method is applied to the parameter and the replace() method executes
        /// using the resulting string as the pattern.
        /// </param>
        /// <param name="repl">
        /// Typically, the string that is inserted in place of the matching content. However,
        /// you can also specify a function as this parameter. If you specify a function, the
        /// string returned by the function is inserted in place of the matching content.
        /// When you specify a string as the repl parameter and specify a regular
        /// expression as the pattern parameter, you can use the following special
        /// $ replacement codes in the repl string:
        /// $ Code
        /// Replacement Text
        /// $$$$&amp;
        /// The matched substring.
        /// $`
        /// The portion of the string that precedes the matched substring. Note that this code
        /// uses the straight left single quote character (`), not the straight single quote
        /// character (&apos;) or the left curly single quote character ().
        /// $&apos;
        /// The portion of string that follows the matched substring. Note that this code uses
        /// the straight single quote character (&apos;).
        /// $n
        /// The nth captured parenthetical group match, where n is a single digit
        /// 1-9 and $n is not followed by a decimal digit.
        /// $nn
        /// The nnth captured parenthetical group match, where nn is a two-digit
        /// decimal number (01-99). If the nnth capture is undefined, the replacement
        /// text is an empty string.
        /// For example, the following shows the use of the $2 and $1
        /// replacement codes, which represent the first and second capturing group matched:var str:String = &quot;flip-flop&quot;;
        /// var pattern:RegExp = /(\w+)-(\w+)/g;
        /// trace(str.replace(pattern, &quot;$2-$1&quot;)); // flop-flip
        /// When you specify a function as the repl, the replace()
        /// method passes the following parameters to the function:
        /// The matching portion of the string. Any captured parenthetical group matches are provided as the next arguments. The
        /// number of arguments passed this way will vary depending on the number of parenthetical
        /// matches. You can determine the number of parenthetical matches by checking arguments.length
        /// - 3 within the function code. The index position in the string where the match begins. The complete string.
        /// For example, consider the following: var str1:String = &quot;abc12 def34&quot;;
        /// var pattern:RegExp = /([a-z]+)([0-9]+)/;
        /// var str2:String = str1.replace(pattern, replFN);
        /// trace (str2);   // 12abc 34def
        /// function replFN():String {
        /// return arguments[2] + arguments[1];
        /// }
        /// The call to the replace() method uses a function as the repl
        /// parameter. The regular expression (/([a-z]([0-9]/g) is matched twice.
        /// The first time, the pattern matches the substring &quot;abc12&quot;, and the
        /// following list of arguments is passed to the function:
        /// {&quot;abc12&quot;, &quot;abc&quot;, &quot;12&quot;, 0, &quot;abc12 def34&quot;}
        /// The second time, the pattern matches the substring &quot;def23&quot;, and the
        /// following list of arguments is passed to the function:
        /// {&quot;def34&quot;, &quot;def&quot;, &quot;34&quot;, 6, &quot;abc123 def34&quot;}
        /// </param>
        /// <returns>
        /// The resulting string. Note
        /// that the source string remains unchanged.
        /// </returns>
        [PageFX.ABC]
        [PageFX.QName("replace", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual String replace(object pattern, object repl);

        [PageFX.ABC]
        [PageFX.QName("replace", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern String replace(object pattern);

        [PageFX.ABC]
        [PageFX.QName("replace", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern String replace();

        /// <summary>
        /// Returns the numeric Unicode character code of the character at the specified index.
        /// If index is not a number from 0 to string.length - 1,
        /// NaN is returned.
        /// This method is similar to String.charAt() except that the returned
        /// value is a 16-bit integer character code, not the actual character.
        /// </summary>
        /// <param name="index">
        /// (default
        /// = 0)  An integer that specifies the position of
        /// a character in the string. The first character is indicated by 0, and
        /// the last character is indicated by my_str.length - 1.
        /// </param>
        /// <returns>
        /// The Unicode character code
        /// of the character at the specified index. Or NaN if the index is outside
        /// the range of this string&apos;s indices.
        /// </returns>
        [PageFX.ABC]
        [PageFX.QName("charCodeAt", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual uint charCodeAt(int index);

        [PageFX.ABC]
        [PageFX.QName("charCodeAt", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint charCodeAt();

        /// <summary>
        /// Returns a copy of this string, with all lowercase characters converted to uppercase.
        /// The original string is unmodified.
        /// This method converts all characters (not simply a-z) for which Unicode uppercase
        /// equivalents exist:  var str:String = &quot;José Barça&quot;;
        /// trace(str.toUpperCase()); // JOSÉ BARÇA
        /// These case mappings are defined in the UnicodeData.txt file and the SpecialCasings.txt file, as defined in the Unicode Character Database specification.
        /// </summary>
        /// <returns>
        /// A copy of this string with
        /// all lowercase characters converted to uppercase.
        /// </returns>
        [PageFX.ABC]
        [PageFX.QName("toUpperCase", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual String toUpperCase();

        /// <summary>
        /// Returns a copy of this string, with all lowercase characters converted to uppercase.
        /// The original string is unmodified. While this method is intended to handle the conversion
        /// in a locale-specific way, the ActionScript 3.0 implementation does not produce a
        /// different result from the toUpperCase() method.
        /// </summary>
        /// <returns>
        /// A copy of this string with
        /// all lowercase characters converted to uppercase.
        /// </returns>
        [PageFX.ABC]
        [PageFX.QName("toLocaleUpperCase", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual String toLocaleUpperCase();

        /// <summary>
        /// Returns a string comprising the characters represented by the Unicode character
        /// codes in the parameters.
        /// </summary>
        /// <returns>
        /// The string value of the specified
        /// Unicode character codes.
        /// </returns>
        [PageFX.ABC]
        [PageFX.QName("fromCharCode", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static String fromCharCode();

        [PageFX.ABC]
        [PageFX.QName("fromCharCode", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static String fromCharCode(object rest0);

        [PageFX.ABC]
        [PageFX.QName("fromCharCode", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static String fromCharCode(object rest0, object rest1);

        [PageFX.ABC]
        [PageFX.QName("fromCharCode", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static String fromCharCode(object rest0, object rest1, object rest2);

        [PageFX.ABC]
        [PageFX.QName("fromCharCode", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static String fromCharCode(object rest0, object rest1, object rest2, object rest3);

        [PageFX.ABC]
        [PageFX.QName("fromCharCode", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static String fromCharCode(object rest0, object rest1, object rest2, object rest3, object rest4);

        [PageFX.ABC]
        [PageFX.QName("fromCharCode", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static String fromCharCode(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5);

        [PageFX.ABC]
        [PageFX.QName("fromCharCode", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static String fromCharCode(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6);

        [PageFX.ABC]
        [PageFX.QName("fromCharCode", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static String fromCharCode(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7);

        [PageFX.ABC]
        [PageFX.QName("fromCharCode", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static String fromCharCode(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8);

        [PageFX.ABC]
        [PageFX.QName("fromCharCode", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static String fromCharCode(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8, object rest9);

        #region Custom Members
        public extern char this[int index]
        {
            [PageFX.ABC]
            [PageFX.QName("charCodeAt", "http://adobe.com/AS3/2006/builtin", "public")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }
        
        //To optimze CIL code
        [PageFX.ABC]
        [PageFX.QName("fromCharCode", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static String fromCharCode(char ch);
        
        [PageFX.ABC]
        [PageFX.QName("fromCharCode", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static String fromCharCode(uint ch);
        #endregion



    }
}
