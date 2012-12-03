using System;
using DataDynamics.PageFX.CLI.IL;
using DataDynamics.PageFX.CLI.Translation.ControlFlow;
using DataDynamics.PageFX.Common;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.CLI.Translation
{
	internal static class TranslatorExtensions
	{
		/// <summary>
		/// Called in analysis and translation phases to prepare eval stack for given basic block.
		/// </summary>
		/// <param name="bb">given basic block</param>
		public static void EnshureEvalStack(this Node bb)
		{
			//NOTE:
			//Taking into account that stack state must be the same before and after 
			//execution of basic block we can consider only first input edge
			var e = bb.FirstIn;
			if (e == null)
			{
				bb.Stack = new EvalStack();
				return;
			}

#if DEBUG
			DebugHooks.DoCancel();
#endif
			var from = e.From;
			var prevStack = @from.Stack;
			bb.Stack = prevStack != null ? prevStack.Clone() : new EvalStack();
		}

		public static void FixSelfCycle(TranslationContext context)
		{
			var block = context.Block;
			var il = block.TranslatedCode;

			var last = il[il.Count - 1];

			//TODO: do this only for endfinally
			//handle self cycle!
			if (last.IsUnconditionalBranch && block.FirstSuccessor == block)
			{
				if (context.Method.IsVoid())
				{
					var code = context.Code.New();
					code.Label().Return(true);
					il.AddRange(code);
				}
				else
				{
					throw new NotImplementedException();
				}
			}
		}

		public static void AppendCast(this TranslationContext context, IType source, IType target)
		{
			if (ReferenceEquals(source, target)) return;

			Checks.CheckValidCast(source, target);

			var cast = context.Provider.Cast(source, target, false);
			if (cast != null && cast.Length > 0)
			{
				var block = context.Block;
				var stack = block.Stack;
				var item = stack.Pop();
				stack.PushResult(item.Instruction, target);

				var code = block.TranslatedCode;
				int n = code.Count;
				if (n == 0)
					throw new ILTranslatorException("Translated code is empty");

				if (code[n - 1].IsBranchOrSwitch())
				{
					code.InsertRange(n - 1, cast);
				}
				else
				{
					code.AddRange(cast);
				}
			}
		}

		public static IType GetUnwrappedType(this IParameter p)
		{
			return p.Type.UnwrapRef();
		}
	}
}