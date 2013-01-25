using System;
using System.Runtime.CompilerServices;

namespace flash.net
{
    [PageFX.AbcInstance(235)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public partial class NetStreamPlayOptions : flash.events.EventDispatcher
    {
        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public Avm.String streamName;

        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public Avm.String oldStreamName;

        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public double start;

        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public double len;

        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public double offset;

        [PageFX.AbcInstanceTrait(5)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public Avm.String transition;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern NetStreamPlayOptions();
    }
}
