using System;
using System.Collections.Generic;
using DataDynamics.PageFX.CLI.IL;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.Execution
{
	/// <summary>
	/// Defines call context.
	/// </summary>
	internal sealed class CallContext
	{
		private readonly VirtualMachine _engine;

		/// <summary>
		/// Instruction pointer.
		/// </summary>
		public int IP;

		public object ReturnValue;

		public Exception Exception;

		public CallContext(VirtualMachine engine, IMethod method, IReadOnlyList<ILocalVariable> locals, object[] args)
		{
			_engine = engine;

			Method = method;
			Locals = locals;
			Arguments = args;

			EvalStack = new Stack<object>(method.Body.MaxStackSize);
		}

		public IMethod Method { get; private set; }

		private Stack<object> EvalStack { get; set; }

		public IReadOnlyList<ILocalVariable> Locals { get; private set; }

		public object[] Arguments { get; set; }

		public IClrMethodBody Body
		{
			get { return (IClrMethodBody)Method.Body; }
		}

		public ILStream Code
		{
			get { return Body.Code; }
		}

		/// <summary>
		/// Current exception handler.
		/// </summary>
		public HandlerBlock Handler { get; set; }

		/// <summary>
		/// Clears eval stack.
		/// </summary>
		public void Clear()
		{
			EvalStack.Clear();
		}

		public void Push(object value)
		{
			Push(value, true);
		}

		public void Push(object value, bool copy)
		{
			value = copy ? value.Copy() : value;
			EvalStack.Push(value);
		}

		public object Peek(bool copy)
		{
			var value = EvalStack.Peek();
			return copy ? value.Copy() : value;
		}

		public object Pop()
		{
			return Pop(true);
		}

		public object Pop(bool copy)
		{
			var value = EvalStack.Pop();
			return copy ? value.Copy() : value;
		}

		public void Dup(bool copy)
		{
			Push(EvalStack.Peek(), copy);
		}

		public object PopObject()
		{
			var obj = Pop(false);
			var ptr = obj as IPointer;
			return ptr != null ? ptr.Value : obj;
		}

		public Array PopArray()
		{
			return PopObject() as Array;
		}

		public Instance PopInstance()
		{
			return PopObject() as Instance;
		}

		public void LoadLocal(int index)
		{
			Push(Locals[index].Value, true);
		}

		public void LoadLocalPtr(int index)
		{
			Push(new LocalPtr(this, index), false);
		}

		public void StoreLocal(int index)
		{
			var value = Pop(true);
			Locals[index].Value = value;
		}

		public void LoadArg(int index)
		{
			var value = Arguments[index];
			//do not copy this argument!
			if (index == 0 && !Method.IsStatic)
			{
				Push(value, false);
			}
			else
			{
				Push(value, true);
			}
		}

		public void LoadArgPtr(int index)
		{
			Push(new ArgumentPtr(this, index), false);
		}

		public void StoreArg(int index)
		{
			//TODO: handle this assignemt in value types such Int32
			Arguments[index] = Pop(true);
		}

		public void Return()
		{
			IP = Code.Count;

			if (Method.IsVoid())
			{
				return;
			}

			ReturnValue = Pop(true);
		}

		public override string ToString()
		{
			return Method.ToString();
		}

		private sealed class LocalPtr : IPointer
		{
			private readonly CallContext _context;
			private readonly int _index;

			public LocalPtr(CallContext context, int index)
			{
				_context = context;
				_index = index;
			}

			public object Value
			{
				get { return _context.Locals[_index].Value;}
				set { _context.Locals[_index].Value = value; }
			}
		}

		private sealed class ArgumentPtr : IPointer
		{
			private readonly CallContext _context;
			private readonly int _index;

			public ArgumentPtr(CallContext context, int index)
			{
				_context = context;
				_index = index;
			}

			public object Value
			{
				get { return _context.Arguments[_index]; }
				set { _context.Arguments[_index] = value; }
			}
		}
	}

	internal interface IPointer
	{
		object Value { get; set; }
	}
}