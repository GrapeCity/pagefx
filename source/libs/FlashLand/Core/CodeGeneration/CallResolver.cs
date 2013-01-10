using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.Abc;
using DataDynamics.PageFX.FlashLand.Core.Inlining;
using DataDynamics.PageFX.FlashLand.Core.SpecialTypes;
using DataDynamics.PageFX.FlashLand.Core.Tools;

namespace DataDynamics.PageFX.FlashLand.Core.CodeGeneration
{
    internal sealed class CallResolver
    {
	    private readonly AbcGenerator _generator;

	    public CallResolver(AbcGenerator generator)
		{
			_generator = generator;
		}

	    private AbcFile Abc
	    {
			get { return _generator.Abc; }
	    }

	    //Entry point to resolve spec runtime calls
        public object Resolve(IMethod method)
        {
            var type = method.DeclaringType;
            var tag = type.Data;
            var avmType = tag as InternalType;
            if (avmType != null)
            {
                Debug.Assert(method.IsInternalCall);
                tag = Resolve(method, null);
                if (tag == null)
                    throw new InvalidOperationException();
                return _generator.SetData(method, tag);
            }

            tag = InlineCodeGenerator.Build(Abc, null, method);
            if (tag != null)
            {
				return _generator.SetData(method, tag);
            }

            var instance = type.AbcInstance();
            if (instance != null)
            {
                tag = Resolve(method, instance);
                if (tag != null)
                {
					return _generator.SetData(method, tag);
                }
            }

            if (!method.IsImplemented())
            {
                throw new InvalidOperationException();
            }

            return null;
        }

        private object Resolve(IMethod method, AbcInstance instance)
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
				return _generator.Delegates.Build(method, instance);
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

        internal static AbcMethod ThrowOrDefineNotImplCall(IMethod method, AbcInstance instance)
        {
            if (AbcGenConfig.ThrowOnUnexpectedCall)
                throw UnexpectedCall(method);
            return instance.DefineNotImplementedMethod(method);
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

	    private AbcMethod ResolveSpecCall(IMethod method, AbcInstance instance)
        {
            int paramNum = method.Parameters.Count;
            if (paramNum == 0)
            {
                string name = method.Name;
                if (name == Const.GetTypeId)
					return _generator.Reflection.DefineGetTypeIdMethod(method.DeclaringType, instance);
            }

			return _generator.ArrayImpl.BuildSpecMethod(method, instance);
        }

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
			RegisterDefiner(_generator.Reflection.Define_Assembly_GetTypeNum, "System.Reflection.Assembly.GetTypeNum");
			RegisterDefiner(_generator.Reflection.Define_Assembly_InitType, "System.Reflection.Assembly.InitType");
        }

        private MethodDefiner FindMethodDefiner(IMethod method)
        {
            InitMethodDefiners();
            string key = method.DeclaringType.FullName + "." + method.Name;
            return (MethodDefiner)_definers[key];
        }
    }
}