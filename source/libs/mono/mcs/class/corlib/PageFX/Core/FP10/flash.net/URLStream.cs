using System;
using System.Runtime.CompilerServices;

namespace flash.net
{
    /// <summary>
    /// The URLStream class provides low-level access to
    /// downloading URLs. Data is made available to application code
    /// immediately as it is downloaded, instead of waiting until
    /// the entire file is complete as with URLLoader.
    /// The URLStream class also lets you close a stream
    /// before it finishes downloading.
    /// The contents of the downloaded file are made available as raw binary data.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class URLStream : flash.events.EventDispatcher, flash.utils.IDataInput
    {
        public extern virtual bool connected
        {
            [PageFX.ABC]
            [PageFX.FP9]
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

        public extern virtual Avm.String endian
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

        public extern virtual uint bytesAvailable
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [PageFX.Event("progress")]
        public event flash.events.ProgressEventHandler progress
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

        [PageFX.Event("ioError")]
        public event flash.events.IOErrorEventHandler ioError
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

        [PageFX.Event("complete")]
        public event flash.events.EventHandler complete
        {
            add { }
            remove { }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern URLStream();

        /// <summary>
        /// Reads an unsigned 32-bit integer from the stream.
        /// The returned value is in the range 0...4294967295.
        /// </summary>
        /// <returns>uint</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual uint readUnsignedInt();

        /// <summary>Reads an IEEE 754 double-precision floating-point number from the stream.</summary>
        /// <returns>Number</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual double readDouble();

        /// <summary>Reads an IEEE 754 single-precision floating-point number from the stream.</summary>
        /// <returns>Number</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual double readFloat();

        /// <summary>
        /// Reads a Boolean value from the stream. A single byte is read,
        /// and true is returned if the byte is nonzero,
        /// false otherwise.
        /// </summary>
        /// <returns>Boolean</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool readBoolean();

        /// <summary>
        /// Reads a signed 16-bit integer from the stream.
        /// The returned value is in the range -32768...32767.
        /// </summary>
        /// <returns>int</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int readShort();

        /// <summary>
        /// Reads an unsigned 16-bit integer from the stream.
        /// The returned value is in the range 0...65535.
        /// </summary>
        /// <returns>uint</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual uint readUnsignedShort();

        /// <summary>
        /// Reads an unsigned byte from the stream.
        /// The returned value is in the range 0...255.
        /// </summary>
        /// <returns>uint</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual uint readUnsignedByte();

        /// <summary>Reads an object from the socket, encoded in Action Message Format (AMF).</summary>
        /// <returns>The deserialized object.</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual object readObject();

        /// <summary>
        /// Begins downloading the URL specified in the request parameter.
        /// Note: If a file being loaded contains non-ASCII characters
        /// (as found in many non-English languages), it is recommended that you save the file
        /// with UTF-8 or UTF-16 encoding, as opposed to a non-Unicode format like ASCII.If the loading operation fails immediately, an IOError or SecurityError
        /// (including the local file security error) exception is thrown describing the failure.
        /// Otherwise, an open event is dispatched if the URL download
        /// starts downloading successfully, or an error event is dispatched if an error occurs.When using this method, consider the Adobe® Flash® Player security model:Data loading is not allowed if the calling SWF file is in the
        /// local-with-file-system sandbox and the target resource is from a network sandbox.Data loading is also not allowed if the calling SWF file is from a network sandbox and
        /// the target resource is local.By default, the URL you load must be in exactly the same domain as the calling SWF
        /// file. For example, a SWF file at www.adobe.com can load data only from sources
        /// that are also at www.adobe.com. To load data from a different domain, put a
        /// cross-domain policy file on the server hosting the SWF file.You can prevent a SWF file from using this method by setting the
        /// allowNetworking parameter of the the object and embed
        /// tags in the HTML page that contains the SWF content.However, in the Apollo runtime, content in the application security sandbox (content
        /// installed with the Apollo application) are not restricted by these security limitations.For more information, see the following:The &quot;Understanding AIR Security&quot; section of the &quot;Getting started with Adobe AIR&quot; chapter
        /// in the Developing AIR Applications book.The security chapter in the
        /// Programming ActionScript 3.0 book and the latest comments on LiveDocsThe Flash Player 9 Security white paper
        /// </summary>
        /// <param name="arg0">
        /// A URLRequest object specifying the URL to download. If the value of
        /// this parameter or the URLRequest.url property of the URLRequest object
        /// passed are null, Flash Player throws a null pointer error.
        /// </param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void load(URLRequest arg0);

        /// <summary>
        /// Reads a multibyte string of specified length from the byte stream using the
        /// specified character set.
        /// </summary>
        /// <param name="arg0">The number of bytes from the byte stream to read.</param>
        /// <param name="arg1">
        /// The string denoting the character set to use to interpret the bytes.
        /// Possible character set strings include &quot;shift_jis&quot;, &quot;CN-GB&quot;,
        /// &quot;iso-8859-1&quot;, and others.
        /// For a complete list, see Supported Character Sets.
        /// </param>
        /// <returns>UTF-8 encoded string.</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String readMultiByte(uint arg0, Avm.String arg1);

        /// <summary>
        /// Reads a UTF-8 string from the stream.  The string
        /// is assumed to be prefixed with an unsigned short indicating
        /// the length in bytes.
        /// </summary>
        /// <returns>String</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String readUTF();

        /// <summary>
        /// Reads a signed 32-bit integer from the stream.
        /// The returned value is in the range -2147483648...2147483647.
        /// </summary>
        /// <returns>int</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int readInt();

        /// <summary>
        /// Reads a sequence of length UTF-8
        /// bytes from the stream, and returns a string.
        /// </summary>
        /// <param name="arg0">A sequence of UTF-8 bytes.</param>
        /// <returns>String</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String readUTFBytes(uint arg0);

        /// <summary>
        /// Reads length bytes of data from the stream.
        /// The bytes are read into the ByteArray object specified
        /// by bytes, starting offset bytes into
        /// the ByteArray object.
        /// </summary>
        /// <param name="arg0">
        /// The ByteArray object to read
        /// data into.
        /// </param>
        /// <param name="arg1">
        /// (default = 0)  The offset into bytes at which data
        /// read should begin.  Defaults to 0.
        /// </param>
        /// <param name="arg2">
        /// (default = 0)  The number of bytes to read.  The default value
        /// of 0 will cause all available data to be read.
        /// </param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void readBytes(flash.utils.ByteArray arg0, uint arg1, uint arg2);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void readBytes(flash.utils.ByteArray arg0, uint arg1);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void readBytes(flash.utils.ByteArray arg0);

        /// <summary>
        /// Reads a signed byte from the stream.
        /// The returned value is in the range -128...127.
        /// </summary>
        /// <returns>int</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int readByte();

        /// <summary>
        /// Immediately closes the stream and
        /// cancels the download operation.
        /// No data can be read from the stream after the close() method is called.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void close();
    }
}
