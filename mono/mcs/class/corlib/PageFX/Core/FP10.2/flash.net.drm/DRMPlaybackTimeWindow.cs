using System;
using System.Runtime.CompilerServices;

namespace flash.net.drm
{
    [PageFX.AbcInstance(56)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public class DRMPlaybackTimeWindow : Avm.Object
    {
        public extern virtual uint period
        {
            [PageFX.AbcInstanceTrait(4)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual Avm.Date startDate
        {
            [PageFX.AbcInstanceTrait(5)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual Avm.Date endDate
        {
            [PageFX.AbcInstanceTrait(6)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DRMPlaybackTimeWindow();


    }
}
