using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Collections;

namespace DataDynamics.PageFX.Common.TypeSystem
{
    /// <summary>
    /// Represents method or property parameter.
    /// </summary>
    public interface IParameter : ICustomAttributeProvider, IConstantProvider, ICodeNode, IDocumentationProvider
    {
        /// <summary>
        /// Gets index of the paramater.
        /// </summary>
        int Index { get; }

        /// <summary>
        /// Gets name of the parameter.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets type of the paramater.
        /// </summary>
        IType Type { get; }

		bool IsIn { get; }

        bool IsOut { get; }

		/// <summary>
        /// Gets or sets flag indicating whether address of this parameter used onto the evaluation stack.
        /// </summary>
        bool IsAddressed { get; set; }

        /// <summary>
        /// Gets or sets instruction which is used as result for this argument.
        /// </summary>
        IInstruction Instruction { get; set; }
    }

	public interface IParameterProxy : IParameter
	{
		IParameter ProxyOf { get; }
	}

    /// <summary>
    /// Represents collection of <see cref="IParameter"/>s.
    /// </summary>
    public interface IParameterCollection : IReadOnlyList<IParameter>, ICodeNode
    {
        /// <summary>
        /// Finds paramater by given name.
        /// </summary>
        /// <param name="name">name of paramater to find.</param>
        /// <returns></returns>
        IParameter this[string name] { get; }

	    void Add(IParameter parameter);
    }
}