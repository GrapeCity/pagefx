using System;
using System.Runtime.CompilerServices;

namespace flash.errors
{
    /// <summary>
    /// The IOError exception is thrown when some type of input or output failure occurs.
    /// For example, an IOError exception is thrown if a read/write operation is attempted on
    /// a socket that has not connected or that has become disconnected.
    /// </summary>
    [PageFX.AbcInstance(47)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class IOError : Avm.Error
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern IOError(Avm.String message, int id);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern IOError(Avm.String message);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern IOError();
    }
}
