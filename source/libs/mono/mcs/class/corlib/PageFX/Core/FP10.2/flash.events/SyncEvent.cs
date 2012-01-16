using System;
using System.Runtime.CompilerServices;

namespace flash.events
{
    /// <summary>
    /// An SharedObject object representing a remote shared object dispatches a SyncEvent object when the remote
    /// shared object has been updated by the server. There is only one type of sync  event:
    /// SyncEvent.SYNC .
    /// </summary>
    [PageFX.AbcInstance(281)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class SyncEvent : flash.events.Event
    {
        /// <summary>
        /// Defines the value of the type property of a sync event object.
        /// This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.changeListAn array with properties that describe the array&apos;s status.targetThe SharedObject instance that has been updated by the server.
        /// </summary>
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String SYNC;

        /// <summary>
        /// An array of objects; each object contains properties that describe the changed members of a remote shared object.
        /// The properties of each object are code, name, and oldValue.
        /// When you initially connect to a remote shared object that is persistent locally and/or on the server, all the
        /// properties of this object are set to empty strings.Otherwise, Flash sets code to &quot;clear&quot;,
        /// &quot;success&quot;, &quot;reject&quot;, &quot;change&quot;, or &quot;delete&quot;. A value of &quot;clear&quot; means either that you have successfully connected to a remote shared object
        /// that is not persistent on the server or the client, or that all the properties of the object have been deleted--for
        /// example, when the client and server copies of the object are so far out of sync that Flash Player resynchronizes the client
        /// object with the server object. In the latter case, SyncEvent.SYNC is dispatched and the &quot;code&quot; value
        /// is set to &quot;change&quot;. A value of &quot;success&quot; means the client changed the shared object. A value of &quot;reject&quot; means the client tried unsuccessfully to change the object; instead, another client changed the object. A value of &quot;change&quot; means another client changed the object or the server resynchronized the object. A value of &quot;delete&quot; means the attribute was deleted. The name property contains the name of the property that has been changed.The oldValue property contains the former value of the changed property. This parameter is
        /// null unless code has a value of &quot;reject&quot; or &quot;change&quot;.
        /// </summary>
        public extern virtual Avm.Array changeList
        {
            [PageFX.AbcInstanceTrait(3)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(4)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern SyncEvent(Avm.String type, bool bubbles, bool cancelable, Avm.Array changeList);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern SyncEvent(Avm.String type, bool bubbles, bool cancelable);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern SyncEvent(Avm.String type, bool bubbles);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern SyncEvent(Avm.String type);

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
