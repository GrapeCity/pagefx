using System;
using System.Runtime.CompilerServices;

namespace flash.net
{
    [PageFX.GlobalFunctions]
    public class Global
    {
        [PageFX.ABC]
        [PageFX.QName("navigateToURL", "flash.net", "package")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void navigateToURL(URLRequest arg0, Avm.String arg1);

        [PageFX.ABC]
        [PageFX.QName("navigateToURL", "flash.net", "package")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void navigateToURL(URLRequest arg0);

        [PageFX.ABC]
        [PageFX.QName("getClassByAlias", "flash.net", "package")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static Avm.Class getClassByAlias(Avm.String arg0);

        [PageFX.ABC]
        [PageFX.QName("registerClassAlias", "flash.net", "package")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void registerClassAlias(Avm.String arg0, Avm.Class arg1);

        [PageFX.ABC]
        [PageFX.QName("sendToURL", "flash.net", "package")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void sendToURL(URLRequest arg0);
    }
}
