using DataDynamics.PageFX.Common.Services;
using DataDynamics.PageFX.Core.IL;
using DataDynamics.PageFX.Core.Translation.ControlFlow;
using DataDynamics.PageFX.Core.Translation.ControlFlow.Services;

namespace DataDynamics.PageFX.Core.Translation
{
	internal sealed class ControlFlowGraph
	{
		/// <summary>
		/// Entry node of flowgraph
		/// </summary>
		public Node Entry;

		/// <summary>
		/// all blocks of flowgraph in order of their layout in code of translated method body.
		/// </summary>
		public NodeList Blocks;

		public void PushState()
		{
			foreach (var bb in Blocks)
				bb.PushState();
		}

		public void PopState()
		{
			foreach (var bb in Blocks)
				bb.PopState();
		}

		/// <summary>
		/// Builds flowgraph. Prepares list of basic blocks to translate.
		/// </summary>
		public static ControlFlowGraph Build(IClrMethodBody body)
		{
#if DEBUG
			DebugHooks.LogInfo("CFG Builder started");
#endif

			var graph = body.ControlFlowGraph;
			if (graph != null)
			{
#if DEBUG
				body.VisualizeGraph(graph.Entry, false);
#endif
				return graph;
			}

			var builder = new GraphBuilder(body, false) { IsVoidCallEnd = true };
			var entry = builder.Build();
			if (entry == null)
				throw new ILTranslatorException("Unable to build flowgraph");

#if DEBUG
			body.VisualizeGraph(entry, false);
#endif

			//Prepares list of basic blocks in the same order as they are located in code.
			var blocks = new NodeList();
			Node prevNode = null;
			foreach (var instruction in body.Code)
			{
				var bb = instruction.BasicBlock;
				//if (bb == null)
				//    throw new InvalidOperationException();
				if (bb == null) continue;
				if (prevNode == bb) continue;
				blocks.Add(bb);
				prevNode = bb;
			}

			graph = new ControlFlowGraph { Entry = entry, Blocks = blocks };

			body.ControlFlowGraph = graph;

			return graph;
		}
	}
}