using System;
using System.Runtime.CompilerServices;

namespace Avm
{
    /// <summary>
    /// The DefinitionError class represents an error that occurs when user code
    /// attempts to define an identifier that is already defined. This error commonly
    /// occurs in redefining classes, interfaces,
    /// and functions.
    /// </summary>
    [PageFX.AbcInstance(36)]
    [PageFX.ABC]
    [PageFX.QName("DefinitionError")]
    [PageFX.FP9]
    public class DefinitionError : Avm.Error
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DefinitionError(Avm.String message, int id);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DefinitionError(Avm.String message);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DefinitionError();
    }
}
