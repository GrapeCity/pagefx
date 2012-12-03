using System;
using System.Runtime.CompilerServices;

namespace flash.events
{
    /// <summary>An object dispatches an HTTPStatusEvent object when a network request returns an HTTP status code.</summary>
    [PageFX.AbcInstance(257)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class HTTPStatusEvent : flash.events.Event
    {
        /// <summary>
        /// The HTTPStatusEvent.HTTP_STATUS constant defines the value of the
        /// type property of a httpStatus event object.
        /// This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.statusThe HTTP status code returned by the server.targetThe network object receiving an HTTP status code.
        /// </summary>
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String HTTP_STATUS;

        /// <summary>
        /// The HTTPStatusEvent.HTTP_RESPONSE_STATUS constant defines the value of the
        /// type property of a httpResponseStatus event object.
        /// This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.responseURLThe URL that the response was returned from.responseHeadersThe response headers that the response returned,
        /// as an array of URLRequestHeader objects.statusThe HTTP status code returned by the server.targetThe network object receiving an HTTP status code.
        /// </summary>
        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String HTTP_RESPONSE_STATUS;

        /// <summary>
        /// The HTTP status code returned by the server. For example, a value of 404 indicates that the server
        /// has not found a match for the requested URI. HTTP status codes can be found in sections 10.4 and 10.5
        /// of the HTTP specification at
        /// ftp://ftp.isi.edu/in-notes/rfc2616.txt.
        /// If Flash Player cannot get a status code from the server, or if it cannot communicate with the
        /// server, the default value of 0 is passed to your ActionScript code. A value of 0 can be generated
        /// in any player (for example, if a malformed URL is requested), and a value of 0 is always generated
        /// by the Flash Player plug-in when it is run in the following browsers, which do not pass HTTP status
        /// codes to the player: Netscape, Mozilla, Safari, Opera, and Internet Explorer for the Macintosh.
        /// </summary>
        public extern virtual int status
        {
            [PageFX.AbcInstanceTrait(5)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// The URL that the response was returned from. In the case of redirects, this will be different
        /// from the request URL.
        /// </summary>
        public extern virtual Avm.String responseURL
        {
            [PageFX.AbcInstanceTrait(6)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(7)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>The response headers that the response returned, as an array of URLRequestHeader objects.</summary>
        public extern virtual Avm.Array responseHeaders
        {
            [PageFX.AbcInstanceTrait(8)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(9)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern HTTPStatusEvent(Avm.String type, bool bubbles, bool cancelable, int status);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern HTTPStatusEvent(Avm.String type, bool bubbles, bool cancelable);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern HTTPStatusEvent(Avm.String type, bool bubbles);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern HTTPStatusEvent(Avm.String type);

        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override flash.events.Event clone();

        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override Avm.String toString();


    }
}
