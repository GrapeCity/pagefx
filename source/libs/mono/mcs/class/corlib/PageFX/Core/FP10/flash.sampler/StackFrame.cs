using System;
using System.Runtime.CompilerServices;

namespace flash.sampler
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class StackFrame : Avm.Object
    {
        [PageFX.ABC]
        [PageFX.FP10]
        public static uint line;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String name;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String file;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern StackFrame();

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String toString();
    }
}
