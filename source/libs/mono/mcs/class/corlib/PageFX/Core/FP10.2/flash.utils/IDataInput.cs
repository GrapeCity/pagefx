using System;
using System.Runtime.CompilerServices;

namespace flash.utils
{
    /// <summary>
    /// The IDataInput interface provides a set of methods for reading binary data.
    /// This interface is the I/O counterpart to the IDataOutput interface, which
    /// writes binary data.
    /// </summary>
    [PageFX.AbcInstance(28)]
    [PageFX.ABC]
    [PageFX.FP9]
    public interface IDataInput
    {
        /// <summary>
        /// Returns the number of bytes of data available for reading
        /// in the input buffer.
        /// User code must call bytesAvailable to ensure
        /// that sufficient data is available before trying to read
        /// it with one of the read methods.
        /// </summary>
        uint bytesAvailable
        {
            [PageFX.AbcInstanceTrait(13)]
            [PageFX.ABC]
            [PageFX.QName("bytesAvailable", "flash.utils:IDataInput", "public")]
            [PageFX.FP9]
            get;
        }

        /// <summary>
        /// Used to determine whether the AMF3 or AMF0 format is used when writing or reading binary data using the
        /// readObject() method. The value is a constant from the ObjectEncoding class.
        /// </summary>
        uint objectEncoding
        {
            [PageFX.AbcInstanceTrait(15)]
            [PageFX.ABC]
            [PageFX.QName("objectEncoding", "flash.utils:IDataInput", "public")]
            [PageFX.FP9]
            get;
            [PageFX.AbcInstanceTrait(16)]
            [PageFX.ABC]
            [PageFX.QName("objectEncoding", "flash.utils:IDataInput", "public")]
            [PageFX.FP9]
            set;
        }

        /// <summary>
        /// The byte order for the data, either the BIG_ENDIAN or LITTLE_ENDIAN constant
        /// from the Endian class.
        /// </summary>
        Avm.String endian
        {
            [PageFX.AbcInstanceTrait(17)]
            [PageFX.ABC]
            [PageFX.QName("endian", "flash.utils:IDataInput", "public")]
            [PageFX.FP9]
            get;
            [PageFX.AbcInstanceTrait(18)]
            [PageFX.ABC]
            [PageFX.QName("endian", "flash.utils:IDataInput", "public")]
            [PageFX.FP9]
            set;
        }

        /// <summary>
        /// Reads the number of data bytes, specified by the length parameter,
        /// from the file stream, byte stream, or byte array. The bytes are read into the
        /// ByteArray objected specified by the bytes parameter, starting at
        /// the position specified by offset.
        /// </summary>
        /// <param name="bytes">
        /// The ByteArray object to read
        /// data into.
        /// </param>
        /// <param name="offset">
        /// (default = 0)  The offset into the bytes parameter at which data
        /// read should begin.
        /// </param>
        /// <param name="length">
        /// (default = 0)  The number of bytes to read.  The default value
        /// of 0 causes all available data to be read.
        /// </param>
        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.QName("readBytes", "flash.utils:IDataInput", "public")]
        [PageFX.FP9]
        void readBytes(flash.utils.ByteArray bytes, uint offset, uint length);

        /// <summary>
        /// Reads a Boolean value from the file stream, byte stream, or byte array. A single byte is read,
        /// and true is returned if the byte is nonzero,
        /// false otherwise.
        /// </summary>
        /// <returns>
        /// A Boolean value, true if the byte is nonzero,
        /// false otherwise.
        /// </returns>
        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.QName("readBoolean", "flash.utils:IDataInput", "public")]
        [PageFX.FP9]
        bool readBoolean();

        /// <summary>Reads a signed byte from the file stream, byte stream, or byte array.</summary>
        /// <returns>The returned value is in the range -128 to 127.</returns>
        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.QName("readByte", "flash.utils:IDataInput", "public")]
        [PageFX.FP9]
        int readByte();

        /// <summary>Reads an unsigned byte from the file stream, byte stream, or byte array.</summary>
        /// <returns>The returned value is in the range 0 to 255.</returns>
        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.QName("readUnsignedByte", "flash.utils:IDataInput", "public")]
        [PageFX.FP9]
        uint readUnsignedByte();

        /// <summary>Reads a signed 16-bit integer from the file stream, byte stream, or byte array.</summary>
        /// <returns>The returned value is in the range -32768 to 32767.</returns>
        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.QName("readShort", "flash.utils:IDataInput", "public")]
        [PageFX.FP9]
        int readShort();

        /// <summary>Reads an unsigned 16-bit integer from the file stream, byte stream, or byte array.</summary>
        /// <returns>The returned value is in the range 0 to 65535.</returns>
        [PageFX.AbcInstanceTrait(5)]
        [PageFX.ABC]
        [PageFX.QName("readUnsignedShort", "flash.utils:IDataInput", "public")]
        [PageFX.FP9]
        uint readUnsignedShort();

        /// <summary>Reads a signed 32-bit integer from the file stream, byte stream, or byte array.</summary>
        /// <returns>The returned value is in the range -2147483648 to 2147483647.</returns>
        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.QName("readInt", "flash.utils:IDataInput", "public")]
        [PageFX.FP9]
        int readInt();

        /// <summary>Reads an unsigned 32-bit integer from the file stream, byte stream, or byte array.</summary>
        /// <returns>The returned value is in the range 0 to 4294967295.</returns>
        [PageFX.AbcInstanceTrait(7)]
        [PageFX.ABC]
        [PageFX.QName("readUnsignedInt", "flash.utils:IDataInput", "public")]
        [PageFX.FP9]
        uint readUnsignedInt();

        /// <summary>Reads an IEEE 754 single-precision floating point number from the file stream, byte stream, or byte array.</summary>
        /// <returns>An IEEE 754 single-precision floating point number.</returns>
        [PageFX.AbcInstanceTrait(8)]
        [PageFX.ABC]
        [PageFX.QName("readFloat", "flash.utils:IDataInput", "public")]
        [PageFX.FP9]
        double readFloat();

        /// <summary>Reads an IEEE 754 double-precision floating point number from the file stream, byte stream, or byte array.</summary>
        /// <returns>An IEEE 754 double-precision floating point number.</returns>
        [PageFX.AbcInstanceTrait(9)]
        [PageFX.ABC]
        [PageFX.QName("readDouble", "flash.utils:IDataInput", "public")]
        [PageFX.FP9]
        double readDouble();

        /// <summary>
        /// Reads a multibyte string of specified length from the file stream, byte stream, or byte array using the
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
        [PageFX.AbcInstanceTrait(10)]
        [PageFX.ABC]
        [PageFX.QName("readMultiByte", "flash.utils:IDataInput", "public")]
        [PageFX.FP9]
        Avm.String readMultiByte(uint length, Avm.String charSet);

        /// <summary>
        /// Reads a UTF-8 string from the file stream, byte stream, or byte array.  The string
        /// is assumed to be prefixed with an unsigned short indicating
        /// the length in bytes.
        /// This method is similar to the readUTF()
        /// method in the Java IDataInput interface.
        /// </summary>
        /// <returns>A UTF-8 string produced by the byte representation of characters.</returns>
        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("readUTF", "flash.utils:IDataInput", "public")]
        [PageFX.FP9]
        Avm.String readUTF();

        /// <summary>
        /// Reads a sequence of length UTF-8
        /// bytes from the file stream, byte stream, or byte array, and returns a string.
        /// </summary>
        /// <returns>A UTF-8 string produced by the byte representation of characters of specified length.</returns>
        [PageFX.AbcInstanceTrait(12)]
        [PageFX.ABC]
        [PageFX.QName("readUTFBytes", "flash.utils:IDataInput", "public")]
        [PageFX.FP9]
        Avm.String readUTFBytes(uint length);

        /// <summary>
        /// Reads an object from the file stream, byte stream, or byte array, encoded in AMF
        /// serialized format.
        /// </summary>
        /// <returns>The deserialized object</returns>
        [PageFX.AbcInstanceTrait(14)]
        [PageFX.ABC]
        [PageFX.QName("readObject", "flash.utils:IDataInput", "public")]
        [PageFX.FP9]
        object readObject();


    }
}
