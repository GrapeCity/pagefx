using System;
using System.Runtime.CompilerServices;

namespace adobe.utils
{
    [PageFX.GlobalFunctions]
    public class Global
    {
        [PageFX.AbcScript(28)]
        [PageFX.AbcScriptTrait(0)]
        [PageFX.ABC]
        [PageFX.QName("MMExecute", "adobe.utils", "package")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static Avm.String MMExecute(Avm.String name);

        [PageFX.AbcScript(28)]
        [PageFX.AbcScriptTrait(1)]
        [PageFX.ABC]
        [PageFX.QName("MMEndCommand", "adobe.utils", "package")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void MMEndCommand(bool endStatus, Avm.String notifyString);
    }
}
