using System;
using System.Runtime.CompilerServices;

namespace Avm
{
    /// <summary>
    /// The ArgumentError class represents an error that occurs when the arguments supplied
    /// in a function do not match the arguments defined for that function. This error occurs,
    /// for example, when a function is called with the wrong number of arguments, an argument
    /// of the incorrect type, or an invalid argument.
    /// </summary>
    [PageFX.AbcInstance(46)]
    [PageFX.ABC]
    [PageFX.QName("ArgumentError")]
    [PageFX.FP9]
    public class ArgumentError : Avm.Error
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ArgumentError(Avm.String message, int id);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ArgumentError(Avm.String message);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ArgumentError();
    }
}
