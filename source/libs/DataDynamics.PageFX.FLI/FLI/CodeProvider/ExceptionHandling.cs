using System;
using System.Collections.Generic;
using System.Diagnostics;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.FLI.ABC;
using DataDynamics.PageFX.FLI.IL;

namespace DataDynamics.PageFX.FLI
{
    internal partial class AvmCodeProvider
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

        public bool PopCatchScope
        {
            get { return _popCatchScope; }
            set { _popCatchScope = value; }
        }
        private bool _popCatchScope;
        #endregion

        #region BeginTry, EndTry
        public IInstruction[] BeginTry()
        {
            var code = new AbcCode(_abc);
            return code.ToArray();
        }

        public IInstruction[] EndTry(bool generateExit, out IInstruction jump)
        {
            var code = new AbcCode(_abc);
            if (generateExit)
            {
                jump = code.Goto(); //exit from protected region
            }
            else
            {
                jump = null;
            }
            return code.ToArray();
        }
        #endregion

        #region BeginCatch, EndCatch
        class CatchInfo
        {
            public int ExceptionVar;
            public bool IsTempVar; //exception var is temp
            public bool IsVarKilled;
        }
        readonly Stack<CatchInfo> _catchStack = new Stack<CatchInfo>();

        int SharedExceptionVar
        {
            get
            {
                if (_sharedExceptionVar < 0)
                    _sharedExceptionVar = NewTempVar(true);
                return _sharedExceptionVar;
            }
        }
        int _sharedExceptionVar = -1;

        void BeginCatch(AbcCode code, AbcExceptionHandler e, ref int var, bool dupException, bool catchAnyException)
        {
            Instruction begin = null;
            Instruction instr;

            var ci = new CatchInfo();
            if (var < 0 || catchAnyException)
            {
                var = SharedExceptionVar;
                ci.IsTempVar = true;
            }
            ci.ExceptionVar = var;
            _catchStack.Push(ci);

            //NOTE: we store exception in temp variable to use for rethrow operation
            if (dupException && !catchAnyException)
            {
                instr = code.Dup();
                begin = instr;
            }

            //store exception in variable to use for rethrow operation
            if (catchAnyException)
            {
                instr = code.CoerceAnyType();
                if (begin == null) begin = instr;
            }

            instr = code.SetLocal(var);
            if (begin == null) begin = instr;

            _resolver.Add(begin, new ExceptionTarget(e));
        }

        static bool IsVesException(IType type)
        {
            if (type == null) return false;
            if (type.Namespace == SystemTypes.Namespace)
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

        static bool MustCatchAnyException(ISehHandlerBlock h)
        {
            var ph = h.PrevHandler;
            if (ph != null)
            {
                if (ph.Tag is SehHandlerTag)
                    return true;
            }

            var tb = h.Owner;
            foreach (var handler in tb.Handlers)
            {
                if (IsVesException(handler.ExceptionType))
                    return true;
            }

            return false;
        }

        public IInstruction[] BeginCatch(ISehHandlerBlock h)
        {
            var tb = h.Owner;
            if (tb.EntryPoint == null)
                throw new ArgumentNullException();
            if (tb.ExitPoint == null)
                throw new ArgumentNullException();

            var exceptionType = h.ExceptionType;
            if (exceptionType != null)
                EnsureType(exceptionType);

            var seh = new AbcExceptionHandler();
            _body.Exceptions.Add(seh);
            _resolver.Add(tb.EntryPoint, new ExceptionFrom(seh));
            _resolver.Add(tb.ExitPoint, new ExceptionTo(seh));

            bool catchAnyException = MustCatchAnyException(h);
            seh.Type = catchAnyException ? _abc.BuiltinTypes.Object : TypeHelper.GetTypeMultiname(h.ExceptionType);

            int var = h.ExceptionVariable;
            if (var >= 0)
                var = GetVarIndex(var);

            var code = new AbcCode(_abc);
            BeginCatch(code, seh, ref var, !_popException, catchAnyException);

            if (catchAnyException)
            {
                RouteException(code, h, var);
                _sehsToResolve.Add(tb);
            }

            return code.ToArray();
        }

        public IInstruction[] EndCatch(bool isLast, bool generateExit, out IInstruction jump)
        {
            var ci = _catchStack.Pop();

            jump = null;
            var code = new AbcCode(_abc);
            if (_popCatchScope)
                code.PopScope(); //pops catch scope

            //we now no need in exception variable
            KillExceptionVariable(code, ci);

            //NOTE: no need to generate exit jump for last catch block
            if (generateExit && !isLast)
                jump = code.Goto();

            return code.ToArray();
        }

        void RouteException(AbcCode code, ISehHandlerBlock h, int var)
        {
            var exceptionType = h.ExceptionType;

            if (h.PrevHandler == null)
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

            var tag = new SehHandlerTag();
            h.Tag = tag;

            tag.CheckExceptionLabel = code.Label();
            //NOTE: Exception on stack can be routed from previous handlers
            code.SetLocal(var);
            code.GetLocal(var);
            code.As(exceptionType, true);
            code.PushNull();
            var ifMyException = code.IfNotEquals();

            //Routing to another exception handler or rethrow
            //Instruction routing = Label();
            if (h.NextHandler == null)
            {
                code.GetLocal(var);
                code.Throw();
            }
            else
            {
                code.GetLocal(var);
                tag.JumpToNextHandler = code.Goto();
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
            var avmErrors = Corlib.FindType("AvmErrors");
            EnsureType(avmErrors);

            var fromError = MethodHelper.Find(avmErrors, "ExceptionFromError", 1);
            var m = DefineAbcMethod(fromError);

            code.Getlex(m);
            code.GetLocal(var);
            code.Call(m);
        }
        #endregion

        #region Exception Subjects
        class ExceptionFrom : IInstructionSubject
        {
            public ExceptionFrom(AbcExceptionHandler e)
            {
                _e = e;
            }
            readonly AbcExceptionHandler _e;

            #region IInstructionSubject Members
            public void Apply(IInstruction instruction)
            {
                _e.From = instruction.Index;
            }
            #endregion
        }

        class ExceptionTo : IInstructionSubject
        {
            public ExceptionTo(AbcExceptionHandler e)
            {
                _e = e;
            }
            readonly AbcExceptionHandler _e;

            #region IInstructionSubject Members
            public void Apply(IInstruction instruction)
            {
                _e.To = instruction.Index;
            }
            #endregion
        }

        class ExceptionTarget : IInstructionSubject
        {
            public ExceptionTarget(AbcExceptionHandler e)
            {
                _e = e;
            }
            readonly AbcExceptionHandler _e;

            #region IInstructionSubject Members
            public void Apply(IInstruction instruction)
            {
                _e.Target = instruction.Index;
            }
            #endregion
        }
        #endregion

        #region BeginFinally, EndFinally, BeginFault, EndFault
        class FinallyInfo
        {
            public int varRethrowFlag;
            public int varException = -1;
            public bool fault;
        }
        readonly Stack<FinallyInfo> _finallyStack = new Stack<FinallyInfo>();

        IInstruction[] BeginFinally(ISehTryBlock tb, bool fault)
        {
            var e = new AbcExceptionHandler();
            _body.Exceptions.Add(e);
            e.To = -1;
            e.Target = -1;

            var fi = new FinallyInfo();
            fi.fault = fault;
            _finallyStack.Push(fi);

            _resolver.Add(tb.EntryPoint, new ExceptionFrom(e));

            var code = new AbcCode(_abc);

            if (!fault)
            {
                //Reset rethrow flag
                fi.varRethrowFlag = NewTempVar(true);
                code.Add(InstructionCode.Pushfalse);
                code.SetLocal(fi.varRethrowFlag);
            }

            //Add goto finally body
            var gotoBody = code.Goto();

            _resolver.Add(gotoBody, new ExceptionTo(e));

            //NOTE: Insert empty handler to catch unhandled or rethrown exception
            //begin catch
            BeginCatch(code, e, ref fi.varException, false, false);

            var end = (Instruction)code[code.Count - 1];

            if (!fault)
            {
                //Set rethrow flag to true to rethrow exception
                code.Add(InstructionCode.Pushtrue);
                end = code.SetLocal(fi.varRethrowFlag);
            }

            if (_popCatchScope)
            {
                end = code.PopScope(); //pops catch scope
            }
            //end of catch

            gotoBody.GotoNext(end);
            
            return code.ToArray();
        }

        IInstruction[] EndFinally(bool fault)
        {
            var ci = _catchStack.Pop();
            var fi = _finallyStack.Pop();
            if (fi.fault != fault)
                throw new InvalidOperationException();

            var code = new AbcCode(_abc);

            if (fault)
            {
                code.GetLocal(ci.ExceptionVar);
                KillExceptionVariable(code, ci);
                code.Throw();
            }
            else
            {
                code.GetLocal(fi.varRethrowFlag);
                KillTempVar(code, fi.varRethrowFlag);
                var br = code.IfFalse();

                code.GetLocal(ci.ExceptionVar);
                KillExceptionVariable(code, ci);

                var end = code.Throw();
                
                br.GotoNext(end);
            }

            return code.ToArray();
        }

        public IInstruction[] BeginFinally(ISehTryBlock tb)
        {
            return BeginFinally(tb, false);
        }

        public IInstruction[] EndFinally()
        {
            return EndFinally(false);
        }

        public IInstruction[] BeginFault(ISehTryBlock tb)
        {
            return BeginFinally(tb, true);
        }

        public IInstruction[] EndFault()
        {
            return EndFinally(true);
        }
        #endregion

        #region Throw, Rethrow
        bool ExceptionBreakEnabled
        {
            get 
            {
                if (!IsSwf) return false;
                var opts = _generator.sfc.Options;
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

        public IInstruction[] Rethrow()
        {
            var ci = _catchStack.Peek();
            var code = new AbcCode(_abc);
            code.GetLocal(ci.ExceptionVar);
            KillExceptionVariable(code, ci);
            code.Throw();
            return code.ToArray();
        }

        public IInstruction[] ThrowRuntimeError(string message)
        {
            var code = new AbcCode(_abc);
            InsertExceptionBreak(code);
            code.ThrowException(Corlib.Types.ExecutionEngineException, message);
            return code.ToArray();
        }

        public IInstruction[] ThrowTypeLoadException(string message)
        {
            var code = new AbcCode(_abc);
            InsertExceptionBreak(code);
            code.ThrowException(Corlib.Types.TypeLoadException, message);
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

        #region ResolveExceptionHandlers
        void ResolveExceptionHandlers()
        {
            foreach (var tb in _sehsToResolve)
            {
                var h = Algorithms.Find(
                    tb.Handlers,
                    seh => seh.Tag is SehHandlerTag);

                Debug.Assert(h != null);

                while (true)
                {
                    var tag = h.Tag as SehHandlerTag;
                    Debug.Assert(tag != null);

                    var next = h.NextHandler;
                    if (next != null)
                    {
                        var nextTag = next.Tag as SehHandlerTag;
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

    internal class SehHandlerTag
    {
        public Instruction CheckExceptionLabel;
        public Instruction JumpToNextHandler;
    }
}