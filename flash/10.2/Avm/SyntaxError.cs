using System;
using System.Runtime.CompilerServices;

namespace Avm
{
    /// <summary>
    /// A SyntaxError exception is thrown when a parsing error occurs.
    /// ActionScript throws SyntaxError exceptions when an invalid
    /// regular expression is parsed by the RegExp class.ActionScript throws SyntaxError exceptions when invalid XML is
    /// parsed by the XML class.
    /// </summary>
    [PageFX.AbcInstance(41)]
    [PageFX.ABC]
    [PageFX.QName("SyntaxError")]
    [PageFX.FP9]
    public partial class SyntaxError : Avm.Error
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern SyntaxError(Avm.String message, int id);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern SyntaxError(Avm.String message);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern SyntaxError();
    }
}
