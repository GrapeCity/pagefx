using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.CodeModel.TypeSystem;

namespace DataDynamics.PageFX.CLI.Translation.Values
{
    class ComputedPtr : IValue
    {
        readonly IType _type;

        public ComputedPtr(IType type)
        {
            _type = type;
        }

        #region IValue Members
        public IType Type
        {
            get { return _type; }
        }

        public ValueKind Kind
        {
            get { return ValueKind.ComputedPtr; }
        }

        public bool IsPointer
        {
            get { return true; }
        }

        public bool IsMockPointer
        {
            get { return false; }
        }
        #endregion

        public override string ToString()
        {
            return string.Format("ComputedPtr({0})", _type.FullName);
        }
    }
}