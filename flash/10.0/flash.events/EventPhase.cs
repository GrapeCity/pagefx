using System;
using System.Runtime.CompilerServices;

namespace flash.events
{
    /// <summary>The EventPhase class provides values for the eventPhase  property of the Event class.</summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class EventPhase : Avm.Object
    {
        /// <summary>The bubbling phase, which is the third phase of the event flow.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint BUBBLING_PHASE;

        /// <summary>The target phase, which is the second phase of the event flow.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint AT_TARGET;

        /// <summary>The capturing phase, which is the first phase of the event flow.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint CAPTURING_PHASE;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern EventPhase();
    }
}
