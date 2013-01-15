using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Core.IL;

namespace DataDynamics.PageFX.Core.JavaScript
{
	internal sealed class InstructionKey
	{
		private readonly ITypeMember _member;
		private readonly InstructionCode _code;

		public InstructionKey(InstructionCode code, ITypeMember member)
		{
			_member = member;
			_code = code;
		}

		public override bool Equals(object obj)
		{
			var k = obj as InstructionKey;
			if (k == null) return false;
			return k._member == _member && k._code == _code;
		}

		public override int GetHashCode()
		{
			return _member.GetHashCode() ^ (int)_code ^ typeof(InstructionKey).GetHashCode();
		}
	}
}