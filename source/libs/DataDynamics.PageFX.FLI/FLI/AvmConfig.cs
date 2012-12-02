using System;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.CodeModel.TypeSystem;

namespace DataDynamics.PageFX.FLI
{
    internal static class AvmConfig
    {
        public static bool BooleanAsInt = true;

        public static bool SupportNativeInt64;
        public static bool SupportNativeDecimal;

        public static bool SupportSmallIntegers;

        public static void CheckInt64(IType type)
        {
            if (type.Is(SystemTypeCode.Int64) && !SupportNativeInt64)
                throw new InvalidOperationException("AVM does not supported 64-bit integral numbers");
        }
    }
}