using System;
using System.Runtime.CompilerServices;

namespace flash.net
{
    [PageFX.AbcInstance(165)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public class NetGroupSendResult : Avm.Object
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String ERROR;

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String NO_ROUTE;

        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String SENT;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern NetGroupSendResult();
    }
}
