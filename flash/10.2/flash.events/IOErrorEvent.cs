using System;
using System.Runtime.CompilerServices;

namespace flash.events
{
    /// <summary>An object dispatches an IOErrorEvent object when an error causes a send or load operation to fail.</summary>
    [PageFX.AbcInstance(158)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class IOErrorEvent : flash.events.ErrorEvent
    {
        /// <summary>
        /// Defines the value of the type property of an ioError event object.
        /// This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.errorIDA reference number associated with the specific error.targetThe network object experiencing the input/output error.textText to be displayed as an error message.
        /// </summary>
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String IO_ERROR;

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String NETWORK_ERROR;

        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String DISK_ERROR;

        [PageFX.AbcClassTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String VERIFY_ERROR;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern IOErrorEvent(Avm.String type, bool bubbles, bool cancelable, Avm.String text, int id);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern IOErrorEvent(Avm.String type, bool bubbles, bool cancelable, Avm.String text);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern IOErrorEvent(Avm.String type, bool bubbles, bool cancelable);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern IOErrorEvent(Avm.String type, bool bubbles);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern IOErrorEvent(Avm.String type);

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
    }
}
