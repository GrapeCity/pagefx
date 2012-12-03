using System;
using System.Runtime.CompilerServices;

namespace Avm
{
    /// <summary>
    /// The EvalError class represents an error that occurs when user code
    /// calls the eval()  function or attempts to use the new
    /// operator with the Function object. Calling eval()  and
    /// calling new  with the Function object are not supported.
    /// </summary>
    [PageFX.AbcInstance(37)]
    [PageFX.ABC]
    [PageFX.QName("EvalError")]
    [PageFX.FP9]
    public class EvalError : Avm.Error
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern EvalError(Avm.String message, int id);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern EvalError(Avm.String message);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern EvalError();
    }
}
