using System;
using System.Diagnostics;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.FLI.ABC;
using DataDynamics.PageFX.FLI.IL;

namespace DataDynamics.PageFX.FLI
{
    partial class AbcGenerator
    {
		private readonly OperatorResolver _opCache = new OperatorResolver();

        #region Op Cache

        public IMethod FindOperator(BinaryOperator op, IType left, IType right)
        {
	        return _opCache.Find(op, left, right);
        }

        public IMethod FindOperator(UnaryOperator op, IType type)
        {
	        return _opCache.Find(op, type);
        }

        public IMethod FindBooleanOperator(IType type, bool isTrue)
        {
	        return _opCache.Find(type, isTrue);
        }

        #endregion

        #region Binary Operator
        public AbcMethod DefineOperator(BinaryOperator op, IType left, IType right)
        {
            var method = FindOperator(op, left, right);
            if (method == null)
                throw new InvalidOperationException();

            var abcOp = DefineAbcMethod(method);

            var instance = DefineAbcInstance(method.DeclaringType);

            Debug.Assert(instance.ABC == _abc);

            var thisName = _abc.DefineGlobalQName("this_" + abcOp.TraitName.NameString);
            var retType = DefineReturnType(method.Type);

            return instance.DefineInstanceMethod(
                thisName, retType,
                code =>
	                {
		                code.Getlex(instance);
		                code.GetLocal(0); //left
		                code.GetLocal(1); //right
		                code.Call(abcOp);
		                code.Coerce(retType);
		                code.ReturnValue();
	                },
                right, "right");
        }

        public AbcMethod DefineOperator(BranchOperator op, IType left, IType right)
        {
            return DefineOperator(op.ToBinaryOperator(), left, right);
        }
        #endregion

        #region DefineUnaryOperator
        public AbcMethod DefineUnaryOperator(UnaryOperator op, IType type)
        {
            var method = FindOperator(op, type);
            if (method == null)
                throw new InvalidOperationException();
            return DefineUnaryOperator(method);
        }

        public AbcMethod DefineUnaryOperator(IMethod op)
        {
            if (op == null)
                throw new ArgumentNullException("op");

            var abcOp = DefineAbcMethod(op);

            var instance = DefineAbcInstance(op.DeclaringType);

            var thisName = _abc.DefineGlobalQName("this_" + abcOp.TraitName.NameString);
            var retType = DefineReturnType(op.Type);

            return instance.DefineInstanceMethod(
                thisName, retType,
                code =>
	                {
		                code.Getlex(instance);
		                code.GetLocal(0);
		                code.Call(abcOp);
		                code.Coerce(op.Type, true);
		                code.ReturnValue();
	                });
        }
        #endregion

        #region DefineBooleanOperator, DefineTruthOperator
        public AbcMethod DefineBooleanOperator(IType type, bool isTrue)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (!type.IsDecimalOrInt64())
                throw new ArgumentException();

            var op = FindBooleanOperator(type, isTrue);
            if (op == null)
                throw new InvalidOperationException();

            return DefineUnaryOperator(op);
        }

        public AbcMethod DefineTruthOperator(IType type, bool isTrue)
        {
            var abcOp = DefineBooleanOperator(type, isTrue);

            var instance = DefineAbcInstance(type);

            var name = _abc.DefineGlobalQName(isTrue ? "__true__" : "__false__");

            var retType = DefineReturnType(SystemTypes.Boolean);

            return instance.DefineStaticMethod(
                name, retType,
                code =>
	                {
		                code.GetLocal(1);
		                code.PushNull();
		                var ifnotNull = new Instruction(InstructionCode.Ifne);
		                code.Add(ifnotNull);
		                code.PushBool(!isTrue);
		                code.ReturnValue();

		                ifnotNull.BranchTarget = code.Label();

		                code.GetLocal(1);
		                code.Call(abcOp);

		                code.ReturnValue();
	                },
                instance.Name, "value");
        }
        #endregion
    }
}