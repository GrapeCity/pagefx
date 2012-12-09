using System.Collections.Generic;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Services;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Ecma335.IL;
using DataDynamics.PageFX.Ecma335.Translation.ControlFlow;

namespace DataDynamics.PageFX.Ecma335.Translation
{
    internal static class Analyzer
    {
	    /// <summary>
		/// Performs flowgraph analysis.
		/// </summary>
		public static void Analyze(IClrMethodBody body, ICodeProvider provider)
		{
			Analyze(new Context(body, provider));
		}

		private class Context
		{
			public readonly IClrMethodBody Body;
			public readonly IMethod Method;
			public readonly ICodeProvider Provider;

			public Context(IClrMethodBody body, ICodeProvider provider)
			{
				Body = body;
				Method = body.Method;
				Provider = provider;
			}
		}

	    private static void Analyze(Context context)
        {
#if DEBUG
            DebugHooks.LogInfo("Flowgraph analysis started");
            DebugHooks.DoCancel();
#endif
            foreach (var bb in context.Body.ControlFlowGraph.Blocks)
            {
#if DEBUG
                DebugHooks.DoCancel();
#endif
                AnalyzeBlock(bb, context);
            }

            foreach (var bb in context.Body.ControlFlowGraph.Blocks)
            {
#if DEBUG
                DebugHooks.DoCancel();
#endif
                bb.Stack = null;
            }

#if DEBUG
            DebugHooks.LogInfo("Flowgraph analysis succeeded");
            DebugHooks.DoCancel();
#endif
        }

	    /// <summary>
	    /// Performs analysis for given basic block.
	    /// </summary>
	    /// <param name="bb">block to analyse</param>
	    /// <param name="context"></param>
	    private static void AnalyzeBlock(Node bb, Context context)
        {
            if (bb.IsAnalysed) return;
            AnalyzePredecessors(bb, context);

            if (bb.IsAnalysed) return;
            bb.IsAnalysed = true;

            AnalyzeBlockCore(bb, context);
        }

	    /// <summary>
	    /// Performs analysis of predecessors of given basic block.
	    /// </summary>
	    /// <param name="bb"></param>
	    /// <param name="context"></param>
	    private static void AnalyzePredecessors(Node bb, Context context)
        {
            foreach (var e in bb.InEdges)
            {
#if DEBUG
                DebugHooks.DoCancel();
#endif
                if (ShouldAnalyzePredecessor(e, bb))
                    AnalyzeBlock(e.From, context);
            }
        }

		private static bool ShouldAnalyzePredecessor(Edge e, Node bb)
		{
			if (e == null) return false;
			if (e.IsBack) return false;
			var from = e.From;
			if (from == bb) return false;
			if (from.IsAnalysed) return false;
			if (from.HasInBackEdges) return false;
			return true;
		}

	    private static void AnalyzeBlockCore(Node bb, Context context)
        {
            bb.EnshureEvalStack();

            foreach (var instr in bb.Code)
            {
#if DEBUG
                DebugHooks.DoCancel();
#endif
                AnalyzeInstruction(bb, instr, context);
            }
        }

	    /// <summary>
	    /// Performs analysis of given instruction.
	    /// </summary>
	    /// <param name="currentBlock"></param>
	    /// <param name="instr"></param>
	    /// <param name="context"></param>
	    private static void AnalyzeInstruction(Node currentBlock, Instruction instr, Context context)
        {
            var stack = currentBlock.Stack;

            //NOTE: Can not use instruction.IsCall because it excludes Newobj instruction
            if (instr.IsCall())
            {
                AnalyzeCallInstruction(currentBlock, instr, stack, context);
            }
            else if (instr.Code == InstructionCode.Newarr)
            {
                var n = stack.Pop();
                var last = n.Last;
                stack.Push(new EvalItem(instr, last));
            }
            else
            {
                var last = stack.Pop(context.Method, instr);
                //NOTE: We remember last instruction to insert receiver of static call before last instruction
                if (instr.IsBranch)
                {
                    currentBlock.LastInstruction = last;
                }
                else
                {
                    stack.Push(instr, last);
                }
            }
        }

	    #region AnalyzeCallInstruction

	    /// <summary>
	    /// Performs analysis of given call instruction
	    /// </summary>
	    /// <param name="currentBlock"></param>
	    /// <param name="instr"></param>
	    /// <param name="stack"></param>
	    /// <param name="context"></param>
	    private static void AnalyzeCallInstruction(Node currentBlock, Instruction instr, EvalStack stack, Context context)
        {
            var method = instr.Method;
            if (method == null)
                throw new ILTranslatorException(string.Format("Instruction {0} has no method", instr.Code));

            int n = method.Parameters.Count;
            var type = method.DeclaringType;

            bool isGetTypeFromHandle = method.IsGetTypeFromHandle();

            var last = instr;
            for (int i = n - 1; i >= 0; --i)
            {
                var p = method.Parameters[i];
                var arg = stack.Pop();
                last = arg.Last;
                SetParam(currentBlock, arg.Instruction, p, method, context);
                p.Instruction = arg.Instruction;
                //p.Instruction = last;

                if (isGetTypeFromHandle && !arg.IsTypeToken)
                {
                    isGetTypeFromHandle = false;
                }
            }

            if (instr.HasReceiver())
            {
                var r = stack.Pop();
                last = r.Last;
                r.Instruction.ReceiverFor = instr;
                r.Instruction.BoxingType = ResolveBoxingType(instr, type, context);
            }

            last = FixReceiverInsertPoint(currentBlock, last, method);
            if (!(method.IsInitializeArray() || isGetTypeFromHandle))
            {
                Instruction dup;
                if (IsDup(stack, out dup))
                {
                    var call = new CallInstructionInfo(method, instr)
                                   {
                                       SwapAfter = true
                                   };
                    dup.EndStack.Push(call);
                }
                else
                {
                    last.BeginStack.Push(new CallInstructionInfo(method, instr));
                }
            }

            if (instr.HasReturnValue())
            {
                stack.Push(new EvalItem(instr, last));
            }
        }
        #endregion

        #region AnalyseCall Utils
        private static void SetParam(Node currentBlock, Instruction instr, IParameter p, IMethod method, Context context)
        {
            if (FixTernaryParam(currentBlock, instr, p)) return;

            instr.Parameter = p;
            instr.ParameterFor = method;

            if (p.IsByRef())
                AnalyseAddrInstruction(instr, context);
        }

        private static void AnalyseAddrInstruction(Instruction instr, Context context)
        {
            switch (instr.Code)
            {
                case InstructionCode.Ldloca:
                case InstructionCode.Ldloca_S:
                    {
                        int index = (int)instr.Value;
                        var v = context.Body.LocalVariables[index];
                        v.IsAddressed = true;
                    }
                    break;

                case InstructionCode.Ldarg_0:
                    if (!context.Method.IsStatic)
                        context.Provider.IsThisAddressed = true;
                    break;

                case InstructionCode.Ldarga:
                case InstructionCode.Ldarga_S:
                    {
                        int index = (int)instr.Value;
                        index = ToRealArgIndex(context.Method, index);
                        if (index >= 0)
                        {
                            var p = context.Method.Parameters[index];
                            p.IsAddressed = true;
                        }
                        else
                        {
                            if (context.Method.IsStatic)
                                throw new ILTranslatorException();
                            context.Provider.IsThisAddressed = true;
                        }
                    }
                    break;
            }
        }

        private static bool FixTernaryParam(Node currentBlock, Instruction instr, IParameter p)
        {
            var F = instr.BasicBlock;
			if (F == currentBlock) return false;

            Node T, C;
            if (!F.IsBranchOfTernaryExpression(out T, out C))
                return false;

            //NOTE: T or F can be not analysed yet
            T.Parameter = p;
            F.Parameter = p;

            return true;
        }

        private static bool IsNormalTwoWay(Node node)
        {
            return node.IsTwoWay && !node.HasInBackEdges;
        }

        /// <summary>
        /// Determines whether the call node is reacheable from first and second successors of given two way node.
        /// </summary>
        /// <param name="from">given two way node</param>
        /// <returns></returns>
        private static bool IsCallNodeReachable(Node currentBlock, Node from)
        {
            //NOTE: _block is call node
            if (!IsNormalTwoWay(from))
                return false;

            return IsCallNodeReachableFromSuccessorsOf(currentBlock, from);
        }

        private static bool IsCallNodeReachableFromSuccessorsOf(Node currentBlock, Node from)
        {
			if (from == currentBlock) return true;
            foreach (var s in from.Successors)
            {
				if (s == currentBlock) continue;
				if (!currentBlock.IsReachable(s))
                    return false;
				if (!IsCallNodeReachableFromSuccessorsOf(currentBlock, s))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Finds begin of boolean or ternary expression
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        private static Node FindBeginOfExpression(Node currentBlock, Node b)
        {
            var e = b.FirstIn;
            if (e == null) return b;
            while (true)
            {
                var from = e.From;
                if (!IsCallNodeReachable(currentBlock, from))
                    return e.To;

                e = from.FirstIn;
                if (e == null)
                    return from;
            }
        }

        /// <summary>
        /// Fixes instruction before which receiver should be inserted
        /// </summary>
        /// <param name="last"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        private static Instruction FixReceiverInsertPoint(Node currentBlock, Instruction last, IMethod method)
        {
            var b = last.BasicBlock;
            if (b == currentBlock) return last;

            var e = b.FirstIn;
            if (e == null) return last;

            var from = e.From;
            //NOTE: Loop headers are ignored.
            if (!IsNormalTwoWay(from))
                return last;

            if (last.Index > b.EntryIndex)
                return last;

            b = FindBeginOfExpression(currentBlock, b);

            //Check instruction for first argument;
            //Debug.Assert(method.Parameters.Count > 0);
            //var fp = method.Parameters[0];
            //var fpi = (Instruction)fp.Instruction;
            //if (fpi.OwnerNode == b)
            //    return fpi;

            //if (b.LastInstruction == null)
            //    return b.Code[0];
            //return b.LastInstruction;

            return b.Code[0];
        }
        #endregion

	    private static int ToRealArgIndex(IMethod method, int index)
	    {
		    return method.IsStatic ? index : index - 1;
	    }

	    private static IType ResolveBoxingType(IInstruction instr, IType methodDeclType, Context context)
        {
            var type = HasConstrainedPrefix(instr, context) ?? methodDeclType;
        	return IsBoxableType(type, context) ? type : null;
        }

        private static bool IsBoxableType(IType type, Context context)
        {
            return type != null && !ReferenceEquals(type, context.Method.DeclaringType)
                   && type.IsBoxableType();
        }

        private static IType HasConstrainedPrefix(IInstruction instr, Context context)
        {
            int index = instr.Index - 1;
            if (index < 0) return null;
            var prev = context.Body.Code[index];
            if (prev.Code == InstructionCode.Constrained)
                return prev.Type;
            return null;
        }

        private static bool IsDup(Stack<EvalItem> stack, out Instruction dup)
        {
            dup = null;
            if (stack.Count == 0) return false;
            var v = stack.Peek();
            dup = v.Instruction;
            if (dup == null) return false;
            return dup.Code == InstructionCode.Dup;
        }
    }
}