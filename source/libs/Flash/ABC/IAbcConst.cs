using DataDynamics.PageFX.Flash.Swf;

namespace DataDynamics.PageFX.Flash.Abc
{
    /// <summary>
    /// Interfaces ABC constant.
    /// </summary>
    public interface IAbcConst : ISwfIndexedAtom, ISupportXmlDump
    {
        /// <summary>
        /// Returns kind of the constant.
        /// </summary>
        AbcConstKind Kind { get; }

        /// <summary>
        /// Gets or sets value of the constant.
        /// </summary>
        object Value { get; set; }

		/// <summary>
		/// Gets unique key of the constant.
		/// </summary>
        string Key { get; }
    }
}