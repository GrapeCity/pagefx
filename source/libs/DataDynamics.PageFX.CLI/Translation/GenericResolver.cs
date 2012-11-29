using System;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.CLI.IL;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.Translation
{
    /// <summary>
	/// Implements resolving of generics.
    /// </summary>
    internal static class GenericResolver
    {
	    public static void Resolve(IMethod currentMethod, IClrMethodBody body)
        {
		    ResolveBaseTypes(currentMethod.DeclaringType);

		    if (!body.HasGenerics) return;

            if (body.HasGenericInstructions)
            {
                foreach (var instr in body.Code)
                {
                    ProcessInstruction(currentMethod, instr);
                }
            }

            if (body.HasGenericVars)
            {
                ProcessLocalVars(currentMethod, body);
            }

            if (body.InstanceCount > 1)
            {
                //NOTE: We should reset parameter tag since it can be used for pointer emulation
                foreach (var p in currentMethod.Parameters)
                    p.Tag = null;
            }

			if (body.HasGenericExceptions)
			{
				ProcessExceptionTypes(currentMethod, body.ProtectedBlocks.Cast<Block>());
			}
        }

		private static void ProcessLocalVars(IMethod currentMethod, IClrMethodBody body)
        {
            foreach (var var in body.LocalVariables)
            {
                //NOTE: We should reset var tag since it can be used for pointer emulation
                var.Tag = null;

                if (var.GenericType != null)
                {
                    var.Type = ResolveType(currentMethod, var.GenericType);
                    continue;
                }

                var type = ResolveType(currentMethod, var.Type);
                if (!ReferenceEquals(type, var.Type))
                {
                    if (var.GenericType == null)
                        var.GenericType = var.Type;
                    var.Type = type;
                }
            }
        }

	    private static void ResolveBaseTypes(IType type)
		{
			while (type != null)
				type = type.BaseType;
		}

	    private static void ProcessExceptionTypes(IMethod currentMethod, Block block)
        {
            var hb = block as HandlerBlock;
            if (hb != null)
            {
                if (hb.GenericExceptionType != null)
                    hb.ExceptionType = hb.GenericExceptionType;
				var type = ResolveType(currentMethod, hb.ExceptionType);
                if (!ReferenceEquals(type, hb.ExceptionType))
                {
                    if (hb.GenericExceptionType == null)
                        hb.GenericExceptionType = hb.ExceptionType;
                    hb.ExceptionType = type;
                }
            }

			ProcessExceptionTypes(currentMethod, block.Kids);

            var pb = block as TryCatchBlock;
            if (pb != null)
            {
				ProcessExceptionTypes(currentMethod, pb.Handlers.Cast<HandlerBlock>());
            }
        }

		private static void ProcessExceptionTypes(IMethod currentMethod, IEnumerable<HandlerBlock> set)
        {
            if (set == null) return;
            foreach (var block in set)
                ProcessExceptionTypes(currentMethod, block);
        }

		private static void ProcessExceptionTypes(IMethod currentMethod, IEnumerable<Block> set)
        {
            if (set == null) return;
            foreach (var block in set)
                ProcessExceptionTypes(currentMethod, block);
        }

	    private static void ProcessInstruction(IMethod currentMethod, Instruction instruction)
        {
            var member = instruction.Member;
            if (member == null) return;
            if (!GenericType.IsGenericContext(member)) return;

            switch (member.MemberType)
            {
                case MemberType.Constructor:
                case MemberType.Method:
                    instruction.Member = ResolveMethod(currentMethod, (IMethod)member);
                    break;

                case MemberType.Field:
                    instruction.Member = ResolveField(currentMethod, (IField)member, instruction);
                    break;

                case MemberType.Event:
                case MemberType.Property:
                    throw new NotSupportedException();

                case MemberType.Type:
                    {
                        var type = (IType)member;
                        type = ResolveType(currentMethod, type, instruction);
                        instruction.Member = type;
                    }
                    break;
            }
        }

	    #region ResolveMethod
		private static IMethod ResolveMethod(IMethod currentMethod, IMethod method)
        {
            if (method.IsGenericInstance)
                return ResolveGenericMethodInstance(currentMethod, method);

            var declType = method.DeclaringType;
            declType = ResolveType(currentMethod, declType);

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

		private static IMethod ResolveGenericMethodInstance(IMethod currentMethod, IMethod method)
        {
            return GenericType.ResolveMethodInstance(currentMethod.DeclaringType, currentMethod, method);
        }
        #endregion

        #region ResolveField
		private static IField ResolveField(IMethod currentMethod, IField field, Instruction instr)
        {
            var declType = field.DeclaringType;
			declType = ResolveType(currentMethod, declType, instr);

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
		private static IType ResolveType(IMethod currentMethod, IType type)
        {
            return ResolveType(currentMethod, type, null);
        }

		private static IType ResolveType(IMethod currentMethod, IType type, Instruction instr)
        {
            if (instr != null && instr.Code == InstructionCode.Ldtoken)
            {
                //NOTE: Ldtoken can has any metadata token including generic types.
                if (type is IGenericType)
                    return type;
            }

            var t = GenericType.Resolve(currentMethod.DeclaringType, currentMethod, type);
            if (t == null)
                throw new ILTranslatorException("Unable to resolve type");

            CheckInstantiation(t, instr);
            return t;
        }
        #endregion

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
    }
}