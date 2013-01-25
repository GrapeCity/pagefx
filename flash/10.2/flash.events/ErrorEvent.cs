using System;
using System.Runtime.CompilerServices;

namespace flash.events
{
    /// <summary>
    /// An object dispatches an ErrorEvent object when an error causes a network operation to fail.
    /// There is only one type of error  event: ErrorEvent.ERROR .
    /// </summary>
    [PageFX.AbcInstance(157)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class ErrorEvent : flash.events.TextEvent
    {
        /// <summary>
        /// Defines the value of the type property of an error event object.
        /// This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetThe object experiencing a network operation failure.textText to be displayed as an error message.
        /// </summary>
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String ERROR;

        /// <summary>
        /// Contains the reference number associated with the specific error.
        /// For a custom ErrorEvent object, this number is the value from the id
        /// parameter supplied in the constructor.
        /// </summary>
        public extern virtual int errorID
        {
            [PageFX.AbcInstanceTrait(0)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ErrorEvent(Avm.String type, bool bubbles, bool cancelable, Avm.String text, int id);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ErrorEvent(Avm.String type, bool bubbles, bool cancelable, Avm.String text);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ErrorEvent(Avm.String type, bool bubbles, bool cancelable);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ErrorEvent(Avm.String type, bool bubbles);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ErrorEvent(Avm.String type);

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
