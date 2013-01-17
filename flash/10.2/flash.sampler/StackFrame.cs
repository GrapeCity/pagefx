using System;
using System.Runtime.CompilerServices;

namespace flash.sampler
{
    [PageFX.AbcInstance(18)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public class StackFrame : Avm.Object
    {
        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String name;

        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String file;

        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint line;

        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static double scriptID;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern StackFrame();

        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String toString();
    }
}
