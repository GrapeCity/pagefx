using System.Linq;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.JavaScript
{
	internal static class JsExpressionExtensions
	{
		public static JsId Id(this string value)
		{
			return new JsId(value);
		}

		public static JsId Id(this JsVar var)
		{
			return new JsId(var.Name);
		}

		public static JsVar Var(this JsNode value, string name)
		{
			return new JsVar(name, value);
		}

		public static JsNode Call(this JsNode value, params object[] args)
		{
			return new JsCall(value, args);
		}

		public static JsNode Call<T>(this JsNode value, params T[] args)
		{
			return new JsCall(value, args.Select(x => (object)x));
		}

		public static JsNode Apply(this IMethod method, JsNode obj, object args)
		{
			return method.IsStatic
				       ? method.JsFullName().Id().Apply(obj, args)
				       : obj.Get(method.JsName()).Apply(obj, args);
		}

		public static JsNode Apply(this JsNode f, JsNode obj, object args)
		{
			return f.Get("apply").Call(obj, args);
		}

		public static JsNode AsStatement(this JsNode value)
		{
			return new JsStatement(value);
		}

		public static JsNode Return(this object value)
		{
			return new JsReturn(value);
		}

		public static JsNode New(this IType type)
		{
			return new JsNewobj(type);
		}

		public static JsNode Op(this JsNode left, object right, BinaryOperator op)
		{
			return new JsBinaryOperator(left, right, op);
		}

		public static JsNode Op(this JsNode left, object right, string op)
		{
			return new JsBinaryOperator(left, right, op);
		}

		public static JsNode Set(this JsNode dest, object value)
		{
			return dest.Op(value, BinaryOperator.Assign).AsStatement();
		}

		public static JsNode Get(this JsNode obj, object name)
		{
			return new JsPropertyRef(obj, name);
		}

		public static JsNode Set(this JsNode obj, string name, object value)
		{
			return Get(obj, name).Set(value);
		}

		public static JsNode Set(this JsNode obj, JsNode name, object value)
		{
			return Get(obj, name).Set(value);
		}
	}
}