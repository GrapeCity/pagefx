using System;
using System.Runtime.CompilerServices;

namespace Avm
{
    /// <summary>
    /// The uint class provides methods for working with a data type representing a 32-bit
    /// unsigned integer. Because an unsigned integer can only be positive, its maximum
    /// value is twice that of the int class.
    /// </summary>
    [PageFX.AbcInstance(7)]
    [PageFX.ABC]
    [PageFX.QName("uint")]
    [PageFX.FP9]
    public partial class UInt : Avm.Object
    {
        /// <summary>The smallest representable unsigned integer, which is 0.</summary>
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint MIN_VALUE;

        /// <summary>The largest representable 32-bit unsigned integer, which is 4,294,967,295.</summary>
        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint MAX_VALUE;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern UInt(object value);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern UInt();

        /// <summary>Returns the string representation of a uint object.</summary>
        /// <param name="radix">
        /// Specifies the numeric base (from 2 to 36) to use for the number-to-string conversion.
        /// If you do not specify the radix parameter, the default value is 10.
        /// </param>
        /// <returns>
        /// The string representation
        /// of the uint object.
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

        /// <summary>Returns the primitive uint type value of the specified uint object.</summary>
        /// <returns>
        /// The primitive uint type value
        /// of this uint object.
        /// </returns>
        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.QName("valueOf", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual uint valueOf();

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
