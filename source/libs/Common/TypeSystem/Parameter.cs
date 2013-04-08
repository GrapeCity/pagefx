using System;
using System.Collections.Generic;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Syntax;

namespace DataDynamics.PageFX.Common.TypeSystem
{
	/// <summary>
	/// Mutable parameter implementation.
	/// </summary>
    public sealed class Parameter : CustomAttributeProvider, IParameter
    {
	    public Parameter()
        {
        }

        public Parameter(IType type, string name)
        {
            Type = type;
            Name = name;
        }

        public Parameter(IType type, string name, int index)
        {
            Type = type;
            Name = name;
            Index = index;
        }

		public Parameter(IParameter other)
			: this(other.Type, other.Name, other.Index)
		{
			Documentation = other.Documentation;
			IsIn = other.IsIn;
			IsOut = other.IsOut;
			Value = Clone(Value);
			IsAddressed = other.IsAddressed;
		}

	    public int Index { get; set; }

	    /// <summary>
	    /// Gets or sets param type.
	    /// </summary>
	    public IType Type { get; set; }

	    /// <summary>
	    /// Gets or sets param name
	    /// </summary>
	    public string Name { get; set; }

	    /// <summary>
	    ///  Gets or sets param value
	    /// </summary>
	    public object Value { get; set; }

		public bool IsIn { get; set; }

		public bool IsOut { get; set; }

        /// <summary>
        /// Gets or sets flag indicating whether address of this parameter used onto the evaluation stack.
        /// </summary>
        public bool IsAddressed { get; set; }

        public IInstruction Instruction { get; set; }

	    public IEnumerable<ICodeNode> ChildNodes
        {
            get { return null; }
        }

    	/// <summary>
    	/// Gets or sets user defined data assotiated with this object.
    	/// </summary>
    	public object Data { get; set; }

	    public string ToString(string format, IFormatProvider formatProvider)
        {
            return SyntaxFormatter.Format(this, format, formatProvider);
        }

	    /// <summary>
	    /// Gets or sets documentation of this member
	    /// </summary>
	    public string Documentation { get; set; }

	    private static object Clone(object obj)
        {
            var c = obj as ICloneable;
            if (c != null)
                return c.Clone();
            return obj;
        }

        public override string ToString()
        {
            return ToString(null, null);
        }
    }
}