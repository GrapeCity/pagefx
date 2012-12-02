using System.Linq;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.CodeModel.TypeSystem;

namespace DataDynamics.PageFX.CLI.JavaScript
{
	internal static class JsExpressionExtensions
	{
		public static string ToJsString(this object value)
		{
			var writer = new JsWriter();
			writer.WriteValue(value);
			writer.Flush();
			return writer.ToString();
		}

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
			return new JsCall(value, args.Cast<object>());
		}

		public static JsNode Call<T>(this IMethod method, JsNode obj, params T[] args)
		{
			return method.IsStatic()
					   ? method.JsFullName().Id().Apply(obj, new JsArray(args.Cast<object>()))
					   : obj.Get(method.JsName()).Call(obj, args);
		}

		public static JsNode Apply(this IMethod method, JsNode obj, object args)
		{
			return method.IsStatic()
				       ? method.JsFullName().Id().Apply(obj, args)
				       : obj.Get(method.JsName()).Apply(obj, args);
		}

		public static JsNode Apply(this JsNode f, object obj, object args)
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

		public static JsNode New(this IType type, params object[] args)
		{
			return new JsNewobj(type, args);
		}

		public static JsNode Op(this JsNode left, object right, BinaryOperator op)
		{
			return new JsBinaryOperator(left, right, op);
		}

		public static JsNode Op(this JsNode left, object right, string op)
		{
			return new JsBinaryOperator(left, right, op);
		}

		public static JsNode Op(this JsNode value, string op)
		{
			return new JsUnaryOperator(value, op);
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

		public static JsNode And(this JsNode left, object right)
		{
			var a = left as JsAnd;
			if (a != null)
			{
				a.Args.Add(right);
				return a;
			}
			return new JsAnd(left, right);
		}

		public static JsNode Or(this JsNode left, object right)
		{
			var e = left as JsOr;
			if (e != null)
			{
				e.Args.Add(right);
				return e;
			}
			return new JsOr(left, right);
		}

		public static JsNode Ternary(this JsNode condition, object left, object right)
		{
			return new JsConditionalExpression(condition, left, right);
		}
	}
}