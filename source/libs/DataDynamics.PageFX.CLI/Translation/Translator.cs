using System;
using System.Diagnostics;
using System.Linq;
using DataDynamics.PageFX.CLI.IL;
using DataDynamics.PageFX.CLI.Translation.ControlFlow.Services;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.CodeModel.TypeSystem;

namespace DataDynamics.PageFX.CLI.Translation
{
	/// <summary>
    /// Implements <see cref="ITranslator"/> from CIL.
    /// </summary>
    internal partial class Translator : ITranslator
	{
		private TranslationContext _context;
		private TranslatorResult _result;

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

			_context = new TranslationContext(new Code(method, clrBody, provider), null);

#if PERF
            ++CallCount;
#endif

            _result = TranslateCore(_context);

			return _result.Output.ToArray();
        }

		public void DumpILMap(string format, string filename)
		{
			ILMapDump.Dump(_context.Body, _result, format, filename);
		}

		private TranslatorResult TranslateCore(TranslationContext context)
        {
            if (context.Method.DeclaringType is IGenericType)
                throw new ILTranslatorException("Not supported");

#if DEBUG
            DebugHooks.LogSeparator();
            DebugHooks.LogInfo("ILTranslator started for method: {0}", context.Method);
			if (DebugHooks.CanBreak(context.Method)) Debugger.Break();
#endif

            try
            {
                var output = Process(context);
#if DEBUG
				DebugHooks.LogInfo("ILTranslator succeeded for method: {0}", context.Method);
                DebugHooks.LogSeparator();
#endif
	            return output;
            }
            catch (Exception e)
            {
#if DEBUG
				DebugHooks.SetLastError(context.Method);
#endif
                if (e is CompilerException)
                    throw;
				throw Errors.CILTranslator.UnableToTranslateMethod.CreateInnerException(e, context.FullMethodName);
            }
        }

		private TranslatorResult Process(TranslationContext context)
        {
            context.Body.InstanceCount++;

            if (Checks.IsGenericNestingExceeds(context))
            {
                var code = context.Code.New();
                //_code.AddRange(_provider.ThrowRuntimeError("The max nesting of generic instantiations exceeds"));
                code.AddRange(code.Provider.ThrowTypeLoadException("The max nesting of generic instantiations exceeds"));

	            return new TranslatorResult { Output = code };
            }

			ControlFlowGraph.Build(context.Body);

			PushState(context);

			GenericResolver.Resolve(context.Method, context.Body);

			Analyzer.Analyze(context.Body, context.Provider);

			TranslateGraph(context);

			var result = Emitter.Emit(context, DebugFile);

			Branch.Resolve(result.Branches, context.Provider);

			context.Provider.Finish();

#if DEBUG
			context.Body.VisualizeGraph(context.Body.ControlFlowGraph.Entry, true);
			DumpILMap("I: N V", "ilmap_i.txt");
#endif

			PopState(context);

			return result;
        }

		private static void PushState(TranslationContext context)
        {
			context.Body.ControlFlowGraph.PushState();
            
            foreach (var instr in context.Body.Code)
            {
                instr.PushState();

                var member = instr.Value as ITypeMember;
                if (member != null)
                    instr.Member = member;
            }
        }

		private static void PopState(TranslationContext context)
        {
            if (!IsGenericContext(context)) return;

            foreach (var instr in context.Body.Code)
                instr.PopState();

			context.Body.ControlFlowGraph.PopState();
        }

		private static bool IsGenericContext(TranslationContext context)
		{
			var method = context.Method;
			return method.DeclaringType is IGenericInstance || method.IsGenericInstance;
		}
    }
}