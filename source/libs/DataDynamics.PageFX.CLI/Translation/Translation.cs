using System.Linq;
using DataDynamics.PageFX.CLI.IL;
using DataDynamics.PageFX.CLI.Translation.ControlFlow;
using DataDynamics.PageFX.CLI.Translation.Values;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.CodeModel.TypeSystem;

namespace DataDynamics.PageFX.CLI.Translation
{
	//STEP3 - Translation of code for every basic block in flow graph
    internal partial class Translator
    {
	    private int _bbIndex;
		private bool _popScope;

	    /// <summary>
        /// Translates all basic blocks in flow graph.
        /// </summary>
		private void TranslateGraph(TranslationContext context)
        {
#if DEBUG
			DebugHooks.DoCancel();
            DebugHooks.LogInfo("TranslateBlocks started for method: {0}", context.Method);
#endif

            //Note: CIL pops exception from stack for us if it is not used
            context.Provider.PopException = false;
            context.Provider.BeforeTranslation();

			foreach (var bb in context.Body.ControlFlowGraph.Blocks)
			{
#if DEBUG
				DebugHooks.DoCancel();
				//if (DebugHooks.CanBreak(_method)) Debugger.Break();
#endif
				TranslateBlock(context.New(bb));
			}

        	context.Provider.AfterTranslation();

#if DEBUG
            DebugHooks.LogInfo("TranslateBlocks succeeded for method: {0}", context.Method);
#endif
        }

	    /// <summary>
        /// Translates given basic block.
        /// </summary>
        /// <param name="context">Specifies context of basic block to translate.</param>
        /// <remarks>
        /// Also enshures translation of blocks that should be translated before the given block.
        /// It includes incoming blocks, previous handler blocks.
        /// </remarks>
		private void TranslateBlock(TranslationContext context)
	    {
		    var block = context.Block;
            if (block.IsTranslated) return;

            //hook to avoid stack overflow exception.
            if (block.TranslationIndex < 0)
            {
                block.TranslationIndex = _bbIndex;
                ++_bbIndex;
            }
            else
            {
                //this can happen in loops.
                block.TranslationIndex = -1;
                TranslateBlockCore(context);
                return;
            }

            TranslateBeforeBlock(block);

            block.TranslationIndex = -1;

            if (block.IsTranslated) return;

            TranslateBlockCore(context);
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
            TranslateBlock(_context.New(node));
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
                    TranslateBlock(_context.New(e.From));
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
				TranslateBlock(_context.New(n));
			}
        }

    	private Node GetBasicBlockSafe(int index)
    	{
    		var n = _context.Code.GetInstructionBasicBlock(index);
    		if (n == null)
    			throw new ILTranslatorException("Invalid index of SEH block");
    		return n;
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

			var context = _context.New(bb);
            bb.IsFirstAssignment = false;
            InsertCast(context.New(e1.From), type1, commonAncestor);
            InsertCast(context.New(e2.From), type2, commonAncestor);
        }
        #endregion

        #region TranslateBlockCore
		private void TranslateBlockCore(TranslationContext context)
		{
			var block = context.Block;
            block.IsTranslated = true;

            block.EnshureEvalStack();

            block.StackBefore = block.Stack.Clone();

            int n = block.Code.Count;
            if (n > 0)
            {
                TranslateBlockCode(context);
                Optimize(context);
            }

			// ensure target code is not empty
			if (context.Block.TranslatedCode.Count == 0)
			{
				context.Block.TranslatedCode.Add(context.Provider.Nop());
			}
        }

		private static void Optimize(TranslationContext context)
        {
            if (!GlobalSettings.EnableOptimization) return;

			var block = context.Block;
            var code = block.TranslatedCode;
			if (code.Count <= 0) return;

            var before = code.ToArray();
            var after = context.Provider.OptimizeBasicBlock(before);
            if (ReferenceEquals(before, after)) return;

            code.Clear();
            code.AddRange(after);
        }
        #endregion

        #region TranslateBlockCode
        /// <summary>
        /// Translates code of current translated basic block
        /// </summary>
		private void TranslateBlockCode(TranslationContext context)
        {
			BeginBlock(context);

	        foreach (var i in context.Block.Code)
	        {
		        TranslateInstruction(context.New(), i);
	        }

	        //FIX: For ternary params
            CastToBlockParam(context);

			EndBlock(context);
        }

        //Prepares translator for current block.
		private void BeginBlock(TranslationContext context)
        {
            LabelBlock(context);
            PushScope();
            context.EmitBeginSeh();
        }

		private void EndBlock(TranslationContext context)
        {
            PopScope(context);
        }

		private static bool ItShouldBeLabeled(TranslationContext context)
		{
			var block = context.Block;
            var first = block.Code[0];
            if (first.IsBranchTarget)
                return true;
            
            if (block.IsSehBegin)
                return false;

            //if (first.IsUnconditionalBranch
            //    || first.IsThrow
            //    || first.IsReturn)
            //    return false;

            return block.IsNWay || block.Predecessors.Any(p => p.IsNWay);
        }

		private void LabelBlock(TranslationContext context)
        {
            int n = context.Block.CodeLength;
            if (n == 0) return;
			if (!ItShouldBeLabeled(context)) return;

			var label = context.Provider.Label();
			if (label != null)
			{
				context.Emit(label);
			}
        }

		private void PushScope()
        {
            _popScope = true;
        }

		private void PopScope(TranslationContext context)
        {
            if (_popScope)
            {
                _popScope = false;
                context.EmitEndSeh();
            }
        }
        #endregion

	    #region TranslateInstruction
		private void TranslateInstruction(TranslationContext context, Instruction instruction)
        {
            context.Provider.SourceInstruction = instruction;

            EmitSequencePoint(context, instruction);
            AddInstructionPrefix(context, instruction);

            context.CastToParamType = true;

			var core = new InstructionTranslator(context);
			core.Translate(instruction);

			var castToParamType = context.CastToParamType;

			var code = context.Code;
			var set = code.ToArray();

            //we should box receiver for call on boxable types
            BoxReceiver(context, instruction, ref set);
            
            if (instruction.IsEndOfBasicBlock())
            {
                CastToBlockParam(context);
                PopScope(context.New());
            }

            if (WithSuffix(instruction))
            {
	            context.Emit(set);
				if (castToParamType)
                    CastToParamType(context, instruction);
                AddInstructionSuffix(context, instruction);
            }
        }

	    private static void BoxReceiver(TranslationContext context, Instruction instruction, ref IInstruction[] code)
        {
            IValue ptr;
            var type = GetReceiverBoxingType(context, instruction, out ptr);
            if (type != null)
            {
	            context.Emit(code);

	            if (ptr != null)
                {
					var c = LoadPtr(context, ptr);
	                context.Emit(c);
                }

                code = context.Provider.BoxPrimitive(type);
	            return;
            }
            if (ptr != null)
            {
	            context.Emit(code);

	            //TODO: Add more comments
                //#BUG 10, see also case 125862
                if (instruction.Code == InstructionCode.Dup)
                {
                    var ds = ptr as IDupSource;
                    if (ds != null && ds.DupSource != null)
                        code = LoadPtr(context, ds.DupSource);
                    else
                        code = LoadPtr(context, ptr);
	                context.Emit(code);
                }
                code = LoadPtr(context, ptr);
            }
        }

	    private static IInstruction[] LoadPtr(TranslationContext context, IValue ptr)
	    {
		    var code = context.Code.New();
		    code.LoadPtr(ptr);
		    return code.ToArray();
	    }

	    #endregion

        #region CastToParamType, CastToBlockParam
        //FIX: For ternary params
        private static void CastToBlockParam(TranslationContext context)
        {
	        var block = context.Block;
            if (block.Parameter != null)
            {
                var p = block.Parameter;
                if (CastToParamType(context, null, p, true))
                    CopyValue(context, GetParamType(p));
                block.Parameter = null;
                block.PartOfTernaryParam = p;
            }
        }

	    private static void CopyValue(TranslationContext context, IType type)
	    {
		    var copy = context.Provider.CopyValue(type);
		    if (copy != null)
		    {
			    context.Emit(copy);
		    }
	    }

	    private static void CastToParamType(TranslationContext context, Instruction instruction)
        {
            CastToParamType(context, instruction, instruction.Parameter, false);
        }

		private static bool CastToParamType(TranslationContext context, Instruction instruction, IParameter p, bool force)
        {
            if (p == null) return false;

			var block = context.Block;
            if (block.Stack.Count == 0) return false;

            var v = block.Stack.Peek();
            if (instruction == null)
                instruction = v.Instruction;

            if (instruction != null)
            {
                switch (instruction.Code)
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
	            Checks.CheckValidCast(vtype, ptype);

	            if (!force)
                {
                    if (!NeedCast(vtype, ptype))
                        return true;
                }

	            context.EmitCast(vtype, ptype);
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

	    /// <summary>
        /// Adds some code before current translated instruction
        /// </summary>
		private static void AddInstructionPrefix(TranslationContext context, Instruction instruction)
        {
            var stack = instruction.BeginStack;
            while (stack.Count > 0)
            {
                var item = stack.Pop();

                var call = item as CallInstructionInfo;
                if (call != null)
                {
                    //Note: Delegate invokation now is handled as for normal methods
                    BeginCall(context, call);
                    continue;
                }

                throw new ILTranslatorException("Not implemented");
            }
        }
        
        /// <summary>
        /// Adds some code after current translated instruction.
        /// </summary>
		private static void AddInstructionSuffix(TranslationContext context, Instruction instruction)
        {
            var stack = instruction.EndStack;
            while (stack.Count > 0)
            {
                var item = stack.Pop();

                var call = item as CallInstructionInfo;
                if (call != null)
                {
                    //Note: Delegate invokation now is handled as for normal methods
                    BeginCall(context, call);
                    continue;
                }

                throw new ILTranslatorException("Not implemented");
            }
        }

	    private static void BeginCall(TranslationContext context, CallInstructionInfo call)
        {
            var code = context.Provider.LoadReceiver(call.Method, call.IsNewobj);

            bool canSwap = false;
            if (code != null && code.Length > 0)
            {
                canSwap = true;
	            context.Emit(code);
            }

            code = context.Provider.BeginCall(call.Method);
			context.Emit(code);

			if (canSwap && call.SwapAfter)
			{
				context.EmitSwap();
			}
        }

	    #region EmitSequencePoint
		private string _debugFile;
		private string _curDebugFile;
		private int _curDebugLine = -1;

	    internal string DebugFile
	    {
			get { return _debugFile; }
	    }

		private void EmitSequencePoint(TranslationContext context, Instruction currentInstruction)
        {
            var sp = currentInstruction.SequencePoint;
            if (sp == null) return;

            if (_debugFile == null)
            {
                _debugFile = _curDebugFile = sp.File;
            }
            else if (_curDebugFile != sp.File)
            {
                _curDebugFile = sp.File;
	            context.Emit(context.Provider.DebugFile(_curDebugFile));
            }

            if (_curDebugLine != sp.StartRow)
            {
                if (_curDebugLine < 0)
                    context.Provider.DebugFirstLine = sp.StartRow;
                _curDebugLine = sp.StartRow;

	            context.Emit(context.Provider.DebugLine(_curDebugLine));
            }
        }
        #endregion

        #region Ternary Assignment
        //NOTE: Fix of avm verify error, 
        //when type of trueValue or falseValue in ternary assignment does not equal 
        //to left part of assignment
		internal static bool FixTernaryAssignment(TranslationContext context, IType type)
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
                InsertCast(context.New(trueNode), typeT, type);

            if (!ReferenceEquals(typeF, type))
                InsertCast(context.New(falseNode), typeF, type);

            return true;
        }

        //checks ternary config
        private static bool IsTernary(Node bb, out Node trueNode, out Node falseNode)
        {
            //   C
            //  / \
            // T   F
            //  \ /
            //   V
            trueNode = null;
            falseNode = null;
            var falseEdge = bb.FirstIn;
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

	    private static void InsertCast(TranslationContext context, IType source, IType target)
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
        #endregion

        #region Utils

	    private static IType GetReceiverBoxingType(TranslationContext context, Instruction currentInstruction, out IValue ptr)
        {
            ptr = null;
            if (currentInstruction.Code == InstructionCode.Box)
                return null;

		    var block = context.Block;
			EvalItem v;
            if (currentInstruction.BoxingType != null)
            {
                v = block.Stack.Peek();
                if (v.IsPointer)
                    ptr = v.Value;
                return currentInstruction.BoxingType;
            }

            var call = currentInstruction.ReceiverFor;
            if (call == null) return null;

            var m = call.Method;

            v = block.Stack.Peek();
            if (v.IsPointer)
                ptr = v.Value;

            if (IsBoxableType(context, v.Type))
                return v.Type;

            if (IsBoxableType(context, m.DeclaringType))
                return m.DeclaringType;

            return null;
        }

		private static bool IsBoxableType(TranslationContext context, IType type)
		{
			return type != null && !ReferenceEquals(type, context.Method.DeclaringType)
				   && type.IsBoxableType();
		}

		private static IType GetParamType(IParameter p)
        {
            return p.Type.UnwrapRef();
        }

	    private static bool WithSuffix(Instruction currentInstruction)
	    {
		    return !currentInstruction.IsEndOfTryFinally;
	    }

	    #endregion
    }
}