using System;
using System.Runtime.CompilerServices;

namespace flash.sampler
{
    [PageFX.AbcInstance(20)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class NewObjectSample : flash.sampler.Sample
    {
        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static double id;

        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.Class type;

        public extern virtual object @object
        {
            [PageFX.AbcInstanceTrait(2)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double size
        {
            [PageFX.AbcInstanceTrait(3)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern NewObjectSample();


    }
}
