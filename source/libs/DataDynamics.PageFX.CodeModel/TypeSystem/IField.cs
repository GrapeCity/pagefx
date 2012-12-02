using System.Collections.Generic;
using DataDynamics.PageFX.CodeModel.Expressions;

namespace DataDynamics.PageFX.CodeModel.TypeSystem
{
    /// <summary>
    /// Represents type field.
    /// </summary>
    public interface IField : ITypeMember, IConstantProvider
    {
        int Offset { get; set; }

        /// <summary>
        /// Gets or sets flag indicating whether the field is compile time constant.
        /// </summary>
        bool IsConstant { get; set; }

        bool IsReadOnly { get; set; }

        IExpression Initializer { get; set; }

        IField ProxyOf { get; }

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