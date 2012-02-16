using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.IL
{
    using Code = List<IInstruction>;

    internal partial class ILTranslator
    {
        #region utils
        void Swap(ICollection<IInstruction> code)
        {
            var i = _provider.Swap();
            if (i == null)
                throw new NotSupportedException("Swap instruction is not supported");
            code.Add(i);
        }

        static bool IsCtor(ITypeMember m)
        {
            var method = m as IMethod;
            if (method == null) return false;
            return method.IsConstructor;
        }

    	bool MustPreventBoxing(IMethod method, IParameter arg)
        {
            return _provider.MustPreventBoxing(method, arg);
        }

        static bool IsBranchOrSwitch(IInstruction i)
        {
            return i.IsBranch || i.IsSwitch;
        }

        static bool IsCall(Instruction i)
        {
            if (i == null) return false;
            return i.FlowControl == FlowControl.Call;
        }

        /// <summary>
        /// Determines whether given instruction performs call of non static method
        /// </summary>
        /// <param name="instruction"></param>
        /// <returns></returns>
        static bool HasReceiver(Instruction instruction)
        {
            if (instruction.Code == InstructionCode.Newobj)
                return false;
            var m = instruction.Method;
            if (m == null)
                return false;
            return !m.IsStatic;
        }

        static bool HasReturnValue(Instruction instruction)
        {
            if (instruction.Code == InstructionCode.Newobj)
                return true;
            var m = instruction.Method;
            if (m == null) return false;
            return !m.IsVoid();
        }
        #endregion

        #region type utils
        void Cast(Code code, IType source, IType target)
        {
            if (target != source)
            {
                var cast = _provider.Cast(source, target, false);
                if (cast != null)
                    code.AddRange(cast);
            }
        }

        void Cast(Code code, IType source, IType target, bool swap)
        {
            if (target != source)
            {
                if (swap) Swap(code);
                Cast(code, source, target);
                if (swap) Swap(code);
            }
        }

        static IType GetBitwiseCD(IType lt, IType rt)
        {
            if (lt.IsEnum)
                lt = lt.ValueType;
            if (rt.IsEnum)
                rt = rt.ValueType;

            var l = lt.SystemType;
            if (l == null)
                throw new ILTranslatorException();
            var r = rt.SystemType;
            if (r == null)
                throw new ILTranslatorException();
            if (!l.IsNumeric)
                throw new ILTranslatorException();
            if (!r.IsNumeric)
                throw new ILTranslatorException();

            switch (l.Code)
            {
                case SystemTypeCode.Double:
                    switch (r.Code)
                    {
                        case SystemTypeCode.Decimal:
                            return rt;

                        default:
                            return lt;
                    }

                case SystemTypeCode.Single:
                    switch (r.Code)
                    {
                        case SystemTypeCode.Decimal:
                        case SystemTypeCode.Double:
                            return rt;

                        default:
                            return lt;
                    }

                case SystemTypeCode.Boolean:
                    return rt;

                case SystemTypeCode.Int8:
                case SystemTypeCode.UInt8:
                    if (r.Size <= 1)
                        return lt;
                    return rt;

                case SystemTypeCode.Int16:
                case SystemTypeCode.UInt16:
                case SystemTypeCode.Char:
                    if (r.Size <= 2)
                        return lt;
                    return rt;

                case SystemTypeCode.Int32:
                case SystemTypeCode.UInt32:
                    if (r.Size <= 4)
                        return lt;
                    return rt;

                case SystemTypeCode.Int64:
                case SystemTypeCode.UInt64:
                    if (r.Size <= 8)
                        return lt;
                    return rt;

                case SystemTypeCode.Decimal:
                default:
                    return lt;
            }
        }

        void ReduceToCD(Code code, BinaryOperator op, ref IType lt, ref IType rt)
        {
            if (op == BinaryOperator.BitwiseAnd
                || op == BinaryOperator.BitwiseOr
                || op == BinaryOperator.ExclusiveOr)
            {
                var d = GetBitwiseCD(lt, rt);
                Cast(code, ref lt, ref rt, d);
            }
            else
            {
                ReduceToCD(code, ref lt, ref rt);
            }
        }

        void ReduceToCD(Code code, ref IType lt, ref IType rt)
        {
            var d = SystemTypes.GetCommonDenominator(lt, rt);
            if (d == null)
                return;

            Cast(code, ref lt, ref rt, d);
        }

        void Cast(Code code, ref IType lt, ref IType rt, IType d)
        {
            Cast(code, rt, d, false);
            Cast(code, lt, d, true);
            lt = d;
            rt = d;
        }

        static bool IsSignedUnsigned(IType ltype, IType rtype)
        {
            return (SystemTypes.IsSigned(ltype) && SystemTypes.IsUnsigned(rtype))
                   || (SystemTypes.IsUnsigned(ltype) && SystemTypes.IsSigned(rtype));
        }

        void ToUnsigned(Code code, ref IType type, bool swap)
        {
            var ut = SystemTypes.ToUnsigned(type);
            if (ut != null)
            {
                if (swap) Swap(code);
                code.AddRange(_provider.Cast(type, ut, false));
                if (swap) Swap(code);
                type = ut;
            }
        }

        void ToUnsigned(Code code, ref IType ltype, ref IType rtype)
        {
            var u = SystemTypes.UInt32OR64(ltype, rtype);
            if (u != null)
            {
                Cast(code, rtype, u, false);
                Cast(code, ltype, u, true);
                ltype = u;
                rtype = u;
            }
        }
        #endregion
    }
}