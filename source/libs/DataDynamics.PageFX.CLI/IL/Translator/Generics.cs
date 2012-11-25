using System;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.IL
{
    //contains resolving of generics
    internal partial class ILTranslator
    {
        #region ResolveGenerics
        private void ResolveGenerics()
        {
            ResolveBaseTypes();

            if (!_body.HasGenerics) return;

            if (_body.HasGenericInstructions)
            {
                foreach (var instr in _body.Code)
                {
                    ResolveInstructionGenerics(instr);
                }
            }

            if (_body.HasGenericVars)
            {
                ResolveGenericVars();
            }

            if (_body.InstanceCount > 1)
            {
                //NOTE: We should reset parameter tag since it can be used for pointer emulation
                foreach (var p in _method.Parameters)
                    p.Tag = null;
            }

			if (_body.HasGenericExceptions)
			{
				ResolveGenericExceptions(_body.ProtectedBlocks.Cast<Block>());
			}
        }

		private void ResolveGenericVars()
        {
            foreach (var var in _body.LocalVariables)
            {
                //NOTE: We should reset var tag since it can be used for pointer emulation
                var.Tag = null;

                if (var.GenericType != null)
                {
                    var.Type = ResolveType(var.GenericType);
                    continue;
                }

                var type = ResolveType(var.Type);
                if (!ReferenceEquals(type, var.Type))
                {
                    if (var.GenericType == null)
                        var.GenericType = var.Type;
                    var.Type = type;
                }
            }
        }

		private void ResolveBaseTypes()
        {
            var t = _declType;
            while (t != null)
                t = t.BaseType;
        }
        #endregion

        #region ResolveGenericExceptions
		private void ResolveGenericExceptions(Block block)
        {
            var hb = block as HandlerBlock;
            if (hb != null)
            {
                if (hb.GenericExceptionType != null)
                    hb.ExceptionType = hb.GenericExceptionType;
                var type = ResolveType(hb.ExceptionType);
                if (!ReferenceEquals(type, hb.ExceptionType))
                {
                    if (hb.GenericExceptionType == null)
                        hb.GenericExceptionType = hb.ExceptionType;
                    hb.ExceptionType = type;
                }
            }

            ResolveGenericExceptions(block.Kids);

            var pb = block as TryCatchBlock;
            if (pb != null)
            {
				ResolveGenericExceptions(pb.Handlers.Cast<HandlerBlock>());
            }
        }

		private void ResolveGenericExceptions(IEnumerable<HandlerBlock> set)
        {
            if (set == null) return;
            foreach (var block in set)
                ResolveGenericExceptions(block);
        }

		private void ResolveGenericExceptions(IEnumerable<Block> set)
        {
            if (set == null) return;
            foreach (var block in set)
                ResolveGenericExceptions(block);
        }
        #endregion

        #region ResolveInstructionGenerics
		private void ResolveInstructionGenerics(Instruction instr)
        {
            var member = instr.Member;
            if (member == null) return;
            if (!GenericType.IsGenericContext(member)) return;

            switch (member.MemberType)
            {
                case MemberType.Constructor:
                case MemberType.Method:
                    instr.Member = ResolveMethod((IMethod)member);
                    break;

                case MemberType.Field:
                    instr.Member = ResolveField((IField)member, instr);
                    break;

                case MemberType.Event:
                case MemberType.Property:
                    throw new NotSupportedException();

                case MemberType.Type:
                    {
                        var type = (IType)member;
                        type = ResolveType(type, instr);
                        instr.Member = type;
                    }
                    break;
            }
        }
        #endregion

        #region ResolveMethod
		private IMethod ResolveMethod(IMethod method)
        {
            if (method.IsGenericInstance)
                return ResolveGenericMethodInstance(method);

            var declType = method.DeclaringType;
            declType = ResolveType(declType);

            if (declType.IsArray && method.IsInternalCall)
                return ArrayType.ResolveMethod(declType, method);

            var gi = declType as IGenericInstance;
            if (gi == null)
                throw new ILTranslatorException("declType is not generic instance");

            var proxy = GenericType.FindMethodProxy(gi, method);
            if (proxy != null)
                return proxy;

            //check that sig of method is resolved
            if (method.GenericParameters.Count > 0)
                throw new ILTranslatorException("method {0} is not resolved");

            return method;
        }

		private IMethod ResolveGenericMethodInstance(IMethod method)
        {
            return GenericType.ResolveMethodInstance(_declType, _method, method);
        }
        #endregion

        #region ResolveField
		private IField ResolveField(IField field, Instruction instr)
        {
            var declType = field.DeclaringType;
            declType = ResolveType(declType, instr);

            CheckInstantiation(declType, instr);

            var gi = declType as IGenericInstance;
            if (gi != null)
            {
                var proxy = GenericType.FindFieldProxy(gi, field);
                if (proxy != null)
                    return proxy;
            }

            CheckInstantiation(field.Type, instr);

            return field;
        }
        #endregion

        #region ResolveType
		private IType ResolveType(IType type)
        {
            return ResolveType(type, null);
        }

		private IType ResolveType(IType type, Instruction instr)
        {
            if (instr != null && instr.Code == InstructionCode.Ldtoken)
            {
                //NOTE: Ldtoken can has any metadata token including generic types.
                if (type is IGenericType)
                    return type;
            }

            var t = GenericType.Resolve(_declType, _method, type);
            if (t == null)
                throw new ILTranslatorException("Unable to resolve type");

            CheckInstantiation(t, instr);
            return t;
        }
        #endregion

        #region Utils
		private static void CheckInstantiation(IType t, Instruction instr)
        {
            if (GenericType.HasGenericParams(t))
            {
                //NOTE: Ldtoken can has any metadata token including generic types.
                if (instr != null && instr.Code == InstructionCode.Ldtoken)
                    return;
                throw new ILTranslatorException(string.Format("type {0} is not completely instatiated", t));
            }
        }
        #endregion
    }
}