﻿using System.Runtime.CompilerServices;
using PageFX;

namespace Native
{
	[Native]
	[QName("Error")]
	internal class Error
	{
		public extern int errorID
		{
			[InlineProperty]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern string message
		{
			[InlineProperty]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		[InlineFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern string getStackTrace();
	}

	[Native]
	[QName("RangeError")]
	internal class RangeError : Error
	{
	}
}