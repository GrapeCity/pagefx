using System;
using System.Runtime.CompilerServices;

namespace flash.text.engine
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class FontLookup : Avm.Object
    {
        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String EMBEDDED_CFF;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String DEVICE;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern FontLookup();
    }
}
