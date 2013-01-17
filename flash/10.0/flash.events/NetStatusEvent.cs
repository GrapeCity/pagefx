using System;
using System.Runtime.CompilerServices;

namespace flash.events
{
    /// <summary>
    /// A NetConnection, NetStream, or SharedObject object dispatches NetStatusEvent objects when a it reports its status.
    /// There is only one type of status event: NetStatusEvent.NET_STATUS .
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class NetStatusEvent : Event
    {
        /// <summary>
        /// Defines the value of the type property of a netStatus event object.
        /// This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.infoAn object with properties that describe the object&apos;s status or error condition.targetThe NetConnection or NetStream object reporting its status.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String NET_STATUS;

        public extern virtual object info
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
        public extern NetStatusEvent(Avm.String arg0, bool arg1, bool arg2, object arg3);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern NetStatusEvent(Avm.String arg0, bool arg1, bool arg2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern NetStatusEvent(Avm.String arg0, bool arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern NetStatusEvent(Avm.String arg0);

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
