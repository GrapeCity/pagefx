﻿using System.Linq;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.JavaScript.Inlining
{
	internal sealed class RuntimeHelpersInlines : InlineCodeProvider
	{
		[InlineImpl]
		public static void InitializeArray(IMethod method, JsBlock code)
		{
			var args = method.Parameters.Select(x => x.Name.Id()).ToArray();
			code.Add("$initarr".Id().Call(args).AsStatement());
		}
	}
}