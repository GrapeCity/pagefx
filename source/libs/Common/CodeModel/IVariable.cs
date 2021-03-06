using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Common.CodeModel
{
    /// <summary>
    /// Represents local variable.
    /// </summary>
    public interface IVariable : ICodeNode
    {
        /// <summary>
        /// Gets or sets index of the variable.
        /// </summary>
        int Index { get; set; }

        /// <summary>
        /// Gets or sets name of the variable.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets type of the variable.
        /// </summary>
        IType Type { get; set; }

        bool IsPinned { get; set; }

        /// <summary>
        /// Gets or sets flag indicating whether address of this local variable is used onto the evaluation stack.
        /// </summary>
        bool IsAddressed { get; set; }

        IType GenericType { get; set; }
    }
}