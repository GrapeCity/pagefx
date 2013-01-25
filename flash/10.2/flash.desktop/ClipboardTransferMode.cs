using System;
using System.Runtime.CompilerServices;

namespace flash.desktop
{
    [PageFX.AbcInstance(294)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public partial class ClipboardTransferMode : Avm.Object
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String ORIGINAL_PREFERRED;

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String ORIGINAL_ONLY;

        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String CLONE_PREFERRED;

        [PageFX.AbcClassTrait(3)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String CLONE_ONLY;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ClipboardTransferMode();
    }
}
