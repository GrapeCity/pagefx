using System;
using System.Runtime.CompilerServices;

namespace flash.net.drm
{
    [PageFX.AbcInstance(372)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public class VoucherAccessInfo : Avm.Object
    {
        public extern virtual Avm.String displayName
        {
            [PageFX.AbcInstanceTrait(0)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual Avm.String authenticationMethod
        {
            [PageFX.AbcInstanceTrait(1)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual Avm.String domain
        {
            [PageFX.AbcInstanceTrait(2)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern VoucherAccessInfo();


    }
}
