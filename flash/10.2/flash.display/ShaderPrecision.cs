using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    [PageFX.AbcInstance(320)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public partial class ShaderPrecision : Avm.Object
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String FULL;

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String FAST;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ShaderPrecision();
    }
}
