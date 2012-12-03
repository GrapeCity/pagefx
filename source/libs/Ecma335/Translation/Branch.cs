using System.Collections.Generic;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Services;
using DataDynamics.PageFX.Ecma335.IL;
using DataDynamics.PageFX.Ecma335.Translation.ControlFlow;

namespace DataDynamics.PageFX.Ecma335.Translation
{
	internal struct Branch
	{
		public readonly IInstruction Source;
		public readonly Node TargetBlock;

		public Branch(IInstruction source, Node targetBlock)
		{
			Source = source;
			TargetBlock = targetBlock;
		}

		/// <summary>
		/// Resolves branch instructions.
		/// </summary>
		public static void Resolve(IList<Branch> branches, ICodeProvider provider)
		{
#if DEBUG
			DebugHooks.LogInfo("ResolveBranches started");
			DebugHooks.DoCancel();
#endif
			for (int i = 0; i < branches.Count; ++i)
			{
#if DEBUG
				DebugHooks.DoCancel();
#endif
				var br = branches[i].Source;
				var bb = branches[i].TargetBlock;
				if (br.IsSwitch)
				{
					var e = bb.FirstOut;
					int deftarget = e.To.TranslatedEntryIndex;
					var targets = new List<int>();
					for (e = e.NextOut; e != null; e = e.NextOut)
					{
						targets.Add(e.To.TranslatedEntryIndex);
					}
					provider.SetCaseTargets(br, targets.ToArray(), deftarget);
				}
				else if (br.IsConditionalBranch)
				{
					var e = bb.TrueEdge;
					provider.SetBranchTarget(br, e.To.TranslatedEntryIndex);
				}
				else //unconditional branch
				{
#if DEBUG
					if (!br.IsUnconditionalBranch) //sanity check
					{
						throw new ILTranslatorException("Invalid branch instruction");
					}
#endif

					var e = bb.FirstOut;
					if (e.To != bb) //avoid cycle!
					{
						provider.SetBranchTarget(br, e.To.TranslatedEntryIndex);
					}
					else
					{
						provider.SetBranchTarget(br, bb.TranslatedExitIndex - 1);
					}
				}
			}
#if DEBUG
			DebugHooks.LogInfo("ResolveBranches succeeded");
			DebugHooks.DoCancel();
#endif
		}
	}
}