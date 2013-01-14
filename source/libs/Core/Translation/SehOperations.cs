using System;
using DataDynamics.PageFX.Ecma335.IL;

namespace DataDynamics.PageFX.Ecma335.Translation
{
	internal static class SehOperations
	{
		public static void BeginSeh(this TranslationContext context)
		{
			var block = context.Block.SehBegin;
			if (block == null) return;

			var code = context.Code;
			switch (block.Type)
			{
				case BlockType.Protected:
					code.BeginTry();
					break;

				case BlockType.Catch:
					var handlerBlock = (HandlerBlock)block;
					handlerBlock.ExceptionVariable = GetExceptionVariable(context, block);
					code.BeginCatch(handlerBlock);
					break;

				case BlockType.Fault:
					code.BeginFault((HandlerBlock)block);
					break;

				case BlockType.Finally:
					code.BeginFinally((HandlerBlock)block);
					break;

				case BlockType.Filter:
					throw new NotSupportedException();

				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public static void EndSeh(this TranslationContext context)
		{
			var block = context.Block.SehEnd;
			if (block == null) return;

			var code = context.Code;
			switch (block.Type)
			{
				case BlockType.Protected:
					code.EndTry();
					break;

				case BlockType.Catch:
					code.EndCatch((HandlerBlock)block);
					break;

				case BlockType.Fault:
					code.EndFault((HandlerBlock)block);
					break;

				case BlockType.Finally:
					code.EndFinally((HandlerBlock)block);
					break;

				case BlockType.Filter:
					throw new NotImplementedException();

				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private static int GetExceptionVariable(TranslationContext context, Block b)
		{
			if (b.Code == null) return -1;
			int entryIndex = b.EntryIndex;
			if (entryIndex < 0) return -1;
			int exitIndex = b.ExitIndex;

			int n = 0;
			for (int i = entryIndex; i <= exitIndex; ++i)
			{
				var instr = context.Body.Code[i];
				int pop = CIL.GetPopCount(context.Method, instr);
				if (pop == 1 && n == 0)
				{
					switch (instr.Code)
					{
						case InstructionCode.Stloc:
						case InstructionCode.Stloc_S:
							return (int)instr.Value;
						case InstructionCode.Stloc_0: return 0;
						case InstructionCode.Stloc_1: return 1;
						case InstructionCode.Stloc_2: return 2;
						case InstructionCode.Stloc_3: return 3;
					}
					break;
				}
				n -= pop;
				int push = CIL.GetPushCount(instr);
				n += push;
			}

			return -1;
		}

		public static void EmitBeginSeh(this TranslationContext context)
		{
			if (context.Block.SehBegin == null) return;
			context.BeginSeh();
			context.EmitCode();
		}

		public static void EmitEndSeh(this TranslationContext context)
		{
			if (context.Block.SehEnd == null) return;
			context.EndSeh();
			context.EmitCode();
		}
	}
}
