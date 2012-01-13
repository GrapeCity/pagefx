using System;
using System.Runtime.CompilerServices;

namespace flash.events
{
    /// <summary>
    /// An object dispatches a ProgressEvent object when a load operation has begun or a socket has received data.
    /// These events are usually generated when SWF files, images or data are loaded into Flash
    /// Player or  the Adobe Integrated Runtime (AIR). There are two types of progress events:
    /// ProgressEvent.PROGRESS  and ProgressEvent.SOCKET_DATA .
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class ProgressEvent : Event
    {
        /// <summary>
        /// Defines the value of the type property of a progress event object.
        /// This event has the following properties:PropertyValuebubblesfalsebytesLoadedThe number of items or bytes loaded at the time the listener processes the event.bytesTotalThe total number of items or bytes that ultimately will  be loaded if the loading process succeeds.cancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetThe network object reporting progress.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String PROGRESS;

        /// <summary>
        /// Defines the value of the type property of a socketData event object.
        /// This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event.bytesLoadedThe number of items or bytes loaded at the time the listener processes the event.bytesTotal0; this property is not used by socketData event objects.targetThe Socket object reporting progress.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String SOCKET_DATA;

        public extern virtual uint bytesLoaded
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

        public extern virtual uint bytesTotal
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
        public extern ProgressEvent(Avm.String arg0, bool arg1, bool arg2, uint arg3, uint arg4);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ProgressEvent(Avm.String arg0, bool arg1, bool arg2, uint arg3);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ProgressEvent(Avm.String arg0, bool arg1, bool arg2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ProgressEvent(Avm.String arg0, bool arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ProgressEvent(Avm.String arg0);

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
