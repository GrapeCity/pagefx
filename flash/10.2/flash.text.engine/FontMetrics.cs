using System;
using System.Runtime.CompilerServices;

namespace flash.text.engine
{
    [PageFX.AbcInstance(305)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public partial class FontMetrics : Avm.Object
    {
        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public flash.geom.Rectangle emBox;

        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public double strikethroughOffset;

        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public double strikethroughThickness;

        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public double underlineOffset;

        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public double underlineThickness;

        [PageFX.AbcInstanceTrait(5)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public double subscriptOffset;

        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public double subscriptScale;

        [PageFX.AbcInstanceTrait(7)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public double superscriptOffset;

        [PageFX.AbcInstanceTrait(8)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public double superscriptScale;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern FontMetrics(flash.geom.Rectangle emBox, double strikethroughOffset, double strikethroughThickness, double underlineOffset, double underlineThickness, double subscriptOffset, double subscriptScale, double superscriptOffset, double superscriptScale);
    }
}
