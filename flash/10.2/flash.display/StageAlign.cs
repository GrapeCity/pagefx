using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    /// <summary>The StageAlign class provides constant values to use for the Stage.align  property.</summary>
    [PageFX.AbcInstance(266)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class StageAlign : Avm.Object
    {
        /// <summary>Specifies that the Stage is aligned at the top.</summary>
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String TOP;

        /// <summary>Specifies that the Stage is aligned on the left.</summary>
        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String LEFT;

        /// <summary>Specifies that the Stage is aligned at the bottom.</summary>
        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String BOTTOM;

        /// <summary>Specifies that the Stage is aligned to the right.</summary>
        [PageFX.AbcClassTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String RIGHT;

        /// <summary>Specifies that the Stage is aligned in the top-left corner.</summary>
        [PageFX.AbcClassTrait(4)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String TOP_LEFT;

        /// <summary>Specifies that the Stage is aligned in the top-right corner.</summary>
        [PageFX.AbcClassTrait(5)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String TOP_RIGHT;

        /// <summary>Specifies that the Stage is aligned in the bottom-left corner.</summary>
        [PageFX.AbcClassTrait(6)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String BOTTOM_LEFT;

        /// <summary>Specifies that the Stage is aligned in the bottom-right corner.</summary>
        [PageFX.AbcClassTrait(7)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String BOTTOM_RIGHT;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern StageAlign();
    }
}
