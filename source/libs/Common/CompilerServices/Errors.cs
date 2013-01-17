namespace DataDynamics.PageFX.Common.CompilerServices
{
    public static class Errors
    {
        //TODO: Extend list of errors and use it everywhere in compiler.
        //TODO: Localize compiler errors

        #region General Errors Section
        public static readonly ErrorInfo InvalidCommandLine = new ErrorInfo(0x0001, "Invalid command line arguments.");

        public static readonly ErrorInfo InvalidCommandLineOption = new ErrorInfo(0x0002, "Invalid command line option {0}.");

        public static readonly ErrorInfo NoInputFiles = new ErrorInfo(0x0003, "No input files.");

        public static readonly ErrorInfo UnableToCompileSourceFiles = new ErrorInfo(0x0004, "Unable to compile source files.");

        public static readonly ErrorInfo TwoManyInputAssemblies = new ErrorInfo(0x0005, "Two many input assemblies.");

        public static readonly ErrorInfo UnableToLoadAssembly = new ErrorInfo(0x0006, "Unable to load assembly {0}.");

        public static readonly ErrorInfo UnableToTranslateAssembly = new ErrorInfo(0x0007, "Unable to translate assembly {0}.");

        public static readonly ErrorInfo NotSwf = new ErrorInfo(0x0008, "Specified feature is available only in flash runtime");

        public static readonly ErrorInfo InvalidCorlib = new ErrorInfo(0x0009, "Invalid corlib");
        #endregion

        #region Type Errors section
        public static class Type
        {
            public static readonly ErrorInfo UnableToFind = new ErrorInfo(0x1001, "Unable to find type {0}.");
            public static readonly ErrorInfo NotLinked = new ErrorInfo(0x1002, "Specified type {0} is not linked.");
        }
        #endregion

        #region Linker Errors section
        public static class Linker
        {
            public static readonly ErrorInfo UnableToFindSymbol = new ErrorInfo(0x2001, "Unable to find symbol {0}");
        }
        #endregion

        #region CIL Translator section
        public static class CILTranslator
        {
            public static readonly ErrorInfo UnableToTranslateMethod = new ErrorInfo(0x3001, "Unable to translate method {0}");
        }
        #endregion

        #region ABC Generation Errors section
        public static class ABC
        {
            public static readonly ErrorInfo BadFormat = new ErrorInfo(0x4001, "ABC data is corrupt");

            public static readonly ErrorInfo InvalidBranchOffset = new ErrorInfo(0x4002, "Invalid branch offset");

            public static readonly ErrorInfo IncompatibleCall = new ErrorInfo(0x4003, "Called incompatible method '{0}' only available in Flash Player {1}.");

            public static readonly ErrorInfo IncompatibleField = new ErrorInfo(0x4004, "Referenced incompatible field '{0}' only available in Flash Version {1}.");
        }
        #endregion

        #region SWF Generation Section
        public static class SWF
        {
            public static readonly ErrorInfo InvalidVersion = new ErrorInfo(0x5001, "Flash Version {0} is not supported. Valid values are 9, 10.");
            public static readonly ErrorInfo TagIsNotCharacter = new ErrorInfo(0x5002, "Tag is not character");
            public static readonly ErrorInfo UnableToFindImageResource = new ErrorInfo(0x5003, "Unable to find image resource '{0}'");
            public static readonly ErrorInfo BadImageResource = new ErrorInfo(0x5004, "Bad image resource '{0}'. Unable to load image.");
        }
        #endregion

        #region Resource Bundle Compiler Section
        public static class RBC
        {
            /// <summary>
            /// Format arguments: {0} - resource source, {1} - mimeType
            /// </summary>
            public static readonly ErrorInfo NotSupportedMimeType = new ErrorInfo(0x6001, "Unable to embed resource {0}. MimeType {1} is not supported.");

            /// <summary>
            /// No format arguments
            /// </summary>
            public static readonly ErrorInfo BadMetaEntry = new ErrorInfo(0x6001, "ABC contains invalid ResourceBundle meta entry.");

            /// <summary>
            /// Format arguments: {0} - resource bundle, {1} - locale
            /// </summary>
            public static readonly ErrorInfo UnableToResolve = new ErrorInfo(0x6002, "Unable to resolve resource bundle {0} for {1} locale");

            /// <summary>
            /// Format arguments: {0} - directive string
            /// </summary>
            public static readonly ErrorInfo UnableToParseDirective = new ErrorInfo(0x6003, "Unable to parse directirve '{0}'.");

            /// <summary>
            /// Format arguments: {0} - directive string
            /// </summary>
            public static readonly ErrorInfo UnableToResolveEmbed = new ErrorInfo(0x6004, "Unable to resolve Embed directirve '{0}'.");

            /// <summary>
            /// Format arguments: {0} - image source
            /// </summary>
            public static readonly ErrorInfo UnableToResolveImage = new ErrorInfo(0x6005, "Unable to resolve image source '{0}'.");

            /// <summary>
            /// Format arguments: {0} - directive string
            /// </summary>
            public static readonly ErrorInfo BadDirective = new ErrorInfo(0x6006, "Bad directive '{0}'.");

            /// <summary>
            /// Format arguments: {0} - directive string
            /// </summary>
            public static readonly ErrorInfo UnableToResolveClassReference = new ErrorInfo(0x6006, "Unable to resolve ClassReference directive '{0}'.");
        }
        #endregion

        #region EmbedAssets Section
        public static class EmbedAsset
        {
            public static readonly ErrorInfo FieldIsNotStatic = new ErrorInfo(0x7001, "EmbedAttribute is defined on non static field {0}.");
            public static readonly ErrorInfo NotFlashRuntime = new ErrorInfo(0x7002, "EmbedAttribute can be used only in flash runtime.");
        }
        #endregion

        #region RSL Linking section
        public static class RSL
        {
            public static readonly ErrorInfo UnableToResolve = new ErrorInfo(0x8001, "Unable to resolve RSL {0}");
            public static readonly ErrorInfo SwcIsNotResolved = new ErrorInfo(0x8002, "SWC file is not resolved for given RSL {0}");
            public static readonly ErrorInfo UnableToResolveLibraryDigest = new ErrorInfo(0x8003, "Unable to resolve library digest for given RSL {0}");
        }
        #endregion

        public static readonly ErrorInfo Internal = new ErrorInfo(0x9000, "Internal compiler error");
    }
}