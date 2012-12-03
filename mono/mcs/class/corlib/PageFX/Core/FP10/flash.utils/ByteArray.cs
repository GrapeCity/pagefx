using System;
using System.Runtime.CompilerServices;

namespace flash.utils
{
    /// <summary>
    /// The ByteArray class provides methods and properties to optimize reading, writing,
    /// and working with binary data.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class ByteArray : Avm.Object, IDataInput, IDataOutput
    {
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

        public extern virtual uint position
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

        public extern virtual uint length
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

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ByteArray();

        /// <summary>
        /// Writes a UTF-8 string to the byte stream. Similar to the writeUTF() method,
        /// but writeUTFBytes() does not prefix the string with a 16-bit length word.
        /// </summary>
        /// <param name="arg0">The string value to be written.</param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void writeUTFBytes(Avm.String arg0);

        /// <summary>
        /// Reads a signed 16-bit integer from the byte stream.
        /// The returned value is in the range -32768 to 32767.
        /// </summary>
        /// <returns>A 16-bit signed integer between -32768 and 32767.</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int readShort();

        /// <summary>
        /// Writes a byte to the byte stream.
        /// The low 8 bits of the
        /// parameter are used. The high 24 bits are ignored.
        /// </summary>
        /// <param name="arg0">A 32-bit integer. The low 8 bits are written to the byte stream.</param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void writeByte(int arg0);

        /// <summary>Writes an IEEE 754 double-precision (64-bit) floating-point number to the byte stream.</summary>
        /// <param name="arg0">A double-precision (64-bit) floating-point number.</param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void writeDouble(double arg0);

        /// <summary>
        /// Reads an unsigned 16-bit integer from the byte stream.
        /// The returned value is in the range 0 to 65535.
        /// </summary>
        /// <returns>A 16-bit unsigned integer between 0 and 65535.</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual uint readUnsignedShort();

        /// <summary>Reads an IEEE 754 double-precision (64-bit) floating-point number from the byte stream.</summary>
        /// <returns>A double-precision (64-bit) floating-point number.</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual double readDouble();

        /// <summary>Writes a 32-bit signed integer to the byte stream.</summary>
        /// <param name="arg0">An integer to write to the byte stream.</param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void writeInt(int arg0);

        /// <summary>
        /// Reads an object from the byte array, encoded in AMF
        /// serialized format.
        /// </summary>
        /// <returns>The deserialized object.</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual object readObject();

        /// <summary>
        /// Compresses the byte array using the DEFLATE compression algorithm.
        /// The entire byte array is compressed.
        /// After the call, the length property of the ByteArray is set to the new length.
        /// The position property is set to the end of the byte array.The DEFLATE compression algorithm is described at
        /// http://www.ietf.org/rfc/rfc1951.txt.The DEFLATE compression algorithm is used in several compression
        /// formats, such as zlib, gzip, some zip implementations, and others. When data is
        /// compressed using one of those compression formats, in addition to storing
        /// the compressed version of the original data, the compression format data
        /// (for example, the .zip file) includes metadata information. Some examples of
        /// the types of metadata included in various file formats are file name,
        /// file modification date/time, original file size, optional comments, checksum
        /// data, and more.For example, when a ByteArray is compressed using the compress()
        /// method, the ByteArray is compressed using the zlib compressed data format.
        /// The resulting ByteArray is structured in a specific format. Certain bytes contain
        /// metadata about the compressed data, while other bytes contain the actual compressed
        /// version of the original ByteArray data. As defined by the zlib compressed data
        /// format specification, those bytes (that is, the portion containing
        /// the compressed version of the original data) are compressed using the DEFLATE
        /// algorithm. Consequently those bytes are identical to the result of calling
        /// deflate() on the original ByteArray. However,
        /// the compress() result includes the extra metadata, while the
        /// deflate() result includes only the compressed version of the original
        /// ByteArray data and nothing else.Consequently, in order to use deflate() to compress a ByteArray instance&apos;s
        /// data in a specific format such as gzip or zip, you can&apos;t just call the
        /// instance&apos;s deflate() method. You must create a ByteArray structured
        /// according to the compression format&apos;s specification, including the appropriate
        /// metadata as well as the compressed data obtained using the
        /// deflate() method. Likewise, in order to decode data compressed in a format such
        /// as gzip or zip, you can&apos;t simply use the inflate() method on that data. First
        /// you must separate the metadata from the compressed data, and you can then use the
        /// inflate() method to decompress the compressed data.
        /// </summary>
        [PageFX.ABC]
        [PageFX.QName("deflate", "http://www.adobe.com/2008/actionscript/Flash10/", "public")]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void deflate();

        /// <summary>
        /// Reads a Boolean value from the byte stream. A single byte is read,
        /// and true is returned if the byte is nonzero,
        /// false otherwise.
        /// </summary>
        /// <returns>Returns true if the byte is nonzero, false otherwise.</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool readBoolean();

        /// <summary>
        /// Decompresses the byte array. The byte array must have been previously
        /// compressed with the DEFLATE compression algorithm, as is used by the
        /// deflate() method.
        /// After the call, the length property of the ByteArray is set to the new length.
        /// The position property is set to 0.The DEFLATE compression algorithm is described at
        /// http://www.ietf.org/rfc/rfc1951.txt.In order to decode data compressed in a format that uses the DEFLATE compression algorithm,
        /// such as data in gzip or zip format, it will not work to call the inflate() method on
        /// a ByteArray containing the compression formation data. First you must separate the metadata that is
        /// included as part of the compressed data format from the actual compressed data. For more
        /// information, see the deflate() method description.
        /// </summary>
        [PageFX.ABC]
        [PageFX.QName("inflate", "http://www.adobe.com/2008/actionscript/Flash10/", "public")]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void inflate();

        /// <summary>
        /// Reads a UTF-8 string from the byte stream.  The string
        /// is assumed to be prefixed with an unsigned short indicating
        /// the length in bytes.
        /// </summary>
        /// <returns>UTF-8 encoded  string.</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String readUTF();

        /// <summary>
        /// Reads a sequence of UTF-8 bytes specified by the length
        /// parameter from the byte stream and returns a string.
        /// </summary>
        /// <param name="arg0">An unsigned short indicating the length of the UTF-8 bytes.</param>
        /// <returns>A string composed of the UTF-8 bytes of the specified length.</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String readUTFBytes(uint arg0);

        /// <summary>Writes an IEEE 754 single-precision (32-bit) floating-point number to the byte stream.</summary>
        /// <param name="arg0">A single-precision (32-bit) floating-point number.</param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void writeFloat(double arg0);

        /// <summary>Writes a multibyte string to the byte stream using the specified character set.</summary>
        /// <param name="arg0">The string value to be written.</param>
        /// <param name="arg1">
        /// The string denoting the character set to use. Possible character set strings
        /// include &quot;shift-jis&quot;, &quot;cn-gb&quot;, &quot;iso-8859-1&quot;, and others.
        /// For a complete list, see Supported Character Sets.
        /// </param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void writeMultiByte(Avm.String arg0, Avm.String arg1);

        /// <summary>
        /// Reads an unsigned 32-bit integer from the byte stream.
        /// The returned value is in the range 0 to 4294967295.
        /// </summary>
        /// <returns>A 32-bit unsigned integer between 0 and 4294967295.</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual uint readUnsignedInt();

        /// <summary>
        /// Reads a signed byte from the byte stream.
        /// The returned value is in the range -128 to 127.
        /// </summary>
        /// <returns>An integer between -128 and 127.</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int readByte();

        /// <summary>
        /// Writes a sequence of length bytes from the
        /// specified byte array, bytes,
        /// starting offset (zero-based index) bytes
        /// into the byte stream.
        /// If the length parameter is omitted, the default
        /// length of 0 is used; Flash Player writes the entire buffer starting at
        /// offset.
        /// If the offset parameter is also omitted, the entire buffer is
        /// written. If offset or length
        /// is out of range, they are clamped to the beginning and end
        /// of the bytes array.
        /// </summary>
        /// <param name="arg0">The ByteArray object.</param>
        /// <param name="arg1">(default = 0)  A zero-based index indicating the position into the array to begin writing.</param>
        /// <param name="arg2">(default = 0)  An unsigned integer indicating how far into the buffer to write.</param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void writeBytes(ByteArray arg0, uint arg1, uint arg2);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void writeBytes(ByteArray arg0, uint arg1);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void writeBytes(ByteArray arg0);

        [PageFX.ABC]
        [PageFX.QName("clear", "http://www.adobe.com/2008/actionscript/Flash10/", "public")]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void clear();

        /// <summary>
        /// Writes a UTF-8 string to the byte stream. The length of the UTF-8 string in bytes
        /// is written first, as a 16-bit integer, followed by the bytes representing the
        /// characters of the string.
        /// </summary>
        /// <param name="arg0">The string value to be written.</param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void writeUTF(Avm.String arg0);

        /// <summary>
        /// Writes a Boolean value. A single byte is written according to the value parameter,
        /// either 1 if true or 0 if false.
        /// </summary>
        /// <param name="arg0">
        /// A Boolean value determining which byte is written. If the parameter is true,
        /// Flash Player writes a 1; if false, Flash Player writes a 0.
        /// </param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void writeBoolean(bool arg0);

        /// <summary>
        /// Reads an unsigned byte from the byte stream.
        /// The returned value is in the range 0 to 255.
        /// </summary>
        /// <returns>A 32-bit unsigned integer between 0 and 255.</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual uint readUnsignedByte();

        /// <summary>Writes a 32-bit unsigned integer to the byte stream.</summary>
        /// <param name="arg0">An unsigned integer to write to the byte stream.</param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void writeUnsignedInt(uint arg0);

        /// <summary>
        /// Writes a 16-bit integer to the byte stream. The low 16 bits of the parameter are used.
        /// The high 16 bits are ignored.
        /// </summary>
        /// <param name="arg0">32-bit integer, whose low 16 bits are written to the byte stream.</param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void writeShort(int arg0);

        /// <summary>
        /// Compresses the byte array using the zlib compressed data format.
        /// The entire byte array is compressed.
        /// After the call, the length property of the ByteArray is set to the new length.
        /// The position property is set to the end of the byte array.The zlib compressed data format is described at
        /// http://www.ietf.org/rfc/rfc1950.txt.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void compress();

        /// <summary>
        /// Converts the byte array to a string.
        /// If the data in the array begins with a Unicode byte order mark, Flash will honor that mark
        /// when converting to a string. If System.useCodePage is set to true, the player will treat
        /// the data in the array as being in the current system code page when converting.
        /// </summary>
        /// <returns>The string representation of the byte array.</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String toString();

        /// <summary>Reads an IEEE 754 single-precision (32-bit) floating-point number from the byte stream.</summary>
        /// <returns>A single-precision (32-bit) floating-point number.</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual double readFloat();

        /// <summary>
        /// Reads a signed 32-bit integer from the byte stream.
        /// The returned value is in the range -2147483648 to 2147483647.
        /// </summary>
        /// <returns>A 32-bit signed integer between -2147483648 and 2147483647.</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int readInt();

        /// <summary>
        /// Reads a multibyte string of specified length from the byte stream using the
        /// specified character set.
        /// </summary>
        /// <param name="arg0">The number of bytes from the byte stream to read.</param>
        /// <param name="arg1">
        /// The string denoting the character set to use to interpret the bytes.
        /// Possible character set strings include &quot;shift-jis&quot;, &quot;cn-gb&quot;,
        /// &quot;iso-8859-1&quot;, and others.
        /// For a complete list, see Supported Character Sets.
        /// </param>
        /// <returns>UTF-8 encoded string.</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String readMultiByte(uint arg0, Avm.String arg1);

        /// <summary>
        /// Decompresses the byte array.  The byte array
        /// must have been compressed using the
        /// zlib compressed data format.
        /// After the call, the length property of the ByteArray is set to the new length.
        /// The position property is set to 0.The zlib compressed data format is described at
        /// http://www.ietf.org/rfc/rfc1950.txt.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void uncompress();

        /// <summary>
        /// Reads the number of data bytes, specified by the length parameter, from the byte stream.
        /// The bytes are read into the ByteArray object specified by the bytes parameter, starting
        /// at the position specified by offset.
        /// </summary>
        /// <param name="arg0">
        /// The ByteArray object to read
        /// data into.
        /// </param>
        /// <param name="arg1">
        /// (default = 0)  The offset into bytes at which the data
        /// read should begin.
        /// </param>
        /// <param name="arg2">
        /// (default = 0)  The number of bytes to read.  The default value
        /// of 0 causes all available data to be read.
        /// </param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void readBytes(ByteArray arg0, uint arg1, uint arg2);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void readBytes(ByteArray arg0, uint arg1);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void readBytes(ByteArray arg0);

        /// <summary>
        /// Writes an object into the byte array in AMF
        /// serialized format.
        /// </summary>
        /// <param name="arg0">The object to serialize.</param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void writeObject(object arg0);
    }
}
