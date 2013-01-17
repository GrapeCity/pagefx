using System;
using System.Runtime.CompilerServices;

namespace flash.events
{
    /// <summary>An object dispatches an HTTPStatusEvent object when a network request returns an HTTP status code.</summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class HTTPStatusEvent : Event
    {
        /// <summary>
        /// The HTTPStatusEvent.HTTP_STATUS constant defines the value of the
        /// type property of a httpStatus event object.
        /// This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.statusThe HTTP status code returned by the server.targetThe network object receiving an HTTP status code.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String HTTP_STATUS;

        public extern virtual int status
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern HTTPStatusEvent(Avm.String arg0, bool arg1, bool arg2, int arg3);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern HTTPStatusEvent(Avm.String arg0, bool arg1, bool arg2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern HTTPStatusEvent(Avm.String arg0, bool arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern HTTPStatusEvent(Avm.String arg0);

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
