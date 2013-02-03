using System;

namespace DataDynamics.PageFX.Common.TypeSystem
{
    public class Field : TypeMember, IField
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

	    public int Offset
	    {
		    get
		    {
			    if (_offset == -1)
				    _offset = ResolveOffset();
			    return -1;
		    }
		    set { _offset = value; }
	    }

    	public int Slot { get; set; }

	    public object Value
	    {
		    get { return _value ?? (_value = ResolveValue()); }
		    set { _value = value; }
	    }

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

		protected virtual object ResolveValue()
		{
			return null;
		}

		protected virtual int ResolveOffset()
		{
			return -1;
		}
    }
}