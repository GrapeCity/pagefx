using System.Collections.Generic;

namespace DataDynamics.PageFX.CodeModel
{
    /// <summary>
    /// Represents namespace.
    /// </summary>
    public interface INamespace : ICodeNode
    {
        /// <summary>
        /// Gets or sets name of the namespace.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets the types declared in this namespace.
        /// </summary>
        ITypeCollection Types { get; }
    }

    /// <summary>
    /// Represents collection of namespaces.
    /// </summary>
    public interface INamespaceCollection : IEnumerable<INamespace>, ICodeNode
    {
        int Count { get; }
        INamespace this[int index] { get; }
        INamespace this[string name] { get; }
        void Sort();
    }
}