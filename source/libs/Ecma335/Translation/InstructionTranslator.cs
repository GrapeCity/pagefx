using System;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.CodeModel.Expressions;
using DataDynamics.PageFX.Common.Services;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Ecma335.IL;
using DataDynamics.PageFX.Ecma335.Translation.ControlFlow;
using DataDynamics.PageFX.Ecma335.Translation.Values;

namespace DataDynamics.PageFX.Ecma335.Translation
{
	/// <summary>
	/// Implements translation for every CIL instruction.
	/// </summary>
    internal sealed class InstructionTranslator
    {
	    private readonly TranslationContext _context;

		public InstructionTranslator(TranslationContext context)
		{
			_context = context;
		}

		private Node Block
		{
			get { return _context.Block; }
		}

		private IAssembly Assembly
		{
			get { return _context.Method.DeclaringType.Assembly; }
		}

		private SystemTypes SystemTypes
		{
			get { return Assembly.SystemTypes; }
		}

		/// <summary>
        /// Translates current instruction to array of instructions for target IL.
        /// This is a big switch by instruction code.
        /// </summary>
        /// <returns>array of instructions which performs the same computation as current translated instruction.</returns>
		public void Translate(Instruction currentInstruction)
	    {
		    var code = _context.Code;

            //TODO: Add comment for every instruction from ECMA #335
            switch (currentInstruction.Code)
            {
                #region constants
                case InstructionCode.Ldnull:
                    OpLdc(code, currentInstruction, null);
					break;
                case InstructionCode.Ldc_I4_M1:
					OpLdc(code, currentInstruction, -1);
					break;
                case InstructionCode.Ldc_I4_0:
					OpLdc(code, currentInstruction, 0);
					break;
                case InstructionCode.Ldc_I4_1:
					OpLdc(code, currentInstruction, 1);
					break;
                case InstructionCode.Ldc_I4_2:
					OpLdc(code, currentInstruction, 2);
					break;
                case InstructionCode.Ldc_I4_3:
					OpLdc(code, currentInstruction, 3);
					break;
                case InstructionCode.Ldc_I4_4:
					OpLdc(code, currentInstruction, 4);
					break;
                case InstructionCode.Ldc_I4_5:
					OpLdc(code, currentInstruction, 5);
					break;
                case InstructionCode.Ldc_I4_6:
					OpLdc(code, currentInstruction, 6);
					break;
                case InstructionCode.Ldc_I4_7:
					OpLdc(code, currentInstruction, 7);
					break;
                case InstructionCode.Ldc_I4_8:
					OpLdc(code, currentInstruction, 8);
					break;
                case InstructionCode.Ldc_I4_S:
					OpLdc(code, currentInstruction, (int)currentInstruction.Value);
					break;
                case InstructionCode.Ldc_I4:
					OpLdc(code, currentInstruction, (int)currentInstruction.Value);
					break;
                case InstructionCode.Ldc_I8:
					OpLdc(code, currentInstruction, (long)currentInstruction.Value);
					break;
                case InstructionCode.Ldc_R4:
					OpLdc(code, currentInstruction, (float)currentInstruction.Value);
					break;
                case InstructionCode.Ldc_R8:
					OpLdc(code, currentInstruction, (double)currentInstruction.Value);
					break;
                case InstructionCode.Ldstr:
					OpLdc(code, currentInstruction, currentInstruction.Value);
					break;
                #endregion

                #region load instructions
                case InstructionCode.Ldloc_0:
					OpLdloc(code, currentInstruction, 0);
					break;
				case InstructionCode.Ldloc_1:
					OpLdloc(code, currentInstruction, 1);
					break;
				case InstructionCode.Ldloc_2:
					OpLdloc(code, currentInstruction, 2);
					break;
				case InstructionCode.Ldloc_3:
					OpLdloc(code, currentInstruction, 3);
					break;
                case InstructionCode.Ldloc_S:
				case InstructionCode.Ldloc:
					OpLdloc(code, currentInstruction, (int)currentInstruction.Value);
					break;
                case InstructionCode.Ldloca_S:
                case InstructionCode.Ldloca:
					OpLdloca(code, currentInstruction, (int)currentInstruction.Value);
					break;

                case InstructionCode.Ldarg_0:
					OpLdarg(code, currentInstruction, 0);
					break;
				case InstructionCode.Ldarg_1:
					OpLdarg(code, currentInstruction, 1);
					break;
				case InstructionCode.Ldarg_2:
					OpLdarg(code, currentInstruction, 2);
					break;
				case InstructionCode.Ldarg_3:
					OpLdarg(code, currentInstruction, 3);
					break;
                case InstructionCode.Ldarg_S:
                case InstructionCode.Ldarg:
					OpLdarg(code, currentInstruction, (int)currentInstruction.Value);
					break;
                case InstructionCode.Ldarga_S:
                case InstructionCode.Ldarga:
					OpLdarga(code, currentInstruction, (int)currentInstruction.Value);
					break;

                case InstructionCode.Ldfld:
                case InstructionCode.Ldsfld:
                    OpLdfld(code, currentInstruction, currentInstruction.Field);
					break;

                case InstructionCode.Ldflda:
                case InstructionCode.Ldsflda:
                    OpLdflda(code, currentInstruction, currentInstruction.Field);
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
                    OpLdind(code, currentInstruction);
					break;

                case InstructionCode.Ldftn:
                    OpLdftn(code, currentInstruction);
					break;

                case InstructionCode.Ldvirtftn:
                    OpLdvirtftn(code, currentInstruction);
					break;

                case InstructionCode.Ldtoken:
                    OpLdtoken(currentInstruction);
					break;

                case InstructionCode.Ldobj:
                    OpLdobj(code, currentInstruction);
					break;
                #endregion

                #region store instructions
                case InstructionCode.Stloc_0:
                    OpStloc(code, currentInstruction, 0);
					break;
                case InstructionCode.Stloc_1:
                    OpStloc(code, currentInstruction, 1);
					break;
                case InstructionCode.Stloc_2:
                    OpStloc(code, currentInstruction, 2);
					break;
                case InstructionCode.Stloc_3:
                    OpStloc(code, currentInstruction, 3);
					break;
                case InstructionCode.Stloc_S:
                case InstructionCode.Stloc:
                    OpStloc(code, currentInstruction, (int)currentInstruction.Value);
					break;

                case InstructionCode.Starg_S:
                case InstructionCode.Starg:
                    OpStarg(code, currentInstruction, (int)currentInstruction.Value);
					break;

                case InstructionCode.Stfld:
                    OpStfld(code, currentInstruction, currentInstruction.Field);
					break;

                case InstructionCode.Stsfld:
                    OpStsfld(code, currentInstruction, currentInstruction.Field);
					break;

                case InstructionCode.Stind_Ref:
                case InstructionCode.Stind_I1:
                case InstructionCode.Stind_I2:
                case InstructionCode.Stind_I4:
                case InstructionCode.Stind_I8:
                case InstructionCode.Stind_R4:
                case InstructionCode.Stind_R8:
                case InstructionCode.Stind_I:
                    OpStind(code, currentInstruction);
					break;

                case InstructionCode.Stobj:
                    OpStobj(code, currentInstruction);
					break;
                #endregion

                #region stack operations
                case InstructionCode.Dup:
                    OpDup(code, currentInstruction);
					break;

                case InstructionCode.Pop:
                    OpPop(code, currentInstruction);
					break;
                #endregion

                #region call instructions
                case InstructionCode.Call:
                    OpCall(code, currentInstruction, false);
					break;

                case InstructionCode.Callvirt:
                    OpCall(code, currentInstruction, true);
					break;

                //call of function onto the stack
                case InstructionCode.Calli:
		            throw new NotSupportedException();

	            case InstructionCode.Newobj:
                    OpNewobj(code, currentInstruction);
					break;

                case InstructionCode.Initobj:
                    OpInitobj(code, currentInstruction);
					break;
                #endregion

                #region branches, switch
                case InstructionCode.Brfalse_S:
                case InstructionCode.Brfalse:
                    OpBranch(code, currentInstruction, BranchOperator.False, false);
					break;

                case InstructionCode.Brtrue_S:
                case InstructionCode.Brtrue:
					OpBranch(code, currentInstruction, BranchOperator.True, false);
					break;

                case InstructionCode.Beq_S:
                case InstructionCode.Beq:
					OpBranch(code, currentInstruction, BranchOperator.Equality, false);
					break;

                case InstructionCode.Bge_S:
                case InstructionCode.Bge:
					OpBranch(code, currentInstruction, BranchOperator.GreaterThanOrEqual, false);
					break;

                case InstructionCode.Bgt_S:
                case InstructionCode.Bgt:
					OpBranch(code, currentInstruction, BranchOperator.GreaterThan, false);
					break;

                case InstructionCode.Ble_S:
                case InstructionCode.Ble:
					OpBranch(code, currentInstruction, BranchOperator.LessThanOrEqual, false);
					break;

                case InstructionCode.Blt_S:
                case InstructionCode.Blt:
					OpBranch(code, currentInstruction, BranchOperator.LessThan, false);
					break;

                case InstructionCode.Bne_Un_S:
                case InstructionCode.Bne_Un:
					OpBranch(code, currentInstruction, BranchOperator.Inequality, true);
					break;

                case InstructionCode.Bge_Un_S:
                case InstructionCode.Bge_Un:
					OpBranch(code, currentInstruction, BranchOperator.GreaterThanOrEqual, true);
					break;

                case InstructionCode.Bgt_Un_S:
                case InstructionCode.Bgt_Un:
					OpBranch(code, currentInstruction, BranchOperator.GreaterThan, true);
					break;

                case InstructionCode.Ble_Un_S:
                case InstructionCode.Ble_Un:
					OpBranch(code, currentInstruction, BranchOperator.LessThanOrEqual, true);
					break;

                case InstructionCode.Blt_Un_S:
                case InstructionCode.Blt_Un:
					OpBranch(code, currentInstruction, BranchOperator.LessThan, true);
					break;

                case InstructionCode.Br_S:
                case InstructionCode.Br:
		            code.Branch();
		            break;

                case InstructionCode.Switch:
                    OpSwitch(code, currentInstruction);
					break;
                #endregion

                #region binary, unary arithmetic operations
                //arithmetic operations
                // a + b
                case InstructionCode.Add:
                    Op(code, currentInstruction, BinaryOperator.Addition, false, false);
					break;
                case InstructionCode.Add_Ovf:
					Op(code, currentInstruction, BinaryOperator.Addition, false, true);
					break;
                case InstructionCode.Add_Ovf_Un:
					Op(code, currentInstruction, BinaryOperator.Addition, true, true);
					break;

                // a - b
                case InstructionCode.Sub:
					Op(code, currentInstruction, BinaryOperator.Subtraction, false, false);
					break;
                case InstructionCode.Sub_Ovf:
					Op(code, currentInstruction, BinaryOperator.Subtraction, false, true);
					break;
                case InstructionCode.Sub_Ovf_Un:
					Op(code, currentInstruction, BinaryOperator.Subtraction, true, true);
					break;

                // a * b
                case InstructionCode.Mul:
					Op(code, currentInstruction, BinaryOperator.Multiply, false, false);
					break;
                case InstructionCode.Mul_Ovf:
					Op(code, currentInstruction, BinaryOperator.Multiply, false, true);
					break;
                case InstructionCode.Mul_Ovf_Un:
					Op(code, currentInstruction, BinaryOperator.Multiply, true, true);
					break;

                // a / b
                case InstructionCode.Div:
					Op(code, currentInstruction, BinaryOperator.Division, false, false);
					break;
                case InstructionCode.Div_Un:
					Op(code, currentInstruction, BinaryOperator.Division, true, false);
					break;

                // a % b
                case InstructionCode.Rem:
					Op(code, currentInstruction, BinaryOperator.Modulus, false, false);
					break;
                case InstructionCode.Rem_Un:
					Op(code, currentInstruction, BinaryOperator.Modulus, true, false);
					break;

                //bitwise operations
                // a & b
                case InstructionCode.And:
					Op(code, currentInstruction, BinaryOperator.BitwiseAnd, false, false);
					break;
                // a | b
                case InstructionCode.Or:
					Op(code, currentInstruction, BinaryOperator.BitwiseOr, false, false);
					break;
                // a ^ b
                case InstructionCode.Xor:
					Op(code, currentInstruction, BinaryOperator.ExclusiveOr, false, false);
					break;
                // a << b
                case InstructionCode.Shl:
					Op(code, currentInstruction, BinaryOperator.LeftShift, false, false);
					break;
                // a >> b
                case InstructionCode.Shr:
					Op(code, currentInstruction, BinaryOperator.RightShift, false, false);
					break;
                case InstructionCode.Shr_Un:
					Op(code, currentInstruction, BinaryOperator.RightShift, true, false);
					break;

                //unary operations
                case InstructionCode.Neg:
					Op(code, currentInstruction, UnaryOperator.Negate, false);
					break;
                case InstructionCode.Not:
					Op(code, currentInstruction, UnaryOperator.BitwiseNot, false);
					break;

                //relation operations
                // a == b
                case InstructionCode.Ceq:
					Op(code, currentInstruction, BinaryOperator.Equality, false, false);
					break;
                // a > b
                case InstructionCode.Cgt:
					Op(code, currentInstruction, BinaryOperator.GreaterThan, false, false);
					break;
                case InstructionCode.Cgt_Un:
					Op(code, currentInstruction, BinaryOperator.GreaterThan, true, false);
					break;
                // a < b
                case InstructionCode.Clt:
					Op(code, currentInstruction, BinaryOperator.LessThan, false, false);
					break;
                case InstructionCode.Clt_Un:
					Op(code, currentInstruction, BinaryOperator.LessThan, true, false);
					break;
                #endregion

                #region conversion instructions
                //TODO: IntPtr, UIntPtr is not supported
                case InstructionCode.Conv_I1:
					OpConv(code, currentInstruction, SystemTypes.Int8, false, false);
					break;
                case InstructionCode.Conv_I2:
					OpConv(code, currentInstruction, SystemTypes.Int16, false, false);
					break;
                case InstructionCode.Conv_I4:
					OpConv(code, currentInstruction, SystemTypes.Int32, false, false);
					break;
                case InstructionCode.Conv_I8:
					OpConv(code, currentInstruction, SystemTypes.Int64, false, false);
					break;
                case InstructionCode.Conv_R4:
					OpConv(code, currentInstruction, SystemTypes.Single, false, false);
					break;
                case InstructionCode.Conv_R8:
					OpConv(code, currentInstruction, SystemTypes.Double, false, false);
					break;

                case InstructionCode.Conv_U1:
					OpConv(code, currentInstruction, SystemTypes.UInt8, false, false);
					break;
                case InstructionCode.Conv_U2:
					OpConv(code, currentInstruction, SystemTypes.UInt16, false, false);
					break;
                case InstructionCode.Conv_U4:
					OpConv(code, currentInstruction, SystemTypes.UInt32, false, false);
					break;
                case InstructionCode.Conv_U8:
					OpConv(code, currentInstruction, SystemTypes.UInt64, false, false);
					break;

                case InstructionCode.Conv_Ovf_I1_Un:
					OpConv(code, currentInstruction, SystemTypes.Int8, true, true);
					break;
                case InstructionCode.Conv_Ovf_I2_Un:
					OpConv(code, currentInstruction, SystemTypes.Int16, true, true);
					break;
                case InstructionCode.Conv_Ovf_I4_Un:
					OpConv(code, currentInstruction, SystemTypes.Int32, true, true);
					break;
                case InstructionCode.Conv_Ovf_I8_Un:
					OpConv(code, currentInstruction, SystemTypes.Int64, true, true);
					break;
                case InstructionCode.Conv_Ovf_U1_Un:
					OpConv(code, currentInstruction, SystemTypes.UInt8, true, true);
					break;
                case InstructionCode.Conv_Ovf_U2_Un:
					OpConv(code, currentInstruction, SystemTypes.UInt16, true, true);
					break;
                case InstructionCode.Conv_Ovf_U4_Un:
					OpConv(code, currentInstruction, SystemTypes.UInt32, true, true);
					break;
                case InstructionCode.Conv_Ovf_U8_Un:
					OpConv(code, currentInstruction, SystemTypes.UInt64, true, true);
					break;
                case InstructionCode.Conv_Ovf_I_Un:
					//TODO: native int
					OpConv(code, currentInstruction, SystemTypes.Int32, true, true);
					break;
                case InstructionCode.Conv_Ovf_U_Un:
					// TODO: native uint
					OpConv(code, currentInstruction, SystemTypes.UInt32, true, true);
					break;
                case InstructionCode.Conv_Ovf_I1:
					OpConv(code, currentInstruction, SystemTypes.Int8, true, false);
					break;
                case InstructionCode.Conv_Ovf_U1:
					OpConv(code, currentInstruction, SystemTypes.UInt8, true, false);
					break;
                case InstructionCode.Conv_Ovf_I2:
					OpConv(code, currentInstruction, SystemTypes.Int16, true, false);
					break;
                case InstructionCode.Conv_Ovf_U2:
					OpConv(code, currentInstruction, SystemTypes.UInt16, true, false);
					break;
                case InstructionCode.Conv_Ovf_I4:
					OpConv(code, currentInstruction, SystemTypes.Int32, true, false);
					break;
                case InstructionCode.Conv_Ovf_U4:
					OpConv(code, currentInstruction, SystemTypes.UInt32, true, false);
					break;
                case InstructionCode.Conv_Ovf_I8:
					OpConv(code, currentInstruction, SystemTypes.Int64, true, false);
					break;
                case InstructionCode.Conv_Ovf_U8:
					OpConv(code, currentInstruction, SystemTypes.UInt64, true, false);
					break;

                case InstructionCode.Conv_I:
					OpConv(code, currentInstruction, SystemTypes.Int32, false, false);
					break;
                case InstructionCode.Conv_Ovf_I:
					OpConv(code, currentInstruction, SystemTypes.Int32, true, false);
					break;
                case InstructionCode.Conv_Ovf_U:
					OpConv(code, currentInstruction, SystemTypes.UInt32, true, false);
					break;
                case InstructionCode.Conv_U:
					OpConv(code, currentInstruction, SystemTypes.UInt32, false, false);
					break;
                case InstructionCode.Conv_R_Un:
					OpConv(code, currentInstruction, SystemTypes.Single, false, true);
					break;
                #endregion

                #region cast operations
                case InstructionCode.Castclass:
					OpCastclass(code, currentInstruction);
					break;

                case InstructionCode.Isinst:
					OpIsinst(code, currentInstruction);
					break;

                case InstructionCode.Box:
					OpBox(code, currentInstruction);
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
					OpUnbox(code, currentInstruction);
					break;
                #endregion

                #region array instructions
                case InstructionCode.Newarr:
					OpNewarr(code, currentInstruction);
					break;
                case InstructionCode.Ldlen:
					OpLdlen(code, currentInstruction);
					break;

                case InstructionCode.Ldelema:
					OpLdelema(code, currentInstruction);
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
					OpLdelem(code, currentInstruction);
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
					OpStelem(code, currentInstruction);
					break;
                #endregion

                #region exception handling
                case InstructionCode.Throw:
		            _context.Pop(currentInstruction);
		            code.Throw();
		            break;

                case InstructionCode.Rethrow:
					code.Rethrow(currentInstruction);
					break;

                case InstructionCode.Leave:
                case InstructionCode.Leave_S:
		            code.Branch();
		            break;

                case InstructionCode.Endfinally:
		            code.Branch();
		            break;

                case InstructionCode.Endfilter:
                    throw new NotSupportedException();
                #endregion

                #region misc
                case InstructionCode.Nop:
                    code.Nop();
					break;

                case InstructionCode.Break:
                    code.DebuggerBreak();
					break;

                case InstructionCode.Ret:
					OpReturn(code, currentInstruction);
					break;

                case InstructionCode.Sizeof:
		            throw new NotSupportedException();

		            #endregion

                #region not supported instructions
                case InstructionCode.Arglist:
                case InstructionCode.Cpblk:
                case InstructionCode.Initblk:
                case InstructionCode.Ckfinite:
                case InstructionCode.Localloc:
                case InstructionCode.Jmp:
                case InstructionCode.Cpobj:

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
        }

	    private IValue PopPtr(Instruction currentInstruction, Code code)
        {
            var obj = _context.PopValue(currentInstruction);
			code.LoadPtr(obj);
			return obj;
        }

	    #region load instructions

		private void BeforeStoreValue(Code code, EvalItem value, IType type)
        {
            var valType = value.Type;

            if (Block.IsFirstAssignment && value.Instruction.IsCall())
            {
                Block.IsFirstAssignment = false;
            }

            type = type.UnwrapRef();

			// fixing possible verifier error (unable to reconcile types)
            if (!TypeReconciler.ReconcileTernaryAssignment(_context, type))
            {
	            code.Cast(valType, type);
            }

			if (type.Is(SystemTypeCode.String))
            {
				// String are implemented via native string, so no need to copy it.
            }
            else
			{
				code.CopyValue(type);
			}
        }

		private void PassByValue(Code code, Instruction currentInstruction, IType valueType)
        {
            var p = currentInstruction.Parameter;
            if (p == null) return;

            var ptype = p.Type.UnwrapRef();

            var method = currentInstruction.ParameterFor;
            if (MustPreventBoxing(code, method, p))
            {
                if (valueType.Is(SystemTypeCode.String))
                {
                    _context.CastToParamType = false;
                    return;
                }
            }

			code.Cast(valueType, ptype);
			code.CopyValue(ptype);
			_context.CastToParamType = false;
        }

		private void OpLdc(Code code, Instruction currentInstruction, object value)
        {
			var v = new ConstValue(value, SystemTypes.ResolveType(value));
			_context.Push(currentInstruction, v);

			code.LoadConstant(value);
            PassByValue(code, currentInstruction, v.Type);
        }

		private void OpLdthis(Code code, Instruction currentInstruction, bool ptr)
        {
            var type = code.Method.DeclaringType;

            if (ptr)
            {
                if (code.Method.IsStatic)
                    throw new ILTranslatorException();

                if (code.Provider.IsThisAddressed)
                {
                    code.GetThisPtr();
	                _context.Push(currentInstruction, new ThisPtr(type));
                }
                else
                {
	                _context.Push(currentInstruction, new MockThisPtr(type));
                }
            }
            else
            {
                if (currentInstruction.IsByRef())
                {
                    code.GetThisPtr();
	                _context.Push(currentInstruction, new ThisPtr(type));
                }
                else
                {
                    code.LoadThis();
                    PassByValue(code, currentInstruction, type);
	                _context.Push(currentInstruction, new ThisValue(code.Method));
                }
            }
        }

		private void OpLdarg(Code code, Instruction currentInstruction, int index, bool ptr)
        {
            var p = code.Method.Parameters[index];

            if (ptr)
            {
                if (p.IsByRef())
                {
                    code.LoadArgument(p);
	                _context.Push(currentInstruction, new ArgPtr(p));
                }
                else if (p.IsAddressed)
                {
                    code.GetArgPtr(p);
	                _context.Push(currentInstruction, new ArgPtr(p));
                }
                else
                {
	                _context.Push(currentInstruction, new MockArgPtr(p));
                }
            }
            else if (p.IsByRef())
            {
                code.LoadArgument(p);
	            _context.Push(currentInstruction, new ArgPtr(p));
            }
            else
            {
                code.LoadArgument(p);
                PassByValue(code, currentInstruction, p.Type);
	            _context.Push(currentInstruction, new Arg(p));
            }
        }

		private static int ToRealArgIndex(Code code, int index)
		{
			return code.Method.IsStatic ? index : index - 1;
		}

	    private void OpLdarg(Code code, Instruction currentInstruction, int index)
        {
            index = ToRealArgIndex(code, index);
            if (index < 0)
                OpLdthis(code, currentInstruction, false);
			else
				OpLdarg(code, currentInstruction, index, false);
        }

		private void OpLdarga(Code code, Instruction currentInstruction, int index)
        {
            index = ToRealArgIndex(code, index);
            if (index < 0)
                OpLdthis(code, currentInstruction, true);
			else
				OpLdarg(code, currentInstruction, index, true);
        }

		private void OpLdloc(Code code, Instruction currentInstruction, int index)
        {
            var var = code.Body.LocalVariables[index];
            var type = var.Type;

            code.LoadVariable(var);

            PassByValue(code, currentInstruction, type);

			_context.Push(currentInstruction, new Var(var));
        }

		private void OpLdloca(Code code, Instruction currentInstruction, int index)
        {
            var var = code.Body.LocalVariables[index];
            if (var.IsAddressed)
            {
	            code.GetVarPtr(var);
	            _context.Push(currentInstruction, new VarPtr(var));
            }
            else
            {
	            _context.Push(currentInstruction, new MockVarPtr(var));
            }
        }

		private void OpLdfld(Code code, Instruction currentInstruction, IField field)
        {
            if (!field.IsStatic)
                PopPtr(currentInstruction, code);

            code.LoadField(field);
            PassByValue(code, currentInstruction, field.Type);

			_context.Push(currentInstruction, new FieldValue(field));
        }

		private void OpLdflda(Code code, Instruction currentInstruction, IField field)
        {
            if (!field.IsStatic)
                PopPtr(currentInstruction, code);

            if (currentInstruction.IsByRef())
            {
                code.GetFieldPtr(field);
	            _context.Push(currentInstruction, new FieldPtr(field));
            }
            else
            {
                if (field.IsStatic)
                {
	                _context.Push(currentInstruction, new MockFieldPtr(field));
                }
                else
                {
                    int obj = code.StoreTempVar();
	                _context.Push(currentInstruction, new MockFieldPtr(field, obj));
                }
            }
        }

		private void OpLdtoken(Instruction currentInstruction)
		{
			_context.Push(currentInstruction, new TokenValue(currentInstruction.Member));
		}

		private void OpLdftn(Code code, Instruction currentInstruction)
        {
            //stack transition:  ... -> ..., ftn
            var method = currentInstruction.Method;
            if (method.IsStatic)
            {
                code.LoadStaticInstance(method.DeclaringType);
            }
            else
            {
                //TODO: Add option to LoadFunction to save stack state
                //We need to duplicate target because ldftn instruction does not pop any object from stack
                code.Dup();
            }
            code.LoadFunction(method);
			_context.Push(currentInstruction, new Func(method, SystemTypes.IntPtr));
        }

		private void OpLdvirtftn(Code code, Instruction currentInstruction)
        {
            //stack transition: ..., object -> ..., ftn
            var obj = _context.Pop(currentInstruction);
            var method = currentInstruction.Method;
            if (method.IsStatic)
            {
                throw new ILTranslatorException();
            }
			_context.Push(currentInstruction, new Func(obj.Value, method));
			code.LoadFunction(method);
        }
        #endregion

        #region store instructions
		private void OpStarg(Code code, Instruction currentInstruction, int index)
        {
            if (!code.Method.IsStatic)
                --index;
            var value = _context.Pop(currentInstruction);
            var p = code.Method.Parameters[index];
            BeforeStoreValue(code, value, p.Type);
            code.StoreArgument(p);
        }

		private void OpStloc(Code code, Instruction currentInstruction, int index)
        {
            var value = _context.Pop(currentInstruction);
            var var = code.Body.LocalVariables[index];
            BeforeStoreValue(code, value, var.Type);
            code.StoreVariable(var);
        }

		private void OpStfld(Code code, Instruction currentInstruction, IField field)
        {
            var value = _context.Pop(currentInstruction);
            var obj = _context.Pop(currentInstruction);

            var type = field.Type;
            BeforeStoreValue(code, value, type);

            var ov = obj.Value;
            if (ov.IsPointer)
            {
                if (ov.IsMockPointer)
                {
	                code.LoadPtr(ov);
	                code.Swap();
                }
                else
                {
	                code.Swap();
	                code.LoadIndirect(null);
	                code.Swap();
                }
            }

            code.StoreField(field);
        }

		private void OpStsfld(Code code, Instruction currentInstruction, IField field)
        {
            if (!field.IsStatic)
                throw new InvalidOperationException();

            var value = _context.Pop(currentInstruction);
            BeforeStoreValue(code, value, field.Type);

            code.StoreField(field);
        }
        #endregion

        #region ldind, stind
        //load value indirect onto the stack
        //stack transition: ..., addr -> ..., value
		private void OpLdind(Code code, Instruction currentInstruction)
        {
            var addr = _context.Pop(currentInstruction);

            var v = addr.Value;

            var type = v.Type;

			currentInstruction.InputTypes = new []{type};

            switch (v.Kind)
            {
                case ValueKind.Const:
                case ValueKind.Var:
                case ValueKind.Field:
                case ValueKind.Elem:
                case ValueKind.Token:
                case ValueKind.Function:
                case ValueKind.Computed:
                    throw new InvalidOperationException();

                case ValueKind.This:
                    //TODO: Enshure that this case is only used to unbox primitive value type
                    code.Unbox(type);
		            _context.Push(currentInstruction, new ThisValue(type));
		            break;

                case ValueKind.ThisPtr:
		            code.LoadIndirect(type);
		            _context.Push(currentInstruction, new ThisValue(type));
		            break;

                case ValueKind.MockThisPtr:
                    code.LoadThis();
		            _context.Push(currentInstruction, new ThisValue(type));
		            break;

                case ValueKind.Arg:
		            code.LoadIndirect(type);
		            _context.Push(currentInstruction, v);
		            break;

                case ValueKind.ArgPtr:
                    {
                        var ptr = (ArgPtr)v;
	                    code.LoadIndirect(type);
	                    _context.Push(currentInstruction, new Arg(ptr.arg));
                    }
                    break;

                case ValueKind.MockArgPtr:
                    {
                        var ptr = (MockArgPtr)v;
                        code.LoadArgument(ptr.arg);
	                    _context.Push(currentInstruction, new Arg(ptr.arg));
                    }
                    break;

                case ValueKind.VarPtr:
                    {
                        var ptr = (VarPtr)v;
	                    code.LoadIndirect(type);
	                    _context.Push(currentInstruction, new Var(ptr.var));
                    }
                    break;

                case ValueKind.MockVarPtr:
                    {
                        var ptr = (MockVarPtr)v;
                        code.LoadVariable(ptr.var);
	                    _context.Push(currentInstruction, new Var(ptr.var));
                    }
                    break;

                case ValueKind.FieldPtr:
                    {
                        var ptr = (FieldPtr)v;
	                    code.LoadIndirect(type);
	                    _context.Push(currentInstruction, new FieldValue(ptr.field));
                    }
                    break;

                case ValueKind.MockFieldPtr:
                    {
                        var ptr = (MockFieldPtr)v;
	                    code.LoadFieldPtr(ptr);
	                    _context.Push(currentInstruction, new FieldValue(ptr.field));
                    }
                    break;

                case ValueKind.ElemPtr:
                    {
                        var ptr = (ElemPtr)v;
	                    code.LoadIndirect(type);
	                    _context.Push(currentInstruction, new Elem(ptr.elemType));
                    }
                    break;

                case ValueKind.MockElemPtr:
                    {
                        var ptr = (MockElemPtr)v;
	                    code.LoadElemPtr(ptr);
	                    _context.Push(currentInstruction, new Elem(ptr.elemType));
                    }
                    break;

                case ValueKind.ComputedPtr:
                    {
	                    code.LoadIndirect(type);
	                    _context.PushResult(currentInstruction, v.Type);
                    }
                    break;
            }
        }

        //store value indirect from stack
        //stack transition: ..., addr, val -> ...
		private void OpStind(Code code, Instruction currentInstruction)
        {
            var value = _context.Pop(currentInstruction);
            var addr = _context.Pop(currentInstruction);

            BeforeStoreValue(code, value, addr.Type);

			currentInstruction.InputTypes = new[] { value.Type };
			// NOTE: output type defines type of pointer
			currentInstruction.OutputType = addr.Type;

			switch (addr.Value.Kind)
            {
                case ValueKind.Const:
                case ValueKind.Var:
                case ValueKind.Field:
                case ValueKind.Elem:
                case ValueKind.Token:
                case ValueKind.Function:
                case ValueKind.Computed:
                    throw new ILTranslatorException();

                case ValueKind.This:
                    {
                        var v = (ThisValue)addr.Value;
	                    code.CopyToThis(v.Type);
                    }
                    break;

                default:
		            code.StorePtr(addr.Value, value.Type);
		            break;
            }
        }
        #endregion

	    //copy a value from an address to the stack
        //stack transition: ..., src -> ..., val
		private void OpLdobj(Code code, Instruction currentInstruction)
        {
            OpLdind(code, currentInstruction);
        }

        //store a value at an address
        //stack transition: ..., dest, src -> ...,
		private void OpStobj(Code code, Instruction currentInstruction)
        {
            OpStind(code, currentInstruction);
        }

	    #region stack operations
		private void OpDup(Code code, Instruction currentInstruction)
        {
            var v = _context.Peek().Value;
            switch (v.Kind)
            {
                case ValueKind.MockThisPtr:
                case ValueKind.MockArgPtr:
                case ValueKind.MockVarPtr:
		            _context.Push(currentInstruction, v);
		            return;

                case ValueKind.MockFieldPtr:
                    {
                        var ptr = (MockFieldPtr)v;
	                    if (ptr.IsInstance)
	                    {
		                    int obj = code.MoveTemp(ptr.obj);
		                    var newPtr = new MockFieldPtr(ptr.field, obj) {dup_source = ptr};
		                    _context.Push(currentInstruction, newPtr);
	                    }
	                    else
	                    {
		                    _context.Push(currentInstruction, v);
	                    }
	                    return;
                    }

                case ValueKind.MockElemPtr:
                    {
                        var ptr = (MockElemPtr)v;
                        int arr = code.MoveTemp(ptr.arr);
                        int index = code.MoveTemp(ptr.index);
                    	var newPtr = new MockElemPtr(ptr.arrType, ptr.elemType, arr, index) {dup_source = ptr};
	                    _context.Push(currentInstruction, newPtr);
	                    return;
                    }

            }

			_context.Push(currentInstruction, v);
			code.Dup();
        }

		private void OpPop(Code code, Instruction currentInstruction)
        {
            if (!currentInstruction.IsHandlerBegin)
            {
                var v = _context.PopValue(currentInstruction);
                if (v.IsMockPointer)
                {
                    switch (v.Kind)
                    {
                        case ValueKind.MockFieldPtr:
                            {
                                var ptr = (MockFieldPtr)v;
                                if (ptr.IsInstance)
                                {
                                    code.KillTempVar(ptr.obj);
                                    return;
                                }
                                return;
                            }

                        case ValueKind.MockElemPtr:
                            {
                                var ptr = (MockElemPtr)v;
                                code.KillTempVar(ptr.arr);
                                code.KillTempVar(ptr.index);
                                return;
                            }

                        default:
                            return;
                    }
                }
            }

			code.Pop();
        }
        #endregion

        #region branches

	    private void OpBranch(Code code, Instruction currentInstruction, BranchOperator op, bool unsigned)
        {
            IType leftType, rightType = null;
            if (op == BranchOperator.False || op == BranchOperator.True)
            {
                var v = _context.Pop(currentInstruction);
                v.ItShouldBeNonPointer();
                leftType = v.Type;

	            currentInstruction.InputTypes = new [] {leftType};

                if (v.IsInstance || !(leftType.IsNumeric() || leftType.IsEnum))
                {
                    op = op == BranchOperator.False ? BranchOperator.Null : BranchOperator.NotNull;
                }
            }
            else
            {
                var right = _context.Pop(currentInstruction);
                var left = _context.Pop(currentInstruction);

                right.ItShouldBeNonPointer();
                left.ItShouldBeNonPointer();

                leftType = left.Type;
                rightType = right.Type;

	            currentInstruction.InputTypes = new[] {leftType, rightType};

                //FIX: Problem with comparions signed and unsigned numbers.
                if (unsigned || CastingOperations.IsSignedUnsigned(leftType, rightType))
                {
                    code.ToUnsigned(ref leftType, ref rightType);
                }
            }

            code.Branch(op, leftType, rightType);
        }

		private void OpSwitch(Code code, Instruction currentInstruction)
        {
            var index = _context.Pop(currentInstruction);
            index.ItShouldBeNonPointer();

            //NOTE: AVM requires int value for switch instruction
			code.Cast(index.Type, SystemTypes.Int32);

			var targets = (int[])currentInstruction.Value;
			code.Switch(targets.Length);
        }
        #endregion

        #region arithmetic operations

	    /// <summary>
	    /// Performs binary operation
	    /// </summary>
	    /// <param name="currentInstruction"></param>
	    /// <param name="op"></param>
	    /// <param name="unsigned"></param>
	    /// <param name="checkOverflow"></param>
	    /// <returns></returns>
	    private void Op(Code code, Instruction currentInstruction, BinaryOperator op, bool unsigned, bool checkOverflow)
        {
            //stack transition: left, right -> result
            var right = _context.Pop(currentInstruction);
            var left = _context.Pop(currentInstruction);

            right.ItShouldBeNonPointer();
            left.ItShouldBeNonPointer();

            var leftType = left.Type;
            var rightType = right.Type;

            //NOTE: Fix for relation operations.
            //NOTE: Sequence of instructions (isinst, null, cgt) does not work in avm.
            //NOTE: -1 == 4294967295 is false in avm
            if (op.IsRelation())
            {
                if (!op.IsEquality()
                    && ((left.IsNull && right.IsInstance) || (right.IsNull && left.IsInstance)))
                {
                    op = BinaryOperator.Inequality;
                }
                else if (CastingOperations.IsSignedUnsigned(leftType, rightType))
                {
                    //TODO: Optimize for constants
                    code.ToUnsigned(ref leftType, ref rightType);
                }
            }

            if (op.IsBoolean())
            {

            }
            else
            {
                if (unsigned)
                {
                    if (op.IsShift())
                    {
                        code.CastToInt32(ref rightType);
                        code.ToUnsigned(ref leftType, true);
                    }
                    else
                    {
                        code.ToUnsigned(ref leftType, ref rightType);
                    }
                }
                else
                {
                    if (op.IsShift())
                    {
                        code.CastToInt32(ref rightType);
                    }
                    else
                    {
                        code.CastOperands(op, ref leftType, ref rightType);
                    }
                }
            }

            var type = BinaryExpression.GetResultType(leftType, rightType, op);
            if (type == null)
                throw new ILTranslatorException();

	        currentInstruction.InputTypes = new[] {leftType, rightType};
	        currentInstruction.OutputType = type;

		    _context.PushResult(currentInstruction, type);

		    code.Op(op, leftType, rightType, type, checkOverflow);
        }

		private void Op(Code code, Instruction currentInstruction, UnaryOperator op, bool checkOverflow)
        {
            var value = _context.Pop(currentInstruction);
            value.ItShouldBeNonPointer();

            var vtype = value.Type;
            var type = UnaryExpression.GetResultType(vtype, op);
            if (type == null)
                throw new ILTranslatorException();

			currentInstruction.InputTypes = new[] {vtype};
			currentInstruction.OutputType = type;

			_context.PushResult(currentInstruction, type);

			code.Op(op, vtype, checkOverflow);
        }
        #endregion

        #region conversion operations
		private void OpConv(Code code, Instruction currentInstruction, IType targetType, bool checkOverflow, bool unsigned)
        {
            var value = _context.Pop(currentInstruction);
            value.ItShouldBeNonPointer();

            var sourceType = value.Type;

			currentInstruction.InputTypes = new []{sourceType};
			currentInstruction.OutputType = targetType;

			if (!checkOverflow && OpConvRet(code, currentInstruction, sourceType, targetType))
                return;

			_context.PushResult(currentInstruction, targetType);

			if (unsigned)
                code.ToUnsigned(ref sourceType, false);

			code.Cast(sourceType, targetType, checkOverflow);
        }

	    private bool OpConvRet(Code code, Instruction currentInstruction, IType sourceType, IType targetType)
        {
            int nextIndex = currentInstruction.Index + 1;
            if (nextIndex >= code.Body.Code.Count) return false;
            var next = code.GetInstruction(nextIndex);
            if (next.BasicBlock != currentInstruction.BasicBlock) return false;
            if (next.Code != InstructionCode.Ret) return false;

            if (targetType.IsInt64())
            {
                var st = sourceType.SystemType();
                if (st != null && st.IsIntegral32)
                {
                    var retType = code.Method.Type;
                    if (retType.IsInt64())
                    {
                        code.Provider.DonotCopyReturnValue = true;
	                    _context.PushResult(currentInstruction, retType);
	                    code.Cast(sourceType, retType, false);
	                    return true;
                    }
                }
            }

            return false;
        }

	    #endregion

        #region cast, isinst, box, unbox
		private void OpCastclass(Code code, Instruction currentInstruction)
        {
            var type = currentInstruction.Type;
            
            var value = PopPtr(currentInstruction, code);
			code.Cast(value.Type, type);

			_context.PushResult(currentInstruction, type);
        }

        //test if an object is an instance of a class or interface
        //stack transition: ..., obj -> ..., result
		private void OpIsinst(Code code, Instruction currentInstruction)
        {
            var type = currentInstruction.Type;
            PopPtr(currentInstruction, code);
            code.As(type);
			_context.PushResult(currentInstruction, type);
        }

		private void OpBox(Code code, Instruction currentInstruction)
        {
            var type = currentInstruction.Type;
            var value = _context.PopValue(currentInstruction);

			_context.PushResult(currentInstruction, type.IsNullableInstance() ? type.GetTypeArgument(0) : type);

			var vtype = type;
            if (type.IsEnum)
                vtype = type.ValueType;

            var m = currentInstruction.ParameterFor;
            if (MustPreventBoxing(code, m, currentInstruction.Parameter))
            {
                //TODO: check value types (can be problems with Int64).
	            code.LoadPtr(value);
	            return;
            }

			code.LoadPtr(value)
			    .Cast(value.Type, vtype)
			    .Box(type);
        }

		private void OpUnbox(Code code, Instruction currentInstruction)
        {
            var type = currentInstruction.Type;
            PopPtr(currentInstruction, code);
            code.Unbox(type);
			_context.PushResult(currentInstruction, type);
        }
        #endregion

        #region call operations
		private bool PopArgs(Instruction currentInstruction, IMethod method, ref IType rtype)
        {
            int n = method.Parameters.Count;
            for (int i = 0; i < n; ++i)
            {
                var arg = _context.Pop(currentInstruction);
            }
            if (currentInstruction.HasReceiver())
            {
                var obj = _context.Pop(currentInstruction);
                rtype = obj.Type;
                switch (obj.Value.Kind)
                {
                    case ValueKind.This:
                    case ValueKind.ThisPtr:
                    case ValueKind.MockThisPtr:
                        return true;
                }
            }
            return false;
        }

		private static void Call(Code code, Instruction currentInstruction, IType receiverType, IMethod method, CallFlags flags)
        {
            if (currentInstruction.Code == InstructionCode.Newobj)
                flags |= CallFlags.Newobj;
            code.CallMethod(receiverType, method, flags);
            code.EndCall(method);
        }

		private void OpCall(Code code, Instruction currentInstruction, bool virtcall)
        {
            var method = currentInstruction.Method;
			if (method.IsInitializeArray())
			{
				InitializeArray(code, currentInstruction);
				return;
			}

			if (IsGetTypeFromHandle(method))
			{
				TypeOf(code, currentInstruction);
				return;
			}

			var type = method.Type;
            if (type.TypeKind == TypeKind.Pointer)
                throw new ILTranslatorException("Pointers are not supported");

            //if (method.Name == "Get" && _method.Name == "MoveNext")
            //    Debugger.Break();

            CallFlags flags = 0;
            IType receiverType = null;
            bool thiscall = PopArgs(currentInstruction, method, ref receiverType);

            if (thiscall) flags |= CallFlags.Thiscall;
            if (virtcall) flags |= CallFlags.Virtcall;

            bool basecall = false;
            if (!thiscall && !virtcall
				&& !method.IsStatic && !method.IsConstructor
                && !ReferenceEquals(receiverType, method.DeclaringType))
                basecall = receiverType.IsSubclassOf(method.DeclaringType);

            if (basecall) flags |= CallFlags.Basecall;

			currentInstruction.CallInfo = new CallInfo(receiverType, flags);
			
            if (!method.IsVoid())
            {
                if (type.TypeKind == TypeKind.Reference)
                {
	                _context.Push(currentInstruction, new ComputedPtr(type.UnwrapRef()));
                }
                else
                {
	                _context.PushResult(currentInstruction, method.Type);
                }
            }

            Call(code, currentInstruction, receiverType, method, flags);
        }

		private void InitializeArray(Code code, Instruction currentInstruction)
        {
            var token = _context.Pop(currentInstruction);
            var f = token.Instruction.Field;
            var arr = _context.Pop(currentInstruction);
            var arrType = (IArrayType)arr.Type;
            var elemType = arrType.ElementType;
            var vals = CLR.ReadArrayValues(f, elemType.SystemType().Code);

            int n = vals.Count;
            for (int i = 0; i < n; ++i)
            {
                //put array onto the stack
	            code.Dup()
	                .LoadConstant(i) //index
	                .LoadConstant(vals[i])
	                .SetArrayElem(elemType);
            }

			//TODO: remove this pop by doing n-1 dups only
            //Note: Now we must remove from stack array because it does InitializeArray CLR method
            code.Pop();
        }

		private bool IsGetTypeFromHandle(IMethod m)
        {
            if (!m.IsGetTypeFromHandle()) return false;
            return _context.Peek().IsTypeToken;
        }

		private void TypeOf(Code code, Instruction currentInstruction)
        {
            var token = _context.Pop(currentInstruction);
            var type = token.Instruction.Type;
			_context.PushResult(currentInstruction, SystemTypes.Type);
			code.TypeOf(type);
        }

	    private void OpNewobj(Code code, Instruction currentInstruction)
        {
            var ctor = currentInstruction.Method;

            IType rtype = null;
            PopArgs(currentInstruction, ctor, ref rtype);

            var type = ctor.DeclaringType;
		    _context.PushResult(currentInstruction, type);

		    //FixTernaryAssignment(type);

            Call(code, currentInstruction, rtype, ctor, 0);
        }

		private void OpInitobj(Code code, Instruction currentInstruction)
        {
            var addr = _context.Pop(currentInstruction);

            switch (addr.Value.Kind)
            {
                case ValueKind.Const:
                case ValueKind.Var:
                case ValueKind.Field:
                case ValueKind.Token:
                case ValueKind.Function:
                case ValueKind.Computed:
                case ValueKind.Elem:
                    throw new NotSupportedException();
            }

            //TODO: Handle ValueKind.This

            var type = currentInstruction.Type;

            code.InitObject(type);

			code.StorePtr(addr.Value, type);
        }
        #endregion

        #region array instructions
		private void OpNewarr(Code code, Instruction currentInstruction)
        {
            var n = _context.Pop(currentInstruction);
            n.ItShouldBeNonPointer();

			var nType = SystemTypes.Int32;
            if (!TypeReconciler.ReconcileTernaryAssignment(_context, nType))
            {
	            code.Cast(n.Type, nType);
            }

			var elemType = currentInstruction.Type;

			_context.PushResult(currentInstruction, TypeFactory.MakeArray(elemType));

			code.NewArray(elemType);
        }

		private void OpLdlen(Code code, Instruction currentInstruction)
        {
            var arr = _context.Pop(currentInstruction);
            arr.ItShouldBeArray();
			_context.PushResult(currentInstruction, SystemTypes.Int32);
			code.GetArrayLength();
        }

		private static IType GetElemType(IType type)
        {
            var ct = type as ICompoundType;
            if (ct == null)
                throw new ArgumentException();
            return ct.ElementType;
        }

		private void OpLdelem(Code code, Instruction currentInstruction)
        {
            var index = _context.Pop(currentInstruction);
            var arr = _context.Pop(currentInstruction);
            arr.ItShouldBeArray();

            var elemType = GetElemType(arr.Type);

			currentInstruction.InputTypes = new[] {elemType};
			currentInstruction.OutputType = elemType;

            code.GetArrayElem(elemType);
            PassByValue(code, currentInstruction, elemType);

			_context.Push(currentInstruction, new Elem(elemType));
        }

		private void OpLdelema(Code code, Instruction currentInstruction)
        {
            var index = _context.Pop(currentInstruction);
            var arr = _context.Pop(currentInstruction);
            arr.ItShouldBeArray();

            var elemType = GetElemType(arr.Type);

            //cast index to int
			code.Cast(index.Type, SystemTypes.Int32);

			if (currentInstruction.IsByRef())
            {
                code.GetElemPtr(elemType);

	            _context.Push(currentInstruction, new ElemPtr(arr.Type, elemType));
            }
            else
            {
                int vindex = code.StoreTempVar();
                int varr = code.StoreTempVar();
	            _context.Push(currentInstruction, new MockElemPtr(arr.Type, elemType, varr, vindex));
            }
        }

        //store element to array
        //stack transition: ..., array, index, value, -> ...
		private void OpStelem(Code code, Instruction currentInstruction)
        {
            var value = _context.Pop(currentInstruction);
            var index = _context.Pop(currentInstruction);
            var arr = _context.Pop(currentInstruction);
            arr.ItShouldBeArray();
            index.ItShouldBeNonPointer();

			currentInstruction.InputTypes = new[] {value.Type};

            var elemType = GetElemType(arr.Type);

            BeforeStoreValue(code, value, elemType);
            code.SetArrayElem(elemType);
        }
        #endregion

	    private void OpReturn(Code code, Instruction currentInstruction)
		{
			bool isvoid = code.Method.IsVoid();
			if (!isvoid)
			{
				var v = _context.Pop(currentInstruction);
				code.Cast(v.Type, code.Method.Type);
			}
			code.AddRange(code.Provider.Return(isvoid));
		}

	    private static bool MustPreventBoxing(Code code, IMethod method, IParameter arg)
		{
			return code.Provider.MustPreventBoxing(method, arg);
		}
    }
}