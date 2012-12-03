using System;
using System.Collections.Generic;
using DataDynamics.PageFX.Common.Expressions;
using DataDynamics.PageFX.Common.Syntax;

namespace DataDynamics.PageFX.Common.TypeSystem
{
    public class FieldProxy : IField
    {
        readonly IGenericInstance _instance;
        readonly IField _field;
        readonly IType _type;

        public FieldProxy(IGenericInstance instance, IField field)
        {
            if (instance == null)
                throw new ArgumentNullException("instance");
            if (field == null)
                throw new ArgumentNullException("field");
            _instance = instance;
            _field = field;
            _type = GenericType.Resolve(instance, field.Type);
        }

        #region IField Members
        public int Offset
        {
            get { return _field.Offset; }
            set { throw new NotSupportedException(); }
        }

		public int Slot
		{
			get { return _field.Slot; }
			set { throw new NotSupportedException(); }
		}

        public bool IsConstant
        {
            get { return _field.IsConstant; }
            set { throw new NotSupportedException(); }
        }

        public bool IsReadOnly
        {
            get { return _field.IsReadOnly; }
            set { throw new NotSupportedException(); }
        }

        public IExpression Initializer
        {
            get { return _field.Initializer; }
            set { throw new NotSupportedException(); }
        }

        public IField ProxyOf
        {
            get { return _field; }
        }
        #endregion

        #region ITypeMember Members
        public IAssembly Assembly
        {
            get { return _field.Assembly; }
        }

        public IModule Module
        {
            get { return _field.Module; }
            set { throw new NotSupportedException(); }
        }

        public MemberType MemberType
        {
            get { return MemberType.Field; }
        }

        public string Name
        {
            get { return _field.Name; }
            set { throw new NotSupportedException(); }
        }

        public string FullName
        {
            get { return _field.FullName; }
        }

        public string DisplayName
        {
            get { return _field.DisplayName; }
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
            get { return _field.Visibility; }
            set { throw new NotSupportedException(); }
        }

        public bool IsVisible
        {
            get { return _field.IsVisible; }
        }

        public bool IsStatic
        {
            get { return _field.IsStatic; }
            set { throw new NotSupportedException(); }
        }

        public bool IsSpecialName
        {
            get { return _field.IsSpecialName; }
            set { throw new NotSupportedException(); }
        }

        public bool IsRuntimeSpecialName
        {
            get { return _field.IsRuntimeSpecialName; }
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// Gets or sets value that identifies a metadata element. 
        /// </summary>
        public int MetadataToken
        {
            get { return _field.MetadataToken; }
            set { throw new NotSupportedException(); }
        }
        #endregion

        #region ICustomAttributeProvider Members
        public ICustomAttributeCollection CustomAttributes
        {
            get { return _field.CustomAttributes; }
        }
        #endregion

        #region ICodeNode Members

        public CodeNodeType NodeType
        {
            get { return CodeNodeType.Field; }
        }

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
            get { return _field.Documentation; }
            set { throw new NotSupportedException(); }
        }
        #endregion

        #region IConstantProvider Members
        public object Value
        {
            get { return _field.Value; }
            set { throw new NotSupportedException(); }
        }
        #endregion

        public override string ToString()
        {
            return ToString(null, null);
        }
    }
}