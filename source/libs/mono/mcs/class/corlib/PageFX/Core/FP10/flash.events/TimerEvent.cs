using System;
using System.Runtime.CompilerServices;

namespace flash.events
{
    /// <summary>
    /// A Timer object dispatches a TimerEvent objects whenever the Timer object reaches the interval
    /// specified by the Timer.delay  property.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class TimerEvent : Event
    {
        /// <summary>
        /// Defines the value of the type property of a timerComplete event object.
        /// This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetThe Timer object that has completed its requests.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String TIMER_COMPLETE;

        /// <summary>
        /// Defines the value of the type property of a timer event object.
        /// This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetThe Timer object that has reached its interval.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String TIMER;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TimerEvent(Avm.String arg0, bool arg1, bool arg2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TimerEvent(Avm.String arg0, bool arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TimerEvent(Avm.String arg0);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override Avm.String toString();

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override Event clone();

        /// <summary>Instructs Flash Player to render after processing of this event completes, if the display list has been modified</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void updateAfterEvent();
    }
}
