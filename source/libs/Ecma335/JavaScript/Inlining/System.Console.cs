using System.Linq;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Ecma335.JavaScript.Inlining
{
	//TODO: remove this temp inlines, inline only internal methods used by Console impl

	internal sealed class ConsoleInlines : InlineCodeProvider
	{
		[InlineImpl("*", Attrs = MethodAttrs.Constructor)]
		public static void Ctor(IMethod method, JsBlock code)
		{
		}

		[InlineImpl("WriteLine", ArgCount = 0)]
		public static void WriteLine0(MethodContext context, JsBlock code)
		{
			code.Add("console.log".Id().Call(""));
		}

		[InlineImpl("WriteLine", ArgCount = 1, ArgTypes = new []{"String"})]
		public static void WriteLine_String(IMethod method, JsBlock code)
		{
			var arg = method.JsArgs()[0];
			code.Add("console.log".Id().Call(arg));
		}

		[InlineImpl("WriteLine", ArgCount = 1, ArgTypes = new[] { "Boolean" })]
		public static void WriteLine_Boolean(IMethod method, JsBlock code)
		{
			var arg = method.JsArgs()[0];
			var val = new JsText(string.Format("{0} ? 'True' : 'False'", arg.Value));
			code.Add("console.log".Id().Call(val));
		}

		[InlineImpl("WriteLine", ArgCount = 1, ArgTypes = new[] { "Int32" })]
		public static void WriteLine_Int32(IMethod method, JsBlock code)
		{
			var arg = method.JsArgs()[0];
			code.Add("console.log".Id().Call(arg));
		}

		[InlineImpl("WriteLine", ArgCount = 1, ArgTypes = new[] { "UInt32" })]
		public static void WriteLine_UInt32(IMethod method, JsBlock code)
		{
			var arg = method.JsArgs()[0];
			code.Add("console.log".Id().Call(arg));
		}

		[InlineImpl("WriteLine", ArgCount = 1, ArgTypes = new[] { "$float" })]
		public static void WriteLine_Floats(IMethod method, JsBlock code)
		{
			var arg = method.JsArgs()[0];
			code.Add("console.log".Id().Call(arg));
		}

		[InlineImpl("WriteLine", ArgCount = 1, ArgTypes = new[] { "Object" })]
		public static void WriteLine_Object(MethodContext context, JsBlock code)
		{
			var toString = SystemTypes.Object.Methods.Find("ToString").First();
			context.Host.CompileCallMethod(toString);

			var arg = context.Method.JsArgs()[0];
			var val = "$tostr".Id().Call(arg);
			code.Add("console.log".Id().Call(val));
		}
	}
}
