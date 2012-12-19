using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using DataDynamics.PageFX.Common.Services;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.Abc;
using DataDynamics.PageFX.FlashLand.Core.SpecialTypes;
using DataDynamics.PageFX.FlashLand.Core.Tools;
using DataDynamics.PageFX.FlashLand.IL;

namespace DataDynamics.PageFX.FlashLand.Core.ByteCodeGeneration
{
    internal partial class AbcGenerator
    {
        #region ResolveCall
        //Entry point to resolve spec runtime calls
        object ResolveCall(IMethod method)
        {
            var type = method.DeclaringType;
            var tag = type.Tag;
            var avmType = tag as InternalType;
            if (avmType != null)
            {
                Debug.Assert(method.IsInternalCall);
                tag = ResolveCall(method, null);
                if (tag == null)
                    throw new InvalidOperationException();
                method.Tag = tag;
                return tag;
            }

            tag = DefineInlineCode(method, (AbcInstance)null);
            if (tag != null)
            {
                method.Tag = tag;
                return tag;
            }

            var instance = type.Tag as AbcInstance;
            if (instance != null)
            {
                tag = ResolveCall(method, instance);
                if (tag != null)
                {
                    method.Tag = tag;
                    return tag;
                }
            }

            if (!method.IsImplemented())
            {
                throw new InvalidOperationException();
            }

            return null;
        }

        object ResolveCall(IMethod method, AbcInstance instance)
        {
            object tag = DefineInlineCode(method, instance);
            if (tag != null) return tag;

            if (method.CodeType == MethodCodeType.Native)
                return ThrowOrDefineNotImplCall(method, instance);

            if (method.IsInternalCall)
                return ResolveInternalCall(method, instance);

            if (method.CodeType == MethodCodeType.Runtime)
                return ResolveRuntimeCode(method, instance);

            return null;
        }

        object ResolveRuntimeCode(IMethod method, AbcInstance instance)
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

        object ResolveInternalCall(IMethod method, AbcInstance instance)
        {
            var code = DefineInlineCode(method, instance);
            if (code != null) return code;

            //special methods
            var m = ResolveSpecCall(method, instance);
            if (m != null) return m;

            var definer = FindMethodDefiner(method);
            if (definer != null) return definer(method, instance);

            return ThrowOrDefineNotImplCall(method, instance);
        }

        object ThrowOrDefineNotImplCall(IMethod method, AbcInstance instance)
        {
            if (AbcGenConfig.ThrowOnUnexpectedCall)
                throw UnexpectedCall(method);
            return DefineNotImplementedMethod(method, instance);
        }

        static Exception UnexpectedCall(IMethod method)
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
        AbcMethod ResolveSpecCall(IMethod method, AbcInstance instance)
        {
            int paramNum = method.Parameters.Count;
            if (paramNum == 0)
            {
                string name = method.Name;
                if (name == Const.GetTypeId)
                    return DefineGetTypeIdMethod(method.DeclaringType, instance);
            }
            var m = DefineArrayCtor(method, instance);
            if (m != null) return m;
            m = DefineArrayGetter(method, instance);
            if (m != null) return m;
            m = DefineArraySetter(method, instance);
            if (m != null) return m;
            m = DefineArrayAddress(method, instance);
            return m;
        }
        #endregion

        #region Method Definers
        delegate AbcMethod MethodDefiner(IMethod method, AbcInstance instance);

        Hashtable _definers;

        void RegisterDefiner(MethodDefiner definer, string key)
        {
            Debug.Assert(!_definers.Contains(key));
            _definers[key] = definer;
        }

        void InitMethodDefiners()
        {
            if (_definers != null) return;
            _definers = new Hashtable();
            RegisterDefiner(Define_Assembly_GetTypeNum, "System.Reflection.Assembly.GetTypeNum");
            RegisterDefiner(Define_Assembly_InitType, "System.Reflection.Assembly.InitType");
        }

        MethodDefiner FindMethodDefiner(IMethod method)
        {
            InitMethodDefiners();
            string key = method.DeclaringType.FullName + "." + method.Name;
            return (MethodDefiner)_definers[key];
        }
        #endregion

        #region DefineNotImplementedMethod
        AbcMethod DefineNotImplementedMethod(IMethod method, AbcInstance instance)
        {
            var abcMethod = new AbcMethod(method);

            var trait = DefineMethodTrait(abcMethod, method);
            instance.AddTrait(trait, method.IsStatic);
            abcMethod.Trait = trait;
            abcMethod.Name = trait.Name.Name;
            abcMethod.ReturnType = DefineReturnType(method.Type);
            DefineParameters(abcMethod, method);

            var body = new AbcMethodBody(abcMethod);
            AddMethod(abcMethod);

            var code = new AbcCode(Abc);

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

            body.Finish(code);
            
            return abcMethod;
        }
        #endregion
    }
}