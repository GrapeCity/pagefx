using System.Runtime.CompilerServices;
using PageFX;

namespace Native
{
	internal static class ByteArrayFactory
	{
		public static ByteArray Create(bool isLittleEndian)
		{
			ByteArray arr = new ByteArray();
			arr.endian = isLittleEndian ? "littleEndian" : "bigEndian";
			return arr;
		}

		public static ByteArray Create(bool isLittleEndian, byte[] bytes, int startIndex)
		{
			ByteArray arr = Create(isLittleEndian);
			while (startIndex < bytes.Length)
				arr.WriteByte(bytes[startIndex++]);
			arr.position = 0;
			return arr;
		}
	}

	[Native]
	[QName("ByteArray", "flash.utils", "package")]
	internal class ByteArray
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern ByteArray();

		public extern string endian
		{
			[InlineProperty]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[InlineProperty]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern uint position
		{
			[InlineProperty]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[InlineProperty]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		[InlineFunction("writeByte")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void WriteByte(byte value);

		[InlineFunction("readByte")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int ReadByte();

		[InlineFunction("readFloat")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern float ReadFloat();

		[InlineFunction("writeFloat")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void WriteFloat(float value);

		[InlineFunction("readDouble")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern double ReadDouble();

		[InlineFunction("writeDouble")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void WriteDouble(double value);
	}
}