namespace DataDynamics.PageFX
{
    public static class Warnings
    {
        public static readonly Error InvalidLocale = new Error(0x0001, "Specified locale {0} is invalid", true);

        /// <summary>
        /// Format arguments: {0} - display string
        /// </summary>
        public static readonly Error InvalidDebuggerDisplayString = new Error(0x0002, "DebuggerDisplay attribute has invalid display string '{0}'. Please check balance of braces.");

        /// <summary>
        /// Format arguments: {0} - format name
        /// </summary>
        public static readonly Error UnsupportedFormat = new Error(0x0003, "Specified format '{0}' is unknown. SWF format will be used.");
    }
}