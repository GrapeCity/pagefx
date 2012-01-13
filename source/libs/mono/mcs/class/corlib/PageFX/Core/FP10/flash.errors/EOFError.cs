using System;
using System.Runtime.CompilerServices;

namespace flash.errors
{
    /// <summary>
    /// An EOFError exception is thrown when you attempt to read past the end of the available data. For example, an
    /// EOFError is thrown when one of the read methods in the IDataInput interface is
    /// called and there is insufficient data to satisfy the read request.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class EOFError : IOError
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern EOFError(Avm.String arg0, int arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern EOFError(Avm.String arg0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern EOFError();
    }
}
