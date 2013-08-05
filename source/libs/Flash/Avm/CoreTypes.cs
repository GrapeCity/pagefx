using System.Collections.Generic;

namespace DataDynamics.PageFX.Flash.Avm
{
	//TODO: provide caching of AbcInstance for global type like it was done for system types

	internal sealed class CoreTypes
	{
		public const string CorlibNamespace = "Avm";

		private static readonly HashSet<string> Set = new HashSet<string>(
			new[]
				{
					"void",
					"int",
					"uint",
					"Number",
					"String",
					"Object",
					"Boolean",
					"Array",
					"Date",
					"Class",
					"Function",
					"Math",
					"Namespace",
					"QName",
					"XML",
					"XMLList",
					"RegExp",
					"Error",
					"EvalError",
					"ReferenceError",
					"ArgumentError",
					"DefinitionError",
					"RangeError",
					"SecurityError",
					"SyntaxError",
					"TypeError",
					"URIError",
					"VerifyError"
				});

		public static bool Contains(string name)
		{
			return Set.Contains(name);
		}

		public static string GetCorlibTypeName(string name)
		{
			switch (name)
			{
				case "void":
					return "System.Void";
				case "int":
					return CorlibNamespace + ".Int32";
				case "uint":
					return CorlibNamespace + ".UInt32";
				default:
					return CorlibNamespace + "." + name;
			}
		}
	}
}
