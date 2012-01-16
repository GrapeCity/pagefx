using System;
using System.Runtime.CompilerServices;

namespace flash.text
{
    [PageFX.AbcInstance(228)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class TextRun : Avm.Object
    {
        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public int beginIndex;

        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        public int endIndex;

        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        public flash.text.TextFormat textFormat;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextRun(int beginIndex, int endIndex, flash.text.TextFormat textFormat);
    }
}
