using System;
using System.Runtime.CompilerServices;

namespace flash.errors
{
    /// <summary>
    /// The ScriptTimeoutError exception is thrown when the script timeout interval is reached.
    /// The script timeout interval is 15 seconds.
    /// </summary>
    [PageFX.AbcInstance(85)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class ScriptTimeoutError : Avm.Error
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ScriptTimeoutError(Avm.String message, int id);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ScriptTimeoutError(Avm.String message);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ScriptTimeoutError();
    }
}
