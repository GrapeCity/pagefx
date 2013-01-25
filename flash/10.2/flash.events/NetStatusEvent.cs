using System;
using System.Runtime.CompilerServices;

namespace flash.events
{
    /// <summary>
    /// A NetConnection, NetStream, or SharedObject object dispatches NetStatusEvent objects when a it reports its status.
    /// There is only one type of status event: NetStatusEvent.NET_STATUS .
    /// </summary>
    [PageFX.AbcInstance(195)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class NetStatusEvent : flash.events.Event
    {
        /// <summary>
        /// Defines the value of the type property of a netStatus event object.
        /// This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.infoAn object with properties that describe the object&apos;s status or error condition.targetThe NetConnection or NetStream object reporting its status.
        /// </summary>
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String NET_STATUS;

        /// <summary>
        /// An object with properties that describe the object&apos;s status or error condition.
        /// The information object could have a code property containing a string
        /// that represents a specific event or a level property containing a string
        /// that is either &quot;status&quot; or &quot;error&quot;. The information object could also be something different. The code and
        /// level properties might not work for some implementations and some servers
        /// might send different objects.
        /// The following table describes the possible values of the code and level
        /// properties.Code propertyLevel propertyMeaningNetStream.Buffer.Empty&quot;status&quot;Data is not being received quickly enough to fill the buffer. Data flow will be interrupted until the buffer refills, at which time a NetStream.Buffer.Full message will be sent and the stream will begin playing again.NetStream.Buffer.Full&quot;status&quot;The buffer is full and the stream will begin playing.NetStream.Buffer.Flush&quot;status&quot;Data has finished streaming, and the remaining buffer will be emptied.NetStream.Publish.Start&quot;status&quot;Publish was successful.NetStream.Publish.BadName&quot;error&quot;Attempt to publish a stream which is already being published by someone else.NetStream.Publish.Idle&quot;status&quot;The publisher of the stream has been idling for too long.NetStream.Unpublish.Success&quot;status&quot;The unpublish operation was successfuul.NetStream.Play.Start&quot;status&quot;Playback has started.NetStream.Play.Stop&quot;status&quot;Playback has stopped.NetStream.Play.Failed&quot;error&quot;An error has occurred in playback for a reason other than those listed elsewhere
        /// in this table, such as the subscriber not having read access.NetStream.Play.StreamNotFound&quot;error&quot;The FLV passed to the play() method can&apos;t be found.NetStream.Play.Reset&quot;status&quot;Caused by a play list reset.NetStream.Play.PublishNotify&quot;status&quot;The initial publish to a stream is sent to all subscribers.NetStream.Play.UnpublishNotify&quot;status&quot;An unpublish from a stream is sent to all subscribers.NetStream.Pause.Notify&quot;status&quot;The stream is paused.NetStream.Unpause.Notify&quot;status&quot;The stream is resumed.NetStream.Record.Start&quot;status&quot;Recording has started.NetStream.Record.NoAccess&quot;error&quot;Attempt to record a stream that is still playing or the client has no access right.NetStream.Record.Stop&quot;status&quot;Recording stopped.NetStream.Record.Failed&quot;error&quot;An attempt to record a stream failed.NetStream.Seek.Failed&quot;error&quot;The seek fails, which happens if the stream is not seekable.NetStream.Seek.InvalidTime&quot;error&quot;For video downloaded with progressive download, the user has tried to seek or play
        /// past the end of the video data that has downloaded thus far, or past
        /// the end of the video once the entire file has downloaded. The message.details
        /// property contains a time code
        /// that indicates the last valid position to which the user can seek.NetStream.Seek.Notify&quot;status&quot;The seek operation is complete.NetConnection.Call.BadVersion&quot;error&quot;Packet encoded in an unidentified format.NetConnection.Call.Failed&quot;error&quot;The NetConnection.call method was not able to invoke the server-side
        /// method or command.NetConnection.Call.Prohibited&quot;error&quot;An Action Message Format (AMF) operation is prevented for
        /// security reasons. Either the AMF URL is not in the same domain as the SWF file, or
        /// the AMF server does not have a policy file that trusts the domain of the SWF file.
        /// NetConnection.Connect.Closed&quot;status&quot;The connection was closed successfully.NetConnection.Connect.Failed&quot;error&quot;The connection attempt failed.NetConnection.Connect.Success&quot;status&quot;The connection attempt succeeded.NetConnection.Connect.Rejected&quot;error&quot;The connection attempt did not have permission to access the application.NetConnection.Connect.AppShutdown&quot;error&quot;The specified application is shutting down.NetConnection.Connect.InvalidApp&quot;error&quot;The application name specified during connect is invalid.SharedObject.Flush.Success&quot;status&quot;The &quot;pending&quot; status is resolved and the SharedObject.flush() call succeeded.SharedObject.Flush.Failed&quot;error&quot;The &quot;pending&quot; status is resolved, but the SharedObject.flush() failed.SharedObject.BadPersistence&quot;error&quot;A request was made for a shared object with persistence flags, but the request cannot be granted because the object has already been created with different flags.SharedObject.UriMismatch&quot;error&quot;An attempt was made to connect to a NetConnection object that has a different URI (URL) than the shared object.If you consistently see errors regarding the buffer, try changing the buffer using the NetStream.bufferTime property.
        /// </summary>
        public extern virtual object info
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
        public extern NetStatusEvent(Avm.String type, bool bubbles, bool cancelable, object info);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern NetStatusEvent(Avm.String type, bool bubbles, bool cancelable);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern NetStatusEvent(Avm.String type, bool bubbles);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern NetStatusEvent(Avm.String type);

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
