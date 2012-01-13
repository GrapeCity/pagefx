using System;
using System.Collections.Generic;
using System.IO;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.IL
{
    internal class InstructionDecompiler
    {
        #region Fields
        private readonly Stack<IExpression> _stack;
        private readonly IMethodBody _body;
        #endregion

        #region ctor
        public InstructionDecompiler(IMethodBody body, Stack<IExpression> stack)
        {
            _body = body;
            _stack = stack;
        }
        #endregion

        #region Properties
        public bool NegateBranches
        {
            get { return _negateBranches; }
            set { _negateBranches = value; }
        }
        private bool _negateBranches;

        public ICatchClause CatchClause
        {
            get { return _catchClause; }
            set { _catchClause = value; }
        }
        private ICatchClause _catchClause;

        private IMethod Method
        {
            get { return _body.Method; }
        }
        #endregion

        #region DecompileInstruction
        private Instruction _instruction;

        public ICodeNode Decompile(Instruction instruction)
        {
            _instruction = instruction;
            InstructionCode code = (InstructionCode)instruction.OpCode.Value;
            switch (code)
            {
                case InstructionCode.Nop:
                    return null;

                #region load value
                #region ldc
                case InstructionCode.Ldc_I4:
                case InstructionCode.Ldc_I4_S:
                    return new ConstExpression((int)instruction.Value);

                case InstructionCode.Ldc_I4_0:
                    return new ConstExpression(0);
                case InstructionCode.Ldc_I4_1:
                    return new ConstExpression(1);
                case InstructionCode.Ldc_I4_2:
                    return new ConstExpression(2);
                case InstructionCode.Ldc_I4_3:
                    return new ConstExpression(3);
                case InstructionCode.Ldc_I4_4:
                    return new ConstExpression(4);
                case InstructionCode.Ldc_I4_5:
                    return new ConstExpression(5);
                case InstructionCode.Ldc_I4_6:
                    return new ConstExpression(6);
                case InstructionCode.Ldc_I4_7:
                    return new ConstExpression(7);
                case InstructionCode.Ldc_I4_8:
                    return new ConstExpression(8);
                case InstructionCode.Ldc_I4_M1:
                    return new ConstExpression(-1);

                case InstructionCode.Ldc_I8:
                    return new ConstExpression((long)instruction.Value);

                case InstructionCode.Ldc_R4:
                    return new ConstExpression((float)instruction.Value);

                case InstructionCode.Ldc_R8:
                    return new ConstExpression((double)instruction.Value);
                #endregion

                case InstructionCode.Ldnull:
                    return new ConstExpression(null);

                case InstructionCode.Ldstr:
                    return new ConstExpression(instruction.Value);

                #region ldarg
                case InstructionCode.Ldarg_0:
                    return GetArgRef(0);
                case InstructionCode.Ldarg_1:
                    return GetArgRef(1);
                case InstructionCode.Ldarg_2:
                    return GetArgRef(2);
                case InstructionCode.Ldarg_3:
                    return GetArgRef(3);

                case InstructionCode.Ldarg:
                case InstructionCode.Ldarg_S:
                    return GetArgRef((int)instruction.Value);

                case InstructionCode.Ldarga:
                case InstructionCode.Ldarga_S:
                    return GetArgRef((int)instruction.Value, false);
                #endregion

                #region ldloc
                case InstructionCode.Ldloc_0:
                    return GetVarRef(0);
                case InstructionCode.Ldloc_1:
                    return GetVarRef(1);
                case InstructionCode.Ldloc_2:
                    return GetVarRef(2);
                case InstructionCode.Ldloc_3:
                    return GetVarRef(3);
                case InstructionCode.Ldloc:
                case InstructionCode.Ldloc_S:
                    return GetVarRef((int)instruction.Value);

                case InstructionCode.Ldloca:
                case InstructionCode.Ldloca_S:
                    return GetVarRef((int)instruction.Value, false);
                #endregion

                #region ldelem
                case InstructionCode.Ldelem:
                case InstructionCode.Ldelema:
                    return Ldelem((IType)instruction.Value);

                case InstructionCode.Ldelem_I:
                    return Ldelem(SystemTypes.IntPtr);
                case InstructionCode.Ldelem_I1:
                    return Ldelem(SystemTypes.Int8);
                case InstructionCode.Ldelem_I2:
                    return Ldelem(SystemTypes.Int16);
                case InstructionCode.Ldelem_I4:
                    return Ldelem(SystemTypes.Int32);
                case InstructionCode.Ldelem_I8:
                    return Ldelem(SystemTypes.Int64);
                case InstructionCode.Ldelem_R4:
                    return Ldelem(SystemTypes.Float32);
                case InstructionCode.Ldelem_R8:
                    return Ldelem(SystemTypes.Float64);
                case InstructionCode.Ldelem_Ref:
                    return Ldelem(SystemTypes.Object);
                case InstructionCode.Ldelem_U1:
                    return Ldelem(SystemTypes.UInt8);
                case InstructionCode.Ldelem_U2:
                    return Ldelem(SystemTypes.UInt16);
                case InstructionCode.Ldelem_U4:
                    return Ldelem(SystemTypes.Int32);
                #endregion

                #region ldind
                //load value indirect onto the stack
                case InstructionCode.Ldind_I:
                    return Ldind(SystemTypes.IntPtr);
                case InstructionCode.Ldind_I1:
                    return Ldind(SystemTypes.Int8);
                case InstructionCode.Ldind_I2:
                    return Ldind(SystemTypes.Int16);
                case InstructionCode.Ldind_I4:
                    return Ldind(SystemTypes.Int32);
                case InstructionCode.Ldind_I8:
                    return Ldind(SystemTypes.Int64);
                case InstructionCode.Ldind_R4:
                    return Ldind(SystemTypes.Float32);
                case InstructionCode.Ldind_R8:
                    return Ldind(SystemTypes.Float64);
                case InstructionCode.Ldind_Ref:
                    return Ldind(SystemTypes.Object);
                case InstructionCode.Ldind_U1:
                    return Ldind(SystemTypes.UInt8);
                case InstructionCode.Ldind_U2:
                    return Ldind(SystemTypes.UInt16);
                case InstructionCode.Ldind_U4:
                    return Ldind(SystemTypes.UInt32);
                #endregion

                #region ldfld
                case InstructionCode.Ldfld:
                case InstructionCode.Ldsfld:
                case InstructionCode.Ldsflda:
                case InstructionCode.Ldflda:
                    return GetFieldRef((IField)instruction.Value);
                #endregion

                //load the length of an array
                case InstructionCode.Ldlen:
                    {
                        IExpression arr = Pop();
                        return new ArrayLengthExpression(arr);
                    }

                case InstructionCode.Ldftn:
                    {
                        IMethod m = (IMethod)instruction.Value;
                        return new MethodReferenceExpression(m);
                    }

                case InstructionCode.Ldvirtftn:
                    {
                        IMethod m = (IMethod)instruction.Value;
                        IExpression obj = Pop();
                        return new MethodReferenceExpression(obj, m);
                    }

                //copy a value from an address to the stack
                case InstructionCode.Ldobj:
                    {
                        IExpression src = Pop();
                        return src;
                    }

                case InstructionCode.Ldtoken:
                    {
                        ITypeMember m = (ITypeMember)instruction.Value;
                        //if (m.MemberType == TypeMemberType.Field)
                        //{
                        //    IField f = (IField)m;
                        //    if (f.IsConstant)
                        //    {
                        //        return new ConstExpression(f.Value);
                        //    }
                        //}
                        return GetMemberRef(m);
                    }
                #endregion

                #region store value
                #region stloc
                case InstructionCode.Stloc_0:
                    return Assign(GetVarRef(0));

                case InstructionCode.Stloc_1:
                    return Assign(GetVarRef(1));

                case InstructionCode.Stloc_2:
                    return Assign(GetVarRef(2));

                case InstructionCode.Stloc_3:
                    return Assign(GetVarRef(3));

                case InstructionCode.Stloc:
                case InstructionCode.Stloc_S:
                    return Assign(GetVarRef((int)instruction.Value));
                #endregion

                #region starg
                case InstructionCode.Starg:
                case InstructionCode.Starg_S:
                    return Assign(GetArgRef((int)instruction.Value));
                #endregion

                #region stelem
                case InstructionCode.Stelem:
                    return Stelem((IType)instruction.Value);

                case InstructionCode.Stelem_I:
                    return Stelem(SystemTypes.IntPtr);
                case InstructionCode.Stelem_I1:
                    return Stelem(SystemTypes.Int8);
                case InstructionCode.Stelem_I2:
                    return Stelem(SystemTypes.Int16);
                case InstructionCode.Stelem_I4:
                    return Stelem(SystemTypes.Int32);
                case InstructionCode.Stelem_I8:
                    return Stelem(SystemTypes.Int64);
                case InstructionCode.Stelem_R4:
                    return Stelem(SystemTypes.Float32);
                case InstructionCode.Stelem_R8:
                    return Stelem(SystemTypes.Float64);
                case InstructionCode.Stelem_Ref:
                    return Stelem(SystemTypes.Object);
                #endregion

                #region stind
                case InstructionCode.Stind_I:
                    return Stind(SystemTypes.IntPtr);
                case InstructionCode.Stind_I1:
                    return Stind(SystemTypes.Int8);
                case InstructionCode.Stind_I2:
                    return Stind(SystemTypes.Int16);
                case InstructionCode.Stind_I4:
                    return Stind(SystemTypes.Int32);
                case InstructionCode.Stind_I8:
                    return Stind(SystemTypes.Int64);
                case InstructionCode.Stind_R4:
                    return Stind(SystemTypes.Float32);
                case InstructionCode.Stind_R8:
                    return Stind(SystemTypes.Float64);
                case InstructionCode.Stind_Ref:
                    return Stind(SystemTypes.Object);
                #endregion

                case InstructionCode.Stfld:
                case InstructionCode.Stsfld:
                    {
                        IExpression value = Pop();
                        IFieldReferenceExpression field = GetFieldRef((IField)instruction.Value);
                        return AssignInc(field, value);
                    }

                case InstructionCode.Stobj:
                    {
                        //TODO:
                        IType type = (IType)instruction.Value;
                        IExpression src = Pop();
                        IExpression dest = Pop();
                        return Assign(dest, src);
                    }
                #endregion

                #region Expressions
                #region cast
                case InstructionCode.Conv_I:
                    return Cast(SystemTypes.IntPtr);
                case InstructionCode.Conv_Ovf_I:
                case InstructionCode.Conv_Ovf_I_Un:
                    return Cast(SystemTypes.IntPtr, true);

                case InstructionCode.Conv_I1:
                    return Cast(SystemTypes.Int8);
                case InstructionCode.Conv_I2:
                    return Cast(SystemTypes.Int16);
                case InstructionCode.Conv_I4:
                    return Cast(SystemTypes.Int32);
                case InstructionCode.Conv_I8:
                    return Cast(SystemTypes.Int64);

                case InstructionCode.Conv_Ovf_I1:
                case InstructionCode.Conv_Ovf_I1_Un:
                    return Cast(SystemTypes.Int8, true);

                case InstructionCode.Conv_Ovf_I2:
                case InstructionCode.Conv_Ovf_I2_Un:
                    return Cast(SystemTypes.Int16, true);

                case InstructionCode.Conv_Ovf_I4:
                case InstructionCode.Conv_Ovf_I4_Un:
                    return Cast(SystemTypes.Int32, true);

                case InstructionCode.Conv_Ovf_I8:
                case InstructionCode.Conv_Ovf_I8_Un:
                    return Cast(SystemTypes.Int64, true);

                case InstructionCode.Conv_U:
                    return Cast(SystemTypes.NativeUInt);
                case InstructionCode.Conv_Ovf_U:
                case InstructionCode.Conv_Ovf_U_Un:
                    return Cast(SystemTypes.NativeUInt, true);

                case InstructionCode.Conv_Ovf_U1:
                case InstructionCode.Conv_Ovf_U1_Un:
                    return Cast(SystemTypes.UInt8, true);

                case InstructionCode.Conv_Ovf_U2:
                case InstructionCode.Conv_Ovf_U2_Un:
                    return Cast(SystemTypes.UInt16, true);

                case InstructionCode.Conv_Ovf_U4:
                case InstructionCode.Conv_Ovf_U4_Un:
                    return Cast(SystemTypes.UInt16, true);

                case InstructionCode.Conv_Ovf_U8:
                case InstructionCode.Conv_Ovf_U8_Un:
                    return Cast(SystemTypes.UInt8, true);

                case InstructionCode.Conv_R_Un:
                case InstructionCode.Conv_R4:
                    return Cast(SystemTypes.Float32);

                case InstructionCode.Conv_R8:
                    return Cast(SystemTypes.Float64);

                case InstructionCode.Conv_U1:
                    return Cast(SystemTypes.UInt8);
                case InstructionCode.Conv_U2:
                    return Cast(SystemTypes.UInt16);
                case InstructionCode.Conv_U4:
                    return Cast(SystemTypes.UInt32);
                case InstructionCode.Conv_U8:
                    return Cast(SystemTypes.UInt64);

                case InstructionCode.Castclass:
                    return As((IType)instruction.Value);

                case InstructionCode.Isinst:
                    return Is((IType)instruction.Value);
                #endregion

                #region operators
                case InstructionCode.Add:
                    return Op(BinaryOperator.Addition);
                case InstructionCode.Add_Ovf:
                case InstructionCode.Add_Ovf_Un:
                    return Op(BinaryOperator.Addition, true);

                case InstructionCode.Sub:
                    return Op(BinaryOperator.Subtraction);
                case InstructionCode.Sub_Ovf:
                case InstructionCode.Sub_Ovf_Un:
                    return Op(BinaryOperator.Subtraction, true);

                case InstructionCode.Mul:
                    return Op(BinaryOperator.Multiply);
                case InstructionCode.Mul_Ovf:
                case InstructionCode.Mul_Ovf_Un:
                    return Op(BinaryOperator.Multiply, true);

                case InstructionCode.Div:
                case InstructionCode.Div_Un:
                    return Op(BinaryOperator.Division);

                case InstructionCode.Rem:
                case InstructionCode.Rem_Un:
                    return Op(BinaryOperator.Modulus);

                case InstructionCode.And:
                    return Op(BinaryOperator.BitwiseAnd);
                case InstructionCode.Or:
                    return Op(BinaryOperator.BitwiseOr);
                case InstructionCode.Xor:
                    return Op(BinaryOperator.ExclusiveOr);
                case InstructionCode.Shl:
                    return Op(BinaryOperator.LeftShift);
                case InstructionCode.Shr:
                case InstructionCode.Shr_Un:
                    return Op(BinaryOperator.RightShift);
                case InstructionCode.Neg:
                    return Op(UnaryOperator.Negate);
                case InstructionCode.Not:
                    return Op(UnaryOperator.BitwiseNot);

                case InstructionCode.Ceq:
                    return Op(BinaryOperator.Equality);

                case InstructionCode.Cgt:
                case InstructionCode.Cgt_Un:
                    return Op(BinaryOperator.GreaterThan);

                case InstructionCode.Clt:
                case InstructionCode.Clt_Un:
                    return Op(BinaryOperator.LessThan);
                #endregion

                case InstructionCode.Call:
                case InstructionCode.Callvirt:
                    return Call((IMethod)instruction.Value);

                case InstructionCode.Calli:
                    {
                        IMethod sig = (IMethod)instruction.Value;
                        IExpression ftn = Pop();
                        IMethodReferenceExpression mr = new MethodReferenceExpression(ftn, sig);
                        CallExpression e = new CallExpression(mr);
                        PopArguments(sig, e.Arguments);
                        return e;
                    }

                case InstructionCode.Newobj:
                    return Newobj((IMethod)instruction.Value);

                //create a zero-based, one-dimensional array
                case InstructionCode.Newarr:
                    return Newarr((IType)instruction.Value);

                case InstructionCode.Dup:
                    {
                        IExpression e = _stack.Peek();
                        return e;
                    }

                case InstructionCode.Arglist:
                    {
                        return new ExpressionCollection();
                    }

                #region box/unbox
                case InstructionCode.Box:
                    {
                        IExpression e = Pop();
                        return new BoxExpression((IType)instruction.Value, e);
                        //return new CastExpression(SystemTypes.Object, e, CastOperator.Cast);
                    }

                case InstructionCode.Unbox:
                    {
                        IExpression e = Pop();
                        return new UnboxExpression((IType)instruction.Value, e);
                    }

                case InstructionCode.Unbox_Any:
                    {
                        IExpression e = Pop();
                        return new UnboxExpression((IType)instruction.Value, e);
                    }
                #endregion

                case InstructionCode.Localloc:
                    {
                        //TODO:
                        return ExpList(1);
                    }

                case InstructionCode.Sizeof:
                    {
                        IType type = (IType)instruction.Value;
                        return new SizeOfExpression(type);
                    }
                #endregion

                #region Branches
                case InstructionCode.Beq:
                case InstructionCode.Beq_S:
                    return BinBr(BinaryOperator.Equality);

                case InstructionCode.Bne_Un:
                case InstructionCode.Bne_Un_S:
                    return BinBr(BinaryOperator.Inequality);

                case InstructionCode.Bge:
                case InstructionCode.Bge_S:
                case InstructionCode.Bge_Un:
                case InstructionCode.Bge_Un_S:
                    return BinBr(BinaryOperator.GreaterThanOrEqual);

                case InstructionCode.Bgt:
                case InstructionCode.Bgt_S:
                case InstructionCode.Bgt_Un:
                case InstructionCode.Bgt_Un_S:
                    return BinBr(BinaryOperator.GreaterThan);

                case InstructionCode.Ble:
                case InstructionCode.Ble_S:
                case InstructionCode.Ble_Un:
                case InstructionCode.Ble_Un_S:
                    return BinBr(BinaryOperator.LessThanOrEqual);

                case InstructionCode.Blt:
                case InstructionCode.Blt_S:
                case InstructionCode.Blt_Un:
                case InstructionCode.Blt_Un_S:
                    return BinBr(BinaryOperator.LessThan);

                case InstructionCode.Brfalse:
                case InstructionCode.Brfalse_S:
                    {
                        IExpression e = Pop();
                        if (_negateBranches) return e;
                        return BooleanAlgebra.Not(e);
                    }

                case InstructionCode.Brtrue:
                case InstructionCode.Brtrue_S:
                    {
                        IExpression e = Pop();
                        if (_negateBranches) return BooleanAlgebra.Not(e);
                        return e;
                    }

                case InstructionCode.Switch:
                    {
                        IExpression e = Pop();
                        return e;
                    }

                case InstructionCode.Endfilter:
                case InstructionCode.Endfinally:
                case InstructionCode.Leave:
                case InstructionCode.Leave_S:
                case InstructionCode.Br:
                case InstructionCode.Br_S:
                    return null;
                #endregion

                #region Statements
                case InstructionCode.Pop:
                    {
                        if (_stack.Count == 0)
                        {
                            if (IsCatchBlock)
                                return null;
                        }
                        IExpression e = Pop();
                        return new ExpressionStatement(e);
                    }

                case InstructionCode.Ret:
                    return Return();

                case InstructionCode.Break:
                    return new DebuggerBreakStatement();

                #region throw/rethrow
                case InstructionCode.Throw:
                    return Throw(false);
                case InstructionCode.Rethrow:
                    return Throw(true);
                #endregion

                case InstructionCode.Jmp:
                    {
                        //IMethod m = (IMethod)i.Value;
                        throw new NotImplementedException();
                    }

                case InstructionCode.Ckfinite:
                    {
                        return null;
                    }

                case InstructionCode.Cpobj:
                    {
                        return null;
                    }

                case InstructionCode.Cpblk:
                    {
                        MemoryCopyStatement st = new MemoryCopyStatement();
                        st.Destination = Pop();
                        st.Source = Pop();
                        st.Size = Pop();
                        return st;
                    }

                case InstructionCode.Initblk:
                    {
                        MemoryInitializeStatement st = new MemoryInitializeStatement();
                        st.Offset = Pop();
                        st.Value = Pop();
                        st.Size = Pop();
                        return st;
                    }
                #endregion

                #region Prefixes to instructions
                case InstructionCode.Constrained:
                case InstructionCode.Readonly:
                case InstructionCode.Prefix1:
                case InstructionCode.Prefix2:
                case InstructionCode.Prefix3:
                case InstructionCode.Prefix4:
                case InstructionCode.Prefix5:
                case InstructionCode.Prefix6:
                case InstructionCode.Prefix7:
                case InstructionCode.Prefixref:
                case InstructionCode.Tailcall:
                case InstructionCode.Unaligned:
                case InstructionCode.Volatile:
                    return null;
                #endregion

                case InstructionCode.Initobj:
                    {
                        //TODO:
                        IType type = (IType)instruction.Value;
                        IExpression dest = Pop();
                        NewObjectExpression e = new NewObjectExpression();
                        e.ObjectType = type;
                        BinaryExpression assign = new BinaryExpression(dest, e, BinaryOperator.Assign);
                        return new ExpressionStatement(assign);
                    }

                case InstructionCode.Mkrefany:
                    {
                        IExpression ptr = Pop();
                        //TODO:
                        return ptr;
                    }

                case InstructionCode.Refanytype:
                    {
                        IExpression typedRef = Pop();
                        //TODO:
                        return typedRef;
                    }

                case InstructionCode.Refanyval:
                    {
                        IExpression typedRef = Pop();
                        //TODO:
                        return typedRef;
                    }

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private IExpression BinBr(BinaryOperator op)
        {
            if (_negateBranches)
                op = ExpressionService.Negate(op);
            return Op(op);
        }
        #endregion

        #region Expressions
        private IExpression Ldind(IType type)
        {
            IExpression addr = Pop();
            //return new AddressDereferenceExpression(type, addr);
            return addr;
        }

        private ICodeNode Stind(IType type)
        {
            IExpression value = Pop();
            IExpression addr = Pop();
            //IExpression left = new AddressDereferenceExpression(type, addr);
            IExpression left = addr;
            return AssignInc(left, value);
        }

        private IArrayIndexerExpression GetArrayIndexer(IType type)
        {
            IExpression index = Pop();
            IExpression arr = Pop();
            return new ArrayIndexerExpression(arr, index);
        }

        /// <summary>
        /// load an element of an array
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private IExpression Ldelem(IType type)
        {
            return GetArrayIndexer(type);
        }

        private ICodeNode Stelem(IType type)
        {
            IExpression value = Pop();
            IExpression arr = GetArrayIndexer(type);
            return AssignInc(arr, value);
        }

        private IExpression ExpList(int n)
        {
            ExpressionCollection res = new ExpressionCollection();
            for (int i = 0; i < n; ++i)
            {
                IExpression e = Pop();
                res.Add(e);
            }
            return res;
        }

        private void PopArguments(IMethod m, ICollection<IExpression> args, int n)
        {
            if (n > 0)
            {
                IExpression[] arr = new IExpression[n];
                for (int i = 0; i < n; ++i)
                {
                    IExpression arg = Pop();
                    arr[n - 1 - i] = arg;
                }
                for (int i = 0; i < n; ++i)
                {
                    IExpression arg = arr[i];
                    IParameter p = m.Parameters[i];
                    arg = ExpressionService.FixConstant(arg, p.Type);
                    args.Add(arg);
                }
            }
        }

        private void PopArguments(IMethod m, IList<IExpression> args)
        {
            PopArguments(m, args, m.Parameters.Count);
        }

        /// <summary>
        /// creates a new object
        /// </summary>
        /// <param name="ctor"></param>
        /// <returns></returns>
        private IExpression Newobj(IMethod ctor)
        {
            IType type = ctor.DeclaringType;
            if (type.Kind == TypeKind.Delegate)
            {
                IMethodReferenceExpression m = Pop() as IMethodReferenceExpression;
                if (m == null)
                    throw new InvalidOperationException();
                IExpression obj = Pop();
                if (m.Method.IsStatic)
                {
                    if (m.Target == null)
                        m.Target = new TypeReferenceExpression(m.Method.DeclaringType);
                }
                else
                {
                    m.Target = obj;
                }
                return new NewDelegateExpression(type, m);
            }
            else
            {
                NewObjectExpression e = new NewObjectExpression();
                e.Constructor = ctor;
                e.ObjectType = type;
                PopArguments(ctor, e.Arguments);
                return e;
            }
        }

        /// <summary>
        /// creates a zero-based, one-dimensional array
        /// </summary>
        /// <param name="elemType"></param>
        /// <returns></returns>
        private IExpression Newarr(IType elemType)
        {
            IExpression size = Pop();
            NewArrayExpression e = new NewArrayExpression();
            e.Dimensions.Add(size);
            e.ElementType = elemType;
            return e;
        }

        private ICodeNode _CallExp(IMethod m)
        {
            CallExpression e = new CallExpression();
            PopArguments(m, e.Arguments);
            e.Method = GetMethodRef(m);
            if (e.Method == null)
                throw new DecompileException();
            if (TypeService.IsVoid(m))
                return new ExpressionStatement(e);
            return e;
        }

        private ICodeNode _Call(IMethod m)
        {
            IType type = m.DeclaringType;
            if (type.Kind == TypeKind.Delegate)
            {
                if (m.Name == "Invoke")
                {
                    DelegateInvokeExpression e = new DelegateInvokeExpression();
                    e.Method = m;
                    PopArguments(m, e.Arguments);
                    e.Target = Pop();
                    if (TypeService.IsVoid(m))
                        return new ExpressionStatement(e);
                    return e;
                }
            }
            return _CallExp(m);
        }

        private ICodeNode Call(IMethod m)
        {
            ITypeMember assoc = m.Association;
            if (assoc != null)
            {
                IProperty prop = assoc as IProperty;
                if (prop != null)
                {
                    if (m == prop.Getter)
                    {
                        //indexer?
                        if (m.Parameters.Count > 0)
                        {
                            IndexerExpression indexer = new IndexerExpression();
                            PopArguments(m, indexer.Index);
                            indexer.Property = GetPropertyRef(prop);
                            return indexer;
                        }
                        return GetPropertyRef(prop);
                    }
                    else
                    {
                        if (m != prop.Setter)
                            throw new InvalidOperationException();

                        //indexer?
                        IExpression value = Pop();
                        if (m.Parameters.Count > 1)
                        {
                            IndexerExpression indexer = new IndexerExpression();
                            PopArguments(m, indexer.Index, m.Parameters.Count - 1);
                            indexer.Property = GetPropertyRef(prop);
                            return AssignInc(indexer, value);
                        }

                        IPropertyReferenceExpression pref = GetPropertyRef(prop);
                        return AssignInc(pref, value);
                    }
                }
            }
            ICodeNode node = _Call(m);
            ICallExpression call = node as ICallExpression;
            if (call != null)
            {
                return TranslateCall(call);
            }
            IStatement st = node as IStatement;
            if (st != null)
            {
                node = TranslateStatement(st);
            }
            return node;
        }

        private static IExpression TranslateCall(ICallExpression call)
        {
            IExpression e = CLR.TypeOf(call);
            if (e != null) return e;
            return call;
        }

        private static ICodeNode TranslateStatement(IStatement st)
        {
            if (CLR.InitializeArray(st)) return null;
            return st;
        }

        private IExpression GetArgRef(int index, bool address)
        {
            IExpression e;
            if (!Method.IsStatic)
            {
                if (index == 0)
                {
                    e = new ThisReferenceExpression(Method.DeclaringType);
                    if (address)
                        return new AddressOfExpression(e);
                    return e;
                }
                --index;
            }

            if (index < 0 || index >= Method.Parameters.Count)
                throw new IndexOutOfRangeException();

            IParameter p = Method.Parameters[index];
            e = new ArgumentReferenceExpression(p);
            if (address)
                return new AddressOfExpression(e);
            return e;
        }

        private IExpression GetArgRef(int index)
        {
            return GetArgRef(index, false);
        }

        private IExpression GetVarRef(int index, bool address)
        {
            IVariable v = _body.LocalVariables[index];
            IExpression e = new VariableReferenceExpression(v);
            if (address) return new AddressOfExpression(e);
            return e;
        }

        private IExpression GetVarRef(int index)
        {
            return GetVarRef(index, false);
        }

        private IExpression GetTarget(ITypeMember m)
        {
            if (m.IsStatic)
                return new TypeReferenceExpression(m.DeclaringType);

            IExpression e = Pop();
            IThisReferenceExpression thisRef = e as IThisReferenceExpression;
            if (thisRef != null)
            {
                if (m.DeclaringType != Method.DeclaringType)
                    return new BaseReferenceExpression(m.DeclaringType);
            }
            return e;
        }

        private IExpression GetMemberRef(ITypeMember member)
        {
            IType type = member as IType;
            if (type != null)
                return new TypeReferenceExpression(type);
            IExpression target = GetTarget(member);
            switch (member.MemberType)
            {
                case TypeMemberType.Field:
                    return new FieldReferenceExpression(target, (IField)member);

                case TypeMemberType.Method:
                case TypeMemberType.Constructor:
                    return new MethodReferenceExpression(target, (IMethod)member);

                case TypeMemberType.Property:
                    return new PropertyReferenceExpression(target, (IProperty)member);

                case TypeMemberType.Event:
                    return new EventReferenceExpression(target, (IEvent)member);

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private IFieldReferenceExpression GetFieldRef(IField field)
        {
            IExpression target = GetTarget(field);
            return new FieldReferenceExpression(target, field);
        }

        private IPropertyReferenceExpression GetPropertyRef(IProperty prop)
        {
            IExpression target = GetTarget(prop);
            return new PropertyReferenceExpression(target, prop);
        }

        private IMethodReferenceExpression GetMethodRef(IMethod method)
        {
            IExpression target = GetTarget(method);
            return new MethodReferenceExpression(target, method);
        }

        private IExpression Cast(IType targetType, bool ovf)
        {
            IExpression e = Pop();
            return new CastExpression(targetType, e, CastOperator.Cast);
        }

        private IExpression Cast(IType targetType)
        {
            return Cast(targetType, false);
        }

        private IExpression Is(IType targetType)
        {
            IExpression e = Pop();
            return new CastExpression(targetType, e, CastOperator.Is);
        }

        private IExpression As(IType targetType)
        {
            IExpression e = Pop();
            return new CastExpression(targetType, e, CastOperator.As);
        }

        private IExpression Op(BinaryOperator op, bool ovf)
        {
            IExpression right = Pop();
            IExpression left = Pop();
            IBinaryExpression e = new BinaryExpression(left, right, op);
            return BooleanAlgebra.Simplify(e);
        }

        private IExpression Op(BinaryOperator op)
        {
            return Op(op, false);
        }

        private IExpression Op(UnaryOperator op)
        {
            IExpression e = Pop();
            return new UnaryExpression(e, op);
        }

        private static bool IsConstOne(IExpression e)
        {
            IConstantExpression c = e as IConstantExpression;
            if (c != null)
            {
                object value = c.Value;
                if (value != null)
                {
                    TypeCode tc = Type.GetTypeCode(value.GetType());
                    switch(tc)
                    {
                        case TypeCode.SByte:
                            return (sbyte)value == 1;
                        case TypeCode.Byte:
                            return (byte)value == 1;
                        case TypeCode.Int16:
                            return (short)value == 1;
                        case TypeCode.UInt16:
                            return (ushort)value == 1;
                        case TypeCode.Int32:
                            return (int)value == 1;
                        case TypeCode.UInt32:
                            return (uint)value == 1;
                        case TypeCode.Int64:
                            return (long)value == 1;
                        case TypeCode.UInt64:
                            return (ulong)value == 1;
                        case TypeCode.Single:
                            return (float)value == 1;
                        case TypeCode.Double:
                            return (double)value == 1;
                        case TypeCode.Decimal:
                            return (decimal)value == 1;
                        default:
                            return false;
                    }
                }
            }
            return false;
        }

        private ICodeNode Assign(IExpression left)
        {
            IExpression right = Pop();
            IType ltype = left.ResultType;
            if (BooleanAlgebra.IsBoolean(ltype))
            {
                right = BooleanAlgebra.ToBool(right);
                right = BooleanAlgebra.Simplify(right);
            }
            return AssignInc(left, right);
        }

        private ICodeNode AssignInc(IExpression left, IExpression right)
        {
            IBinaryExpression be = right as IBinaryExpression;
            if (be != null)
            {
                BinaryOperator op = be.Operator;
                if (IsAddOrSub(op) && Equals(left, be.Left) && IsConstOne(be.Right))
                {
                    if (op == BinaryOperator.Addition)
                    {
                        IExpression e = CreateIncrementExpression(be, UnaryOperator.PreIncrement, UnaryOperator.PostIncrement);
                        if (e != null) return e;
                    }
                    if (op == BinaryOperator.Subtraction)
                    {
                        IExpression e = CreateIncrementExpression(be, UnaryOperator.PreDecrement, UnaryOperator.PostDecrement);
                        if (e != null) return e;
                    }
                }
            }
            return Assign(left, right);
        }

        private static IStatement Assign(IExpression left, IExpression right)
        {
            IType ltype = left.ResultType;
            if (BooleanAlgebra.IsBoolean(ltype))
            {
                right = BooleanAlgebra.ToBool(right);
            }
            else
            {
                IType rtype = right.ResultType;
                if (!TypeService.HasImplicitConversion(ltype, rtype))
                    right = new CastExpression(ltype, right, CastOperator.Cast);
            }
            IExpression e = new BinaryExpression(left, right, BinaryOperator.Assign);
            return new ExpressionStatement(e);
        }

        private static bool IsAddOrSub(BinaryOperator op)
        {
            return op == BinaryOperator.Addition || op == BinaryOperator.Subtraction;
        }

        private IUnaryExpression CreateIncrementExpression(IBinaryExpression be, UnaryOperator pre, UnaryOperator post)
        {
            if (_stack.Count > 0)
            {
                IExpression top = _stack.Peek();
                if (Equals(top, be.Left))
                {
                    _stack.Pop();
                    return new UnaryExpression(be.Left, post);
                }
                if (Equals(top, be))
                {
                    _stack.Pop();
                    return new UnaryExpression(be.Left, pre);
                }
            }
            return null;
        }
        #endregion

        #region Statements
        private IStatement Return()
        {
            IType type = Method.Type;
            if (type == null || type == SystemTypes.Void) 
                return new ReturnStatement(null);
            IExpression e = Pop();
            e = ExpressionService.FixConstant(e, type);
            return new ReturnStatement(e);
        }

        private IStatement Throw(bool rethrow)
        {
            if (rethrow)
                return new ThrowExceptionStatement();
            IExpression e = Pop();
            return new ThrowExceptionStatement(e);
        }
        #endregion

        #region Utils
        private bool IsCatchBlock
        {
            get
            {
                if (_catchClause != null) return true;
                return _instruction.Block is CatchBlock;
            }
        }

        private IExpression CreateExceptionVariableReference()
        {
            if (_catchClause != null)
            {
                IVariable v = _catchClause.Variable;
                if (v == null)
                {
                    v = new Variable();
                    v.Type = _catchClause.ExceptionType;
                    v.Index = _body.LocalVariables.Count;
                    v.Name = string.Format("v{0}", v.Index);
                    _body.LocalVariables.Add(v);
                    _catchClause.Variable = v;
                }
                return new VariableReferenceExpression(v);
            }
            return null;
        }

        private IExpression Pop()
        {
            if (_stack.Count == 0)
            {
                if (IsCatchBlock)
                {
                    return CreateExceptionVariableReference();
                }
            }
            return _stack.Pop();
        }
        #endregion
    }
}