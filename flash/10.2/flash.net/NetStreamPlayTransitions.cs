using System;
using System.Runtime.CompilerServices;

namespace flash.net
{
    [PageFX.AbcInstance(325)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public partial class NetStreamPlayTransitions : Avm.Object
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String APPEND;

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String RESET;

        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String SWITCH;

        [PageFX.AbcClassTrait(3)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String SWAP;

        [PageFX.AbcClassTrait(4)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STOP;

        [PageFX.AbcClassTrait(5)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String RESUME;

        [PageFX.AbcClassTrait(6)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String APPEND_AND_WAIT;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern NetStreamPlayTransitions();
    }
}
