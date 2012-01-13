using System;
using System.Runtime.CompilerServices;

namespace flash.sampler
{
    [PageFX.ABC]
    [PageFX.FP9]
    public class DeleteObjectSample : Sample
    {
        [PageFX.ABC]
        [PageFX.FP9]
        public static double size;

        [PageFX.ABC]
        [PageFX.FP9]
        public static double id;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DeleteObjectSample();
    }
}
