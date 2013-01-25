using System;
using System.Runtime.CompilerServices;

namespace flash.net
{
    /// <summary>
    /// The Socket class enables code to make
    /// socket connections and to read and write raw binary data.  It is similar
    /// to XMLSocket but does not dictate the format of
    /// the received or transmitted data.
    /// </summary>
    [PageFX.AbcInstance(148)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class Socket : flash.events.EventDispatcher, flash.utils.IDataInput, flash.utils.IDataOutput
    {
        public extern virtual uint timeout
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

        /// <summary>
        /// The number of bytes of data available for reading in the input buffer.
        /// Your code must access bytesAvailable to ensure
        /// that sufficient data is available before trying to read
        /// it with one of the read methods.
        /// </summary>
        public extern virtual uint bytesAvailable
        {
            [PageFX.AbcInstanceTrait(35)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Indicates whether this Socket object is currently connected.
        /// A call to this property returns a value of true if the socket
        /// is currently connected, or false otherwise.
        /// </summary>
        public extern virtual bool connected
        {
            [PageFX.AbcInstanceTrait(36)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>Controls the version of AMF used when writing or reading an object.</summary>
        public extern virtual uint objectEncoding
        {
            [PageFX.AbcInstanceTrait(42)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(43)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// Indicates the byte order for the data; possible values are
        /// constants from the flash.utils.Endian class,
        /// Endian.BIG_ENDIAN or Endian.LITTLE_ENDIAN.
        /// </summary>
        public extern virtual Avm.String endian
        {
            [PageFX.AbcInstanceTrait(44)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(45)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [PageFX.Event("securityError")]
        public event flash.events.SecurityErrorEventHandler securityError
        {
            add { }
            remove { }
        }

        [PageFX.Event("socketData")]
        public event flash.events.ProgressEventHandler socketData
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
        public extern Socket(Avm.String host, int port);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Socket(Avm.String host);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Socket();

        /// <summary>
        /// Connects the socket to the specified host and port.
        /// If the connection fails immediately, either an event is dispatched
        /// or an exception is thrown: an error event is dispatched if a host was
        /// specified, and an exception is thrown if no host was specified.
        /// Otherwise, the status of the connection is reported by an event.
        /// If the socket is already connected, the existing connection is closed first.
        /// By default, the value you pass for host must be in
        /// the same domain and the value you pass for port must be 1024 or higher.
        /// For example, a SWF file at adobe.com can connect only to a server daemon running at
        /// adobe.com. If you want to connect to a socket on a different host than the one from which
        /// the connecting SWF file was served, or if you want to connect to a port lower than 1024 on any host,
        /// you must obtain an xmlsocket: policy file from the host to which you are
        /// connecting. For more information, see the &quot;Flash Player Security&quot; chapter in Programming
        /// ActionScript 3.0.
        /// </summary>
        /// <param name="host">
        /// The name of the host to connect to. If no host is specified,
        /// the host that is contacted is the host where the calling SWF file
        /// resides. If you do not specify a host, use an event listener to
        /// determine whether the connection was successful.
        /// </param>
        /// <param name="port">The port number to connect to.</param>
        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void connect(Avm.String host, int port);

        /// <summary>
        /// Reads the number of data bytes specified by the length
        /// parameter from the socket. The bytes are read into the specified byte
        /// array, starting at the position indicated by offset.
        /// </summary>
        /// <param name="bytes">The ByteArray object to read data into.</param>
        /// <param name="offset">
        /// (default = 0)  The offset at which data reading should begin in the byte
        /// array.
        /// </param>
        /// <param name="length">
        /// (default = 0)  The number of bytes to read. The default value of 0 causes
        /// all available data to be read.
        /// </param>
        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void readBytes(flash.utils.ByteArray bytes, uint offset, uint length);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void readBytes(flash.utils.ByteArray bytes, uint offset);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void readBytes(flash.utils.ByteArray bytes);

        /// <summary>
        /// Writes a sequence of bytes from the specified byte array. The write
        /// operation starts at the position specified by offset.
        /// If you omit the length parameter the default
        /// length of 0 causes the method to write the entire buffer starting at
        /// offset.If you also omit the offset parameter, the entire buffer is written.If offset or length
        /// is out of range, they are adjusted to match the beginning and end
        /// of the bytes array.Note: Data written by this method is not
        /// immediately transmitted; it is queued until the flush() method is called.
        /// </summary>
        /// <param name="bytes">The ByteArray object to write data from.</param>
        /// <param name="offset">
        /// (default = 0)  The zero-based offset into the bytes ByteArray
        /// object at which data writing should begin.
        /// </param>
        /// <param name="length">
        /// (default = 0)  The number of bytes to write.  The default value of 0 causes
        /// the entire buffer to be written, starting at the value specified by
        /// the offset parameter.
        /// </param>
        [PageFX.AbcInstanceTrait(12)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void writeBytes(flash.utils.ByteArray bytes, uint offset, uint length);

        [PageFX.AbcInstanceTrait(12)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void writeBytes(flash.utils.ByteArray bytes, uint offset);

        [PageFX.AbcInstanceTrait(12)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void writeBytes(flash.utils.ByteArray bytes);

        /// <summary>
        /// Writes a Boolean value to the socket. This method writes a single byte,
        /// with either a value of 1 (true) or 0 (false).
        /// Note: Data written by this method is not
        /// immediately transmitted; it is queued until the flush() method is called.
        /// </summary>
        /// <param name="value">The value to write to the socket: 1 (true) or 0 (false).</param>
        [PageFX.AbcInstanceTrait(13)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void writeBoolean(bool value);

        /// <summary>
        /// Writes a byte to the socket.
        /// Note: Data written by this method is not
        /// immediately transmitted; it is queued until the flush() method is called.
        /// </summary>
        /// <param name="value">
        /// The value to write to the socket. The low 8 bits of the
        /// value are used; the high 24 bits are ignored.
        /// </param>
        [PageFX.AbcInstanceTrait(14)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void writeByte(int value);

        /// <summary>
        /// Writes a 16-bit integer to the socket. The bytes written are as follows:
        /// (v &gt;&gt; 8) &amp; 0xff v &amp; 0xffThe low 16 bits of the parameter are used; the high 16 bits
        /// are ignored.Note: Data written by this method is not
        /// immediately transmitted; it is queued until the flush() method is called.
        /// </summary>
        /// <param name="value">The value to write to the socket.</param>
        [PageFX.AbcInstanceTrait(15)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void writeShort(int value);

        /// <summary>
        /// Writes a 32-bit signed integer to the socket.
        /// Note: Data written by this method is not
        /// immediately transmitted; it is queued until the flush() method is called.
        /// </summary>
        /// <param name="value">The value to write to the socket.</param>
        [PageFX.AbcInstanceTrait(16)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void writeInt(int value);

        /// <summary>
        /// Writes a 32-bit unsigned integer to the socket.
        /// Note: Data written by this method is not
        /// immediately transmitted; it is queued until the flush() method is called.
        /// </summary>
        /// <param name="value">The value to write to the socket.</param>
        [PageFX.AbcInstanceTrait(17)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void writeUnsignedInt(uint value);

        /// <summary>
        /// Writes an IEEE 754 single-precision floating-point number to the socket.
        /// Note: Data written by this method is not
        /// immediately transmitted; it is queued until the flush() method is called.
        /// </summary>
        /// <param name="value">The value to write to the socket.</param>
        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void writeFloat(double value);

        /// <summary>
        /// Writes an IEEE 754 double-precision floating-point number to the socket.
        /// Note: Data written by this method is not
        /// immediately transmitted; it is queued until the flush() method is called.
        /// </summary>
        /// <param name="value">The value to write to the socket.</param>
        [PageFX.AbcInstanceTrait(19)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void writeDouble(double value);

        /// <summary>
        /// Writes a multibyte string from the byte stream, using the specified character set.
        /// Note: Data written by this method is not
        /// immediately transmitted; it is queued until the flush() method is called.
        /// </summary>
        /// <param name="value">The string value to be written.</param>
        /// <param name="charSet">
        /// The string denoting the character set to use to interpret the bytes.
        /// Possible character set strings include &quot;shift_jis&quot;, &quot;CN-GB&quot;,
        /// and &quot;iso-8859-1&quot;. For a complete list, see
        /// Supported Character Sets.
        /// </param>
        [PageFX.AbcInstanceTrait(20)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void writeMultiByte(Avm.String value, Avm.String charSet);

        /// <summary>
        /// Writes the following data to the socket: a 16-bit unsigned integer, which
        /// indicates the length of the specified UTF-8 string in bytes, followed by
        /// the string itself.
        /// Before writing the string, the method calculates the number of bytes
        /// needed to represent all characters of the string.Note: Data written by this method is not
        /// immediately transmitted; it is queued until the flush() method is called.
        /// </summary>
        /// <param name="value">The string to write to the socket.</param>
        [PageFX.AbcInstanceTrait(21)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void writeUTF(Avm.String value);

        /// <summary>
        /// Writes a UTF-8 string to the socket.
        /// Note: Data written by this method is not
        /// immediately transmitted; it is queued until the flush()
        /// method is called.
        /// </summary>
        /// <param name="value">The string to write to the socket.</param>
        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void writeUTFBytes(Avm.String value);

        /// <summary>
        /// Reads a Boolean value from the socket. After reading a single byte, the
        /// method returns true if the byte is nonzero, and
        /// false otherwise.
        /// </summary>
        /// <returns>
        /// A value of true if the byte read is nonzero,
        /// otherwise false.
        /// </returns>
        [PageFX.AbcInstanceTrait(23)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool readBoolean();

        /// <summary>Reads a signed byte from the socket.</summary>
        /// <returns>A value from -128 to 127.</returns>
        [PageFX.AbcInstanceTrait(24)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int readByte();

        /// <summary>Reads an unsigned byte from the socket.</summary>
        /// <returns>A value from 0 to 255.</returns>
        [PageFX.AbcInstanceTrait(25)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual uint readUnsignedByte();

        /// <summary>Reads a signed 16-bit integer from the socket.</summary>
        /// <returns>A value from -32768 to 32767.</returns>
        [PageFX.AbcInstanceTrait(26)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int readShort();

        /// <summary>Reads an unsigned 16-bit integer from the socket.</summary>
        /// <returns>A value from 0 to 65535.</returns>
        [PageFX.AbcInstanceTrait(27)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual uint readUnsignedShort();

        /// <summary>Reads a signed 32-bit integer from the socket.</summary>
        /// <returns>A value from -2147483648 to 2147483647.</returns>
        [PageFX.AbcInstanceTrait(28)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int readInt();

        /// <summary>Reads an unsigned 32-bit integer from the socket.</summary>
        /// <returns>A value from 0 to 4294967295.</returns>
        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual uint readUnsignedInt();

        /// <summary>Reads an IEEE 754 single-precision floating-point number from the socket.</summary>
        /// <returns>Number</returns>
        [PageFX.AbcInstanceTrait(30)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual double readFloat();

        /// <summary>Reads an IEEE 754 double-precision floating-point number from the socket.</summary>
        /// <returns>Number</returns>
        [PageFX.AbcInstanceTrait(31)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual double readDouble();

        /// <summary>Reads a multibyte string from the byte stream, using the specified character set.</summary>
        /// <param name="length">The number of bytes from the byte stream to read.</param>
        /// <param name="charSet">
        /// The string denoting the character set to use to interpret the bytes.
        /// Possible character set strings include &quot;shift_jis&quot;, &quot;CN-GB&quot;, and
        /// &quot;iso-8859-1&quot;.
        /// For a complete list, see Supported Character Sets.
        /// </param>
        /// <returns>A UTF-8 encoded string.</returns>
        [PageFX.AbcInstanceTrait(32)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String readMultiByte(uint length, Avm.String charSet);

        /// <summary>
        /// Reads a UTF-8 string from the socket.  The string is assumed to be prefixed
        /// with an unsigned short integer that indicates the length in bytes.
        /// </summary>
        /// <returns>String</returns>
        [PageFX.AbcInstanceTrait(33)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String readUTF();

        /// <summary>
        /// Reads the number of UTF-8 data bytes specified by the length
        /// parameter from the socket, and returns a string.
        /// </summary>
        /// <param name="length">The number of bytes to read.</param>
        /// <returns>String</returns>
        [PageFX.AbcInstanceTrait(34)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String readUTFBytes(uint length);

        /// <summary>
        /// Closes the socket. You cannot read or write any data after the close() method
        /// has been called.
        /// The close event is dispatched only when the server
        /// closes the connection; it is not dispatched when you call the close() method.You can reuse the Socket object by calling the connect() method on it again.
        /// </summary>
        [PageFX.AbcInstanceTrait(37)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void close();

        /// <summary>
        /// Flushes any accumulated data in the socket&apos;s output buffer.
        /// Data written by the write methods is not
        /// immediately transmitted; it is queued until the
        /// flush() method is called.
        /// </summary>
        [PageFX.AbcInstanceTrait(39)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void flush();

        /// <summary>
        /// Write an object to the socket in AMF serialized format.
        /// Note: Data written by this method is not
        /// immediately transmitted; it is queued until the flush() method is called.
        /// </summary>
        /// <param name="@object">The object to be serialized.</param>
        [PageFX.AbcInstanceTrait(40)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void writeObject(object @object);

        /// <summary>Reads an object from the socket, encoded in AMF serialized format.</summary>
        /// <returns>The deserialized object</returns>
        [PageFX.AbcInstanceTrait(41)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual object readObject();


    }
}
