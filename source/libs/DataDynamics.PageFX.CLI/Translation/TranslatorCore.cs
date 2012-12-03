using System.Linq;
using DataDynamics.PageFX.CLI.IL;
using DataDynamics.PageFX.CLI.Translation.ControlFlow;
using DataDynamics.PageFX.CLI.Translation.Values;
using DataDynamics.PageFX.Common;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Services;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.CLI.Translation
{
	/// <summary>
	/// Implements translation of code for every basic block in flow graph.
	/// </summary>
    internal sealed class TranslatorCore
    {
		private readonly DebugInfo _debugInfo;
	    private int _bbIndex;
		private bool _popScope;

		public TranslatorCore(DebugInfo debugInfo)
		{
			_debugInfo = debugInfo;
		}
		
	    /// <summary>
        /// Translates all basic blocks in flow graph.
        /// </summary>
		public void Translate(TranslationContext context)
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

            TranslateBeforeBlock(context);

            block.TranslationIndex = -1;

            if (block.IsTranslated) return;

            TranslateBlockCore(context);
        }

	    #region TranslateBeforeBlock

		private void TranslateBeforeBlock(TranslationContext context)
        {
            EnsurePrevHandlerBlock(context);
            TranslateIncomingBlocks(context);
            EnsureSehBlocks(context);
            TypeReconciler.Reconcile(context);
        }

		private void EnsurePrevHandlerBlock(TranslationContext context)
        {
            var hb = GetHandlerBlock(context.Block);
            if (hb == null) return;

            int i = hb.Index;
            if (i == 0) return;

            var prev = (HandlerBlock)hb.Owner.Handlers[i - 1];
            var node = prev.EntryPoint.BasicBlock;
            TranslateBlock(context.New(node));
        }

		private static HandlerBlock GetHandlerBlock(Node block)
        {
            if (block.CodeLength == 0) return null;
            var first = block.Code[0];
            var seh = first.SehBlock;
            if (seh == null) return null;
            if (seh.EntryIndex != first.Index) return null;
            return seh as HandlerBlock;
        }

		private void TranslateIncomingBlocks(TranslationContext context)
		{
			//NOTE:
            //First we must translate predecessors. Why?
            //1. In order to prepare evaluation stack for translated basic block
            //2. Reconciliation of stack types.
            foreach (var e in context.Block.InEdges)
            {
				if (ShouldTranslatePredecessor(e, context.Block))
                    TranslateBlock(context.New(e.From));
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

		private void EnsureSehBlocks(TranslationContext context)
		{
			if (context.Block.SehBegin != null)
				EnsureSehBlock(context, context.Block.SehBegin);

			//For basic blocks that is an end of SEH block
            //we must enshure that basic block for SEH block
            //entry instruction is translated
			if (context.Block.SehEnd != null)
            {
                EnsureInstructionBlock(context, context.Block.SehEnd.EntryIndex);
            }
        }

		private void EnsureSehBlock(TranslationContext context, Block block)
        {
            if (block == null) return;

			if (block.Parent != null)
			{
				EnsureSehBlock(context, block.Parent);
			}

        	var handler = block as HandlerBlock;
            if (handler != null)
            {
            	EnsureEntryPoints(context, handler.Owner);
				EnsureInstructionBlock(context, handler.EntryIndex);
            }
            else
            {
                EnsureEntryPoints(context, block);
            }
        }

		private void EnsureEntryPoints(TranslationContext context, Block seh)
        {
            EnsureInstructionBlock(context, seh.EntryIndex);

			//we should translate begin basic block of SEH clause before exit basic block
        	var tryBlock = seh as TryCatchBlock;
			if (tryBlock != null)
			{
				foreach (var handler in tryBlock.Handlers.Cast<HandlerBlock>())
				{
					EnsureInstructionBlock(context, handler.EntryIndex);
				}
			}
        }

		private void EnsureInstructionBlock(TranslationContext context, int index)
        {
        	var block = GetBasicBlockSafe(context, index);
			if (block != context.Block) // avoid stackoverflow!
			{
				TranslateBlock(context.New(block));
			}
        }

    	private static Node GetBasicBlockSafe(TranslationContext context, int index)
    	{
    		var n = context.Code.GetInstructionBasicBlock(index);
    		if (n == null)
    			throw new ILTranslatorException("Invalid index of SEH block");
    		return n;
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

		private static void LabelBlock(TranslationContext context)
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

			_debugInfo.Inject(context, instruction);
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
                    CopyValue(context, p.GetUnwrappedType());
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
            var ptype = p.GetUnwrappedType();

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

		private static bool WithSuffix(Instruction currentInstruction)
	    {
		    return !currentInstruction.IsEndOfTryFinally;
	    }
    }
}