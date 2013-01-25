using System;
using System.Runtime.CompilerServices;

namespace flash.text
{
    [PageFX.AbcInstance(75)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public partial class TextInteractionMode : Avm.Object
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String NORMAL;

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String SELECTION;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextInteractionMode();
    }
}
