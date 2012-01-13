using System;
using System.Runtime.CompilerServices;

namespace flash.profiler
{
    [PageFX.GlobalFunctions]
    public class Global
    {
        [PageFX.ABC]
        [PageFX.QName("showRedrawRegions", "flash.profiler", "package")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void showRedrawRegions(bool arg0, uint arg1);

        [PageFX.ABC]
        [PageFX.QName("showRedrawRegions", "flash.profiler", "package")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void showRedrawRegions(bool arg0);

        [PageFX.ABC]
        [PageFX.QName("profile", "flash.profiler", "package")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void profile(bool arg0);
    }
}
