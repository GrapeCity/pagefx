using System;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.CLI.JavaScript.Inlining
{
	//TODO: inline methods

	internal sealed class AvmArrayInlines : InlineCodeProvider
	{
		private static void Get(IMethod method, JsBlock code, TypeCode coerce)
		{
			var args = method.JsArgs();
			code.Add("this".Id().Get(args[0]).Return());
		}

		[InlineImpl]
		public static void get_Item(IMethod method, JsBlock code)
		{
			Get(method, code, TypeCode.Empty);
		}

		[InlineImpl]
		public static void set_Item(IMethod method, JsBlock code)
		{
			var args = method.JsArgs();
			code.Add("this".Id().Set(args[0], args[1]));
		}

		[InlineImpl]
		public static void SetValue(IMethod method, JsBlock code)
		{
			var args = method.JsArgs();
			code.Add("this".Id().Set(args[0], args[1]));
		}

		[InlineImpl]
		public static void GetBool(IMethod method, JsBlock code)
		{
			Get(method, code, TypeCode.Boolean);
		}

		[InlineImpl]
		public static void GetInt32(IMethod method, JsBlock code)
		{
			Get(method, code, TypeCode.Int32);
		}

		[InlineImpl]
		public static void GetUInt32(IMethod method, JsBlock code)
		{
			Get(method, code, TypeCode.UInt32);
		}

		[InlineImpl]
		public static void GetDouble(IMethod method, JsBlock code)
		{
			Get(method, code, TypeCode.Double);
		}

		[InlineImpl]
		public static void GetString(IMethod method, JsBlock code)
		{
			Get(method, code, TypeCode.String);
		}

		[InlineImpl]
		public static void GetChar(IMethod method, JsBlock code)
		{
			Get(method, code, TypeCode.Char);
		}
	}
}