using System;
using System.Collections.Generic;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Core.IL;
using DataDynamics.PageFX.Core.Translation.ControlFlow;
using DataDynamics.PageFX.Core.Translation.Values;

namespace DataDynamics.PageFX.Core.Translation
{
	internal sealed class TranslationContext
	{
		public readonly Node Block;
		public readonly Code Code;
		public bool CastToParamType;

		/// <summary>
		/// Initializes new instance of <see cref="TranslationContext"/> class.
		/// </summary>
		/// <param name="code">The code generator.</param>
		/// <param name="block">Null if you want to create a global context.</param>
		public TranslationContext(Code code, Node block)
		{
			if (code == null)
				throw new ArgumentNullException("code");

			Code = code;
			Block = block;			
		}

		public IMethod Method
		{
			get { return Code.Method; }
		}

		public IClrMethodBody Body
		{
			get { return Code.Body; }
		}

		public string FullMethodName
		{
			get { return Method.DeclaringType.FullName + "." + Method.Name; }
		}

		public IType ReturnType
		{
			get { return Method.Type; }
		}

		public ICodeProvider Provider
		{
			get { return Code.Provider; }
		}

		public EvalStack Stack
		{
			get { return Block.Stack; }
		}

		public void Push(Instruction instruction, IValue value)
		{
			Block.Stack.Push(instruction, value);
		}

		public void PushResult(Instruction instruction, IType type)
		{
			Push(instruction, new ComputedValue(type));
		}

		public EvalItem Peek()
		{
			return Stack.Peek();
		}

		public EvalItem Pop(Instruction instruction)
		{
			var stack = Stack;
			if (stack.Count == 0)
			{
				if (!instruction.IsHandlerBegin)
				{
					throw new ILTranslatorException("EvalStack is empty");
				}
				var cb = instruction.SehBlock as HandlerBlock;
				if (cb == null)
					throw new ILTranslatorException();
				return new EvalItem(null, new ComputedValue(cb.ExceptionType));
			}
			return stack.Pop();
		}

		public IValue PopValue(Instruction instruction)
		{
			return Pop(instruction).Value;
		}

		public TranslationContext New(Node block)
		{
			return new TranslationContext(Code.New(), block);
		}

		public TranslationContext New()
		{
			return New(Block);
		}

		/// <summary>
		/// Emits given instruction to current block.
		/// </summary>
		/// <param name="i">instruction to add.</param>
		public void Emit(IInstruction i)
		{
			if (i == null)
				throw new ArgumentNullException("i");

			var block = Block;
			var list = block.TranslatedCode;

			int n = list.Count;
			if (n > 0)
			{
				//TODO: remove duplicate instructions in block optimization phase
				//Remove duplicate instructions
				if (Provider.IsDuplicate(list[n - 1], i))
					return;
			}

			list.Add(i);
		}

		/// <summary>
		/// Adds specified code to current block.
		/// </summary>
		/// <param name="code">set of instructions to add.</param>
		public void Emit(IEnumerable<IInstruction> code)
		{
			if (code == null) return;

			foreach (var i in code)
				Emit(i);
		}

		public void EmitCast(IType source, IType target)
		{
			if (ReferenceEquals(target, source)) return;
			Emit(Provider.Cast(source, target, false));
		}

		public void EmitSwap()
		{
			var i = Provider.Swap();
			if (i == null)
				throw new NotSupportedException("Swap instruction is not supported");
			Emit(i);
		}

		public void EmitCode()
		{
			Emit(Code);
			Code.Clear();
		}
	}
}