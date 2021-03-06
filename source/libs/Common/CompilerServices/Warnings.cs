namespace DataDynamics.PageFX.Common.CompilerServices
{
    public static class Warnings
    {
        public static readonly ErrorInfo InvalidLocale = new ErrorInfo(0x0001, "Specified locale {0} is invalid", true);

        /// <summary>
        /// Format arguments: {0} - display string
        /// </summary>
        public static readonly ErrorInfo InvalidDebuggerDisplayString = new ErrorInfo(0x0002, "DebuggerDisplay attribute has invalid display string '{0}'. Please check balance of braces.", true);

        /// <summary>
        /// Format arguments: {0} - format name
        /// </summary>
		public static readonly ErrorInfo UnsupportedFormat = new ErrorInfo(0x0003, "Specified format '{0}' is unknown. SWF format will be used.", true);

		/// <summary>
		/// Format arguments: {0} - type full name
		/// </summary>
		public static readonly ErrorInfo UnableImportType = new ErrorInfo(0x0004, "Unable to import type {0}.", true);

		/// <summary>
		/// Format arguments: {0} - type full name
		/// </summary>
		public static readonly ErrorInfo UnableFindSwfAsset = new ErrorInfo(0x0005, "Unable to find SWF asset {0}.", true);
    }
}