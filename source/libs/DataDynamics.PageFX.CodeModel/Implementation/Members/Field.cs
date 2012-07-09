using System;

namespace DataDynamics.PageFX.CodeModel
{
    public sealed class Field : TypeMember, IField
    {
        /// <summary>
        /// Gets the kind of this member.
        /// </summary>
        public override MemberType MemberType
        {
            get { return MemberType.Field; }
        }

        #region IField Members

    	public int Offset { get; set; }

    	public int Slot { get; set; }

    	public object Value { get; set; }

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
        #endregion
    }
}