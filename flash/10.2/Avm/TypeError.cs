using System;
using System.Runtime.CompilerServices;

namespace Avm
{
    /// <summary>
    /// A TypeError exception is thrown when the actual type of an operand is different
    /// from the expected type.
    /// </summary>
    [PageFX.AbcInstance(42)]
    [PageFX.ABC]
    [PageFX.QName("TypeError")]
    [PageFX.FP9]
    public partial class TypeError : Avm.Error
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TypeError(Avm.String message, int id);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TypeError(Avm.String message);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TypeError();
    }
}
