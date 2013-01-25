using System;
using System.Runtime.CompilerServices;

namespace flash.events
{
    [PageFX.AbcInstance(273)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class NetFilterEvent : flash.events.Event
    {
        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public flash.utils.ByteArray header;

        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        public flash.utils.ByteArray data;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern NetFilterEvent(Avm.String type, bool bubbles, bool cancelable, flash.utils.ByteArray header, flash.utils.ByteArray data);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern NetFilterEvent(Avm.String type, bool bubbles, bool cancelable, flash.utils.ByteArray header);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern NetFilterEvent(Avm.String type, bool bubbles, bool cancelable);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern NetFilterEvent(Avm.String type, bool bubbles);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern NetFilterEvent(Avm.String type);

        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override flash.events.Event clone();

        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override Avm.String toString();
    }
}
