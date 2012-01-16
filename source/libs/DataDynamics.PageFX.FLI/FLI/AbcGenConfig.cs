namespace DataDynamics.PageFX.FLI
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

        public static bool UseAvmString = true;

        public static bool UseActivationTraits = true;

        public static bool UseFuncPointers;

        //NOTE: Currently we can not use non parameterless ctors as instance initializers
        //because of Object.MemberwiseClone implementation
        public static bool IsInitializerParameterless = true;

        public static bool ParameterlessEntryPoint = true;

        public static bool UseIsNull;

        public static bool UseCastToValueType;
    }
}