using System;
using System.Collections.Generic;
using System.Diagnostics;
using DataDynamics.PageFX.CLI.CFG;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.IL
{
    using Code = List<IInstruction>;

    /// <summary>
    /// Implements <see cref="ITranslator"/> from CIL.
    /// </summary>
    internal partial class ILTranslator : ITranslator
    {
        #region enum Phase
        enum Phase
        {
            Analysis,
            Translation
        }
        #endregion

        #region Fields
        MethodBody _body; //input: body of input method
        ICodeProvider _provider; //input: code provider
        List<IInstruction> _outcode; //output instruction set
        IMethod _method; //input method to translate
        IType _declType; //declaring type of input method
        Phase _phase; //current translation phase
        #endregion

        #region Translate - Entry Point
        /// <summary>
        /// Gets the method is being compiled
        /// </summary>
        public IMethod Method
        {
            get { return _method; }
        }

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
            var lb = body as LateMethodBody;
            if (lb != null)
            {
                _body = lb.RealBody;
            }
            else
            {
                _body = body as MethodBody;
                if (_body == null)
                    throw new NotSupportedException("Unsupported body");
            }

            _provider = provider;
            _method = method;
            _declType = _method.DeclaringType;

#if PERF
            ++CallCount;
#endif

            TranslateCore();

            return _outcode.ToArray();
        }

        void TranslateCore()
        {
            if (_declType is IGenericType)
                throw new ILTranslatorException("Not supported");

#if DEBUG
            CLIDebug.LogSeparator();
            CLIDebug.LogInfo("ILTranslator started for method: {0}", _method);
            CLIDebug.EvalFilter(_method);
            if (CLIDebug.IsBreak)
                Debugger.Break();
#endif

            try
            {
                Process();
#if DEBUG
                CLIDebug.LogInfo("ILTranslator succeeded for method: {0}", _method);
                CLIDebug.LogSeparator();
#endif
            }
            catch (Exception e)
            {
#if DEBUG
                CLIDebug.SetLastError(_method);
#endif
                if (e is CompilerException)
                    throw;
                throw Errors.CILTranslator.UnableToTranslateMethod.CreateInnerException(e, FullMethodName);
            }
        }

        string FullMethodName
        {
            get
            {
                return _declType.FullName + "." + _method.Name;
            }
        }
        #endregion

        #region Process
        public const int MaxGenericNesting = 100;

        static void CalcGenericNesting(IGenericInstance gi, ref int depth)
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

        bool CheckGenericNesting()
        {
            var gi = _declType as IGenericInstance;
            if (gi == null) return false;
            int depth = 1;
            CalcGenericNesting(gi, ref depth);
            return depth > MaxGenericNesting;
        }

        void Process()
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
                ConcatBlocks();
                ResolveBranches();
#if DEBUG
                if (CLIDebug.DumpILMap)
                    DumpILMap("I: N V", "ilmap_i.txt");
#endif
                PopState();
            }
        }

        void PushState()
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

        bool IsGenericInstance
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

        void PopState()
        {
            if (!IsGenericInstance) return;
            foreach (var instr in _body.Code)
                instr.PopState();
            foreach (var bb in Blocks)
                bb.PopState();
        }
        #endregion

        #region Utils
        ILStream SourceCode
        {
            get { return _body.Code; }
        }

        Instruction GetInstruction(int index)
        {
            var code = SourceCode;
            if (index < 0 || index >= code.Count)
                throw new ArgumentOutOfRangeException("index");
            return code[index];
        }

        Node GetInstructionBlock(int index)
        {
            var instr = GetInstruction(index);
            return instr.OwnerNode;
        }

        IType ReturnType
        {
            get { return _method.Type; }
        }
        #endregion

        #region STEP1 - Builds Flow Graph
        /// <summary>
        /// Builds flowgraph. Prepares list of basic blocks to translate.
        /// </summary>
        void BuildGraph()
        {
#if DEBUG
            CLIDebug.LogInfo("CFG Builder started");
            CLIDebug.DoCancel();
#endif

            _flowgraph = _body.FlowGraph;
            if (_flowgraph != null)
            {
#if DEBUG
                CLIDebug.DoCancel();
                CLIDebug.VisualizeGraph(_body, _flowgraph.Entry);
#endif
                return;
            }

        	var builder = new GraphBuilder(_body, false) {IsVoidCallEnd = true};
        	var entry = builder.Build();
            if (entry == null)
                throw new ILTranslatorException("Unable to build flowgraph");

#if DEBUG
            CLIDebug.DoCancel();
            CLIDebug.VisualizeGraph(_body, entry);
#endif

            //Prepares list of basic blocks in the same order as they are located in code.
            var blocks = new NodeList();
            Node prevNode = null;
            foreach (var instruction in _body.Code)
            {
#if DEBUG
                CLIDebug.DoCancel();
#endif
                var bb = instruction.OwnerNode;
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
        void ConcatBlocks()
        {
#if DEBUG
            CLIDebug.LogInfo("ConcatBlocks started");
            CLIDebug.DoCancel();
#endif

            _brlist = new Code();
            _brnodes = new List<Node>();

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
                CLIDebug.DoCancel();
#endif
                bb.TranslatedEntryIndex = _outcode.Count;
                var code = bb.TranslatedCode;
                Emit(code);

                int clen = code.Count;
                if (clen > 0)
                {
                    var last = code[clen - 1];
                    if (IsBranchOrSwitch(last))
                    {
                        _brlist.Add(last);
                        _brnodes.Add(bb);
                    }
                }
            }

            Emit(_endCode);

#if DEBUG
            CLIDebug.LogInfo("ConcatBlocks succeeded. CodeSize = {0}", _outcode.Count);
            CLIDebug.DoCancel();
#endif
        }

        /// <summary>
        /// Adds set of instruction to output instruction set
        /// </summary>
        /// <param name="code"></param>
        void Emit(IEnumerable<IInstruction> code)
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

        void DeclareVariables()
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
        Code _brlist;
        List<Node> _brnodes;

        /// <summary>
        /// Resolves branch instructions.
        /// </summary>
        void ResolveBranches()
        {
#if DEBUG
            CLIDebug.LogInfo("ResolveBranches started");
            CLIDebug.DoCancel();
#endif
            int n = _brlist.Count;
            for (int i = 0; i < n; ++i)
            {
#if DEBUG
                CLIDebug.DoCancel();
#endif
                var br = _brlist[i];
                var bb = _brnodes[i];
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
                    //sanity check
                    if (!br.IsUnconditionalBranch)
                        throw new ILTranslatorException("Invalid branch instruction");

                    var e = bb.FirstOut;
                    _provider.SetBranchTarget(br, e.To.TranslatedEntryIndex);
                }
            }
#if DEBUG
            CLIDebug.LogInfo("ResolveBranches succeeded");
            CLIDebug.DoCancel();
#endif
        }
        #endregion
    }
}