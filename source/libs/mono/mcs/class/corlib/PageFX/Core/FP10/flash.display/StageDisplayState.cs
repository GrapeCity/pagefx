using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    /// <summary>The StageDisplayState class provides values for the Stage.displayState  property.</summary>
    [PageFX.ABC]
    [PageFX.FP10]
    public class StageDisplayState : Avm.Object
    {
        /// <summary>Specifies that the Stage is in full-screen mode.</summary>
        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String FULL_SCREEN;

        /// <summary>Specifies that the Stage is in normal mode.</summary>
        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String NORMAL;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern StageDisplayState();
    }
}
