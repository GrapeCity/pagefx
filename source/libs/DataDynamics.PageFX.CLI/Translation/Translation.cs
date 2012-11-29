using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DataDynamics.PageFX.CLI.IL;
using DataDynamics.PageFX.CLI.Translation.ControlFlow;
using DataDynamics.PageFX.CLI.Translation.Values;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.Translation
{
	//STEP3 - Translation of code for every basic block in flow graph
    partial class Translator
    {
        #region fields

	    /// <summary>
        /// current analysed or translated basic block
        /// </summary>
        private Node _block;

        /// <summary>
        /// Current translated instruction
        /// </summary>
        private Instruction _instruction;

        private int _bbIndex;
		private bool _popScope;
		private bool _castToParamType;

        #endregion

	    /// <summary>
        /// Translates all basic blocks in flow graph.
        /// </summary>
		private void TranslateGraph()
        {
#if DEBUG
			DebugHooks.DoCancel();
            DebugHooks.LogInfo("TranslateBlocks started for method: {0}", _method);
#endif

            //Note: CIL pops exception from stack for us if it is not used
            _provider.PopException = false;
            _provider.BeforeTranslation();

			foreach (var bb in _body.ControlFlowGraph.Blocks)
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

	    /// <summary>
        /// Translates given basic block.
        /// </summary>
        /// <param name="bb">basic block to translate.</param>
        /// <remarks>
        /// Also enshures translation of blocks that should be translated before the given block.
        /// It includes incoming blocks, previous handler blocks.
        /// </remarks>
		private void TranslateBlock(Node bb)
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

	    #region TranslateBeforeBlock

		private void TranslateBeforeBlock(Node bb)
        {
            EnsurePrevHandlerBlock(bb);
            TranslateIncomingBlocks(bb);
            EnsureSehBlocks(bb);
            ReconcileTypes(bb);
        }

		private void EnsurePrevHandlerBlock(Node bb)
        {
            var hb = GetHandlerBlock(bb);
            if (hb == null) return;

            int i = hb.Index;
            if (i == 0) return;

            var prev = (HandlerBlock)hb.Owner.Handlers[i - 1];
            var node = prev.EntryPoint.BasicBlock;
            TranslateBlock(node);
        }

		private static HandlerBlock GetHandlerBlock(Node bb)
        {
            if (bb.CodeLength == 0) return null;
            var first = bb.Code[0];
            var block = first.SehBlock;
            if (block == null) return null;
            if (block.EntryIndex != first.Index) return null;
            return block as HandlerBlock;
        }

		private void TranslateIncomingBlocks(Node bb)
        {
            //NOTE:
            //First we must translate predecessors. Why?
            //1. In order to prepare evaluation stack for translated basic block
            //2. Reconciliation of stack types.
            foreach (var e in bb.InEdges)
            {
				if (ShouldTranslatePredecessor(e, bb))
                    TranslateBlock(e.From);
            }
        }

		private static bool ShouldTranslatePredecessor(Edge e, Node bb)
		{
			if (e == null) return false;
			if (e.IsBack) return false;
			var from = e.From;
			if (from == bb) return false;
			if (from.IsTranslated) return false;
			if (from.HasInBackEdges) return false;
			return true;
		}

		private void EnsureSehBlocks(Node bb)
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

		private void EnsureSehBegin(Node bb)
        {
            var block = bb.SehBegin;
            if (block == null) return;
            EnsureSehBlock(bb, block);
        }

		private void EnsureSehBlock(Node bb, Block block)
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

		private void EnsureEntryPoints(Node bb, Block seh)
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

		private void EnsureInstructionBlock(Node bb, int index)
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
		private void CheckStackBalance(Node bb)
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
		private static bool PeekType(Edge e, ref IType type)
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

		private void ReconcileTypes(Node bb)
        {
            var e1 = bb.FirstIn;
            IType type1 = null;
            if (!PeekType(e1, ref type1)) return;

            var e2 = e1.NextIn;
            IType type2 = null;
            if (!PeekType(e2, ref type2)) return;

            if (ReferenceEquals(type1, type2)) return;

            var commonAncestor = type1.GetCommonAncestor(type2);

            bb.IsFirstAssignment = false;
            InsertCast(e1.From, type1, commonAncestor);
            InsertCast(e2.From, type2, commonAncestor);
        }
        #endregion

        #region TranslateBlockCore
		private void TranslateBlockCore(Node bb)
        {
            bb.IsTranslated = true;
            _block = bb;
            bb.EnshureEvalStack();

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

		private void Optimize(Node bb)
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
		private void TranslateBlockCode(Node bb)
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
		private void BeginBlock()
        {
            LabelBlock();
            PushScope();
            SehBegin();
        }

		private void EndBlock()
        {
            PopScope();
        }

		private bool CanLabel()
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

		private void LabelBlock()
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

		private void PushScope()
        {
            _popScope = true;
        }

		private void PopScope()
        {
            if (_popScope)
            {
                _popScope = false;
                SehEnd();
            }
        }
        #endregion

        #region Debug Utils
#if DEBUG
		private bool IsMain
        {
            get { return _method.IsStatic && _method.Name == "Main"; }
        }

		private bool IsTest
		{
			get { return _method.IsStatic && _method.Name == "Test"; }
		}

		private bool IsName(string name)
        {
            return _method.Name == name;
        }

		private bool IsType(string name)
        {
            var type = _method.DeclaringType;
            return type.FullName == name || type.Name == name;
        }
#endif
        #endregion

        #region Protected & Handler Blocks

		private void EnshureNotEmpty(Node bb)
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

		private void SehBegin()
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

		private int GetExceptionVariable(Block b)
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

		private void SehEnd()
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
		private void TranslateInstruction()
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
            
            if (_instruction.IsEndOfBasicBlock())
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

		private void AdjustReceiverType(ref IInstruction[] code)
        {
            var call = _instruction.ReceiverFor;
            if (call == null) return;
            if (call.Code != InstructionCode.Call) return;
            var m = call.Method;
            if (m.IsStatic) return;
            if (m.IsConstructor) return;

            var rtype = PeekType();
            if (!ReferenceEquals(rtype, m.DeclaringType))
            {
                var cast = new Code(_provider);
	            cast.Cast(rtype, m.DeclaringType);
	            EmitBlockCode(code);
                code = cast.ToArray();
            }
        }

		private bool BoxReceiver(ref IInstruction[] code)
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

	    #endregion

        #region CastToParamType, CastToBlockParam
        //FIX: For ternary params
        private void CastToBlockParam()
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

		private void CastToParamType()
        {
            CastToParamType(_instruction, _instruction.Parameter, false);
        }

		private bool CastToParamType(Instruction instr, IParameter p, bool force)
        {
            if (p == null) return false;

            if (_block.Stack.Count == 0) return false;

            var v = Peek();
            if (instr == null)
                instr = v.Instruction;

            if (instr != null)
            {
                switch (instr.Code)
                {
                    case InstructionCode.Box:
                    case InstructionCode.Ldtoken:
                        return false;
                }
            }

            if (v.Value.Kind == ValueKind.Token)
                return false;

            var vtype = v.Type;
            var ptype = GetParamType(p);

            if (!ReferenceEquals(vtype, ptype))
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

		private static bool NeedCast(IType source, IType target)
        {
            //if (source == null) return true;
            if (source.IsImplicitCast(target))
                return false;
            return true;
        }
        #endregion

        #region CheckCast
		private static void CheckCast(IType source, IType target)
        {
            if (IsInvalidCast(source, target))
            {
                throw new InvalidOperationException();
            }
        }

		private static bool IsInvalidCast(IType source, IType target)
        {
            if (source.IsNumeric())
            {
                if (!IsNumericOrEnumOrObject(target))
                    return true;
            }
            else if (target.IsNumeric())
            {
                if (!IsNumericOrEnumOrObject(source))
                    return true;
            }
            return false;
        }

		private static bool IsNumericOrEnumOrObject(IType type)
        {
            if (type == null) return false;
            return type.IsNumeric()
                   || type.IsEnum
                   || type.Is(SystemTypeCode.Object)
                   || type.Is(SystemTypeCode.ValueType);
        }
        #endregion

        #region Instruction Prefix & Suffix
        /// <summary>
        /// Adds some code before current translated instruction
        /// </summary>
		private void AddInstructionPrefix()
        {
            var stack = _instruction.BeginStack;
            while (stack.Count > 0)
            {
                var item = stack.Pop();

                var call = item as CallInstructionInfo;
                if (call != null)
                {
                    //Note: Delegate invokation now is handled as for normal methods
                    BeginCall(call);
                    continue;
                }

                throw new ILTranslatorException("Not implemented");
            }
        }
        
        /// <summary>
        /// Adds some code after current translated instruction.
        /// </summary>
		private void AddInstructionSuffix()
        {
            var stack = _instruction.EndStack;
            while (stack.Count > 0)
            {
                var item = stack.Pop();

                var call = item as CallInstructionInfo;
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
		private void BeginCall(CallInstructionInfo call)
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
		private string _debugFile;
		private string _curDebugFile;
		private int _curDebugLine = -1;

	    internal string DebugFile
	    {
			get { return _debugFile; }
	    }

		private void EmitSequencePoint()
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
		private bool FixTernaryAssignment(IType type)
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
            if (ReferenceEquals(typeT, typeF) && ReferenceEquals(typeT, type)) return false;

            if (IsInvalidCast(typeT, type))
                return false;

            if (IsInvalidCast(typeF, type))
                return false;

            if (!ReferenceEquals(typeT, type))
                InsertCast(T, typeT, type);

            if (!ReferenceEquals(typeF, type))
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

        private static bool CheckTernaryBranches(Node T, Node F)
        {
            int tn = T.Stack.Count;
            if (tn == 0) return false;

            int fn = F.Stack.Count;
            if (fn == 0) return false;

            return tn == fn;
        }

        private void InsertCast(Node bb, IType source, IType target)
        {
            if (ReferenceEquals(source, target)) return;

            CheckCast(source, target);

            var cast = _provider.Cast(source, target, false);
            if (cast != null)
            {
                var stack = bb.Stack;
                var item = stack.Pop();
                stack.PushResult(item.Instruction, target);

                var code = bb.TranslatedCode;
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
        #endregion

        #region Utils
        /// <summary>
        /// Emits given instruction to current block.
        /// </summary>
        /// <param name="i">instruction to add.</param>
		private void EmitBlockInstruction(IInstruction i)
        {
            if (i == null)
                throw new ArgumentNullException("i");
            var list = _block.TranslatedCode;
            int n = list.Count;
            if (n > 0)
            {
				//TODO: remove duplicate instructions block optimization phase
                //Remove duplicate instructions
                if (_provider.IsDuplicate(list[n - 1], i))
                    return;
            }
            list.Add(i);
        }

        /// <summary>
        /// Adds specified code to current block.
        /// </summary>
        /// <param name="code">set of instructions to add.</param>
		private void EmitBlockCode(IEnumerable<IInstruction> code)
        {
            if (code != null)
            {
                foreach (var instr in code)
                    EmitBlockInstruction(instr);
            }
        }

		private void EmitCast(IType source, IType target)
		{
			if (ReferenceEquals(target, source)) return;
			EmitBlockCode(_provider.Cast(source, target, false));
		}

	    private void EmitSwap()
        {
            var i = _provider.Swap();
            if (i == null)
                throw new NotSupportedException("Swap instruction is not supported");
            EmitBlockInstruction(i);
        }

		private IType PeekType()
        {
            var v = Peek();
            return v.Type;
        }

		private IType GetReceiverBoxingType(out IValue ptr)
        {
            ptr = null;
            if (_instruction.Code == InstructionCode.Box)
                return null;

            EvalItem v;
            if (_instruction.BoxingType != null)
            {
                v = Peek();
                if (v.IsPointer)
                    ptr = v.Value;
                return _instruction.BoxingType;
            }

            var call = _instruction.ReceiverFor;
            if (call == null) return null;

            var m = call.Method;

            Debug.Assert(!IsStackEmpty);

            v = Peek();
            if (v.IsPointer)
                ptr = v.Value;

            if (IsBoxableType(v.Type))
                return v.Type;

            if (IsBoxableType(m.DeclaringType))
                return m.DeclaringType;

            return null;
        }

		private bool IsBoxableType(IType type)
		{
			return type != null && !ReferenceEquals(type, _method.DeclaringType)
				   && type.IsBoxableType();
		}

		private static IType GetParamType(IParameter p)
        {
            return p.Type.UnwrapRef();
        }

		private bool IsEndOfTryFinally()
        {
            return _instruction.IsEndOfTryFinally;
        }

		private bool FilterInstruction()
        {
            if (IsEndOfTryFinally()) return false;
            return true;
        }
        #endregion
    }
}