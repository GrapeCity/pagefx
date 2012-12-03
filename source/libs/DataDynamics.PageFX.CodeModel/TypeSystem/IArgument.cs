using System;
using System.Collections.Generic;

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

    public interface IArgumentCollection : IList<IArgument>, ICodeNode
    {
        IArgument this[string name] { get; }
    }
}