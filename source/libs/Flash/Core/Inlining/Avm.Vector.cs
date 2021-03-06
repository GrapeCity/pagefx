﻿using System;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Flash.Abc;
using DataDynamics.PageFX.Flash.Core.SpecialTypes;
using DataDynamics.PageFX.Flash.IL;

namespace DataDynamics.PageFX.Flash.Core.Inlining
{
	internal sealed class AvmVectorInlines : InlineCodeProvider
	{
		public static readonly InlineCodeProvider Instance = new AvmVectorInlines();

		public static InlineCall Get(AbcFile abc, IMethod method)
		{
			var call = Instance.GetImplementation(abc, method);
			if (call != null)
			{
				return call;
			}

			var code = new AbcCode(abc);
			code.CallAS3(method.Name, method.Parameters.Count);
			return new InlineCall(method, null, null, code);
		}

		[InlineImpl(Name = ".ctor", Attrs = MethodAttrs.Constructor)]
		public static void Ctor(IMethod method, AbcCode code)
		{
			var vec = method.DeclaringType.Data as IVectorType;
			if (vec == null)
				throw new InvalidOperationException();
			code.Construct(method.Parameters.Count);
			code.Coerce(vec.Name);
		}

		private static bool IsTypedVector(IMethod method)
		{
			var type = method.DeclaringType;
			if (type.Data is IVectorType) return true;

			var instance = type.AbcInstance();
			if (instance != null && instance.IsNative)
			{
				var mn = instance.Name;
				string name = mn.FullName;
				return name.Length != AS3.Vector.FullName.Length;
			}

			return false;
		}

		[InlineImpl(Name = "*", Attrs = MethodAttrs.Getter)]
		public static void Getter(IMethod method, AbcCode code)
		{
			var prop = method.Association as IProperty;
			if (prop == null)
				throw new InvalidOperationException();
			if (prop.Parameters.Count > 0) //indexer
			{
				if (IsTypedVector(method))
				{
					code.GetProperty(code.Abc.NameArrayIndexer);
					code.Coerce(method.Type, true);
				}
				else
				{
					code.GetNativeArrayItem();
				}
			}
			else
			{
				code.GetProperty(prop.Name);
			}
		}

		[InlineImpl(Name = "*", Attrs = MethodAttrs.Setter)]
		public static void Setter(IMethod method, AbcCode code)
		{
			var prop = method.Association as IProperty;
			if (prop == null)
				throw new InvalidOperationException();
			if (prop.Parameters.Count > 0) //indexer
			{
				code.SetNativeArrayItem();
			}
			else
			{
				code.SetProperty(prop.Name);
			}
		}

		[InlineImpl(Name = "*", Attrs = MethodAttrs.Operator)]
		public static void Operator(IMethod method, AbcCode code)
		{			
		}
	}
}