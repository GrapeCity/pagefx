using System;
using System.Runtime.CompilerServices;

namespace flash.desktop
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class ClipboardTransferMode : Avm.Object
    {
        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String ORIGINAL_PREFERRED;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String CLONE_ONLY;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String ORIGINAL_ONLY;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String CLONE_PREFERRED;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ClipboardTransferMode();
    }
}
