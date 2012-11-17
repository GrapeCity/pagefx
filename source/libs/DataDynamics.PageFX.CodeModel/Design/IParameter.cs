using System;
using System.Collections.Generic;

namespace DataDynamics.PageFX.CodeModel
{
    /// <summary>
    /// Represents method or property parameter.
    /// </summary>
    public interface IParameter : ICustomAttributeProvider, IConstantProvider, ICodeNode, IDocumentationProvider, ICloneable
    {
        /// <summary>
        /// Gets or sets index of the paramater.
        /// </summary>
        int Index { get; set; }

        /// <summary>
        /// Gets or sets name of the parameter.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets type of the paramater.
        /// </summary>
        IType Type { get; set; }

        /// <summary>
        /// Gets the flag indicating whether parameter is passed by reference.
        /// </summary>
        bool IsByRef { get; }

        bool IsIn { get; }

        bool IsOut { get; }

        /// <summary>
        /// Gets or sets flags indicating whether the method parameter that takes an argument where the number of arguments is variable.
        /// </summary>
        bool HasParams { get; set; }

        /// <summary>
        /// Gets or sets flag indicating whether address of this parameter used onto the evaluation stack.
        /// </summary>
        bool IsAddressed { get; set; }

        /// <summary>
        /// Gets or sets instruction which is used as result for this argument.
        /// </summary>
        IInstruction Instruction { get; set; }

        /// <summary>
        /// Returns true if type of this parameter was changed during resolving.
        /// </summary>
        bool HasResolvedType { get; }
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