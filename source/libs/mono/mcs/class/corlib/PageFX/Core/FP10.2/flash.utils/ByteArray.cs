using System;
using System.Runtime.CompilerServices;

namespace flash.utils
{
    /// <summary>
    /// The ByteArray class provides methods and properties to optimize reading, writing,
    /// and working with binary data.
    /// </summary>
    [PageFX.AbcInstance(312)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class ByteArray : Avm.Object, flash.utils.IDataInput, flash.utils.IDataOutput
    {
        /// <summary>
        /// The length of the ByteArray object, in bytes.
        /// If the length is set to a value that is larger than the
        /// current length, Flash Player fills the right side with zeros.If the length is set to a value that is smaller than the
        /// current length, the array is truncated.
        /// </summary>
        public extern virtual uint length
        {
            [PageFX.AbcInstanceTrait(24)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(25)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// The number of bytes of data available for reading
        /// from the current position in the byte array to the
        /// end of the array.
        /// Use the bytesAvailable property in conjunction
        /// with the read methods each time you access a ByteArray object
        /// to ensure that you are reading valid data.
        /// </summary>
        public extern virtual uint bytesAvailable
        {
            [PageFX.AbcInstanceTrait(34)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Moves, or returns the current position, in bytes, of the file
        /// pointer into the ByteArray object. This is the
        /// point at which the next call to a read
        /// method starts reading or a write
        /// method starts writing.
        /// </summary>
        public extern virtual uint position
        {
            [PageFX.AbcInstanceTrait(35)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(36)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// Used to determine whether the ActionScript 3.0, ActionScript 2.0, or ActionScript 1.0 format should be
        /// used when writing to, or reading from, a ByteArray instance. The value is a
        /// constant from the ObjectEncoding class.
        /// </summary>
        public extern virtual uint objectEncoding
        {
            [PageFX.AbcInstanceTrait(37)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(38)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// Changes or reads the byte order for the data; either Endian.BIG_ENDIAN or
        /// Endian.LITTLE_ENDIAN.
        /// </summary>
        public extern virtual Avm.String endian
        {
            [PageFX.AbcInstanceTrait(39)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(40)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// Denotes the default object encoding for the ByteArray class to use for a new ByteArray instance.
        /// When you create a new ByteArray instance, the encoding on that instance starts
        /// with the value of defaultObjectEncoding.
        /// The defaultObjectEncoding property is initialized to ObjectEncoding.AMF3.
        /// When an object is written to or read from binary data, the objectEncoding value
        /// is used to determine whether the ActionScript 3.0, ActionScript2.0, or ActionScript 1.0 format should be used. The value is a
        /// constant from the ObjectEncoding class.
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

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ByteArray();

        /// <summary>
        /// Reads the number of data bytes, specified by the length parameter, from the byte stream.
        /// The bytes are read into the ByteArray object specified by the bytes parameter, starting
        /// at the position specified by offset.
        /// </summary>
        /// <param name="bytes">
        /// The ByteArray object to read
        /// data into.
        /// </param>
        /// <param name="offset">
        /// (default = 0)  The offset into bytes at which the data
        /// read should begin.
        /// </param>
        /// <param name="length">
        /// (default = 0)  The number of bytes to read.  The default value
        /// of 0 causes all available data to be read.
        /// </param>
        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void readBytes(flash.utils.ByteArray bytes, uint offset, uint length);

        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void readBytes(flash.utils.ByteArray bytes, uint offset);

        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void readBytes(flash.utils.ByteArray bytes);

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
        /// <param name="bytes">The ByteArray object.</param>
        /// <param name="offset">(default = 0)  A zero-based index indicating the position into the array to begin writing.</param>
        /// <param name="length">(default = 0)  An unsigned integer indicating how far into the buffer to write.</param>
        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void writeBytes(flash.utils.ByteArray bytes, uint offset, uint length);

        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void writeBytes(flash.utils.ByteArray bytes, uint offset);

        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void writeBytes(flash.utils.ByteArray bytes);

        /// <summary>
        /// Writes a Boolean value. A single byte is written according to the value parameter,
        /// either 1 if true or 0 if false.
        /// </summary>
        /// <param name="value">
        /// A Boolean value determining which byte is written. If the parameter is true,
        /// Flash Player writes a 1; if false, Flash Player writes a 0.
        /// </param>
        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void writeBoolean(bool value);

        /// <summary>
        /// Writes a byte to the byte stream.
        /// The low 8 bits of the
        /// parameter are used. The high 24 bits are ignored.
        /// </summary>
        /// <param name="value">A 32-bit integer. The low 8 bits are written to the byte stream.</param>
        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void writeByte(int value);

        /// <summary>
        /// Writes a 16-bit integer to the byte stream. The low 16 bits of the parameter are used.
        /// The high 16 bits are ignored.
        /// </summary>
        /// <param name="value">32-bit integer, whose low 16 bits are written to the byte stream.</param>
        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void writeShort(int value);

        /// <summary>Writes a 32-bit signed integer to the byte stream.</summary>
        /// <param name="value">An integer to write to the byte stream.</param>
        [PageFX.AbcInstanceTrait(5)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void writeInt(int value);

        /// <summary>Writes a 32-bit unsigned integer to the byte stream.</summary>
        /// <param name="value">An unsigned integer to write to the byte stream.</param>
        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void writeUnsignedInt(uint value);

        /// <summary>Writes an IEEE 754 single-precision (32-bit) floating-point number to the byte stream.</summary>
        /// <param name="value">A single-precision (32-bit) floating-point number.</param>
        [PageFX.AbcInstanceTrait(7)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void writeFloat(double value);

        /// <summary>Writes an IEEE 754 double-precision (64-bit) floating-point number to the byte stream.</summary>
        /// <param name="value">A double-precision (64-bit) floating-point number.</param>
        [PageFX.AbcInstanceTrait(8)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void writeDouble(double value);

        /// <summary>Writes a multibyte string to the byte stream using the specified character set.</summary>
        /// <param name="value">The string value to be written.</param>
        /// <param name="charSet">
        /// The string denoting the character set to use. Possible character set strings
        /// include &quot;shift-jis&quot;, &quot;cn-gb&quot;, &quot;iso-8859-1&quot;, and others.
        /// For a complete list, see Supported Character Sets.
        /// </param>
        [PageFX.AbcInstanceTrait(9)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void writeMultiByte(Avm.String value, Avm.String charSet);

        /// <summary>
        /// Writes a UTF-8 string to the byte stream. The length of the UTF-8 string in bytes
        /// is written first, as a 16-bit integer, followed by the bytes representing the
        /// characters of the string.
        /// </summary>
        /// <param name="value">The string value to be written.</param>
        [PageFX.AbcInstanceTrait(10)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void writeUTF(Avm.String value);

        /// <summary>
        /// Writes a UTF-8 string to the byte stream. Similar to the writeUTF() method,
        /// but writeUTFBytes() does not prefix the string with a 16-bit length word.
        /// </summary>
        /// <param name="value">The string value to be written.</param>
        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void writeUTFBytes(Avm.String value);

        /// <summary>
        /// Reads a Boolean value from the byte stream. A single byte is read,
        /// and true is returned if the byte is nonzero,
        /// false otherwise.
        /// </summary>
        /// <returns>Returns true if the byte is nonzero, false otherwise.</returns>
        [PageFX.AbcInstanceTrait(12)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool readBoolean();

        /// <summary>
        /// Reads a signed byte from the byte stream.
        /// The returned value is in the range -128 to 127.
        /// </summary>
        /// <returns>An integer between -128 and 127.</returns>
        [PageFX.AbcInstanceTrait(13)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int readByte();

        /// <summary>
        /// Reads an unsigned byte from the byte stream.
        /// The returned value is in the range 0 to 255.
        /// </summary>
        /// <returns>A 32-bit unsigned integer between 0 and 255.</returns>
        [PageFX.AbcInstanceTrait(14)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual uint readUnsignedByte();

        /// <summary>
        /// Reads a signed 16-bit integer from the byte stream.
        /// The returned value is in the range -32768 to 32767.
        /// </summary>
        /// <returns>A 16-bit signed integer between -32768 and 32767.</returns>
        [PageFX.AbcInstanceTrait(15)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int readShort();

        /// <summary>
        /// Reads an unsigned 16-bit integer from the byte stream.
        /// The returned value is in the range 0 to 65535.
        /// </summary>
        /// <returns>A 16-bit unsigned integer between 0 and 65535.</returns>
        [PageFX.AbcInstanceTrait(16)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual uint readUnsignedShort();

        /// <summary>
        /// Reads a signed 32-bit integer from the byte stream.
        /// The returned value is in the range -2147483648 to 2147483647.
        /// </summary>
        /// <returns>A 32-bit signed integer between -2147483648 and 2147483647.</returns>
        [PageFX.AbcInstanceTrait(17)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int readInt();

        /// <summary>
        /// Reads an unsigned 32-bit integer from the byte stream.
        /// The returned value is in the range 0 to 4294967295.
        /// </summary>
        /// <returns>A 32-bit unsigned integer between 0 and 4294967295.</returns>
        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual uint readUnsignedInt();

        /// <summary>Reads an IEEE 754 single-precision (32-bit) floating-point number from the byte stream.</summary>
        /// <returns>A single-precision (32-bit) floating-point number.</returns>
        [PageFX.AbcInstanceTrait(19)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual double readFloat();

        /// <summary>Reads an IEEE 754 double-precision (64-bit) floating-point number from the byte stream.</summary>
        /// <returns>A double-precision (64-bit) floating-point number.</returns>
        [PageFX.AbcInstanceTrait(20)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual double readDouble();

        /// <summary>
        /// Reads a multibyte string of specified length from the byte stream using the
        /// specified character set.
        /// </summary>
        /// <param name="length">The number of bytes from the byte stream to read.</param>
        /// <param name="charSet">
        /// The string denoting the character set to use to interpret the bytes.
        /// Possible character set strings include &quot;shift-jis&quot;, &quot;cn-gb&quot;,
        /// &quot;iso-8859-1&quot;, and others.
        /// For a complete list, see Supported Character Sets.
        /// </param>
        /// <returns>UTF-8 encoded string.</returns>
        [PageFX.AbcInstanceTrait(21)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String readMultiByte(uint length, Avm.String charSet);

        /// <summary>
        /// Reads a UTF-8 string from the byte stream.  The string
        /// is assumed to be prefixed with an unsigned short indicating
        /// the length in bytes.
        /// </summary>
        /// <returns>UTF-8 encoded  string.</returns>
        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String readUTF();

        /// <summary>
        /// Reads a sequence of UTF-8 bytes specified by the length
        /// parameter from the byte stream and returns a string.
        /// </summary>
        /// <param name="length">An unsigned short indicating the length of the UTF-8 bytes.</param>
        /// <returns>A string composed of the UTF-8 bytes of the specified length.</returns>
        [PageFX.AbcInstanceTrait(23)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String readUTFBytes(uint length);

        /// <summary>
        /// Writes an object into the byte array in AMF
        /// serialized format.
        /// </summary>
        /// <param name="@object">The object to serialize.</param>
        [PageFX.AbcInstanceTrait(26)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void writeObject(object @object);

        /// <summary>
        /// Reads an object from the byte array, encoded in AMF
        /// serialized format.
        /// </summary>
        /// <returns>The deserialized object.</returns>
        [PageFX.AbcInstanceTrait(27)]
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
        [PageFX.AbcInstanceTrait(28)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void deflate();

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
        [PageFX.AbcInstanceTrait(30)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void inflate();

        /// <summary>
        /// Converts the byte array to a string.
        /// If the data in the array begins with a Unicode byte order mark, Flash will honor that mark
        /// when converting to a string. If System.useCodePage is set to true, the player will treat
        /// the data in the array as being in the current system code page when converting.
        /// </summary>
        /// <returns>The string representation of the byte array.</returns>
        [PageFX.AbcInstanceTrait(32)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String toString();

        [PageFX.AbcInstanceTrait(41)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void clear();

        /// <summary>
        /// Compresses the byte array using the zlib compressed data format.
        /// The entire byte array is compressed.
        /// After the call, the length property of the ByteArray is set to the new length.
        /// The position property is set to the end of the byte array.The zlib compressed data format is described at
        /// http://www.ietf.org/rfc/rfc1950.txt.
        /// </summary>
        [PageFX.AbcInstanceTrait(42)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void compress();

        /// <summary>
        /// Decompresses the byte array.  The byte array
        /// must have been compressed using the
        /// zlib compressed data format.
        /// After the call, the length property of the ByteArray is set to the new length.
        /// The position property is set to 0.The zlib compressed data format is described at
        /// http://www.ietf.org/rfc/rfc1950.txt.
        /// </summary>
        [PageFX.AbcInstanceTrait(43)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void uncompress();


    }
}
