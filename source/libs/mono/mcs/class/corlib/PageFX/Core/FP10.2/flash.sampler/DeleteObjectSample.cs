using System;
using System.Runtime.CompilerServices;

namespace flash.sampler
{
    [PageFX.AbcInstance(21)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class DeleteObjectSample : flash.sampler.Sample
    {
        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static double id;

        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static double size;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DeleteObjectSample();
    }
}
