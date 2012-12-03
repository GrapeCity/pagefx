using System;
using System.Runtime.CompilerServices;

namespace flash.events
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class SampleDataEvent : Event
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

        public extern virtual flash.utils.ByteArray data
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
        public extern SampleDataEvent(Avm.String arg0, bool arg1, bool arg2, double arg3, flash.utils.ByteArray arg4);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern SampleDataEvent(Avm.String arg0, bool arg1, bool arg2, double arg3);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern SampleDataEvent(Avm.String arg0, bool arg1, bool arg2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern SampleDataEvent(Avm.String arg0, bool arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern SampleDataEvent(Avm.String arg0);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override Avm.String toString();

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override Event clone();
    }
}
