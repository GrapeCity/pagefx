using System;
using System.Runtime.CompilerServices;

namespace flash.net.drm
{
    [PageFX.AbcInstance(156)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public class LoadVoucherSetting : Avm.Object
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String FORCE_REFRESH;

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String LOCAL_ONLY;

        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String ALLOW_SERVER;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern LoadVoucherSetting();
    }
}
