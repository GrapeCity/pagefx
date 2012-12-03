using System;
using System.Runtime.CompilerServices;

namespace flash.events
{
    /// <summary>
    /// An object dispatches an ErrorEvent object when an error causes a network operation to fail.
    /// There is only one type of error  event: ErrorEvent.ERROR .
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class ErrorEvent : TextEvent
    {
        /// <summary>
        /// Defines the value of the type property of an error event object.
        /// This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetThe object experiencing a network operation failure.textText to be displayed as an error message.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String ERROR;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ErrorEvent(Avm.String arg0, bool arg1, bool arg2, Avm.String arg3);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ErrorEvent(Avm.String arg0, bool arg1, bool arg2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ErrorEvent(Avm.String arg0, bool arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ErrorEvent(Avm.String arg0);

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
