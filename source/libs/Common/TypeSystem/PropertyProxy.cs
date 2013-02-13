using System;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Syntax;

namespace DataDynamics.PageFX.Common.TypeSystem
{
    public sealed class PropertyProxy : IProperty
    {
        private readonly IGenericInstance _instance;
		private readonly IProperty _property;
		private IMethod _getter;
		private IMethod _setter;
		private IType _type;
		private IParameterCollection _parameters;

        public PropertyProxy(IGenericInstance instance, IProperty property)
        {
            _instance = instance;
            _property = property;
        }

	    public IProperty ProxyOf
	    {
			get { return _property; }
	    }

	    public bool HasDefault
        {
            get { return _property.HasDefault; }
        }

        public IParameterCollection Parameters
        {
            get { return _parameters ?? (_parameters = new PropertyParameterCollection(this)); }
        }

        public IMethod Getter
        {
            get { return _getter ?? (_getter = ResolveGetter()); }
			set { _getter = value; }
        }

	    public IMethod Setter
        {
            get { return _setter ?? (_setter = ResolveSetter()); }
			set { _setter = value; }
        }

	    public bool IsAbstract
        {
            get { return _property.IsAbstract; }
        }

        public bool IsVirtual
        {
            get { return _property.IsVirtual; }
        }

        public bool IsFinal
        {
            get { return _property.IsFinal; }
        }

        public bool IsNewSlot
        {
            get { return _property.IsNewSlot; }
        }

        public bool IsOverride
        {
            get { return _property.IsOverride; }
        }

	    public IAssembly Assembly
        {
            get { return _property.Assembly; }
        }

        public IModule Module
        {
            get { return _property.Module; }
        }

        public MemberType MemberType
        {
            get { return MemberType.Property; }
        }

        public string Name
        {
            get { return _property.Name; }
        }

        public string FullName
        {
            get { return _property.FullName; }
        }

        public string DisplayName
        {
            get { return _property.DisplayName; }
        }

        public IType DeclaringType
        {
            get { return _instance; }
        }

        public IType Type
        {
            get { return _type ?? (_type = ResolveType()); }
        }

        public Visibility Visibility
        {
            get { return _property.Visibility; }
        }

	    public bool IsStatic
        {
            get { return _property.IsStatic; }
        }

        public bool IsSpecialName
        {
            get { return _property.IsSpecialName; }
        }

        public bool IsRuntimeSpecialName
        {
            get { return _property.IsRuntimeSpecialName; }
        }

        /// <summary>
        /// Gets or sets value that identifies a metadata element. 
        /// </summary>
        public int MetadataToken
        {
            get { return _property.MetadataToken; }
        }

	    public ICustomAttributeCollection CustomAttributes
        {
            get { return _property.CustomAttributes; }
        }

	    public IEnumerable<ICodeNode> ChildNodes
        {
            get { return new ICodeNode[0]; }
        }

        public object Data { get; set; }

	    public string Documentation
        {
            get { return _property.Documentation; }
            set { throw new NotSupportedException(); }
        }

	    public object Value
        {
            get { return _property.Value; }
            set { throw new NotSupportedException(); }
        }

	    private IMethod ResolveGetter()
		{
			if (_property.Getter == null)
				return null;

			_getter =  _instance.Methods.First(x => x.ProxyOf == _property.Getter);

			return _getter;
		}

	    private IMethod ResolveSetter()
		{
			if (_property.Setter == null)
				return null;

			_setter = _instance.Methods.First(x => x.ProxyOf == _property.Setter);

			return _setter;
		}

	    private IType ResolveType()
	    {
		    var getter = _property.Getter;
		    if (getter != null)
		    {
			    return getter.Type;
		    }

		    var setter = _property.Setter;
			if (setter != null)
			{
				return setter.Parameters[setter.Parameters.Count - 1].Type;
			}

		    return null;
	    }

	    public string ToString(string format, IFormatProvider formatProvider)
	    {
		    return SyntaxFormatter.Format(this, format, formatProvider);
	    }

	    public override string ToString()
	    {
		    return ToString(null, null);
	    }
    }
}