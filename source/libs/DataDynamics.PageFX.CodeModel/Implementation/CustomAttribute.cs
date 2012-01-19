using System;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.CodeModel.Syntax;

namespace DataDynamics.PageFX.CodeModel
{
    /// <summary>
    /// Represents custom attribute.
    /// </summary>
    public class CustomAttribute : ICustomAttribute
    {
        #region Constructors
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

    	#endregion

        #region ICustomAttribute Members
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
            get { return _args; }
        }
        private readonly ArgumentCollection _args = new ArgumentCollection();

        public IArgumentCollection FixedArguments
        {
            get { return new ArgumentCollection(_args.Where(a => a.IsFixed)); }
        }

        public IArgumentCollection NamedArguments
        {
            get { return new ArgumentCollection(_args.Where(a => a.IsNamed)); }
        }
        #endregion

        #region ICodeNode Members

        public CodeNodeType NodeType
        {
            get { return CodeNodeType.Attribute; }
        }

        public IEnumerable<ICodeNode> ChildNodes
        {
            get { return null; }
        }

    	/// <summary>
    	/// Gets or sets user defined data assotiated with this object.
    	/// </summary>
    	public object Tag { get; set; }

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
        	foreach (var item in _args.Select(arg => (IArgument)arg.Clone()))
        	{
        		attr._args.Add(item);
        	}
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

    public class CustomAttributeCollection : List<ICustomAttribute>, ICustomAttributeCollection
    {
        #region ICustomAttributeCollection Members
        public ICustomAttribute[] this[IType type]
        {
            get
            {
                var list = new List<ICustomAttribute>();
                foreach (var attr in list)
                {
                    if (attr.Type == type)
                        list.Add(attr);
                }
                return list.ToArray();
            }
        }

        public ICustomAttribute[] this[string typeFullName]
        {
            get
            {
                var list = new List<ICustomAttribute>();
                foreach (var attr in list)
                {
                    if (attr.Type.FullName == typeFullName)
                        list.Add(attr);
                }
                return list.ToArray();
            }
        }
        #endregion

        #region ICodeNode Members

        public CodeNodeType NodeType
        {
            get { return CodeNodeType.Attributes; }
        }

        public IEnumerable<ICodeNode> ChildNodes
        {
            get { return CMHelper.Convert(this); }
        }

    	/// <summary>
    	/// Gets or sets user defined data assotiated with this object.
    	/// </summary>
    	public object Tag { get; set; }

    	#endregion

        #region IFormattable Members
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return SyntaxFormatter.Format(this, format, formatProvider);
        }
        #endregion
    }

    public class CustomAttributeProvider : ICustomAttributeProvider
    {
        public ICustomAttributeCollection CustomAttributes
        {
            get { return _attributes; }
        }
        private readonly CustomAttributeCollection _attributes = new CustomAttributeCollection();
    }
}