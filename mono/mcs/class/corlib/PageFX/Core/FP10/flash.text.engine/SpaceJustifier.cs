using System;
using System.Runtime.CompilerServices;

namespace flash.text.engine
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class SpaceJustifier : TextJustifier
    {
        public extern virtual bool letterSpacing
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
        public extern SpaceJustifier(Avm.String arg0, Avm.String arg1, bool arg2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern SpaceJustifier(Avm.String arg0, Avm.String arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern SpaceJustifier(Avm.String arg0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern SpaceJustifier();

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override TextJustifier clone();


    }
}
