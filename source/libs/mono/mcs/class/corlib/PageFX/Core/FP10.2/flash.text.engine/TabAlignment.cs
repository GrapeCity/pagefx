using System;
using System.Runtime.CompilerServices;

namespace flash.text.engine
{
    [PageFX.AbcInstance(363)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public class TabAlignment : Avm.Object
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String START;

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String CENTER;

        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String END;

        [PageFX.AbcClassTrait(3)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String DECIMAL;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TabAlignment();
    }
}
