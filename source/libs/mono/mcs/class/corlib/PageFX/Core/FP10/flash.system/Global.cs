using System;
using System.Runtime.CompilerServices;

namespace flash.system
{
    [PageFX.GlobalFunctions]
    public class Global
    {
        [PageFX.ABC]
        [PageFX.QName("fscommand", "flash.system", "package")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void fscommand(Avm.String arg0, Avm.String arg1);

        [PageFX.ABC]
        [PageFX.QName("fscommand", "flash.system", "package")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void fscommand(Avm.String arg0);
    }
}
