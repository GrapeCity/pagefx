using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using DataDynamics.PageFX.Common.Services;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.Abc;
using DataDynamics.PageFX.FlashLand.Core.Inlining;
using DataDynamics.PageFX.FlashLand.Core.SpecialTypes;
using DataDynamics.PageFX.FlashLand.Core.Tools;

namespace DataDynamics.PageFX.FlashLand.Core.CodeGeneration
{
    internal partial class AbcGenerator
    {
        #region ResolveCall
        //Entry point to resolve spec runtime calls
        private object ResolveCall(IMethod method)
        {
            var type = method.DeclaringType;
            var tag = type.Data;
            var avmType = tag as InternalType;
            if (avmType != null)
            {
                Debug.Assert(method.IsInternalCall);
                tag = ResolveCall(method, null);
                if (tag == null)
                    throw new InvalidOperationException();
                return SetData(method, tag);
            }

            tag = InlineCodeGenerator.Build(Abc, (AbcInstance)null, method);
            if (tag != null)
            {
                return SetData(method, tag);
            }

            var instance = type.AbcInstance();
            if (instance != null)
            {
                tag = ResolveCall(method, instance);
                if (tag != null)
                {
                    return SetData(method, tag);
                }
            }

            if (!method.IsImplemented())
            {
                throw new InvalidOperationException();
            }

            return null;
        }

        private object ResolveCall(IMethod method, AbcInstance instance)
        {
            object tag = InlineCodeGenerator.Build(Abc, instance, method);
            if (tag != null) return tag;

            if (method.CodeType == MethodCodeType.Native)
                return ThrowOrDefineNotImplCall(method, instance);

            if (method.IsInternalCall)
                return ResolveInternalCall(method, instance);

            if (method.CodeType == MethodCodeType.Runtime)
                return ResolveRuntimeCode(method, instance);

            return null;
        }

        private object ResolveRuntimeCode(IMethod method, AbcInstance instance)
        {
            var type = method.DeclaringType;
            if (type.TypeKind == TypeKind.Delegate)
            {
                if (method.IsConstructor)
                    return DefineDelegateConstructor(method, instance);
                if (method.Name == "Invoke")
                    return DefineDelegateInvoke(method, instance);
            }

            return ThrowOrDefineNotImplCall(method, instance);
        }

        private object ResolveInternalCall(IMethod method, AbcInstance instance)
        {
            var code = InlineCodeGenerator.Build(Abc, instance, method);
            if (code != null) return code;

            //special methods
            var m = ResolveSpecCall(method, instance);
            if (m != null) return m;

            var definer = FindMethodDefiner(method);
            if (definer != null) return definer(method, instance);

            return ThrowOrDefineNotImplCall(method, instance);
        }

        private object ThrowOrDefineNotImplCall(IMethod method, AbcInstance instance)
        {
            if (AbcGenConfig.ThrowOnUnexpectedCall)
                throw UnexpectedCall(method);
            return DefineNotImplementedMethod(method, instance);
        }

        private static Exception UnexpectedCall(IMethod method)
        {
#if DEBUG
            if (DebugService.BreakInternalCall)
                Debugger.Break();
#endif
            return new NotSupportedException(string.Format("Unexpected internal call: {0}",
                                                           method.GetFullName()));
        }
        #endregion

        #region ResolveSpecCall
        private AbcMethod ResolveSpecCall(IMethod method, AbcInstance instance)
        {
            int paramNum = method.Parameters.Count;
            if (paramNum == 0)
            {
                string name = method.Name;
                if (name == Const.GetTypeId)
                    return DefineGetTypeIdMethod(method.DeclaringType, instance);
            }

	        return DefineArrayCtor(method, instance)
	               ?? DefineArrayGetter(method, instance)
	               ?? DefineArraySetter(method, instance)
	               ?? DefineArrayAddress(method, instance);
        }
        #endregion

        #region Method Definers
        private delegate AbcMethod MethodDefiner(IMethod method, AbcInstance instance);

        private Hashtable _definers;

        private void RegisterDefiner(MethodDefiner definer, string key)
        {
            Debug.Assert(!_definers.Contains(key));
            _definers[key] = definer;
        }

        private void InitMethodDefiners()
        {
            if (_definers != null) return;
            _definers = new Hashtable();
            RegisterDefiner(Define_Assembly_GetTypeNum, "System.Reflection.Assembly.GetTypeNum");
            RegisterDefiner(Define_Assembly_InitType, "System.Reflection.Assembly.InitType");
        }

        private MethodDefiner FindMethodDefiner(IMethod method)
        {
            InitMethodDefiners();
            string key = method.DeclaringType.FullName + "." + method.Name;
            return (MethodDefiner)_definers[key];
        }
        #endregion

        #region DefineNotImplementedMethod
        private AbcMethod DefineNotImplementedMethod(IMethod method, AbcInstance instance)
        {
	        return instance.DefineMethod(
		        SigOf(method),
		        code =>
			        {
				        var exceptionType = GetType(CorlibTypeId.NotImplementedException);
				        code.ThrowException(exceptionType);

				        //TODO: Is it needed???
				        if (method.IsVoid())
				        {
					        code.ReturnVoid();
				        }
				        else
				        {
					        code.PushNull();
					        code.ReturnValue();
				        }
			        });
        }
        #endregion
    }
}