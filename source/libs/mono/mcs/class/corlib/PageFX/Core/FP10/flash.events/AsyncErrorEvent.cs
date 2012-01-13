using System;
using System.Runtime.CompilerServices;

namespace flash.events
{
    /// <summary>
    /// An object dispatches an AsyncErrorEvent when an exception is thrown from native
    /// asynchronous code, which could be from, for example, LocalConnection, NetConnection,
    /// SharedObject , or NetStream. There is only one type of asynchronous error event:
    /// AsyncErrorEvent.ASYNC_ERROR .
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class AsyncErrorEvent : ErrorEvent
    {
        /// <summary>The exception that was thrown.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public Avm.Error error;

        /// <summary>
        /// The AsyncErrorEvent.ASYNC_ERROR constant defines the value of the
        /// type property of an asyncError event object.
        /// This event has the following properties:PropertyValuebubblesfalse
        /// This property applies to ActionScript 3.0 display objects (in SWF files).cancelablefalse; there is no default
        /// behavior to cancel. This property applies to display objects
        /// in SWF content, which use the ActionScript 3.0 display architecture.currentTargetThe object that is actively processing the
        /// Event object with an event listener. This property applies to display
        /// objects in SWF content, which use the ActionScript 3.0 display architecture.targetThis property applies to display objects in SWF
        /// content, which use the ActionScript 3.0 display architecture.errorThe error that triggered the event.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String ASYNC_ERROR;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern AsyncErrorEvent(Avm.String arg0, bool arg1, bool arg2, Avm.String arg3, Avm.Error arg4);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern AsyncErrorEvent(Avm.String arg0, bool arg1, bool arg2, Avm.String arg3);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern AsyncErrorEvent(Avm.String arg0, bool arg1, bool arg2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern AsyncErrorEvent(Avm.String arg0, bool arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern AsyncErrorEvent(Avm.String arg0);

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
