using System;
using System.Runtime.CompilerServices;

namespace flash.text.ime
{
    [PageFX.AbcInstance(354)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public class CompositionAttributeRange : Avm.Object
    {
        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public int relativeStart;

        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public int relativeEnd;

        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public bool selected;

        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public bool converted;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern CompositionAttributeRange(int relativeStart, int relativeEnd, bool selected, bool converted);
    }
}
