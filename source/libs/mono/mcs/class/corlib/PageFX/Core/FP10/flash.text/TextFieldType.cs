using System;
using System.Runtime.CompilerServices;

namespace flash.text
{
    /// <summary>
    /// The TextFieldType class is an enumeration of constant values used in setting the type  property
    /// of the TextField class.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class TextFieldType : Avm.Object
    {
        /// <summary>Used to specify an input TextField.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String INPUT;

        /// <summary>Used to specify a dynamic TextField.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String DYNAMIC;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextFieldType();
    }
}
