using System;
using System.Runtime.CompilerServices;

namespace flash.media
{
    [PageFX.AbcInstance(67)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public partial class VideoStatus : Avm.Object
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String UNAVAILABLE;

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String SOFTWARE;

        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String ACCELERATED;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern VideoStatus();
    }
}
