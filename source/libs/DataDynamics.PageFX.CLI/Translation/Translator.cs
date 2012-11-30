using System;
using System.Diagnostics;
using System.Linq;
using DataDynamics.PageFX.CLI.IL;
using DataDynamics.PageFX.CLI.Translation.ControlFlow;
using DataDynamics.PageFX.CLI.Translation.ControlFlow.Services;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.Translation
{
	/// <summary>
    /// Implements <see cref="ITranslator"/> from CIL.
    /// </summary>
    internal partial class Translator : ITranslator
    {
	    private IClrMethodBody _body; //input: body of input method
		private ICodeProvider _provider; //input: code provider
		private IMethod _method; //input method to translate
		private TranslatorResult _result;

		internal ICodeProvider CodeProvider
		{
			get { return _provider; }
		}

#if PERF
        public static int CallCount;
#endif

	    /// <summary>
	    /// Translates given <see cref="IMethodBody"/> using given  <see cref="ICodeProvider"/>.
	    /// </summary>
	    /// <param name="method">The method to translate.</param>
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

#if PERF
            ++CallCount;
#endif

            return TranslateCore();
        }

		public void DumpILMap(string format, string filename)
		{
			ILMapDump.Dump(_body, _result, format, filename);
		}

		private IInstruction[] TranslateCore()
        {
            if (_method.DeclaringType is IGenericType)
                throw new ILTranslatorException("Not supported");

#if DEBUG
            DebugHooks.LogSeparator();
            DebugHooks.LogInfo("ILTranslator started for method: {0}", _method);
            if (DebugHooks.CanBreak(_method)) Debugger.Break();
#endif

            try
            {
                var output = Process();
#if DEBUG
                DebugHooks.LogInfo("ILTranslator succeeded for method: {0}", _method);
                DebugHooks.LogSeparator();
#endif
	            return output;
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
            get { return _method.DeclaringType.FullName + "." + _method.Name; }
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
            var gi = _method.DeclaringType as IGenericInstance;
            if (gi == null) return false;
            int depth = 1;
            CalcGenericNesting(gi, ref depth);
            return depth > MaxGenericNesting;
        }

		private IInstruction[] Process()
        {
            _body.InstanceCount++;

            if (CheckGenericNesting())
            {
                var code = new Code(_provider);
                //_code.AddRange(_provider.ThrowRuntimeError("The max nesting of generic instantiations exceeds"));
                code.AddRange(_provider.ThrowTypeLoadException("The max nesting of generic instantiations exceeds"));

	            _result = new TranslatorResult { Code = code };

	            return code.ToArray();
            }

			ControlFlowGraph.Build(_body);
			PushState();
			GenericResolver.Resolve(_method, _body);
			Analyzer.Analyze(_body, _provider);
			TranslateGraph();
			var result = Emitter.Emit(_body, this);
			Branch.Resolve(result.Branches, _provider);

			_provider.Finish();

#if DEBUG
			_body.VisualizeGraph(_body.ControlFlowGraph.Entry, true);
			DumpILMap("I: N V", "ilmap_i.txt");
#endif

			PopState();

			_result = result;

			return result.Code.ToArray();
        }

		private void PushState()
        {
			_body.ControlFlowGraph.PushState();
            
            foreach (var instr in _body.Code)
            {
                instr.PushState();

                var member = instr.Value as ITypeMember;
                if (member != null)
                    instr.Member = member;
            }
        }

		private void PopState()
        {
            if (!IsGenericInstance) return;

            foreach (var instr in _body.Code)
                instr.PopState();

			_body.ControlFlowGraph.PopState();
        }

		private bool IsGenericInstance
		{
			get 
			{
				if (_method.DeclaringType is IGenericInstance)
					return true;
				if (_method.IsGenericInstance)
					return true;
				return false;
			}
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

		internal void FixSelfCycle(Node bb)
		{
			var il = bb.TranslatedCode;

			var last = il[il.Count - 1];

			//TODO: do this only for endfinally
			//handle self cycle!
			if (last.IsUnconditionalBranch && bb.FirstSuccessor == bb)
			{
				if (_method.IsVoid())
				{
					var label = _provider.Label();
					if (label != null)
					{
						il.Add(label);
					}
					var code = new Code(_provider);
					OpReturn(code, null);
					il.AddRange(code);
				}
				else
				{
					throw new NotImplementedException();
				}
			}
		}
    }
}