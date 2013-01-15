using DataDynamics.PageFX.Core.IL;

namespace DataDynamics.PageFX.Core.Translation
{
	internal sealed class DebugInfo
	{
		private string _curDebugFile;
		private int _curDebugLine = -1;

		internal string DebugFile { get; private set; }

		public void Inject(TranslationContext context, Instruction instruction)
		{
			var sp = instruction.SequencePoint;
			if (sp == null) return;

			if (DebugFile == null)
			{
				DebugFile = _curDebugFile = sp.File;
			}
			else if (_curDebugFile != sp.File)
			{
				_curDebugFile = sp.File;
				context.Emit(context.Provider.DebugFile(_curDebugFile));
			}

			if (_curDebugLine != sp.StartRow)
			{
				if (_curDebugLine < 0)
					context.Provider.DebugFirstLine = sp.StartRow;
				_curDebugLine = sp.StartRow;

				context.Emit(context.Provider.DebugLine(_curDebugLine));
			}
		}
	}
}
