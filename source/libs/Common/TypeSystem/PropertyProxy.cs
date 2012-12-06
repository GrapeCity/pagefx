using System;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.CodeModel.Expressions;
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
		private ParameterCollection _params;

        public PropertyProxy(IGenericInstance instance, IProperty property)
        {
            _instance = instance;
            _property = property;
        }

	    public IProperty ProxyOf
	    {
			get { return _property; }
	    }

		#region IProperty Members
        public bool HasDefault
        {
            get { return _property.HasDefault; }
            set { throw new NotSupportedException(); }
        }

        public IParameterCollection Parameters
        {
            get
            {
				if (_params == null)
				{
					ResolveSignature();
				}
	            return _params;
            }
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

	    public IExpression Initializer
        {
            get { return _property.Initializer; }
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// Returns true if the property is indexer.
        /// </summary>
        public bool IsIndexer
        {
            get { return _property.IsIndexer; }
        }
        #endregion

        #region IOverridableMember Members
        public bool IsAbstract
        {
            get { return _property.IsAbstract; }
            set { throw new NotSupportedException(); }
        }

        public bool IsVirtual
        {
            get { return _property.IsVirtual; }
            set { throw new NotSupportedException(); }
        }

        public bool IsFinal
        {
            get { return _property.IsFinal; }
            set { throw new NotSupportedException(); }
        }

        public bool IsNewSlot
        {
            get { return _property.IsNewSlot; }
            set { throw new NotSupportedException(); }
        }

        public bool IsOverride
        {
            get { return _property.IsOverride; }
        }
        #endregion

        #region ITypeMember Members
        public IAssembly Assembly
        {
            get { return _property.Assembly; }
        }

        public IModule Module
        {
            get { return _property.Module; }
            set { throw new NotSupportedException(); }
        }

        public MemberType MemberType
        {
            get { return MemberType.Property; }
        }

        public string Name
        {
            get { return _property.Name; }
            set { throw new NotSupportedException(); }
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
            set { }
        }

        public IType Type
        {
            get
            {
	            if (_type == null)
	            {
		            ResolveSignature();
	            }
	            return _type;
            }
            set { throw new NotSupportedException(); }
        }

        public Visibility Visibility
        {
            get { return _property.Visibility; }
            set { throw new NotSupportedException(); }
        }

        public bool IsVisible
        {
            get { return _property.IsVisible; }
        }

        public bool IsStatic
        {
            get { return _property.IsStatic; }
            set { throw new NotSupportedException(); }
        }

        public bool IsSpecialName
        {
            get { return _property.IsSpecialName; }
            set { throw new NotSupportedException(); }
        }

        public bool IsRuntimeSpecialName
        {
            get { return _property.IsRuntimeSpecialName; }
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// Gets or sets value that identifies a metadata element. 
        /// </summary>
        public int MetadataToken
        {
            get { return _property.MetadataToken; }
            set { throw new NotSupportedException(); }
        }
        #endregion

        #region ICustomAttributeProvider Members
        public ICustomAttributeCollection CustomAttributes
        {
            get { return _property.CustomAttributes; }
        }
        #endregion

        #region ICodeNode Members

	    public IEnumerable<ICodeNode> ChildNodes
        {
            get { return new ICodeNode[0]; }
        }

        public object Tag { get; set; }
        #endregion

        #region IFormattable Members
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return SyntaxFormatter.Format(this, format, formatProvider);
        }
        #endregion

        #region IDocumentationProvider Members
        public string Documentation
        {
            get { return _property.Documentation; }
            set { throw new NotSupportedException(); }
        }
        #endregion

        #region IConstantProvider Members
        public object Value
        {
            get { return _property.Value; }
            set { throw new NotSupportedException(); }
        }
        #endregion

        public override string ToString()
        {
            return ToString(null, null);
        }

		private IMethod ResolveGetter()
		{
			if (_property.Getter == null)
				return null;

			_getter =  _instance.Methods.First(x => x.ProxyOf == _property.Getter);

			_getter.Association = this;

			return _getter;
		}

		private IMethod ResolveSetter()
		{
			if (_property.Setter == null)
				return null;

			_setter = _instance.Methods.First(x => x.ProxyOf == _property.Setter);

			_setter.Association = this;

			return _setter;
		}

	    private void ResolveSignature()
	    {
		    if (_property.Getter != null)
			    ResolveSignatureByGetter(Getter);
		    else
			    ResolveSignatureBySetter(Setter);
	    }

	    private void ResolveSignatureByGetter(IMethod getter)
		{
			_type = getter.Type;
			_params = new ParameterCollection();
			foreach (var p in getter.Parameters)
			{
				_params.Add(new Parameter(p.Type, p.Name, p.Index));
			}
		}

		private void ResolveSignatureBySetter(IMethod setter)
		{
			int n = setter.Parameters.Count;

			_type = setter.Parameters[n - 1].Type;

			_params = new ParameterCollection();
			for (int i = 0; i < n - 1; ++i)
			{
				_params.Add(setter.Parameters[i]);
			}
		}
    }
}