using System.Collections.Generic;
using DataDynamics.PageFX.CLI.CFG;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.IL
{
	//STEP2 - Flow Graph Analysis
    internal partial class ILTranslator
    {
        /// <summary>
        /// Returns true if current phase is analysis
        /// </summary>
        private bool IsAnalysis
        {
            get { return _phase == Phase.Analysis; }
        }

	    /// <summary>
        /// Performs flowgraph analysis.
        /// </summary>
        private void AnalyzeGraph()
        {
            _phase = Phase.Analysis;
#if DEBUG
            DebugHooks.LogInfo("Flowgraph analysis started");
            DebugHooks.DoCancel();
#endif
            foreach (var bb in Blocks)
            {
#if DEBUG
                DebugHooks.DoCancel();
#endif
                AnalyzeBlock(bb);
            }

            foreach (var bb in Blocks)
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
        /// Called in analysis and translation phases to prepare eval stack for given basic block.
        /// </summary>
        /// <param name="bb">given basic block</param>
        private void EnshureEvalStack(Node bb)
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

            if (IsAnalysis)
            {
                if (!bb.IsAnalysed)
                    throw new ILTranslatorException("Basic block is not analysed yet");
            }
            else
            {
                if (!bb.IsTranslated)
                    throw new ILTranslatorException("Basic block is not translated yet");
            }

#if DEBUG
            DebugHooks.DoCancel();
#endif
            var from = e.From;
            var prevStack = from.Stack;
            bb.Stack = prevStack != null ? prevStack.Clone() : new EvalStack();
        }

	    /// <summary>
        /// Performs analysis for given basic block.
        /// </summary>
        /// <param name="bb">block to analyse</param>
        private void AnalyzeBlock(Node bb)
        {
            if (bb.IsAnalysed) return;
            AnalyzePredecessors(bb);

            if (bb.IsAnalysed) return;
            bb.IsAnalysed = true;

            AnalyzeBlockCore(bb);
        }

	    /// <summary>
        /// Performs analysis of predecessors of given basic block.
        /// </summary>
        /// <param name="bb"></param>
        private void AnalyzePredecessors(Node bb)
        {
            foreach (var e in bb.InEdges)
            {
#if DEBUG
                DebugHooks.DoCancel();
#endif
                if (CheckIncomingBlock(e, bb))
                    AnalyzeBlock(e.From);
            }
        }

	    private void AnalyzeBlockCore(Node bb)
        {
            EnshureEvalStack(bb);

            _block = bb;

            foreach (var instr in bb.Code)
            {
#if DEBUG
                DebugHooks.DoCancel();
#endif
                AnalyzeInstruction(bb, instr);
            }
        }

	    /// <summary>
        /// Performs analysis of given instruction.
        /// </summary>
        /// <param name="bb"></param>
        /// <param name="instr"></param>
        private void AnalyzeInstruction(Node bb, Instruction instr)
        {
            var stack = bb.Stack;

            //NOTE: Can not use instruction.IsCall because it excludes Newobj instruction
            if (instr.IsCall())
            {
                AnalyzeCallInstruction(instr, stack);
            }
            else if (instr.Code == InstructionCode.Newarr)
            {
                var n = stack.Pop();
                var last = n.Last;
                stack.Push(new EvalItem(instr, last));
            }
            else
            {
                var last = stack.Pop(_method, instr);
                //NOTE: We remember last instruction to insert receiver of static call before last instruction
                if (instr.IsBranch)
                {
                    bb.LastInstruction = last;
                }
                else
                {
                    stack.Push(instr, last);
                }
            }
        }

	    #region class CallInfo
        private class CallInfo
        {
            public readonly IMethod Method;
            public readonly Instruction Instruction;
            public bool SwapAfter;

            public bool IsNewobj
            {
                get { return Instruction.Code == InstructionCode.Newobj; }
            }

            public CallInfo(IMethod method, Instruction call)
            {
                Method = method;
                Instruction = call;
            }
        }
        #endregion

        #region AnalyzeCallInstruction
        /// <summary>
        /// Performs analysis of given call instruction
        /// </summary>
        /// <param name="instr"></param>
        /// <param name="stack"></param>
        void AnalyzeCallInstruction(Instruction instr, EvalStack stack)
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
                SetParam(arg.Instruction, p, method);
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
                r.Instruction.BoxingType = ResolveBoxingType(instr, type);
            }

            last = FixReceiverInsertPoint(last, method);
            if (!(method.IsInitializeArray() || isGetTypeFromHandle))
            {
                Instruction dup;
                if (IsDup(stack, out dup))
                {
                    var call = new CallInfo(method, instr)
                                   {
                                       SwapAfter = true
                                   };
                    dup.EndStack.Push(call);
                }
                else
                {
                    last.BeginStack.Push(new CallInfo(method, instr));
                }
            }

            if (instr.HasReturnValue())
            {
                stack.Push(new EvalItem(instr, last));
            }
        }
        #endregion

        #region AnalyseCall Utils
        void SetParam(Instruction instr, IParameter p, IMethod method)
        {
            if (FixTernaryParam(instr, p)) return;

            instr.Parameter = p;
            instr.ParameterFor = method;

            if (p.IsByRef)
                AnalyseAddrInstruction(instr);
        }

        void AnalyseAddrInstruction(Instruction instr)
        {
            switch (instr.Code)
            {
                case InstructionCode.Ldloca:
                case InstructionCode.Ldloca_S:
                    {
                        int index = (int)instr.Value;
                        var v = _body.LocalVariables[index];
                        v.IsAddressed = true;
                    }
                    break;

                case InstructionCode.Ldarg_0:
                    if (!_method.IsStatic)
                        _provider.IsThisAddressed = true;
                    break;

                case InstructionCode.Ldarga:
                case InstructionCode.Ldarga_S:
                    {
                        int index = (int)instr.Value;
                        index = ToRealArgIndex(index);
                        if (index >= 0)
                        {
                            var p = _method.Parameters[index];
                            p.IsAddressed = true;
                        }
                        else
                        {
                            if (_method.IsStatic)
                                throw new ILTranslatorException();
                            _provider.IsThisAddressed = true;
                        }
                    }
                    break;
            }
        }

        bool FixTernaryParam(Instruction instr, IParameter p)
        {
            var F = instr.BasicBlock;
            if (F == _block) return false;

            Node T, C;
            if (!F.IsBranchOfTernaryExpression(out T, out C))
                return false;

            //NOTE: T or F can be not analysed yet
            T.Parameter = p;
            F.Parameter = p;

            return true;
        }

        static bool IsNormalTwoWay(Node node)
        {
            return node.IsTwoWay && !node.HasInBackEdges;
        }

        /// <summary>
        /// Determines whether the call node is reacheable from first and second successors of given two way node.
        /// </summary>
        /// <param name="from">given two way node</param>
        /// <returns></returns>
        bool IsCallNodeReachable(Node from)
        {
            //NOTE: _block is call node
            if (!IsNormalTwoWay(from))
                return false;

            return IsCallNodeReachableFromSuccessorsOf(from);
        }

        bool IsCallNodeReachableFromSuccessorsOf(Node from)
        {
            if (from == _block) return true;
            foreach (var s in from.Successors)
            {
                if (s == _block) continue;
                if (!_block.IsReachable(s))
                    return false;
                if (!IsCallNodeReachableFromSuccessorsOf(s))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Finds begin of boolean or ternary expression
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        Node FindBeginOfExpression(Node b)
        {
            var e = b.FirstIn;
            if (e == null) return b;
            while (true)
            {
                var from = e.From;
                if (!IsCallNodeReachable(from))
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
        Instruction FixReceiverInsertPoint(Instruction last, IMethod method)
        {
            var b = last.BasicBlock;
            if (b == _block) return last;

            var e = b.FirstIn;
            if (e == null) return last;

            var from = e.From;
            //NOTE: Loop headers are ignored.
            if (!IsNormalTwoWay(from))
                return last;

            if (last.Index > b.EntryIndex)
                return last;

            b = FindBeginOfExpression(b);

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

        #region Utils

        private IType ResolveBoxingType(IInstruction instr, IType methodDeclType)
        {
            var type = HasConstrainedPrefix(instr) ?? methodDeclType;
        	return IsBoxableType(type) ? type : null;
        }

        private bool IsBoxableType(IType type)
        {
            return type != null && !ReferenceEquals(type, _declType)
                   && type.IsBoxableType();
        }

        private IType HasConstrainedPrefix(IInstruction instr)
        {
            int index = instr.Index - 1;
            if (index < 0) return null;
            var prev = _body.Code[index];
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

        #endregion
    }
}