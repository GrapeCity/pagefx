using System;
using System.Runtime.CompilerServices;

namespace flash.text
{
    [PageFX.ABC]
    [PageFX.FP9]
    public class TextExtent : Avm.Object
    {
        [PageFX.ABC]
        [PageFX.FP9]
        public double ascent;

        [PageFX.ABC]
        [PageFX.FP9]
        public double width;

        [PageFX.ABC]
        [PageFX.FP9]
        public double height;

        [PageFX.ABC]
        [PageFX.FP9]
        public double textFieldWidth;

        [PageFX.ABC]
        [PageFX.FP9]
        public double descent;

        [PageFX.ABC]
        [PageFX.FP9]
        public double textFieldHeight;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextExtent(double arg0, double arg1, double arg2, double arg3, double arg4, double arg5);
    }
}
