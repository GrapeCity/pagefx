using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    /// <summary>
    /// The ActionScriptVersion class is an enumeration of constant values that
    /// indicate the language version of a loaded SWF file.
    /// The language version constants are provided for use in checking the
    /// actionScriptVersion  property of a flash.display.LoaderInfo object.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class ActionScriptVersion : Avm.Object
    {
        /// <summary>ActionScript language version 2.0 and earlier.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint ACTIONSCRIPT2;

        /// <summary>ActionScript language version 3.0.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint ACTIONSCRIPT3;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ActionScriptVersion();
    }
}
