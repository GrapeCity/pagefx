using System;
using System.Runtime.CompilerServices;

namespace flash.net
{
    /// <summary>
    /// The Responder class provides an object that is used
    /// in NetConnection.call()  to handle return
    /// values from the server.
    /// </summary>
    [PageFX.AbcInstance(27)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class Responder : Avm.Object
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Responder(Avm.Function result, Avm.Function status);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Responder(Avm.Function result);
    }
}
