using System.Reflection.Emit;
using DataDynamics.PageFX.CLI.IL;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.JavaScript
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

		public IInstruction[] Begin()
		{
			return Empty;
		}

		public IInstruction[] End()
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

		public IInstruction[] DeclareVariable(IVariable v)
		{
			return NopArray;
		}

		public IInstruction[] LoadConstant(object value)
		{
			return NopArray;
		}

		public IInstruction[] LoadVariable(IVariable v)
		{
			return NopArray;
		}

		public IInstruction[] LoadArgument(IParameter p)
		{
			return NopArray;
		}

		public IInstruction[] LoadField(IField field)
		{
			return NopArray;
		}

		public IInstruction[] StoreThis()
		{
			return NopArray;
		}

		public IInstruction[] LoadThis()
		{
			return NopArray;
		}

		public IInstruction[] LoadBase()
		{
			return NopArray;
		}

		public IInstruction[] LoadStaticInstance(IType type)
		{
			return NopArray;
		}

		public IInstruction[] StoreVariable(IVariable v)
		{
			return NopArray;
		}

		public IInstruction[] StoreArgument(IParameter p)
		{
			return NopArray;
		}

		public IInstruction[] StoreField(IField field)
		{
			return NopArray;
		}

		public bool IsThisAddressed { get; set; }

		public IInstruction[] GetThisPtr()
		{
			return NopArray;
		}

		public IInstruction[] GetVarPtr(IVariable v)
		{
			return NopArray;
		}

		public IInstruction[] GetArgPtr(IParameter p)
		{
			return NopArray;
		}

		public IInstruction[] GetFieldPtr(IField field)
		{
			return NopArray;
		}

		public IInstruction[] GetElemPtr(IType elemType)
		{
			return NopArray;
		}

		public IInstruction[] LoadIndirect(IType valueType)
		{
			return NopArray;
		}

		public IInstruction[] StoreIndirect(IType valueType)
		{
			return NopArray;
		}

		public bool SupportStaticTarget
		{
			get { return false; }
		}

		public IInstruction[] SetTempVar(out int var, bool keepStackState)
		{
			var = 100;
			return NopArray;
		}

		public IInstruction[] GetTempVar(int var)
		{
			return NopArray;
		}

		public IInstruction[] KillTempVar(int var)
		{
			return NopArray;
		}

		public bool SupportBranchOperator(BranchOperator op)
		{
			return true;
		}

		public IInstruction[] Branch(BranchOperator op, IType left, IType right)
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

		public IInstruction[] Return(bool isvoid)
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

		public IInstruction[] BeginTry()
		{
			return NopArray;
		}

		public IInstruction[] EndTry(bool generateExit, out IInstruction jump)
		{
			jump = OpNop;
			return NopArray;
		}

		public IInstruction[] BeginCatch(ISehHandlerBlock handlerBlock)
		{
			return NopArray;
		}

		public IInstruction[] EndCatch(ISehHandlerBlock block, bool isLast, bool generateExit, out IInstruction jump)
		{
			jump = OpNop;
			return NopArray;
		}

		public IInstruction[] BeginFinally(ISehHandlerBlock block)
		{
			return NopArray;
		}

		public IInstruction[] EndFinally(ISehHandlerBlock block)
		{
			return NopArray;
		}

		public IInstruction[] BeginFault(ISehHandlerBlock block)
		{
			return NopArray;
		}

		public IInstruction[] EndFault(ISehHandlerBlock block)
		{
			return NopArray;
		}

		public IInstruction[] Throw()
		{
			return NopArray;
		}

		public IInstruction[] Rethrow(ISehBlock block)
		{
			return NopArray;
		}

		public IInstruction[] ThrowRuntimeError(string message)
		{
			return NopArray;
		}

		public IInstruction[] ThrowTypeLoadException(string message)
		{
			return NopArray;
		}

		public IInstruction[] Op(BinaryOperator op, IType left, IType right, IType result, bool checkOverflow)
		{
			return NopArray;
		}

		public IInstruction[] Op(UnaryOperator op, IType type, bool checkOverflow)
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

		public IInstruction[] BeginCall(IMethod method)
		{
			return NopArray;
		}

		public IInstruction[] EndCall(IMethod method)
		{
			return NopArray;
		}

		public IInstruction[] LoadReceiver(IMethod method, bool newobj)
		{
			return NopArray;
		}

		public IInstruction[] CallMethod(IType receiverType, IMethod method, CallFlags flags)
		{
			return NopArray;
		}

		public IInstruction[] InitObject(IType type)
		{
			return NopArray;
		}

		public IInstruction[] CopyValue(IType type)
		{
			return NopArray;
		}

		public IInstruction[] LoadFunction(IMethod method)
		{
			return NopArray;
		}

		public IInstruction[] InvokeDelegate(IMethod method)
		{
			return NopArray;
		}

		public IInstruction[] As(IType type)
		{
			return NopArray;
		}

		public IInstruction[] Cast(IType source, IType target, bool checkOverflow)
		{
			return NopArray;
		}

		public IInstruction[] BoxPrimitive(IType type)
		{
			return NopArray;
		}

		public IInstruction[] Box(IType type)
		{
			return NopArray;
		}

		public IInstruction[] Unbox(IType type)
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

		public IInstruction[] NewArray(IType elemType)
		{
			return NopArray;
		}

		public IInstruction[] SetArrayElem(IType elemType)
		{
			return NopArray;
		}

		public IInstruction[] GetArrayElem(IType elemType)
		{
			return NopArray;
		}

		public IInstruction[] GetArrayLength()
		{
			return NopArray;
		}

		public IInstruction[] DebuggerBreak()
		{
			return NopArray;
		}

		public IInstruction[] DebugFile(string file)
		{
			return NopArray;
		}

		public IInstruction[] DebugLine(int line)
		{
			return NopArray;
		}

		public int DebugFirstLine { get; set; }

		public IInstruction[] TypeOf(IType type)
		{
			return NopArray;
		}

		public IInstruction[] SizeOf(IType type)
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

		public IInstruction[] CopyToThis(IType valueType)
		{
			return NopArray;
		}

		public bool MustPreventBoxing(IMethod method, IParameter arg)
		{
			return false;
		}

		public IInstruction[] OptimizeBasicBlock(IInstruction[] code)
		{
			return code;
		}

		public void CompileMethod(IMethod method)
		{
		}
	}
}