﻿using System.Runtime.CompilerServices;
using PageFX;

namespace Native
{
	[GlobalFunctions]
	internal static class Global
	{
		[InlineFunction("parseFloat")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern static double parseFloat(string str);

		[FP9]
		[InlineFunction]
		[QName("getTimer", "flash.utils", "package")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern static int getTimer();
	}
}
