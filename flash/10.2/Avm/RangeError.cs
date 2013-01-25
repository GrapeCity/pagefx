using System;
using System.Runtime.CompilerServices;

namespace Avm
{
    /// <summary>
    /// A RangeError exception is thrown when a numeric value is outside the acceptable range. When working with arrays,
    /// referring to an index position of an array item that does not exist will throw a RangeError exception.
    /// Using Number.toExponential() , Number.toPrecision() , and Number.toFixed()  methods
    /// will throw a RangeError exception in cases
    /// where the arguments are outside the acceptable range of numbers. You can extend Number.toExponential() ,
    /// Number.toPrecision() , and Number.toFixed()  to avoid throwing a RangeError.
    /// </summary>
    [PageFX.AbcInstance(38)]
    [PageFX.ABC]
    [PageFX.QName("RangeError")]
    [PageFX.FP9]
    public partial class RangeError : Avm.Error
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern RangeError(Avm.String message, int id);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern RangeError(Avm.String message);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern RangeError();
    }
}
