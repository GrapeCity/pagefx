using System;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Syntax;

namespace DataDynamics.PageFX.Common.TypeSystem
{
    /// <summary>
    /// Represents custom attribute.
    /// </summary>
    public sealed class CustomAttribute : ICustomAttribute
    {
	    public CustomAttribute()
        {
        }

        public CustomAttribute(IMethod ctor)
        {
            if (ctor == null)
                throw new ArgumentNullException("ctor");
            Constructor = ctor;
            Type = ctor.DeclaringType;
        }

        public CustomAttribute(IType type)
            : this(GetDefaultCtor(type))
        {
        }

        public CustomAttribute(string type)
        {
            _typeName = type;
        }

        private static bool IsDeaultCtor(IMethod m)
        {
            if (m.IsStatic) return false;
            if (!m.IsConstructor) return false;
            if (m.Parameters.Count != 0) return false;
            return true;
        }

        private static IMethod GetDefaultCtor(IType type)
        {
        	return type.Methods.FirstOrDefault(IsDeaultCtor);
        }

	    /// <summary>
        /// Gets or sets type name.
        /// </summary>
        public string TypeName
        {
            get { return Type != null ? Type.FullName : _typeName; }
        	set { _typeName = value; }
        }
        private string _typeName;

    	/// <summary>
    	/// Attribute type
    	/// </summary>
    	public IType Type { get; set; }

    	/// <summary>
        /// Attribute target
        /// </summary>
        public ICustomAttributeProvider Owner { get; set; }

    	/// <summary>
    	/// Attribute constructor
    	/// </summary>
    	public IMethod Constructor { get; set; }

    	/// <summary>
        /// Gets the arguments used in attribute constructor.
        /// </summary>
        public IArgumentCollection Arguments
        {
            get { return _args ?? (_args = new ArgumentCollection()); }
			set { _args = value; }
        }
        private IArgumentCollection _args;

        #region ICodeNode Members

	    public IEnumerable<ICodeNode> ChildNodes
        {
            get { return null; }
        }

    	/// <summary>
    	/// Gets or sets user defined data assotiated with this object.
    	/// </summary>
    	public object Data { get; set; }

    	#endregion

        #region IFormattable Members
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return SyntaxFormatter.Format(this, format, formatProvider);
        }
        #endregion

        #region ICloneable Members

	    public object Clone()
	    {
		    var attr = new CustomAttribute
			    {
				    Constructor = Constructor,
				    Type = Type,
				    _typeName = _typeName
			    };
		    var args = new ArgumentCollection();
		    attr.Arguments = args;
		    args.AddRange(Arguments.Select(arg => (IArgument)arg.Clone()));
		    return attr;
	    }

	    #endregion

        #region Object Override Members
        public override string ToString()
        {
            return ToString(null, null);
        }
        #endregion
    }

	public static class CustomAttributeExtensions
	{
		public static IEnumerable<IArgument> FixedArguments(this ICustomAttribute attribute)
		{
			return attribute.Arguments.Where(x => x.IsFixed);
		}

		public static IEnumerable<IArgument> NamedArguments(this ICustomAttribute attribute)
		{
			return attribute.Arguments.Where(x => x.IsNamed);
		}
	}
}