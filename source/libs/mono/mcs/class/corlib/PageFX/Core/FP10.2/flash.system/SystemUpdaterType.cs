using System;
using System.Runtime.CompilerServices;

namespace flash.system
{
    [PageFX.AbcInstance(290)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public class SystemUpdaterType : Avm.Object
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String SYSTEM;

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String DRM;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern SystemUpdaterType();
    }
}
