using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Collections;

namespace DataDynamics.PageFX.Common.TypeSystem
{
    /// <summary>
    /// Represents type field.
    /// </summary>
    public interface IField : ITypeMember, IConstantProvider
    {
        int Offset { get; }

        /// <summary>
        /// Gets a flag indicating whether the field is compile time constant.
        /// </summary>
        bool IsConstant { get; }

        bool IsReadOnly { get; }

	    IField ProxyOf { get; }

		//TODO: move to runtime field data
		int Slot { get; set; }
    }

    /// <summary>
    /// Represents collection of <see cref="IField"/>s.
    /// </summary>
    public interface IFieldCollection : IReadOnlyList<IField>, ICodeNode
    {
        void Add(IField field);

        /// <summary>
        /// Finds field by specified name.
        /// </summary>
        /// <param name="name">name of field to find</param>
        /// <returns></returns>
        IField this[string name] { get; }
    }
}