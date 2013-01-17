using System;
using System.Runtime.CompilerServices;

namespace flash.events
{
    /// <summary>
    /// An SharedObject object representing a remote shared object dispatches a SyncEvent object when the remote
    /// shared object has been updated by the server. There is only one type of sync  event:
    /// SyncEvent.SYNC .
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class SyncEvent : Event
    {
        /// <summary>
        /// Defines the value of the type property of a sync event object.
        /// This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.changeListAn array with properties that describe the array&apos;s status.targetThe SharedObject instance that has been updated by the server.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String SYNC;

        public extern virtual Avm.Array changeList
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
        public extern SyncEvent(Avm.String arg0, bool arg1, bool arg2, Avm.Array arg3);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern SyncEvent(Avm.String arg0, bool arg1, bool arg2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern SyncEvent(Avm.String arg0, bool arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern SyncEvent(Avm.String arg0);

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
