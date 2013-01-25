using System;
using System.Runtime.CompilerServices;

namespace Avm
{
    /// <summary>
    /// A ReferenceError exception is thrown when a reference to an undefined property is
    /// attempted on a sealed (nondynamic) object. References to undefined variables will
    /// result in ReferenceError exceptions to inform you of potential bugs and help you troubleshoot
    /// application code.
    /// </summary>
    [PageFX.AbcInstance(39)]
    [PageFX.ABC]
    [PageFX.QName("ReferenceError")]
    [PageFX.FP9]
    public partial class ReferenceError : Avm.Error
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ReferenceError(Avm.String message, int id);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ReferenceError(Avm.String message);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ReferenceError();
    }
}
