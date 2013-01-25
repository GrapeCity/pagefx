using System;
using System.Runtime.CompilerServices;

namespace flash.events
{
    /// <summary>
    /// A Camera or Microphone object dispatches an ActivityEvent object whenever the camera or microphone reports that it has
    /// become active or inactive. There is only one type of activity event: ActivityEvent.ACTIVITY .
    /// </summary>
    [PageFX.AbcInstance(213)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class ActivityEvent : flash.events.Event
    {
        /// <summary>
        /// The ActivityEvent.ACTIVITY constant defines the value of the type property of an activity event object.
        /// This event has the following properties:PropertyValueactivatingtrue if the device is activating or false if it is deactivating.bubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetThe object beginning or ending a session, such as a Camera or
        /// Microphone object.
        /// </summary>
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String ACTIVITY;

        /// <summary>
        /// Indicates whether the device is activating (true) or deactivating
        /// (false).
        /// </summary>
        public extern virtual bool activating
        {
            [PageFX.AbcInstanceTrait(3)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(4)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ActivityEvent(Avm.String type, bool bubbles, bool cancelable, bool activating);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ActivityEvent(Avm.String type, bool bubbles, bool cancelable);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ActivityEvent(Avm.String type, bool bubbles);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ActivityEvent(Avm.String type);

        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override flash.events.Event clone();

        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override Avm.String toString();


    }
}
