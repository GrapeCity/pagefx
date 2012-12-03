using System;
using System.Runtime.CompilerServices;

namespace flash.sampler
{
    [PageFX.ABC]
    [PageFX.FP9]
    public class NewObjectSample : Sample
    {
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.Class type;

        [PageFX.ABC]
        [PageFX.FP9]
        public static double id;

        public extern virtual object @object
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern NewObjectSample();


    }
}
