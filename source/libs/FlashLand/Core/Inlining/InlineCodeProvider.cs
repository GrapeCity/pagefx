using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DataDynamics.PageFX.Common.Extensions;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.Abc;
using DataDynamics.PageFX.FlashLand.IL;

namespace DataDynamics.PageFX.FlashLand.Core.Inlining
{
	internal class InlineCodeProvider
	{
		private readonly Dictionary<string, IList<KeyValuePair<Func<IMethod, bool>, Action<IMethod, AbcCode>>>> _impls =
			new Dictionary<string, IList<KeyValuePair<Func<IMethod, bool>, Action<IMethod, AbcCode>>>>();

		public InlineCodeProvider()
		{
			CollectImpls();
		}

		private void CollectImpls()
		{
			var type = GetType();
			foreach (var mi in type.GetMethods(BindingFlags.Static | BindingFlags.Public))
			{
				var methodInfo = mi;
				var info = methodInfo.GetAttribute<InlineImplAttribute>(false);
				if (info == null) continue;

				var name = info.Name ?? methodInfo.Name;
				IList<KeyValuePair<Func<IMethod, bool>, Action<IMethod, AbcCode>>> list;
				if (!_impls.TryGetValue(name, out list))
				{
					list = new List<KeyValuePair<Func<IMethod, bool>, Action<IMethod, AbcCode>>>();
					_impls.Add(name, list);
				}

				Func<IMethod, bool> match;
				if (info.ArgCount >= 0)
				{
					match = x => CheckAttrs(x, info.Attrs) && x.Parameters.Count == info.ArgCount;
				}
				else if (info.ArgTypes != null)
				{
					match = x =>
						{
							if (!CheckAttrs(x, info.Attrs)) return false;
							if (x.Parameters.Count < info.ArgTypes.Length) return false;
							return !info.ArgTypes.Where((t, i) => x.Parameters[i].Type.Name != t).Any();
						};
				}
				else
				{
					match = x => CheckAttrs(x, info.Attrs);
				}

				Action<IMethod, AbcCode> f;
				if (methodInfo.GetParameters().Length == 1)
				{
					f = (method, code) => methodInfo.Invoke(null, new object[] {code});
				}
				else if (methodInfo.GetParameters().Length == 2)
				{
					f = (method, code) => methodInfo.Invoke(null, new object[] {method, code});
				}
				else
				{
					throw new InvalidOperationException("Invalid signature of inline function.");
				}

				list.Add(new KeyValuePair<Func<IMethod, bool>, Action<IMethod, AbcCode>>(match, f));
			}
		}

		public InlineCall GetImplementation(AbcFile abc, IMethod method)
		{
			IList<KeyValuePair<Func<IMethod, bool>, Action<IMethod, AbcCode>>> list;
			if (!_impls.TryGetValue(method.Name, out list))
			{
				if (!_impls.TryGetValue("*", out list))
				{
					return null;
				}
			}

			foreach (var pair in list)
			{
				if (pair.Key(method))
				{
					var code = new AbcCode(abc);
					pair.Value(method, code);
					return new InlineCall(method, null, null, code);
				}
			}

			return null;
		}

		private static bool CheckAttrs(IMethod method, MethodAttrs attrs)
		{
			if ((attrs & MethodAttrs.Constructor) != 0 && !method.IsConstructor)
			{
				return false;
			}
			if ((attrs & MethodAttrs.Getter) != 0 && !method.IsGetter())
			{
				return false;
			}
			if ((attrs & MethodAttrs.Setter) != 0 && !method.IsSetter())
			{
				return false;
			}
			if ((attrs & MethodAttrs.Static) != 0 && !method.IsStatic)
			{
				return false;
			}
			if ((attrs & MethodAttrs.Operator) != 0 && !(method.IsStatic && method.Name.StartsWith("op_")))
			{
				return false;
			}
			if ((attrs & MethodAttrs.WithArgs) != 0 && method.Parameters.Count == 0)
			{
				return false;
			}
			return true;
		}
	}

	internal sealed class InlineImplAttribute : Attribute
	{
		/// <summary>
		/// Specifies method name.
		/// </summary>
		public string Name;

		public int ArgCount = -1;

		public string[] ArgTypes;

		public MethodAttrs Attrs;
		
		public InlineImplAttribute()
		{
		}

		public InlineImplAttribute(string name)
		{
			Name = name;
		}
	}

	[Flags]
	internal enum MethodAttrs
	{
		None = 0x00,
		Constructor = 0x01,
		Getter = 0x02,
		Setter = 0x04,
		Static = 0x08,
		Operator = 0x10,
		WithArgs = 0x20
	}
}