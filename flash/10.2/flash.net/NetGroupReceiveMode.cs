using System;
using System.Runtime.CompilerServices;

namespace flash.net
{
    [PageFX.AbcInstance(138)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public partial class NetGroupReceiveMode : Avm.Object
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String EXACT;

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String NEAREST;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern NetGroupReceiveMode();
    }
}
