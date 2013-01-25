using System;
using System.Runtime.CompilerServices;

namespace flash.net
{
    [PageFX.AbcInstance(163)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public partial class NetStreamAppendBytesAction : Avm.Object
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String RESET_BEGIN;

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String RESET_SEEK;

        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String END_SEQUENCE;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern NetStreamAppendBytesAction();
    }
}
