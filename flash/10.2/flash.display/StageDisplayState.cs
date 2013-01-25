using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    /// <summary>The StageDisplayState class provides values for the Stage.displayState  property.</summary>
    [PageFX.AbcInstance(280)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public partial class StageDisplayState : Avm.Object
    {
        /// <summary>Specifies that the Stage is in full-screen mode.</summary>
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String FULL_SCREEN;

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String FULL_SCREEN_INTERACTIVE;

        /// <summary>Specifies that the Stage is in normal mode.</summary>
        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String NORMAL;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern StageDisplayState();
    }
}
