using System.Collections.Generic;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Flash.Abc;
using DataDynamics.PageFX.Flash.Core.SpecialTypes;

namespace DataDynamics.PageFX.Flash.Core.Inlining
{
	internal static class InlineCodeGenerator
	{
		private static readonly Dictionary<string, InlineCodeProvider> Inlines =
			new Dictionary<string, InlineCodeProvider>
				{
					{"System.Object", new SystemObjectInlines()},
					{"System.String", new StringInlines()},
					{"System.Delegate", new DelegateInlines()},
					{"System.Diagnostics.Debugger", new DebuggerInlines()},
					{"avm", new AvmInlines()},
					{"Native.Date", new DateInlines()},
					{"Native.NativeArray", new AvmArrayInlines()},
					{"Native.ByteArray", new FlashByteArrayInlines()},
				};

		private static readonly Dictionary<string, InlineCodeProvider> AvmInlines =
			new Dictionary<string, InlineCodeProvider>
				{
					{"Object", new AvmObjectInlines()},
					{"String", new AvmStringInlines()},
					{"Function", new AvmFunctionInlines()},
					{"Class", new AvmClassInlines()},
					{"Array", new AvmArrayInlines()},
					{"XML", new AvmXmlInlines()},
					{"XMLList", new AvmXmlInlines()},
				};

		public static InlineCall Build(AbcFile abc, AbcInstance instance, IMethod method)
		{
			if (instance != null && instance.IsNative)
			{
				var mn = instance.Name;
				string name = mn.FullName;

				InlineCodeProvider provider;
				if (AvmInlines.TryGetValue(name, out provider))
				{
					return provider.GetImplementation(abc, method);
				}

				if (name.StartsWith(AS3.Vector.FullName))
				{
					return AvmVectorInlines.Get(abc, method);
				}
			}

			var type = method.DeclaringType;
			if (type.Data is IVectorType)
			{
				return AvmVectorInlines.Get(abc, method);
			}

			return GetImpl(abc, type, method);
		}

		private static InlineCall GetImpl(AbcFile abc, IType type, IMethod method)
		{
			var info = method.GetInlineInfo();
			if (info != null)
			{
				return InlineCall.Create(abc, method, info);
			}

			InlineCodeProvider provider;
			if (Inlines.TryGetValue(type.FullName, out provider))
			{
				return provider.GetImplementation(abc, method);
			}

			return null;
		}
	}
}