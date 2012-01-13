using System;
using System.Collections.Generic;
using DataDynamics.PageFX.CodeModel.Syntax;

namespace DataDynamics.PageFX.CodeModel
{
    public class PropertyProxy : IProperty
    {
        readonly IGenericInstance _instance;
        readonly IProperty _property;
        readonly IMethod _getter;
        readonly IMethod _setter;
        readonly IType _type;
        readonly ParameterCollection _params;

        public PropertyProxy(IGenericInstance instance, IProperty property,
            IMethod getter, IMethod setter)
        {
            if (getter == null && setter == null)
                throw new InvalidOperationException("property must have getter or setter");

            _instance = instance;
            _property = property;
            _getter = getter;
            _setter = setter;

            if (getter != null)
                getter.Association = this;
            if (setter != null)
                setter.Association = this;

            _params = new ParameterCollection();
            if (getter != null)
            {
                _type = getter.Type;
                foreach (var p in getter.Parameters)
                {
                    _params.Add(new Parameter(p.Type, p.Name, p.Index));
                }
            }
            else
            {
                int n = setter.Parameters.Count;
                _type = setter.Parameters[n - 1].Type;
                for (int i = 0; i < n - 1; ++i)
                {
                    _params.Add(setter.Parameters[i]);
                }
            }
        }

        #region IProperty Members
        public bool HasDefault
        {
            get { return _property.HasDefault; }
            set { throw new NotSupportedException(); }
        }

        public IParameterCollection Parameters
        {
            get { return _params; }
        }

        public IMethod Getter
        {
            get { return _getter; }
            set { throw new NotSupportedException(); }
        }

        public IMethod Setter
        {
            get { return _setter; }
            set { throw new NotSupportedException(); }
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

        public TypeMemberType MemberType
        {
            get { return TypeMemberType.Property; }
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
            get { return _type; }
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
        public CodeNodeType NodeType
        {
            get { return CodeNodeType.Property; }
        }

        public IEnumerable<ICodeNode> ChildNodes
        {
            get { return CMHelper.EmptyCodeNodes; }
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
    }
}