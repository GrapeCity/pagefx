using DataDynamics.PageFX.Flash.Abc;
using DataDynamics.PageFX.Flash.Core;

namespace DataDynamics.PageFX.Flash.Avm
{
	internal enum AvmTypeCode
	{
		[QName("void")]
		Void,

		Boolean,

		[QName("sbyte")]
		Int8,

		[QName("byte")]
		UInt8,

		[QName("short")]
		Int16,

		[QName("ushort")]
		UInt16,

		[QName("int")]
		Int32,

		[QName("uint")]
		UInt32,

		[QName("long")]
		Int64,

		[QName("ulong")]
		UInt64,

		Number,

		[QName("float")]
		Float,

		[QName("double")]
		Double,

		[QName("decimal")]
		Decimal,

		String,
		Object,
		Error,
		Array,
		Function,
		Class,
		Namespace,
		TypeError,
		XML,
		XMLList,
		QName,

		[QName(AS3.Vector.Namespace, AS3.Vector.Name)]
		Verctor
	}

	internal static class AvmTypeCodeExtensions
	{
		public static string FullName(this AvmTypeCode typeCode)
		{
			return BuiltinTypes.GetFullName(typeCode);
		}
	}
}