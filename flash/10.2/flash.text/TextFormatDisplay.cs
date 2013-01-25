using System;
using System.Runtime.CompilerServices;

namespace flash.text
{
    [PageFX.AbcInstance(286)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class TextFormatDisplay : Avm.Object
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String INLINE;

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String BLOCK;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextFormatDisplay();
    }
}
