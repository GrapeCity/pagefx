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
    [PageFX.AbcInstance(167)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class AsyncErrorEvent : flash.events.ErrorEvent
    {
        /// <summary>The exception that was thrown.</summary>
        [PageFX.AbcInstanceTrait(0)]
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
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String ASYNC_ERROR;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern AsyncErrorEvent(Avm.String type, bool bubbles, bool cancelable, Avm.String text, Avm.Error error);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern AsyncErrorEvent(Avm.String type, bool bubbles, bool cancelable, Avm.String text);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern AsyncErrorEvent(Avm.String type, bool bubbles, bool cancelable);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern AsyncErrorEvent(Avm.String type, bool bubbles);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern AsyncErrorEvent(Avm.String type);

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
