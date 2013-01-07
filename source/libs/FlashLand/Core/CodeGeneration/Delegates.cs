using System;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.Abc;
using DataDynamics.PageFX.FlashLand.IL;

namespace DataDynamics.PageFX.FlashLand.Core.CodeGeneration
{
    internal partial class AbcGenerator
    {
        private void EnshureDelegateMethods(IType type)
        {
            var m = type.Methods.Find(Const.Delegate.AddEventListeners, 2);
            if (m == null)
                throw new InvalidOperationException("Invalid corlib");
            DefineAbcMethod(m);

	        m = type.Methods.Find(Const.Delegate.RemoveEventListeners, 2);
	        if (m == null)
                throw new InvalidOperationException("Invalid corlib");
            DefineAbcMethod(m);
        }

		private void EnshureDelegateMethods()
        {
            if (_enshureDelegateMethods) return;
            _enshureDelegateMethods = true;
            EnshureDelegateMethods(SystemTypes.Delegate);
            EnshureDelegateMethods(SystemTypes.MulticastDelegate);
        }

		private bool _enshureDelegateMethods;

        #region DefineDelegateConstructor
		private AbcMethod DefineDelegateConstructor(IMethod method, AbcInstance instance)
		{
			EnshureDelegateMethods();

			if (method.Parameters.Count != 2)
				throw new InvalidOperationException();

			var targetParam = CreateParam(SystemTypes.Object, method.Parameters[0].Name);
			var funcParam = CreateParam(Abc.BuiltinTypes.Function, method.Parameters[1].Name);

			var sig = SigOf(method);
			sig.Args = new object[] {targetParam, funcParam};

			return instance.DefineMethod(
				sig,
				code =>
					{
						code.PushThisScope();

						code.ConstructSuper();

						const int target = 1;
						const int func = 2;

						code.LoadThis();
						code.GetLocal(target);
						code.SetField(FieldId.Delegate_Target);

						code.LoadThis();
						code.GetLocal(func);
						code.SetField(FieldId.Delegate_Function);

						code.ReturnVoid();
					});
		}

	    #endregion

        #region DefineDelegateInvoke
		private AbcMethod DefineDelegateInvoke(IMethod method, AbcInstance instance)
        {
            EnshureDelegateMethods();
            //TODO: Check m_function on "not null"

			var sig = SigOf(method);
			var traitName = Abc.DefineName(sig.Name);
			sig.Name = traitName; // minor opt to quickly define name next time

			return instance.DefineMethod(
				sig,
				code =>
					{
						bool isVoid = method.IsVoid();
						int paramNum = method.Parameters.Count;
						var type = method.DeclaringType;

						int prev = paramNum + 1;
						code.LoadThis();
						code.GetField(FieldId.Delegate_Prev);
						code.SetLocal(prev);

						code.GetLocal(prev);
						var gotoCall = code.IfFalse();

						code.GetLocal(prev);
						code.Coerce(type, false);
						code.LoadArguments(method);
						//Note: currently we ignore return value of prev function call
						code.Add(InstructionCode.Callpropvoid, traitName, paramNum);

						gotoCall.BranchTarget = code.Label();

						code.LoadThis();
						code.GetField(FieldId.Delegate_Function);

						code.LoadThis();
						code.GetField(FieldId.Delegate_Target);

						code.LoadArguments(method);
						code.Add(InstructionCode.Call, paramNum);

						if (isVoid)
						{
							code.Pop();
							code.ReturnVoid();
						}
						else
						{
							code.ReturnValue();
						}
					});
        }
        #endregion
    }
}