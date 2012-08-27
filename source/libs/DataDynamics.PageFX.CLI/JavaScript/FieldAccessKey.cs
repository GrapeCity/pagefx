using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.JavaScript
{
	internal sealed class FieldAccessKey
	{
		private readonly IMethod _context;
		private readonly IField _field;
		
		public FieldAccessKey(IMethod context, IField field)
		{
			_context = context;
			_field = field;
		}

		public override bool Equals(object obj)
		{
			var k = obj as FieldAccessKey;
			if (k == null) return false;
			return k._field == _field && k._context == _context;
		}

		public override int GetHashCode()
		{
			return _field.GetHashCode() ^ _context.GetHashCode() ^ typeof(FieldAccessKey).GetHashCode();
		}
	}
}