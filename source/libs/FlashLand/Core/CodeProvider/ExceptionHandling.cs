using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Services;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.Abc;
using DataDynamics.PageFX.FlashLand.Avm;
using DataDynamics.PageFX.FlashLand.IL;

namespace DataDynamics.PageFX.FlashLand.Core.CodeProvider
{
    internal partial class CodeProviderImpl
    {
        #region Config Properties

        /// <summary>
        /// Gets or sets flag indicating whether to pop exception from stack in exception handler blocks.
        /// </summary>
        public bool PopException
        {
            get { return _popException; }
            set { _popException = value; }
        }
        private bool _popException = true;

    	public bool PopCatchScope { get; set; }

    	#endregion

        #region BeginTry, EndTry

        public IInstruction[] BeginTry()
        {
            return new IInstruction[0];
        }

        public IInstruction[] EndTry(bool generateExit, out IInstruction jump)
        {
            var code = new AbcCode(_abc);
        	jump = generateExit
					? code.Goto() //exit from protected region
        	       	: null;
            return code.ToArray();
        }

        #endregion

        #region BeginCatch, EndCatch

        private int SharedExceptionVar
        {
            get
            {
                if (_sharedExceptionVar < 0)
                    _sharedExceptionVar = NewTempVar(true);
                return _sharedExceptionVar;
            }
        }
        int _sharedExceptionVar = -1;

		void BeginCatch(ISehHandlerBlock block, AbcCode code, AbcExceptionHandler e, ref int var, bool dupException, bool catchAnyException)
		{
			var beginIndex = code.Count;
            
			var catchInfo = new CatchInfo
			                	{
			                		CatchAnyException = catchAnyException
			                	};
			bool coerceAny = catchAnyException;
            if (var < 0 || catchAnyException)
            {
            	coerceAny = true;
                var = SharedExceptionVar;
                catchInfo.IsTempVar = true;
            }
            catchInfo.ExceptionVar = var;
			e.LocalVariable = var;

        	var handlerInfo = block.Tag as SehHandlerInfo;
			if (handlerInfo == null)
			{
				handlerInfo = new SehHandlerInfo { CatchInfo = catchInfo };
				block.Tag = handlerInfo;
			}
			else
			{
				handlerInfo.CatchInfo = catchInfo;
			}
        	
            //NOTE: we store exception in temp variable to use for rethrow operation
            if (dupException && !catchAnyException)
            {
                code.Dup();
            }

			if (coerceAny)
			{
				code.CoerceAnyType();
			}

			//store exception in variable to use for rethrow operation
			code.SetLocal(var);

			_resolver.Add(code[beginIndex], new ExceptionTarget(e));
        }

        static bool IsVesException(IType type)
        {
            if (type == null) return false;
            if (type.Namespace == SystemType.Namespace)
            {
                switch (type.Name)
                {
                    case "Object":
                    case "Exception":
                    case "NullReferenceException":
                    case "InvalidCastException":
                    //case "IndexOutOfRangeException":
                        return true;
                }
            }
            return false;
        }

        static bool MustCatchAnyException(ISehHandlerBlock block)
        {
        	var prev = block.PrevHandler;
			if (prev != null)
			{
				var info = prev.Tag as SehHandlerInfo;
				if (info != null)
				{
					return info.CatchInfo.CatchAnyException;
				}
			}

        	return block.Owner.Handlers.Any(handler => IsVesException(handler.ExceptionType));
        }

        public IInstruction[] BeginCatch(ISehHandlerBlock handlerBlock)
        {
            var tryBlock = handlerBlock.Owner;
            
            var exceptionType = handlerBlock.ExceptionType;
            if (exceptionType != null)
                EnsureType(exceptionType);

            var seh = new AbcExceptionHandler();
            _body.Exceptions.Add(seh);
			_resolver.Add(tryBlock, new ExceptionFrom(seh), new ExceptionTo(seh));
            
            bool catchAnyException = MustCatchAnyException(handlerBlock);
            seh.Type = catchAnyException ? _abc.BuiltinTypes.Object : handlerBlock.ExceptionType.GetMultiname();

            int var = handlerBlock.ExceptionVariable;
			if (var >= 0)
			{
				var = GetVarIndex(var);
			}

        	var code = new AbcCode(_abc);
            BeginCatch(handlerBlock, code, seh, ref var, !_popException, catchAnyException);

            if (catchAnyException)
            {
                RouteException(code, handlerBlock, var);
                _sehsToResolve.Add(tryBlock);
            }

            return code.ToArray();
        }

		public IInstruction[] EndCatch(ISehHandlerBlock handlerBlock, bool isLast, bool generateExit, out IInstruction jump)
		{
			var ci = handlerBlock.GetCatchInfo();
			
            jump = null;
            var code = new AbcCode(_abc);
            if (PopCatchScope)
                code.PopScope(); //pops catch scope

            //we now no need in exception variable
            KillExceptionVariable(code, ci);

            //NOTE: no need to generate exit jump for last catch block
            if (generateExit && !isLast)
                jump = code.Goto();

            return code.ToArray();
        }

        void RouteException(AbcCode code, ISehHandlerBlock block, int var)
        {
			var exceptionType = block.ExceptionType;

            if (block.PrevHandler == null)
            {
                //if err is AVM error then we translate it to System.Exception.
                //code.GetLocal(var);
                //code.As(AvmTypeCode.Error);
                //code.PushNull();
                //var ifNotError = code.IfEquals();

                code.GetLocal(var);
                code.As(SystemTypes.Exception, true);
                code.PushNull();
                var ifExc = code.IfNotEquals();

                code.GetLocal(var);
                code.As(AvmTypeCode.Error);
                code.PushNull();
                var ifNotError = code.IfEquals();

                CallToException(code, var);
                code.CoerceAnyType();
                code.SetLocal(var);

                //check my exception
                var labelNotError = code.Label();
                ifExc.BranchTarget = labelNotError;
                ifNotError.BranchTarget = labelNotError;
            }

            code.GetLocal(var);

            var handlerInfo = (SehHandlerInfo)block.Tag;
            
            handlerInfo.CheckExceptionLabel = code.Label();
            //NOTE: Exception on stack can be routed from previous handlers
            code.SetLocal(var);
            code.GetLocal(var);
            code.As(exceptionType, true);
            code.PushNull();
            var ifMyException = code.IfNotEquals();

            //Routing to another exception handler or rethrow
            //Instruction routing = Label();
            if (block.NextHandler == null)
            {
                code.GetLocal(var);
                code.Throw();
            }
            else
            {
                code.GetLocal(var);
                handlerInfo.JumpToNextHandler = code.Goto();
            }

            //Normal Execution: Prepare stack for handler
            var normal = code.Label();
            ifMyException.BranchTarget = normal;

            code.GetLocal(var);
            code.Coerce(exceptionType, true);

            //21 instructions for first handler
            //11 instructions for other handlers
        }

        void CallToException(AbcCode code, int var)
        {
            var avmErrors = Assembly.Corlib().FindType("AvmErrors");
			if (avmErrors == null)
				throw new InvalidOperationException(string.Format("Unable to find AvmErrors. Invalid corlib."));

            EnsureType(avmErrors);

            var fromError = avmErrors.Methods.Find("ExceptionFromError", 1);
            var m = DefineAbcMethod(fromError);

            code.Getlex(m);
            code.GetLocal(var);
            code.Call(m);
        }
        #endregion

        #region BeginFinally, EndFinally, BeginFault, EndFault

		IInstruction[] BeginFinally(ISehHandlerBlock block, bool fault)
        {
            var e = new AbcExceptionHandler();
            _body.Exceptions.Add(e);
            e.To = -1;
            e.Target = -1;

        	var fi = new FinallyInfo {IsFault = fault};
        	block.Tag = new SehHandlerInfo {FinallyInfo = fi};

			_resolver.Add(block.Owner, new ExceptionFrom(e), null);

            var code = new AbcCode(_abc);

            if (!fault)
            {
                //Reset rethrow flag
                fi.RethrowFlagVariable = NewTempVar(true);
				_initializableTempVars.Add(new TempVar(fi.RethrowFlagVariable, new Instruction(InstructionCode.Pushfalse)));
                code.PushNativeBool(false);
				// Trying to fix IVDiffGramTest. Coercing to any type does not help. The test failed with error:
				// The Dark Side clouds everything. Impossible to see, the future is. (c) Yoda
	            // code.CoerceAnyType();
                code.SetLocal(fi.RethrowFlagVariable);
            }

            //Add goto finally body
            var gotoBody = code.Goto();

            _resolver.Add(gotoBody, new ExceptionTo(e));

            //NOTE: Insert empty handler to catch unhandled or rethrown exception
            //begin catch
            BeginCatch(block, code, e, ref fi.ExceptionVariable, false, false);

            var end = (Instruction)code[code.Count - 1];

            if (!fault)
            {
                //Set rethrow flag to true to rethrow exception
                code.PushNativeBool(true);
				// Trying to fix IVDiffGramTest. Coercing to any type does not help. The test failed with error:
				// The Dark Side clouds everything. Impossible to see, the future is. (c) Yoda
	            // code.CoerceAnyType();
                end = code.SetLocal(fi.RethrowFlagVariable);
            }

            if (PopCatchScope)
            {
                end = code.PopScope(); //pops catch scope
            }
            //end of catch

            gotoBody.GotoNext(end);
            
            return code.ToArray();
        }

		IInstruction[] EndFinally(ISehHandlerBlock block, bool fault)
        {
        	var handlerInfo = block.GetHandlerInfo();
        	var ci = handlerInfo.CatchInfo;
        	var fi = handlerInfo.FinallyInfo;
            if (fi.IsFault != fault)
                throw new InvalidOperationException("Finally block type mistmatch!");

            var code = new AbcCode(_abc);

            if (fault)
            {
                code.GetLocal(ci.ExceptionVar);
                KillExceptionVariable(code, ci);
                code.Throw();
            }
            else
            {
				// check if we should rethrow exception
                code.GetLocal(fi.RethrowFlagVariable);
				// trying to fix IVDiffGramTest
                // KillTempVar(code, fi.RethrowFlagVariable);
                var br = code.IfFalse();

                code.GetLocal(ci.ExceptionVar);
                KillExceptionVariable(code, ci);
                var end = code.Throw();
                
                br.GotoNext(end);
            }

            return code.ToArray();
        }

		public IInstruction[] BeginFinally(ISehHandlerBlock block)
        {
            return BeginFinally(block, false);
        }

		public IInstruction[] EndFinally(ISehHandlerBlock block)
        {
            return EndFinally(block, false);
        }

		public IInstruction[] BeginFault(ISehHandlerBlock block)
        {
            return BeginFinally(block, true);
        }

		public IInstruction[] EndFault(ISehHandlerBlock block)
        {
            return EndFinally(block, true);
        }
        #endregion

        #region Throw, Rethrow

        bool ExceptionBreakEnabled
        {
            get 
            {
                if (!IsSwf) return false;
                var opts = _generator.SwfCompiler.Options;
                if (opts.IgnoreExceptionBreaks) return false;
                if (opts.ExceptionBreak) return true;
                return PfxConfig.Compiler.ExceptionBreak;
            }
        }

        void InsertExceptionBreak(AbcCode code)
        {
            if (IsEmitDebugInfo && ExceptionBreakEnabled)
            {
                code.DebuggerBreak();
            }
        }

        public IInstruction[] Throw()
        {
            var code = new AbcCode(_abc);
            InsertExceptionBreak(code);
            code.Throw();
            return code.ToArray();
        }

		public IInstruction[] Rethrow(ISehBlock block)
        {
			var ci = block.GetCatchInfo();
            var code = new AbcCode(_abc);
            code.GetLocal(ci.ExceptionVar);
            KillExceptionVariable(code, ci);
            code.Throw();
            return code.ToArray();
        }

	    public IInstruction[] ThrowTypeLoadException(string message)
        {
            var code = new AbcCode(_abc);
            InsertExceptionBreak(code);
			var exceptionType = _generator.Corlib.GetType(CorlibTypeId.TypeLoadException);
            code.ThrowException(exceptionType, message);
            return code.ToArray();
        }

        #endregion

        #region Utils

        void KillExceptionVariable(AbcCode code, CatchInfo ci)
        {
            if (!ci.IsVarKilled)
            {
                ci.IsVarKilled = true;
                if (ci.IsTempVar)
                {
                    code.Add(KillTempVarCore(ci.ExceptionVar));
                }
                else
                {
                    //NOTE: Fix to avoid VerifyError when types can not be reconciled.
                    //code.PushUndefined();
                    //code.SetLocal(ci.ExceptionVar);
                    //code.Add(InstructionCode.Kill, ci.varExc);
                }
            }
        }

        #endregion

		void FinishExceptionHandlers()
		{
			ResolveExceptionHandlers();

			_body.Exceptions.Sort();
		}

        #region ResolveExceptionHandlers
        void ResolveExceptionHandlers()
        {
            foreach (var tb in _sehsToResolve)
            {
                var h = tb.Handlers.FirstOrDefault(seh => seh.Tag is SehHandlerInfo);

                Debug.Assert(h != null);

                while (true)
                {
                    var tag = h.Tag as SehHandlerInfo;
                    Debug.Assert(tag != null);

                    var next = h.NextHandler;
                    if (next != null)
                    {
                        var nextTag = next.Tag as SehHandlerInfo;
                        Debug.Assert(nextTag != null);
                        Debug.Assert(nextTag.CheckExceptionLabel != null);
                        tag.JumpToNextHandler.BranchTarget = nextTag.CheckExceptionLabel;

                        h = next;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        private readonly List<ISehTryBlock> _sehsToResolve = new List<ISehTryBlock>();
        #endregion
    }

	internal sealed class SehHandlerInfo
	{
		public Instruction CheckExceptionLabel;
        public Instruction JumpToNextHandler;

    	public CatchInfo CatchInfo;
    	public FinallyInfo FinallyInfo;
    }

	internal sealed class CatchInfo
	{
		public bool CatchAnyException;
		public int ExceptionVar;
		public bool IsTempVar; //exception var is temp
		public bool IsVarKilled;
	}

	internal sealed class FinallyInfo
	{
		public int RethrowFlagVariable;
		public int ExceptionVariable = -1;
		public bool IsFault;
	}

	internal static class SehBlockExtensions
	{
		public static SehHandlerInfo GetHandlerInfo(this ISehBlock block)
		{
			var handlerInfo = block.Tag as SehHandlerInfo;
			if (handlerInfo == null)
				throw new InvalidOperationException("Handler info is not set yet!");

			return handlerInfo;
		}

		public static CatchInfo GetCatchInfo(this ISehBlock block)
		{
			var handlerInfo = GetHandlerInfo(block);

			var info = handlerInfo.CatchInfo;
			if (info == null)
				throw new InvalidOperationException("Catch info is not set.");

			return info;
		}
	}

	#region Exception Subjects

	internal sealed class ExceptionFrom : IInstructionSubject
	{
		private readonly AbcExceptionHandler _e;

		public ExceptionFrom(AbcExceptionHandler e)
		{
			_e = e;
		}
		
		public void Apply(IInstruction instruction)
		{
			_e.From = instruction.Index;
		}
	}

	internal sealed class ExceptionTo : IInstructionSubject
	{
		private readonly AbcExceptionHandler _e;

		public ExceptionTo(AbcExceptionHandler e)
		{
			_e = e;
		}
		
		public void Apply(IInstruction instruction)
		{
			_e.To = instruction.Index;
		}
	}

	internal sealed class ExceptionTarget : IInstructionSubject
	{
		private readonly AbcExceptionHandler _e;

		public ExceptionTarget(AbcExceptionHandler e)
		{
			_e = e;
		}

		public void Apply(IInstruction instruction)
		{
			_e.Target = instruction.Index;
		}
	}

	#endregion
}