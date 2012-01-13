using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    /// <summary>The StageAlign class provides constant values to use for the Stage.align  property.</summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class StageAlign : Avm.Object
    {
        /// <summary>Specifies that the Stage is aligned on the left.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String LEFT;

        /// <summary>Specifies that the Stage is aligned in the bottom-right corner.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String BOTTOM_RIGHT;

        /// <summary>Specifies that the Stage is aligned at the bottom.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String BOTTOM;

        /// <summary>Specifies that the Stage is aligned in the top-left corner.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String TOP_LEFT;

        /// <summary>Specifies that the Stage is aligned in the top-right corner.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String TOP_RIGHT;

        /// <summary>Specifies that the Stage is aligned at the top.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String TOP;

        /// <summary>Specifies that the Stage is aligned in the bottom-left corner.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String BOTTOM_LEFT;

        /// <summary>Specifies that the Stage is aligned to the right.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String RIGHT;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern StageAlign();
    }
}
