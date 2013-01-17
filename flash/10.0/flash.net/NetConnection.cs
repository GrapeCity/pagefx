using System;
using System.Runtime.CompilerServices;

namespace flash.net
{
    /// <summary>
    /// The NetConnection class lets you invoke commands on a remote
    /// application server, such as Adobe&apos;s Flash Media Server 2 or Adobe Flex, and
    /// to play streaming Flash Video (FLV) files from either an HTTP address or
    /// a local drive. Typically, you use NetConnection objects with NetStream objects.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class NetConnection : flash.events.EventDispatcher
    {
        public extern virtual Avm.Array unconnectedPeerStreams
        {
            [PageFX.ABC]
            [PageFX.QName("unconnectedPeerStreams", "http://www.adobe.com/2008/actionscript/Flash10/", "public")]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual Avm.String nearID
        {
            [PageFX.ABC]
            [PageFX.QName("nearID", "http://www.adobe.com/2008/actionscript/Flash10/", "public")]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual uint objectEncoding
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

        public extern virtual uint maxPeerConnections
        {
            [PageFX.ABC]
            [PageFX.QName("maxPeerConnections", "http://www.adobe.com/2008/actionscript/Flash10/", "public")]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.QName("maxPeerConnections", "http://www.adobe.com/2008/actionscript/Flash10/", "public")]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual Avm.String protocol
        {
            [PageFX.ABC]
            [PageFX.QName("protocol", "http://www.adobe.com/2008/actionscript/Flash10/", "public")]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual Avm.String proxyType
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

        public extern virtual bool connected
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual object client
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

        public extern virtual Avm.String uri
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual Avm.String nearNonce
        {
            [PageFX.ABC]
            [PageFX.QName("nearNonce", "http://www.adobe.com/2008/actionscript/Flash10/", "public")]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual bool usingTLS
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual Avm.String farID
        {
            [PageFX.ABC]
            [PageFX.QName("farID", "http://www.adobe.com/2008/actionscript/Flash10/", "public")]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual Avm.String farNonce
        {
            [PageFX.ABC]
            [PageFX.QName("farNonce", "http://www.adobe.com/2008/actionscript/Flash10/", "public")]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual Avm.String connectedProxyType
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern static uint defaultObjectEncoding
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

        [PageFX.Event("netStatus")]
        public event flash.events.NetStatusEventHandler netStatus
        {
            add { }
            remove { }
        }

        [PageFX.Event("securityError")]
        public event flash.events.SecurityErrorEventHandler securityError
        {
            add { }
            remove { }
        }

        [PageFX.Event("ioError")]
        public event flash.events.IOErrorEventHandler ioError
        {
            add { }
            remove { }
        }

        [PageFX.Event("asyncError")]
        public event flash.events.AsyncErrorEventHandler asyncError
        {
            add { }
            remove { }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern NetConnection();

        /// <summary>
        /// Opens a connection to a server. Through this connection, you can play back audio or
        /// video (FLV) files
        /// from the local file system, or you
        /// can invoke commands on a remote server.
        /// When using this method, consider the Flash Player security model and the following security
        /// considerations:By default, the website denies access between sandboxes.  The website can enable access to a resource
        /// by using a cross-domain policy file. A website can deny access to a resource by adding server-side ActionScript application
        /// logic in Flash Media Server.You cannot use the NetConnection.connect() method if the calling SWF file is in the
        /// local-with-file-system sandbox.You can prevent a SWF file from using this method by setting the  allowNetworking
        /// parameter of the the object and embed tags in the HTML
        /// page that contains the SWF content.However, in the Apollo runtime, content in the application security sandbox (content
        /// installed with the Apollo application) are not restricted by these security limitations.For more information, see the following:The security chapter in the
        /// Programming ActionScript 3.0 book and the latest comments on LiveDocs
        /// </summary>
        /// <param name="arg0">
        /// Set this parameter to null if you are connecting to video without a server
        /// (that is, video on the local computer that is running the SWF file).
        /// If you are connecting to a server, set this parameter to the URI of the
        /// application on the server that runs when the connection is made. Use the following
        /// format (items in brackets are optional):protocol:[//host][:port]/appname/[instanceName]If the SWF file is served from the same host where the server is installed,
        /// you can omit the host parameter. If you omit the instanceName parameter,
        /// Flash Player connects to the application&apos;s default instance (_definst_).
        /// </param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void connect(Avm.String arg0);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void connect(Avm.String arg0, object rest0);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void connect(Avm.String arg0, object rest0, object rest1);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void connect(Avm.String arg0, object rest0, object rest1, object rest2);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void connect(Avm.String arg0, object rest0, object rest1, object rest2, object rest3);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void connect(Avm.String arg0, object rest0, object rest1, object rest2, object rest3, object rest4);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void connect(Avm.String arg0, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void connect(Avm.String arg0, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void connect(Avm.String arg0, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void connect(Avm.String arg0, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void connect(Avm.String arg0, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8, object rest9);

        /// <summary>
        /// Adds a context header to the AMF packet structure. This header is sent with
        /// every future AMF packet. If you call NetConnection.addHeader()
        /// using the same name, the new header replaces the existing header, and the new header
        /// persists for the duration of the NetConnection object. You can remove a header by
        /// calling NetConnection.addHeader() with the name of the header to remove
        /// an undefined object. This method is relevant when used with a server, such as Flex or
        /// Flash Media Server.
        /// </summary>
        /// <param name="arg0">Identifies the header and the ActionScript object data associated with it.</param>
        /// <param name="arg1">
        /// (default = false)  A value of true indicates that the server must understand
        /// and process this header before it handles any of the following headers or messages.
        /// </param>
        /// <param name="arg2">(default = null)  Any ActionScript object.</param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void addHeader(Avm.String arg0, bool arg1, object arg2);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void addHeader(Avm.String arg0, bool arg1);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void addHeader(Avm.String arg0);

        /// <summary>
        /// Closes the connection that was opened locally or with the server and dispatches
        /// the netStatus event
        /// with a code property of NetConnection.Connect.Closed.
        /// This method disconnects all NetStream objects running over this connection;
        /// any queued data that has not been sent is discarded. (To terminate
        /// streams
        /// without closing the connection, use NetStream.close().)
        /// If you call this method and then want to reconnect, you must recreate the NetStream object.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void close();

        /// <summary>
        /// Invokes a command or method on the server running Flash Media Server, or on an application server,
        /// to which the application instance is connected.
        /// You must create a server-side function to pass to this method.
        /// </summary>
        /// <param name="arg0">
        /// A method specified in the form [objectPath/]method. For example,
        /// the someObject/doSomething command tells the remote server
        /// to invoke the clientObject.someObject.doSomething() method, with all the optional
        /// ... arguments parameters. If the object path is missing,
        /// clientObject.doSomething() is invoked on the remote server.
        /// </param>
        /// <param name="arg1">
        /// An optional object that is used to handle return values from the server.
        /// The Responder object can have two defined methods to handle the returned result:
        /// result and status. If an error is returned as the result,
        /// status is invoked; otherwise, result is invoked.
        /// </param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void call(Avm.String arg0, Responder arg1);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void call(Avm.String arg0, Responder arg1, object rest0);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void call(Avm.String arg0, Responder arg1, object rest0, object rest1);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void call(Avm.String arg0, Responder arg1, object rest0, object rest1, object rest2);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void call(Avm.String arg0, Responder arg1, object rest0, object rest1, object rest2, object rest3);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void call(Avm.String arg0, Responder arg1, object rest0, object rest1, object rest2, object rest3, object rest4);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void call(Avm.String arg0, Responder arg1, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void call(Avm.String arg0, Responder arg1, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void call(Avm.String arg0, Responder arg1, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void call(Avm.String arg0, Responder arg1, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void call(Avm.String arg0, Responder arg1, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8, object rest9);


    }
}
