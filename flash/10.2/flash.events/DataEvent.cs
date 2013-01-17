using System;
using System.Runtime.CompilerServices;

namespace flash.events
{
    /// <summary>
    /// An object dispatches a DataEvent object when raw data has completed loading.
    /// Ther are two types of data event:
    /// DataEvent.DATA: dispatched for data sent or received.DataEvent.UPLOAD_COMPLETE_DATA: dispatched when data is sent and the server has responded.
    /// </summary>
    [PageFX.AbcInstance(248)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class DataEvent : flash.events.TextEvent
    {
        /// <summary>
        /// Defines the value of the type property of a data event object.
        /// This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.dataThe raw data loaded into Flash Player.targetThe XMLSocket object receiving data.
        /// </summary>
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String DATA;

        /// <summary>
        /// Defines the value of the type property of an uploadCompleteData event object.
        /// This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.dataThe raw data returned from the server after a successful file upload.targetThe FileReference object receiving data after a successful upload.
        /// </summary>
        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String UPLOAD_COMPLETE_DATA;

        /// <summary>The raw data loaded into Flash Player.</summary>
        public extern virtual Avm.String data
        {
            [PageFX.AbcInstanceTrait(2)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(3)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DataEvent(Avm.String type, bool bubbles, bool cancelable, Avm.String data);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DataEvent(Avm.String type, bool bubbles, bool cancelable);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DataEvent(Avm.String type, bool bubbles);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DataEvent(Avm.String type);

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
