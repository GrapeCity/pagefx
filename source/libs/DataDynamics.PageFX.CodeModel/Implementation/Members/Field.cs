using System;

namespace DataDynamics.PageFX.CodeModel
{
    public sealed class Field : TypeMember, IField
    {
        /// <summary>
        /// Gets the kind of this member.
        /// </summary>
        public override TypeMemberType MemberType
        {
            get { return TypeMemberType.Field; }
        }

        #region IField Members
        public int Offset
        {
            get { return _offset; }
            set { _offset = value; }
        }
        private int _offset;

        public object Value
        {
            get { return _value; }
            set { _value = value; }
        }
        private object _value;

        public IExpression Initializer
        {
            get { return _initializer; }
            set { _initializer = value; }
        }
        private IExpression _initializer;

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