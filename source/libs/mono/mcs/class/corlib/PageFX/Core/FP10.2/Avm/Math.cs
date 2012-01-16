using System;
using System.Runtime.CompilerServices;

namespace Avm
{
    /// <summary>
    /// The Math class contains methods and constants that represent common mathematical
    /// functions and values.
    /// </summary>
    [PageFX.AbcInstance(176)]
    [PageFX.ABC]
    [PageFX.QName("Math")]
    [PageFX.FP9]
    public class Math : Avm.Object
    {
        /// <summary>
        /// A mathematical constant for the base of natural logarithms, expressed as e.
        /// The approximate value of eis 2.71828182845905.
        /// </summary>
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static double E;

        /// <summary>
        /// A mathematical constant for the natural logarithm of 10, expressed as log10, with
        /// an approximate value of 2.302585092994046.
        /// </summary>
        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static double LN10;

        /// <summary>
        /// A mathematical constant for the natural logarithm of 2, expressed as log2, with
        /// an approximate value of 0.6931471805599453.
        /// </summary>
        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static double LN2;

        /// <summary>
        /// A mathematical constant for the base-10 logarithm of the constant e (Math.E),
        /// expressed as loge, with an approximate value of 0.4342944819032518.
        /// The Math.log() method computes the natural logarithm of a number. Multiply
        /// the result of Math.log() by Math.LOG10E to obtain the
        /// base-10 logarithm.
        /// </summary>
        [PageFX.AbcClassTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static double LOG10E;

        /// <summary>
        /// A mathematical constant for the base-2 logarithm of the constant e, expressed
        /// as log2e, with an approximate value of 1.442695040888963387.
        /// The Math.log method computes the natural logarithm of a number. Multiply
        /// the result of Math.log() by Math.LOG2E to obtain the base-2
        /// logarithm.
        /// </summary>
        [PageFX.AbcClassTrait(4)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static double LOG2E;

        /// <summary>
        /// A mathematical constant for the ratio of the circumference of a circle to its diameter,
        /// expressed as pi, with a value of 3.141592653589793.
        /// </summary>
        [PageFX.AbcClassTrait(5)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static double PI;

        /// <summary>
        /// A mathematical constant for the square root of one-half, with an approximate value
        /// of 0.7071067811865476.
        /// </summary>
        [PageFX.AbcClassTrait(6)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static double SQRT1_2;

        /// <summary>A mathematical constant for the square root of 2, with an approximate value of 1.4142135623730951.</summary>
        [PageFX.AbcClassTrait(7)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static double SQRT2;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Math();

        /// <summary>
        /// Computes and returns an absolute value for the number specified by the parameter
        /// val.
        /// </summary>
        /// <param name="val">The number whose absolute value is returned.</param>
        /// <returns>
        /// The absolute value of the
        /// specified paramater.
        /// </returns>
        [PageFX.AbcClassTrait(10)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double abs(double val);

        /// <summary>
        /// Computes and returns the arc cosine of the number specified in the parameter val,
        /// in radians.
        /// </summary>
        /// <param name="val">A number from -1.0 to 1.0.</param>
        /// <returns>
        /// The arc cosine of the parameter
        /// val.
        /// </returns>
        [PageFX.AbcClassTrait(11)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double acos(double val);

        /// <summary>
        /// Computes and returns the arc sine for the number specified in the parameter val,
        /// in radians.
        /// </summary>
        /// <param name="val">A number from -1.0 to 1.0.</param>
        /// <returns>
        /// A number between negative
        /// pi divided by 2 and positive pi divided by 2.
        /// </returns>
        [PageFX.AbcClassTrait(12)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double asin(double val);

        /// <summary>
        /// Computes and returns the value, in radians, of the angle whose tangent is specified
        /// in the parameter val. The return value is between negative pi divided
        /// by 2 and positive pi divided by 2.
        /// </summary>
        /// <param name="val">A number that represents the tangent of an angle.</param>
        /// <returns>
        /// A number between negative
        /// pi divided by 2 and positive pi divided by 2.
        /// </returns>
        [PageFX.AbcClassTrait(13)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double atan(double val);

        /// <summary>
        /// Returns the ceiling of the specified number or expression. The ceiling of a number
        /// is the closest integer that is greater than or equal to the number.
        /// </summary>
        /// <param name="val">A number or expression.</param>
        /// <returns>
        /// An integer that is both closest
        /// to, and greater than or equal to, the parameter val.
        /// </returns>
        [PageFX.AbcClassTrait(14)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double ceil(double val);

        /// <summary>
        /// Computes and returns the cosine of the specified angle in radians. To calculate
        /// a radian, see the overview of the Math class.
        /// </summary>
        /// <param name="angleRadians">A number that represents an angle measured in radians.</param>
        /// <returns>A number from -1.0 to 1.0.</returns>
        [PageFX.AbcClassTrait(15)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double cos(double angleRadians);

        /// <summary>
        /// Returns the value of the base of the natural logarithm (e), to the power
        /// of the exponent specified in the parameter x. The constant Math.E
        /// can provide the value of e.
        /// </summary>
        /// <param name="val">The exponent; a number or expression.</param>
        /// <returns>
        /// e to the power of the
        /// parameter val.
        /// </returns>
        [PageFX.AbcClassTrait(16)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double exp(double val);

        /// <summary>
        /// Returns the floor of the number or expression specified in the parameter val.
        /// The floor is the closest integer that is less than or equal to the specified number
        /// or expression.
        /// </summary>
        /// <param name="val">A number or expression.</param>
        /// <returns>
        /// The integer that is both closest
        /// to, and less than or equal to, the parameter val.
        /// </returns>
        [PageFX.AbcClassTrait(17)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double floor(double val);

        /// <summary>Returns the natural logarithm of the parameter val.</summary>
        /// <param name="val">A number or expression with a value greater than 0.</param>
        /// <returns>
        /// The natural logarithm of parameter
        /// val.
        /// </returns>
        [PageFX.AbcClassTrait(18)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double log(double val);

        /// <summary>
        /// Rounds the value of the parameter val up or down to the nearest integer
        /// and returns the value. If val is equidistant from its two nearest integers
        /// (that is, if the number ends in .5), the value is rounded up to the next higher
        /// integer.
        /// </summary>
        /// <param name="val">The number to round.</param>
        /// <returns>
        /// The parameter val
        /// rounded to the nearest whole number.
        /// </returns>
        [PageFX.AbcClassTrait(19)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double round(double val);

        /// <summary>
        /// Computes and returns the sine of the specified angle in radians. To calculate a
        /// radian, see the overview of the Math class.
        /// </summary>
        /// <param name="angleRadians">A number that represents an angle measured in radians.</param>
        /// <returns>
        /// A number; the sine of the
        /// specified angle (between -1.0 and 1.0).
        /// </returns>
        [PageFX.AbcClassTrait(20)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double sin(double angleRadians);

        /// <summary>Computes and returns the square root of the specified number.</summary>
        /// <param name="val">A number or expression greater than or equal to 0.</param>
        /// <returns>
        /// If the parameter val
        /// is greater than or equal to zero, a number; otherwise NaN (not a number).
        /// </returns>
        [PageFX.AbcClassTrait(21)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double sqrt(double val);

        /// <summary>
        /// Computes and returns the tangent of the specified angle. To calculate a radian,
        /// see the overview of the Math class.
        /// </summary>
        /// <param name="angleRadians">A number that represents an angle measured in radians.</param>
        /// <returns>
        /// The tangent of the parameter
        /// angleRadians.
        /// </returns>
        [PageFX.AbcClassTrait(22)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double tan(double angleRadians);

        /// <summary>
        /// Computes and returns the angle of the point y/x in radians,
        /// when measured counterclockwise from a circle&apos;s x axis (where 0,0 represents
        /// the center of the circle). The return value is between positive pi and negative
        /// pi. Note that the first parameter to atan2 is always the y coordinate.
        /// </summary>
        /// <param name="y">The y coordinate of the point.</param>
        /// <param name="x">The x coordinate of the point.</param>
        /// <returns>A number.</returns>
        [PageFX.AbcClassTrait(23)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double atan2(double y, double x);

        /// <summary>Computes and returns val1 to the power of val2.</summary>
        /// <param name="val1">A number to be raised by the power of the parameter val2.</param>
        /// <param name="val2">A number specifying the power that the parameter val2 is raised by.</param>
        /// <returns>
        /// The value of val1
        /// raised to the power of val2.
        /// </returns>
        [PageFX.AbcClassTrait(24)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double pow(double val1, double val2);

        /// <summary>
        /// Evaluates val1 and val2 (or more values) and returns the
        /// largest value.
        /// </summary>
        /// <param name="val1">A number or expression.</param>
        /// <param name="val2">A number or expression.</param>
        /// <returns>
        /// The largest of the parameters
        /// val1 and val2 (or more values).
        /// </returns>
        [PageFX.AbcClassTrait(26)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double max(double val1, double val2);

        [PageFX.AbcClassTrait(26)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double max(double val1, double val2, object rest0);

        [PageFX.AbcClassTrait(26)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double max(double val1, double val2, object rest0, object rest1);

        [PageFX.AbcClassTrait(26)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double max(double val1, double val2, object rest0, object rest1, object rest2);

        [PageFX.AbcClassTrait(26)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double max(double val1, double val2, object rest0, object rest1, object rest2, object rest3);

        [PageFX.AbcClassTrait(26)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double max(double val1, double val2, object rest0, object rest1, object rest2, object rest3, object rest4);

        [PageFX.AbcClassTrait(26)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double max(double val1, double val2, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5);

        [PageFX.AbcClassTrait(26)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double max(double val1, double val2, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6);

        [PageFX.AbcClassTrait(26)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double max(double val1, double val2, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7);

        [PageFX.AbcClassTrait(26)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double max(double val1, double val2, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8);

        [PageFX.AbcClassTrait(26)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double max(double val1, double val2, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8, object rest9);

        [PageFX.AbcClassTrait(26)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double max(double val1);

        [PageFX.AbcClassTrait(26)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double max();

        /// <summary>
        /// Evaluates val1 and val2 (or more values) and returns the
        /// smallest value.
        /// </summary>
        /// <param name="val1">A number or expression.</param>
        /// <param name="val2">A number or expression.</param>
        /// <returns>
        /// The smallest of the parameters
        /// val1 and val2 (or more values).
        /// </returns>
        [PageFX.AbcClassTrait(27)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double min(double val1, double val2);

        [PageFX.AbcClassTrait(27)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double min(double val1, double val2, object rest0);

        [PageFX.AbcClassTrait(27)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double min(double val1, double val2, object rest0, object rest1);

        [PageFX.AbcClassTrait(27)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double min(double val1, double val2, object rest0, object rest1, object rest2);

        [PageFX.AbcClassTrait(27)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double min(double val1, double val2, object rest0, object rest1, object rest2, object rest3);

        [PageFX.AbcClassTrait(27)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double min(double val1, double val2, object rest0, object rest1, object rest2, object rest3, object rest4);

        [PageFX.AbcClassTrait(27)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double min(double val1, double val2, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5);

        [PageFX.AbcClassTrait(27)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double min(double val1, double val2, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6);

        [PageFX.AbcClassTrait(27)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double min(double val1, double val2, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7);

        [PageFX.AbcClassTrait(27)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double min(double val1, double val2, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8);

        [PageFX.AbcClassTrait(27)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double min(double val1, double val2, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8, object rest9);

        [PageFX.AbcClassTrait(27)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double min(double val1);

        [PageFX.AbcClassTrait(27)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double min();

        /// <summary>
        /// Returns a pseudo-random number n, where 0 &lt;= n &lt; 1. The number returned is
        /// calculated in an undisclosed manner, and pseudo-random because the calculation inevitably
        /// contains some element of non-randomness.
        /// </summary>
        /// <returns>A pseudo-random number.</returns>
        [PageFX.AbcClassTrait(28)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double random();
    }
}
