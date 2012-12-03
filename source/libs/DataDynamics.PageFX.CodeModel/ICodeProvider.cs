using System;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Common
{
    /// <summary>
    /// Represents back end of compiler.
    /// </summary>
    public interface ICodeProvider
    {
        bool DonotCopyReturnValue { get; set; }

        /// <summary>
        /// Gets or sets <see cref="IInstruction"/> currently translated by <see cref="ITranslator"/>.
        /// </summary>
        IInstruction SourceInstruction { get; set; }

        void BeforeTranslation();

        void AfterTranslation();

        IInstruction Nop();

        #region Begin/End
        /// <summary>
        /// This method is called by <see cref="ITranslator"/> before generation of instruction list for given method body.
        /// </summary>
        /// <returns></returns>
        IInstruction[] Begin();

        /// <summary>
        /// This method is called by <see cref="ITranslator"/> after generation of instruction list for given method body.
        /// </summary>
        /// <returns></returns>
        IInstruction[] End();

    	void Finish();
        #endregion

        int GetVarIndex(int index, bool tobackend);

        /// <summary>
        /// Provides code to declare given variable.
        /// </summary>
        /// <param name="v">variable to declare.</param>
        /// <returns>code to perform given operation</returns>
        IInstruction[] DeclareVariable(IVariable v);

        #region Data Flow
        /// <summary>
        /// Loads given constant onto the evaluation stack.
        /// </summary>
        /// <param name="value">value to load</param>
        /// <returns>code to perform given operation</returns>
        IInstruction[] LoadConstant(object value);

        /// <summary>
        /// Loads value of variable indicated by given index onto the evaluation stack.
        /// </summary>
        /// <param name="v">variable whose value should be loaded onto the stack.</param>
        /// <returns>code to perform given operation</returns>
        IInstruction[] LoadVariable(IVariable v);

        /// <summary>
        /// Loads value of argument indicated by given index onto the evaluation stack.
        /// </summary>
        /// <param name="p">argument whose value should be loaded onto the stack.</param>
        /// <returns>code to perform given operation</returns>
        IInstruction[] LoadArgument(IParameter p);

        /// <summary>
        /// Loads value of given field onto the evaluation stack.
        /// </summary>
        /// <param name="field">the field whose value should be loaded onto the stack.</param>
        /// <returns>code to perform given operation</returns>
        IInstruction[] LoadField(IField field);

        IInstruction[] StoreThis();

        /// <summary>
        /// Returns code to load "this" onto the stack.
        /// </summary>
        /// <returns>code to perform given operation</returns>
        IInstruction[] LoadThis();

        /// <summary>
        /// Returns code to load "base" onto the stack.
        /// </summary>
        /// <returns>code to perform given operation</returns>
        IInstruction[] LoadBase();

        /// <summary>
        /// Returns code to load static instance of given type.
        /// </summary>
        /// <param name="type">type for which the operation will be performed.</param>
        /// <returns>code to perform given operation</returns>
        IInstruction[] LoadStaticInstance(IType type);
        
        /// <summary>
        /// Returns code to store value onto the stack in specified variable indicated by given index.
        /// </summary>
        /// <param name="v">variable where value should be stored.</param>
        /// <returns>code to perform given operation</returns>
        IInstruction[] StoreVariable(IVariable v);

        /// <summary>
        /// Returns code to store value onto the stack in specified argument indicated by given index.
        /// </summary>
        /// <param name="p">argument where value should be stored.</param>
        /// <returns>code to perform given operation</returns>
        IInstruction[] StoreArgument(IParameter p);

        /// <summary>
        /// Returns code to store value onto the stack in specified field.
        /// </summary>
        /// <param name="field">the field where value should be stored.</param>
        /// <returns>code to perform given operation</returns>
        IInstruction[] StoreField(IField field);
        #endregion

        #region Pointers
        bool IsThisAddressed { get; set; }

        /// <summary>
        /// Provides code to load address of this argument.
        /// </summary>
        /// <returns>Array of instructions (code) to perform the operation.</returns>
        IInstruction[] GetThisPtr();

        /// <summary>
        /// Provides code to load address of given variable onto the evaluation stack.
        /// </summary>
        /// <param name="v">local variable to load address.</param>
        /// <returns>Array of instructions (code) to perform the operation.</returns>
        IInstruction[] GetVarPtr(IVariable v);

        /// <summary>
        /// Provides code to load address of given argument onto the evaluation stack.
        /// </summary>
        /// <param name="p">given argument</param>
        /// <returns>Array of instructions (code) to perform the operation.</returns>
        IInstruction[] GetArgPtr(IParameter p);

        /// <summary>
        /// Provides code to load address of given field onto the evaluation stack.
        /// </summary>
        /// <param name="field">field to load address.</param>
        /// <returns>Array of instructions (code) to perform the operation.</returns>
        IInstruction[] GetFieldPtr(IField field);

        /// <summary>
        /// Provides code to load address of an element of an array onto the evaluation stack.
        /// </summary>
        /// <param name="elemType">type of an array element to load address.</param>
        /// <returns>Array of instructions (code) to perform the operation.</returns>
        IInstruction[] GetElemPtr(IType elemType);

        /// <summary>
        /// Provides code to load value indirect onto the stack.
        /// </summary>
        /// <param name="valueType">type of value to load.</param>
        /// <returns>Array of instructions (code) to perform the operation.</returns>
        IInstruction[] LoadIndirect(IType valueType);

        /// <summary>
        /// Provides code to store value indirect from stack.
        /// </summary>
        /// <param name="valueType">type of value to store by address onto the stack.</param>
        /// <returns>Array of instructions (code) to perform the operation.</returns>
        IInstruction[] StoreIndirect(IType valueType);
        #endregion

        #region Temp Variables
        /// <summary>
        /// Determines whether back end supports 
        /// </summary>
        bool SupportStaticTarget { get; }

        /// <summary>
        /// Saves value onto the stack in temporary variable
        /// </summary>
        /// <param name="var">index of temp variable</param>
        /// <param name="keepStackState">true to keep stack state</param>
        /// <returns>code to perform given operation</returns>
        IInstruction[] SetTempVar(out int var, bool keepStackState);

        /// <summary>
        /// Loads value from temporary variable onto the stack
        /// </summary>
        /// <param name="var">index of temp variable whose value will be loaded onto the stack.</param>
        /// <returns>code to perform given operation</returns>
        IInstruction[] GetTempVar(int var);

        /// <summary>
        /// Free temporary variable
        /// </summary>
        /// <param name="var">index of temp variable to kill.</param>
        /// <returns>code to perform given operation</returns>
        IInstruction[] KillTempVar(int var);
        #endregion

        #region Control Flow
        /// <summary>
        /// Determines whether specified branch operator is supported by back end.
        /// </summary>
        /// <param name="op"></param>
        /// <returns></returns>
        bool SupportBranchOperator(BranchOperator op);

        /// <summary>
        /// Performs conditional branch.
        /// </summary>
        /// <param name="op"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>instruction to perform specified branch.</returns>
        IInstruction[] Branch(BranchOperator op, IType left, IType right);

        /// <summary>
        /// Performs unconditional branch.
        /// </summary>
        /// <returns></returns>
        IInstruction Branch();

        /// <summary>
        /// Performs unconditional branch to specified instruction indicated by given index.
        /// </summary>
        /// <param name="index">index of destination instruction</param>
        /// <returns></returns>
        IInstruction Branch(int index);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="br"></param>
        /// <param name="index"></param>
        void SetBranchTarget(IInstruction br, int index);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isvoid"></param>
        /// <returns>code to perform given operation</returns>
        IInstruction[] Return(bool isvoid);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        bool IsLabel(IInstruction i);

        IInstruction Label();
        #endregion

        #region Switch
        /// <summary>
        /// 
        /// </summary>
        bool IsSwitchSupported { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="caseCount"></param>
        /// <returns></returns>
        IInstruction Switch(int caseCount);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="targets"></param>
        /// <param name="defaultTarget"></param>
        void SetCaseTargets(IInstruction sw, int[] targets, int defaultTarget);
        #endregion

        #region Exception Handling
        /// <summary>
        /// Gets or sets flag indicating whether to pop exception from stack in exception handler blocks.
        /// </summary>
        bool PopException { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>code to perform given operation</returns>
        IInstruction[] BeginTry();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="generateExit"></param>
        /// <param name="jump"></param>
        /// <returns>code to perform given operation</returns>
        IInstruction[] EndTry(bool generateExit, out IInstruction jump);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handlerBlock"></param>
        /// <returns></returns>
		IInstruction[] BeginCatch(ISehHandlerBlock handlerBlock);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isLast"></param>
        /// <param name="generateExit"></param>
        /// <param name="jump"></param>
        /// <returns>code to perform given operation</returns>
		IInstruction[] EndCatch(ISehHandlerBlock block, bool isLast, bool generateExit, out IInstruction jump);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="block"></param>
        /// <returns>code to perform given operation</returns>
		IInstruction[] BeginFinally(ISehHandlerBlock block);

        /// <summary>
        /// 
        /// </summary>
        /// <returns>code to perform given operation</returns>
		IInstruction[] EndFinally(ISehHandlerBlock block);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="block"></param>
        /// <returns></returns>
        IInstruction[] BeginFault(ISehHandlerBlock block);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		IInstruction[] EndFault(ISehHandlerBlock block);
        
        /// <summary>
        /// Returns code to throw value onto the stack.
        /// </summary>
        /// <returns>code to perform given operation</returns>
        IInstruction[] Throw();

        /// <summary>
        /// Returns code to rethrow exception catched within current exception handler block.
        /// </summary>
        /// <returns>code to perform given operation</returns>
		IInstruction[] Rethrow(ISehBlock block);

        IInstruction[] ThrowRuntimeError(string message);

        IInstruction[] ThrowTypeLoadException(string message);
        #endregion

        #region Computing
        /// <summary>
        /// 
        /// </summary>
        /// <param name="op"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="result"></param>
        /// <param name="checkOverflow"></param>
        /// <returns>code to perform given operation</returns>
        IInstruction[] Op(BinaryOperator op, IType left, IType right, IType result, bool checkOverflow);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="op"></param>
        /// <param name="type"></param>
        /// <param name="checkOverflow"></param>
        /// <returns>code to perform given operation</returns>
        IInstruction[] Op(UnaryOperator op, IType type, bool checkOverflow);

        /// <summary>
        /// 
        /// </summary>
        bool SupportIncrementOperators { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns>code to perform given operation</returns>
        IInstruction Increment(IType type);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns>code to perform given operation</returns>
        IInstruction Decrement(IType type);
        #endregion

        #region Calling
        /// <summary>
        /// Called before invokation of given method.
        /// </summary>
        /// <param name="method"></param>
        /// <returns>code to perform given operation</returns>
        IInstruction[] BeginCall(IMethod method);

        /// <summary>
        /// Called after invokation of given method.
        /// </summary>
        /// <param name="method"></param>
        /// <returns>code to perform given operation</returns>
        IInstruction[] EndCall(IMethod method);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <param name="newobj"></param>
        /// <returns>code to perform given operation</returns>
        IInstruction[] LoadReceiver(IMethod method, bool newobj);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="receiverType"></param>
        /// <param name="method"></param>
        /// <param name="flags"></param>
        /// <returns>code to perform given operation</returns>
        IInstruction[] CallMethod(IType receiverType, IMethod method, CallFlags flags);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns>code to perform given operation</returns>
        IInstruction[] InitObject(IType type);

        /// <summary>
        /// Copies value of given value type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns>code to perform given operation</returns>
        IInstruction[] CopyValue(IType type);
        #endregion

        #region Delegates
        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <returns>code to perform given operation</returns>
        IInstruction[] LoadFunction(IMethod method);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <returns>code to perform given operation</returns>
        IInstruction[] InvokeDelegate(IMethod method);
        #endregion

        #region Cast Operations
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns>code to perform given operation</returns>
        IInstruction[] As(IType type);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source">source type</param>
        /// <param name="target">target type</param>
        /// <param name="checkOverflow"></param>
        /// <returns>code to perform given operation</returns>
        IInstruction[] Cast(IType source, IType target, bool checkOverflow);

        IInstruction[] BoxPrimitive(IType type);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns>code to perform given operation</returns>
        IInstruction[] Box(IType type);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns>code to perform given operation</returns>
        IInstruction[] Unbox(IType type);
        #endregion

        #region Stack Operations
        /// <summary>
        /// Diplicates value onto the stack.
        /// </summary>
        /// <returns>instruction that performs duplicate operation</returns>
        IInstruction Dup();

        /// <summary>
        /// Swaps two values onto the stack.
        /// </summary>
        /// <returns>instruction that performs swap operation.</returns>
        IInstruction Swap();

        IInstruction Pop();
        #endregion

        #region Arrays
        /// <summary>
        /// 
        /// </summary>
        /// <param name="elemType"></param>
        /// <returns>code to perform given operation</returns>
        IInstruction[] NewArray(IType elemType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elemType"></param>
        /// <returns>code to perform given operation</returns>
        IInstruction[] SetArrayElem(IType elemType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elemType"></param>
        /// <returns>code to perform given operation</returns>
        IInstruction[] GetArrayElem(IType elemType);

        /// <summary>
        /// 
        /// </summary>
        /// <returns>code to perform given operation</returns>
        IInstruction[] GetArrayLength();
        #endregion

        #region Debug Support
        /// <summary>
        /// Enters to debugger.
        /// </summary>
        /// <returns></returns>
        IInstruction[] DebuggerBreak();
        
        /// <summary>
        /// Sets filename of current method body.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        IInstruction[] DebugFile(string file);

        /// <summary>
        /// Indicates the current line number the debugger should be using for the code currently executing.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        IInstruction[] DebugLine(int line);

        /// <summary>
        /// Gets or sets first line of method.
        /// </summary>
        int DebugFirstLine { get; set; }
        #endregion

        #region Reflection Support
        /// <summary>
        /// Provides IL code to perform typeof operation for given type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IInstruction[] TypeOf(IType type);

        /// <summary>
        /// Provides IL code to perform sizeof operation for given type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IInstruction[] SizeOf(IType type);
        #endregion

        /// <summary>
        /// Determines whether given instructions are the same and can be removed when they are subsequent.
        /// </summary>
        /// <param name="a">First instruction under test.</param>
        /// <param name="b">Second instruction under test.</param>
        /// <returns></returns>
        bool IsDuplicate(IInstruction a, IInstruction b);

	    bool HasCopy(IType type);

        IInstruction[] CopyToThis(IType valueType);

        bool MustPreventBoxing(IMethod method, IParameter arg);

        /// <summary>
        /// Performs local optimization.
        /// </summary>
        /// <param name="code">instruction set to optimize</param>
        /// <returns>optimized instruction set</returns>
        IInstruction[] OptimizeBasicBlock(IInstruction[] code);

        void CompileMethod(IMethod method);
    }

    [Flags]
    public enum CallFlags
    {
        None = 0,
        Thiscall = 1,
        Virtcall = 2,
        Newobj = 4,
        Basecall = 8,
    }
}