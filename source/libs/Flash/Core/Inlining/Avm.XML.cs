﻿using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Flash.Avm;
using DataDynamics.PageFX.Flash.IL;

namespace DataDynamics.PageFX.Flash.Core.Inlining
{
	internal sealed class AvmXmlInlines : InlineCodeProvider
	{
		[InlineImpl("get_Item", ArgCount = 1)]
		public static void GetItem1(IMethod method, AbcCode code)
		{
			var p0 = method.Parameters[0].Type;
			if (p0.Is(SystemTypeCode.Int32))
			{
				code.GetNativeArrayItem();
				code.CoerceXML();
			}
			else //string
			{
				//TODO: It seams that any namespace can not be used with runtime qnames.
				code.PushGlobalPackage();
				code.Swap();
				code.GetRuntimeProperty();
				code.CoerceXMLList();
			}
		}

		[InlineImpl("get_Item", ArgCount = 2)]
		public static void GetItem2(IMethod method, AbcCode code)
		{
			var p0 = method.Parameters[0].Type;
			if (p0.Name == "Namespace")
			{
				code.GetRuntimeProperty();
				code.CoerceXMLList();
			}
			else //namespace as Avm.String
			{
				code.Swap(); //stack [name, nsname]
				var ns = code[AvmTypeCode.Namespace];
				code.FindPropertyStrict(ns); //stack [name, nsname, global]
				code.Swap(); //stack [name, global, nsname]
				code.ConstructProperty(ns, 1); //stack [name, ns]
				code.Coerce(ns); //stack [name, ns]
				code.Swap(); //stack [ns, name]
				code.GetRuntimeProperty();
				code.CoerceXMLList();
			}
		}
	}
}