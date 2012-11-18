using System;

namespace DataDynamics.PageFX.CodeModel
{
    public sealed class Field : TypeMember, IField
    {
	    private int _offset = -1;
	    private object _value;

	    /// <summary>
        /// Gets the kind of this member.
        /// </summary>
        public override MemberType MemberType
        {
            get { return MemberType.Field; }
        }

		public IMetaField Meta { get; set; }

	    public int Offset
	    {
		    get
		    {
			    if (_offset == -1 && Meta != null)
				    return Meta.Offset;
			    return -1;
		    }
		    set { _offset = value; }
	    }

    	public int Slot { get; set; }

	    public object Value
	    {
		    get
		    {
			    if (_value == null && Meta != null)
				    return Meta.Value;
			    return null;
		    }
			set { _value = value; }
	    }

    	public IExpression Initializer { get; set; }

    	public bool IsConstant
        {
            get { return GetModifier(Modifiers.Const); }
            set { SetModifier(value, Modifiers.Const); }
        }

        public bool IsReadOnly
        {
            get { return GetModifier(Modifiers.ReadOnly); }
            set { SetModifier(value, Modifiers.ReadOnly); }
        }

        public IField ProxyOf
        {
            get { throw new NotSupportedException(); }
        }

		protected override IType ResolveType()
		{
			return Meta != null ? Meta.Type : null;
		}

		protected override IType ResolveDeclaringType()
		{
			return Meta != null ? Meta.DeclaringType : null;
		}
    }

	public interface IMetaField
	{
		IType Type { get; }

		IType DeclaringType { get; }

		object Value { get; }

		int Offset { get; }
	}
}