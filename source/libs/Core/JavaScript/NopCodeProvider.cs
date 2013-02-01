using System.Collections.Generic;
using System.Reflection.Emit;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Services;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Core.IL;

namespace DataDynamics.PageFX.Core.JavaScript
{
	internal class NopCodeProvider : ICodeProvider
	{
		private static readonly IInstruction OpNop = new Instruction { OpCode = OpCodes.Nop };
		private static readonly IInstruction[] NopArray = new[] { OpNop };
		private static readonly IInstruction[] Empty = new IInstruction[0];

		private readonly JsCompiler _host;
		private readonly JsClass _klass;
		private readonly IMethod _method;

		public NopCodeProvider(JsCompiler host, JsClass klass, IMethod method)
		{
			_host = host;
			_klass = klass;
			_method = method;
		}

		public JsFunction Function { get; private set; }

		public bool DonotCopyReturnValue { get; set; }

		public IInstruction SourceInstruction { get; set; }

		public void BeforeTranslation()
		{
		}

		public void AfterTranslation()
		{
		}

		public IInstruction Nop()
		{
			return OpNop;
		}

		public IEnumerable<IInstruction> Begin()
		{
			return Empty;
		}

		public IEnumerable<IInstruction> End()
		{
			return Empty;
		}

		public void Finish()
		{
			Function = _host.CompileMethodBody(_klass, _method, (IClrMethodBody)_method.Body);
		}

		public int GetVarIndex(int index, bool tobackend)
		{
			return index;
		}

		public IEnumerable<IInstruction> DeclareVariable(IVariable v)
		{
			return NopArray;
		}

		public IEnumerable<IInstruction> LoadConstant(object value)
		{
			return NopArray;
		}

		public IEnumerable<IInstruction> LoadVariable(IVariable v)
		{
			return NopArray;
		}

		public IEnumerable<IInstruction> LoadArgument(IParameter p)
		{
			return NopArray;
		}

		public IEnumerable<IInstruction> LoadField(IField field)
		{
			return NopArray;
		}

		public IEnumerable<IInstruction> StoreThis()
		{
			return NopArray;
		}

		public IEnumerable<IInstruction> LoadThis()
		{
			return NopArray;
		}

		public IEnumerable<IInstruction> LoadBase()
		{
			return NopArray;
		}

		public IEnumerable<IInstruction> LoadStaticInstance(IType type)
		{
			return NopArray;
		}

		public IEnumerable<IInstruction> StoreVariable(IVariable v)
		{
			return NopArray;
		}

		public IEnumerable<IInstruction> StoreArgument(IParameter p)
		{
			return NopArray;
		}

		public IEnumerable<IInstruction> StoreField(IField field)
		{
			return NopArray;
		}

		public bool IsThisAddressed { get; set; }

		public IEnumerable<IInstruction> GetThisPtr()
		{
			return NopArray;
		}

		public IEnumerable<IInstruction> GetVarPtr(IVariable v)
		{
			return NopArray;
		}

		public IEnumerable<IInstruction> GetArgPtr(IParameter p)
		{
			return NopArray;
		}

		public IEnumerable<IInstruction> GetFieldPtr(IField field)
		{
			return NopArray;
		}

		public IEnumerable<IInstruction> GetElemPtr(IType elemType)
		{
			return NopArray;
		}

		public IEnumerable<IInstruction> LoadIndirect(IType valueType)
		{
			return NopArray;
		}

		public IEnumerable<IInstruction> StoreIndirect(IType valueType)
		{
			return NopArray;
		}

		public bool SupportStaticTarget
		{
			get { return false; }
		}

		public IEnumerable<IInstruction> SetTempVar(out int var, bool keepStackState)
		{
			var = 100;
			return NopArray;
		}

		public IEnumerable<IInstruction> GetTempVar(int var)
		{
			return NopArray;
		}

		public IEnumerable<IInstruction> KillTempVar(int var)
		{
			return NopArray;
		}

		public bool SupportBranchOperator(BranchOperator op)
		{
			return true;
		}

		public IEnumerable<IInstruction> Branch(BranchOperator op, IType left, IType right)
		{
			return NopArray;
		}

		public IInstruction Branch()
		{
			return OpNop;
		}

		public IInstruction Branch(int index)
		{
			return OpNop;
		}

		public void SetBranchTarget(IInstruction br, int index)
		{
		}

		public IEnumerable<IInstruction> Return(bool isvoid)
		{
			return NopArray;
		}

		public bool IsLabel(IInstruction i)
		{
			return false;
		}

		public IInstruction Label()
		{
			return OpNop;
		}

		public bool IsSwitchSupported
		{
			get { return true; }
		}

		public IInstruction Switch(int caseCount)
		{
			return OpNop;
		}

		public void SetCaseTargets(IInstruction sw, int[] targets, int defaultTarget)
		{
		}

		public bool PopException { get; set; }

		public IEnumerable<IInstruction> BeginTry()
		{
			return NopArray;
		}

		public IEnumerable<IInstruction> EndTry(bool generateExit, out IInstruction jump)
		{
			jump = OpNop;
			return NopArray;
		}

		public IEnumerable<IInstruction> BeginCatch(ISehHandlerBlock handlerBlock)
		{
			return NopArray;
		}

		public IEnumerable<IInstruction> EndCatch(ISehHandlerBlock block, bool isLast, bool generateExit, out IInstruction jump)
		{
			jump = OpNop;
			return NopArray;
		}

		public IEnumerable<IInstruction> BeginFinally(ISehHandlerBlock block)
		{
			return NopArray;
		}

		public IEnumerable<IInstruction> EndFinally(ISehHandlerBlock block)
		{
			return NopArray;
		}

		public IEnumerable<IInstruction> BeginFault(ISehHandlerBlock block)
		{
			return NopArray;
		}

		public IEnumerable<IInstruction> EndFault(ISehHandlerBlock block)
		{
			return NopArray;
		}

		public IEnumerable<IInstruction> Throw()
		{
			return NopArray;
		}

		public IEnumerable<IInstruction> Rethrow(ISehBlock block)
		{
			return NopArray;
		}

		public IEnumerable<IInstruction> ThrowTypeLoadException(string message)
		{
			return NopArray;
		}

		public IEnumerable<IInstruction> Op(BinaryOperator op, IType left, IType right, IType result, bool checkOverflow)
		{
			return NopArray;
		}

		public IEnumerable<IInstruction> Op(UnaryOperator op, IType type, bool checkOverflow)
		{
			return NopArray;
		}

		public bool SupportIncrementOperators
		{
			get { return false; }
		}

		public IInstruction Increment(IType type)
		{
			return OpNop;
		}

		public IInstruction Decrement(IType type)
		{
			return OpNop;
		}

		public IEnumerable<IInstruction> BeginCall(IMethod method)
		{
			return NopArray;
		}

		public IEnumerable<IInstruction> EndCall(IMethod method)
		{
			return NopArray;
		}

		public IEnumerable<IInstruction> LoadReceiver(IMethod method, bool newobj)
		{
			return NopArray;
		}

		public IEnumerable<IInstruction> CallMethod(IType receiverType, IMethod method, CallFlags flags)
		{
			return NopArray;
		}

		public IEnumerable<IInstruction> InitObject(IType type)
		{
			return NopArray;
		}

		public IEnumerable<IInstruction> CopyValue(IType type)
		{
			return NopArray;
		}

		public IEnumerable<IInstruction> LoadFunction(IMethod method)
		{
			return NopArray;
		}

		public IEnumerable<IInstruction> InvokeDelegate(IMethod method)
		{
			return NopArray;
		}

		public IEnumerable<IInstruction> As(IType type)
		{
			return NopArray;
		}

		public IEnumerable<IInstruction> Cast(IType source, IType target, bool checkOverflow)
		{
			return NopArray;
		}

		public IEnumerable<IInstruction> BoxPrimitive(IType type)
		{
			return NopArray;
		}

		public IEnumerable<IInstruction> Box(IType type)
		{
			return NopArray;
		}

		public IEnumerable<IInstruction> Unbox(IType type)
		{
			return NopArray;
		}

		public IInstruction Dup()
		{
			return OpNop;
		}

		public IInstruction Swap()
		{
			return OpNop;
		}

		public IInstruction Pop()
		{
			return OpNop;
		}

		public IEnumerable<IInstruction> NewArray(IType elemType)
		{
			return NopArray;
		}

		public IEnumerable<IInstruction> SetArrayElem(IType elemType)
		{
			return NopArray;
		}

		public IEnumerable<IInstruction> GetArrayElem(IType elemType)
		{
			return NopArray;
		}

		public IEnumerable<IInstruction> GetArrayLength()
		{
			return NopArray;
		}

		public IEnumerable<IInstruction> DebuggerBreak()
		{
			return NopArray;
		}

		public IEnumerable<IInstruction> DebugFile(string file)
		{
			return NopArray;
		}

		public IEnumerable<IInstruction> DebugLine(int line)
		{
			return NopArray;
		}

		public int DebugFirstLine { get; set; }

		public IEnumerable<IInstruction> TypeOf(IType type)
		{
			return NopArray;
		}

		public IEnumerable<IInstruction> SizeOf(IType type)
		{
			return NopArray;
		}

		public bool IsDuplicate(IInstruction a, IInstruction b)
		{
			return false;
		}

		public bool HasCopy(IType type)
		{
			return true;
		}

		public IEnumerable<IInstruction> CopyToThis(IType valueType)
		{
			return NopArray;
		}

		public bool MustPreventBoxing(IMethod method, IParameter arg)
		{
			return false;
		}

		public IEnumerable<IInstruction> OptimizeBasicBlock(IInstruction[] code)
		{
			return code;
		}

		public void CompileMethod(IMethod method)
		{
		}
	}
}