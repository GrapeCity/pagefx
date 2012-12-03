using System;
using System.Runtime.CompilerServices;

namespace flash.system
{
    [PageFX.GlobalFunctions]
    public class Global
    {
        [PageFX.AbcScript(27)]
        [PageFX.AbcScriptTrait(0)]
        [PageFX.ABC]
        [PageFX.QName("fscommand", "flash.system", "package")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void fscommand(Avm.String command, Avm.String args);

        [PageFX.AbcScript(27)]
        [PageFX.AbcScriptTrait(0)]
        [PageFX.ABC]
        [PageFX.QName("fscommand", "flash.system", "package")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void fscommand(Avm.String command);
    }
}
