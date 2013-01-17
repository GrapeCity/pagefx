using System;
using System.Runtime.CompilerServices;

namespace flash.text.engine
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class TabStop : Avm.Object
    {
        public extern virtual double position
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

        public extern virtual Avm.String alignment
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

        public extern virtual Avm.String decimalAlignmentToken
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
        public extern TabStop(Avm.String arg0, double arg1, Avm.String arg2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TabStop(Avm.String arg0, double arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TabStop(Avm.String arg0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TabStop();


    }
}
