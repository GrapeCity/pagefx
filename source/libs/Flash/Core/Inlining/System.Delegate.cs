﻿using DataDynamics.PageFX.Flash.IL;

namespace DataDynamics.PageFX.Flash.Core.Inlining
{
	internal sealed class DelegateInlines : InlineCodeProvider
	{
		[InlineImpl]
		public static void AddEventListener(AbcCode code)
		{
			code.AddEventListener();
		}

		[InlineImpl]
		public static void RemoveEventListener(AbcCode code)
		{
			code.RemoveEventListener();
		}
	}
}