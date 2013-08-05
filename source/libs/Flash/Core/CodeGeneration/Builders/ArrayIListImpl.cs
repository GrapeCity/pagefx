using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DataDynamics.PageFX.Common.Services;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Flash.Abc;
using DataDynamics.PageFX.Flash.Core.CodeGeneration.Corlib;
using DataDynamics.PageFX.Flash.IL;

namespace DataDynamics.PageFX.Flash.Core.CodeGeneration.Builders
{
	internal static class ArrayIListImpl
	{
		public static IDictionary<string, Action<IMethod, AbcCode>> GetImpls(Type type)
		{
			return type
				.GetMethods(BindingFlags.Public | BindingFlags.Static)
				.Where(x =>
					{
						var p = x.GetParameters();
						return p.Length == 2
						       && p[0].ParameterType == typeof(IMethod)
						       && p[1].ParameterType == typeof(AbcCode);
					})
				.ToDictionary(
					x => x.Name,
					x =>
						{
							Action<IMethod, AbcCode> invoke = (m, code) => x.Invoke(null, new object[] {m, code});
							return invoke;
						});
		}

		private static readonly IDictionary<string, Action<IMethod, AbcCode>> Impls = GetImpls(typeof(ArrayIListImpl));

		public static void Implement(AbcInstance instance, IType iface)
		{
			foreach (var method in iface.Methods.Where(x => x.AbcMethod() != null))
			{
				Build(instance, method);
			}
		}

		private static void Build(AbcInstance instance, IMethod method)
		{
			instance.DefineMethod(
				Sig.@from(method.AbcMethod()),
				code =>
					{
						var impl = Impls[method.Name];
						impl(method, code);
					});
		}

		public static void RemoveAt(IMethod method, AbcCode code)
		{
			code.ThrowException(CorlibTypeId.NotSupportedException);
		}

		public static void Insert(IMethod method, AbcCode code)
		{
			code.ThrowException(CorlibTypeId.NotSupportedException);
		}

		public static void IndexOf(IMethod method, AbcCode code)
		{
			code.LoadThis();
			var type = method.Parameters[0].Type;
			code.BoxVariable(type, 1);
			code.Call(ArrayMethodId.IndexOf);
			code.ReturnValue();
		}

		public static void get_Item(IMethod method, AbcCode code)
		{
			var type = method.DeclaringType.GetTypeArgument(0);
			code.LoadThis();
			code.GetLocal(1);
			code.GetArrayElem(type, true);
			code.ReturnValue();
		}

		public static void set_Item(IMethod method, AbcCode code)
		{
			code.LoadThis();
			code.GetLocal(1);
			code.GetLocal(2);
			code.SetArrayElem(true);
			code.ReturnVoid();
		}
	}
}
