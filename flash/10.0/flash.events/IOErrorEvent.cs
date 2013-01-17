using System;
using System.Runtime.CompilerServices;

namespace flash.events
{
    /// <summary>An object dispatches an IOErrorEvent object when an error causes a send or load operation to fail.</summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class IOErrorEvent : ErrorEvent
    {
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String DISK_ERROR;

        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String NETWORK_ERROR;

        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String VERIFY_ERROR;

        /// <summary>
        /// Defines the value of the type property of an ioError event object.
        /// This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.errorIDA reference number associated with the specific error.targetThe network object experiencing the input/output error.textText to be displayed as an error message.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String IO_ERROR;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern IOErrorEvent(Avm.String arg0, bool arg1, bool arg2, Avm.String arg3);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern IOErrorEvent(Avm.String arg0, bool arg1, bool arg2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern IOErrorEvent(Avm.String arg0, bool arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern IOErrorEvent(Avm.String arg0);

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
