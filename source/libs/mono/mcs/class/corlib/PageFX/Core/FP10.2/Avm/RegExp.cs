using System;
using System.Runtime.CompilerServices;

namespace Avm
{
    /// <summary>
    /// The RegExp class lets you work with regular expressions, which are patterns that you can use
    /// to perform searches in strings and to replace text in strings.
    /// </summary>
    [PageFX.AbcInstance(197)]
    [PageFX.ABC]
    [PageFX.QName("RegExp")]
    [PageFX.FP9]
    public class RegExp : Avm.Object
    {
        /// <summary>Specifies the pattern portion of the regular expression.</summary>
        public extern virtual Avm.String source
        {
            [PageFX.AbcInstanceTrait(0)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Specifies whether to use global matching for the regular expression. When
        /// global == true, the lastIndex property is set after a match is
        /// found. The next time a match is requested, the regular expression engine starts from
        /// the lastIndex position in the string. Use the g flag when
        /// constructing a regular expression  to set global to true.
        /// </summary>
        public extern virtual bool global
        {
            [PageFX.AbcInstanceTrait(1)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Specifies whether the regular expression ignores case sensitivity. Use the
        /// i flag when constructing a regular expression to set
        /// ignoreCase = true.
        /// </summary>
        public extern virtual bool ignoreCase
        {
            [PageFX.AbcInstanceTrait(2)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Specifies whether the m (multiline) flag is set. If it is set,
        /// the caret (^) and dollar sign ($) in a regular expression
        /// match before and after new lines.
        /// Use the m flag when constructing a regular expression to set
        /// multiline = true.
        /// </summary>
        public extern virtual bool multiline
        {
            [PageFX.AbcInstanceTrait(3)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Specifies the index position in the string at which to start the next search. This property
        /// affects the exec() and test() methods of the RegExp class.
        /// However, the match(), replace(), and search() methods
        /// of the String class ignore the lastIndex property and start all searches from
        /// the beginning of the string.
        /// When the exec() or test() method finds a match and the g
        /// (global) flag is set to true for the regular expression, the method
        /// automatically sets the lastIndex property to the index position of the character
        /// after the last character in the matching substring of the last match. If the
        /// g (global) flag is set to false, the method does not
        /// set the lastIndexproperty.You can set the lastIndex property to adjust the starting position
        /// in the string for regular expression matching.
        /// </summary>
        public extern virtual int lastIndex
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
        /// Specifies whether the dot character (.) in a regular expression pattern matches
        /// new-line characters. Use the s flag when constructing
        /// a regular expression to set dotall = true.
        /// </summary>
        public extern virtual bool dotall
        {
            [PageFX.AbcInstanceTrait(6)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Specifies whether to use extended mode for the regular expression.
        /// When a RegExp object is in extended mode, white space characters in the constructor
        /// string are ignored. This is done to allow more readable constructors.
        /// Use the x flag when constructing a regular expression to set
        /// extended = true.
        /// </summary>
        public extern virtual bool extended
        {
            [PageFX.AbcInstanceTrait(7)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern RegExp(object pattern, object options);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern RegExp(object pattern);

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
        /// <param name="str">The string to search.</param>
        /// <returns>
        /// If there is no match, null; otherwise, an object with the following properties:
        /// An array, in which element 0 contains the complete matching substring, and
        /// other elements of the array (1 through n) contain substrings that match parenthetical groups
        /// in the regular expression index  The character position of the matched substring within
        /// the stringinput  The string (str)
        /// </returns>
        [PageFX.AbcInstanceTrait(8)]
        [PageFX.ABC]
        [PageFX.QName("exec", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual object exec(Avm.String str);

        [PageFX.AbcInstanceTrait(8)]
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
        /// <param name="str">The string to test.</param>
        /// <returns>If there is a match, true; otherwise, false.</returns>
        [PageFX.AbcInstanceTrait(9)]
        [PageFX.ABC]
        [PageFX.QName("test", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool test(Avm.String str);

        [PageFX.AbcInstanceTrait(9)]
        [PageFX.ABC]
        [PageFX.QName("test", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern bool test();
    }
}
