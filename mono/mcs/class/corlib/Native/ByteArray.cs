using System.Runtime.CompilerServices;
using PageFX;

namespace Native
{
	[Native]
	[QName("ByteArray", "flash.utils", "package")]
	internal class ByteArray
	{
		// ByteArray arr = new ByteArray();
		// arr.endian = IsLittleEndian ? Endian.LITTLE_ENDIAN : Endian.BIG_ENDIAN;
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern ByteArray(bool isLittleEndian);

		[QName("writeByte")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void WriteByte(byte value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Seek(int value);

		[QName("readByte")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int ReadByte();

		[QName("readFloat")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern float ReadFloat();

		[QName("writeFloat")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void WriteFloat(float value);

		[QName("readDouble")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern double ReadDouble();

		[QName("writeDouble")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void WriteDouble(double value);
	}
}