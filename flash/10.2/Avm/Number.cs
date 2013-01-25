using System;
using System.Runtime.CompilerServices;

namespace Avm
{
    /// <summary>
    /// A data type representing an IEEE-754 double-precision floating-point number. You
    /// can manipulate primitive numeric values by using the methods and properties associated
    /// with the Number class. This class is identical to the JavaScript Number class.
    /// </summary>
    [PageFX.AbcInstance(5)]
    [PageFX.ABC]
    [PageFX.QName("Number")]
    [PageFX.FP9]
    public partial class Number : Avm.Object
    {
        /// <summary>The IEEE-754 value representing Not a Number (NaN).</summary>
        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static double NaN;

        /// <summary>
        /// Specifies the IEEE-754 value representing negative infinity. The value of this property
        /// is the same as that of the constant -Infinity.
        /// Negative infinity is a special numeric value that is returned when a mathematical
        /// operation or function returns a negative value larger than can be represented.
        /// </summary>
        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static double NEGATIVE_INFINITY;

        /// <summary>
        /// Specifies the IEEE-754 value representing positive infinity. The value of this property
        /// is the same as that of the constant Infinity.
        /// Positive infinity is a special numeric value that is returned when a mathematical
        /// operation or function returns a value larger than can be represented.
        /// </summary>
        [PageFX.AbcClassTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static double POSITIVE_INFINITY;

        /// <summary>
        /// The smallest representable non-negative, non-zero, number (double-precision IEEE-754).
        /// This number is approximately 5e-324. The smallest representable number overall is
        /// actually -Number.MAX_VALUE.
        /// </summary>
        [PageFX.AbcClassTrait(4)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static double MIN_VALUE;

        /// <summary>
        /// The largest representable number (double-precision IEEE-754). This number is approximately
        /// 1.79e+308.
        /// </summary>
        [PageFX.AbcClassTrait(5)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static double MAX_VALUE;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Number(object value);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Number();

        /// <summary>
        /// Returns the string representation of the specified Number object (myNumber).
        /// If the value of the Number object is a decimal number without a leading zero (such
        /// as .4), Number.toString() adds a leading zero (0.4).
        /// </summary>
        /// <param name="radix">
        /// (default
        /// = 10)  Specifies the numeric base (from 2 to 36)
        /// to use for the number-to-string conversion. If you do not specify the radix
        /// parameter, the default value is 10.
        /// </param>
        /// <returns>
        /// The numeric representation
        /// of the Number object as a string.
        /// </returns>
        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.QName("toString", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String toString(object radix);

        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.QName("toString", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.String toString();

        /// <summary>Returns the primitive value type of the specified Number object.</summary>
        /// <returns>
        /// The primitive type value of
        /// the Number object.
        /// </returns>
        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.QName("valueOf", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual double valueOf();

        /// <summary>
        /// Returns a string representation of the number in exponential notation. The string
        /// contains one digit before the decimal point and up to 20 digits after the decimal
        /// point, as specified by the fractionDigits parameter.
        /// </summary>
        /// <param name="fractionDigits">
        /// An integer between 0 and 20, inclusive, that represents the desired number
        /// of decimal places.
        /// </param>
        /// <returns>String</returns>
        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.QName("toExponential", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String toExponential(object fractionDigits);

        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.QName("toExponential", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.String toExponential();

        /// <summary>
        /// Returns a string representation of the number either in exponential notation or
        /// in fixed-point notation. The string will contain the number of digits specified
        /// in the precision parameter.
        /// </summary>
        /// <param name="precision">
        /// An integer between 1 and 21, inclusive, that represents the desired number of digits
        /// to represent in the resulting string.
        /// </param>
        /// <returns>String</returns>
        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.QName("toPrecision", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String toPrecision(object precision);

        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.QName("toPrecision", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.String toPrecision();

        /// <summary>
        /// Returns a string representation of the number in fixed-point notation. Fixed-point
        /// notation means that the string will contain a specific number of digits after the
        /// decimal point, as specified in the fractionDigits parameter. The valid
        /// range for the fractionDigits parameter is from 0 to 20. Specifying
        /// a value outside this range throws an exception.
        /// </summary>
        /// <param name="fractionDigits">
        /// An integer between 0 and 20, inclusive, that represents the desired number
        /// of decimal places.
        /// </param>
        /// <returns>String</returns>
        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.QName("toFixed", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String toFixed(object fractionDigits);

        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.QName("toFixed", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.String toFixed();
    }
}
