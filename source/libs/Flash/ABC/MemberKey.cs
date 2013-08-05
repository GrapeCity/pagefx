using System;
using DataDynamics.PageFX.Common.Extensions;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.Core;

namespace DataDynamics.PageFX.FlashLand.Abc
{
	internal static class MemberKey
	{
		private sealed class QName
		{
			public string Name;
			public string Namespace;
			public AbcConstKind NamespaceKind;
		}

		public static string FullName(ITypeMember member)
		{
			var name = GetName(member);
			return name.Namespace.MakeFullName(name.Name);
		}

		public static string BuildKey(ITypeMember member)
		{
			var name = GetName(member);
			return BuildKey(name.Namespace, name.Name, name.NamespaceKind);
		}

		private static QName GetName(ITypeMember member)
		{
			string name;
			string ns = "";
			var nskind = AbcConstKind.PackageNamespace;

			var type = member as IType;
			var attr = member.FindAttribute(Attrs.QName);
			if (attr == null)
			{
				if (type != null)
					ns = type.Namespace;

				var method = member as IMethod;
				if (method != null && (method.IsGetter() || method.IsSetter()))
				{
					name = member.Name.Substring(4);
				}
				else
				{
					name = member.Name;
				}
			}
			else
			{
				int n = attr.Arguments.Count;
				if (n == 0)
					throw new InvalidOperationException("Invalid qname attribute");

				name = (string)attr.Arguments[0].Value;
				if (n > 1)
				{
					ns = (string)attr.Arguments[1].Value;
					string kind = (string)attr.Arguments[2].Value;
					nskind = AbcNamespace.FromShortNsKind(kind);
				}
			}

			return new QName {Name = name, Namespace = ns, NamespaceKind = nskind};
		}

		public static string BuildKey(AbcMultiname name)
		{
			var ns = name.Namespace;
			return BuildKey(ns.NameString, name.Name.Value, ns.Kind);
		}

		private static string BuildKey(string ns, string name, AbcConstKind nskind)
		{
			string result = "";
			if (!string.IsNullOrEmpty(ns))
			{
				result += ns;
				result += ".";
			}
			result += name;
			result += ":";
			result += (int)nskind;
			return result;
		}
	}
}