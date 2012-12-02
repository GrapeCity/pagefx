using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.CodeModel.TypeSystem;

namespace DataDynamics.PageFX.CLI.JavaScript.Inlining
{
	using Match = Func<IMethod, bool>;
	using InlineFunc = Action<MethodContext, JsBlock>;

	internal class InlineCodeProvider
	{
		private readonly Dictionary<string, IList<KeyValuePair<Match, InlineFunc>>> _impls =
			new Dictionary<string, IList<KeyValuePair<Match, InlineFunc>>>();

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
				IList<KeyValuePair<Match, InlineFunc>> list;
				if (!_impls.TryGetValue(name, out list))
				{
					list = new List<KeyValuePair<Match, InlineFunc>>();
					_impls.Add(name, list);
				}

				Match match;
				if (info.ArgTypes != null && info.ArgTypes.Length > 0)
				{
					match = x =>
						{
							if (!CheckAttrs(x, info.Attrs)) return false;
							if (info.ArgCount >= 0 && x.Parameters.Count != info.ArgCount) return false;
							if (x.Parameters.Count < info.ArgTypes.Length) return false;
							return !info.ArgTypes.Where((t, i) => !CheckType(x.Parameters[i].Type, t)).Any();
						};
				}
				else if (info.ArgCount >= 0)
				{
					match = x => CheckAttrs(x, info.Attrs) && x.Parameters.Count == info.ArgCount;
				}
				else
				{
					match = x => CheckAttrs(x, info.Attrs);
				}

				InlineFunc f;
				var parameters = methodInfo.GetParameters();
				if (parameters.Length == 1)
				{
					f = (ctx, code) => methodInfo.Invoke(null, new object[] {code});
				}
				else if (parameters.Length == 2)
				{
					int i = parameters.IndexOf(p => p.ParameterType.Name == "JsBlock");
					if (i == 0)
					{
						if (parameters[1].ParameterType == typeof(IMethod))
						{
							f = (ctx, code) => methodInfo.Invoke(null, new object[] { code, ctx.Method });
						}
						else if (parameters[1].ParameterType == typeof(MethodContext))
						{
							f = (ctx, code) => methodInfo.Invoke(null, new object[] { code, ctx });
						}
						else
						{
							throw new InvalidOperationException();
						}
					}
					else
					{
						if (parameters[0].ParameterType == typeof(IMethod))
						{
							f = (ctx, code) => methodInfo.Invoke(null, new object[] {ctx.Method, code});
						}
						else if (parameters[0].ParameterType == typeof(MethodContext))
						{
							f = (ctx, code) => methodInfo.Invoke(null, new object[] {ctx, code});
						}
						else
						{
							throw new InvalidOperationException();
						}
					}
				}
				else
				{
					throw new InvalidOperationException("Invalid signature of inline function.");
				}

				list.Add(new KeyValuePair<Match, InlineFunc>(match, f));
			}
		}

		public JsFunction GetImplementation(MethodContext context)
		{
			var method = context.Method;

			IList<KeyValuePair<Match, InlineFunc>> list;
			if (_impls.TryGetValue(method.Name, out list))
			{
				var impl = GetImpl(context, list);
				if (impl != null)
				{
					return impl;
				}
			}

			if (_impls.TryGetValue("*", out list))
			{
				return GetImpl(context, list);
			}

			return null;
		}

		private static JsFunction GetImpl(MethodContext context, IEnumerable<KeyValuePair<Match, InlineFunc>> list)
		{
			var method = context.Method;
			foreach (var pair in list)
			{
				if (pair.Key(method))
				{
					var parameters = method.Parameters.Select(x => x.Name).ToArray();
					var func = new JsFunction(null, parameters);
					pair.Value(context, func.Body);
					return func;
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
			if ((attrs & MethodAttrs.Getter) != 0 && !method.IsGetter)
			{
				return false;
			}
			if ((attrs & MethodAttrs.Setter) != 0 && !method.IsSetter)
			{
				return false;
			}
			if ((attrs & MethodAttrs.Static) != 0 && !method.IsStatic)
			{
				return false;
			}
			if ((attrs & MethodAttrs.Instance) != 0 && method.IsStatic)
			{
				return false;
			}
			if ((attrs & MethodAttrs.Operator) != 0 && !(method.IsStatic && method.Name.StartsWith("op_")))
			{
				return false;
			}
			return true;
		}

		private static bool CheckType(IType type, string spec)
		{
			if (spec[0] == '$')
			{
				switch (spec.Substring(1))
				{
					case "int32":
						switch (type.GetTypeCode())
						{
							case TypeCode.SByte:
							case TypeCode.Byte:
							case TypeCode.Int16:
							case TypeCode.UInt16:
							case TypeCode.Int32:
							case TypeCode.UInt32:
								return true;
							default:
								return false;
						}
					case "float":
						switch (type.GetTypeCode())
						{
							case TypeCode.Single:
							case TypeCode.Double:
								return true;
							default:
								return false;
						}
					default:
						throw new NotImplementedException();
				}
			}
			return type.Name == spec;
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
		Instance = 0x10,
		Operator = 0x20,
	}
}