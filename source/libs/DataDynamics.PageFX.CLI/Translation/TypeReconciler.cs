using DataDynamics.PageFX.CLI.Translation.ControlFlow;
using DataDynamics.PageFX.CodeModel.TypeSystem;

namespace DataDynamics.PageFX.CLI.Translation
{
	internal static class TypeReconciler
	{
		/// <summary>
		/// Fix of avm verify error, when type of trueValue or falseValue in ternary assignment does not equal to left part of assignment.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="type"></param>
		internal static bool ReconcileTernaryAssignment(TranslationContext context, IType type)
		{
			var block = context.Block;

			if (!block.IsFirstAssignment) return false;
			block.IsFirstAssignment = false;

			Node trueNode, falseNode;
			if (!IsTernary(block, out trueNode, out falseNode))
				return false;

			if (trueNode.PartOfTernaryParam != null && falseNode.PartOfTernaryParam != null)
				return false;

			var topT = trueNode.Stack.Peek();
			var topF = falseNode.Stack.Peek();
			var typeT = topT.Type;
			var typeF = topF.Type;
			if (ReferenceEquals(typeT, typeF) && ReferenceEquals(typeT, type))
				return false;

			if (Checks.IsInvalidCast(typeT, type))
				return false;

			if (Checks.IsInvalidCast(typeF, type))
				return false;

			if (!ReferenceEquals(typeT, type))
				context.New(trueNode).AppendCast(typeT, type);

			if (!ReferenceEquals(typeF, type))
				context.New(falseNode).AppendCast(typeF, type);

			return true;
		}

		/// <summary>
		/// Determines ternary configuration.
		/// </summary>
		/// <param name="block"></param>
		/// <param name="trueNode"></param>
		/// <param name="falseNode"></param>
		/// <returns></returns>
		private static bool IsTernary(Node block, out Node trueNode, out Node falseNode)
		{
			//   C
			//  / \
			// T   F
			//  \ /
			//   V
			trueNode = null;
			falseNode = null;
			var falseEdge = block.FirstIn;
			if (falseEdge == null) return false;
			var trueEdge = falseEdge.NextIn;
			if (trueEdge == null) return false;
			if (trueEdge.NextIn != null) return false;

			falseNode = falseEdge.From;
			trueNode = trueEdge.From;
			if (falseNode == trueNode) return false;
			if (!falseNode.HasOneIn) return false;
			if (!trueNode.HasOneIn) return false;

			var condition = trueNode.FirstIn.From;
			if (condition != falseNode.FirstIn.From) return false;

			int tn = trueNode.Stack.Count;
			if (tn == 0) return false;

			int fn = falseNode.Stack.Count;
			if (fn == 0) return false;

			return tn == fn;
		}

		public static void Reconcile(TranslationContext context)
		{
			var block = context.Block;
			var e1 = block.FirstIn;
			IType type1 = null;
			if (!PeekType(e1, ref type1)) return;

			var e2 = e1.NextIn;
			IType type2 = null;
			if (!PeekType(e2, ref type2)) return;

			if (ReferenceEquals(type1, type2)) return;

			var commonAncestor = type1.GetCommonAncestor(type2);

			block.IsFirstAssignment = false;
			context.New(e1.From).AppendCast(type1, commonAncestor);
			context.New(e2.From).AppendCast(type2, commonAncestor);
		}

		private static bool PeekType(Edge e, ref IType type)
		{
			if (e == null) return false;
			var b = e.From;
			if (!b.IsTranslated) return false;
			var stack = b.Stack;
			if (stack.Count == 0) return false;
			var p = b.PartOfTernaryParam;
			type = p != null ? p.GetUnwrappedType() : stack.Peek().Type;
			return true;
		}
	}
}
