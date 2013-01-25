using System;
using System.Runtime.CompilerServices;

namespace flash.net
{
    /// <summary>
    /// The URLLoader class downloads data from a URL
    /// as text, binary data, or URL-encoded variables. It is useful for downloading
    /// text files, XML, or other information to be used in a
    /// dynamic, data-driven application.
    /// </summary>
    [PageFX.AbcInstance(130)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class URLLoader : flash.events.EventDispatcher
    {
        /// <summary>
        /// The data received from the load operation. This property
        /// is populated only when the load operation is complete.
        /// The format of the data depends on the setting of the
        /// dataFormat property:
        /// If the dataFormat property is URLLoaderDataFormat.TEXT,
        /// the received data is a string containing the text of the loaded file.If the dataFormat property is URLLoaderDataFormat.BINARY,
        /// the received data is a ByteArray object containing the raw binary data.If the dataFormat property is URLLoaderDataFormat.VARIABLES,
        /// the received data is a URLVariables object containing the URL-encoded variables.
        /// </summary>
        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public object data;

        /// <summary>
        /// Controls whether the downloaded data is received as
        /// text (URLLoaderDataFormat.TEXT), raw binary data
        /// (URLLoaderDataFormat.BINARY), or URL-encoded variables
        /// (URLLoaderDataFormat.VARIABLES).
        /// If the value of the dataFormat property is URLLoaderDataFormat.TEXT,
        /// the received data is a string containing the text of the loaded file.If the value of the dataFormat property is URLLoaderDataFormat.BINARY,
        /// the received data is a ByteArray object containing the raw binary data.If the value of the dataFormat property is URLLoaderDataFormat.VARIABLES,
        /// the received data is a URLVariables object containing the URL-encoded variables.
        /// </summary>
        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        public Avm.String dataFormat;

        /// <summary>
        /// Indicates the number of bytes that have been loaded thus far
        /// during the load operation.
        /// </summary>
        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        public uint bytesLoaded;

        /// <summary>
        /// Indicates the total number of bytes in the downloaded data.
        /// This property contains 0 while the load operation is in progress
        /// and is populated when the operation is complete.
        /// </summary>
        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.FP9]
        public uint bytesTotal;

        [PageFX.Event("httpResponseStatus")]
        public event flash.events.HTTPStatusEventHandler httpResponseStatus
        {
            add { }
            remove { }
        }

        [PageFX.Event("httpStatus")]
        public event flash.events.HTTPStatusEventHandler httpStatus
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

        [PageFX.Event("progress")]
        public event flash.events.ProgressEventHandler progress
        {
            add { }
            remove { }
        }

        [PageFX.Event("complete")]
        public event flash.events.EventHandler complete
        {
            add { }
            remove { }
        }

        [PageFX.Event("open")]
        public event flash.events.EventHandler open
        {
            add { }
            remove { }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern URLLoader(flash.net.URLRequest request);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern URLLoader();

        [PageFX.AbcInstanceTrait(5)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override void addEventListener(Avm.String type, Avm.Function listener, bool useCapture, int priority, bool useWeakReference);

        /// <summary>
        /// Sends and loads data from the specified URL. The data can be received as
        /// text, raw binary data, or URL-encoded variables, depending on the
        /// value you set for the dataFormat property. Note that
        /// the default value of the dataFormat property is text.
        /// If you want to send data to the specified URL, you can set the data
        /// property in the URLRequest object.
        /// Note: If a file being loaded contains non-ASCII characters (as found
        /// in many non-English languages), it is recommended that you save the
        /// file with UTF-8 or UTF-16 encoding as opposed to a non-Unicode format
        /// like ASCII.By default, for Flash Player content and  AIR content not
        /// in the application security domain, the URL you load must be in exactly the same
        /// domain as the calling file. For example, a file at www.adobe.com can load data only from sources that are
        /// also at www.adobe.com. To load data from a different domain, put a
        /// cross-domain policy file on the server hosting the SWF file.In AIR applications, content in the application security sandbox can load content
        /// from any domain.When you use this method in content in security sandboxes other
        /// than then application security sandbox, consider the Flash PlayerAIR security model:When you use this class in Flash Player and in
        /// AIR application content in security sandboxes other than then application security sandbox,
        /// consider the Flash PlayerAIR security model:For Flash Player 8 and later:Data loading is not allowed if the calling file is in the local-with-file-system
        /// sandbox and the target resource is from a network sandbox.Data loading is also not allowed if the calling file is from a network sandbox and the
        /// target resource is local.For Flash Player 7 and later websites can permit cross-domain access to a resource through a
        /// cross-domain policy file. In files of any version running in Flash
        /// Player 7 and later, url must be in exactly the same domain. For example, a file
        /// at www.adobe.com can load data only from sources that are also at www.adobe.com.In SWF files that are running in a version of the player earlier than
        /// Flash Player 7, url must be in the same superdomain as the SWF file that is issuing
        /// this call. A superdomain is derived by removing the leftmost component of a file&apos;s URL.
        /// For example, a SWF file at www.adobe.com can load data from sources at store.adobe.com
        /// because both files are in the same superdomain of adobe.com.For more information, see the following:The security chapter in the
        /// Programming ActionScript 3.0 book and the latest comments on LiveDocsThe &quot;Understanding AIR Security&quot; section of the &quot;Getting started with Adobe AIR&quot; chapter
        /// in the Developing AIR Applications book.The Flash Player 9 Security white paper
        /// </summary>
        /// <param name="request">A URLRequest object specifying the URL to download.</param>
        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void load(flash.net.URLRequest request);

        /// <summary>
        /// Closes the load operation in progress.  Any load
        /// operation in progress is immediately terminated.
        /// If no URL is currently being streamed, an invalid stream error is thrown.
        /// </summary>
        [PageFX.AbcInstanceTrait(7)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void close();
    }
}
