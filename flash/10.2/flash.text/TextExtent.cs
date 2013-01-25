using System;
using System.Runtime.CompilerServices;

namespace flash.text
{
    [PageFX.AbcInstance(301)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class TextExtent : Avm.Object
    {
        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public double width;

        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        public double height;

        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        public double textFieldWidth;

        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        public double textFieldHeight;

        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.FP9]
        public double ascent;

        [PageFX.AbcInstanceTrait(5)]
        [PageFX.ABC]
        [PageFX.FP9]
        public double descent;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextExtent(double width, double height, double textFieldWidth, double textFieldHeight, double ascent, double descent);
    }
}
