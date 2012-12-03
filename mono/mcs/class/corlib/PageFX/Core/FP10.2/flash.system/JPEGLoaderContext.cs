using System;
using System.Runtime.CompilerServices;

namespace flash.system
{
    [PageFX.AbcInstance(144)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public class JPEGLoaderContext : flash.system.LoaderContext
    {
        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public double deblockingFilter;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern JPEGLoaderContext(double deblockingFilter, bool checkPolicyFile, flash.system.ApplicationDomain applicationDomain, flash.system.SecurityDomain securityDomain);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern JPEGLoaderContext(double deblockingFilter, bool checkPolicyFile, flash.system.ApplicationDomain applicationDomain);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern JPEGLoaderContext(double deblockingFilter, bool checkPolicyFile);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern JPEGLoaderContext(double deblockingFilter);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern JPEGLoaderContext();
    }
}
