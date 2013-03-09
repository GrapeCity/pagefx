using System;
using System.Collections.Generic;
using System.Text;
using DataDynamics.PageFX.Common.Extensions;
using DataDynamics.PageFX.Common.Syntax;

namespace DataDynamics.PageFX.Common.TypeSystem
{
	public static class TypeNameExtensions
	{
		public static string BuildDisplayName(this IType type)
		{
			string k = type.CSharpKeyword();
			if (!String.IsNullOrEmpty(k)) return k;
			return ToDisplayName(type.FullName)
			       + type.GenericParameters.BuildString(TypeNameKind.DisplayName, false);
		}

		public static string BuildSigName(this IType type)
		{
			//TODO: support IGenericInstance
			string k = type.CSharpKeyword();
			return String.IsNullOrEmpty(k) ? ToDisplayName(type.FullName, true) : k;
		}

		public static string BuildKey(this IType type)
		{
			if (type.IsGeneric())
			{
				return type.FullName + type.GenericParameters.BuildString(TypeNameKind.Key, true);
			}
			//TODO: support IGenericInstance
			return type.FullName;
		}

		public static string GetName(this IType type, TypeNameKind kind)
		{
			switch (kind)
			{
				case TypeNameKind.DisplayName:
					return type.DisplayName;
				case TypeNameKind.FullName:
					return type.FullName;
				case TypeNameKind.Key:
					return type.Key;
				case TypeNameKind.Name:
					return type.Name;
				case TypeNameKind.NestedName:
					return type.NestedName;
				case TypeNameKind.SigName:
					return type.SigName;
				case TypeNameKind.CSharpKeyword:
					return type.CSharpKeyword();
			}
			return type.FullName;
		}

		public static string BuildFullName(this IType type)
		{
			var declaringType = type.DeclaringType;
			if (declaringType == null)
			{
				return String.IsNullOrEmpty(type.Namespace) ? type.Name : type.Namespace + "." + type.Name;
			}
			return declaringType.BuildFullName() + "+" + type.Name;
		}

		public static string BuildNestedName(this IType type)
		{
			var name = type.Name;
			var t = type.DeclaringType;
			while (t != null)
			{
				name = t.Name + "+" + name;
				t = t.DeclaringType;
			}
			return name;
		}

		public static string GetKeyword(this IType type, string lang)
		{
			var st = type.SystemType();
			if (st != null)
			{
				string name = st.Code.EnumString(lang);
				if (!String.IsNullOrEmpty(name))
					return name;
			}
			return "";
		}

		private static readonly string SigPrefix = ((int)'[').ToString("X2");
		private static readonly string SigSuffix = ((int)']').ToString("X2");

		internal static string BuildString<T>(this IEnumerable<T> args, TypeNameKind kind, bool clr)
			where T:IType
		{
			bool sig = kind == TypeNameKind.SigName;
			string prefix, suffix, sep;
			if (sig)
			{
				prefix = SigPrefix;
				suffix = SigSuffix;
				sep = "_";
			}
			else
			{
				prefix = clr ? "[" : "<";
				suffix = clr ? "]" : ">";
				sep = ",";
			}
			return args.Join(prefix, suffix, sep, x => x.GetName(kind));
		}

		internal static string ToDisplayName(string name)
		{
			return ToDisplayName(name, false);
		}

		internal static string ToDisplayName(string name, bool sig)
		{
			if (name == null) return null;
			int n = name.Length;
			if (n <= 2)
			{
				return name;
			}

			var sb = new StringBuilder();
			for (int i = 0; i < n; ++i)
			{
				char c = name[i];
				if (c == '`')
				{
					int j = i + 1;
					for (; j < n; ++j)
					{
						if (!Char.IsDigit(name[j]))
							break;
					}
					if (j == i + 1)
						sb.Append(c);
					else
						i = j - 1;
				}
				else
				{
					if (sig)
					{
						switch (c)
						{
							case '.':
								sb.Append('_');
								continue;
						}
					}

					sb.Append(c);
				}
			}

			return sb.ToString();
		}
	}
}
