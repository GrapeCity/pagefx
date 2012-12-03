using System;
using System.Runtime.CompilerServices;

namespace flash.text.engine
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class FontMetrics : Avm.Object
    {
        [PageFX.ABC]
        [PageFX.FP10]
        public double strikethroughThickness;

        [PageFX.ABC]
        [PageFX.FP10]
        public flash.geom.Rectangle emBox;

        [PageFX.ABC]
        [PageFX.FP10]
        public double superscriptScale;

        [PageFX.ABC]
        [PageFX.FP10]
        public double strikethroughOffset;

        [PageFX.ABC]
        [PageFX.FP10]
        public double underlineThickness;

        [PageFX.ABC]
        [PageFX.FP10]
        public double subscriptScale;

        [PageFX.ABC]
        [PageFX.FP10]
        public double superscriptOffset;

        [PageFX.ABC]
        [PageFX.FP10]
        public double subscriptOffset;

        [PageFX.ABC]
        [PageFX.FP10]
        public double underlineOffset;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern FontMetrics(flash.geom.Rectangle arg0, double arg1, double arg2, double arg3, double arg4, double arg5, double arg6, double arg7, double arg8);
    }
}
