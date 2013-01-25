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
    [PageFX.AbcInstance(214)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class NetConnection : flash.events.EventDispatcher
    {
        /// <summary>
        /// Indicates whether Flash Player has connected to a server through
        /// a persistent RTMP connection (true) or not (false).
        /// When connected through HTTP, this property is always false.
        /// It is always true for AMF connections to application servers.
        /// </summary>
        public extern virtual bool connected
        {
            [PageFX.AbcInstanceTrait(0)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// The URI of the application server that was passed to NetConnection.connect(),
        /// if connect was used to connect to a server.
        /// If NetConnection.connect() hasn&apos;t yet been called or if no URI was passed,
        /// this property is undefined.
        /// </summary>
        public extern virtual Avm.String uri
        {
            [PageFX.AbcInstanceTrait(1)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Indicates the object on which callback methods should be invoked. The default is
        /// this NetConnection instance. If you set the client property to another object,
        /// callback methods will be invoked on that object.
        /// </summary>
        public extern virtual object client
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

        /// <summary>
        /// The object encoding (AMF version) for this NetConnection instance. The default
        /// value of this property is the value of defaultObjectEncoding. Acceptable values are
        /// ObjectEncoding.AMF3 and ObjectEncoding.AMF0.
        /// It&apos;s important to understand this property if your ActionScript 3.0 SWF file needs to
        /// communicate with servers released prior to Flash Player 9.When an object is written to or read from binary data, the objectEncoding
        /// property indicates which Action Message Format version should be used:
        /// the ActionScript 3.0 format (ObjectEncoding.AMF3) or the ActionScript 1.0
        /// and ActionScript 2.0 format (ObjectEncoding.AMF0).
        /// When you use NetConnection objects to connect to a server,
        /// three scenarios are possible:Connecting exclusively to a server that was released with or after Flash Player 9,
        /// such as Flex 2. The default value of defaultObjectEncoding is
        /// ObjectEncoding.AMF3. All NetConnection instances created in this
        /// SWF file use AMF3 serialization, so you don&apos;t need to tell Flash Player
        /// which AMF encoding to use.Connecting exclusively to a server that was released prior to Flash Player 9,
        /// such as Flex 1.5 and Flash Media Server 2.
        /// In this scenario, set the static defaultObjectEncoding property to
        /// ObjectEncoding.AMF0. All NetConnection instances created in this
        /// SWF file use AMF0 serialization. You don&apos;t need to set the
        /// objectEncoding property. Connecting to multiple servers that use different encoding versions. Instead of
        /// using defaultObjectEncoding, set the object encoding on a per-connection
        /// basis using the objectEncoding property for each connection.
        /// Set it to ObjectEncoding.AMF0 to connect to
        /// servers that use AMF0 encoding, such as Flex 1.5 and Flash Media Server 2,
        /// and set it to ObjectEncoding.AMF3 to connect to
        /// servers that use AMF3 encoding, such as Flex 2.Changing defaultObjectEncoding does not affect existing
        /// NetConnection instances; it affects only instances that are created subsequently.Once the NetConnection instance is connected, its objectEncoding
        /// property is read-only.If you use the wrong encoding to connect to a server,
        /// Flash Player dispatches the netStatus event. The NetStatusEvent.info
        /// property contains an information object with a code property value of
        /// NetConnection.Connect.Failed, and a description explaining that the object
        /// encoding is incorrect.
        /// </summary>
        public extern virtual uint objectEncoding
        {
            [PageFX.AbcInstanceTrait(8)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(9)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// Determines whether native SSL is used for RTMPS instead of HTTPS,
        /// and whether the CONNECT method of tunneling is used to connect through a proxy
        /// server. Acceptable values are &quot;none&quot;,
        /// &quot;HTTP&quot;, &quot;CONNECT&quot;, and &quot;best&quot;.
        /// This property is used in Flex applications and Flash Media Server 2 applications.
        /// In Flash Player 9, this property is applicable only when
        /// using RTMP, RTMPS, or RTMPT. The CONNECT method is applicable only to
        /// users who are connected to the network by a proxy server.The proxyType property determines which fallback methods are tried if the
        /// initial connection attempt fails. You must set the proxyType property before
        /// calling the NetConnection.connect() method.
        /// This property determines whether to try to use native Transport Layer Security (TLS), and how
        /// to work with proxy servers that your application encounters when attempting to make a connection.In Flash Player 9, the default value for this property is &quot;none&quot;; if you do not
        /// change this value, Flash Player uses HTTPS tunneling for RTMPS.
        /// If the property is set to &quot;best&quot;, the optimal method of connecting is
        /// established, attempting the best method first and then falling back to other methods
        /// if they fail. For RTMPS connections, native SSL sockets are used by default,
        /// and a fallback to other methods is used if necessary.
        /// If the property is set to &quot;HTTP&quot;
        /// and a direct connection fails, the old method of HTTP tunneling is used.
        /// If the property is set to &quot;CONNECT&quot; and a direct connection fails,
        /// the CONNECT method of tunneling is used. If that fails, the connection will
        /// not fall back to HTTP tunneling.
        /// </summary>
        public extern virtual Avm.String proxyType
        {
            [PageFX.AbcInstanceTrait(10)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(11)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// If a successful connection is made, indicates the method that was used to make it:
        /// a direct connection, the CONNECT method, or HTTP tunneling.
        /// Possible values are &quot;none&quot;,
        /// &quot;HTTP&quot;, &quot;HTTPS&quot;, and &quot;CONNECT&quot;.
        /// This property is valid only when a NetConnection object is connected.
        /// This property is used in Flex applications and Flash Media Server applications.
        /// In Flash Player 9, this property is applicable only when
        /// RTMP, RTMPS, or RTMPT is used. The CONNECT method is applicable only to
        /// users who are connected to the network by a proxy server.
        /// You can read this property to determine which method of connection
        /// was used. This property returns &quot;none&quot; if a direct connection was made,
        /// &quot;HTTP&quot; if HTTP tunneling was used,
        /// &quot;HTTPS&quot; if secure HTTPS tunneling was used,
        /// and &quot;CONNECT&quot; if the
        /// proxy CONNECT method was used.
        /// </summary>
        public extern virtual Avm.String connectedProxyType
        {
            [PageFX.AbcInstanceTrait(12)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Indicates whether a secure connection was made using native Transport Layer Security (TLS)
        /// rather than HTTPS. This property is valid only when a NetConnection object is connected.
        /// </summary>
        public extern virtual bool usingTLS
        {
            [PageFX.AbcInstanceTrait(13)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual Avm.String protocol
        {
            [PageFX.AbcInstanceTrait(14)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual uint maxPeerConnections
        {
            [PageFX.AbcInstanceTrait(15)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(16)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual Avm.String nearID
        {
            [PageFX.AbcInstanceTrait(17)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual Avm.String farID
        {
            [PageFX.AbcInstanceTrait(18)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual Avm.String nearNonce
        {
            [PageFX.AbcInstanceTrait(19)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual Avm.String farNonce
        {
            [PageFX.AbcInstanceTrait(20)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual Avm.Array unconnectedPeerStreams
        {
            [PageFX.AbcInstanceTrait(21)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// The default object encoding (AMF version) for NetConnection objects created in the SWF file.
        /// When an object is written to or read from binary data, the defaultObjectEncoding
        /// property
        /// indicates which Action Message Format version should be used: the ActionScript 3.0 format
        /// or the ActionScript 1.0 and ActionScript 2.0 format.
        /// By default, NetConnection.defaultObjectEncoding is set to use the
        /// ActionScript 3.0 format, AMF3. Changing NetConnection.defaultObjectEncoding
        /// does not affect existing NetConnection instances; it affects only instances that
        /// are created subsequently.To set an object&apos;s encoding separately (rather than setting object encoding for the entire
        /// SWF file), set the objectEncoding property of the NetConnection object instead.For more detailed information, see the description of the objectEncoding
        /// property.
        /// </summary>
        public extern static uint defaultObjectEncoding
        {
            [PageFX.AbcClassTrait(0)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcClassTrait(1)]
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
        /// Closes the connection that was opened locally or with the server and dispatches
        /// the netStatus event
        /// with a code property of NetConnection.Connect.Closed.
        /// This method disconnects all NetStream objects running over this connection;
        /// any queued data that has not been sent is discarded. (To terminate
        /// streams
        /// without closing the connection, use NetStream.close().)
        /// If you call this method and then want to reconnect, you must recreate the NetStream object.
        /// </summary>
        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void close();

        /// <summary>
        /// Adds a context header to the AMF packet structure. This header is sent with
        /// every future AMF packet. If you call NetConnection.addHeader()
        /// using the same name, the new header replaces the existing header, and the new header
        /// persists for the duration of the NetConnection object. You can remove a header by
        /// calling NetConnection.addHeader() with the name of the header to remove
        /// an undefined object. This method is relevant when used with a server, such as Flex or
        /// Flash Media Server.
        /// </summary>
        /// <param name="operation">Identifies the header and the ActionScript object data associated with it.</param>
        /// <param name="mustUnderstand">
        /// (default = false)  A value of true indicates that the server must understand
        /// and process this header before it handles any of the following headers or messages.
        /// </param>
        /// <param name="param">(default = null)  Any ActionScript object.</param>
        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void addHeader(Avm.String operation, bool mustUnderstand, object param);

        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void addHeader(Avm.String operation, bool mustUnderstand);

        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void addHeader(Avm.String operation);

        /// <summary>
        /// Invokes a command or method on the server running Flash Media Server, or on an application server,
        /// to which the application instance is connected.
        /// You must create a server-side function to pass to this method.
        /// </summary>
        /// <param name="command">
        /// A method specified in the form [objectPath/]method. For example,
        /// the someObject/doSomething command tells the remote server
        /// to invoke the clientObject.someObject.doSomething() method, with all the optional
        /// ... arguments parameters. If the object path is missing,
        /// clientObject.doSomething() is invoked on the remote server.
        /// </param>
        /// <param name="responder">
        /// An optional object that is used to handle return values from the server.
        /// The Responder object can have two defined methods to handle the returned result:
        /// result and status. If an error is returned as the result,
        /// status is invoked; otherwise, result is invoked.
        /// </param>
        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void call(Avm.String command, flash.net.Responder responder);

        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void call(Avm.String command, flash.net.Responder responder, object rest0);

        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void call(Avm.String command, flash.net.Responder responder, object rest0, object rest1);

        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void call(Avm.String command, flash.net.Responder responder, object rest0, object rest1, object rest2);

        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void call(Avm.String command, flash.net.Responder responder, object rest0, object rest1, object rest2, object rest3);

        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void call(Avm.String command, flash.net.Responder responder, object rest0, object rest1, object rest2, object rest3, object rest4);

        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void call(Avm.String command, flash.net.Responder responder, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5);

        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void call(Avm.String command, flash.net.Responder responder, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6);

        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void call(Avm.String command, flash.net.Responder responder, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7);

        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void call(Avm.String command, flash.net.Responder responder, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8);

        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void call(Avm.String command, flash.net.Responder responder, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8, object rest9);

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
        /// <param name="command">
        /// Set this parameter to null if you are connecting to video without a server
        /// (that is, video on the local computer that is running the SWF file).
        /// If you are connecting to a server, set this parameter to the URI of the
        /// application on the server that runs when the connection is made. Use the following
        /// format (items in brackets are optional):protocol:[//host][:port]/appname/[instanceName]If the SWF file is served from the same host where the server is installed,
        /// you can omit the host parameter. If you omit the instanceName parameter,
        /// Flash Player connects to the application&apos;s default instance (_definst_).
        /// </param>
        [PageFX.AbcInstanceTrait(5)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void connect(Avm.String command);

        [PageFX.AbcInstanceTrait(5)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void connect(Avm.String command, object rest0);

        [PageFX.AbcInstanceTrait(5)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void connect(Avm.String command, object rest0, object rest1);

        [PageFX.AbcInstanceTrait(5)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void connect(Avm.String command, object rest0, object rest1, object rest2);

        [PageFX.AbcInstanceTrait(5)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void connect(Avm.String command, object rest0, object rest1, object rest2, object rest3);

        [PageFX.AbcInstanceTrait(5)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void connect(Avm.String command, object rest0, object rest1, object rest2, object rest3, object rest4);

        [PageFX.AbcInstanceTrait(5)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void connect(Avm.String command, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5);

        [PageFX.AbcInstanceTrait(5)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void connect(Avm.String command, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6);

        [PageFX.AbcInstanceTrait(5)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void connect(Avm.String command, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7);

        [PageFX.AbcInstanceTrait(5)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void connect(Avm.String command, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8);

        [PageFX.AbcInstanceTrait(5)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void connect(Avm.String command, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8, object rest9);


    }
}
