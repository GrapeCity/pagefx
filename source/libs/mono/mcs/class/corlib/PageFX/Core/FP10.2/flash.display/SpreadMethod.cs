using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    /// <summary>
    /// The SpreadMethod class provides values for the spreadMethod  parameter
    /// in the beginGradientFill()  and lineGradientStyle()  methods of the Graphics class.
    /// </summary>
    [PageFX.AbcInstance(216)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class SpreadMethod : Avm.Object
    {
        /// <summary>Specifies that the gradient use the pad spread method.</summary>
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String PAD;

        /// <summary>Specifies that the gradient use the reflect spread method.</summary>
        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String REFLECT;

        /// <summary>Specifies that the gradient use the repeat spread method.</summary>
        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String REPEAT;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern SpreadMethod();
    }
}
