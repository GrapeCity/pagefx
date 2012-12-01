using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.CLI.IL;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.Translation
{
	internal static class Emitter
	{
		/// <summary>
		/// Concatenates code of all basic blocks in order of basic blocks layout.
		/// Prepares output code (instruction list).
		/// </summary>
		public static TranslatorResult Emit(TranslationContext context, string debugFile)
		{
#if DEBUG
			DebugHooks.LogInfo("ConcatBlocks started");
			DebugHooks.DoCancel();
#endif

			var begin = Begin(context, debugFile);

			var provider = context.Provider;
			var branches = new List<Branch>();
			var output = context.Code.New();

			output.Emit(begin);

			foreach (var bb in context.Body.ControlFlowGraph.Blocks)
			{
#if DEBUG
				DebugHooks.DoCancel();
#endif
				//UNCOMMENT TO CHECK STACK BALANCE
				//Checks.CheckStackBalance(bb);

				bb.TranslatedEntryIndex = output.Count;

				var il = bb.TranslatedCode;

				var last = il[il.Count - 1];
				if (last.IsBranchOrSwitch())
				{
					branches.Add(new Branch(last, bb));
				}

				TranslatorExtensions.FixSelfCycle(context.New(bb));
				
				output.Emit(il);

				bb.TranslatedExitIndex = output.Count - 1;
			}

			var end = provider.End();
			output.Emit(end);

#if DEBUG
			DebugHooks.LogInfo("ConcatBlocks succeeded. CodeSize = {0}", output.Count);
			DebugHooks.DoCancel();
#endif
			return new TranslatorResult
				{
					Begin = begin,
					End = end,
					Output = output,
					Branches = branches
				};
		}

		private static IInstruction[] Begin(TranslationContext context, string debugFile)
		{
			var provider = context.Provider;
			var code = context.Code.New();

			// initial debug file
			if (!string.IsNullOrEmpty(debugFile))
			{
				code.DebugFile(debugFile);
			}

			code.AddRange(provider.Begin());

			// declare vars
			if (context.Body.LocalVariables != null)
			{
				var set = context.Body.LocalVariables.SelectMany(v => provider.DeclareVariable(v));
				code.AddRange(set);
			}

			return code.ToArray();
		}
	}
}
