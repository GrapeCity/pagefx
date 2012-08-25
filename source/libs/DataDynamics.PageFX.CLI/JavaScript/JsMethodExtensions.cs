using System;
using System.Linq;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.JavaScript
{
	internal static class JsMethodExtensions
	{
		public static bool IsStatic(this IMethod method)
		{
			return method.IsStatic 
				|| (method.IsConstructor && method.DeclaringType.IsString());
		}

		public static string[] JsParams(this IMethod method)
		{
			return method.Parameters.Select(x => x.Name.ToValidId(Runtime.Js)).ToArray();
		}

		public static JsId[] JsArgs(this IMethod method)
		{
			return method.Parameters.Select(x => x.Name.ToValidId(Runtime.Js).Id()).ToArray();
		}

		public static string JsName(this IMethod method)
		{
			if (method.IsToString()) return "toString";
			return method.GetSigName(Runtime.Js);
		}

		public static string JsFullName(this IMethod method)
		{
			return method.JsFullName(method.DeclaringType);
		}

		public static string JsFullName(this IMethod method, IType type)
		{
			var typeName = type.JsFullName(method);
			return String.Format("{0}.{1}{2}", typeName, method.IsStatic() ? "" : "prototype.", method.JsName());
		}

		//TODO: PERF process method custom attributes only once (one iteration)
		//TODO: PERF cache inline info in custom attributes

		public static InlineMethodInfo GetInlineInfo(this IMethod method)
		{
			string name = null;
			var kind = InlineKind.Function;

			foreach (var attr in method.CustomAttributes)
			{
				switch (attr.TypeName)
				{
					case Attrs.InlineFunction:
						name = attr.Arguments.Count == 0 ? method.Name : (string)attr.Arguments[0].Value;
						break;

					case Attrs.InlineProperty:
						name = attr.Arguments.Count == 0 ? method.Name : (string)attr.Arguments[0].Value;
						kind = InlineKind.Property;
						break;

					case Attrs.InlineOperator:
						name = attr.Arguments.Count == 0 ? method.Name : (string)attr.Arguments[0].Value;
						kind = InlineKind.Operator;
						break;

					case Attrs.ABC:
						if (name == null)
						{
							if (method.IsAccessor())
							{
								kind = InlineKind.Property;
								name = method.Association.Name;
							}
							else
							{
								kind = InlineKind.Function;
								name = method.Name;
							}
						}
						break;
				}
			}

			if (name == null)
				return null;

			return new InlineMethodInfo(name, kind);
		}

		public static bool IsGetType(this IMethod method)
		{
			if (method.DeclaringType.IsInterface) return false;
			return !method.IsStatic
				&& method.Parameters.Count == 0
				&& method.Type == SystemTypes.Type
				&& method.Name == "GetType";
		}

		public static bool IsToString(this IMethod method)
		{
			if (method.DeclaringType.IsInterface) return false;
			return !method.IsStatic 
				&& method.Parameters.Count == 0 
				&& method.Type == SystemTypes.String
				&& method.Name == "ToString";
		}

		public static bool IsEquals(this IMethod method)
		{
			if (method.DeclaringType.IsInterface) return false;
			return !method.IsStatic
				&& method.Parameters.Count == 1
				&& method.Parameters[0].Type == SystemTypes.Object
				&& method.Type == SystemTypes.Boolean
				&& method.Name == "Equals";
		}
	}

	internal sealed class InlineMethodInfo
	{
		public readonly string Name;
		public readonly InlineKind Kind;

		public InlineMethodInfo(string name, InlineKind kind)
		{
			Name = name;
			Kind = kind;
		}
	}

	internal enum InlineKind
	{
		Function,
		Property,
		Operator
	}

	internal static class Attrs
	{
		private const string Ns = "PageFX.";

		public const string InlineFunction = Ns + "InlineFunctionAttribute";
		public const string InlineProperty = Ns + "InlinePropertyAttribute";
		public const string InlineOperator = Ns + "InlineOperatorAttribute";

		public const string ABC = Ns + "ABCAttribute";
	}
}