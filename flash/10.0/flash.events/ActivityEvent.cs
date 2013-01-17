using System;
using System.Runtime.CompilerServices;

namespace flash.events
{
    /// <summary>
    /// A Camera or Microphone object dispatches an ActivityEvent object whenever the camera or microphone reports that it has
    /// become active or inactive. There is only one type of activity event: ActivityEvent.ACTIVITY .
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class ActivityEvent : Event
    {
        /// <summary>
        /// The ActivityEvent.ACTIVITY constant defines the value of the type property of an activity event object.
        /// This event has the following properties:PropertyValueactivatingtrue if the device is activating or false if it is deactivating.bubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetThe object beginning or ending a session, such as a Camera or
        /// Microphone object.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String ACTIVITY;

        public extern virtual bool activating
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ActivityEvent(Avm.String arg0, bool arg1, bool arg2, bool arg3);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ActivityEvent(Avm.String arg0, bool arg1, bool arg2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ActivityEvent(Avm.String arg0, bool arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ActivityEvent(Avm.String arg0);

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
