using System;
using System.Runtime.CompilerServices;

namespace flash.net
{
    [PageFX.AbcInstance(226)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public partial class NetGroupSendMode : Avm.Object
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String NEXT_INCREASING;

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String NEXT_DECREASING;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern NetGroupSendMode();
    }
}
