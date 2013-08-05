using System;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Syntax;

namespace DataDynamics.PageFX.Common.TypeSystem
{
    public sealed class FieldProxy : IField
    {
        private readonly IType _instance;
		private readonly IField _field;
		private readonly IType _type;

        public FieldProxy(IType instance, IField field)
        {
            if (instance == null) throw new ArgumentNullException("instance");
            if (field == null) throw new ArgumentNullException("field");

            _instance = instance;
            _field = field;
            _type = GenericType.Resolve(instance, field.Type);
        }

        #region IField Members

        public int Offset
        {
            get { return _field.Offset; }
        }

		public int Slot
		{
			get { return _field.Slot; }
			set { throw new NotSupportedException(); }
		}

        public bool IsConstant
        {
            get { return _field.IsConstant; }
        }

        public bool IsReadOnly
        {
            get { return _field.IsReadOnly; }
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
        }

        public MemberType MemberType
        {
            get { return MemberType.Field; }
        }

        public string Name
        {
            get { return _field.Name; }
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
        }

        public IType Type
        {
            get { return _type; }
        }

        public Visibility Visibility
        {
            get { return _field.Visibility; }
        }

	    public bool IsStatic
        {
            get { return _field.IsStatic; }
        }

        public bool IsSpecialName
        {
            get { return _field.IsSpecialName; }
        }

        public bool IsRuntimeSpecialName
        {
            get { return _field.IsRuntimeSpecialName; }
        }

        /// <summary>
        /// Gets or sets value that identifies a metadata element. 
        /// </summary>
        public int MetadataToken
        {
            get { return _field.MetadataToken; }
        }

        #endregion

        #region ICustomAttributeProvider Members

        public ICustomAttributeCollection CustomAttributes
        {
            get { return _field.CustomAttributes; }
        }

        #endregion

        #region ICodeNode Members

	    public IEnumerable<ICodeNode> ChildNodes
        {
            get { return Enumerable.Empty<ICodeNode>(); }
        }

    	public object Data { get; set; }

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
        }

        #endregion

        public override string ToString()
        {
            return ToString(null, null);
        }
    }
}