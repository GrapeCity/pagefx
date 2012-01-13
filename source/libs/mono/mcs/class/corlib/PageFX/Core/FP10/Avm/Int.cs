using System;
using System.Runtime.CompilerServices;

namespace Avm
{
    /// <summary>
    /// The int class lets you work with the data type representing a 32-bit signed integer.
    /// The range of values represented by the int class is -2,147,483,648 (-2^31) to 2,147,483,647
    /// (2^31-1).
    /// </summary>
    [PageFX.ABC]
    [PageFX.QName("int")]
    [PageFX.FP9]
    public class Int : Object
    {
        /// <summary>The smallest representable 32-bit signed integer, which is -2,147,483,648.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static int MIN_VALUE;

        /// <summary>The largest representable 32-bit signed integer, which is 2,147,483,647.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static int MAX_VALUE;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Int(object arg0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Int();

        /// <summary>
        /// Returns a string representation of the number either in exponential notation or
        /// in fixed-point notation. The string will contain the number of digits specified
        /// in the precision parameter.
        /// </summary>
        /// <param name="arg0">
        /// An integer between 1 and 21, inclusive, that represents the desired number of digits
        /// to represent in the resulting string.
        /// </param>
        /// <returns>String</returns>
        [PageFX.ABC]
        [PageFX.QName("toPrecision", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual String toPrecision(object arg0);

        [PageFX.ABC]
        [PageFX.QName("toPrecision", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern String toPrecision();

        /// <summary>Returns the primitive value of the specified int object.</summary>
        /// <returns>An int value.</returns>
        [PageFX.ABC]
        [PageFX.QName("valueOf", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int valueOf();

        /// <summary>
        /// Returns a string representation of the number in fixed-point notation. Fixed-point
        /// notation means that the string will contain a specific number of digits after the
        /// decimal point, as specified in the fractionDigits parameter. The valid
        /// range for the fractionDigits parameter is from 0 to 20. Specifying
        /// a value outside this range throws an exception.
        /// </summary>
        /// <param name="arg0">
        /// An integer between 0 and 20, inclusive, that represents the desired number
        /// of decimal places.
        /// </param>
        /// <returns>String</returns>
        [PageFX.ABC]
        [PageFX.QName("toFixed", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual String toFixed(object arg0);

        [PageFX.ABC]
        [PageFX.QName("toFixed", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern String toFixed();

        /// <summary>
        /// Returns a string representation of the number in exponential notation. The string
        /// contains one digit before the decimal point and up to 20 digits after the decimal
        /// point, as specified by the fractionDigits parameter.
        /// </summary>
        /// <param name="arg0">
        /// An integer between 0 and 20, inclusive, that represents the desired number
        /// of decimal places.
        /// </param>
        /// <returns>String</returns>
        [PageFX.ABC]
        [PageFX.QName("toExponential", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual String toExponential(object arg0);

        [PageFX.ABC]
        [PageFX.QName("toExponential", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern String toExponential();

        /// <summary>Returns the string representation of an int object.</summary>
        /// <param name="arg0">
        /// Specifies the numeric base (from 2 to 36) to use for the number-to-string conversion.
        /// If you do not specify the radix parameter, the default value is 10.
        /// </param>
        /// <returns>A string.</returns>
        [PageFX.ABC]
        [PageFX.QName("toString", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual String toString(object arg0);

        [PageFX.ABC]
        [PageFX.QName("toString", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern String toString();
    }
}
