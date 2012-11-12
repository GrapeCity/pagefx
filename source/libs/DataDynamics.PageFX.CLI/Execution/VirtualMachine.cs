using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using DataDynamics.PageFX.CLI.IL;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.Execution
{
	//TODO: - nullables
	//TODO: - reflection: Object.GetType
	//TODO: - exception handling
	//TODO: - generics
	//TODO: - multi dimensional arrays

	/// <summary>
	/// Implements CLR emulator.
	/// </summary>
	public sealed class VirtualMachine
	{
		private static readonly Dictionary<string, NativeInvoker> SharedNativeInvokers = new Dictionary<string, NativeInvoker>();
		private readonly Dictionary<string, NativeInvoker> _nativeInvokers = new Dictionary<string, NativeInvoker>();

		private readonly Stack<CallContext> _callStack = new Stack<CallContext>();

		static VirtualMachine()
		{
			RegiterSni(typeof(Boolean));
			RegiterSni(typeof(SByte));
			RegiterSni(typeof(Byte));
			RegiterSni(typeof(Int16));
			RegiterSni(typeof(UInt16));
			RegiterSni(typeof(Int32));
			RegiterSni(typeof(UInt32));
			RegiterSni(typeof(Int64));
			RegiterSni(typeof(UInt64));
			RegiterSni(typeof(Single));
			RegiterSni(typeof(Double));
			RegiterSni(typeof(Decimal));
			RegiterSni(typeof(DateTime));
			RegiterSni(typeof(Object));
			RegiterSni(typeof(String));
			RegiterSni(typeof(Char));
			RegiterSni(typeof(Type));
			RegiterSni(typeof(Exception));
			RegiterSni(typeof(Math));
			RegiterSni(typeof(Random));
			RegiterSni(typeof(Convert));
			RegiterSni(typeof(Console));
			RegiterSni(typeof(File));
			RegiterSni(typeof(Directory));
			//TODO: add all native classes
		}

		private static void RegiterSni(Type type)
		{
			SharedNativeInvokers.Add(type.FullName, new NativeInvoker(type));
		}

		private void RegiterNi(Type type, NativeInvoker native)
		{
			_nativeInvokers.Add(type.FullName, native);
		}

		private NativeInvoker GetInvoker(IMethod method)
		{
			return GetInvoker(method.DeclaringType);
		}

		private NativeInvoker GetInvoker(Type type)
		{
			return GetInvoker(type.FullName);
		}

		private NativeInvoker GetInvoker(IType type)
		{
			if (type.IsArray)
				return GetInvoker(typeof(Array));
			if (type.TypeKind == TypeKind.Delegate)
				return GetInvoker(typeof(Delegate));
			return GetInvoker(type.FullName);
		}

		private NativeInvoker GetInvoker(string fullName)
		{
			NativeInvoker value;
			if (_nativeInvokers.TryGetValue(fullName, out value))
			{
				return value;
			}

			if (SharedNativeInvokers.TryGetValue(fullName, out value))
			{
				return value;
			}

			return null;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="VirtualMachine"/> class.
		/// </summary>
		public VirtualMachine() : this(null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="VirtualMachine"/> class.
		/// </summary>
		/// <param name="console">The optional console output.</param>
		public VirtualMachine(TextWriter console)
		{
			if (console != null)
			{
				RegiterNi(typeof(Console), new ConsoleInvoker(console));
			}

			RegiterNi(typeof(Array), new ArrayInvoker(this));
			RegiterNi(typeof(Delegate), new DelegateInvoker(this));
			RegiterNi(typeof(RuntimeHelpers), new RuntimeHelpersInvoker(this));
		}

		public int Run(string path, string options, string[] args)
		{
			CommonLanguageInfrastructure.ClearCache();

			var assembly = CommonLanguageInfrastructure.Deserialize(path, null);

			return Run(assembly, options, args);
		}

		public int Run(IAssembly assembly, string options, string[] args)
		{
			if (assembly == null) throw new ArgumentNullException("assembly");

			var method = assembly.EntryPoint;
			if (method == null)
			{
				throw new InvalidOperationException("The assembly is not executable. It is class library.");
			}

			object[] arguments = new object[] { args };

			GetClass(method.DeclaringType);
			var context = CreateContext(method, arguments);

			Call(context);

			// exit code
			if (context.ReturnValue is IConvertible)
			{
				return Convert.ToInt32(context.ReturnValue);
			}

			return 0;
		}

		private Class GetClass(IType type)
		{
			var klass = type.Tag as Class;
			if (klass != null)
			{
				return klass;
			}

			klass = new Class(type);
			type.Tag = klass;

			InitFields(klass);

			var cctor = type.Methods.StaticConstructor;
			if (cctor != null)
			{
				//TODO: emulate type init exception
				Call(cctor, new object[0]);
			}

			if (type.BaseType != null)
			{
				klass.BaseClass = GetClass(type.BaseType);
			}

			return klass;
		}

		internal void InitFields(IFieldStorage trait)
		{
			foreach (var slot in trait.Fields.Where(x => x.IsValueType))
			{
				slot.Value = InitObject(slot.Field.Type);
			}
		}

		private CallContext CreateContext(IMethod method, object[] args)
		{
			var body = method.Body as IClrMethodBody;
			if (body == null)
			{
				throw new NotSupportedException("Not supported method format.");
			}

			var context = new CallContext(this, method, GetLocals(body), args);

			//TODO: resolve generics

			return context;
		}

		private IReadOnlyList<ILocalVariable> GetLocals(IClrMethodBody body)
		{
			var vars = body.LocalVariables.Select(x => (ILocalVariable)new LocalVariable(x, InitObject)).AsReadOnlyList();
			//TODO: resolve generics
			return vars;
		}

		private object Call(CallContext context)
		{
			_callStack.Push(context);

			try
			{
				EvalWithSeh(context);

				return context.ReturnValue;
			}
			finally
			{
				_callStack.Pop();
			}
		}

		internal object Call(IMethod method, object[] args)
		{
			var context = CreateContext(method, args);
			return Call(context);
		}

		private void CallPush(CallContext context, IMethod method, object[] args)
		{
			object result = Call(method, args);

			if (!method.IsVoid())
			{
				context.Push(result);
			}
		}

		private void EvalWithSeh(CallContext context)
		{
			try
			{
				Eval(context);
			}
			catch (Exception e)
			{
				context.Exception = e;

				var tryCatch = FindTryCatchBlock(context);
				if (tryCatch != null)
				{
					// now find exception handler
					var handler = FindSeh(tryCatch, e);
					if (handler != null)
					{
						// prepare stack
						context.Clear();
						context.Push(e);

						context.Handler = handler;
						context.IP = handler.EntryIndex;

						EvalWithSeh(context);
					}
				}
				else
				{
					// the block is not protected
					throw;
				}
			}
		}

		private static TryCatchBlock FindTryCatchBlock(CallContext context)
		{
			if (!context.Body.HasProtectedBlocks) return null;

			TryCatchBlock tryCatch = null;

			foreach (var block in context.Body.GetAllProtectedBlocks())
			{
				if (block.EntryIndex <= context.IP && context.IP <= block.ExitIndex)
				{
					if (tryCatch == null)
					{
						tryCatch = block;
					}
					else if (block.EntryIndex >= tryCatch.EntryIndex)
					{
						tryCatch = block;
					}
				}
			}

			return tryCatch;
		}

		private static HandlerBlock FindSeh(TryCatchBlock block, Exception e)
		{
			var instance = e as Instance;
			foreach (var handler in block.Handlers.Cast<HandlerBlock>())
			{
				switch (handler.Type)
				{
					case BlockType.Finally:
					case BlockType.Fault:
						return handler;
					case BlockType.Catch:
						if (instance != null && instance.IsInstanceOf(handler.ExceptionType))
						{
							return handler;
						}
						if (e.GetType().IsInstanceOf(handler.ExceptionType))
						{
							return handler;
						}
						break;
					case BlockType.Filter:
					default:
						throw new NotImplementedException();
				}
			}
			return null;
		}

		private void Eval(CallContext context)
		{
			while (context.IP < context.Code.Count)
			{
				EvalInstruction(context);
			}
		}

		private void EvalInstruction(CallContext context)
		{
			var i = context.Code[context.IP];

			switch (i.Code)
			{
				#region constants
				case InstructionCode.Ldnull:
					context.Push(null);
					break;
				case InstructionCode.Ldc_I4_M1:
					context.Push(-1);
					break;
				case InstructionCode.Ldc_I4_0:
					context.Push(0);
					break;
				case InstructionCode.Ldc_I4_1:
					context.Push(1);
					break;
				case InstructionCode.Ldc_I4_2:
					context.Push(2);
					break;
				case InstructionCode.Ldc_I4_3:
					context.Push(3);
					break;
				case InstructionCode.Ldc_I4_4:
					context.Push(4);
					break;
				case InstructionCode.Ldc_I4_5:
					context.Push(5);
					break;
				case InstructionCode.Ldc_I4_6:
					context.Push(6);
					break;
				case InstructionCode.Ldc_I4_7:
					context.Push(7);
					break;
				case InstructionCode.Ldc_I4_8:
					context.Push(8);
					break;
				case InstructionCode.Ldc_I4_S:
					context.Push((int)i.Value);
					break;
				case InstructionCode.Ldc_I4:
					context.Push((int)i.Value);
					break;
				case InstructionCode.Ldc_I8:
					context.Push((long)i.Value);
					break;
				case InstructionCode.Ldc_R4:
					context.Push((float)i.Value);
					break;
				case InstructionCode.Ldc_R8:
					context.Push((double)i.Value);
					break;
				case InstructionCode.Ldstr:
					context.Push(i.Value);
					break;
				#endregion

				#region load instructions
				case InstructionCode.Ldloc_0:
					context.LoadLocal(0);
					break;
				case InstructionCode.Ldloc_1:
					context.LoadLocal(1);
					break;
				case InstructionCode.Ldloc_2:
					context.LoadLocal(2);
					break;
				case InstructionCode.Ldloc_3:
					context.LoadLocal(3);
					break;
				case InstructionCode.Ldloc_S:
				case InstructionCode.Ldloc:
					context.LoadLocal((int)i.Value);
					break;
				case InstructionCode.Ldloca_S:
				case InstructionCode.Ldloca:
					context.LoadLocalPtr((int)i.Value);
					break;
				case InstructionCode.Ldarg_0:
					context.LoadArg(0);
					break;
				case InstructionCode.Ldarg_1:
					context.LoadArg(1);
					break;
				case InstructionCode.Ldarg_2:
					context.LoadArg(2);
					break;
				case InstructionCode.Ldarg_3:
					context.LoadArg(3);
					break;
				case InstructionCode.Ldarg_S:
				case InstructionCode.Ldarg:
					context.LoadArg((int)i.Value);
					break;
				case InstructionCode.Ldarga_S:
				case InstructionCode.Ldarga:
					context.LoadArgPtr((int)i.Value);
					break;

				case InstructionCode.Ldfld:
				case InstructionCode.Ldsfld:
					{
						var field = i.Field;
						if (field.IsStatic)
						{
							context.Push(GetClass(field).Fields[field.Slot].Value);
						}
						else
						{
							var instance = context.PopInstance();
							context.Push(instance.Fields[field.Slot].Value);
						}
					}
					break;

				case InstructionCode.Ldflda:
				case InstructionCode.Ldsflda:
					{
						var field = i.Field;
						if (field.IsStatic)
						{
							var klass = GetClass(field);
							context.Push(new FieldPtr(klass, field));
						}
						else
						{
							var instance = context.PopInstance();
							context.Push(new FieldPtr(instance, field));
						}
					}
					break;

				case InstructionCode.Ldind_I1:
				case InstructionCode.Ldind_U1:
				case InstructionCode.Ldind_I2:
				case InstructionCode.Ldind_U2:
				case InstructionCode.Ldind_I4:
				case InstructionCode.Ldind_U4:
				case InstructionCode.Ldind_I8:
				case InstructionCode.Ldind_I:
				case InstructionCode.Ldind_R4:
				case InstructionCode.Ldind_R8:
				case InstructionCode.Ldind_Ref:
					{
						var ptr = context.PopPtr();
						context.Push(ptr.Value);
					}
					break;

				case InstructionCode.Ldftn:
					context.Push(new MethodPtr(i.Method));
					break;
				case InstructionCode.Ldvirtftn:
					Op_Ldvirtftn(context, i.Method);
					break;

				case InstructionCode.Ldtoken:
					context.Push(i.Member);
					break;

				case InstructionCode.Ldobj:
					{
						var ptr = context.PopPtr();
						context.Push(ptr.Value);
					}
					break;
				#endregion

				#region store instructions
				case InstructionCode.Stloc_0:
					context.StoreLocal(0);
					break;
				case InstructionCode.Stloc_1:
					context.StoreLocal(1);
					break;
				case InstructionCode.Stloc_2:
					context.StoreLocal(2);
					break;
				case InstructionCode.Stloc_3:
					context.StoreLocal(3);
					break;
				case InstructionCode.Stloc_S:
				case InstructionCode.Stloc:
					context.StoreLocal((int)i.Value);
					break;

				case InstructionCode.Starg_S:
				case InstructionCode.Starg:
					context.StoreArg((int)i.Value);
					break;

				case InstructionCode.Stfld:
					{
						var value = context.Pop(true);
						var instance = context.PopInstance();
						instance.Fields[i.Field.Slot].Value = value;
					}
					break;
				case InstructionCode.Stsfld:
					{
						var value = context.Pop(true);
						GetClass(i.Field).Fields[i.Field.Slot].Value = value;
					}
					break;

				case InstructionCode.Stind_Ref:
				case InstructionCode.Stind_I1:
				case InstructionCode.Stind_I2:
				case InstructionCode.Stind_I4:
				case InstructionCode.Stind_I8:
				case InstructionCode.Stind_R4:
				case InstructionCode.Stind_R8:
				case InstructionCode.Stind_I:
					{
						var value = context.Pop(true);
						var ptr = context.PopPtr();
						ptr.Value = value;
					}
					break;

				case InstructionCode.Stobj:
					{
						var value = context.Pop(true);
						var ptr = context.PopPtr();
						ptr.Value = value;
					}
					break;
				#endregion

				case InstructionCode.Cpobj:
					{
						var dest = context.PopPtr();
						var src = context.PopPtr();
						var value = ConvertTo(src.Value.Copy(), i.Type);
						dest.Value = value;
					}
					break;

				#region stack operations
				case InstructionCode.Dup:
					context.Dup(true);
					break;
				case InstructionCode.Pop:
					context.Pop(false);
					break;
				#endregion

				#region call instructions
				case InstructionCode.Call:
					Op_Call(context, i.Method, false);
					break;
				case InstructionCode.Callvirt:
					Op_Call(context, i.Method, true);
					break;

				//call of function onto the stack
				case InstructionCode.Calli:
					CallIndirect(context);
					break;

				case InstructionCode.Newobj:
					NewObject(context, i.Method);
					break;
				case InstructionCode.Initobj:
					Op_Initobj(context, i.Type);
					break;
				#endregion

				#region branches, switch

				case InstructionCode.Brfalse_S:
				case InstructionCode.Brfalse:
					if (IsBranch(context, BranchOperator.False, false))
					{
						context.IP = i.BranchTarget;
						return;
					}
					break;

				case InstructionCode.Brtrue_S:
				case InstructionCode.Brtrue:
					if (IsBranch(context, BranchOperator.True, false))
					{
						context.IP = i.BranchTarget;
						return;
					}
					break;

				case InstructionCode.Beq_S:
				case InstructionCode.Beq:
					if (IsBranch(context, BranchOperator.Equality, false))
					{
						context.IP = i.BranchTarget;
						return;
					}
					break;

				case InstructionCode.Bge_S:
				case InstructionCode.Bge:
					if (IsBranch(context, BranchOperator.GreaterThanOrEqual, false))
					{
						context.IP = i.BranchTarget;
						return;
					}
					break;

				case InstructionCode.Bgt_S:
				case InstructionCode.Bgt:
					if (IsBranch(context, BranchOperator.GreaterThan, false))
					{
						context.IP = i.BranchTarget;
						return;
					}
					break;

				case InstructionCode.Ble_S:
				case InstructionCode.Ble:
					if (IsBranch(context, BranchOperator.LessThanOrEqual, false))
					{
						context.IP = i.BranchTarget;
						return;
					}
					break;

				case InstructionCode.Blt_S:
				case InstructionCode.Blt:
					if (IsBranch(context, BranchOperator.LessThan, false))
					{
						context.IP = i.BranchTarget;
						return;
					}
					break;

				case InstructionCode.Bne_Un_S:
				case InstructionCode.Bne_Un:
					if (IsBranch(context, BranchOperator.Inequality, true))
					{
						context.IP = i.BranchTarget;
						return;
					}
					break;

				case InstructionCode.Bge_Un_S:
				case InstructionCode.Bge_Un:
					if (IsBranch(context, BranchOperator.GreaterThanOrEqual, true))
					{
						context.IP = i.BranchTarget;
						return;
					}
					break;

				case InstructionCode.Bgt_Un_S:
				case InstructionCode.Bgt_Un:
					if (IsBranch(context, BranchOperator.GreaterThan, true))
					{
						context.IP = i.BranchTarget;
						return;
					}
					break;

				case InstructionCode.Ble_Un_S:
				case InstructionCode.Ble_Un:
					if (IsBranch(context, BranchOperator.LessThanOrEqual, true))
					{
						context.IP = i.BranchTarget;
						return;
					}
					break;

				case InstructionCode.Blt_Un_S:
				case InstructionCode.Blt_Un:
					if (IsBranch(context, BranchOperator.LessThan, true))
					{
						context.IP = i.BranchTarget;
						return;
					}
					break;

				case InstructionCode.Br_S:
				case InstructionCode.Br:
					context.IP = i.BranchTarget;
					return;

				case InstructionCode.Switch:
					Op_Switch(context, (int[])i.Value);
					return;

				#endregion

				#region binary, unary arithmetic operations
				//arithmetic operations
				// a + b
				case InstructionCode.Add:
					Op(context, BinaryOperator.Addition, false, false);
					break;
				case InstructionCode.Add_Ovf:
					Op(context, BinaryOperator.Addition, false, true);
					break;
				case InstructionCode.Add_Ovf_Un:
					Op(context, BinaryOperator.Addition, true, true);
					break;

				// a - b
				case InstructionCode.Sub:
					Op(context, BinaryOperator.Subtraction, false, false);
					break;
				case InstructionCode.Sub_Ovf:
					Op(context, BinaryOperator.Subtraction, false, true);
					break;
				case InstructionCode.Sub_Ovf_Un:
					Op(context, BinaryOperator.Subtraction, true, true);
					break;

				// a * b
				case InstructionCode.Mul:
					Op(context, BinaryOperator.Multiply, false, false);
					break;
				case InstructionCode.Mul_Ovf:
					Op(context, BinaryOperator.Multiply, false, true);
					break;
				case InstructionCode.Mul_Ovf_Un:
					Op(context, BinaryOperator.Multiply, true, true);
					break;

				// a / b
				case InstructionCode.Div:
					Op(context, BinaryOperator.Division, false, false);
					break;
				case InstructionCode.Div_Un:
					Op(context, BinaryOperator.Division, true, false);
					break;

				// a % b
				case InstructionCode.Rem:
					Op(context, BinaryOperator.Modulus, false, false);
					break;
				case InstructionCode.Rem_Un:
					Op(context, BinaryOperator.Modulus, true, false);
					break;

				//bitwise operations
				// a & b
				case InstructionCode.And:
					Op(context, BinaryOperator.BitwiseAnd, false, false);
					break;
				// a | b
				case InstructionCode.Or:
					Op(context, BinaryOperator.BitwiseOr, false, false);
					break;
				// a ^ b
				case InstructionCode.Xor:
					Op(context, BinaryOperator.ExclusiveOr, false, false);
					break;
				// a << b
				case InstructionCode.Shl:
					Op(context, BinaryOperator.LeftShift, false, false);
					break;
				// a >> b
				case InstructionCode.Shr:
					Op(context, BinaryOperator.RightShift, false, false);
					break;
				case InstructionCode.Shr_Un:
					Op(context, BinaryOperator.RightShift, true, false);
					break;

				//unary operations
				case InstructionCode.Neg:
					Op(context, UnaryOperator.Negate, false);
					break;
				case InstructionCode.Not:
					Op(context, UnaryOperator.BitwiseNot, false);
					break;

				//relation operations
				// a == b
				case InstructionCode.Ceq:
					Op(context, BinaryOperator.Equality, false, false);
					break;
				// a > b
				case InstructionCode.Cgt:
					Op(context, BinaryOperator.GreaterThan, false, false);
					break;
				case InstructionCode.Cgt_Un:
					Op(context, BinaryOperator.GreaterThan, true, false);
					break;
				// a < b
				case InstructionCode.Clt:
					Op(context, BinaryOperator.LessThan, false, false);
					break;
				case InstructionCode.Clt_Un:
					Op(context, BinaryOperator.LessThan, true, false);
					break;
				#endregion

				#region conversion instructions
				//TODO: IntPtr, UIntPtr is not supported
				case InstructionCode.Conv_I1:
					ConvertTo(context, SystemTypes.Int8, false, false);
					break;
				case InstructionCode.Conv_I2:
					ConvertTo(context, SystemTypes.Int16, false, false);
					break;
				case InstructionCode.Conv_I4:
					ConvertTo(context, SystemTypes.Int32, false, false);
					break;
				case InstructionCode.Conv_I8:
					ConvertTo(context, SystemTypes.Int64, false, false);
					break;
				case InstructionCode.Conv_R4:
					ConvertTo(context, SystemTypes.Float32, false, false);
					break;
				case InstructionCode.Conv_R8:
					ConvertTo(context, SystemTypes.Float64, false, false);
					break;

				case InstructionCode.Conv_U1:
					ConvertTo(context, SystemTypes.UInt8, false, false);
					break;
				case InstructionCode.Conv_U2:
					ConvertTo(context, SystemTypes.UInt16, false, false);
					break;
				case InstructionCode.Conv_U4:
					ConvertTo(context, SystemTypes.UInt32, false, false);
					break;
				case InstructionCode.Conv_U8:
					ConvertTo(context, SystemTypes.UInt64, false, false);
					break;

				case InstructionCode.Conv_Ovf_I1_Un:
					ConvertTo(context, SystemTypes.Int8, true, true);
					break;
				case InstructionCode.Conv_Ovf_I2_Un:
					ConvertTo(context, SystemTypes.Int16, true, true);
					break;
				case InstructionCode.Conv_Ovf_I4_Un:
					ConvertTo(context, SystemTypes.Int32, true, true);
					break;
				case InstructionCode.Conv_Ovf_I8_Un:
					ConvertTo(context, SystemTypes.Int64, true, true);
					break;
				case InstructionCode.Conv_Ovf_U1_Un:
					ConvertTo(context, SystemTypes.UInt8, true, true);
					break;
				case InstructionCode.Conv_Ovf_U2_Un:
					ConvertTo(context, SystemTypes.UInt16, true, true);
					break;
				case InstructionCode.Conv_Ovf_U4_Un:
					ConvertTo(context, SystemTypes.UInt32, true, true);
					break;
				case InstructionCode.Conv_Ovf_U8_Un:
					ConvertTo(context, SystemTypes.UInt64, true, true);
					break;
				case InstructionCode.Conv_Ovf_I_Un:
					ConvertTo(context, SystemTypes.NativeInt, true, true);
					break;
				case InstructionCode.Conv_Ovf_U_Un:
					ConvertTo(context, SystemTypes.NativeUInt, true, true);
					break;
				case InstructionCode.Conv_Ovf_I1:
					ConvertTo(context, SystemTypes.Int8, true, false);
					break;
				case InstructionCode.Conv_Ovf_U1:
					ConvertTo(context, SystemTypes.UInt8, true, false);
					break;
				case InstructionCode.Conv_Ovf_I2:
					ConvertTo(context, SystemTypes.Int16, true, false);
					break;
				case InstructionCode.Conv_Ovf_U2:
					ConvertTo(context, SystemTypes.UInt16, true, false);
					break;
				case InstructionCode.Conv_Ovf_I4:
					ConvertTo(context, SystemTypes.Int32, true, false);
					break;
				case InstructionCode.Conv_Ovf_U4:
					ConvertTo(context, SystemTypes.UInt32, true, false);
					break;
				case InstructionCode.Conv_Ovf_I8:
					ConvertTo(context, SystemTypes.Int64, true, false);
					break;
				case InstructionCode.Conv_Ovf_U8:
					ConvertTo(context, SystemTypes.UInt64, true, false);
					break;

				case InstructionCode.Conv_I:
					ConvertTo(context, SystemTypes.NativeInt, false, false);
					break;
				case InstructionCode.Conv_Ovf_I:
					ConvertTo(context, SystemTypes.NativeInt, true, false);
					break;
				case InstructionCode.Conv_Ovf_U:
					ConvertTo(context, SystemTypes.NativeUInt, true, false);
					break;
				case InstructionCode.Conv_U:
					ConvertTo(context, SystemTypes.NativeUInt, false, false);
					break;
				case InstructionCode.Conv_R_Un:
					ConvertTo(context, SystemTypes.Float32, false, true);
					break;
				#endregion

				#region cast operations
				case InstructionCode.Castclass:
					Op_Castclass(context, i.Type);
					break;

				case InstructionCode.Isinst:
					Op_Isinst(context, i.Type);
					break;

				case InstructionCode.Box:
					ConvertTo(context, i.Type, false, false);
					break;

				//NOTE:
				//The constrained. prefix is permitted only on a callvirt instruction.
				//The type of ptr must be a managed pointer (&) to thisType.
				//The constrained prefix is designed to allow callvirt instructions to be made in a
				//uniform way independent of whether thisType is a value type or a reference type.
				case InstructionCode.Constrained:
					break;

				case InstructionCode.Unbox:
				case InstructionCode.Unbox_Any:
					ConvertTo(context, i.Type, false, false);
					break;
				#endregion

				#region array instructions
				case InstructionCode.Newarr:
					NewArray(context, i.Type);
					break;
				case InstructionCode.Ldlen:
					context.Push(context.PopArray().Length);
					break;

				case InstructionCode.Ldelema:
					{
						var index = Convert.ToInt64(context.Pop(false));
						var array = context.PopArray();
						CheckBounds(array, index);
						context.Push(new ArrayElementPtr(array, index));
					}
					break;

				case InstructionCode.Ldelem:
				case InstructionCode.Ldelem_I1:
				case InstructionCode.Ldelem_U1:
				case InstructionCode.Ldelem_I2:
				case InstructionCode.Ldelem_U2:
				case InstructionCode.Ldelem_I4:
				case InstructionCode.Ldelem_U4:
				case InstructionCode.Ldelem_I8:
				case InstructionCode.Ldelem_I:
				case InstructionCode.Ldelem_R4:
				case InstructionCode.Ldelem_R8:
				case InstructionCode.Ldelem_Ref:
					{
						var index = Convert.ToInt64(context.Pop(false));
						var array = context.PopArray();
						CheckBounds(array, index);
						context.Push(array.GetValue(index));
					}
					break;

				case InstructionCode.Stelem_I:
				case InstructionCode.Stelem_I1:
				case InstructionCode.Stelem_I2:
				case InstructionCode.Stelem_I4:
				case InstructionCode.Stelem_I8:
				case InstructionCode.Stelem_R4:
				case InstructionCode.Stelem_R8:
				case InstructionCode.Stelem_Ref:
				case InstructionCode.Stelem:
					{
						var value = context.Pop(true);
						var index = Convert.ToInt64(context.Pop(false));
						var array = context.PopArray();
						CheckBounds(array, index);
						array.SetValue(value, index);
					}
					break;
				#endregion

				#region exception handling
				case InstructionCode.Throw:
					throw (context.PopObject() as Exception);

				case InstructionCode.Rethrow:
					throw context.Exception;

				case InstructionCode.Leave:
				case InstructionCode.Leave_S:
					context.IP = i.BranchTarget;
					return;

				case InstructionCode.Endfinally:
					Op_Endfinally(context);
					break;

				case InstructionCode.Endfilter:
					Op_Endfilter(context);
					break;
				#endregion

				#region misc
				case InstructionCode.Nop:
					break;

				case InstructionCode.Break:
					break;

				case InstructionCode.Ret:
					context.Return();
					return;

				case InstructionCode.Sizeof:
					Op_Sizeof(context);
					break;
				#endregion

				#region not supported instructions
				case InstructionCode.Arglist:
				case InstructionCode.Cpblk:
				case InstructionCode.Initblk:
				case InstructionCode.Ckfinite:
				case InstructionCode.Localloc:
				case InstructionCode.Jmp:
				
				//push a typed reference on the stack
				//stack [ptr -> typedRef]
				case InstructionCode.Mkrefany:

				//load the address out of a typed reference
				//stack [typedRef -> ptr]
				case InstructionCode.Refanyval:

				//load the type out of a typed reference
				//stack [typedRef -> type]
				case InstructionCode.Refanytype:
					throw new NotSupportedException();
				#endregion

				#region ignored instructions
				case InstructionCode.Prefix7:
				case InstructionCode.Prefix6:
				case InstructionCode.Prefix5:
				case InstructionCode.Prefix4:
				case InstructionCode.Prefix3:
				case InstructionCode.Prefix2:
				case InstructionCode.Prefix1:
				case InstructionCode.Prefixref:
				case InstructionCode.Readonly:
				case InstructionCode.Unaligned:
				case InstructionCode.Volatile:
				case InstructionCode.Tailcall:
					//TODO: Log warning
					break;
				#endregion

				default:
					throw new ArgumentOutOfRangeException();
			}

			context.IP++;
		}

		internal Class GetClass(ITypeMember member)
		{
			return GetClass(member.DeclaringType);
		}

		private void Op_Ldvirtftn(CallContext context, IMethod method)
		{
			if (method.IsStatic)
			{
				throw new InvalidOperationException("Static method is not valid in this context.");
			}

			//TODO: vtable

			var obj = context.PopObject();
			
			var instance = obj as Instance;
			if (instance != null)
			{
				var impl = FindImpl(instance.Type, method);
				context.Push(new MethodPtr(impl));
			}
			else
			{
				throw new NotImplementedException();
			}
		}

		private static bool IsBranch(CallContext context, BranchOperator op, bool unsigned)
		{
			object x, y;
			switch (op)
			{
				case BranchOperator.True:
					x = context.Pop();
					return x.IsTrue();
				case BranchOperator.False:
					x = context.Pop();
					return !x.IsTrue();
				case BranchOperator.Null:
					x = context.Pop();
					return x == null;
				case BranchOperator.NotNull:
					x = context.Pop();
					return x != null;
				case BranchOperator.Equality:
					y = context.Pop();
					x = context.Pop();
					return Compare(x, y, unsigned) == 0;
				case BranchOperator.Inequality:
					y = context.Pop();
					x = context.Pop();
					return Compare(x, y, unsigned) != 0;
				case BranchOperator.LessThan:
					y = context.Pop();
					x = context.Pop();
					return Compare(x, y, unsigned) < 0;
				case BranchOperator.LessThanOrEqual:
					y = context.Pop();
					x = context.Pop();
					return Compare(x, y, unsigned) <= 0;
				case BranchOperator.GreaterThan:
					y = context.Pop();
					x = context.Pop();
					return Compare(x, y, unsigned) > 0;
				case BranchOperator.GreaterThanOrEqual:
					y = context.Pop();
					x = context.Pop();
					return Compare(x, y, unsigned) >= 0;
				default:
					throw new ArgumentOutOfRangeException("op");
			}
		}

		private static int Compare(object x, object y, bool unsigned)
		{
			if (x == null)
			{
				if (y == null) return 0;
				return -1;
			}

			if (y == null) return 1;

			if (!x.IsNumeric() || !y.IsNumeric())
			{
				//TODO: use comparer for same types
				return Equals(x, y) ? 0 : 1;
			}
			
			if (unsigned)
			{
				x = x.ToUnsigned();
				y = y.ToUnsigned();
			}

			var c = Convert.ToDecimal(Arithmetic.Subtract(x, y));
			return c > 0 ? 1 : c < 0 ? -1 : 0;
		}

		private void Op_Switch(CallContext context, int[] targets)
		{
			var value = context.Pop();
			var index = Convert.ToInt32(value);
			if (index >= 0 && index < targets.Length)
			{
				var target = targets[index];
				context.IP = target;
			}
			else
			{
				context.IP++;
			}
		}

		private object[] PopArgs(CallContext context, IMethod method)
		{
			object[] args = new object[method.Parameters.Count];
			PopArgs(context, method, args);
			return args;
		}

		private void PopArgs(CallContext context, IMethod method, object[] args)
		{
			for (int i = args.Length - 1, j = method.Parameters.Count - 1; j >= 0; i--, j--)
			{
				var value = context.Pop();
				value = ConvertTo(value, method.Parameters[j].Type);
				args[i] = value;
			}
		}

		private object PopInstance(CallContext context, IMethod method)
		{
			var instance = context.PopObject();
			return ConvertTo(instance, method.DeclaringType);
		}

		private void Op_Call(CallContext context, IMethod method, bool virtcall)
		{
			if (method.IsConstructor && method.DeclaringType == SystemTypes.Object)
			{
				return;
			}

			var declType = method.DeclaringType;
			if (declType == SystemTypes.Object)
			{
				if (method.Name == "GetType")
				{
					GetTypeImpl(context);
					return;
				}

				if (method.Name == "MemberwiseClone")
				{
					MemberwiseCloneImpl(context);
					return;
				}
			}

			var native = GetInvoker(method);
			if (native != null)
			{
				object[] args = PopArgs(context, method);

				object instance = null;
				if (!method.IsStatic)
				{
					instance = PopInstance(context, method);
				}

				var result = native.Invoke(method, instance, args);

				if (!method.IsVoid())
				{
					context.Push(result);
				}

				return;
			}

			if (method.IsStatic)
			{
				CallStatic(context, method);
			}
			else
			{
				CallInstance(context, method, virtcall);
			}
		}

		private static void MemberwiseCloneImpl(CallContext context)
		{
			var obj = context.PopInstance();
			obj = obj.Copy();
			context.Push(obj);
		}

		private static void GetTypeImpl(CallContext context)
		{
			var obj = context.PopObject();
			
			var instance = obj as Instance;
			if (instance != null)
			{
				context.Push(instance.Class.SystemType);
			}
			else
			{
				context.Push(obj.GetType());
			}
		}

		internal Type TypeOf(IType type)
		{
			var native = GetInvoker(type);
			if (native != null)
			{
				return native.Type;
			}
			
			var klass = GetClass(type);
			return klass.SystemType;
		}

		internal object Call(IMethod method, object instance, object[] args, bool virtcall)
		{
			if (method.IsStatic)
			{
				GetClass(method);

				return Call(method, args);
			}

			instance = ConvertTo(instance, method.DeclaringType);

			var all = new object[args.Length + 1];
			Array.Copy(args, 0, all, 1, args.Length);
			all[0] = instance;

			return CallInstance(method, all, virtcall);
		}

		private void CallStatic(CallContext context, IMethod method)
		{
			GetClass(method);

			var args = PopArgs(context, method);
			
			CallPush(context, method, args);
		}

		private void CallInstance(CallContext context, IMethod method, bool virtcall)
		{
			var args = new object[method.Parameters.Count + 1];
			PopArgs(context, method, args);
			args[0] = PopInstance(context, method);

			if (method.IsConstructor)
			{
				CallPush(context, method, args);
			}
			else
			{
				var result = CallInstance(method, args, virtcall);

				if (!method.IsVoid())
				{
					context.Push(result);
				}
			}
		}

		internal object CallInstance(IMethod method, object[] args, bool virtcall)
		{
			var obj = args[0];

			var invoker = GetInvoker(obj.GetType());
			if (invoker != null)
			{
				object[] copy = new object[args.Length - 1];
				Array.Copy(args, 1, copy, 0, args.Length - 1);

				return invoker.Invoke(method, obj, copy);
			}

			//TODO: introduce vtable to optimize virtual call
			var instance = obj as Instance;

			var type = instance.Type;

			bool basecall = false;
			if (!virtcall && type != method.DeclaringType
				&& !method.IsStatic && !method.IsConstructor)
				basecall = type.IsSubclassOf(method.DeclaringType);

			if (!basecall && virtcall && (method.IsAbstract || method.IsVirtual))
			{
				var impl = FindImpl(type, method);
				if (impl != null)
				{
					method = impl;
				}
			}

			return Call(method, args);
		}

		private static IMethod FindImpl(IType type, IMethod method)
		{
			if (method.DeclaringType.IsInterface)
			{
				return type.FindImplementation(method);
			}

			while (type != null)
			{
				foreach (var typeMethod in type.Methods)
				{
					if (Signature.Equals(typeMethod, method, false))
					{
						return typeMethod;
					}
				}
				type = type.BaseType;
			}

			return null;
		}

		private void CallIndirect(CallContext context)
		{
			var f = context.Pop(false) as MethodPtr;

			int n = f.Method.Parameters.Count;

			if (!f.Method.IsStatic)
			{
				n++;
			}

			var args = new object[n];
			PopArgs(context, f.Method, args);
			
			if (!f.Method.IsStatic)
			{
				args[0] = context.PopObject();
			}

			CallPush(context, f.Method, args);
		}

		private void NewObject(CallContext context, IMethod ctor)
		{
			var nativeClass = GetInvoker(ctor);
			if (nativeClass != null)
			{
				var args = PopArgs(context, ctor);
				var obj = nativeClass.Invoke(ctor, null, args);
				context.Push(obj);
				return;
			}
			else
			{
				var klass = GetClass(ctor);
				var instance = new Instance(this, klass);

				var args = new object[1 + ctor.Parameters.Count];
				PopArgs(context, ctor, args);
				args[0] = instance;

				Call(ctor, args);

				context.Push(instance);
			}
		}

		internal object InitObject(IType type)
		{
			var invoker = GetInvoker(type);
			if (invoker != null)
			{
				var obj = invoker.Invoke(type.FindParameterlessConstructor(), null, new object[0]);
				return obj;
			}

			var klass = GetClass(type);
			var instance = new Instance(this, klass);

			//TODO: init value type fields
			var ctor = type.FindParameterlessConstructor();
			if (ctor != null)
			{
				Call(ctor, new object[] { instance });
			}

			return instance;
		}

		private void Op_Initobj(CallContext context, IType type)
		{
			var addr = context.PopPtr();
			
			addr.Value = InitObject(type);
		}

		private void Op(CallContext context, BinaryOperator op, bool unsigned, bool checkOverflow)
		{
			if (checkOverflow)
			{
				//throw new NotImplementedException();
			}

			var y = context.Pop();
			var x = context.Pop();

			if (unsigned)
			{
				x = x.ToUnsigned();
				y = y.ToUnsigned();
			}

			switch (op)
			{
				case BinaryOperator.Addition:
					context.Push(Arithmetic.Add(x, y));
					break;
				case BinaryOperator.Subtraction:
					context.Push(Arithmetic.Subtract(x, y));
					break;
				case BinaryOperator.Multiply:
					context.Push(Arithmetic.Multiply(x, y));
					break;
				case BinaryOperator.Division:
					context.Push(Arithmetic.Divide(x, y));
					break;
				case BinaryOperator.Modulus:
					context.Push(Arithmetic.Modulus(x, y));
					break;
				case BinaryOperator.LeftShift:
					context.Push(Arithmetic.LeftShift(x, y));
					break;
				case BinaryOperator.RightShift:
					context.Push(Arithmetic.RightShift(x, y));
					break;
				case BinaryOperator.Equality:
					context.Push(Compare(x, y, unsigned) == 0);
					break;
				case BinaryOperator.Inequality:
					context.Push(Compare(x, y, unsigned) != 0);
					break;
				case BinaryOperator.BitwiseOr:
					context.Push(Arithmetic.BitwiseOr(x, y));
					break;
				case BinaryOperator.BitwiseAnd:
					context.Push(Arithmetic.BitwiseAnd(x, y));
					break;
				case BinaryOperator.ExclusiveOr:
					context.Push(Arithmetic.BitwiseXor(x, y));
					break;
				case BinaryOperator.BooleanOr:
					context.Push(y.IsTrue() || x.IsTrue());
					break;
				case BinaryOperator.BooleanAnd:
					context.Push(y.IsTrue() && x.IsTrue());
					break;
				case BinaryOperator.LessThan:
					context.Push(Compare(x, y, unsigned) < 0);
					break;
				case BinaryOperator.LessThanOrEqual:
					context.Push(Compare(x, y, unsigned) <= 0);
					break;
				case BinaryOperator.GreaterThan:
					context.Push(Compare(x, y, unsigned) > 0);
					break;
				case BinaryOperator.GreaterThanOrEqual:
					context.Push(Compare(x, y, unsigned) >= 0);
					break;
				default:
					throw new ArgumentOutOfRangeException("op");
			}
		}

		private void Op(CallContext context, UnaryOperator op, bool checkOverflow)
		{
			var x = context.Pop();
			switch (op)
			{
				case UnaryOperator.Negate:
					context.Push(Arithmetic.Negate(x, checkOverflow));
					break;
				case UnaryOperator.BooleanNot:
					context.Push(!x.IsTrue());
					break;
				case UnaryOperator.BitwiseNot:
					context.Push(Arithmetic.BitwiseNot(x, checkOverflow));
					break;
				default:
					throw new ArgumentOutOfRangeException("op");
			}
		}

		private void ConvertTo(CallContext context, IType targetType, bool checkOverflow, bool unsigned)
		{
			var value = context.Pop();

			if (unsigned)
			{
				value = value.ToUnsigned();
			}

			value = ConvertTo(value, targetType);

			context.Push(value);
		}

		private object ConvertTo(object value, IType type)
		{
			switch (type.GetTypeCode())
			{
				case TypeCode.Boolean:
					return Convert.ToBoolean(value);
				case TypeCode.Char:
					return Convert.ToChar(value);
				case TypeCode.SByte:
					return Convert.ToSByte(value);
				case TypeCode.Byte:
					return Convert.ToByte(value.ToUnsigned());
				case TypeCode.Int16:
					return Convert.ToInt16(value);
				case TypeCode.UInt16:
					return Convert.ToUInt16(value.ToUnsigned());
				case TypeCode.Int32:
					return Convert.ToInt32(value);
				case TypeCode.UInt32:
					return Convert.ToUInt32(value.ToUnsigned());
				case TypeCode.Int64:
					return Convert.ToInt64(value);
				case TypeCode.UInt64:
					return Convert.ToUInt64(value.ToUnsigned());
				case TypeCode.Single:
					return Convert.ToSingle(value);
				case TypeCode.Double:
					return Convert.ToDouble(value);
				case TypeCode.Decimal:
					return Convert.ToDecimal(value);
				case TypeCode.String:
					return Convert.ToString(value);
				default:
					//TODO: cast exception
					return value;
			}
		}

		private void Op_Castclass(CallContext context, IType type)
		{
			//TODO: array, string interfaces

			var value = context.PopObject();
			if (value == null)
			{
				context.Push(value);
				return;
			}

			//TODO: casting of delegates
			if (value is DelegateImpl)
			{
				context.Push(value);
				return;
			}

			var instance = value as Instance;
			if (instance != null)
			{
				if (!instance.IsInstanceOf(type))
					throw new InvalidCastException(string.Format("Cannot cast to {0}", type.FullName));
				context.Push(value);
			}
			else
			{
				switch (type.GetTypeCode())
				{
					case TypeCode.Boolean:
						context.Push((Boolean)value);
						break;
					case TypeCode.Char:
						context.Push((Char)value);
						break;
					case TypeCode.SByte:
						context.Push((SByte)value);
						break;
					case TypeCode.Byte:
						context.Push((Byte)value);
						break;
					case TypeCode.Int16:
						context.Push((Int16)value);
						break;
					case TypeCode.UInt16:
						context.Push((UInt16)value);
						break;
					case TypeCode.Int32:
						context.Push((Int32)value);
						break;
					case TypeCode.UInt32:
						context.Push((UInt32)value);
						break;
					case TypeCode.Int64:
						context.Push((Int64)value);
						break;
					case TypeCode.UInt64:
						context.Push((UInt64)value);
						break;
					case TypeCode.Single:
						context.Push((Single)value);
						break;
					case TypeCode.Double:
						context.Push((Double)value);
						break;
					case TypeCode.Decimal:
						context.Push((Decimal)value);
						break;
					case TypeCode.DateTime:
						context.Push((DateTime)value);
						break;
					case TypeCode.String:
						context.Push((String)value);
						break;
					default:
						throw new NotImplementedException();
				}
			}
		}

		private void Op_Isinst(CallContext context, IType type)
		{
			var value = context.PopObject();
			
			var instance = value as Instance;
			if (instance != null)
			{
				context.Push(instance.IsInstanceOf(type));
				return;
			}

			var array = value as Array;
			if (array != null)
			{
				if (!type.IsArray)
				{
					context.Push(false);
					return;
				}
				var elemType = type.GetElementType();
				var itemType = array.GetType().GetElementType();
				context.Push(IsCompatArrayElementType(itemType, elemType));
				return;
			}

			switch (type.GetTypeCode())
			{
				case TypeCode.Boolean:
					context.Push(value is Boolean);
					break;
				case TypeCode.Char:
					context.Push(value is Char);
					break;
				case TypeCode.SByte:
					context.Push(value is SByte);
					break;
				case TypeCode.Byte:
					context.Push(value is Byte);
					break;
				case TypeCode.Int16:
					context.Push(value is Int16);
					break;
				case TypeCode.UInt16:
					context.Push(value is UInt16);
					break;
				case TypeCode.Int32:
					context.Push(value is Int32);
					break;
				case TypeCode.UInt32:
					context.Push(value is UInt32);
					break;
				case TypeCode.Int64:
					context.Push(value is Int64);
					break;
				case TypeCode.UInt64:
					context.Push(value is UInt64);
					break;
				case TypeCode.Single:
					context.Push(value is Single);
					break;
				case TypeCode.Double:
					context.Push(value is Double);
					break;
				case TypeCode.Decimal:
					context.Push(value is Decimal);
					break;
				case TypeCode.DateTime:
					context.Push(value is DateTime);
					break;
				case TypeCode.String:
					context.Push(value is String);
					break;
				default:
					context.Push(value.GetType().IsInstanceOf(type));
					break;
			}
		}

		private static bool IsCompatArrayElementType(Type type, IType itype)
		{
			var code = itype.GetTypeCode();
			switch (Type.GetTypeCode(type))
			{
				case TypeCode.Object:
					//TODO: array element type
					return true;
				case TypeCode.Boolean:
					return code == TypeCode.Boolean;
				case TypeCode.Char:
					return code == TypeCode.Char;
				case TypeCode.SByte:
					case TypeCode.Byte:
					return code == TypeCode.SByte || code == TypeCode.Byte;
				case TypeCode.Int16:
				case TypeCode.UInt16:
					return code == TypeCode.Int16 || code == TypeCode.UInt16;
				case TypeCode.Int32:
				case TypeCode.UInt32:
					return code == TypeCode.Int32 || code == TypeCode.UInt32;
				case TypeCode.Int64:
				case TypeCode.UInt64:
					return code == TypeCode.Int64 || code == TypeCode.UInt64;
				case TypeCode.Single:
					return code == TypeCode.Single;
				case TypeCode.Double:
					return code == TypeCode.Double;
				case TypeCode.Decimal:
					return code == TypeCode.Decimal;
				case TypeCode.DateTime:
					return code == TypeCode.DateTime;
				case TypeCode.String:
					return code == TypeCode.String;
				default:
					return type.IsInstanceOf(itype);
			}
		}

		private void NewArray(CallContext context, IType type)
		{
			var len = Convert.ToInt32(context.Pop(false));
			Array array = NewArraySz(type, len);
			context.Push(array);
		}

		internal Array NewArray(IType elemType, int[] lengths)
		{
			switch (lengths.Length)
			{
				case 1:
					return NewArraySz(elemType, lengths[0]);
				default:
					var type = TypeOf(elemType);
					if (type is RuntimeType)
					{
						type = typeof(object);
						var array = Array.CreateInstance(type, lengths);
						GetClass(elemType);
						if (elemType.TypeKind == TypeKind.Struct)
						{
							var it = new ArrayIterator(array);
							while (it.MoveNext())
							{
								array.SetValue(InitObject(elemType), it.Indices);
							}
						}
						return array;
					}
					return Array.CreateInstance(type, lengths);
			}
		}

		private Array NewArraySz(IType elemType, int len)
		{
			switch (elemType.GetTypeCode())
			{
				case TypeCode.Boolean:
					return new Boolean[len];
				case TypeCode.Char:
					return new Char[len];
				case TypeCode.SByte:
					return new SByte[len];
				case TypeCode.Byte:
					return new Byte[len];
				case TypeCode.Int16:
					return new Int16[len];
				case TypeCode.UInt16:
					return new UInt16[len];
				case TypeCode.Int32:
					return new Int32[len];
				case TypeCode.UInt32:
					return new UInt32[len];
				case TypeCode.Int64:
					return new Int64[len];
				case TypeCode.UInt64:
					return new UInt64[len];
				case TypeCode.Single:
					return new Single[len];
				case TypeCode.Double:
					return new Double[len];
				case TypeCode.Decimal:
					return new Decimal[len];
				case TypeCode.DateTime:
					return new DateTime[len];
				case TypeCode.String:
					return new String[len];
				default:
					//TODO: other native types like Guid, TimeSpan, etc.
					GetClass(elemType);
					var array = new object[len];
					if (elemType.TypeKind == TypeKind.Struct)
					{
						for (int i = 0; i < len; i++)
						{
							array.SetValue(InitObject(elemType), i);
						}
					}
					return array;
			}
		}

		private static void CheckBounds(Array array, long index)
		{
			if (index < 0 || index >= array.LongLength)
				throw new IndexOutOfRangeException();
		}

		private void Op_Endfinally(CallContext context)
		{
			//TODO: auto rethrow
		}

		private void Op_Endfilter(CallContext context)
		{
		}

		private void Op_Sizeof(CallContext context)
		{
			throw new NotImplementedException();
		}
	}
}
