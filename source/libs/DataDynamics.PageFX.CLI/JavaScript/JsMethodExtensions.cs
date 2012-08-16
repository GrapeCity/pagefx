using System;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.JavaScript
{
	internal static class JsMethodExtensions
	{
		public static string JsName(this IMethod method)
		{
			if (method.IsToString()) return "toString";
			return method.GetSigName(SigKind.Js);
		}

		public static string JsFullName(this IMethod method)
		{
			var type = method.DeclaringType;
			var typeName = type.JsFullName(method);
			return String.Format("{0}.{1}{2}", typeName, method.IsStatic ? "" : "prototype.", method.JsName());
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
			return !method.IsStatic && method.Parameters.Count == 0 && method.Name == "GetType";
		}

		public static bool IsToString(this IMethod method)
		{
			return !method.IsStatic && method.Parameters.Count == 0 && method.Name == "ToString";
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