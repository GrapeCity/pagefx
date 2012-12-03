using System;
using System.Runtime.CompilerServices;

namespace flash.net.drm
{
    [PageFX.AbcInstance(155)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public class AuthenticationMethod : Avm.Object
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String ANONYMOUS;

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String USERNAME_AND_PASSWORD;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern AuthenticationMethod();
    }
}
