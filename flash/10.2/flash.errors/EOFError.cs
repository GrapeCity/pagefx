using System;
using System.Runtime.CompilerServices;

namespace flash.errors
{
    /// <summary>
    /// An EOFError exception is thrown when you attempt to read past the end of the available data. For example, an
    /// EOFError is thrown when one of the read methods in the IDataInput interface is
    /// called and there is insufficient data to satisfy the read request.
    /// </summary>
    [PageFX.AbcInstance(48)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class EOFError : flash.errors.IOError
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern EOFError(Avm.String message, int id);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern EOFError(Avm.String message);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern EOFError();
    }
}
