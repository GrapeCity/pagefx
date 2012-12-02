using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.CodeModel.TypeSystem;

namespace DataDynamics.PageFX.CLI.Translation.Values
{
    class TokenValue : IValue
    {
        public ITypeMember member;

        public TokenValue(ITypeMember member)
        {
            this.member = member;
        }

        public IType Type
        {
            get
            {
                var t = member as IType;
                if (t != null) return t;
                return member.Type;
            }
        }

        public ValueKind Kind
        {
            get { return ValueKind.Token; }
        }

        public bool IsPointer
        {
            get { return false; }
        }

        public bool IsMockPointer
        {
            get { return false; }
        }

        public override string ToString()
        {
            return string.Format("Token({0})", member);
        }
    }
}