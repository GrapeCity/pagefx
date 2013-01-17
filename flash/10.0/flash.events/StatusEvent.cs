using System;
using System.Runtime.CompilerServices;

namespace flash.events
{
    /// <summary>
    /// An object dispatches a StatusEvent object when a device, such as a camera or microphone,
    /// or an object such as a LocalConnection object reports its status. There is only
    /// one type of status event: StatusEvent.STATUS .
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class StatusEvent : Event
    {
        /// <summary>
        /// Defines the value of the type property of a status event
        /// object.
        /// This event has the following properties:
        /// Property
        /// Valuebubblesfalsecancelablefalse; there is no default behavior to cancel.code
        /// A description of the object&apos;s status.currentTarget
        /// The object that is actively processing the Event object with an event listener.level
        /// The category of the message, such as &quot;status&quot;, &quot;warning&quot;
        /// or &quot;error&quot;.target
        /// The object reporting its status.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String STATUS;

        public extern virtual Avm.String code
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

        public extern virtual Avm.String level
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
        public extern StatusEvent(Avm.String arg0, bool arg1, bool arg2, Avm.String arg3, Avm.String arg4);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern StatusEvent(Avm.String arg0, bool arg1, bool arg2, Avm.String arg3);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern StatusEvent(Avm.String arg0, bool arg1, bool arg2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern StatusEvent(Avm.String arg0, bool arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern StatusEvent(Avm.String arg0);

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
