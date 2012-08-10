using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DataDynamics.PageFX.CLI.CFG;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.IL
{
    using Code = List<IInstruction>;

    //STEP3 - Translation of code for every basic block in flow graph
    partial class ILTranslator
    {
        #region fields
        FlowGraph _flowgraph;

	    private IEnumerable<Node> Blocks
        {
            get { return _flowgraph.Blocks; }
        }

        /// <summary>
        /// current analysed or translated basic block
        /// </summary>
        Node _block;

        /// <summary>
        /// Current translated instruction
        /// </summary>
        Instruction _instruction;

        /// <summary>
        /// Header of method body code.
        /// </summary>
        IInstruction[] _beginCode;

        /// <summary>
        /// Footer of method body code.
        /// </summary>
        IInstruction[] _endCode;

    	int _bbIndex;
        bool _popScope;
        bool _castToParamType;
        #endregion

        #region TranslateGraph
        /// <summary>
        /// Translates all basic blocks in flow graph.
        /// </summary>
        void TranslateGraph()
        {
            _phase = Phase.Translation;
#if DEBUG
			DebugHooks.DoCancel();
            DebugHooks.LogInfo("TranslateBlocks started for method: {0}", _method);
#endif

            //Note: CIL pops exception from stack for us if it is not used
            _provider.PopException = false;
            _provider.BeforeTranslation();

			foreach (var bb in Blocks)
			{
#if DEBUG
				DebugHooks.DoCancel();
				//if (DebugHooks.CanBreak(_method)) Debugger.Break();
#endif
				TranslateBlock(bb);
			}

        	_provider.AfterTranslation();

#if DEBUG
            DebugHooks.LogInfo("TranslateBlocks succeeded for method: {0}", _method);
#endif
        }
        #endregion

        #region TranslateBlock
        /// <summary>
        /// Translates given basic block.
        /// </summary>
        /// <param name="bb">basic block to translate.</param>
        /// <remarks>
        /// Also enshures translation of blocks that should be translated before the given block.
        /// It includes incoming blocks, previous handler blocks.
        /// </remarks>
        void TranslateBlock(Node bb)
        {
            if (bb.IsTranslated) return;

            //hook to avoid stack overflow exception.
            if (bb.TranslationIndex < 0)
            {
                bb.TranslationIndex = _bbIndex;
                ++_bbIndex;
            }
            else
            {
                //this can happen in loops.
                bb.TranslationIndex = -1;
                TranslateBlockCore(bb);
                return;
            }

            TranslateBeforeBlock(bb);

            bb.TranslationIndex = -1;

            if (bb.IsTranslated) return;

            TranslateBlockCore(bb);
        }
        #endregion

        #region TranslateBeforeBlock
        void TranslateBeforeBlock(Node bb)
        {
            EnsurePrevHandlerBlock(bb);
            TranslateIncomingBlocks(bb);
            EnsureSehBlocks(bb);
            ReconcileTypes(bb);
        }

        void EnsurePrevHandlerBlock(Node bb)
        {
            var hb = GetHandlerBlock(bb);
            if (hb == null) return;

            int i = hb.Index;
            if (i == 0) return;

            var prev = (HandlerBlock)hb.Owner.Handlers[i - 1];
            var node = prev.EntryPoint.BasicBlock;
            TranslateBlock(node);
        }

        static HandlerBlock GetHandlerBlock(Node bb)
        {
            if (bb.CodeLength == 0) return null;
            var first = bb.Code[0];
            var block = first.SehBlock;
            if (block == null) return null;
            if (block.EntryIndex != first.Index) return null;
            return block as HandlerBlock;
        }

        void TranslateIncomingBlocks(Node bb)
        {
            //NOTE:
            //First we must translate predecessors. Why?
            //1. In order to prepare evaluation stack for translated basic block
            //2. Reconciliation of stack types.
            foreach (var e in bb.InEdges)
            {
                if (CheckIncomingBlock(e, bb))
                    TranslateBlock(e.From);
            }
        }

        bool CheckIncomingBlock(Edge e, Node bb)
        {
            if (e == null) return false;
            if (e.IsBack) return false;
            var from = e.From;
            if (from == bb) return false;
            if (IsAnalysis) if (from.IsAnalysed) return false;
            else if (from.IsTranslated) return false;
            if (from.HasInBackEdges) return false;
            return true;
        }

        void EnsureSehBlocks(Node bb)
        {
            EnsureSehBegin(bb);

            //For basic blocks that is an end of SEH block
            //we must enshure that basic block for SEH block
            //entry instruction is translated
            var block = bb.SehEnd;
            if (block != null)
            {
                EnsureInstructionBlock(bb, block.EntryIndex);
            }
        }

        void EnsureSehBegin(Node bb)
        {
            var block = bb.SehBegin;
            if (block == null) return;
            EnsureSehBlock(bb, block);
        }

        void EnsureSehBlock(Node bb, Block block)
        {
            if (block == null) return;

			if (block.Parent != null)
			{
				EnsureSehBlock(bb, block.Parent);
			}

        	var handler = block as HandlerBlock;
            if (handler != null)
            {
            	EnsureEntryPoints(bb, handler.Owner);
				EnsureInstructionBlock(bb, handler.EntryIndex);
            }
            else
            {
                EnsureEntryPoints(bb, block);
            }
        }

        void EnsureEntryPoints(Node bb, Block seh)
        {
            EnsureInstructionBlock(bb, seh.EntryIndex);

			//we should translate begin basic block of SEH clause before exit basic block
        	var tryBlock = seh as TryCatchBlock;
			if (tryBlock != null)
			{
				foreach (var handler in tryBlock.Handlers.Cast<HandlerBlock>())
				{
					EnsureInstructionBlock(bb, handler.EntryIndex);
				}
			}

        	//TODO: CHECK MANY TESTS it seems it is required now to translate end basic block
            // EnsureInstructionBlock(bb, seh.ExitIndex);
        }

        void EnsureInstructionBlock(Node bb, int index)
        {
        	var n = GetBasicBlockSafe(index);
			if (n != bb) // avoid stackoverflow!
			{
				TranslateBlock(n);
			}
        }

    	private Node GetBasicBlockSafe(int index)
    	{
    		var n = GetInstructionBasicBlock(index);
    		if (n == null)
    			throw new ILTranslatorException("Invalid index of SEH block");
    		return n;
    	}

    	#endregion

        #region CheckStackBalance
        void CheckStackBalance(Node bb)
        {
            int nbefore = bb.StackBefore.Count;
            if (bb.InEdges.Select(e => e.From).Any(from => from.IsTranslated && nbefore != from.Stack.Count))
            {
            	throw new ILTranslatorException();
            }

            int nafter = bb.Stack.Count;
            if (bb.InEdges.Select(e => e.To).Any(to => to.IsTranslated && nafter != to.Stack.Count))
            {
            	throw new ILTranslatorException();
            }
        }
        #endregion

        #region ReconcileTypes
        static bool PeekType(Edge e, ref IType type)
        {
            if (e == null) return false;
            var b = e.From;
            if (!b.IsTranslated) return false;
            var stack = b.Stack;
            if (stack.Count == 0) return false;
            var p = b.PartOfTernaryParam;
            type = p != null ? GetParamType(p) : stack.Peek().Type;
            return true;
        }

        void ReconcileTypes(Node bb)
        {
            var e1 = bb.FirstIn;
            IType type1 = null;
            if (!PeekType(e1, ref type1)) return;

            var e2 = e1.NextIn;
            IType type2 = null;
            if (!PeekType(e2, ref type2)) return;

            if (type1 == type2) return;

            var commonAncestor = type1.GetCommonAncestor(type2);

            bb.IsFirstAssignment = false;
            InsertCast(e1.From, type1, commonAncestor);
            InsertCast(e2.From, type2, commonAncestor);
        }
        #endregion

        #region TranslateBlockCore
        void TranslateBlockCore(Node bb)
        {
            bb.IsTranslated = true;
            _block = bb;
            EnshureEvalStack(bb);

            bb.StackBefore = bb.Stack.Clone();

            int n = bb.Code.Count;
            if (n > 0)
            {
                TranslateBlockCode(bb);
                Optimize(bb);
            }

			EnshureNotEmpty(_block);

            _block = null;
        }

        void Optimize(Node bb)
        {
            if (!GlobalSettings.EnableOptimization) return;
            var code = bb.TranslatedCode;
            int n = code.Count;
            if (n <= 0) return;
            var before = code.ToArray();
            var after = _provider.OptimizeBasicBlock(before);
            if (ReferenceEquals(before, after)) return;
            code.Clear();
            code.AddRange(after);
        }
        #endregion

        #region TranslateBlockCode
        /// <summary>
        /// Translates code of current translated basic block
        /// </summary>
        void TranslateBlockCode(Node bb)
        {
            BeginBlock();

            int n = bb.Code.Count;
            var code = bb.Code;
            for (int i = 0; i < n; ++i)
            {
                _instruction = code[i];
                TranslateInstruction();
            }

            //FIX: For ternary params
            CastToBlockParam();

            EndBlock();
        }

        //Prepares translator for current block.
        void BeginBlock()
        {
            LabelBlock();
            PushScope();
            SehBegin();
        }

        void EndBlock()
        {
            PopScope();
        }

        bool CanLabel()
        {
            var first = _block.Code[0];
            if (first.IsBranchTarget)
                return true;
            
            if (_block.IsSehBegin)
                return false;

            //if (first.IsUnconditionalBranch
            //    || first.IsThrow
            //    || first.IsReturn)
            //    return false;

            return _block.IsNWay || _block.Predecessors.Any(p => p.IsNWay);
        }

        void LabelBlock()
        {
            int n = _block.CodeLength;
            if (n == 0) return;
            if (CanLabel())
            {
                var label = _provider.Label();
                if (label != null)
                    EmitBlockInstruction(label);
            }
        }

        void PushScope()
        {
            _popScope = true;
        }

        void PopScope()
        {
            if (_popScope)
            {
                _popScope = false;
                SehEnd();
            }
        }
        #endregion

        #region Debug Hooks
#if DEBUG
        bool IsMain
        {
            get { return _method.IsStatic && _method.Name == "Main"; }
        }

		bool IsTest
		{
			get { return _method.IsStatic && _method.Name == "Test"; }
		}

        bool IsName(string name)
        {
            return _method.Name == name;
        }

        bool IsType(string name)
        {
            var type = _method.DeclaringType;
            return type.FullName == name || type.Name == name;
        }
#endif
        #endregion

        #region Protected & Handler Blocks
        void EnshureNotEmpty(Node bb)
        {
            if (bb.TranslatedCode.Count == 0)
            {
                AddNop(bb);
            }
        }

    	private void AddNop(Node bb)
    	{
    		var instr = _provider.Nop();
    		if (instr == null)
    			throw new NotSupportedException("nop is not supported");
    		bb.TranslatedCode.Add(instr);
    	}

    	void SehBegin()
        {
            var block = _block.SehBegin;
            if (block == null) return;
            switch (block.Type)
            {
                case BlockType.Protected:
                    {
                        var il = _provider.BeginTry();
                        EmitBlockCode(il);
                    }
                    break;

                case BlockType.Catch:
                    {
                        var handlerBlock = (HandlerBlock)block;
                    	handlerBlock.ExceptionVariable = GetExceptionVariable(block);
                        var il = _provider.BeginCatch(handlerBlock);
                        EmitBlockCode(il);
                    }
                    break;

                case BlockType.Fault:
                    {
                        var handlerBlock = (HandlerBlock)block;
                    	var il = _provider.BeginFault(handlerBlock);
                        EmitBlockCode(il);
                    }
                    break;

                case BlockType.Finally:
                    {
                        var handlerBlock = (HandlerBlock)block;
                    	var il = _provider.BeginFinally(handlerBlock);
                        EmitBlockCode(il);
                    }
                    break;

                case BlockType.Filter:
                    throw new NotSupportedException();

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        int GetExceptionVariable(Block b)
        {
            if (b.Code == null) return -1;
            int entryIndex = b.EntryIndex;
            if (entryIndex < 0) return -1;
            int exitIndex = b.ExitIndex;

            int n = 0;
            for (int i = entryIndex; i <= exitIndex; ++i)
            {
                var instr = _body.Code[i];
                int pop = CIL.GetPopCount(_method, instr);
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

    	void SehEnd()
        {
            var block = _block.SehEnd;
            if (block == null) return;
            switch (block.Type)
            {
                case BlockType.Protected:
                    {
                        IInstruction jump;
                        var code = _provider.EndTry(false, out jump);
                        EmitBlockCode(code);
                    }
                    break;

                case BlockType.Catch:
                    {
                        IInstruction jump;
                        var code = _provider.EndCatch((HandlerBlock)block, false, false, out jump);
                        EmitBlockCode(code);
                    }
                    break;

                case BlockType.Fault:
                    {
						var code = _provider.EndFault((HandlerBlock)block);
                        EmitBlockCode(code);
                    }
                    break;

                case BlockType.Finally:
                    {
						var code = _provider.EndFinally((HandlerBlock)block);
                        EmitBlockCode(code);
                    }
                    break;

                case BlockType.Filter:
                    throw new NotImplementedException();

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

    	#endregion

        #region TranslateInstruction
        void TranslateInstruction()
        {
            _provider.SourceInstruction = _instruction;

            EmitSequencePoint();
            AddInstructionPrefix();

            _castToParamType = true;

            var code = TranslateInstructionCore();

            //we should box receiver for call on boxable types
            bool rboxed = BoxReceiver(ref code);
            //if (!rboxed)
            //    AdjustReceiverType(ref code);
            
            if (IsEndOfBlock(_instruction))
            {
                CastToBlockParam();
                PopScope();
            }

            if (FilterInstruction())
            {
                EmitBlockCode(code);
                if (_castToParamType)
                    CastToParamType();
                AddInstructionSuffix();
            }
        }

        void AdjustReceiverType(ref IInstruction[] code)
        {
            var call = _instruction.ReceiverFor;
            if (call == null) return;
            if (call.Code != InstructionCode.Call) return;
            var m = call.Method;
            if (m.IsStatic) return;
            if (m.IsConstructor) return;
            var rtype = PeekType();
            if (rtype != m.DeclaringType)
            {
                var cast = new Code();
                Cast(cast, rtype, m.DeclaringType);
                EmitBlockCode(code);
                code = cast.ToArray();
            }
        }

        bool BoxReceiver(ref IInstruction[] code)
        {
            IValue ptr;
            var type = GetReceiverBoxingType(out ptr);
            if (type != null)
            {
                EmitBlockCode(code);

                if (ptr != null)
                {
                    var c = LoadPtr(ptr);
                    EmitBlockCode(c);
                }

                code = _provider.BoxPrimitive(type);
                return true;
            }
            else if (ptr != null)
            {
                EmitBlockCode(code);
                //TODO: Add more comments
                //#BUG 10, see also case 125862
                if (_instruction.Code == InstructionCode.Dup)
                {
                    var ds = ptr as IDupSource;
                    if (ds != null && ds.DupSource != null)
                        code = LoadPtr(ds.DupSource);
                    else
                        code = LoadPtr(ptr);
                    EmitBlockCode(code);
                }
                code = LoadPtr(ptr);
                return true;
            }
            return false;
        }

        static bool IsEndOfBlock(IInstruction instr)
        {
            return instr.IsBranch || instr.IsSwitch || instr.IsReturn || instr.IsThrow;
        }
        #endregion

        #region CastToParamType, CastToBlockParam
        //FIX: For ternary params
        void CastToBlockParam()
        {
            if (_block.Parameter != null)
            {
                var p = _block.Parameter;
                if (CastToParamType(null, p, true))
                    CopyValue(GetParamType(p));
                _block.Parameter = null;
                _block.PartOfTernaryParam = p;
                _castToParamType = false;
            }
        }

        void CastToParamType()
        {
            CastToParamType(_instruction, _instruction.Parameter, false);
        }

        bool CastToParamType(Instruction instr, IParameter p, bool force)
        {
            if (p == null) return false;

            if (_block.Stack.Count == 0) return false;

            var v = Peek();
            if (instr == null)
                instr = v.instruction;

            if (instr != null)
            {
                switch (instr.Code)
                {
                    case InstructionCode.Box:
                    case InstructionCode.Ldtoken:
                        return false;
                }
            }

            if (v.value.Kind == ValueKind.Token)
                return false;

            var vtype = v.Type;
            var ptype = GetParamType(p);

            if (vtype != ptype)
            {
                CheckCast(vtype, ptype);

                if (!force)
                {
                    if (!NeedCast(vtype, ptype))
                        return true;
                }

                EmitCast(vtype, ptype);
            }

            return true;
        }

        static bool NeedCast(IType source, IType target)
        {
            //if (source == null) return true;
            if (source.IsImplicitCast(target))
                return false;
            return true;
        }
        #endregion

        #region CheckCast
        static void CheckCast(IType source, IType target)
        {
            if (IsInvalidCast(source, target))
            {
                throw new InvalidOperationException();
            }
        }

        static bool IsInvalidCast(IType source, IType target)
        {
            if (SystemTypes.IsNumeric(source))
            {
                if (!IsNumEnumOrObject(target))
                    return true;
            }
            else if (SystemTypes.IsNumeric(target))
            {
                if (!IsNumEnumOrObject(source))
                    return true;
            }
            return false;
        }

        static bool IsNumEnumOrObject(IType type)
        {
            if (type == null) return false;
            return SystemTypes.IsNumeric(type)
                   || type.IsEnum
                   || type == SystemTypes.Object
                   || type == SystemTypes.ValueType;
        }
        #endregion

        #region AddInstructionPrefix
        /// <summary>
        /// Adds some code before current translated instruction
        /// </summary>
        void AddInstructionPrefix()
        {
            var stack = _instruction.BeginStack;
            while (stack.Count > 0)
            {
                var item = stack.Pop();

                var call = item as CallInfo;
                if (call != null)
                {
                    //Note: Delegate invokation now is handled as for normal methods
                    BeginCall(call);
                    continue;
                }

                throw new ILTranslatorException("Not implemented");
            }
        }
        #endregion

        #region AddInstructionSuffix
        /// <summary>
        /// Adds some code after current translated instruction.
        /// </summary>
        void AddInstructionSuffix()
        {
            var stack = _instruction.EndStack;
            while (stack.Count > 0)
            {
                var item = stack.Pop();

                var call = item as CallInfo;
                if (call != null)
                {
                    //Note: Delegate invokation now is handled as for normal methods
                    BeginCall(call);
                    continue;
                }

                throw new ILTranslatorException("Not implemented");
            }
        }
        #endregion

        #region BeginCall
        void BeginCall(CallInfo call)
        {
            var code = _provider.LoadReceiver(call.Method, call.IsNewobj);

            bool canSwap = false;
            if (code != null && code.Length > 0)
            {
                canSwap = true;
                EmitBlockCode(code);
            }

            code = _provider.BeginCall(call.Method);
            EmitBlockCode(code);

            if (canSwap && call.SwapAfter)
                EmitSwap();
        }
        #endregion

        #region EmitSequencePoint
        string _debugFile;
        string _curDebugFile;
        int _curDebugLine = -1;

        void EmitSequencePoint()
        {
            var sp = _instruction.SequencePoint;
            if (sp == null) return;

            if (_debugFile == null)
            {
                _debugFile = _curDebugFile = sp.File;
            }
            else if (_curDebugFile != sp.File)
            {
                _curDebugFile = sp.File;
                var il = _provider.DebugFile(_curDebugFile);
                EmitBlockCode(il);
            }

            if (_curDebugLine != sp.StartRow)
            {
                if (_curDebugLine < 0)
                    _provider.DebugFirstLine = sp.StartRow;
                _curDebugLine = sp.StartRow;
                var il = _provider.DebugLine(_curDebugLine);
                EmitBlockCode(il);
            }
        }
        #endregion

        #region Ternary Assignment
        //NOTE: Fix of avm verify error, 
        //when type of trueValue or falseValue in ternary assignment does not equal 
        //to left part of assignment
        bool FixTernaryAssignment(IType type)
        {
            if (!_block.IsFirstAssignment) return false;
            _block.IsFirstAssignment = false;

            Node T, F;
            if (!IsTernary(_block, out T, out F))
                return false;

            if (T.PartOfTernaryParam != null && F.PartOfTernaryParam != null)
                return false;

            var topT = T.Stack.Peek();
            var topF = F.Stack.Peek();
            var typeT = topT.Type;
            var typeF = topF.Type;
            if (typeT == typeF && typeT == type) return false;

            if (IsInvalidCast(typeT, type))
                return false;

            if (IsInvalidCast(typeF, type))
                return false;

            if (typeT != type)
                InsertCast(T, typeT, type);

            if (typeF != type)
                InsertCast(F, typeF, type);

            return true;
        }

        //checks ternary config
        bool IsTernary(Node bb, out Node T, out Node F)
        {
            //   C
            //  / \
            // T   F
            //  \ /
            //   V
            T = null;
            F = null;
            var FE = bb.FirstIn;
            if (FE == null) return false;
            var TE = FE.NextIn;
            if (TE == null) return false;
            if (TE.NextIn != null) return false;

            F = FE.From;
            T = TE.From;
            if (F == T) return false;
            if (!F.HasOneIn) return false;
            if (!T.HasOneIn) return false;

            var C = T.FirstIn.From;
            if (C != F.FirstIn.From) return false;

            return CheckTernaryBranches(T, F);
        }

        bool CheckTernaryBranches(Node T, Node F)
        {
            if (IsAnalysis)
            {
                if (!T.IsAnalysed || !F.IsAnalysed)
                    throw new ILTranslatorException("Incoming blocks are not analysed yet");
            }
            else
            {
                if (!T.IsTranslated || !F.IsTranslated)
                    throw new ILTranslatorException("Incoming blocks are not translated yet");
            }

            int tn = T.Stack.Count;
            if (tn == 0) return false;

            int fn = F.Stack.Count;
            if (fn == 0) return false;

            return tn == fn;
        }

        void InsertCast(Node bb, IType source, IType target)
        {
            if (source == target) return;

            CheckCast(source, target);

            var cast = _provider.Cast(source, target, false);
            if (cast != null)
            {
                var stack = bb.Stack;
                var item = stack.Pop();
                stack.PushResult(item.instruction, target);

                var code = bb.TranslatedCode;
                int n = code.Count;
                if (n == 0)
                    throw new ILTranslatorException("Translated code is empty");

                if (IsBranchOrSwitch(code[n - 1]))
                {
                    code.InsertRange(n - 1, cast);
                }
                else
                {
                    code.AddRange(cast);
                }
            }
        }
        #endregion

        #region Utils
        /// <summary>
        /// Emits given instruction to current block.
        /// </summary>
        /// <param name="instr">instruction to add.</param>
        void EmitBlockInstruction(IInstruction instr)
        {
            if (instr == null)
                throw new ArgumentNullException("instr");
            var list = _block.TranslatedCode;
            int n = list.Count;
            if (n > 0)
            {
                //Remove duplicate instructions
                if (_provider.IsDuplicate(list[n - 1], instr))
                    return;
            }
            list.Add(instr);
        }

        /// <summary>
        /// Adds specified code to current block.
        /// </summary>
        /// <param name="code">set of instructions to add.</param>
        void EmitBlockCode(IEnumerable<IInstruction> code)
        {
            if (code != null)
            {
                foreach (var instr in code)
                    EmitBlockInstruction(instr);
            }
        }

        void EmitCast(IType source, IType target)
        {
            if (target != source)
            {
                //Not working for arrays
                //Pop();
                //PushResult(target);
                EmitBlockCode(_provider.Cast(source, target, false));
            }
        }

        void EmitSwap()
        {
            var i = _provider.Swap();
            if (i == null)
                throw new NotSupportedException("Swap instruction is not supported");
            EmitBlockInstruction(i);
        }

        IType PeekType()
        {
            var v = Peek();
            return v.Type;
        }

        IType GetReceiverBoxingType(out IValue ptr)
        {
            ptr = null;
            if (_instruction.Code == InstructionCode.Box)
                return null;

            EvalItem v;
            if (_instruction.BoxingType != null)
            {
                v = Peek();
                if (v.IsPointer)
                    ptr = v.value;
                return _instruction.BoxingType;
            }

            var call = _instruction.ReceiverFor;
            if (call == null) return null;

            var m = call.Method;

            Debug.Assert(!IsStackEmpty);

            v = Peek();
            if (v.IsPointer)
                ptr = v.value;

            if (IsBoxableType(v.Type))
                return v.Type;

            if (IsBoxableType(m.DeclaringType))
                return m.DeclaringType;

            return null;
        }

        static IType GetParamType(IParameter p)
        {
            return p.Type.UnwrapRef();
        }

        bool IsEndOfTryFinally()
        {
            return _instruction.IsEndOfTryFinally;
        }

        bool FilterInstruction()
        {
            if (IsEndOfTryFinally()) return false;
            return true;
        }
        #endregion
    }

    #region class FlowGraph
    class FlowGraph
    {
        /// <summary>
        /// Entry node of flowgraph
        /// </summary>
        public Node Entry;

        /// <summary>
        /// all blocks of flowgraph in order of their layout in code of translated method body.
        /// </summary>
        public NodeList Blocks;
    }
    #endregion
}