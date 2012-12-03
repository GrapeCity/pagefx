using System;
using System.Runtime.CompilerServices;

namespace Avm
{
    /// <summary>
    /// A TypeError exception is thrown when the actual type of an operand is different
    /// from the expected type.
    /// </summary>
    [PageFX.ABC]
    [PageFX.QName("TypeError")]
    [PageFX.FP9]
    public class TypeError : Error
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TypeError(String message, int id);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TypeError(String message);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TypeError();
    }
}
