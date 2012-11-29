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
		public static TranslatorResult Emit(IClrMethodBody body, Translator translator)
		{
#if DEBUG
			DebugHooks.LogInfo("ConcatBlocks started");
			DebugHooks.DoCancel();
#endif

			var begin = Begin(body, translator);

			var provider = translator.CodeProvider;
			var branches = new List<Branch>();
			var code = new Code(provider);

			code.Append(begin);

			foreach (var bb in body.ControlFlowGraph.Blocks)
			{
#if DEBUG
				DebugHooks.DoCancel();
#endif
				//UNCOMMENT TO CHECK STACK BALANCE
				//CheckStackBalance(bb);

				bb.TranslatedEntryIndex = code.Count;
				var il = bb.TranslatedCode;

				var last = il[il.Count - 1];
				if (last.IsBranchOrSwitch())
				{
					branches.Add(new Branch(last, bb));
				}

				translator.FixSelfCycle(bb);
				
				code.Append(il);

				bb.TranslatedExitIndex = code.Count - 1;
			}

			var end = provider.End();
			code.Append(end);

#if DEBUG
			DebugHooks.LogInfo("ConcatBlocks succeeded. CodeSize = {0}", code.Count);
			DebugHooks.DoCancel();
#endif
			return new TranslatorResult
				{
					Begin = begin,
					End = end,
					Code = code,
					Branches = branches
				};
		}

		private static IInstruction[] Begin(IClrMethodBody body, Translator translator)
		{
			var provider = translator.CodeProvider;
			var code = new Code(provider);

			// initial debug file
			if (!string.IsNullOrEmpty(translator.DebugFile))
			{
				var set = provider.DebugFile(translator.DebugFile);
				code.Append(set);
			}

			code.Append(provider.Begin());

			// declare vars
			if (body.LocalVariables != null)
			{
				var set = body.LocalVariables.SelectMany(v => provider.DeclareVariable(v));
				code.Append(set);
			}

			return code.ToArray();
		}
	}
}
