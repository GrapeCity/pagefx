using System;
using System.Runtime.CompilerServices;

namespace flash.utils
{
    /// <summary>
    /// The Endian class contains values that denote the byte order used to represent multibyte
    /// numbers. The byte order is either bigEndian (most significant byte first) or littleEndian (least
    /// significant byte first).
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class Endian : Avm.Object
    {
        /// <summary>
        /// Indicates the most significant byte of the multibyte number appears first in the sequence of bytes.
        /// The hexadecimal number 0x12345678 has 4 bytes (2 hexadecimal digits per byte).
        /// The most significant byte is 0x12.  The least significant byte is 0x78. (For the equivalent
        /// decimal number, 305419896, the most significant digit is 3, and the least significant digit
        /// is 6).A stream using the bigEndian byte order (the most significant byte first)
        /// writes:
        /// 12 34 56 78
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String BIG_ENDIAN;

        /// <summary>
        /// Indicates the least significant byte of the multibyte number appears first in the sequence of bytes.
        /// The hexadecimal number 0x12345678 has 4 bytes (2 hexadecimal digits per byte).
        /// The most significant byte is 0x12.  The least significant byte is 0x78. (For the equivalent
        /// decimal number, 305419896, the most significant digit is 3, and the least significant digit
        /// is 6).A stream using the littleEndian byte order (the least significant byte
        /// first) writes:
        /// 78 56 34 12
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String LITTLE_ENDIAN;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Endian();
    }
}
