using System;
using System.Runtime.CompilerServices;

namespace flash.net
{
    /// <summary>
    /// The XMLSocket class implements client sockets that let the computer that is running Flash Player communicate
    /// with a server computer identified by an IP address or domain name. The XMLSocket class is useful for
    /// client-server applications that require low latency, such as real-time chat systems. A traditional
    /// HTTP-based chat solution frequently polls the server and downloads new messages using an HTTP
    /// request. In contrast, an XMLSocket chat solution maintains an open connection to the server, which
    /// lets the server immediately send incoming messages without a request from the client.
    /// To use the XMLSocket class, the server computer must run a daemon that understands the protocol used
    /// by the XMLSocket class. The protocol is described in the following list:
    /// XML messages are sent over a full-duplex TCP/IP stream socket connection.Each XML message is a complete XML document, terminated by a zero (0) byte.An unlimited number of XML messages can be sent and received over a single XMLSocket
    /// connection.
    /// </summary>
    [PageFX.AbcInstance(229)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class XMLSocket : flash.events.EventDispatcher
    {
        public extern virtual int timeout
        {
            [PageFX.AbcInstanceTrait(7)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(8)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// Indicates whether this XMLSocket object is currently connected. You can also check
        /// whether the connection succeeded by registering for the connect
        /// event and ioError event.
        /// </summary>
        public extern virtual bool connected
        {
            [PageFX.AbcInstanceTrait(10)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
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

        [PageFX.Event("data")]
        public event flash.events.DataEventHandler data
        {
            add { }
            remove { }
        }

        [PageFX.Event("connect")]
        public event flash.events.EventHandler OnConnect
        {
            add { }
            remove { }
        }

        [PageFX.Event("close")]
        public event flash.events.EventHandler OnClose
        {
            add { }
            remove { }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern XMLSocket(Avm.String host, int port);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern XMLSocket(Avm.String host);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern XMLSocket();

        /// <summary>
        /// Establishes a connection to the specified Internet host using the specified TCP port. By default
        /// you can only connect to port 1024 or higher, unless you are using a policy file.
        /// If you specify null for the host parameter, the host
        /// contacted is the one where the SWF file calling XMLSocket.connect() resides.
        /// For example, if the SWF file was downloaded from www.adobe.com, specifying null
        /// for the host parameter is the same as entering the IP address for www.adobe.com.In SWF files running in a version of the player earlier than Flash Player 7,
        /// host must be in the same superdomain as the SWF file that is issuing this
        /// call. For example, a SWF file at www.adobe.com can send or receive variables from a SWF file at
        /// store.adobe.com because both files are in the same superdomain of adobe.com.In SWF files of any version running in Flash Player 7 or later,
        /// host must be in exactly the same domain. For example, a SWF file at www.adobe.com
        /// that is published for Flash Player 5, but is running in Flash Player 7 or later can send or receive
        /// variables only from SWF files that are also at www.adobe.com. If you want to send or receive variables
        /// from a different domain, you can place a cross-domain policy file on the server hosting
        /// the SWF file that is being accessed.You can prevent a SWF file from using this method by setting the
        /// allowNetworking parameter of the the object and embed
        /// tags in the HTML page that contains the SWF content.For more information, see the following:The security chapter in the
        /// Programming ActionScript 3.0 book and the latest comments on LiveDocsThe Flash Player 9 Security white paper
        /// </summary>
        /// <param name="host">
        /// A fully qualified DNS domain name or an IP address in the form
        /// aaa.bbb.ccc.ddd. You can also specify null to connect to the host server
        /// on which the SWF file resides. If the SWF file issuing this call is running in a web browser,
        /// host must be in the same domain as the SWF file.
        /// </param>
        /// <param name="port">
        /// The TCP port number on the host used to establish a connection. The port
        /// number must be 1024 or greater, unless a policy file is being used.
        /// </param>
        [PageFX.AbcInstanceTrait(5)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void connect(Avm.String host, int port);

        /// <summary>
        /// Converts the XML object or data specified in the object parameter
        /// to a string and transmits it to the server, followed by a zero (0) byte. If object is an XML object, the string is
        /// the XML textual representation of the XML object. The
        /// send operation is asynchronous; it returns immediately, but the data may be transmitted at a
        /// later time. The XMLSocket.send() method does not return a value indicating whether
        /// the data was successfully transmitted.
        /// If you do not connect the XMLSocket object to the server using
        /// XMLSocket.connect()), the XMLSocket.send()
        /// operation fails.
        /// </summary>
        /// <param name="@object">An XML object or other data to transmit to the server.</param>
        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void send(object @object);

        /// <summary>
        /// Closes the connection specified by the XMLSocket object.
        /// The close event is dispatched only when the server
        /// closes the connection; it is not dispatched when you call the close() method.
        /// </summary>
        [PageFX.AbcInstanceTrait(9)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void close();


    }
}
