using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    /// <summary>
    /// The GradientType class provides values for the type  parameter in the
    /// beginGradientFill()  and lineGradientFill()  methods of the flash.display.Graphics class.
    /// </summary>
    [PageFX.AbcInstance(334)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class GradientType : Avm.Object
    {
        /// <summary>Value used to specify a linear gradient fill.</summary>
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String LINEAR;

        /// <summary>Value used to specify a radial gradient fill.</summary>
        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String RADIAL;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GradientType();
    }
}
