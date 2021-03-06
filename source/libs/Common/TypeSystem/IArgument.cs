using System;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Collections;

namespace DataDynamics.PageFX.Common.TypeSystem
{
    public interface IArgument : ICodeNode, ICustomAttributeProvider, ICloneable
    {
        /// <summary>
        /// Gets or sets value type of the argument.
        /// </summary>
        IType Type { get; set; }

        /// <summary>
        /// Gets or sets kind of the argument.
        /// </summary>
        ArgumentKind Kind { get; set; }

        /// <summary>
        /// Gets or sets name of the argument.
        /// </summary>
        string Name { get; set; }
        
        /// <summary>
        /// Gets or sets value of the argument.
        /// </summary>
        object Value { get; set; }

        /// <summary>
        /// Returns true if the argument is fixed.
        /// </summary>
        bool IsFixed { get; }

        /// <summary>
        /// Returns true if the argument is named (not fixed).
        /// </summary>
        bool IsNamed { get; }

        /// <summary>
        /// Gets or sets the member to initialize (actual only for named arguments)
        /// </summary>
        ITypeMember Member { get; set;  }
    }

    public interface IArgumentCollection : IReadOnlyList<IArgument>, ICodeNode
    {
        IArgument this[string name] { get; }

	    void Add(IArgument argument);
    }

	public enum ArgumentKind : byte
	{
		Fixed = 0,
		Field = 0x53,
		Property = 0x54,
	}
}