using System;
using System.Runtime.CompilerServices;

namespace adobe.utils
{
    [PageFX.GlobalFunctions]
    public class Global
    {
        [PageFX.ABC]
        [PageFX.QName("MMEndCommand", "adobe.utils", "package")]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void MMEndCommand(bool arg0, Avm.String arg1);

        [PageFX.ABC]
        [PageFX.QName("MMExecute", "adobe.utils", "package")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static Avm.String MMExecute(Avm.String arg0);
    }
}
