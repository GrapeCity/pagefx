using System;
using System.Collections.Generic;

namespace DataDynamics.PageFX.Common.CodeModel
{
    /// <summary>
    /// Represents node in CodeModel
    /// </summary>
    public interface ICodeNode : IFormattable
    {
	    /// <summary>
        /// Gets the child nodes of this node.
        /// </summary>
        IEnumerable<ICodeNode> ChildNodes { get; }

        /// <summary>
        /// Gets or sets user defined data assotiated with this object.
        /// </summary>
        object Data { get; set; }
    }
}