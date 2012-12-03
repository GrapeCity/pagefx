using System;
using System.Runtime.CompilerServices;

namespace flash.text
{
    [PageFX.ABC]
    [PageFX.FP9]
    public class TextFormatDisplay : Avm.Object
    {
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String INLINE;

        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String BLOCK;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextFormatDisplay();
    }
}
