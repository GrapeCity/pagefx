using System;
using System.Runtime.CompilerServices;

namespace flash.media
{
    [PageFX.AbcInstance(190)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public class SoundCodec : Avm.Object
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String NELLYMOSER;

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String SPEEX;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern SoundCodec();
    }
}
