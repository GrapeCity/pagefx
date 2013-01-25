using System;
using System.Runtime.CompilerServices;

namespace flash.events
{
    /// <summary>
    /// A Timer object dispatches a TimerEvent objects whenever the Timer object reaches the interval
    /// specified by the Timer.delay  property.
    /// </summary>
    [PageFX.AbcInstance(172)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class TimerEvent : flash.events.Event
    {
        /// <summary>
        /// Defines the value of the type property of a timer event object.
        /// This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetThe Timer object that has reached its interval.
        /// </summary>
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String TIMER;

        /// <summary>
        /// Defines the value of the type property of a timerComplete event object.
        /// This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetThe Timer object that has completed its requests.
        /// </summary>
        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String TIMER_COMPLETE;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TimerEvent(Avm.String type, bool bubbles, bool cancelable);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TimerEvent(Avm.String type, bool bubbles);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TimerEvent(Avm.String type);

        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override flash.events.Event clone();

        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override Avm.String toString();

        /// <summary>Instructs Flash Player to render after processing of this event completes, if the display list has been modified</summary>
        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void updateAfterEvent();
    }
}
