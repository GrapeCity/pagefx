using System;
using System.Runtime.CompilerServices;

namespace flash.sampler
{
    [PageFX.AbcInstance(19)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class Sample : Avm.Object
    {
        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static double time;

        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.Array stack;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Sample();
    }
}
