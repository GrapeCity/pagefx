using System;
using System.Collections.Generic;
using DataDynamics.PageFX.CLI.IL;
using DataDynamics.PageFX.CLI.Translation.ControlFlow;
using DataDynamics.PageFX.Common;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.CLI.Translation
{
	internal sealed class Code : List<IInstruction>
	{
		public readonly IMethod Method;
		public readonly IClrMethodBody Body;
		public readonly ICodeProvider Provider;

		public Code(IMethod method, IClrMethodBody body, ICodeProvider provider)
		{
			if (method == null) throw new ArgumentNullException("method");
			if (body == null) throw new ArgumentNullException("body");
			if (provider == null) throw new ArgumentNullException("provider");

			Method = method;
			Body = body;
			Provider = provider;
		}

		public new void AddRange(IEnumerable<IInstruction> set)
		{
			if (set == null) return;

			if (ReferenceEquals(set, this))
				throw new InvalidOperationException();

			base.AddRange(set);
		}

		public Instruction GetInstruction(int index)
		{
			var code = Body.Code;
			if (index < 0 || index >= code.Count)
				throw new ArgumentOutOfRangeException("index");
			return code[index];
		}

		public Node GetInstructionBasicBlock(int index)
		{
			return GetInstruction(index).BasicBlock;
		}

		public Code New()
		{
			return new Code(Method, Body, Provider);
		}
	}

	internal static class CodeExtensions
	{
		/// <summary>
		/// Apends instructions set to specified code block.
		/// </summary>
		/// <param name="code">The code to append to.</param>
		/// <param name="set">The instruction set to append.</param>
		/// <remarks>This method also set index for every instruction in the set.</remarks>		
		public static Code Emit(this Code code, IEnumerable<IInstruction> set)
		{
			if (set == null) return code;

			int index = code.Count;
			foreach (var instruction in set)
			{
				instruction.Index = index;
				code.Add(instruction);
				++index;
			}

			return code;
		}

		public static Code Nop(this Code code)
		{
			code.Add(code.Provider.Nop());
			return code;
		}

		public static Code Op(this Code code, UnaryOperator op, IType valueType, bool checkOverflow)
		{
			code.AddRange(code.Provider.Op(op, valueType, checkOverflow));
			return code;
		}

		public static Code Op(this Code code, BinaryOperator op, IType left, IType right, IType result, bool checkOverflow)
		{
			var il = code.Provider.Op(op, left, right, result, checkOverflow);
			if (il == null || il.Length == 0)
				throw new ILTranslatorException("No context for given binary operation");

			code.AddRange(il);
			return code;
		}

		public static Code Pop(this Code code)
		{
			code.Add(code.Provider.Pop());
			return code;
		}

		public static Code Dup(this Code code)
		{
			code.Add(code.Provider.Dup());
			return code;
		}

		public static Code Swap(this Code code)
		{
			var i = code.Provider.Swap();
			if (i == null)
				throw new NotSupportedException("Swap instruction is not supported");
			code.Add(i);
			return code;
		}

		public static Code SwapIf(this Code code, bool condition)
		{
			return condition ? code.Swap() : code;
		}

		public static Code Return(this Code code, bool isvoid)
		{
			code.AddRange(code.Provider.Return(isvoid));
			return code;
		}

		public static Code As(this Code code, IType type)
		{
			code.AddRange(code.Provider.As(type));
			return code;
		}

		public static Code Cast(this Code code, IType source, IType target, bool checkOverflow)
		{
			code.AddRange(code.Provider.Cast(source, target, checkOverflow));
			return code;
		}

		public static Code Cast(this Code code, IType source, IType target)
		{
			if (ReferenceEquals(target, source))
				return code;

			code.AddRange(code.Provider.Cast(source, target, false));

			return code;
		}

		public static Code TypeOf(this Code code, IType type)
		{
			code.AddRange(code.Provider.TypeOf(type));
			return code;
		}

		public static Code LoadConstant(this Code code, object value)
		{
			code.AddRange(code.Provider.LoadConstant(value));
			return code;
		}

		public static Code LoadThis(this Code code)
		{
			code.AddRange(code.Provider.LoadThis());
			return code;
		}

		public static Code GetThisPtr(this Code code)
		{
			code.AddRange(code.Provider.GetThisPtr());
			return code;
		}

		public static Code StoreThis(this Code code)
		{
			code.AddRange(code.Provider.StoreThis());
			return code;
		}

		public static Code LoadArgument(this Code code, IParameter arg)
		{
			code.AddRange(code.Provider.LoadArgument(arg));
			return code;
		}

		public static Code GetArgPtr(this Code code, IParameter arg)
		{
			code.AddRange(code.Provider.GetArgPtr(arg));
			return code;
		}

		public static Code StoreArgument(this Code code, IParameter arg)
		{
			code.AddRange(code.Provider.StoreArgument(arg));
			return code;
		}

		public static Code LoadVariable(this Code code, IVariable var)
		{
			code.AddRange(code.Provider.LoadVariable(var));
			return code;
		}

		public static Code GetVarPtr(this Code code, IVariable var)
		{
			code.AddRange(code.Provider.GetVarPtr(var));
			return code;
		}

		public static Code StoreVariable(this Code code, IVariable var)
		{
			code.AddRange(code.Provider.StoreVariable(var));
			return code;
		}

		public static Code LoadField(this Code code, IField field)
		{
			code.AddRange(code.Provider.LoadField(field));
			return code;
		}

		public static Code GetFieldPtr(this Code code, IField field)
		{
			code.AddRange(code.Provider.GetFieldPtr(field));
			return code;
		}

		public static Code StoreField(this Code code, IField field)
		{
			code.AddRange(code.Provider.StoreField(field));
			return code;
		}

		public static Code NewArray(this Code code, IType elemType)
		{
			code.AddRange(code.Provider.NewArray(elemType));
			return code;
		}

		public static Code GetArrayLength(this Code code)
		{
			code.AddRange(code.Provider.GetArrayLength());
			return code;
		}

		public static Code GetArrayElem(this Code code, IType elemType)
		{
			code.AddRange(code.Provider.GetArrayElem(elemType));
			return code;
		}

		public static Code GetElemPtr(this Code code, IType elemType)
		{
			code.AddRange(code.Provider.GetElemPtr(elemType));
			return code;
		}

		public static Code SetArrayElem(this Code code, IType elemType)
		{
			code.AddRange(code.Provider.SetArrayElem(elemType));
			return code;
		}

		public static Code LoadIndirect(this Code code, IType type)
		{
			code.AddRange(code.Provider.LoadIndirect(type));
			return code;
		}

		public static Code StoreIndirect(this Code code, IType type)
		{
			code.AddRange(code.Provider.StoreIndirect(type));
			return code;
		}

		public static Code LoadStaticInstance(this Code code, IType type)
		{
			code.AddRange(code.Provider.LoadStaticInstance(type));
			return code;
		}

		public static Code LoadFunction(this Code code, IMethod method)
		{
			code.AddRange(code.Provider.LoadFunction(method));
			return code;
		}

		public static Code CopyValue(this Code code, IType type)
		{
			code.AddRange(code.Provider.CopyValue(type));
			return code;
		}

		public static Code CopyToThis(this Code code, IType type)
		{
			if (code.Provider.HasCopy(type))
			{
				var il = code.Provider.CopyToThis(type);
				code.AddRange(il);
			}
			return code;
		}

		public static Code Box(this Code code, IType type)
		{
			code.AddRange(code.Provider.Box(type));
			return code;
		}

		public static Code BoxPrimitive(this Code code, IType type)
		{
			code.AddRange(code.Provider.BoxPrimitive(type));
			return code;
		}

		public static Code Unbox(this Code code, IType type)
		{
			code.AddRange(code.Provider.Unbox(type));
			return code;
		}

		public static bool LoadReceiver(this Code code, IMethod method, bool newobj)
		{
			var il = code.Provider.LoadReceiver(method, newobj);

			if (il != null && il.Length > 0)
			{
				code.AddRange(il);
				return true;
			}

			return false;
		}

		public static Code CallMethod(this Code code, IType receiverType, IMethod method, CallFlags flags)
		{
			code.AddRange(code.Provider.CallMethod(receiverType, method, flags));
			return code;
		}

		public static Code BeginCall(this Code code, IMethod method)
		{
			code.AddRange(code.Provider.BeginCall(method));
			return code;
		}

		public static Code EndCall(this Code code, IMethod method)
		{
			code.AddRange(code.Provider.EndCall(method));
			return code;
		}

		public static Code InitObject(this Code code, IType type)
		{
			code.AddRange(code.Provider.InitObject(type));
			return code;
		}

		#region Exception Handling

		public static Code Throw(this Code code)
		{
			code.AddRange(code.Provider.Throw());

			//NOTE: Super fix for throw immediatly problem
			//TODO: Check when nop is not needed, i.e. check end of protected region.
			code.Nop();

			return code;
		}

		public static Code Rethrow(this Code code, Instruction currentInstruction)
		{
			code.AddRange(code.Provider.Rethrow(currentInstruction.SehBlock));
			return code;
		}

		public static Code BeginTry(this Code code)
		{
			code.AddRange(code.Provider.BeginTry());
			return code;
		}

		public static Code EndTry(this Code code)
		{
			IInstruction jump;
			code.AddRange(code.Provider.EndTry(false, out jump));
			return code;
		}

		public static Code BeginCatch(this Code code, ISehHandlerBlock handlerBlock)
		{
			code.AddRange(code.Provider.BeginCatch(handlerBlock));
			return code;
		}

		public static Code EndCatch(this Code code, ISehHandlerBlock handlerBlock)
		{
			IInstruction jump;
			code.AddRange(code.Provider.EndCatch(handlerBlock, false, false, out jump));
			return code;
		}

		public static Code BeginFault(this Code code, ISehHandlerBlock handlerBlock)
		{
			code.AddRange(code.Provider.BeginFault(handlerBlock));
			return code;
		}

		public static Code EndFault(this Code code, ISehHandlerBlock handlerBlock)
		{
			code.AddRange(code.Provider.EndFault(handlerBlock));
			return code;
		}

		public static Code BeginFinally(this Code code, ISehHandlerBlock handlerBlock)
		{
			code.AddRange(code.Provider.BeginFinally(handlerBlock));
			return code;
		}

		public static Code EndFinally(this Code code, ISehHandlerBlock handlerBlock)
		{
			code.AddRange(code.Provider.EndFinally(handlerBlock));
			return code;
		}

		#endregion

		public static Code Label(this Code code)
		{
			code.Add(code.Provider.Label());
			return code;
		}

		public static Code Branch(this Code code)
		{
			code.Add(code.Provider.Branch());
			return code;
		}

		public static Code Branch(this Code code, BranchOperator op, IType leftType, IType rightType)
		{
			code.AddRange(code.Provider.Branch(op, leftType, rightType));
			return code;
		}

		public static Code Switch(this Code code, int count)
		{
			var set = code.Provider.Switch(count);
			if (set == null)
				throw new NotSupportedException();
			code.Add(set);
			return code;
		}

		public static Code DebugFile(this Code code, string debugFile)
		{
			code.AddRange(code.Provider.DebugFile(debugFile));
			return code;
		}

		public static Code DebugLine(this Code code, int line)
		{
			code.AddRange(code.Provider.DebugLine(line));
			return code;
		}

		public static Code DebuggerBreak(this Code code)
		{
			code.AddRange(code.Provider.DebuggerBreak());
			return code;
		}

		public static Code LoadTempVar(this Code code, int var)
		{
			code.AddRange(code.Provider.GetTempVar(var));
			return code;
		}

		public static int StoreTempVar(this Code code)
		{
			int var;
			code.AddRange(code.Provider.SetTempVar(out var, false));
			return var;
		}

		public static Code KillTempVar(this Code code, int var)
		{
			code.AddRange(code.Provider.KillTempVar(var));
			return code;
		}

		public static int MoveTemp(this Code code, int var)
		{
			code.LoadTempVar(var);
			return code.StoreTempVar();
		}
	}
}
