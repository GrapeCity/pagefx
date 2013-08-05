namespace DataDynamics.PageFX.Flash.Core
{
    internal static class AbcGenConfig
    {
        public static bool ThrowOnUnexpectedCall;

        /// <summary>
        /// True - AbcGen will generate cinit for enum types, False - AbcGen will use const traits.
        /// </summary>
        public static bool InitEnumFields = true;

        public static bool ExludeEnumConstants = true;

        public static bool FlexAppCtorAsHandler = true;

	    public static bool UseActivationTraits = true;

        public static bool UseFuncPointers;

        public static bool ParameterlessEntryPoint = true;

        public static bool UseIsNull;

        public static bool UseCastToValueType;
    }
}