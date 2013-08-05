using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Flash.Abc;
using DataDynamics.PageFX.Flash.Core.Inlining;
using DataDynamics.PageFX.Flash.Core.SpecialTypes;
using DataDynamics.PageFX.Flash.Core.Tools;

namespace DataDynamics.PageFX.Flash.Core.CodeGeneration
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
        public IMethodCall Resolve(IMethod method)
        {
            var type = method.DeclaringType;

	        IMethodCall call;

            var internalType = type.Data as InternalType;
            if (internalType != null)
            {
                Debug.Assert(method.IsInternalCall);

                call = Resolve(method, null);
                if (call == null)
                    throw new InvalidOperationException();

                return SetData(method, call);
            }

			var instance = type.AbcInstance();
			var inlineCall = InlineCodeGenerator.Build(Abc, instance, method);
			if (inlineCall != null)
            {
				return SetData(method, inlineCall);
            }

            if (instance != null)
            {
                call = Resolve(method, instance);
                if (call != null)
                {
					return SetData(method, call);
                }
            }

            if (!method.IsImplemented())
            {
                throw new InvalidOperationException();
            }

            return null;
        }

		private IMethodCall SetData(IMethod method, IMethodCall call)
		{
			return _generator.SetData(method, call) as IMethodCall;
		}

        private IMethodCall Resolve(IMethod method, AbcInstance instance)
        {
            var inlineCall = InlineCodeGenerator.Build(Abc, instance, method);
			if (inlineCall != null)
			{
				return inlineCall;
			}

	        if (method.CodeType == MethodCodeType.Native)
	        {
		        return ThrowOrDefineNotImplCall(method, instance);
	        }

	        if (method.IsInternalCall)
	        {
		        return ResolveInternalCall(method, instance);
	        }

	        if (method.CodeType == MethodCodeType.Runtime)
	        {
		        return ResolveRuntimeCode(method, instance);
	        }

	        return null;
        }

        private AbcMethod ResolveRuntimeCode(IMethod method, AbcInstance instance)
        {
            var type = method.DeclaringType;
            if (type.TypeKind == TypeKind.Delegate)
            {
				return _generator.Delegates.Build(method, instance);
            }
            return ThrowOrDefineNotImplCall(method, instance);
        }

        private IMethodCall ResolveInternalCall(IMethod method, AbcInstance instance)
        {
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

		//TODO: revise MethodDefiners, allow to define methods that will be finished on ABC flushing phase

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