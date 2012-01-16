using System;
using System.Runtime.CompilerServices;

namespace flash.events
{
    [PageFX.AbcInstance(227)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public class StageVideoAvailabilityEvent : flash.events.Event
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STAGE_VIDEO_AVAILABILITY;

        public extern virtual Avm.String availability
        {
            [PageFX.AbcInstanceTrait(0)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern StageVideoAvailabilityEvent(Avm.String type, bool bubbles, bool cancelable, Avm.String availability);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern StageVideoAvailabilityEvent(Avm.String type, bool bubbles, bool cancelable);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern StageVideoAvailabilityEvent(Avm.String type, bool bubbles);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern StageVideoAvailabilityEvent(Avm.String type);


    }
}
