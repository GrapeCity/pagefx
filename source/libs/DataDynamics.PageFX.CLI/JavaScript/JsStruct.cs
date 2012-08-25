using System;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.JavaScript
{
	internal sealed class JsStruct
	{
		public static void CopyImpl(JsClass klass)
		{
			var func = new JsFunction(null);

			var obj = "o".Id();

			func.Body.Add(klass.Type.New().Var(obj.Value));

			foreach (var field in GetInstanceFields(klass))
			{
				var name = field.JsName();
				var value = "this".Id().Get(name);

				if (field.Type.TypeKind == TypeKind.Struct)
				{
					value = "$copy".Id().Call(value);
				}

				func.Body.Add(obj.Set(name, value));
			}

			func.Body.Add(obj.Return());

			klass.Add(new JsGeneratedMethod(String.Format("{0}.prototype.$copy", klass.Type.JsFullName()), func));
		}

		public static void DefaultEqualsImpl(JsCompiler host, JsClass klass)
		{
			var other = "o".Id();

			var func = new JsFunction(null, other.Value);

			func.Body.Add(new JsText("if (o === null || o === undefined) return false;"));

			//TODO: check object type

			JsNode result = null;

			foreach (var field in GetInstanceFields(klass))
			{
				var name = field.JsName();
				var left = "this".Id().Get(name);
				var right = other.Get(name);

				JsNode e;
				// primitive and ref types
				if (field.Type.TypeKind != TypeKind.Struct && !field.Type.IsInt64Based())
				{
					e = left.Op(right, BinaryOperator.Equality);
				}
				else // value types, int64 based
				{
					var eq = SystemTypes.Object.Methods.Find("Equals", SystemTypes.Object, SystemTypes.Object);
					host.CompileMethod(eq);
					e = eq.JsFullName().Id().Call(left, right);
				}

				result = result == null ? e : result.And(e);
			}

			func.Body.Add(result == null ? "false".Id().Return() : result.Return());

			var methodName = GetObjectEqualsMethod().JsName();

			klass.Add(new JsGeneratedMethod(String.Format("{0}.prototype.{1}", klass.Type.JsFullName(), methodName), func));
		}

		public static IMethod GetObjectEqualsMethod()
		{
			return SystemTypes.Object.Methods.Find("Equals", SystemTypes.Object);
		}

		private static IEnumerable<IField> GetInstanceFields(JsClass klass)
		{
			return klass.Type.Fields.Where(field => !field.IsStatic && !field.IsConstant);
		}
	}
}