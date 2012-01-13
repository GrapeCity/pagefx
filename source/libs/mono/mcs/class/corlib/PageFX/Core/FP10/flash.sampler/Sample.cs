using System;
using System.Runtime.CompilerServices;

namespace flash.sampler
{
    [PageFX.ABC]
    [PageFX.FP9]
    public class Sample : Avm.Object
    {
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.Array stack;

        [PageFX.ABC]
        [PageFX.FP9]
        public static double time;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Sample();
    }
}
