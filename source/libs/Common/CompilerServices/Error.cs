namespace DataDynamics.PageFX.Common.CompilerServices
{
	/// <summary>
	/// Defines error codes.
	/// </summary>
	public enum Error
	{
		#region General Errors

        InvalidCommandLine = 0x0001, // "Invalid command line arguments."

        InvalidCommandLineOption = 0x0002, // "Invalid command line option {0}."

        NoInputFiles = 0x0003, // "No input files."

        UnableToCompileSourceFiles = 0x0004, // "Unable to compile source files."

        TwoManyInputAssemblies = 0x0005, // "Two many input assemblies."

        UnableToLoadAssembly = 0x0006, // "Unable to load assembly {0}."

        UnableToTranslateAssembly = 0x0007, // "Unable to translate assembly {0}."

        NotSwf = 0x0008, // "Specified feature is available only in flash runtime"

        InvalidCorlib = 0x0009, // "Invalid corlib"

        #endregion

        #region Type Errors

		UnableToFindType = 0x1001, // "Unable to find type {0}."
        NotLinkedType = 0x1002, // "Specified type {0} is not linked."

        #endregion

        #region Linker Errors

		UnableToFindSymbol = 0x2001, // "Unable to find symbol {0}"

        #endregion

        #region CIL Translator section

		UnableToTranslateMethod = 0x3001, // "Unable to translate method {0}"

        #endregion

        #region ABC Generation Errors

        AbcBadFormat = 0x4001, // "ABC data is corrupt"

        InvalidBranchOffset = 0x4002, // "Invalid branch offset"

        IncompatibleCall = 0x4003, // "Called incompatible method '{0}' only available in Flash Player {1}."

        IncompatibleField = 0x4004, // "Referenced incompatible field '{0}' only available in Flash Version {1}."

        #endregion

        #region SWF Generation Errors

		InvalidFlashVersion = 0x5001, // "Flash Version {0} is not supported. Valid values are 9, 10."
        TagIsNotCharacter = 0x5002, // "Tag is not character"
        UnableToFindImageResource = 0x5003, // "Unable to find image resource '{0}'"
        BadImageResource = 0x5004, // "Bad image resource '{0}'. Unable to load image."
		
        #endregion

        #region Resource Bundle Compiler Errors

		/// <summary>
        /// Format arguments: {0} - resource source, {1} - mimeType
        /// </summary>
        NotSupportedMimeType = 0x6001, // "Unable to embed resource {0}. MimeType {1} is not supported."

        /// <summary>
        /// No format arguments
        /// </summary>
        BadMetaEntry = 0x6001, // "ABC contains invalid ResourceBundle meta entry."

        /// <summary>
        /// Format arguments: {0} - resource bundle, {1} - locale
        /// </summary>
        UnableToResolve = 0x6002, // "Unable to resolve resource bundle {0} for {1} locale"

        /// <summary>
        /// Format arguments: {0} - directive string
        /// </summary>
        UnableToParseDirective = 0x6003, // "Unable to parse directirve '{0}'."

        /// <summary>
        /// Format arguments: {0} - directive string
        /// </summary>
        UnableToResolveEmbed = 0x6004, // "Unable to resolve Embed directirve '{0}'."

        /// <summary>
        /// Format arguments: {0} - image source
        /// </summary>
        UnableToResolveImage = 0x6005, // "Unable to resolve image source '{0}'."

        /// <summary>
        /// Format arguments: {0} - directive string
        /// </summary>
        BadDirective = 0x6006, // "Bad directive '{0}'."

        /// <summary>
        /// Format arguments: {0} - directive string
        /// </summary>
        UnableToResolveClassReference = 0x6006, // "Unable to resolve ClassReference directive '{0}'."

        #endregion

        #region EmbedAssets Section

		FieldIsNotStatic = 0x7001, // "EmbedAttribute is defined on non static field {0}."
        NotFlashRuntime = 0x7002, // "EmbedAttribute can be used only in flash runtime."

        #endregion

        #region RSL Linking Errors

		UnableToResolveRsl = 0x8001, // "Unable to resolve RSL {0}"
        SwcIsNotResolved = 0x8002, // "SWC file is not resolved for given RSL {0}"
        UnableToResolveLibraryDigest = 0x8003, // "Unable to resolve library digest for given RSL {0}"

        #endregion

        Internal = 0x9000, // "Internal compiler error"
	}

	/// <summary>
	/// Defines warning codes.
	/// </summary>
	public enum Warning
	{
	}
}