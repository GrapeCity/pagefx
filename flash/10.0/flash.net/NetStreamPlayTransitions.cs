using System;
using System.Runtime.CompilerServices;

namespace flash.net
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class NetStreamPlayTransitions : Avm.Object
    {
        [PageFX.ABC]
        [PageFX.FP10]
        public static object SWAP;

        [PageFX.ABC]
        [PageFX.FP10]
        public static object SWITCH;

        [PageFX.ABC]
        [PageFX.FP10]
        public static object STOP;

        [PageFX.ABC]
        [PageFX.FP10]
        public static object APPEND;

        [PageFX.ABC]
        [PageFX.FP10]
        public static object RESET;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern NetStreamPlayTransitions();
    }
}
