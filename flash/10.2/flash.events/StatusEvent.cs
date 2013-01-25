using System;
using System.Runtime.CompilerServices;

namespace flash.events
{
    /// <summary>
    /// An object dispatches a StatusEvent object when a device, such as a camera or microphone,
    /// or an object such as a LocalConnection object reports its status. There is only
    /// one type of status event: StatusEvent.STATUS .
    /// </summary>
    [PageFX.AbcInstance(141)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class StatusEvent : flash.events.Event
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
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String STATUS;

        /// <summary>A description of the object&apos;s status.</summary>
        public extern virtual Avm.String code
        {
            [PageFX.AbcInstanceTrait(4)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(5)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// The category of the message, such as &quot;status&quot;, &quot;warning&quot;
        /// or &quot;error&quot;.
        /// </summary>
        public extern virtual Avm.String level
        {
            [PageFX.AbcInstanceTrait(6)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(7)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern StatusEvent(Avm.String type, bool bubbles, bool cancelable, Avm.String code, Avm.String level);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern StatusEvent(Avm.String type, bool bubbles, bool cancelable, Avm.String code);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern StatusEvent(Avm.String type, bool bubbles, bool cancelable);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern StatusEvent(Avm.String type, bool bubbles);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern StatusEvent(Avm.String type);

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
