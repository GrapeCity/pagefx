using System;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.JavaScript
{
	internal sealed class JsStruct
	{
		public static void Compile(JsCompiler compiler, JsClass klass, ObjectMethodId id)
		{
			switch (id)
			{
				case ObjectMethodId.Equals:
					CompileEquals(compiler, klass);
					break;
				case ObjectMethodId.GetHashCode:
					CompileGetHashCode(compiler, klass);
					break;
				case ObjectMethodId.ToString:
					// Object.ToString will be used
					break;
				default:
					throw new ArgumentOutOfRangeException("id");
			}
		}

		public static void CompileCopy(JsClass klass)
		{
			var type = klass.Type;
			var func = new JsFunction(null);

			var obj = "o".Id();
			
			func.Body.Add(type.New().Var(obj.Value));

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

			klass.Add(new JsGeneratedMethod(String.Format("{0}.prototype.$copy", type.JsFullName()), func));
		}

		private static void CompileEquals(JsCompiler compiler, JsClass klass)
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
					compiler.CompileMethod(eq);
					e = eq.JsFullName().Id().Call(left, right);
				}

				result = result == null ? e : result.And(e);
			}

			func.Body.Add(result == null ? "false".Id().Return() : result.Return());

			var methodName = ObjectMethods.Find(ObjectMethodId.Equals).JsName();

			klass.Add(new JsGeneratedMethod(String.Format("{0}.prototype.{1}", klass.Type.JsFullName(), methodName), func));
		}

		private static IEnumerable<IField> GetInstanceFields(JsClass klass)
		{
			return klass.Type.Fields.Where(field => !field.IsStatic && !field.IsConstant);
		}

		private static void CompileGetHashCode(JsCompiler compiler, JsClass klass)
		{
			// TODO: implement GetHashCode for value types
		}
	}
}