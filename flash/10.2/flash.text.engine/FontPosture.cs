using System;
using System.Runtime.CompilerServices;

namespace flash.text.engine
{
    [PageFX.AbcInstance(318)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public partial class FontPosture : Avm.Object
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String NORMAL;

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String ITALIC;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern FontPosture();
    }
}
