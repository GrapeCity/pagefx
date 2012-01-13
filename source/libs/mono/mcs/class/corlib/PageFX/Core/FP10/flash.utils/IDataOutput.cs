using System;
using System.Runtime.CompilerServices;

namespace flash.utils
{
    /// <summary>
    /// The IDataOutput interface provides a set of methods for writing binary data.
    /// This interface is the I/O counterpart to the IDataInput interface, which
    /// reads binary data. The IDataOutput interface is implemented by the FileStream, Socket
    /// and ByteArray classes.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public interface IDataOutput
    {
        Avm.String endian
        {
            [PageFX.ABC]
            [PageFX.QName("endian", "flash.utils:IDataOutput", "public")]
            [PageFX.FP9]
            get;
            [PageFX.ABC]
            [PageFX.QName("endian", "flash.utils:IDataOutput", "public")]
            [PageFX.FP9]
            set;
        }

        uint objectEncoding
        {
            [PageFX.ABC]
            [PageFX.QName("objectEncoding", "flash.utils:IDataOutput", "public")]
            [PageFX.FP9]
            get;
            [PageFX.ABC]
            [PageFX.QName("objectEncoding", "flash.utils:IDataOutput", "public")]
            [PageFX.FP9]
            set;
        }

        /// <summary>Writes a multibyte string to the file stream, byte stream, or byte array, using the specified character set.</summary>
        /// <param name="arg0">The string value to be written.</param>
        /// <param name="arg1">
        /// The string denoting the character set to use. Possible character set strings
        /// include &quot;shift-jis&quot;, &quot;cn-gb&quot;, &quot;iso-8859-1&quot; and others.
        /// For a complete list, see Supported Character Sets.
        /// </param>
        [PageFX.ABC]
        [PageFX.QName("writeMultiByte", "flash.utils:IDataOutput", "public")]
        [PageFX.FP9]
        void writeMultiByte(Avm.String arg0, Avm.String arg1);

        /// <summary>
        /// Writes a UTF-8 string. Similar to writeUTF(),
        /// but does not prefix the string with a 16-bit length word.
        /// </summary>
        /// <param name="arg0">The string value to be written.</param>
        [PageFX.ABC]
        [PageFX.QName("writeUTFBytes", "flash.utils:IDataOutput", "public")]
        [PageFX.FP9]
        void writeUTFBytes(Avm.String arg0);

        /// <summary>
        /// Writes a 16-bit integer. The low 16 bits of the parameter are used.
        /// The high 16 bits are ignored.
        /// </summary>
        /// <param name="arg0">A byte value as an integer.</param>
        [PageFX.ABC]
        [PageFX.QName("writeShort", "flash.utils:IDataOutput", "public")]
        [PageFX.FP9]
        void writeShort(int arg0);

        /// <summary>
        /// Writes a byte.
        /// The low 8 bits of the
        /// parameter are used. The high 24 bits are ignored.
        /// </summary>
        /// <param name="arg0">A byte value as an integer.</param>
        [PageFX.ABC]
        [PageFX.QName("writeByte", "flash.utils:IDataOutput", "public")]
        [PageFX.FP9]
        void writeByte(int arg0);

        /// <summary>
        /// Writes a UTF-8 string to the file stream, byte stream, or byte array. The length of the UTF-8 string in bytes
        /// is written first, as a 16-bit integer, followed by the bytes representing the
        /// characters of the string.
        /// </summary>
        /// <param name="arg0">The string value to be written.</param>
        [PageFX.ABC]
        [PageFX.QName("writeUTF", "flash.utils:IDataOutput", "public")]
        [PageFX.FP9]
        void writeUTF(Avm.String arg0);

        /// <summary>
        /// Writes a Boolean value. A single byte is written according to the value parameter,
        /// either 1 if true or 0 if false.
        /// </summary>
        /// <param name="arg0">
        /// A Boolean value determining which byte is written. If the parameter is true
        /// Flash Player writes a 1; if false, Flash Player writes a 0.
        /// </param>
        [PageFX.ABC]
        [PageFX.QName("writeBoolean", "flash.utils:IDataOutput", "public")]
        [PageFX.FP9]
        void writeBoolean(bool arg0);

        /// <summary>
        /// Writes a sequence of length bytes from the
        /// specified byte array, bytes,
        /// starting offset(zero-based index) bytes
        /// into the file stream, byte stream, or byte array.
        /// If the length parameter is omitted, the default
        /// length of 0 is used; Flash Player writes the entire buffer starting at
        /// offset.
        /// If the offset parameter is also omitted, the entire buffer is
        /// written. If the offset or length parameter
        /// is out of range, they are clamped to the beginning and end
        /// of the bytes array.
        /// </summary>
        /// <param name="arg0">The byte array to write.</param>
        /// <param name="arg1">(default = 0)  A zero-based index indicating the position into the array to begin writing.</param>
        /// <param name="arg2">(default = 0)  An unsigned integer indicating how far into the buffer to write.</param>
        [PageFX.ABC]
        [PageFX.QName("writeBytes", "flash.utils:IDataOutput", "public")]
        [PageFX.FP9]
        void writeBytes(ByteArray arg0, uint arg1, uint arg2);

        [PageFX.ABC]
        [PageFX.QName("writeBytes", "flash.utils:IDataOutput", "public")]
        [PageFX.FP9]
        void writeBytes(ByteArray arg0, uint arg1);

        [PageFX.ABC]
        [PageFX.QName("writeBytes", "flash.utils:IDataOutput", "public")]
        [PageFX.FP9]
        void writeBytes(ByteArray arg0);

        /// <summary>Writes a 32-bit signed integer.</summary>
        /// <param name="arg0">A byte value as a signed integer.</param>
        [PageFX.ABC]
        [PageFX.QName("writeInt", "flash.utils:IDataOutput", "public")]
        [PageFX.FP9]
        void writeInt(int arg0);

        /// <summary>Writes an IEEE 754 double-precision (64-bit) floating point number.</summary>
        /// <param name="arg0">A double-precision (64-bit) floating point number.</param>
        [PageFX.ABC]
        [PageFX.QName("writeDouble", "flash.utils:IDataOutput", "public")]
        [PageFX.FP9]
        void writeDouble(double arg0);

        /// <summary>Writes a 32-bit unsigned integer.</summary>
        /// <param name="arg0">A byte value as an unsigned integer.</param>
        [PageFX.ABC]
        [PageFX.QName("writeUnsignedInt", "flash.utils:IDataOutput", "public")]
        [PageFX.FP9]
        void writeUnsignedInt(uint arg0);

        /// <summary>Writes an IEEE 754 single-precision (32-bit) floating point number.</summary>
        /// <param name="arg0">A single-precision (32-bit) floating point number.</param>
        [PageFX.ABC]
        [PageFX.QName("writeFloat", "flash.utils:IDataOutput", "public")]
        [PageFX.FP9]
        void writeFloat(double arg0);

        /// <summary>
        /// Writes an object to the file stream, byte stream, or byte array, in AMF serialized
        /// format.
        /// </summary>
        /// <param name="arg0">the object to be serialized</param>
        [PageFX.ABC]
        [PageFX.QName("writeObject", "flash.utils:IDataOutput", "public")]
        [PageFX.FP9]
        void writeObject(object arg0);
    }
}
