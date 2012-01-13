using System;
using System.Runtime.CompilerServices;

namespace flash.media
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class SoundCodec : Avm.Object
    {
        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String SPEEX;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String NELLYMOSER;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern SoundCodec();
    }
}
