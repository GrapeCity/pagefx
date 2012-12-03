
//
// Copyright (C) 2004 Novell, Inc (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

//
// System.Reflection.Emit/ILGenerator.cs
//
// Author:
//   Paolo Molaro (lupus@ximian.com)
//
// (C) 2001 Ximian, Inc.  http://www.ximian.com
//

using System;
using System.Collections;
using System.Diagnostics.SymbolStore;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit {

	internal struct ILExceptionBlock {
		public const int CATCH = 0;
		public const int FILTER = 1;
		public const int FINALLY = 2;
		public const int FAULT = 4;

		internal Type extype;
		internal int type;
		internal int start;
		internal int len;
		internal int filter_offset;
		
		internal void Debug () {
#if NO
			System.Console.Write ("\ttype="+type.ToString()+" start="+start.ToString()+" len="+len.ToString());
			if (extype != null)
				System.Console.WriteLine (" extype="+extype.ToString());
			else
				System.Console.WriteLine (String.Empty);
#endif
		}
	}
	internal struct ILExceptionInfo {
		ILExceptionBlock[] handlers;
		internal int start;
		int len;
		internal Label end;

		internal int NumHandlers ()
		{
			return handlers.Length;
		}
		
		internal void AddCatch (Type extype, int offset)
		{
			int i;
			End (offset);
			add_block (offset);
			i = handlers.Length - 1;
			handlers [i].type = ILExceptionBlock.CATCH;
			handlers [i].start = offset;
			handlers [i].extype = extype;
		}

		internal void AddFinally (int offset)
		{
			int i;
			End (offset);
			add_block (offset);
			i = handlers.Length - 1;
			handlers [i].type = ILExceptionBlock.FINALLY;
			handlers [i].start = offset;
			handlers [i].extype = null;
		}

		internal void AddFault (int offset)
		{
			int i;
			End (offset);
			add_block (offset);
			i = handlers.Length - 1;
			handlers [i].type = ILExceptionBlock.FAULT;
			handlers [i].start = offset;
			handlers [i].extype = null;
		}

		internal void AddFilter (int offset)
		{
			int i;
			End (offset);
			add_block (offset);
			i = handlers.Length - 1;
			handlers [i].type = ILExceptionBlock.FILTER;
			handlers [i].extype = null;
			handlers [i].filter_offset = offset;
		}

		internal void End (int offset)
		{
			if (handlers == null)
				return;
			int i = handlers.Length - 1;
			if (i >= 0)
				handlers [i].len = offset - handlers [i].start;
		}

		internal int LastClauseType ()
		{
			if (handlers != null)
				return handlers [handlers.Length-1].type;
			else
				return ILExceptionBlock.CATCH;
		}

		internal void PatchLastClauseStart (int start)
		{
			if (handlers != null && handlers.Length > 0)
				handlers [handlers.Length - 1].start = start;
		}

		internal void Debug (int b)
		{
#if NO
			System.Console.WriteLine ("Handler {0} at {1}, len: {2}", b, start, len);
			for (int i = 0; i < handlers.Length; ++i)
				handlers [i].Debug ();
#endif
		}

		void add_block (int offset)
		{
			if (handlers != null) {
				int i = handlers.Length;
				ILExceptionBlock[] new_b = new ILExceptionBlock [i + 1];
				System.Array.Copy (handlers, new_b, i);
				handlers = new_b;
				handlers [i].len = offset - handlers [i].start;
			} else {
				handlers = new ILExceptionBlock [1];
				len = offset - start;
			}
		}
	}
	
	internal struct ILTokenInfo {
		public MemberInfo member;
		public int code_pos;
	}

	internal interface TokenGenerator {
		int GetToken (string str);

		int GetToken (MemberInfo member);

		int GetToken (MethodInfo method, Type[] opt_param_types);

		int GetToken (SignatureHelper helper);
	}		

#if NET_2_0
	[ComVisible (true)]
#endif
	[ClassInterface (ClassInterfaceType.None)]
	public class ILGenerator: _ILGenerator {
		private struct LabelFixup {
			public int offset;    // The number of bytes between pos and the
							      // offset of the jump
			public int pos;	      // Where offset of the label is placed
			public int label_idx; // The label to jump to
		};
		
		struct LabelData {
			public LabelData (int addr, int maxStack)
			{
				this.addr = addr;
				this.maxStack = maxStack;
			}
			
			public int addr;
			public int maxStack; 
		}
		
		static readonly Type void_type = typeof (void);
		#region Sync with reflection.h
		private byte[] code;
		private int code_len;
		private int max_stack;
		private int cur_stack;
		private LocalBuilder[] locals;
		private ILExceptionInfo[] ex_handlers;
		private int num_token_fixups;
		private ILTokenInfo[] token_fixups;
		#endregion
		
		private LabelData [] labels;
		private int num_labels;
		private LabelFixup[] fixups;
		private int num_fixups;
		internal Module module;
		private Stack scopes;
		private int cur_block;
		private Stack open_blocks;
		private TokenGenerator token_gen;
		
		const int defaultFixupSize = 4;
		const int defaultLabelsSize = 4;
		const int defaultExceptionStackSize = 2;
		
		ArrayList sequencePointLists;
		SequencePointList currentSequence;

		internal ILGenerator (Module m, TokenGenerator token_gen, int size)
		{
			if (size < 0)
				size = 128;
			code = new byte [size];
			token_fixups = new ILTokenInfo [8];
			module = m;
			this.token_gen = token_gen;
		}
		
		private void add_token_fixup (MemberInfo mi)
		{
			if (num_token_fixups == token_fixups.Length) {
				ILTokenInfo[] ntf = new ILTokenInfo [num_token_fixups * 2];
				token_fixups.CopyTo (ntf, 0);
				token_fixups = ntf;
			}
			token_fixups [num_token_fixups].member = mi;
			token_fixups [num_token_fixups++].code_pos = code_len;
		}

		private void make_room (int nbytes)
		{
			if (code_len + nbytes < code.Length)
				return;
			byte[] new_code = new byte [(code_len + nbytes) * 2 + 128];
			System.Array.Copy (code, 0, new_code, 0, code.Length);
			code = new_code;
		}

		private void emit_int (int val)
		{
			code [code_len++] = (byte) (val & 0xFF);
			code [code_len++] = (byte) ((val >> 8) & 0xFF);
			code [code_len++] = (byte) ((val >> 16) & 0xFF);
			code [code_len++] = (byte) ((val >> 24) & 0xFF);
		}

		/* change to pass by ref to avoid copy */
		private void ll_emit (OpCode opcode)
		{
			/* 
			 * there is already enough room allocated in code.
			 */
			// access op1 and op2 directly since the Value property is useless
			if (opcode.Size == 2)
				code [code_len++] = opcode.op1;
			code [code_len++] = opcode.op2;
			/*
			 * We should probably keep track of stack needs here.
			 * Or we may want to run the verifier on the code before saving it
			 * (this may be needed anyway when the ILGenerator is not used...).
			 */
			switch (opcode.StackBehaviourPush) {
			case StackBehaviour.Push1:
			case StackBehaviour.Pushi:
			case StackBehaviour.Pushi8:
			case StackBehaviour.Pushr4:
			case StackBehaviour.Pushr8:
			case StackBehaviour.Pushref:
			case StackBehaviour.Varpush: /* again we are conservative and assume it pushes 1 */
				cur_stack ++;
				break;
			case StackBehaviour.Push1_push1:
				cur_stack += 2;
				break;
			}
			if (max_stack < cur_stack)
				max_stack = cur_stack;

			/* 
			 * Note that we adjust for the pop behaviour _after_ setting max_stack.
			 */
			switch (opcode.StackBehaviourPop) {
			case StackBehaviour.Varpop:
				break; /* we are conservative and assume it doesn't decrease the stack needs */
			case StackBehaviour.Pop1:
			case StackBehaviour.Popi:
			case StackBehaviour.Popref:
				cur_stack --;
				break;
			case StackBehaviour.Pop1_pop1:
			case StackBehaviour.Popi_pop1:
			case StackBehaviour.Popi_popi:
			case StackBehaviour.Popi_popi8:
			case StackBehaviour.Popi_popr4:
			case StackBehaviour.Popi_popr8:
			case StackBehaviour.Popref_pop1:
			case StackBehaviour.Popref_popi:
				cur_stack -= 2;
				break;
			case StackBehaviour.Popi_popi_popi:
			case StackBehaviour.Popref_popi_popi:
			case StackBehaviour.Popref_popi_popi8:
			case StackBehaviour.Popref_popi_popr4:
			case StackBehaviour.Popref_popi_popr8:
			case StackBehaviour.Popref_popi_popref:
				cur_stack -= 3;
				break;
			}
		}

		private static int target_len (OpCode opcode)
		{
			if (opcode.OperandType == OperandType.InlineBrTarget)
				return 4;
			return 1;
		}

		private void InternalEndClause ()
		{
			switch (ex_handlers [cur_block].LastClauseType ()) {
			case ILExceptionBlock.CATCH:
			case ILExceptionBlock.FILTER:
				// how could we optimize code size here?
				Emit (OpCodes.Leave, ex_handlers [cur_block].end);
				break;
			case ILExceptionBlock.FAULT:
			case ILExceptionBlock.FINALLY:
				Emit (OpCodes.Endfinally);
				break;
			}
		}

		public virtual void BeginCatchBlock (Type exceptionType)
		{
			if (open_blocks == null)
				open_blocks = new Stack (defaultExceptionStackSize);

			if (open_blocks.Count <= 0)
				throw new NotSupportedException ("Not in an exception block");

			if (ex_handlers [cur_block].LastClauseType () == ILExceptionBlock.FILTER) {
				if (exceptionType != null)
					throw new ArgumentException ("Do not supply an exception type for filter clause");
				Emit (OpCodes.Endfilter);
				ex_handlers [cur_block].PatchLastClauseStart (code_len);
			} else {
				InternalEndClause ();
				ex_handlers [cur_block].AddCatch (exceptionType, code_len);
			}
			
			cur_stack = 1; // the exception object is on the stack by default
			if (max_stack < cur_stack)
				max_stack = cur_stack;

			//System.Console.WriteLine ("Begin catch Block: {0} {1}",exceptionType.ToString(), max_stack);
		}

		public virtual void BeginExceptFilterBlock ()
		{
			if (open_blocks == null)
				open_blocks = new Stack (defaultExceptionStackSize);
			
			if (open_blocks.Count <= 0)
				throw new NotSupportedException ("Not in an exception block");
			InternalEndClause ();

			ex_handlers [cur_block].AddFilter (code_len);
		}

		public virtual Label BeginExceptionBlock ()
		{
			//System.Console.WriteLine ("Begin Block");
			if (open_blocks == null)
				open_blocks = new Stack (defaultExceptionStackSize);
			
			if (ex_handlers != null) {
				cur_block = ex_handlers.Length;
				ILExceptionInfo[] new_ex = new ILExceptionInfo [cur_block + 1];
				System.Array.Copy (ex_handlers, new_ex, cur_block);
				ex_handlers = new_ex;
			} else {
				ex_handlers = new ILExceptionInfo [1];
				cur_block = 0;
			}
			open_blocks.Push (cur_block);
			ex_handlers [cur_block].start = code_len;
			return ex_handlers [cur_block].end = DefineLabel ();
		}

		public virtual void BeginFaultBlock()
		{
			if (open_blocks == null)
				open_blocks = new Stack (defaultExceptionStackSize);
			
			if (open_blocks.Count <= 0)
				throw new NotSupportedException ("Not in an exception block");
			InternalEndClause ();
			//System.Console.WriteLine ("Begin fault Block");
			ex_handlers [cur_block].AddFault (code_len);
		}
		
		public virtual void BeginFinallyBlock()
		{
			if (open_blocks == null)
				open_blocks = new Stack (defaultExceptionStackSize);
			
			if (open_blocks.Count <= 0)
				throw new NotSupportedException ("Not in an exception block");
			InternalEndClause ();
			//System.Console.WriteLine ("Begin finally Block");
			ex_handlers [cur_block].AddFinally (code_len);
		}
		
		public virtual void BeginScope ()
		{ }
		
		public LocalBuilder DeclareLocal (Type localType)
		{
			return DeclareLocal (localType, false);
		}


#if NET_2_0
		public
#else
		internal
#endif
		LocalBuilder DeclareLocal (Type localType, bool pinned)
		{
			if (localType == null)
				throw new ArgumentNullException ("localType");

			LocalBuilder res = new LocalBuilder (localType, this);
			res.is_pinned = pinned;
			
			if (locals != null) {
				LocalBuilder[] new_l = new LocalBuilder [locals.Length + 1];
				System.Array.Copy (locals, new_l, locals.Length);
				new_l [locals.Length] = res;
				locals = new_l;
			} else {
				locals = new LocalBuilder [1];
				locals [0] = res;
			}
			res.position = (ushort)(locals.Length - 1);
			return res;
		}
		
		public virtual Label DefineLabel ()
		{
			if (labels == null)
				labels = new LabelData [defaultLabelsSize];
			else if (num_labels >= labels.Length) {
				LabelData [] t = new LabelData [labels.Length * 2];
				Array.Copy (labels, t, labels.Length);
				labels = t;
			}
			
			labels [num_labels] = new LabelData (-1, 0);
			
			return new Label (num_labels++);
		}
		
		public virtual void Emit (OpCode opcode)
		{
			make_room (2);
			ll_emit (opcode);
		}
		
		public virtual void Emit (OpCode opcode, Byte val)
		{
			make_room (3);
			ll_emit (opcode);
			code [code_len++] = val;
		}
		
#if NET_2_0
		[ComVisible (true)]
#endif
		public virtual void Emit (OpCode opcode, ConstructorInfo constructor)
		{
			int token = token_gen.GetToken (constructor);
			make_room (6);
			ll_emit (opcode);
			if (constructor.DeclaringType.Module == module)
				add_token_fixup (constructor);
			emit_int (token);
			
			if (opcode.StackBehaviourPop == StackBehaviour.Varpop)
				cur_stack -= constructor.GetParameterCount ();
		}
		
		public virtual void Emit (OpCode opcode, double val)
		{
			byte[] s = System.BitConverter.GetBytes (val);
			make_room (10);
			ll_emit (opcode);
			if (BitConverter.IsLittleEndian){
				System.Array.Copy (s, 0, code, code_len, 8);
				code_len += 8;
			} else {
				code [code_len++] = s [7];
				code [code_len++] = s [6];
				code [code_len++] = s [5];
				code [code_len++] = s [4];
				code [code_len++] = s [3];
				code [code_len++] = s [2];
				code [code_len++] = s [1];
				code [code_len++] = s [0];				
			}
		}
		
		public virtual void Emit (OpCode opcode, FieldInfo field)
		{
			int token = token_gen.GetToken (field);
			make_room (6);
			ll_emit (opcode);
			if (field.DeclaringType.Module == module)
				add_token_fixup (field);
			emit_int (token);
		}
		
		public virtual void Emit (OpCode opcode, Int16 val)
		{
			make_room (4);
			ll_emit (opcode);
			code [code_len++] = (byte) (val & 0xFF);
			code [code_len++] = (byte) ((val >> 8) & 0xFF);
		}
		
		public virtual void Emit (OpCode opcode, int val)
		{
			make_room (6);
			ll_emit (opcode);
			emit_int (val);
		}
		
		public virtual void Emit (OpCode opcode, long val)
		{
			make_room (10);
			ll_emit (opcode);
			code [code_len++] = (byte) (val & 0xFF);
			code [code_len++] = (byte) ((val >> 8) & 0xFF);
			code [code_len++] = (byte) ((val >> 16) & 0xFF);
			code [code_len++] = (byte) ((val >> 24) & 0xFF);
			code [code_len++] = (byte) ((val >> 32) & 0xFF);
			code [code_len++] = (byte) ((val >> 40) & 0xFF);
			code [code_len++] = (byte) ((val >> 48) & 0xFF);
			code [code_len++] = (byte) ((val >> 56) & 0xFF);
		}
		
		public virtual void Emit (OpCode opcode, Label label)
		{
			int tlen = target_len (opcode);
			make_room (6);
			ll_emit (opcode);
			if (cur_stack > labels [label.label].maxStack)
				labels [label.label].maxStack = cur_stack;
			
			if (fixups == null)
				fixups = new LabelFixup [defaultFixupSize]; 
			else if (num_fixups >= fixups.Length) {
				LabelFixup[] newf = new LabelFixup [fixups.Length * 2];
				System.Array.Copy (fixups, newf, fixups.Length);
				fixups = newf;
			}
			fixups [num_fixups].offset = tlen;
			fixups [num_fixups].pos = code_len;
			fixups [num_fixups].label_idx = label.label;
			num_fixups++;
			code_len += tlen;

		}
		
		public virtual void Emit (OpCode opcode, Label[] labels)
		{
			/* opcode needs to be switch. */
			int count = labels.Length;
			make_room (6 + count * 4);
			ll_emit (opcode);

			for (int i = 0; i < count; ++i)
				if (cur_stack > this.labels [labels [i].label].maxStack)
					this.labels [labels [i].label].maxStack = cur_stack;

			emit_int (count);
			if (fixups == null)
				fixups = new LabelFixup [defaultFixupSize + count]; 
			else if (num_fixups + count >= fixups.Length) {
				LabelFixup[] newf = new LabelFixup [count + fixups.Length * 2];
				System.Array.Copy (fixups, newf, fixups.Length);
				fixups = newf;
			}
			
			// ECMA 335, Partition III, p94 (7-10)
			//
			// The switch instruction implements a jump table. The format of 
			// the instruction is an unsigned int32 representing the number of targets N,
			// followed by N int32 values specifying jump targets: these targets are
			// represented as offsets (positive or negative) from the beginning of the 
			// instruction following this switch instruction.
			//
			// We must make sure it gets an offset from the *end* of the last label
			// (eg, the beginning of the instruction following this).
			//
			// remaining is the number of bytes from the current instruction to the
			// instruction that will be emitted.
			
			for (int i = 0, remaining = count * 4; i < count; ++i, remaining -= 4) {
				fixups [num_fixups].offset = remaining;
				fixups [num_fixups].pos = code_len;
				fixups [num_fixups].label_idx = labels [i].label;
				num_fixups++;
				code_len += 4;
			}
		}

		public virtual void Emit (OpCode opcode, LocalBuilder lbuilder)
		{
			uint pos = lbuilder.position;
			bool load_addr = false;
			bool is_store = false;
			make_room (6);

			if (lbuilder.ilgen != this)
				throw new Exception ("Trying to emit a local from a different ILGenerator.");

			/* inline the code from ll_emit () to optimize il code size */
			if (opcode.StackBehaviourPop == StackBehaviour.Pop1) {
				cur_stack --;
				is_store = true;
			} else {
				cur_stack++;
				if (cur_stack > max_stack)
					max_stack = cur_stack;
				load_addr = opcode.StackBehaviourPush == StackBehaviour.Pushi;
			}
			if (load_addr) {
				if (pos < 256) {
					code [code_len++] = (byte)0x12;
					code [code_len++] = (byte)pos;
				} else {
					code [code_len++] = (byte)0xfe;
					code [code_len++] = (byte)0x0d;
					code [code_len++] = (byte)(pos & 0xff);
					code [code_len++] = (byte)((pos >> 8) & 0xff);
				}
			} else {
				if (is_store) {
					if (pos < 4) {
						code [code_len++] = (byte)(0x0a + pos);
					} else if (pos < 256) {
						code [code_len++] = (byte)0x13;
						code [code_len++] = (byte)pos;
					} else {
						code [code_len++] = (byte)0xfe;
						code [code_len++] = (byte)0x0e;
						code [code_len++] = (byte)(pos & 0xff);
						code [code_len++] = (byte)((pos >> 8) & 0xff);
					}
				} else {
					if (pos < 4) {
						code [code_len++] = (byte)(0x06 + pos);
					} else if (pos < 256) {
						code [code_len++] = (byte)0x11;
						code [code_len++] = (byte)pos;
					} else {
						code [code_len++] = (byte)0xfe;
						code [code_len++] = (byte)0x0c;
						code [code_len++] = (byte)(pos & 0xff);
						code [code_len++] = (byte)((pos >> 8) & 0xff);
					}
				}
			}
		}

		public virtual void Emit (OpCode opcode, MethodInfo method)
		{
			if (method == null)
				throw new ArgumentNullException ("method");

#if NET_2_0
			// For compatibility with MS
			if ((method is DynamicMethod) && ((opcode == OpCodes.Ldftn) || (opcode == OpCodes.Ldvirtftn) || (opcode == OpCodes.Ldtoken)))
				throw new ArgumentException ("Ldtoken, Ldftn and Ldvirtftn OpCodes cannot target DynamicMethods.");
#endif

			int token = token_gen.GetToken (method);
			make_room (6);
			ll_emit (opcode);
			Type declaringType = method.DeclaringType;
			// Might be a DynamicMethod with no declaring type
			if (declaringType != null) {
				if (declaringType.Module == module)
					add_token_fixup (method);
			}
			emit_int (token);
			if (method.ReturnType != void_type)
				cur_stack ++;

			if (opcode.StackBehaviourPop == StackBehaviour.Varpop)
				cur_stack -= method.GetParameterCount ();
		}

		private void Emit (OpCode opcode, MethodInfo method, int token)
		{
			make_room (6);
			ll_emit (opcode);
			// Might be a DynamicMethod with no declaring type
			Type declaringType = method.DeclaringType;
			if (declaringType != null) {
				if (declaringType.Module == module)
					add_token_fixup (method);
			}
			emit_int (token);
			if (method.ReturnType != void_type)
				cur_stack ++;

			if (opcode.StackBehaviourPop == StackBehaviour.Varpop)
				cur_stack -= method.GetParameterCount ();
		}

		[CLSCompliant(false)]
		public void Emit (OpCode opcode, sbyte val)
		{
			make_room (3);
			ll_emit (opcode);
			code [code_len++] = (byte)val;
		}

		public virtual void Emit (OpCode opcode, SignatureHelper shelper)
		{
			int token = token_gen.GetToken (shelper);
			make_room (6);
			ll_emit (opcode);
			emit_int (token);
		}

		public virtual void Emit (OpCode opcode, float val)
		{
			byte[] s = System.BitConverter.GetBytes (val);
			make_room (6);
			ll_emit (opcode);
			if (BitConverter.IsLittleEndian){
				System.Array.Copy (s, 0, code, code_len, 4);
				code_len += 4;
			} else {
				code [code_len++] = s [3];
				code [code_len++] = s [2];
				code [code_len++] = s [1];
				code [code_len++] = s [0];				
			}
		}

		public virtual void Emit (OpCode opcode, string val)
		{
			int token = token_gen.GetToken (val);
			make_room (6);
			ll_emit (opcode);
			emit_int (token);
		}

		public virtual void Emit (OpCode opcode, Type type)
		{
			make_room (6);
			ll_emit (opcode);
			emit_int (token_gen.GetToken (type));
		}

		[MonoLimitation ("vararg methods are not supported")]
		public void EmitCall (OpCode opcode, MethodInfo methodinfo, Type[] optionalParamTypes)
		{
			if (methodinfo == null)
				throw new ArgumentNullException ("methodinfo can not be null");
			short value = opcode.Value;
			if (!(value == OpCodes.Call.Value || value == OpCodes.Callvirt.Value))
				throw new NotSupportedException ("Only Call and CallVirt are allowed");
			if (optionalParamTypes != null){
				if ((methodinfo.CallingConvention & CallingConventions.VarArgs)  == 0){
					throw new InvalidOperationException ("Method is not VarArgs method and optional types were passed");
				}

				int token = token_gen.GetToken (methodinfo, optionalParamTypes);
				Emit (opcode, methodinfo, token);
				return;
			}
			Emit (opcode, methodinfo);
		}

		public void EmitCalli (OpCode opcode, CallingConvention unmanagedCallConv, Type returnType, Type[] paramTypes)
		{
			SignatureHelper helper 
				= SignatureHelper.GetMethodSigHelper (module, 0, unmanagedCallConv, returnType, paramTypes);
			Emit (opcode, helper);
		}

		public void EmitCalli (OpCode opcode, CallingConventions callConv, Type returnType, Type[] paramTypes, Type[] optionalParamTypes)
		{
			if (optionalParamTypes != null)
				throw new NotImplementedException ();

			SignatureHelper helper 
				= SignatureHelper.GetMethodSigHelper (module, callConv, 0, returnType, paramTypes);
			Emit (opcode, helper);
		}
		
		public virtual void EmitWriteLine (FieldInfo field)
		{
			if (field == null)
				throw new ArgumentNullException ("field");
			
			// The MS implementation does not check for valuetypes here but it
			// should. Also, it should check that if the field is not static,
			// then it is a member of this type.
			if (field.IsStatic)
				Emit (OpCodes.Ldsfld, field);
			else {
				Emit (OpCodes.Ldarg_0);
				Emit (OpCodes.Ldfld, field);
			}
			Emit (OpCodes.Call, 
			      typeof (Console).GetMethod ("WriteLine",
							  new Type[1] { field.FieldType }));
		}

		public virtual void EmitWriteLine (LocalBuilder lbuilder)
		{
			if (lbuilder == null)
				throw new ArgumentNullException ("lbuilder");
			if (lbuilder.LocalType is TypeBuilder)
				throw new  ArgumentException ("Output streams do not support TypeBuilders.");
			// The MS implementation does not check for valuetypes here but it
			// should.
			Emit (OpCodes.Ldloc, lbuilder);
			Emit (OpCodes.Call, 
			      typeof (Console).GetMethod ("WriteLine",
							  new Type[1] { lbuilder.LocalType }));
		}
		
		public virtual void EmitWriteLine (string val)
		{
			Emit (OpCodes.Ldstr, val);
			Emit (OpCodes.Call, 
			      typeof (Console).GetMethod ("WriteLine",
							  new Type[1] { typeof(string)}));
		}

		public virtual void EndExceptionBlock ()
		{
			if (open_blocks == null)
				open_blocks = new Stack (defaultExceptionStackSize);
			
			if (open_blocks.Count <= 0)
				throw new NotSupportedException ("Not in an exception block");
			InternalEndClause ();
			MarkLabel (ex_handlers [cur_block].end);
			ex_handlers [cur_block].End (code_len);
			ex_handlers [cur_block].Debug (cur_block);
			//System.Console.WriteLine ("End Block {0} (handlers: {1})", cur_block, ex_handlers [cur_block].NumHandlers ());
			open_blocks.Pop ();
			if (open_blocks.Count > 0)
				cur_block = (int)open_blocks.Peek ();
			//Console.WriteLine ("curblock restored to {0}", cur_block);
			//throw new NotImplementedException ();
		}

		public virtual void EndScope ()
		{ }

		public virtual void MarkLabel (Label loc)
		{
			if (loc.label < 0 || loc.label >= num_labels)
				throw new System.ArgumentException ("The label is not valid");
			if (labels [loc.label].addr >= 0)
				throw new System.ArgumentException ("The label was already defined");
			labels [loc.label].addr = code_len;
			if (labels [loc.label].maxStack > cur_stack)
				cur_stack = labels [loc.label].maxStack;
		}

		public virtual void MarkSequencePoint (ISymbolDocumentWriter document, int startLine,
						       int startColumn, int endLine, int endColumn)
		{
			if (currentSequence == null || currentSequence.Document != document) {
				if (sequencePointLists == null)
					sequencePointLists = new ArrayList ();
				currentSequence = new SequencePointList (document);
				sequencePointLists.Add (currentSequence);
			}
			
			currentSequence.AddSequencePoint (code_len, startLine, startColumn, endLine, endColumn);
		}
		
		internal void GenerateDebugInfo (ISymbolWriter symbolWriter)
		{
			if (sequencePointLists != null) {
				SequencePointList first = (SequencePointList) sequencePointLists [0];
				SequencePointList last = (SequencePointList) sequencePointLists [sequencePointLists.Count - 1];
				symbolWriter.SetMethodSourceRange (first.Document, first.StartLine, first.StartColumn, last.Document, last.EndLine, last.EndColumn);
				
				foreach (SequencePointList list in sequencePointLists)
					symbolWriter.DefineSequencePoints (list.Document, list.GetOffsets(), list.GetLines(), list.GetColumns(), list.GetEndLines(), list.GetEndColumns());
				
				if (locals != null) {
					foreach (LocalBuilder local in locals) {
						if (local.Name != null && local.Name.Length > 0) {
							SignatureHelper sighelper = SignatureHelper.GetLocalVarSigHelper (module);
							sighelper.AddArgument (local.LocalType);
							byte[] signature = sighelper.GetSignature ();
							symbolWriter.DefineLocalVariable (local.Name, FieldAttributes.Public, signature, SymAddressKind.ILOffset, local.position, 0, 0, local.StartOffset, local.EndOffset);
						}
					}
				}
				sequencePointLists = null;
			}
		}
		
		internal bool HasDebugInfo
		{
			get { return sequencePointLists != null; }
		}

		public virtual void ThrowException (Type exceptionType)
		{
			if (exceptionType == null)
				throw new ArgumentNullException ("exceptionType");
			if (! ((exceptionType == typeof (Exception)) || 
				   exceptionType.IsSubclassOf (typeof (Exception))))
				throw new ArgumentException ("Type should be an exception type", "exceptionType");
			ConstructorInfo ctor = exceptionType.GetConstructor (new Type[0]);
			if (ctor == null)
				throw new ArgumentException ("Type should have a default constructor", "exceptionType");
			Emit (OpCodes.Newobj, ctor);
			Emit (OpCodes.Throw);
		}

		[MonoTODO("Not implemented")]
		public void UsingNamespace (String usingNamespace)
		{
			throw new NotImplementedException ();
		}

		internal void label_fixup ()
		{
			for (int i = 0; i < num_fixups; ++i) {
				
				// Diff is the offset from the end of the jump instruction to the address of the label
				int diff = labels [fixups [i].label_idx].addr - (fixups [i].pos + fixups [i].offset);
				if (fixups [i].offset == 1) {
					code [fixups [i].pos] = (byte)((sbyte) diff);
				} else {
					int old_cl = code_len;
					code_len = fixups [i].pos;
					emit_int (diff);
					code_len = old_cl;
				}
			}
		}

		internal static int Mono_GetCurrentOffset (ILGenerator ig)
		{
			return ig.code_len;
		}

		void _ILGenerator.GetIDsOfNames ([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException ();
		}

		void _ILGenerator.GetTypeInfo (uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException ();
		}

		void _ILGenerator.GetTypeInfoCount (out uint pcTInfo)
		{
			throw new NotImplementedException ();
		}

		void _ILGenerator.Invoke (uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException ();
		}
	}
	
	internal class SequencePointList
	{
		ISymbolDocumentWriter doc;
		SequencePoint[] points;
		int count;
		const int arrayGrow = 10;
		
		public SequencePointList (ISymbolDocumentWriter doc)
		{
			this.doc = doc;
		}
		
		public ISymbolDocumentWriter Document {
			get { return doc; }
		}
		
		public int[] GetOffsets()
		{
			int[] data = new int [count];
			for (int n=0; n<count; n++) data [n] = points[n].Offset;
			return data; 
		}
		public int[] GetLines()
		{
			int[] data = new int [count];
			for (int n=0; n<count; n++) data [n] = points[n].Line;
			return data; 
		}
		public int[] GetColumns()
		{
			int[] data = new int [count];
			for (int n=0; n<count; n++) data [n] = points[n].Col;
			return data; 
		}
		public int[] GetEndLines()
		{
			int[] data = new int [count];
			for (int n=0; n<count; n++) data [n] = points[n].EndLine;
			return data; 
		}
		public int[] GetEndColumns()
		{
			int[] data = new int [count];
			for (int n=0; n<count; n++) data [n] = points[n].EndCol;
			return data; 
		}
		public int StartLine {
			get { return points[0].Line; }
		}
		public int EndLine {
			get { return points[count - 1].Line; }
		}
		public int StartColumn {
			get { return points[0].Col; }
		}
		public int EndColumn {
			get { return points[count - 1].Col; }
		}
		
		public void AddSequencePoint (int offset, int line, int col, int endLine, int endCol)
		{
			SequencePoint s = new SequencePoint ();
			s.Offset = offset;
			s.Line = line;
			s.Col = col;
			s.EndLine = endLine;
			s.EndCol = endCol;
			
			if (points == null) {
				points = new SequencePoint [arrayGrow];
			} else if (count >= points.Length) {
				SequencePoint[] temp = new SequencePoint [count + arrayGrow];
				Array.Copy (points, temp, points.Length);
				points = temp;
			}
			
			points [count] = s;
			count++;
		}
	}
	
	struct SequencePoint {
		public int Offset;
		public int Line;
		public int Col;
		public int EndLine;
		public int EndCol;
	}
}
