using System;
using System.Runtime.CompilerServices;

namespace flash.events
{
    [PageFX.AbcInstance(358)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public partial class StageVideoEvent : flash.events.Event
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String RENDER_STATE;

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String RENDER_STATUS_UNAVAILABLE;

        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String RENDER_STATUS_SOFTWARE;

        [PageFX.AbcClassTrait(3)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String RENDER_STATUS_ACCELERATED;

        public extern virtual Avm.String status
        {
            [PageFX.AbcInstanceTrait(0)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual Avm.String colorSpace
        {
            [PageFX.AbcInstanceTrait(1)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern StageVideoEvent(Avm.String type, bool bubbles, bool cancelable, Avm.String status, Avm.String colorSpace);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern StageVideoEvent(Avm.String type, bool bubbles, bool cancelable, Avm.String status);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern StageVideoEvent(Avm.String type, bool bubbles, bool cancelable);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern StageVideoEvent(Avm.String type, bool bubbles);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern StageVideoEvent(Avm.String type);


    }
}
