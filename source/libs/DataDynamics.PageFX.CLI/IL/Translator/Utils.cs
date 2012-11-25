using System;
using System.Collections.Generic;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.IL
{
    using Code = List<IInstruction>;

    internal partial class ILTranslator
    {
	    private void Swap(ICollection<IInstruction> code)
        {
            var i = _provider.Swap();
            if (i == null)
                throw new NotSupportedException("Swap instruction is not supported");
            code.Add(i);
        }

	    private bool MustPreventBoxing(IMethod method, IParameter arg)
        {
            return _provider.MustPreventBoxing(method, arg);
        }

	    private void Cast(Code code, IType source, IType target)
        {
		    if (ReferenceEquals(target, source)) return;

		    var cast = _provider.Cast(source, target, false);
		    if (cast != null)
			    code.AddRange(cast);
        }

		private void Cast(Code code, IType source, IType target, bool swap)
        {
			if (ReferenceEquals(target, source)) return;
			if (swap) Swap(code);
			Cast(code, source, target);
			if (swap) Swap(code);
        }

		private static IType GetBitwiseCD(IType leftType, IType rightType)
        {
            if (leftType.IsEnum)
                leftType = leftType.ValueType;
            if (rightType.IsEnum)
                rightType = rightType.ValueType;

            var l = leftType.SystemType();
            if (l == null)
                throw new ILTranslatorException();
            var r = rightType.SystemType();
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
                            return rightType;

                        default:
                            return leftType;
                    }

                case SystemTypeCode.Single:
                    switch (r.Code)
                    {
                        case SystemTypeCode.Decimal:
                        case SystemTypeCode.Double:
                            return rightType;

                        default:
                            return leftType;
                    }

                case SystemTypeCode.Boolean:
                    return rightType;

                case SystemTypeCode.Int8:
                case SystemTypeCode.UInt8:
                    if (r.Size <= 1)
                        return leftType;
                    return rightType;

                case SystemTypeCode.Int16:
                case SystemTypeCode.UInt16:
                case SystemTypeCode.Char:
                    if (r.Size <= 2)
                        return leftType;
                    return rightType;

                case SystemTypeCode.Int32:
                case SystemTypeCode.UInt32:
                    if (r.Size <= 4)
                        return leftType;
                    return rightType;

                case SystemTypeCode.Int64:
                case SystemTypeCode.UInt64:
                    if (r.Size <= 8)
                        return leftType;
                    return rightType;

                case SystemTypeCode.Decimal:
                default:
                    return leftType;
            }
        }

		private void ReduceToCD(Code code, BinaryOperator op, ref IType lt, ref IType rt)
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

		private void ReduceToCD(Code code, ref IType lt, ref IType rt)
        {
            var d = SystemTypes.GetCommonDenominator(lt, rt);
            if (d == null)
                return;

            Cast(code, ref lt, ref rt, d);
        }

		private void Cast(Code code, ref IType lt, ref IType rt, IType d)
        {
            Cast(code, rt, d, false);
            Cast(code, lt, d, true);
            lt = d;
            rt = d;
        }

        private static bool IsSignedUnsigned(IType ltype, IType rtype)
        {
            return (ltype.IsSigned() && rtype.IsUnsigned())
                   || (ltype.IsUnsigned() && rtype.IsSigned());
        }

		private void ToUnsigned(Code code, ref IType type, bool swap)
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

		private void ToUnsigned(Code code, ref IType ltype, ref IType rtype)
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
    }
}