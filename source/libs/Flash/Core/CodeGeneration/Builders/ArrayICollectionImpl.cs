using System;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.Services;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Flash.Abc;
using DataDynamics.PageFX.Flash.Core.CodeGeneration.Corlib;
using DataDynamics.PageFX.Flash.IL;

namespace DataDynamics.PageFX.Flash.Core.CodeGeneration.Builders
{
	internal static class ArrayICollectionImpl
	{
		private static readonly IDictionary<string, Action<IMethod, AbcCode>>
			Impls = ArrayIListImpl.GetImpls(typeof(ArrayICollectionImpl));

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

		public static void get_Count(IMethod method, AbcCode code)
		{
			code.LoadThis();
			code.Call(ArrayMethodId.GetLength);
			code.ReturnValue();
		}

		public static void get_IsReadOnly(IMethod method, AbcCode code)
		{
			code.PushBool(true);
			code.ReturnValue();
		}

		public static void Add(IMethod method, AbcCode code)
		{
			code.ThrowException(CorlibTypeId.NotSupportedException);
		}

		public static void Remove(IMethod method, AbcCode code)
		{
			code.ThrowException(CorlibTypeId.NotSupportedException);
		}

		public static void Clear(IMethod method, AbcCode code)
		{
			code.ThrowException(CorlibTypeId.NotSupportedException);
		}

		public static void CopyTo(IMethod method, AbcCode code)
		{
			code.LoadThis();
			code.GetLocal(1);
			code.GetLocal(2);
			code.Call(ArrayMethodId.CopyTo);
			code.ReturnVoid();
		}

		public static void Contains(IMethod method, AbcCode code)
		{
			code.LoadThis();
			var type = method.Parameters[0].Type;
			code.BoxVariable(type, 1);
			code.Call(ArrayMethodId.Contains);
			code.ReturnValue();
		}
	}
}