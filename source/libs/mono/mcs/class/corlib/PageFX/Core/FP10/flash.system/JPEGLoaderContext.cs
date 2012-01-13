using System;
using System.Runtime.CompilerServices;

namespace flash.system
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class JPEGLoaderContext : LoaderContext
    {
        [PageFX.ABC]
        [PageFX.FP10]
        public double deblockingFilter;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern JPEGLoaderContext(double arg0, bool arg1, ApplicationDomain arg2, SecurityDomain arg3);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern JPEGLoaderContext(double arg0, bool arg1, ApplicationDomain arg2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern JPEGLoaderContext(double arg0, bool arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern JPEGLoaderContext(double arg0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern JPEGLoaderContext();
    }
}
