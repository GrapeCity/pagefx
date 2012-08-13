using DataDynamics.PageFX.FLI.IL;

namespace DataDynamics.PageFX.FLI.Inlining
{
	internal sealed class AvmArrayInlines : InlineCodeProvider
	{
		[InlineImpl]
		public static void get_Item(AbcCode code)
		{
			code.GetNativeArrayItem();
		}

		[InlineImpl]
		public static void set_Item(AbcCode code)
		{
			code.SetNativeArrayItem();
		}

		[InlineImpl]
		public static void SetValue(AbcCode code)
		{
			code.SetNativeArrayItem();
		}

		[InlineImpl]
		public static void GetBool(AbcCode code)
		{
			code.GetNativeArrayItem();
			code.CoerceBool();
		}

		[InlineImpl]
		public static void GetInt32(AbcCode code)
		{
			code.GetNativeArrayItem();
			code.CoerceInt32();
		}

		[InlineImpl]
		public static void GetUInt32(AbcCode code)
		{
			code.GetNativeArrayItem();
			code.CoerceUInt32();
		}

		[InlineImpl]
		public static void GetDouble(AbcCode code)
		{
			code.GetNativeArrayItem();
			code.CoerceDouble();
		}

		[InlineImpl]
		public static void GetString(AbcCode code)
		{
			code.GetNativeArrayItem();
			code.CoerceString();
		}

		[InlineImpl]
		public static void GetChar(AbcCode code)
		{
			code.GetNativeArrayItem();
			code.CoerceChar();
		}
	}
}