using System;
using System.Runtime.CompilerServices;

namespace flash.text.engine
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class TabAlignment : Avm.Object
    {
        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String DECIMAL;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String START;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String CENTER;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String END;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TabAlignment();
    }
}
