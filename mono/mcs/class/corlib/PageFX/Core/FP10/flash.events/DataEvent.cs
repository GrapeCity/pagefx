using System;
using System.Runtime.CompilerServices;

namespace flash.events
{
    /// <summary>
    /// An object dispatches a DataEvent object when raw data has completed loading.
    /// Ther are two types of data event:
    /// DataEvent.DATA: dispatched for data sent or received.DataEvent.UPLOAD_COMPLETE_DATA: dispatched when data is sent and the server has responded.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class DataEvent : TextEvent
    {
        /// <summary>
        /// Defines the value of the type property of a data event object.
        /// This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.dataThe raw data loaded into Flash Player.targetThe XMLSocket object receiving data.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String DATA;

        /// <summary>
        /// Defines the value of the type property of an uploadCompleteData event object.
        /// This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.dataThe raw data returned from the server after a successful file upload.targetThe FileReference object receiving data after a successful upload.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String UPLOAD_COMPLETE_DATA;

        public extern virtual Avm.String data
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
        public extern DataEvent(Avm.String arg0, bool arg1, bool arg2, Avm.String arg3);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DataEvent(Avm.String arg0, bool arg1, bool arg2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DataEvent(Avm.String arg0, bool arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DataEvent(Avm.String arg0);

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
