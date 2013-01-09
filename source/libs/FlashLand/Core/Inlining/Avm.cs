using System;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.IL;

namespace DataDynamics.PageFX.FlashLand.Core.Inlining
{
	internal sealed class AvmInlines : InlineCodeProvider
	{
		[InlineImpl]
		public static void get_GlobalPackage(AbcCode code)
		{
			code.PushGlobalPackage();
		}

		[InlineImpl]
		public static void get_IsFlashPlayer(AbcCode code)
		{
			var m = code.Generator.RuntimeImpl.IsFlashPlayer();
			code.Getlex(m);
			code.Call(m);
		}

		[InlineImpl]
		public static void trace(IMethod method, AbcCode code)
		{
			//has global receiver
			var mn = code.DefineGlobalQName("trace");
			//code.Add(InstructionCode.Findpropstrict, mn);
			code.CallVoid(mn, method.Parameters.Count);
		}

		[InlineImpl]
		public static void exit(IMethod method, AbcCode code)
		{
			var m = code.Generator.RuntimeImpl.Exit();
			code.Getlex(m);
			code.Call(m);
		}

		[InlineImpl]
		public static void Console_Write(AbcCode code)
		{
			code.AvmplusSystemWrite();
		}

		[InlineImpl]
		public static void AddEventListener(AbcCode code)
		{
			code.AddEventListener();
		}

		[InlineImpl]
		public static void RemoveEventListener(AbcCode code)
		{
			code.RemoveEventListener();
		}

		[InlineImpl]
		public static void CreateInstance(IMethod method, AbcCode code)
		{
			code.Construct(method.Parameters.Count - 1);
		}

		[InlineImpl]
		public static void GetProperty(AbcCode code)
		{
			code.GetRuntimeProperty();
		}

		[InlineImpl]
		public static void SetProperty(AbcCode code)
		{
			code.SetRuntimeProperty();
		}

		[InlineImpl]
		public static void Findpropstrict(AbcCode code)
		{
			code.FindPropertyStrict(code.Abc.RuntimeQName);
		}

		[InlineImpl]
		public static void Construct(AbcCode code)
		{
			code.ConstructProperty(code.Abc.RuntimeQName, 0);
		}

		[InlineImpl]
		public static void get_m_value(AbcCode code)
		{
			code.GetBoxedValue();
		}

		[InlineImpl]
		public static void set_m_value(AbcCode code)
		{
			code.SetBoxedValue();
		}

		[InlineImpl]
		public static void Concat(IMethod method, AbcCode code)
		{
			int n = method.Parameters.Count;
			if (n <= 1)
				throw new InvalidOperationException();
			for (int i = 1; i < n; ++i)
				code.Add(InstructionCode.Add);
			code.CoerceString();
		}

		[InlineImpl]
		public static void NewObject(IMethod method, AbcCode code)
		{
			code.NewObject(method.Parameters.Count/2);
		}

		[InlineImpl]
		public static void ToString(AbcCode code)
		{
			code.CallGlobal("toString", 0);
		}

		[InlineImpl]
		public static void NewArray(IMethod method, AbcCode code)
		{
			if (method.Parameters.Count == 1)
			{
				//size is onto the stack
				code.Getlex(AvmTypeCode.Array);
				code.Swap();
				code.Construct(1);
				code.CoerceArray();
				return;
			}
			code.Add(InstructionCode.Newarray, 0);
		}

		[InlineImpl]
		public static void CopyArray(AbcCode code)
		{
			var prop = code.Abc.DefineGlobalQName("concat");
			code.Call(prop, 0);
			code.Coerce(AvmTypeCode.Array);
		}

		[InlineImpl]
		public static void IsNull(AbcCode code)
		{
			code.PushNull();
			code.Add(InstructionCode.Equals);
			code.FixBool();
		}

		[InlineImpl]
		public static void IsUndefined(AbcCode code)
		{
			code.PushUndefined();
			code.Add(InstructionCode.Equals);
			code.FixBool();
		}

		[InlineImpl]
		public static void GetArrayElem(AbcCode code)
		{
			code.GetNativeArrayItem();
		}
	}
}