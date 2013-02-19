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

	    /// <summary>
        /// Gets or sets param attributes.
        /// </summary>
        public ParamAttributes Flags { get; set; }

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

	    public bool IsIn
        {
            get { return (Flags & ParamAttributes.In) != 0; }
        }

        public bool IsOut
        {
            get { return (Flags & ParamAttributes.Out) != 0; }
        }

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

	    public object Clone()
	    {
		    return new Parameter(Type, Name, Index)
			    {
				    Documentation = Documentation,
				    Flags = Flags,
				    Value = Clone(Value),
				    IsAddressed = IsAddressed
			    };
	    }

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

	/// <summary>
	/// Method parameter metadata attribute
	/// </summary>
	[Flags]
	public enum ParamAttributes
	{
		/// <summary>
		/// no flag is specified
		/// </summary>
		None = 0x0000,

		/// <summary>
		/// Param is [In]    
		/// </summary>
		In = 0x0001,

		/// <summary>
		/// Param is [Out]   
		/// </summary>
		Out = 0x0002,

		/// <summary>
		/// Param is [lcid]  
		/// </summary>
		Lcid = 0x0004,

		/// <summary>
		/// Param is [Retval]    
		/// </summary>
		Retval = 0x0008,

		/// <summary>
		/// Param is optional 
		/// </summary>
		Optional = 0x0010,

		/// <summary>
		/// Reserved flags for Runtime use only. 
		/// </summary>
		ReservedMask = 0xf000,

		/// <summary>
		/// Param has default value.
		/// </summary>
		HasDefault = 0x1000,

		/// <summary>
		/// Param has FieldMarshal.
		/// </summary>
		HasFieldMarshal = 0x2000,

		/// <summary>
		/// reserved bit
		/// </summary>
		Reserved3 = 0x4000,

		/// <summary>
		/// reserved bit 
		/// </summary>
		Reserved4 = 0x8000
	}
}