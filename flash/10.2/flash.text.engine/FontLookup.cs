using System;
using System.Runtime.CompilerServices;

namespace flash.text.engine
{
    [PageFX.AbcInstance(123)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public partial class FontLookup : Avm.Object
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String DEVICE;

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String EMBEDDED_CFF;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern FontLookup();
    }
}
