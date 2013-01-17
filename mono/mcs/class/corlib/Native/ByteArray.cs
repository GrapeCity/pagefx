using System.Runtime.CompilerServices;

namespace Native
{
	internal class ByteArray
	{
		// ByteArray arr = new ByteArray();
		// arr.endian = IsLittleEndian ? Endian.LITTLE_ENDIAN : Endian.BIG_ENDIAN;
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern ByteArray(bool isLittleEndian);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void WriteByte(byte value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Seek(int value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int ReadByte();

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern float ReadFloat();

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void WriteFloat(float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern double ReadDouble();

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void WriteDouble(double value);
	}
}