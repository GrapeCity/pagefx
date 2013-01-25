using System;
using System.Runtime.CompilerServices;

namespace flash.net
{
    [PageFX.AbcInstance(272)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public partial class NetGroupReplicationStrategy : Avm.Object
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String RAREST_FIRST;

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String LOWEST_FIRST;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern NetGroupReplicationStrategy();
    }
}
