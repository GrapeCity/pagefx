using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Flash.IL;

namespace DataDynamics.PageFX.Flash.Core.Inlining
{
	internal sealed class AvmObjectInlines : InlineCodeProvider
	{
		[InlineImpl(".ctor")]
		public static void Ctor(IMethod method, AbcCode code)
		{
			code.NewObject(method.Parameters.Count / 2);
		}

		[InlineImpl("GetProperty", ArgCount = 1)]
		public static void GetProperty_String(AbcCode code)
		{
			code.PushGlobalPackage();
			code.Swap(); //namespace should be before name
			code.GetRuntimeProperty();
		}

		[InlineImpl("GetProperty", ArgTypes = new[] { "String", "String" })]
		public static void GetProperty_String_String(AbcCode code)
		{
			code.FixRuntimeQName();
			code.GetRuntimeProperty();
		}

		[InlineImpl("GetProperty", ArgTypes = new[] { "Namespace", "String" })]
		public static void GetProperty_Namespace_String(AbcCode code)
		{
			code.GetRuntimeProperty();
		}

		[InlineImpl("SetProperty", ArgCount = 2)]
		public static void SetProperty(AbcCode code)
		{
			// stack: name, value
			var vh = code.Generator.RuntimeImpl.ValueHolder;
			var value = vh.GetStaticSlot("value");

			code.Getlex(vh); //after: name, value, vh
			code.Swap(); //after: name, vh, value
			code.SetSlot(value);

			code.PushGlobalPackage(); //after: name, ns
			code.Swap(); //ns, name

			code.Getlex(vh);
			code.GetSlot(value);

			code.SetRuntimeProperty();
		}

		[InlineImpl("SetProperty", ArgTypes = new[] { "String", "String" })]
		public static void SetProperty_String_String(AbcCode code)
		{
			var vh = code.Generator.RuntimeImpl.ValueHolder;
			var value = vh.GetStaticSlot("value");

			code.Getlex(vh);
			code.Swap();
			code.SetSlot(value);

			code.FixRuntimeQName();

			code.Getlex(vh);
			code.GetSlot(value);

			code.SetRuntimeProperty();
		}

		[InlineImpl("SetProperty", ArgTypes = new[] { "Namespace", "String" })]
		public static void SetProperty_Namespace_String_String(AbcCode code)
		{
			code.SetRuntimeProperty();
		}
	}
}