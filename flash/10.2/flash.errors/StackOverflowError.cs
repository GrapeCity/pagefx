using System;
using System.Runtime.CompilerServices;

namespace flash.errors
{
    /// <summary>
    /// ActionScript throws a StackOverflowError exception when the stack available to the script
    /// is exhausted. ActionScript uses a stack to store information about each method call made in
    /// a script, such as the local variables that the method uses. The amount of stack space
    /// available varies from system to system.
    /// </summary>
    [PageFX.AbcInstance(84)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class StackOverflowError : Avm.Error
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern StackOverflowError(Avm.String message, int id);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern StackOverflowError(Avm.String message);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern StackOverflowError();
    }
}
