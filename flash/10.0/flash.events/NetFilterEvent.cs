using System;
using System.Runtime.CompilerServices;

namespace flash.events
{
    [PageFX.ABC]
    [PageFX.FP9]
    public class NetFilterEvent : Event
    {
        [PageFX.ABC]
        [PageFX.FP9]
        public flash.utils.ByteArray data;

        [PageFX.ABC]
        [PageFX.FP9]
        public flash.utils.ByteArray header;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern NetFilterEvent(Avm.String arg0, bool arg1, bool arg2, flash.utils.ByteArray arg3, flash.utils.ByteArray arg4);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern NetFilterEvent(Avm.String arg0, bool arg1, bool arg2, flash.utils.ByteArray arg3);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern NetFilterEvent(Avm.String arg0, bool arg1, bool arg2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern NetFilterEvent(Avm.String arg0, bool arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern NetFilterEvent(Avm.String arg0);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override Avm.String toString();

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override Event clone();
    }
}
