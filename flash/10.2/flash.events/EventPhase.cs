using System;
using System.Runtime.CompilerServices;

namespace flash.events
{
    /// <summary>The EventPhase class provides values for the eventPhase  property of the Event class.</summary>
    [PageFX.AbcInstance(30)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class EventPhase : Avm.Object
    {
        /// <summary>The capturing phase, which is the first phase of the event flow.</summary>
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint CAPTURING_PHASE;

        /// <summary>The target phase, which is the second phase of the event flow.</summary>
        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint AT_TARGET;

        /// <summary>The bubbling phase, which is the third phase of the event flow.</summary>
        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint BUBBLING_PHASE;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern EventPhase();
    }
}
