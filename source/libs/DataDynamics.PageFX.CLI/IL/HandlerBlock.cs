using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.CodeModel.TypeSystem;

namespace DataDynamics.PageFX.CLI.IL
{
	internal sealed class HandlerBlock : Block, ISehHandlerBlock
	{
		public HandlerBlock(BlockType type)
		{
			_type = type;
		}

		public override BlockType Type
		{
			get { return _type; }
		}
		private readonly BlockType _type;

		public TryCatchBlock Owner { get; set; }

		public int Index
		{
			get
			{
				if (Owner == null) return -1;
				if (_index < 0)
					_index = Owner.Handlers.IndexOf(this);
				return _index;
			}
		}
		private int _index = -1;

		protected override void VisitInstruction(Instruction instruction)
		{
			//FIX:
			//Fix for endfinally, endfilter instructions,
			//in order to avoid promblems with their translation
			if (instruction.Code == InstructionCode.Endfinally && instruction.Value == null)
			{
				var p = Owner.ExitPoint;
				while (true)
				{
					var hb = p.SehBlock as HandlerBlock;
					if (hb == null) break;
					p = hb.Owner.ExitPoint;
				}

				if (p.IsLeave)
				{
					instruction.Value = p.Value;
				}
				else
				{
					bool isLast = instruction.Index == Code.Count - 1;
					instruction.Value = isLast ? instruction.Index : instruction.Index + 1;
				}
			}
		}

		public int FilterIndex;

		ISehTryBlock ISehHandlerBlock.Owner
		{
			get { return Owner; }
		}

		ISehHandlerBlock ISehHandlerBlock.PrevHandler
		{
			get
			{
				if (Owner == null) return null;
				int index = Index;
				if (index > 0)
					return Owner.Handlers[index - 1];
				return null;
			}
		}

		ISehHandlerBlock ISehHandlerBlock.NextHandler
		{
			get
			{
				if (Owner == null) return null;
				int index = Index + 1;
				if (index < Owner.Handlers.Count)
					return Owner.Handlers[index];
				return null;
			}
		}

		public IType ExceptionType { get; set; }

		public int ExceptionVariable { get; set; }

		public IType GenericExceptionType { get; set; }
	}
}