using System;
using System.Runtime.CompilerServices;

namespace flash.net
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class NetStreamPlayOptions : flash.events.EventDispatcher
    {
        [PageFX.ABC]
        [PageFX.FP10]
        public Avm.String oldStreamName;

        [PageFX.ABC]
        [PageFX.FP10]
        public double len;

        [PageFX.ABC]
        [PageFX.FP10]
        public double start;

        [PageFX.ABC]
        [PageFX.FP10]
        public Avm.String streamName;

        [PageFX.ABC]
        [PageFX.FP10]
        public Avm.String transition;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern NetStreamPlayOptions();
    }
}
