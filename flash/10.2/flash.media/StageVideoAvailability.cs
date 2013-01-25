using System;
using System.Runtime.CompilerServices;

namespace flash.media
{
    [PageFX.AbcInstance(263)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public partial class StageVideoAvailability : Avm.Object
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String AVAILABLE;

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String UNAVAILABLE;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern StageVideoAvailability();
    }
}
