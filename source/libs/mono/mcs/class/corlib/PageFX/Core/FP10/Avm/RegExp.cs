using System;
using System.Runtime.CompilerServices;

namespace Avm
{
    /// <summary>
    /// The RegExp class lets you work with regular expressions, which are patterns that you can use
    /// to perform searches in strings and to replace text in strings.
    /// </summary>
    [PageFX.ABC]
    [PageFX.QName("RegExp")]
    [PageFX.FP9]
    public class RegExp : Object
    {
        public extern virtual bool ignoreCase
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual bool global
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual int lastIndex
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

        public extern virtual bool extended
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual String source
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual bool multiline
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual bool dotall
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern RegExp(object arg0, object arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern RegExp(object arg0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern RegExp();

        /// <summary>
        /// Performs a search for the regular expression on the given string str.
        /// If the g (global) flag is not set for the regular
        /// expression, then the search starts
        /// at the beginning of the string (at index position 0); the search ignores
        /// the lastIndex property of the regular expression.If the g (global) flag is set for the regular
        /// expression, then the search starts
        /// at the index position specified by the lastIndex property of the regular expression.
        /// If the search matches a substring, the lastIndex property changes to match the position
        /// of the end of the match.
        /// </summary>
        /// <param name="arg0">The string to search.</param>
        /// <returns>
        /// If there is no match, null; otherwise, an object with the following properties:
        /// An array, in which element 0 contains the complete matching substring, and
        /// other elements of the array (1 through n) contain substrings that match parenthetical groups
        /// in the regular expression index  The character position of the matched substring within
        /// the stringinput  The string (str)
        /// </returns>
        [PageFX.ABC]
        [PageFX.QName("exec", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual object exec(String arg0);

        [PageFX.ABC]
        [PageFX.QName("exec", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object exec();

        /// <summary>
        /// Tests for the match of the regular expression in the given string str.
        /// If the g (global) flag is not set for the regular expression,
        /// then the search starts at the beginning of the string (at index position 0); the search ignores
        /// the lastIndex property of the regular expression.If the g (global) flag is set for the regular expression, then the search starts
        /// at the index position specified by the lastIndex property of the regular expression.
        /// If the search matches a substring, the lastIndex property changes to match the
        /// position of the end of the match.
        /// </summary>
        /// <param name="arg0">The string to test.</param>
        /// <returns>If there is a match, true; otherwise, false.</returns>
        [PageFX.ABC]
        [PageFX.QName("test", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool test(String arg0);

        [PageFX.ABC]
        [PageFX.QName("test", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern bool test();


    }
}
