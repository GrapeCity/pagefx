using System.Runtime.CompilerServices;

namespace Native
{
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

	internal class RangeError : Error
	{
	}
}