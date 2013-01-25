using System;
using System.Runtime.CompilerServices;

namespace flash.profiler
{
    [PageFX.GlobalFunctions]
    public partial class Global
    {
        [PageFX.AbcScript(127)]
        [PageFX.AbcScriptTrait(0)]
        [PageFX.ABC]
        [PageFX.QName("profile", "flash.profiler", "package")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void profile(bool on);

        [PageFX.AbcScript(127)]
        [PageFX.AbcScriptTrait(1)]
        [PageFX.ABC]
        [PageFX.QName("showRedrawRegions", "flash.profiler", "package")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void showRedrawRegions(bool on, uint color);

        [PageFX.AbcScript(127)]
        [PageFX.AbcScriptTrait(1)]
        [PageFX.ABC]
        [PageFX.QName("showRedrawRegions", "flash.profiler", "package")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void showRedrawRegions(bool on);
    }
}
