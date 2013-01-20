using System.Runtime.CompilerServices;
using PageFX;

namespace Native
{
	[Native]
	[QName("Error")]
	internal class Error
	{
		public extern int errorID
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern string message
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern string getStackTrace();
	}

	[Native]
	[QName("RangeError")]
	internal class RangeError : Error
	{
	}
}