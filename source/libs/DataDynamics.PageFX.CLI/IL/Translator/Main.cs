using System;
using System.Collections.Generic;
using System.Diagnostics;
using DataDynamics.PageFX.CLI.ControlFlow;
using DataDynamics.PageFX.CLI.ControlFlow.Services;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.IL
{
    using Code = List<IInstruction>;

    /// <summary>
    /// Implements <see cref="ITranslator"/> from CIL.
    /// </summary>
    internal partial class ILTranslator : ITranslator
    {
	    private enum Phase { Analysis, Translation }

	    private IClrMethodBody _body; //input: body of input method
		private ICodeProvider _provider; //input: code provider
		private List<IInstruction> _outcode; //output instruction set
		private IMethod _method; //input method to translate
		private IType _declType; //declaring type of input method
		private Phase _phase; //current translation phase

#if PERF
        public static int CallCount;
#endif

        /// <summary>
        /// Translates given <see cref="IMethodBody"/> using given  <see cref="ICodeProvider"/>.
        /// </summary>
        /// <param name="body">body to translate.</param>
        /// <param name="provider">code provider to use.</param>
        /// <returns>translated code.</returns>
        public IInstruction[] Translate(IMethod method, IMethodBody body, ICodeProvider provider)
        {
            var clrBody = body as IClrMethodBody;
            if (clrBody == null)
            {
				throw new NotSupportedException("Unsupported body format");
            }

        	_body = clrBody;
            
            _provider = provider;
            _method = method;
            _declType = _method.DeclaringType;

#if PERF
            ++CallCount;
#endif

            TranslateCore();

            return _outcode.ToArray();
        }

        private void TranslateCore()
        {
            if (_declType is IGenericType)
                throw new ILTranslatorException("Not supported");

#if DEBUG
            DebugHooks.LogSeparator();
            DebugHooks.LogInfo("ILTranslator started for method: {0}", _method);
            if (DebugHooks.CanBreak(_method)) Debugger.Break();
#endif

            try
            {
                Process();
#if DEBUG
                DebugHooks.LogInfo("ILTranslator succeeded for method: {0}", _method);
                DebugHooks.LogSeparator();
#endif
            }
            catch (Exception e)
            {
#if DEBUG
                DebugHooks.SetLastError(_method);
#endif
                if (e is CompilerException)
                    throw;
                throw Errors.CILTranslator.UnableToTranslateMethod.CreateInnerException(e, FullMethodName);
            }
        }

        private string FullMethodName
        {
            get { return _declType.FullName + "." + _method.Name; }
        }

	    public const int MaxGenericNesting = 100;

		private static void CalcGenericNesting(IGenericInstance gi, ref int depth)
        {
            foreach (var type in gi.GenericArguments)
            {
                var gi2 = type as IGenericInstance;
                if (gi2 != null)
                {
                    ++depth;
                    CalcGenericNesting(gi2, ref depth);
                }
            }
        }

        private bool CheckGenericNesting()
        {
            var gi = _declType as IGenericInstance;
            if (gi == null) return false;
            int depth = 1;
            CalcGenericNesting(gi, ref depth);
            return depth > MaxGenericNesting;
        }

		private void Process()
        {
            _body.InstanceCount++;

            if (CheckGenericNesting())
            {
                _outcode = new List<IInstruction>();
                //_code.AddRange(_provider.ThrowRuntimeError("The max nesting of generic instantiations exceeds"));
                _outcode.AddRange(_provider.ThrowTypeLoadException("The max nesting of generic instantiations exceeds"));
            }
            else
            {
            	BuildGraph();
            	PushState();
            	ResolveGenerics();
            	AnalyzeGraph();
            	TranslateGraph();
            	var branches = ConcatBlocks();
            	ResolveBranches(branches);

				_provider.Finish();

#if DEBUG
            	_body.VisualizeGraph(_flowgraph.Entry, true);
            	DumpILMap("I: N V", "ilmap_i.txt");
#endif

            	PopState();
            }
        }

		private void PushState()
        {
            foreach (var bb in Blocks)
                bb.PushState();

            foreach (var instr in _body.Code)
            {
                instr.PushState();

                var member = instr.Value as ITypeMember;
                if (member != null)
                    instr.Member = member;
            }
        }

		private bool IsGenericInstance
        {
            get 
            {
                if (_declType is IGenericInstance)
                    return true;
                if (_method.IsGenericInstance)
                    return true;
                return false;
            }
        }

		private void PopState()
        {
            if (!IsGenericInstance) return;
            foreach (var instr in _body.Code)
                instr.PopState();
            foreach (var bb in Blocks)
                bb.PopState();
        }

	    private Instruction GetInstruction(int index)
        {
            var code = _body.Code;
            if (index < 0 || index >= code.Count)
                throw new ArgumentOutOfRangeException("index");
            return code[index];
        }

		private Node GetInstructionBasicBlock(int index)
        {
            var instr = GetInstruction(index);
            return instr.BasicBlock;
        }

		private IType ReturnType
        {
            get { return _method.Type; }
        }

	    #region STEP1 - Builds Flow Graph
        /// <summary>
        /// Builds flowgraph. Prepares list of basic blocks to translate.
        /// </summary>
        private void BuildGraph()
        {
#if DEBUG
            DebugHooks.LogInfo("CFG Builder started");
            DebugHooks.DoCancel();
#endif

            _flowgraph = _body.FlowGraph;
            if (_flowgraph != null)
            {
#if DEBUG
                DebugHooks.DoCancel();
                _body.VisualizeGraph(_flowgraph.Entry, false);
#endif
                return;
            }

        	var builder = new GraphBuilder(_body, false) {IsVoidCallEnd = true};
        	var entry = builder.Build();
            if (entry == null)
                throw new ILTranslatorException("Unable to build flowgraph");

#if DEBUG
            DebugHooks.DoCancel();
            _body.VisualizeGraph(entry, false);
#endif

            //Prepares list of basic blocks in the same order as they are located in code.
            var blocks = new NodeList();
            Node prevNode = null;
            foreach (var instruction in _body.Code)
            {
#if DEBUG
                DebugHooks.DoCancel();
#endif
                var bb = instruction.BasicBlock;
                //if (bb == null)
                //    throw new InvalidOperationException();
                if (bb == null) continue;
                if (prevNode == bb) continue;
                blocks.Add(bb);
                prevNode = bb;
            }

            _flowgraph = new FlowGraph {Entry = entry, Blocks = blocks};
            _body.FlowGraph = _flowgraph;
        }
        #endregion

        #region STEP4 - Concatenation of translated code blocks
        /// <summary>
        /// Concatenates code of all basic blocks in order of basic blocks layout.
        /// Prepares output code (instruction list).
        /// </summary>
        private IList<KeyValuePair<IInstruction, Node>> ConcatBlocks()
        {
#if DEBUG
            DebugHooks.LogInfo("ConcatBlocks started");
            DebugHooks.DoCancel();
#endif

            var branches = new List<KeyValuePair<IInstruction, Node>>();

            _outcode = new List<IInstruction>();

            _beginCode = _provider.Begin();
            _endCode = _provider.End();

            if (_debugFile != null)
            {
                var il = _provider.DebugFile(_debugFile);
                Emit(il);
            }

            Emit(_beginCode);
            DeclareVariables();

#if DEBUG
            //Used in DumpILMap
            _beginSize = _outcode.Count;
#endif

            foreach (var bb in Blocks)
            {
#if DEBUG
                DebugHooks.DoCancel();
#endif
				//UNCOMMENT TO CHECK STACK BALANCE
				//CheckStackBalance(bb);

                bb.TranslatedEntryIndex = _outcode.Count;
                var il = bb.TranslatedCode;

				var last = il[il.Count - 1];
				if (last.IsBranchOrSwitch())
				{
					branches.Add(new KeyValuePair<IInstruction, Node>(last, bb));
				}

				//TODO: do this only for endfinally
				//self cycle!
				if (last.IsUnconditionalBranch && bb.FirstSuccessor == bb)
				{
					var label = _provider.Label();
					if (label != null)
					{
						il.Add(label);
					}
					il.AddRange(Op_Return());
				}

                Emit(il);				

				bb.TranslatedExitIndex = _outcode.Count - 1;
            }

            Emit(_endCode);

#if DEBUG
            DebugHooks.LogInfo("ConcatBlocks succeeded. CodeSize = {0}", _outcode.Count);
            DebugHooks.DoCancel();
#endif
        	return branches;
        }

        /// <summary>
        /// Adds set of instruction to output instruction set
        /// </summary>
        /// <param name="code"></param>
        private void Emit(IEnumerable<IInstruction> code)
        {
            if (code != null)
            {
                int i = _outcode.Count;
                foreach (var instruction in code)
                {
                    instruction.Index = i;
                    _outcode.Add(instruction);
                    ++i;
                }
            }
        }

        private void DeclareVariables()
        {
            if (_body.LocalVariables != null)
            {
                int n = _body.LocalVariables.Count;
                for (int i = 0; i < n; ++i)
                {
                    var var = _body.LocalVariables[i];
                    var code = _provider.DeclareVariable(var);
                    Emit(code);
                }
            }
        }
        #endregion

        #region STEP5 - Resolving of branch targets
        /// <summary>
        /// Resolves branch instructions.
        /// </summary>
        private void ResolveBranches(IList<KeyValuePair<IInstruction,Node>> branches)
        {
#if DEBUG
            DebugHooks.LogInfo("ResolveBranches started");
            DebugHooks.DoCancel();
#endif
            int n = branches.Count;
            for (int i = 0; i < n; ++i)
            {
#if DEBUG
                DebugHooks.DoCancel();
#endif
				var br = branches[i].Key;
				var bb = branches[i].Value;
                if (br.IsSwitch)
                {
                    var e = bb.FirstOut;
                    int deftarget = e.To.TranslatedEntryIndex;
                    var targets = new List<int>();
                    for (e = e.NextOut; e != null; e = e.NextOut)
                    {
                        targets.Add(e.To.TranslatedEntryIndex);
                    }
                    _provider.SetCaseTargets(br, targets.ToArray(), deftarget);
                }
                else if (br.IsConditionalBranch)
                {
                    var e = bb.TrueEdge;
                    _provider.SetBranchTarget(br, e.To.TranslatedEntryIndex);
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
						_provider.SetBranchTarget(br, e.To.TranslatedEntryIndex);
					}
					else
					{
						_provider.SetBranchTarget(br, bb.TranslatedExitIndex - 1);
					}
                }
            }
#if DEBUG
            DebugHooks.LogInfo("ResolveBranches succeeded");
            DebugHooks.DoCancel();
#endif
        }
        #endregion
    }
}