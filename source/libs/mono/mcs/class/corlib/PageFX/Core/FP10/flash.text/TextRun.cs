using System;
using System.Runtime.CompilerServices;

namespace flash.text
{
    [PageFX.ABC]
    [PageFX.FP9]
    public class TextRun : Avm.Object
    {
        [PageFX.ABC]
        [PageFX.FP9]
        public TextFormat textFormat;

        [PageFX.ABC]
        [PageFX.FP9]
        public int endIndex;

        [PageFX.ABC]
        [PageFX.FP9]
        public int beginIndex;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextRun(int arg0, int arg1, TextFormat arg2);
    }
}
