using System;
using System.Runtime.CompilerServices;

namespace flash.media
{
    [PageFX.GlobalFunctions]
    public class Global
    {
        [PageFX.AbcScript(108)]
        [PageFX.AbcScriptTrait(0)]
        [PageFX.ABC]
        [PageFX.QName("scanHardware", "flash.media", "package")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void scanHardware();
    }
}
