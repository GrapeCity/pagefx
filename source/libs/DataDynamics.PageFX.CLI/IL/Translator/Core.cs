using System;
using System.Collections.Generic;
using System.Diagnostics;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.IL
{
    using Code = List<IInstruction>;

    //This part contains translation for every CIL instruction.
    partial class ILTranslator
    {
        #region TranslateInstructionCore
        /// <summary>
        /// Translates current instruction to array of instructions for target IL.
        /// This is a big switch by instruction code.
        /// </summary>
        /// <returns>array of instructions which performs the same computation as current translated instruction.</returns>
        IInstruction[] TranslateInstructionCore()
        {
            //TODO: Add comment for every instruction from ECMA #335
            switch (_instruction.Code)
            {
                #region constants
                case InstructionCode.Ldnull:
                    return Op_Ldc(null);
                case InstructionCode.Ldc_I4_M1:
                    return Op_Ldc(-1);
                case InstructionCode.Ldc_I4_0:
                    return Op_Ldc(0);
                case InstructionCode.Ldc_I4_1:
                    return Op_Ldc(1);
                case InstructionCode.Ldc_I4_2:
                    return Op_Ldc(2);
                case InstructionCode.Ldc_I4_3:
                    return Op_Ldc(3);
                case InstructionCode.Ldc_I4_4:
                    return Op_Ldc(4);
                case InstructionCode.Ldc_I4_5:
                    return Op_Ldc(5);
                case InstructionCode.Ldc_I4_6:
                    return Op_Ldc(6);
                case InstructionCode.Ldc_I4_7:
                    return Op_Ldc(7);
                case InstructionCode.Ldc_I4_8:
                    return Op_Ldc(8);
                case InstructionCode.Ldc_I4_S:
                    return Op_Ldc((int)_instruction.Value);
                case InstructionCode.Ldc_I4:
                    return Op_Ldc((int)_instruction.Value);
                case InstructionCode.Ldc_I8:
                    return Op_Ldc((long)_instruction.Value);
                case InstructionCode.Ldc_R4:
                    return Op_Ldc((float)_instruction.Value);
                case InstructionCode.Ldc_R8:
                    return Op_Ldc((double)_instruction.Value);
                case InstructionCode.Ldstr:
                    return Op_Ldc(_instruction.Value);
                #endregion

                #region load instructions
                case InstructionCode.Ldloc_0: return Op_Ldloc(0);
                case InstructionCode.Ldloc_1: return Op_Ldloc(1);
                case InstructionCode.Ldloc_2: return Op_Ldloc(2);
                case InstructionCode.Ldloc_3: return Op_Ldloc(3);
                case InstructionCode.Ldloc_S: case InstructionCode.Ldloc:
                    return Op_Ldloc((int)_instruction.Value);
                case InstructionCode.Ldloca_S:
                case InstructionCode.Ldloca:
                    return Op_Ldloca((int)_instruction.Value);

                case InstructionCode.Ldarg_0: return Op_Ldarg(0);
                case InstructionCode.Ldarg_1: return Op_Ldarg(1);
                case InstructionCode.Ldarg_2: return Op_Ldarg(2);
                case InstructionCode.Ldarg_3: return Op_Ldarg(3);
                case InstructionCode.Ldarg_S:
                case InstructionCode.Ldarg:
                    return Op_Ldarg((int)_instruction.Value);
                case InstructionCode.Ldarga_S:
                case InstructionCode.Ldarga:
                    return Op_Ldarga((int)_instruction.Value);

                case InstructionCode.Ldfld:
                case InstructionCode.Ldsfld:
                    return Op_Ldfld(_instruction.Field);

                case InstructionCode.Ldflda:
                case InstructionCode.Ldsflda:
                    return Op_Ldflda(_instruction.Field);

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
                    return Op_Ldind();

                case InstructionCode.Ldftn:
                    return Op_Ldftn();

                case InstructionCode.Ldvirtftn:
                    return Op_Ldvirtftn();

                case InstructionCode.Ldtoken:
                    return Op_Ldtoken();

                case InstructionCode.Ldobj:
                    return Op_Ldobj();
                #endregion

                #region store instructions
                case InstructionCode.Stloc_0:
                    return Op_Stloc(0);
                case InstructionCode.Stloc_1:
                    return Op_Stloc(1);
                case InstructionCode.Stloc_2:
                    return Op_Stloc(2);
                case InstructionCode.Stloc_3:
                    return Op_Stloc(3);
                case InstructionCode.Stloc_S:
                case InstructionCode.Stloc:
                    return Op_Stloc((int)_instruction.Value);

                case InstructionCode.Starg_S:
                case InstructionCode.Starg:
                    return Op_Starg((int)_instruction.Value);

                case InstructionCode.Stfld:
                    return Op_Stfld(_instruction.Field);

                case InstructionCode.Stsfld:
                    return Op_Stsfld(_instruction.Field);

                case InstructionCode.Stind_Ref:
                case InstructionCode.Stind_I1:
                case InstructionCode.Stind_I2:
                case InstructionCode.Stind_I4:
                case InstructionCode.Stind_I8:
                case InstructionCode.Stind_R4:
                case InstructionCode.Stind_R8:
                case InstructionCode.Stind_I:
                    return Op_Stind();

                case InstructionCode.Stobj:
                    return Op_Stobj();
                #endregion

                #region stack operations
                case InstructionCode.Dup:
                    return Op_Dup();

                case InstructionCode.Pop:
                    return Op_Pop();
                #endregion

                #region call instructions
                case InstructionCode.Call:
                    return Op_Call(false);

                case InstructionCode.Callvirt:
                    return Op_Call(true);

                //call of function onto the stack
                case InstructionCode.Calli:
                    return Op_Calli();

                case InstructionCode.Newobj:
                    return Op_Newobj();

                case InstructionCode.Initobj:
                    return Op_Initobj();
                #endregion

                #region branches, switch
                case InstructionCode.Brfalse_S:
                case InstructionCode.Brfalse:
                    return Op_Branch(BranchOperator.False, false);

                case InstructionCode.Brtrue_S:
                case InstructionCode.Brtrue:
                    return Op_Branch(BranchOperator.True, false);

                case InstructionCode.Beq_S:
                case InstructionCode.Beq:
                    return Op_Branch(BranchOperator.Equality, false);

                case InstructionCode.Bge_S:
                case InstructionCode.Bge:
                    return Op_Branch(BranchOperator.GreaterThanOrEqual, false);

                case InstructionCode.Bgt_S:
                case InstructionCode.Bgt:
                    return Op_Branch(BranchOperator.GreaterThan, false);

                case InstructionCode.Ble_S:
                case InstructionCode.Ble:
                    return Op_Branch(BranchOperator.LessThanOrEqual, false);

                case InstructionCode.Blt_S:
                case InstructionCode.Blt:
                    return Op_Branch(BranchOperator.LessThan, false);

                case InstructionCode.Bne_Un_S:
                case InstructionCode.Bne_Un:
                    return Op_Branch(BranchOperator.Inequality, true);

                case InstructionCode.Bge_Un_S:
                case InstructionCode.Bge_Un:
                    return Op_Branch(BranchOperator.GreaterThanOrEqual, true);

                case InstructionCode.Bgt_Un_S:
                case InstructionCode.Bgt_Un:
                    return Op_Branch(BranchOperator.GreaterThan, true);

                case InstructionCode.Ble_Un_S:
                case InstructionCode.Ble_Un:
                    return Op_Branch(BranchOperator.LessThanOrEqual, true);

                case InstructionCode.Blt_Un_S:
                case InstructionCode.Blt_Un:
                    return Op_Branch(BranchOperator.LessThan, true);

                case InstructionCode.Br_S:
                case InstructionCode.Br:
                    return Op_Branch();

                case InstructionCode.Switch:
                    return Op_Switch();
                #endregion

                #region binary, unary arithmetic operations
                //arithmetic operations
                // a + b
                case InstructionCode.Add:
                    return Op(BinaryOperator.Addition, false, false);
                case InstructionCode.Add_Ovf:
                    return Op(BinaryOperator.Addition, false, true);
                case InstructionCode.Add_Ovf_Un:
                    return Op(BinaryOperator.Addition, true, true);

                // a - b
                case InstructionCode.Sub:
                    return Op(BinaryOperator.Subtraction, false, false);
                case InstructionCode.Sub_Ovf:
                    return Op(BinaryOperator.Subtraction, false, true);
                case InstructionCode.Sub_Ovf_Un:
                    return Op(BinaryOperator.Subtraction, true, true);

                // a * b
                case InstructionCode.Mul:
                    return Op(BinaryOperator.Multiply, false, false);
                case InstructionCode.Mul_Ovf:
                    return Op(BinaryOperator.Multiply, false, true);
                case InstructionCode.Mul_Ovf_Un:
                    return Op(BinaryOperator.Multiply, true, true);

                // a / b
                case InstructionCode.Div:
                    return Op(BinaryOperator.Division, false, false);
                case InstructionCode.Div_Un:
                    return Op(BinaryOperator.Division, true, false);

                // a % b
                case InstructionCode.Rem:
                    return Op(BinaryOperator.Modulus, false, false);
                case InstructionCode.Rem_Un:
                    return Op(BinaryOperator.Modulus, true, false);

                //bitwise operations
                // a & b
                case InstructionCode.And:
                    return Op(BinaryOperator.BitwiseAnd, false, false);
                // a | b
                case InstructionCode.Or:
                    return Op(BinaryOperator.BitwiseOr, false, false);
                // a ^ b
                case InstructionCode.Xor:
                    return Op(BinaryOperator.ExclusiveOr, false, false);
                // a << b
                case InstructionCode.Shl:
                    return Op(BinaryOperator.LeftShift, false, false);
                // a >> b
                case InstructionCode.Shr:
                    return Op(BinaryOperator.RightShift, false, false);
                case InstructionCode.Shr_Un:
                    return Op(BinaryOperator.RightShift, true, false);

                //unary operations
                case InstructionCode.Neg:
                    return Op(UnaryOperator.Negate, false);
                case InstructionCode.Not:
                    return Op(UnaryOperator.BitwiseNot, false);

                //relation operations
                // a == b
                case InstructionCode.Ceq:
                    return Op(BinaryOperator.Equality, false, false);
                // a > b
                case InstructionCode.Cgt:
                    return Op(BinaryOperator.GreaterThan, false, false);
                case InstructionCode.Cgt_Un:
                    return Op(BinaryOperator.GreaterThan, true, false);
                // a < b
                case InstructionCode.Clt:
                    return Op(BinaryOperator.LessThan, false, false);
                case InstructionCode.Clt_Un:
                    return Op(BinaryOperator.LessThan, true, false);
                #endregion

                #region conversion instructions
                //TODO: IntPtr, UIntPtr is not supported
                case InstructionCode.Conv_I1:
                    return Op_Conv(SystemTypes.Int8, false, false);
                case InstructionCode.Conv_I2:
                    return Op_Conv(SystemTypes.Int16, false, false);
                case InstructionCode.Conv_I4:
                    return Op_Conv(SystemTypes.Int32, false, false);
                case InstructionCode.Conv_I8:
                    return Op_Conv(SystemTypes.Int64, false, false);
                case InstructionCode.Conv_R4:
                    return Op_Conv(SystemTypes.Float32, false, false);
                case InstructionCode.Conv_R8:
                    return Op_Conv(SystemTypes.Float64, false, false);

                case InstructionCode.Conv_U1:
                    return Op_Conv(SystemTypes.UInt8, false, false);
                case InstructionCode.Conv_U2:
                    return Op_Conv(SystemTypes.UInt16, false, false);
                case InstructionCode.Conv_U4:
                    return Op_Conv(SystemTypes.UInt32, false, false);
                case InstructionCode.Conv_U8:
                    return Op_Conv(SystemTypes.UInt64, false, false);

                case InstructionCode.Conv_Ovf_I1_Un:
                    return Op_Conv(SystemTypes.Int8, true, true);
                case InstructionCode.Conv_Ovf_I2_Un:
                    return Op_Conv(SystemTypes.Int16, true, true);
                case InstructionCode.Conv_Ovf_I4_Un:
                    return Op_Conv(SystemTypes.Int32, true, true);
                case InstructionCode.Conv_Ovf_I8_Un:
                    return Op_Conv(SystemTypes.Int64, true, true);
                case InstructionCode.Conv_Ovf_U1_Un:
                    return Op_Conv(SystemTypes.UInt8, true, true);
                case InstructionCode.Conv_Ovf_U2_Un:
                    return Op_Conv(SystemTypes.UInt16, true, true);
                case InstructionCode.Conv_Ovf_U4_Un:
                    return Op_Conv(SystemTypes.UInt32, true, true);
                case InstructionCode.Conv_Ovf_U8_Un:
                    return Op_Conv(SystemTypes.UInt64, true, true);
                case InstructionCode.Conv_Ovf_I_Un:
                    return Op_Conv(SystemTypes.NativeInt, true, true);
                case InstructionCode.Conv_Ovf_U_Un:
                    return Op_Conv(SystemTypes.NativeUInt, true, true);
                case InstructionCode.Conv_Ovf_I1:
                    return Op_Conv(SystemTypes.Int8, true, false);
                case InstructionCode.Conv_Ovf_U1:
                    return Op_Conv(SystemTypes.UInt8, true, false);
                case InstructionCode.Conv_Ovf_I2:
                    return Op_Conv(SystemTypes.Int16, true, false);
                case InstructionCode.Conv_Ovf_U2:
                    return Op_Conv(SystemTypes.UInt16, true, false);
                case InstructionCode.Conv_Ovf_I4:
                    return Op_Conv(SystemTypes.Int32, true, false);
                case InstructionCode.Conv_Ovf_U4:
                    return Op_Conv(SystemTypes.UInt32, true, false);
                case InstructionCode.Conv_Ovf_I8:
                    return Op_Conv(SystemTypes.Int64, true, false);
                case InstructionCode.Conv_Ovf_U8:
                    return Op_Conv(SystemTypes.UInt64, true, false);

                case InstructionCode.Conv_I:
                    return Op_Conv(SystemTypes.NativeInt, false, false);
                case InstructionCode.Conv_Ovf_I:
                    return Op_Conv(SystemTypes.NativeInt, true, false);
                case InstructionCode.Conv_Ovf_U:
                    return Op_Conv(SystemTypes.NativeUInt, true, false);
                case InstructionCode.Conv_U:
                    return Op_Conv(SystemTypes.NativeUInt, false, false);
                case InstructionCode.Conv_R_Un:
                    return Op_Conv(SystemTypes.Float32, false, true);
                #endregion

                #region cast operations
                case InstructionCode.Castclass:
                    return Op_Castclass();

                case InstructionCode.Isinst:
                    return Op_Isinst();

                case InstructionCode.Box:
                    return Op_Box();

                //NOTE:
                //The constrained. prefix is permitted only on a callvirt instruction.
                //The type of ptr must be a managed pointer (&) to thisType.
                //The constrained prefix is designed to allow callvirt instructions to be made in a
                //uniform way independent of whether thisType is a value type or a reference type.
                case InstructionCode.Constrained:
                    return null;

                case InstructionCode.Unbox:
                case InstructionCode.Unbox_Any:
                    return Op_Unbox();
                #endregion

                #region array instructions
                case InstructionCode.Newarr:
                    return Op_Newarr();
                case InstructionCode.Ldlen:
                    return Op_Ldlen();

                case InstructionCode.Ldelema:
                    return Op_Ldelema();

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
                    return Op_Ldelem();

                case InstructionCode.Stelem_I:
                case InstructionCode.Stelem_I1:
                case InstructionCode.Stelem_I2:
                case InstructionCode.Stelem_I4:
                case InstructionCode.Stelem_I8:
                case InstructionCode.Stelem_R4:
                case InstructionCode.Stelem_R8:
                case InstructionCode.Stelem_Ref:
                case InstructionCode.Stelem:
                    return Op_Stelem();
                #endregion

                #region exception handling
                case InstructionCode.Throw:
                    return Op_Throw();

                case InstructionCode.Rethrow:
                    return Op_Rethrow();

                case InstructionCode.Leave:
                case InstructionCode.Leave_S:
                    return Op_Leave();

                case InstructionCode.Endfinally:
                    return Op_Endfinally();

                case InstructionCode.Endfilter:
                    return Op_Endfilter();
                #endregion

                #region misc
                case InstructionCode.Nop:
                    return Op_Nop();

                case InstructionCode.Break:
                    return Op_DebuggerBreak();

                case InstructionCode.Ret:
                    return Op_Return();

                case InstructionCode.Sizeof:
                    return Op_Sizeof();
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
            return null;
        }
        #endregion

        #region Pointers
        //Reads value at given mock field ptr
        void Load(Code code, MockFieldPtr ptr)
        {
            if (ptr.IsInstance)
            {
                LoadTemp(code, ptr.obj);
                code.AddRange(_provider.LoadField(ptr.field));
                KillTemp(code, ptr.obj);
            }
            else
            {
                code.AddRange(_provider.LoadField(ptr.field));
            }
        }

        //Writes value at given mock field ptr
        //value must be onto the stack
        void Store(Code code, MockFieldPtr ptr)
        {
            if (ptr.IsInstance)
            {
                LoadTemp(code, ptr.obj);
                code.Add(_provider.Swap());
                code.AddRange(_provider.StoreField(ptr.field));
                KillTemp(code, ptr.obj);
            }
            else
            {
                code.AddRange(_provider.StoreField(ptr.field));
            }
        }

        //Reads value at given mock elem ptr
        void Load(Code code, MockElemPtr ptr)
        {
            LoadTemp(code, ptr.arr);
            LoadTemp(code, ptr.index);
            code.AddRange(_provider.GetArrayElem(ptr.elemType));
            KillTemp(code, ptr.arr);
            KillTemp(code, ptr.index);
        }

        //Writes value at given mock elem ptr
        //value must be onto the stack
        void Store(Code code, MockElemPtr ptr)
        {
            LoadTemp(code, ptr.arr);
            code.Add(_provider.Swap());
            LoadTemp(code, ptr.index);
            code.Add(_provider.Swap());
            code.AddRange(_provider.SetArrayElem(ptr.elemType));
            KillTemp(code, ptr.arr);
            KillTemp(code, ptr.index);
        }

        void LoadIndirect(Code code, IType vtype)
        {
            code.AddRange(_provider.LoadIndirect(vtype));
        }

        bool LoadPtr(Code code, IValue v)
        {
            if (!v.IsPointer) return false;

            if (v.IsMockPointer)
            {
                switch (v.Kind)
                {
                    case ValueKind.MockThisPtr:
                        code.AddRange(_provider.LoadThis());
                        break;

                    case ValueKind.MockArgPtr:
                        code.AddRange(_provider.LoadArgument(((MockArgPtr)v).arg));
                        break;

                    case ValueKind.MockVarPtr:
                        code.AddRange(_provider.LoadVariable(((MockVarPtr)v).var));
                        break;

                    case ValueKind.MockFieldPtr:
                        Load(code, (MockFieldPtr)v);
                        break;

                    case ValueKind.MockElemPtr:
                        Load(code, (MockElemPtr)v);
                        break;
                }
            }
            else
            {
                var vtype = v.Type;
                LoadIndirect(code, vtype);
            }
            return true;
        }

        IInstruction[] LoadPtr(IValue v)
        {
            var code = new Code();
            LoadPtr(code, v);
            return code.ToArray();
        }

        void StorePtr(Code code, IValue addr, IType vtype)
        {
            if (addr.IsMockPointer)
            {
                switch (addr.Kind)
                {
                    case ValueKind.MockThisPtr:
                        code.AddRange(_provider.StoreThis());
                        break;

                    case ValueKind.MockArgPtr:
                        {
                            var ptr = (MockArgPtr)addr;
                            code.AddRange(_provider.StoreArgument(ptr.arg));
                        }
                        break;

                    case ValueKind.MockVarPtr:
                        {
                            var ptr = (MockVarPtr)addr;
                            code.AddRange(_provider.StoreVariable(ptr.var));
                        }
                        break;

                    case ValueKind.MockFieldPtr:
                        {
                            var ptr = (MockFieldPtr)addr;
                            Store(code, ptr);
                        }
                        break;

                    case ValueKind.MockElemPtr:
                        {
                            var ptr = (MockElemPtr)addr;
                            Store(code, ptr);
                        }
                        break;

                    default:
                        throw new ILTranslatorException();
                }
            }
            else
            {
                var il = _provider.StoreIndirect(vtype);
                code.AddRange(il);
            }
        }

        IValue PopPtr(Code code)
        {
            var obj = PopValue();
            LoadPtr(code, obj);
            return obj;
        }
        #endregion

        #region load instructions
        bool IsByRef
        {
            get
            {
                var p = _instruction.Parameter;
                if (p == null) return false;
                return p.IsByRef;
            }
        }

        void CopyValue(Code code, IType type)
        {
            var copy = _provider.CopyValue(type);
            if (copy != null)
                code.AddRange(copy);
        }

        void CopyValue(IType type)
        {
            var copy = _provider.CopyValue(type);
            if (copy != null)
                EmitBlockCode(copy);
        }

        void BeforeStoreValue(Code code, EvalItem value, IType type)
        {
            var valType = value.Type;

            if (_block.IsFirstAssignment && IsCall(value.instruction))
            {
                _block.IsFirstAssignment = false;
            }

            type = TypeService.UnwrapRef(type);

            if (!FixTernaryAssignment(type))
            {
                Cast(code, valType, type);
            }

            if (type == SystemTypes.String)
            {
                //TODO: Check whether it is need.
                //IInstruction[] copy = _provider.CopyString();
                //if (copy != null)
                //    code.AddRange(copy);
            }
            else
            {
                CopyValue(code, type);
            }
        }

        void PassByValue(Code code, IType valueType)
        {
            var p = _instruction.Parameter;
            if (p == null) return;

            var ptype = TypeService.UnwrapRef(p.Type);

            var method = _instruction.ParameterFor;
            if (MustPreventBoxing(method, p))
            {
                if (valueType == SystemTypes.String)
                {
                    var il = _provider.UnwrapString();
                    if (il != null)
                        code.AddRange(il);
                    _castToParamType = false;
                    return;
                }
            }

            Cast(code, valueType, ptype);
            CopyValue(code, ptype);
            _castToParamType = false;
        }

        IInstruction[] Op_Ldc(object value)
        {
            var v = new ConstValue(value);
            Push(v);

            var code = new Code();
            code.AddRange(_provider.LoadConstant(value));
            PassByValue(code, v.Type);

            return code.ToArray();
        }

        IInstruction[] Op_Ldthis(bool ptr)
        {
            var code = new Code();

            var type = _method.DeclaringType;

            if (ptr)
            {
                if (_method.IsStatic)
                    throw new ILTranslatorException();

                if (_provider.IsThisAddressed)
                {
                    code.AddRange(_provider.GetThisPtr());
                    Push(new ThisPtr(type));
                }
                else
                {
                    Push(new MockThisPtr(type));
                }
            }
            else
            {
                if (IsByRef)
                {
                    code.AddRange(_provider.GetThisPtr());
                    Push(new ThisPtr(type));
                }
                else
                {
                    code.AddRange(_provider.LoadThis());
                    PassByValue(code, type);
                    Push(new ThisValue(_method));
                }
            }

            return code.ToArray();
        }

        IInstruction[] Op_Ldarg(int index, bool ptr)
        {
            var p = _method.Parameters[index];

            var code = new Code();

            if (ptr)
            {
                if (p.IsByRef)
                {
                    code.AddRange(_provider.LoadArgument(p));
                    Push(new ArgPtr(p));
                }
                else if (p.IsAddressed)
                {
                    code.AddRange(_provider.GetArgPtr(p));
                    Push(new ArgPtr(p));
                }
                else
                {
                    Push(new MockArgPtr(p));
                }
            }
            else if (p.IsByRef)
            {
                code.AddRange(_provider.LoadArgument(p));
                Push(new ArgPtr(p));
            }
            else
            {
                code.AddRange(_provider.LoadArgument(p));
                PassByValue(code, p.Type);
                Push(new Arg(p));
            }

            return code.ToArray();
        }

        int ToRealArgIndex(int index)
        {
            if (!_method.IsStatic) //has this
                return index - 1;
            return index;
        }

        IInstruction[] Op_Ldarg(int index)
        {
            index = ToRealArgIndex(index);
            if (index < 0)
                return Op_Ldthis(false);
            return Op_Ldarg(index, false);
        }

        IInstruction[] Op_Ldarga(int index)
        {
            index = ToRealArgIndex(index);
            if (index < 0)
                return Op_Ldthis(true);
            return Op_Ldarg(index, true);
        }

        IInstruction[] Op_Ldloc(int index)
        {
            var v = _body.LocalVariables[index];
            var type = v.Type;

            var code = new Code();
            code.AddRange(_provider.LoadVariable(v));
            PassByValue(code, type);

            Push(new Var(v));

            return code.ToArray();
        }

        IInstruction[] Op_Ldloca(int index)
        {
            var v = _body.LocalVariables[index];
            if (v.IsAddressed)
            {
                var code = new Code();
                code.AddRange(_provider.GetVarPtr(v));
                Push(new VarPtr(v));
                return code.ToArray();
            }
            Push(new MockVarPtr(v));
            return null;
        }

        IInstruction[] Op_Ldfld(IField field)
        {
            var code = new Code();

            if (!field.IsStatic)
                PopPtr(code);

            code.AddRange(_provider.LoadField(field));
            PassByValue(code, field.Type);

            Push(new FieldValue(field));

            return code.ToArray();
        }

        IInstruction[] Op_Ldflda(IField field)
        {
            var code = new Code();

            if (!field.IsStatic)
                PopPtr(code);

            if (IsByRef)
            {
                code.AddRange(_provider.GetFieldPtr(field));
                Push(new FieldPtr(field));
            }
            else
            {
                if (field.IsStatic)
                {
                    Push(new MockFieldPtr(field));
                }
                else
                {
                    int obj = StoreTemp(code);
                    Push(new MockFieldPtr(field, obj));
                }
            }

            return code.ToArray();
        }

        IInstruction[] Op_Ldtoken()
        {
            Push(new TokenValue(_instruction.Member));
            return null;
        }

        IInstruction[] Op_Ldftn()
        {
            //stack transition:  ... -> ..., ftn
            var code = new Code();
            var method = _instruction.Method;
            if (method.IsStatic)
            {
                code.AddRange(_provider.LoadStaticInstance(method.DeclaringType));
            }
            else
            {
                //TODO: Add option to LoadFunction to save stack state
                //We need to duplicate target because ldftn instruction does not pop any object from stack
                code.Add(_provider.Dup());
            }
            code.AddRange(_provider.LoadFunction(method));
            Push(new Func(method));
            return code.ToArray();
        }

        IInstruction[] Op_Ldvirtftn()
        {
            //stack transition: ..., object -> ..., ftn
            var obj = Pop();
            var method = _instruction.Method;
            if (method.IsStatic)
            {
                throw new ILTranslatorException();
            }
            Push(new Func(obj.value, method));
            return _provider.LoadFunction(method);
        }
        #endregion

        #region store instructions
        IInstruction[] Op_Starg(int index)
        {
            if (!_method.IsStatic)
                --index;
            var value = Pop();
            var p = _method.Parameters[index];
            var code = new Code();
            BeforeStoreValue(code, value, p.Type);
            code.AddRange(_provider.StoreArgument(p));
            return code.ToArray();
        }

        IInstruction[] Op_Stloc(int index)
        {
            var value = Pop();
            var v = _body.LocalVariables[index];
            var code = new Code();
            BeforeStoreValue(code, value, v.Type);
            code.AddRange(_provider.StoreVariable(v));
            return code.ToArray();
        }

        IInstruction[] Op_Stfld(IField field)
        {
            var value = Pop();
            var obj = Pop();

            var code = new Code();

            var type = field.Type;
            BeforeStoreValue(code, value, type);

            var ov = obj.value;
            if (ov.IsPointer)
            {
                if (ov.IsMockPointer)
                {
                    LoadPtr(code, ov);
                    Swap(code);
                }
                else
                {
                    Swap(code);
                    LoadIndirect(code, null);
                    Swap(code);
                }
            }

            code.AddRange(_provider.StoreField(field));

            return code.ToArray();
        }

        IInstruction[] Op_Stsfld(IField field)
        {
            if (!field.IsStatic)
                throw new InvalidOperationException();

            var value = Pop();
            var code = new Code();
            BeforeStoreValue(code, value, field.Type);

            code.AddRange(_provider.StoreField(field));
            return code.ToArray();
        }
        #endregion

        #region ldind, stind
        //load value indirect onto the stack
        //stack transition: ..., addr -> ..., value
        IInstruction[] Op_Ldind()
        {
            var addr = Pop();

            var v = addr.value;
            var code = new Code();

            var type = v.Type;

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
                    code.AddRange(_provider.Unbox(type));
                    Push(new ThisValue(type));
                    break;

                case ValueKind.ThisPtr:
                    LoadIndirect(code, type);
                    Push(new ThisValue(type));
                    break;

                case ValueKind.MockThisPtr:
                    code.AddRange(_provider.LoadThis());
                    Push(new ThisValue(type));
                    break;

                case ValueKind.Arg:
                    LoadIndirect(code, type);
                    Push(v);
                    break;

                case ValueKind.ArgPtr:
                    {
                        var ptr = (ArgPtr)v;
                        LoadIndirect(code, type);
                        Push(new Arg(ptr.arg));
                    }
                    break;

                case ValueKind.MockArgPtr:
                    {
                        var ptr = (MockArgPtr)v;
                        code.AddRange(_provider.LoadArgument(ptr.arg));
                        Push(new Arg(ptr.arg));
                    }
                    break;

                case ValueKind.VarPtr:
                    {
                        var ptr = (VarPtr)v;
                        LoadIndirect(code, type);
                        Push(new Var(ptr.var));
                    }
                    break;

                case ValueKind.MockVarPtr:
                    {
                        var ptr = (MockVarPtr)v;
                        code.AddRange(_provider.LoadVariable(ptr.var));
                        Push(new Var(ptr.var));
                    }
                    break;

                case ValueKind.FieldPtr:
                    {
                        var ptr = (FieldPtr)v;
                        LoadIndirect(code, type);
                        Push(new FieldValue(ptr.field));
                    }
                    break;

                case ValueKind.MockFieldPtr:
                    {
                        var ptr = (MockFieldPtr)v;
                        Load(code, ptr);
                        Push(new FieldValue(ptr.field));
                    }
                    break;

                case ValueKind.ElemPtr:
                    {
                        var ptr = (ElemPtr)v;
                        LoadIndirect(code, type);
                        Push(new Elem(ptr.elemType));
                    }
                    break;

                case ValueKind.MockElemPtr:
                    {
                        var ptr = (MockElemPtr)v;
                        Load(code, ptr);
                        Push(new Elem(ptr.elemType));
                    }
                    break;

                case ValueKind.ComputedPtr:
                    {
                        LoadIndirect(code, type);
                        PushResult(v.Type);
                    }
                    break;
            }

            return code.ToArray();
        }

        //store value indirect from stack
        //stack transition: ..., addr, val -> ...
        IInstruction[] Op_Stind()
        {
            var value = Pop();
            var addr = Pop();

            var code = new Code();
            BeforeStoreValue(code, value, addr.Type);

            var av = addr.value;
            switch (av.Kind)
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
                        var v = (ThisValue)av;
                        var vt = v.Type;
                        if (_provider.HasCopy(vt))
                        {
                            var il = _provider.CopyToThis(vt);
                            code.AddRange(il);
                        }
                        else
                        {
                            throw new ILTranslatorException();
                        }
                    }
                    break;

                default:
                    StorePtr(code, av, value.Type);
                    break;
            }

            return code.ToArray();
        }
        #endregion

        #region ldobj, stobj
        //copy a value from an address to the stack
        //stack transition: ..., src -> ..., val
        IInstruction[] Op_Ldobj()
        {
            //IType type = _instruction.Type;
            return Op_Ldind();
        }

        //store a value at an address
        //stack transition: ..., dest, src -> ...,
        IInstruction[] Op_Stobj()
        {
            //IType type = _instruction.Type;
            return Op_Stind();
        }
        #endregion

        #region stack operations
        IInstruction[] Op_Dup()
        {
            var v = Peek().value;
            switch (v.Kind)
            {
                case ValueKind.MockThisPtr:
                case ValueKind.MockArgPtr:
                case ValueKind.MockVarPtr:
                    Push(v);
                    return null;

                case ValueKind.MockFieldPtr:
                    {
                        var ptr = (MockFieldPtr)v;
                        if (ptr.IsInstance)
                        {
                            var code = new Code();
                            int obj = MoveTemp(code, ptr.obj);
                            var newPtr = new MockFieldPtr(ptr.field, obj);
                            newPtr.dup_source = ptr;
                            Push(newPtr);
                            return code.ToArray();
                        }
                        Push(v);
                        return null;
                    }

                case ValueKind.MockElemPtr:
                    {
                        var ptr = (MockElemPtr)v;
                        var code = new Code();
                        int arr = MoveTemp(code, ptr.arr);
                        int index = MoveTemp(code, ptr.index);
                        var newPtr = new MockElemPtr(ptr.arrType, ptr.elemType, arr, index);
                        newPtr.dup_source = ptr;
                        Push(newPtr);
                        return code.ToArray();
                    }

            }
            Push(v);
            return new[] {_provider.Dup()};
        }

        IInstruction[] Op_Pop()
        {
            if (!_instruction.IsHandlerBegin)
            {
                var v = PopValue();
                if (v.IsMockPointer)
                {
                    switch (v.Kind)
                    {
                        case ValueKind.MockFieldPtr:
                            {
                                var ptr = (MockFieldPtr)v;
                                if (ptr.IsInstance)
                                {
                                    var code = new Code();
                                    code.AddRange(_provider.KillTempVar(ptr.obj));
                                    return code.ToArray();
                                }
                                return null;
                            }

                        case ValueKind.MockElemPtr:
                            {
                                var code = new Code();
                                var ptr = (MockElemPtr)v;
                                code.AddRange(_provider.KillTempVar(ptr.arr));
                                code.AddRange(_provider.KillTempVar(ptr.index));
                                return code.ToArray();
                            }

                        default:
                            return null;
                    }
                }
            }
            return new[] { _provider.Pop() };
        }
        #endregion

        #region branches
        IInstruction[] Op_Branch()
        {
            return new[] { _provider.Branch() };
        }

        IInstruction[] Op_Branch(BranchOperator op, bool unsigned)
        {
            var code = new Code();

            IType lt, rt = null;
            if (op == BranchOperator.False || op == BranchOperator.True)
            {
                var v = Pop();
                CheckPointer(v);
                lt = v.Type;

                if (v.IsInstance || !(SystemTypes.IsNumeric(lt) || lt.IsEnum))
                {
                    op = op == BranchOperator.False ? BranchOperator.Null : BranchOperator.NotNull;
                }
            }
            else
            {
                var right = Pop();
                var left = Pop();

                CheckPointer(right);
                CheckPointer(left);

                lt = left.Type;
                rt = right.Type;

                //FIX: Problem with comparions signed and unsigned numbers.
                if (unsigned || IsSignedUnsigned(lt, rt))
                {
                    ToUnsigned(code, ref lt, ref rt);
                }
            }

            code.AddRange(_provider.Branch(op, lt, rt));

            return code.ToArray();
        }

        IInstruction[] Op_Switch()
        {
            var index = Pop();
            CheckPointer(index);

            var code = new Code();

            //NOTE: AVM requires int value for switch instruction
            Cast(code, index.Type, SystemTypes.Int32);

            var targets = (int[])_instruction.Value;
            int n = targets.Length;
            var sw = _provider.Switch(n);
            if (sw == null)
                throw new NotSupportedException();
            code.Add(sw);

            return code.ToArray();
        }
        #endregion

        #region arithmetic operations
        /// <summary>
        /// Performs binary operation
        /// </summary>
        /// <param name="op"></param>
        /// <param name="unsigned"></param>
        /// <param name="checkOverflow"></param>
        /// <returns></returns>
        IInstruction[] Op(BinaryOperator op, bool unsigned, bool checkOverflow)
        {
            //stack transition: left, right -> result
            var right = Pop();
            var left = Pop();

            CheckPointer(right);
            CheckPointer(left);

            var lt = left.Type;
            var rt = right.Type;

            var code = new Code();

            //NOTE: Fix for relation operations.
            //NOTE: Sequence of instructions (isinst, null, cgt) does not work in avm.
            //NOTE: -1 == 4294967295 is false in avm
            if (OpHelper.IsRelation(op))
            {
                if (!OpHelper.IsEquality(op)
                    && ((left.IsNull && right.IsInstance) || (right.IsNull && left.IsInstance)))
                {
                    op = BinaryOperator.Inequality;
                }
                else if (IsSignedUnsigned(lt, rt))
                {
                    //TODO: Optimize for constants
                    ToUnsigned(code, ref lt, ref rt);
                }
            }

            if (OpHelper.IsBoolean(op))
            {

            }
            else
            {
                if (unsigned)
                {
                    if (OpHelper.IsShift(op))
                    {
                        CastToInt32(code, ref rt);
                        ToUnsigned(code, ref lt, true);
                    }
                    else
                    {
                        ToUnsigned(code, ref lt, ref rt);
                    }
                }
                else
                {
                    if (OpHelper.IsShift(op))
                    {
                        CastToInt32(code, ref rt);
                    }
                    else
                    {
                        ReduceToCD(code, op, ref lt, ref rt);
                    }
                }
            }

            var type = BinaryExpression.GetResultType(lt, rt, op);
            if (type == null)
                throw new ILTranslatorException();

            PushResult(type);

            var il = _provider.Op(op, lt, rt, type, checkOverflow);
            if (il == null || il.Length == 0)
                throw new ILTranslatorException("No code for given binary operation");

            code.AddRange(il);

            return code.ToArray();
        }

        IInstruction[] Op(UnaryOperator op, bool checkOverflow)
        {
            var value = Pop();
            CheckPointer(value);
            var vtype = value.Type;
            var type = UnaryExpression.GetResultType(vtype, op);
            if (type == null)
                throw new ILTranslatorException();
            PushResult(type);
            return _provider.Op(op, vtype, checkOverflow);
        }
        #endregion

        #region conversion operations
        IInstruction[] Op_Conv(IType targetType, bool checkOverflow, bool unsigned)
        {
            var value = Pop();
            CheckPointer(value);

            var code = new Code();

            var sourceType = value.Type;

            if (!checkOverflow && Opt_Conv_Ret(code, sourceType, targetType))
                return code.ToArray();

            PushResult(targetType);

            if (unsigned)
                ToUnsigned(code, ref sourceType, false);

            ConvCore(code, sourceType, targetType, checkOverflow);
            
            return code.ToArray();
        }

        void ConvCore(Code code, IType source, IType target, bool checkOverflow)
        {
            var il = _provider.Cast(source, target, checkOverflow);
            code.AddRange(il);
        }

        bool Opt_Conv_Ret(Code code, IType sourceType, IType targetType)
        {
            int ni = _instruction.Index + 1;
            if (ni >= SourceCode.Count) return false;
            var next = GetInstruction(ni);
            if (next.OwnerNode != _instruction.OwnerNode) return false;
            if (next.Code != InstructionCode.Ret) return false;

            if (IsUI64(targetType))
            {
                var st = sourceType.SystemType;
                if (st != null && st.IsIntegral32)
                {
                    var retType = ReturnType;
                    if (IsUI64(retType))
                    {
                        _provider.DonotCopyReturnValue = true;
                        PushResult(retType);
                        ConvCore(code, sourceType, retType, false);
                        return true;
                    }
                }
            }

            return false;
        }

        static bool IsUI64(IType type)
        {
            return type == SystemTypes.Int64 || type == SystemTypes.UInt64;
        }
        #endregion

        #region cast, isinst, box, unbox
        IInstruction[] Op_Castclass()
        {
            var type = _instruction.Type;
            
            var code = new Code();
            var value = PopPtr(code);
            code.AddRange(_provider.Cast(value.Type, type, false));

            PushResult(type);
            return code.ToArray();
        }

        //test if an object is an instance of a class or interface
        //stack transition: ..., obj -> ..., result
        IInstruction[] Op_Isinst()
        {
            var type = _instruction.Type;
            var code = new Code();
            PopPtr(code);
            code.AddRange(_provider.As(type));
            PushResult(type);
            return code.ToArray();
        }

        IInstruction[] Op_Box()
        {
            var type = _instruction.Type;
            var value = PopValue();

        	PushResult(TypeService.IsNullableInstance(type) ? TypeService.GetTypeArg(type, 0) : type);

        	var code = new Code();

            var vtype = type;
            if (type.IsEnum)
                vtype = type.ValueType;

            var m = _instruction.ParameterFor;
            if (MustPreventBoxing(m, _instruction.Parameter))
            {
                //TODO: check value types (can be problems with Int64).
                LoadPtr(code, value);
                return code.ToArray();
            }

            LoadPtr(code, value);
            Cast(code, value.Type, vtype);
            code.AddRange(_provider.Box(type));

            return code.ToArray();
        }

        IInstruction[] Op_Unbox()
        {
            var type = _instruction.Type;
            var code = new Code();
            PopPtr(code);
            code.AddRange(_provider.Unbox(type));
            PushResult(type);
            return code.ToArray();
        }
        #endregion

        #region call operations
        bool PopArgs(Code code, IMethod method, ref IType rtype)
        {
            int n = method.Parameters.Count;
            for (int i = 0; i < n; ++i)
            {
                var arg = Pop();
            }
            if (HasReceiver(_instruction))
            {
                var obj = Pop();
                rtype = obj.Type;
                switch (obj.value.Kind)
                {
                    case ValueKind.This:
                    case ValueKind.ThisPtr:
                    case ValueKind.MockThisPtr:
                        return true;
                }
            }
            return false;
        }

        IInstruction[] Call(Code code, IType receiverType, IMethod method, CallFlags flags)
        {
            if (_instruction.Code == InstructionCode.Newobj)
                flags |= CallFlags.Newobj;
            var c = _provider.CallMethod(receiverType, method, flags);
            code.AddRange(c);
            c = _provider.EndCall(method);
            if (c != null)
                code.AddRange(c);
            return code.ToArray();
        }

        IInstruction[] Op_Call(bool virtcall)
        {
            var method = _instruction.Method;
            if (CLR.IsInitializeArray(method))
                return InitializeArray();
            if (IsGetTypeFromHandle(method))
                return TypeOf();

            var type = method.Type;
            if (type.TypeKind == TypeKind.Pointer)
                throw new ILTranslatorException("Pointers are not supported");

            //if (method.Name == "Get" && _method.Name == "MoveNext")
            //    Debugger.Break();

            var code = new Code();

            CallFlags flags = 0;
            IType receiverType = null;
            bool thiscall = PopArgs(code, method, ref receiverType);

            if (thiscall) flags |= CallFlags.Thiscall;
            if (virtcall) flags |= CallFlags.Virtcall;

            bool basecall = false;
            if (!thiscall && !virtcall
                && receiverType != method.DeclaringType
                && !method.IsStatic && !method.IsConstructor)
                basecall = TypeService.IsSubclassOf(receiverType, method.DeclaringType);

            if (basecall) flags |= CallFlags.Basecall;

            if (!TypeService.IsVoid(method))
            {
                if (type.TypeKind == TypeKind.Reference)
                {
                    Push(new ComputedPtr(TypeService.UnwrapRef(type)));
                }
                else
                {
                    PushResult(method.Type);
                }
            }

            return Call(code, receiverType, method, flags);
        }

        IInstruction[] InitializeArray()
        {
            var token = Pop();
            var f = token.instruction.Field;
            var arr = Pop();
            var arrType = (IArrayType)arr.Type;
            var elemType = arrType.ElementType;
            var vals = CLR.ReadArrayValues(f, elemType);

            int n = vals.Count;
            var code = new Code();
            for (int i = 0; i < n; ++i)
            {
                //put array onto the stack
                code.Add(_provider.Dup());
                code.AddRange(_provider.LoadConstant(i)); //index
                code.AddRange(_provider.LoadConstant(vals[i]));
                code.AddRange(_provider.SetArrayElem(elemType));
            }

            //Note: Now we must remove from stack array because it does InitializeArray CLR method
            code.Add(_provider.Pop());

            return code.ToArray();
        }

        bool IsGetTypeFromHandle(IMethod m)
        {
            if (!CLR.IsGetTypeFromHandle(m)) return false;
            Debug.Assert(!IsStackEmpty);
            return Peek().IsTypeToken;
        }

        IInstruction[] TypeOf()
        {
            var token = Pop();
            var type = token.instruction.Type;
            PushResult(SystemTypes.Type);
            return _provider.TypeOf(type);
        }

        IInstruction[] Op_Calli()
        {
            //TODO: We can do that
            var method = _instruction.Method;
            throw new NotSupportedException();
        }

        IInstruction[] Op_Newobj()
        {
            var ctor = _instruction.Method;
            var code = new Code();

            IType rtype = null;
            PopArgs(code, ctor, ref rtype);

            var type = ctor.DeclaringType;
            PushResult(type);

            //FixTernaryAssignment(type);

            return Call(code, rtype, ctor, 0);
        }

        IInstruction[] Op_Initobj()
        {
            var addr = Pop();

            switch (addr.value.Kind)
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

            var code = new Code();
            var type = _instruction.Type;

            var il = _provider.InitObject(type);
            code.AddRange(il);

            StorePtr(code, addr.value, type);
            
            return code.ToArray();
        }
        #endregion

        #region array instructions
        IInstruction[] Op_Newarr()
        {
            var n = Pop();
            CheckPointer(n);

            var nType = SystemTypes.Int32;
            var code = new Code();
            if (!FixTernaryAssignment(nType))
                Cast(code, n.Type, nType);

            var elemType = _instruction.Type;

            PushResult(TypeFactory.MakeArray(elemType));

            code.AddRange(_provider.NewArray(elemType));

            return code.ToArray();
        }

        IInstruction[] Op_Ldlen()
        {
            var arr = Pop();
            CheckArray(arr);
            PushResult(SystemTypes.Int32);
            return _provider.GetArrayLength();
        }

        static IType GetElemType(IType type)
        {
            var ct = type as ICompoundType;
            if (ct == null)
                throw new ArgumentException();
            return ct.ElementType;
        }

        IInstruction[] Op_Ldelem()
        {
            var index = Pop();
            var arr = Pop();
            CheckArray(arr);

            var elemType = GetElemType(arr.Type);

            var code = new Code();
            code.AddRange(_provider.GetArrayElem(elemType));
            PassByValue(code, elemType);

            Push(new Elem(elemType));

            return code.ToArray();
        }

        IInstruction[] Op_Ldelema()
        {
            var index = Pop();
            var arr = Pop();
            CheckArray(arr);

            var elemType = GetElemType(arr.Type);

            var code = new Code();

            //cast index to int
            Cast(code, index.Type, SystemTypes.Int32);

            if (IsByRef)
            {
                var il = _provider.GetElemPtr(elemType);
                code.AddRange(il);

                Push(new ElemPtr(arr.Type, elemType));
            }
            else
            {
                int vindex = StoreTemp(code);
                int varr = StoreTemp(code);
                Push(new MockElemPtr(arr.Type, elemType, varr, vindex));
            }

            return code.ToArray();
        }

        //store element to array
        //stack transition: ..., array, index, value, -> ...
        IInstruction[] Op_Stelem()
        {
            var value = Pop();
            var index = Pop();
            var arr = Pop();
            CheckArray(arr);
            CheckPointer(index);

            var elemType = GetElemType(arr.Type);

            var code = new Code();
            BeforeStoreValue(code, value, elemType);
            code.AddRange(_provider.SetArrayElem(elemType));
            return code.ToArray();
        }
        #endregion

        #region exceptions
        IInstruction[] Op_Throw()
        {
            Pop();
            var code = new Code();
            code.AddRange(_provider.Throw());
            //NOTE: Super fix for throw immediatly problem
            //TODO: Check when nop is not needed, i.e. check end of protected region.
            code.Add(_provider.Nop());
            return code.ToArray();
        }

        IInstruction[] Op_Rethrow()
        {
            return _provider.Rethrow();
        }

        IInstruction[] Op_Leave()
        {
            return Op_Branch();
        }

        IInstruction[] Op_Endfinally()
        {
            return Op_Branch();
        }

        IInstruction[] Op_Endfilter()
        {
            //TODO:
            return null;
        }
        #endregion

        #region misc
        IInstruction[] Op_Nop()
        {
            var nop = _provider.Nop();
            if (nop == null) return null;
            return new[] { nop };
        }

        IInstruction[] Op_DebuggerBreak()
        {
            return _provider.DebuggerBreak();
        }

        IInstruction[] Op_Return()
        {
            var code = new Code();
            bool isvoid = TypeService.IsVoid(_method);
            if (!isvoid)
            {
                var v = Pop();
                Cast(code, v.Type, _method.Type);
            }
            code.AddRange(_provider.Return(isvoid));
            return code.ToArray();
        }

        IInstruction[] Op_Sizeof()
        {
            var type = _instruction.Type;
            throw new NotSupportedException();
        }
        #endregion

        #region utils
        int MoveTemp(Code code, int var)
        {
            LoadTemp(code, var);
            return StoreTemp(code);
        }

        void LoadTemp(Code code, int var)
        {
            code.AddRange(_provider.GetTempVar(var));
        }

        int StoreTemp(Code code)
        {
            int var;
            code.AddRange(_provider.SetTempVar(out var, false));
            return var;
        }

        void KillTemp(Code code, int var)
        {
            code.AddRange(_provider.KillTempVar(var));
        }

        static void CheckArray(EvalItem arr)
        {
            var arrType = arr.Type;
            if (arrType.TypeKind != TypeKind.Array)
                throw new InvalidOperationException();
        }

        static void CheckPointer(EvalItem v)
        {
            if (v.IsPointer)
                throw new InvalidOperationException();
        }

        void Push(IValue value)
        {
            _block.Stack.Push(_instruction, value);
        }

        void PushResult(IType type)
        {
            Push(new ComputedValue(type));
        }

        EvalStack Stack
        {
            get { return _block.Stack; }
        }

        bool IsStackEmpty
        {
            get { return Stack.Count == 0; }
        }

        EvalItem Peek()
        {
            return _block.Stack.Peek();
        }

        EvalItem Pop()
        {
            var stack = _block.Stack;
            if (stack.Count == 0)
            {
                if (!_instruction.IsHandlerBegin)
                {
                    throw new ILTranslatorException("EvalStack is empty");
                }
                var cb = _instruction.Block as HandlerBlock;
                if (cb == null)
                    throw new ILTranslatorException();
                return new EvalItem(null, new ComputedValue(cb.ExceptionType));
            }
            return stack.Pop();
        }

        IValue PopValue()
        {
            return Pop().value;
        }

        void CastToInt32(Code code, ref IType type)
        {
            Cast(code, type, SystemTypes.Int32);
            type = SystemTypes.Int32;
        }
        #endregion
    }
}