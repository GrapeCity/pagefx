﻿using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Flash.IL;

namespace DataDynamics.PageFX.Flash.Core.Inlining
{
	internal sealed class DateInlines : InlineCodeProvider
	{
		[InlineImpl(".ctor")]
		public static void Ctor(IMethod method, AbcCode code)
		{
			code.Construct(method.Parameters.Count);
		}
	}
}