using System;
using System.Runtime.CompilerServices;

namespace flash.net
{
    /// <summary>
    /// The Responder class provides an object that is used
    /// in NetConnection.call()  to handle return
    /// values from the server.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class Responder : Avm.Object
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Responder(Avm.Function arg0, Avm.Function arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Responder(Avm.Function arg0);
    }
}
