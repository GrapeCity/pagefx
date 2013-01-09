using System;
using System.Diagnostics;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Services;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.Abc;

namespace DataDynamics.PageFX.FlashLand.Core.CodeGeneration.Builders
{
    internal sealed class OperatorBuilder
    {
	    private readonly AbcGenerator _generator;
	    private readonly OperatorResolver _opCache = new OperatorResolver();

		public OperatorBuilder(AbcGenerator generator)
		{
			_generator = generator;
		}

	    private AbcFile Abc
	    {
		    get { return _generator.Abc; }
	    }

	    public IMethod Find(BinaryOperator op, IType left, IType right)
        {
	        return _opCache.Find(op, left, right);
        }

        public IMethod Find(UnaryOperator op, IType type)
        {
	        return _opCache.Find(op, type);
        }

        public IMethod Find(IType type, bool isTrue)
        {
	        return _opCache.Find(type, isTrue);
        }

	    public AbcMethod Build(BinaryOperator op, IType left, IType right)
        {
            var method = Find(op, left, right);
            if (method == null)
                throw new InvalidOperationException();

            var abcOp = _generator.DefineAbcMethod(method);

			var instance = _generator.DefineAbcInstance(method.DeclaringType);

            Debug.Assert(instance.Abc == Abc);

            var thisName = Abc.DefineGlobalQName("this_" + abcOp.TraitName.NameString);
            var retType = _generator.DefineReturnType(method.Type);

	        return instance.DefineMethod(
		        Sig.@this(thisName, retType, right, "right"),
		        code =>
			        {
				        code.Getlex(instance);
				        code.GetLocal(0); //left
				        code.GetLocal(1); //right
				        code.Call(abcOp);
				        code.Coerce(retType);
				        code.ReturnValue();
			        });
        }

        public AbcMethod Build(BranchOperator op, IType left, IType right)
        {
            return Build(op.ToBinaryOperator(), left, right);
        }

	    public AbcMethod Build(UnaryOperator op, IType type)
        {
            var method = Find(op, type);
            if (method == null)
                throw new InvalidOperationException();
            return BuildUnary(method);
        }

        private AbcMethod BuildUnary(IMethod op)
        {
            if (op == null)
                throw new ArgumentNullException("op");

            var abcOp = _generator.DefineAbcMethod(op);

            var instance = _generator.DefineAbcInstance(op.DeclaringType);

            var thisName = Abc.DefineGlobalQName("this_" + abcOp.TraitName.NameString);
            var retType = _generator.DefineReturnType(op.Type);

	        return instance.DefineMethod(
		        Sig.@this(thisName, retType),
		        code =>
			        {
				        code.Getlex(instance);
				        code.GetLocal(0);
				        code.Call(abcOp);
				        code.Coerce(op.Type, true);
				        code.ReturnValue();
			        });
        }

	    public AbcMethod BuildBoolOp(IType type, bool isTrue)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (!type.IsDecimalOrInt64())
                throw new ArgumentException();

            var op = Find(type, isTrue);
            if (op == null)
                throw new InvalidOperationException();

            return BuildUnary(op);
        }
    }
}