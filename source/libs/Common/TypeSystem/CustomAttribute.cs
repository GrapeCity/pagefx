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
        	var attr = new CustomAttribute {Constructor = Constructor, Type = Type, _typeName = _typeName};
        	foreach (var arg in _args)
            {
                var arg2 = (IArgument)arg.Clone();
                attr._args.Add(arg2);
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

    public sealed class CustomAttributeCollection : List<ICustomAttribute>, ICustomAttributeCollection
    {
	    public ICustomAttribute[] this[IType type]
        {
            get { return this.Where(x => ReferenceEquals(x.Type, type)).ToArray(); }
        }

        public ICustomAttribute[] this[string typeFullName]
        {
            get { return this.Where(x => x.Type.FullName == typeFullName).ToArray(); }
        }

	    public CodeNodeType NodeType
        {
            get { return CodeNodeType.Attributes; }
        }

        public IEnumerable<ICodeNode> ChildNodes
        {
            get { return this.Cast<ICodeNode>(); }
        }

    	/// <summary>
    	/// Gets or sets user defined data assotiated with this object.
    	/// </summary>
    	public object Tag { get; set; }

	    public string ToString(string format, IFormatProvider formatProvider)
        {
            return SyntaxFormatter.Format(this, format, formatProvider);
        }
    }

    public class CustomAttributeProvider : ICustomAttributeProvider
    {
        public ICustomAttributeCollection CustomAttributes
        {
            get { return _attributes ?? (_attributes = new CustomAttributeCollection()); }
			set { _attributes = value; }
        }
        private ICustomAttributeCollection _attributes;

		/// <summary>
		/// Gets or sets value that identifies a metadata element. 
		/// </summary>
	    public int MetadataToken { get; set; }
    }

	public static class CustomAttributeProviderExtensions
	{
		public static ICustomAttribute FindAttribute(this ICustomAttributeProvider p, string fullname)
		{
			return p.CustomAttributes.FirstOrDefault(attr => attr.TypeName == fullname);
		}

		public static bool HasAttribute(this ICustomAttributeProvider p, string fullname)
		{
			return p.FindAttribute(fullname) != null;
		}

		public static bool HasAttribute(this ICustomAttributeProvider p, params string[] attrs)
		{
			return p.CustomAttributes.Any(attr => attrs.Contains(attr.TypeName));
		}
	}
}