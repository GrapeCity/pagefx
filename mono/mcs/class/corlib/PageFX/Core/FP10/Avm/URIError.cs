using System;
using System.Runtime.CompilerServices;

namespace Avm
{
    /// <summary>
    /// A URIError exception is thrown when one of the global URI handling functions is
    /// used in a way that is incompatible with its definition. This exception is thrown
    /// when an invalid URI is specified to a function that expects a valid URI, such as
    /// the Socket.connect()  method.
    /// </summary>
    [PageFX.ABC]
    [PageFX.QName("URIError")]
    [PageFX.FP9]
    public class URIError : Error
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern URIError(String message, int id);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern URIError(String message);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern URIError();
    }
}
