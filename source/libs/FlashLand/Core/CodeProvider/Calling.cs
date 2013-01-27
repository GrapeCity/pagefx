using System;
using System.Linq;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.Abc;
using DataDynamics.PageFX.FlashLand.Avm;
using DataDynamics.PageFX.FlashLand.Core.SpecialTypes;
using DataDynamics.PageFX.FlashLand.IL;

namespace DataDynamics.PageFX.FlashLand.Core.CodeProvider
{
    partial class CodeProviderImpl
    {
        #region LoadReceiver
        private static bool HasGlobalReceiver(IMethod method)
        {
			//TODO: simplify using custom attributes
            var type = method.DeclaringType;
            if (type.Data is GlobalFunctionsContainer)
                return true;
            if (type.IsInternalType())
            {
                if (method.Name == "trace")
                    return true;
            }
			if (type.Is(SystemTypeCode.String))
			{
				if (method.Name == "fromCharCode")
					return true;
			}
            if (type.IsNativeType("Class"))
            {
                if (method.Name == "Find")
                    return true;
            }
            return false;
        }

        private void LoadGlobalReceiver(AbcCode code, IMethod method)
        {
            var type = method.DeclaringType;

            if (type.Data is GlobalFunctionsContainer)
            {
                var mn = GetMethodName(method);
                code.FindPropertyStrict(mn);
                return;
            }

            if (type.IsInternalType())
            {
	            string name = method.Name;
	            var mn = _abc.DefineName(QName.Global(name));
                code.FindPropertyStrict(mn);
                return;
            }

			if (type.Is(SystemTypeCode.String))
			{
				code.Getlex(AvmTypeCode.String);
				return;
			}

            if (type.IsNativeType("Class"))
            {
                if (method.Name == "Find")
                {
                    var m = _generator.RuntimeImpl.FindClass();
                    code.Getlex(m);
                    return;
                }
            }

			throw new InvalidOperationException();
        }

        private static bool HasReceiver(IMethod method, bool newobj)
        {
            if (newobj)
            {
				var type = method.DeclaringType;
				if (type.IsNativeType("Object"))
				{
					return false;
				}
				return true;
            }
            
            var tag = method.Data;
            if (tag == null)
            {
	            return false;
            }

            //NOTE: Inline code!!!
	        if (tag is InlineCall)
	        {
		        return HasGlobalReceiver(method);
	        }

	        return true;
        }

        public IInstruction[] LoadReceiver(IMethod method, bool newobj)
        {
            EnsureMethod(method);

            if (!HasReceiver(method, newobj))
				return null;

            var code = new AbcCode(_abc);
            CallStaticCtor(code, method);

            if (newobj)
            {
                LoadCtorReceiver(code, method);
                return code.ToArray();
            }

            var type = method.DeclaringType;
            EnsureType(type);

            if (LoadSpecReceiver(method, code))
                return code.ToArray();

            var tag = method.Data;
            
            var mname = tag as AbcMultiname;
            if (mname != null)
            {
                code.FindPropertyStrict(mname);
                return code.ToArray();
            }

            if (method.IsStaticCall())
            {
                LoadStaticInstance(code, type);
                return code.ToArray();
            }

            //NOTE:
            //Primitive types should be boxed before calling of any instance method
            //But not here because object must be onto the stack.
            //And this operation must be controlled from the top level
            //if (TypeService.IsBoxablePrimitiveType(type))
            //{
            //    BoxPrimitive(code, type, false);
            //}

            return code.ToArray();
        }

        private bool LoadSpecReceiver(IMethod method, AbcCode code)
        {
            if (HasGlobalReceiver(method))
            {
                LoadGlobalReceiver(code, method);
                return true;
            }

            var type = method.DeclaringType;
            var typeTag = type.Data;

            var vec = typeTag as IVectorType;
            if (vec != null)
            {
                EnsureType(vec.Parameter);
                if (method.IsStatic)
                {
                    code.LoadGenericClass(vec.Name);
                    return true;
                }
            }

            return false;
        }

		private void LoadCtorReceiver(AbcCode code, IMethod method)
        {
            if (!method.IsConstructor)
                throw new ArgumentException("method is not ctor", "method");

            var declType = method.DeclaringType;

            var vec = declType.Data as IVectorType;
            if (vec != null)
            {
                code.LoadGenericClass(vec.Name);
                return;
            }

	        var nativeType = declType.Data as NativeType;
			if (nativeType != null)
			{
				code.Getlex(nativeType.Name);
				return;
			}

            var abcMethod = method.AbcMethod();
            if (abcMethod == null)
                throw new ArgumentException("Invalid method tag");

            var instance = DefineAbcInstance(declType);
            if (instance == null)
                throw new InvalidOperationException();

            if (UseThisForStaticReceiver(declType))
            {
                code.LoadThis();
                return;
            }

            code.Getlex(instance);
        }
        #endregion

        #region BeginCall/EndCall
        public IInstruction[] BeginCall(IMethod method)
        {
            EnsureMethod(method);
            return null;
        }

        public IInstruction[] EndCall(IMethod method)
        {
            var code = new AbcCode(_abc);

            if (!AbcGenConfig.FlexAppCtorAsHandler && IsMxAppBaseCtor(method))
            {
                _generator.FlexAppBuilder.CtorAfterSuperCall(code);
            }

            return code.ToArray();
        }
        #endregion

        #region CallMethod
        public IInstruction[] CallMethod(IType receiverType, IMethod method, CallFlags flags)
        {
            EnsureMethod(method);

            var tag = method.Data;
            if (tag == null)
            {
                //if (method.IsConstructor && paramCount == 0)
                //    return null;
                throw new ArgumentException("Method is not defined yet");
            }

            //NOTE: Check inline code
            var inlineCall = tag as InlineCall;
			if (inlineCall != null)
            {
				//NOTE: We need to clone instructions since inline code should be shared and reusable
                return inlineCall.InlineCode.Clone().ToArray();
            }

            var code = new AbcCode(_abc);

            if ((flags & CallFlags.Newobj) != 0)
            {
                NewObject(code, method);
                return code.ToArray();
            }

            if (SuperCall(code, method))
                return code.ToArray();

            if ((flags & CallFlags.Basecall) != 0)
            {
                BaseCall(code, receiverType, method);
                return code.ToArray();
            }

	        var name = GetMethodName(method);
            if (name != null)
            {
                Call(code, method, name, flags);
                return code.ToArray();
            }

            throw new NotImplementedException();
        }

        private static string GetBaseCallPrefix(IMethod method)
        {
            var abcMethod = method.AbcMethod();
            if (abcMethod != null)
            {
                if (abcMethod.IsGetter) return "$get_super$";
                if (abcMethod.IsSetter) return "$set_super$";
            }
            return "$super$";
        }

        private AbcMethod DefineBaseCall(IType receiverType, IMethod method)
        {
            var instance = DefineAbcInstance(receiverType);

            var mname = GetMethodName(method);
            string prefix = "$C" + instance.Index;
            prefix += GetBaseCallPrefix(method);

            var rname = _abc.DefineQName(mname.Namespace, prefix + mname.NameString);
            var retType = DefineMemberType(method.Type);

	        return instance.DefineMethod(
		        Sig.@this(rname, retType, method),
		        code =>
			        {
				        code.LoadThis();
				        code.LoadArguments(method);
				        CallCore(code, method, mname, true);
				        code.Return(method);
			        });
        }

        private void BaseCall(AbcCode code, IType receiverType, IMethod method)
        {
            var m = DefineBaseCall(receiverType, method);
            code.Call(m);
        }

        private void Call(AbcCode code, IMethod method, AbcMultiname prop, CallFlags flags)
        {
            if (prop == null)
                throw new ArgumentNullException("prop");

            CallCore(code, method, prop, flags);

            if (MustCoerceReturnType(method))
            {
                var type = method.Type;
                EnsureType(type);
                code.Coerce(type, true);
            }
        }

        private void CallCore(AbcCode code, IMethod method, AbcMultiname prop, CallFlags flags)
        {
            prop = _abc.ImportConst(prop);

            //determines whether method belongs to base type
            bool super = IsSuperCall(method, flags);
            
            CallCore(code, method, prop, super);
        }

        private void CallCore(AbcCode code, IMethod method, AbcMultiname prop, bool super)
        {
            var abcMethod = method.AbcMethod();
            if (abcMethod != null)
            {
                if (abcMethod.IsGetter)
                {
                    code.Add(super ? InstructionCode.Getsuper : InstructionCode.Getproperty, prop);
                    return;
                }

                if (abcMethod.IsSetter)
                {
                    code.Add(super ? InstructionCode.Setsuper : InstructionCode.Setproperty, prop);
                    return;
                }
            }

            int n = method.Parameters.Count;
            if (method.AsStaticCall())
                ++n;

            bool isVoid = method.IsVoid();

            if (CanUseCallStatic)
            {
                if (method.IsStaticCall()
                    && method.IsManaged()
                    && abcMethod != null && _abc.IsDefined(abcMethod))
                {
                    code.Add(InstructionCode.Callstatic, abcMethod, n);
                    if (isVoid) code.Pop();
                    return;
                }
            }

            if (super)
            {
                if (isVoid)
                {
                    code.Add(InstructionCode.Callsupervoid, prop, n);
                    return;
                }
                code.Add(InstructionCode.Callsuper, prop, n);
                return;
            }

            if (isVoid)
            {
                code.Add(InstructionCode.Callpropvoid, prop, n);
                return;
            }

            code.Add(InstructionCode.Callproperty, prop, n);
        }

		private bool IsSuperCall(IMethod method, CallFlags flags)
		{
			bool thiscall = (flags & CallFlags.Thiscall) != 0;
			bool virtcall = (flags & CallFlags.Virtcall) != 0;
			return thiscall && !virtcall && IsSuperCall(method);
		}

        private bool IsSuperCall(IMethod method)
        {
			if (method.IsStatic || method.IsConstructor || method.IsAbstract) return false;
            return IsBaseMethod(method);
        }

        private bool CanUseCallStatic
        {
            get
            {
            	return false;
				// callstatic fails on FP 10.1 for unclear reason (case 147084)
				/*
				 * 
				var g = _generator;
				if (g.IsCcsRunning || g.IsSwc)
					return false;
				return true;
				 * */
			}
        }

        private static bool MustCoerceReturnType(IMethod method)
        {
            if (method.IsVoid()) return false;

            //NOTE: AVM Verifier uses type info (ABC traits) to determine return type
            //If verifier can not find given method then the method is treated as dynamic and return type is always * (any).
            //In this cases we should add coerce instruction

            var declType = method.DeclaringType;
            if (declType.UseNativeObject())
                return true;

            if (MustCoerceReturnType(declType))
                return true;
            
            var abcMethod = method.AbcMethod();
            if (abcMethod != null)
            {
                if (abcMethod.IsNative) return true;
                if (abcMethod.IsImported) return true;
                if (abcMethod.OriginalMethod != null) return true;
            }

            return false;
        }

		private static bool MustCoerceReturnType(IType declType)
        {
            if (declType.IsInterface)
            {
				//var gi = declType as IGenericInstance;
				//if (gi != null)
				//{
				//    switch (gi.Type.FullName)
				//    {
				//        case CLRNames.Types.IEnumeratorT:
				//        case CLRNames.Types.ICollectionT:
				//        case CLRNames.Types.IListT:
				//            return true;
				//    }
				//}
				//else
				//{
				//    switch (declType.FullName)
				//    {
				//        case CLRNames.Types.IEnumerator:
				//        case CLRNames.Types.ICollection:
				//        case CLRNames.Types.IList:
				//            return true;
				//    }
				//}
            	return true;
            }
            return false;
        }

		private bool SuperCall(AbcCode code, IMethod method)
        {
            if (IsConstructSuper(method))
            {
                int n = method.Parameters.Count;
                if (ShouldPopBaseCtorCall(method))
                {
                    code.Pop(n + 1);
                }
                else
                {
                    code.Add(InstructionCode.Constructsuper, n);
                }
                return true;
            }
            return false;
        }

		private bool IsConstructSuper(IMethod method)
        {
            if (!_method.IsConstructor) return false;
            if (!IsBaseCtor(method)) return false;
            if (IsString) return true;
            return method.IsInitializer();
        }

		private void NewObject(AbcCode code, IMethod method)
        {
            var type = method.DeclaringType;

            var abcMethod = method.AbcMethod();
            if (abcMethod != null)
            {
                if (abcMethod.IsInitializer) //default ctor!
                {
                    code.Construct(method.Parameters.Count);
                    return;
                }

                if (type.Is(SystemTypeCode.String))
                {
                    code.Call(abcMethod);
                    return;
                }

				var ctor = _generator.TypeBuilder.DefineCtorStaticCall(method);
                code.Call(ctor);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
        #endregion

        #region CallStaticCtor
        private bool NeedCallStaticCtor(ITypeMember member)
        {
            if (member == null) return false;
            if (ReferenceEquals(member.DeclaringType, _declType)) return false;

            var method = member as IMethod;
            if (method != null)
                return method.IsConstructor || method.IsStatic;

            var f = member as IField;
            if (f != null)
                return f.IsStatic;

            return false;
        }

        private void CallStaticCtor(AbcCode code, ITypeMember member)
        {
            if (!_method.IsStatic) return;
            if (!_method.IsConstructor) return;
            if (!NeedCallStaticCtor(member)) return;

            var declType = member.DeclaringType;
            if (ReferenceEquals(declType, _declType)) return;
            _generator.StaticCtors.Call(code, declType);
        }
        #endregion

        #region Utils
        public bool IsSwf
        {
            get { return _generator.IsSwf; }
        }

        private bool IsMxApp
        {
            get { return _generator.IsFlexApplication; }
        }

        private bool IsMxAppCtor
        {
            get
            {
                if (!_method.IsConstructor) return false;
                if (_method.Parameters.Count > 0) return false;
                if (!IsMxApp) return false;
                return ReferenceEquals(_declType, _generator.SwfCompiler.FlexAppType);
            }
        }

        private bool IsMxAppBaseCtor(IMethod method)
        {
            return IsMxAppCtor && IsBaseCtor(method);
        }

        private bool ShouldPopBaseCtorCall(IMethod method)
        {
            if (AbcGenConfig.FlexAppCtorAsHandler && IsMxAppBaseCtor(method))
                return true;
            if (IsString)
                return true;
            return false;
        }

        private bool IsString
        {
            get { return _declType.Is(SystemTypeCode.String); }
        }

        #endregion
    }
}