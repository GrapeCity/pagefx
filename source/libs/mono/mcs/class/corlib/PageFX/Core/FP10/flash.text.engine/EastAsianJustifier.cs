using System;
using System.Runtime.CompilerServices;

namespace flash.text.engine
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class EastAsianJustifier : TextJustifier
    {
        public extern virtual Avm.String justificationStyle
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern EastAsianJustifier(Avm.String arg0, Avm.String arg1, Avm.String arg2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern EastAsianJustifier(Avm.String arg0, Avm.String arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern EastAsianJustifier(Avm.String arg0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern EastAsianJustifier();

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override TextJustifier clone();


    }
}
