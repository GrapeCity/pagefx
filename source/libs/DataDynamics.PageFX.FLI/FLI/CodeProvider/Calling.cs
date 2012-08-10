using System;
using System.Collections.Generic;
using System.Diagnostics;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.FLI.ABC;
using DataDynamics.PageFX.FLI.IL;

namespace DataDynamics.PageFX.FLI
{
    partial class AvmCodeProvider
    {
        #region LoadReceiver
        static bool HasGlobalReceiver(IMethod method)
        {
            var type = method.DeclaringType;
            if (type.Tag is GlobalType)
                return true;
            if (type.IsInternalType())
            {
                if (method.Name == "trace")
                    return true;
            }
            if (type.IsNativeType("Class"))
            {
                if (method.Name == "Find")
                    return true;
            }
            return false;
        }

        void LoadGlobalReceiver(AbcCode code, IMethod method)
        {
            var type = method.DeclaringType;
            if (type.Tag is GlobalType)
            {
                var mn = GetMethodName(method);
                code.FindPropertyStrict(mn);
                return;
            }
            if (type.IsInternalType())
            {
                var mn = _abc.DefineGlobalQName(method.Name);
                code.FindPropertyStrict(mn);
                return;
            }
            if (type.IsNativeType("Class"))
            {
                if (method.Name == "Find")
                {
                    var m = _generator.DefineFindClass();
                    code.Getlex(m);
                    return;
                }
            }
            throw new InvalidOperationException();
        }

        static bool HasReceiver(IMethod method, bool newobj)
        {
            if (newobj) return true;
            
            var tag = method.Tag;
            if (tag == null) return false;

            //NOTE: Inline code!!!
            if (tag is AbcCode)
                return HasGlobalReceiver(method);

            return true;
        }

        public IInstruction[] LoadReceiver(IMethod method, bool newobj)
        {
            EnsureMethod(method);

            if (!HasReceiver(method, newobj)) return null;

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

            var tag = method.Tag;
            
            var mname = tag as AbcMultiname;
            if (mname != null)
            {
                code.FindPropertyStrict(mname);
                return code.ToArray();
            }

            if (method.IsStaticCall())
            {
                var mn = tag as AbcMemberName;
                if (mn != null)
                    code.Getlex(mn.Type);
                else
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

        bool LoadSpecReceiver(IMethod method, AbcCode code)
        {
            if (HasGlobalReceiver(method))
            {
                LoadGlobalReceiver(code, method);
                return true;
            }

            var type = method.DeclaringType;
            var typeTag = type.Tag;

            var vec = typeTag as IVectorType;
            if (vec != null)
            {
                EnsureType(vec.Param);
                if (method.IsStatic)
                {
                    code.LoadGenericClass(vec.Name);
                    return true;
                }
            }

            return false;
        }

        void LoadCtorReceiver(AbcCode code, IMethod method)
        {
            if (!method.IsConstructor)
                throw new ArgumentException("method is not ctor", "method");

            var declType = method.DeclaringType;

            var vec = declType.Tag as IVectorType;
            if (vec != null)
            {
                code.LoadGenericClass(vec.Name);
                return;
            }

            var abcMethod = method.Tag as AbcMethod;
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

        #region CallInfo
        private struct CallInfo
        {
        }
        private readonly Stack<CallInfo> _callStack = new Stack<CallInfo>();
        #endregion

        #region BeginCall/EndCall
        public IInstruction[] BeginCall(IMethod method)
        {
            EnsureMethod(method);
            _callStack.Push(new CallInfo());
            return null;
        }

        public IInstruction[] EndCall(IMethod method)
        {
            var code = new AbcCode(_abc);

            if (!AbcGenConfig.FlexAppCtorAsHandler && IsMxAppBaseCtor(method))
            {
                _generator.FlexAppCtorAfterSuperCall(code);
            }

            _callStack.Pop();
            
            return code.ToArray();
        }
        #endregion

        #region CallMethod
        public IInstruction[] CallMethod(IType receiverType, IMethod method, CallFlags flags)
        {
            EnsureMethod(method);

            var tag = method.Tag;
            if (tag == null)
            {
                //if (method.IsConstructor && paramCount == 0)
                //    return null;
                throw new ArgumentException("Method is not defined yet");
            }

            //NOTE: Check inline code
            var code = tag as AbcCode;
            if (code != null)
            {
                var arr = code.ToArray();
                //NOTE: We need to clone instructions
                for (int i = 0; i < arr.Length; ++i)
                    arr[i] = ((Instruction)arr[i]).Clone();
                return arr;
            }

            code = new AbcCode(_abc);

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

            var prop = GetCallName(tag);
            if (prop != null)
            {
                Call(code, method, prop, flags);
                return code.ToArray();
            }

            throw new NotImplementedException();
        }


        static string GetBaseCallPrefix(IMethod method)
        {
            var m = method.Tag as AbcMethod;
            if (m != null)
            {
                if (m.IsGetter) return "$get_super$";
                if (m.IsSetter) return "$set_super$";
            }
            return "$super$";
        }

        AbcMethod DefineBaseCall(IType receiverType, IMethod method)
        {
            var instance = DefineAbcInstance(receiverType);

            var mname = GetCallName(method.Tag);
            string prefix = "$C" + instance.Index;
            prefix += GetBaseCallPrefix(method);
            var rname = _abc.DefineQName(mname.Namespace, prefix + mname.NameString);
            var retType = DefineMemberType(method.Type);
            return instance.DefineInstanceMethod(
                rname, retType,
                code =>
                    {
                        code.LoadThis();
                        code.LoadArguments(method);
                        CallCore(code, method, mname, true);
                        code.Return(method);
                    }, 
                    method);
        }

        void BaseCall(AbcCode code, IType receiverType, IMethod method)
        {
            var m = DefineBaseCall(receiverType, method);
            code.Call(m);
        }

        void Call(AbcCode code, IMethod method, AbcMultiname prop, CallFlags flags)
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

        void CallCore(AbcCode code, IMethod method, AbcMultiname prop, CallFlags flags)
        {
            prop = _abc.ImportConst(prop);

            bool thiscall = (flags & CallFlags.Thiscall) != 0;
            bool virtcall = (flags & CallFlags.Virtcall) != 0;

            //determines whether method belongs to base type
            bool super = false;
            if (thiscall && !virtcall)
                super = IsSuperCall(method);

            CallCore(code, method, prop, super);
        }

        void CallCore(AbcCode code, IMethod method, AbcMultiname prop, bool super)
        {
            var m = method.Tag as AbcMethod;
            if (m != null)
            {
                if (m.IsGetter)
                {
                    code.Add(super ? InstructionCode.Getsuper : InstructionCode.Getproperty, prop);
                    return;
                }

                if (m.IsSetter)
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
                    && m != null && _abc.IsDefined(m))
                {
                    code.Add(InstructionCode.Callstatic, m, n);
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

        bool IsSuperCall(IMethod method)
        {
            if (method.IsAbstract) return false;
            return IsBaseMethod(method);
        }

        bool CanUseCallStatic
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

        static bool MustCoerceReturnType(IMethod method)
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
            
            var abcMethod = method.Tag as AbcMethod;
            if (abcMethod != null)
            {
                if (abcMethod.IsNative) return true;
                if (abcMethod.IsImported) return true;
                if (abcMethod.OriginalMethod != null) return true;
            }

            return false;
        }

        static bool MustCoerceReturnType(IType declType)
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

        bool SuperCall(AbcCode code, IMethod method)
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

        bool IsConstructSuper(IMethod method)
        {
            if (!_method.IsConstructor) return false;
            if (!IsBaseCtor(method)) return false;
            if (IsString) return true;
            return method.IsInitializer();
        }

        void NewObject(AbcCode code, IMethod method)
        {
            var type = method.DeclaringType;

            var abcMethod = method.Tag as AbcMethod;
            if (abcMethod != null)
            {
                if (abcMethod.IsInitializer) //default ctor!
                {
                    code.Construct(method.Parameters.Count);
                    return;
                }

                if (type == SystemTypes.String)
                {
                    code.Call(abcMethod);
                    return;
                }

                var ctor = _generator.DefineCtorStaticCall(method);
                code.Call(ctor);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
        #endregion

        #region CallStaticCtor
        bool NeedCallStaticCtor(ITypeMember member)
        {
            if (member == null) return false;
            if (member.DeclaringType == _declType) return false;

            var method = member as IMethod;
            if (method != null)
                return method.IsConstructor || method.IsStatic;

            var f = member as IField;
            if (f != null)
                return f.IsStatic;

            return false;
        }

        void CallStaticCtor(AbcCode code, ITypeMember member)
        {
            if (!_method.IsStatic) return;
            if (!_method.IsConstructor) return;
            if (!NeedCallStaticCtor(member)) return;

            var declType = member.DeclaringType;
            if (declType == _declType) return;
            _generator.CallStaticCtor(code, declType);
        }
        #endregion

        #region Utils
        public bool IsSwf
        {
            get { return _generator.IsSwf; }
        }

        bool IsMxApp
        {
            get { return _generator.IsMxApplication; }
        }

        bool IsMxAppCtor
        {
            get
            {
                if (!_method.IsConstructor) return false;
                if (_method.Parameters.Count > 0) return false;
                if (!IsMxApp) return false;
                return _declType == _generator.sfc.TypeFlexApp;
            }
        }

        bool IsMxAppBaseCtor(IMethod method)
        {
            return IsMxAppCtor && IsBaseCtor(method);
        }

        bool ShouldPopBaseCtorCall(IMethod method)
        {
            if (AbcGenConfig.FlexAppCtorAsHandler && IsMxAppBaseCtor(method))
                return true;
            if (IsString)
                return true;
            return false;
        }

        private bool IsString
        {
            get { return _declType == SystemTypes.String; }
        }

        static AbcMultiname GetCallName(object tag)
        {
            var prop = tag as AbcMultiname;
            if (prop != null)
                return prop;

            var mn = tag as AbcMemberName;
            if (mn != null)
                return mn.Name;

            var m = tag as AbcMethod;
            if (m != null)
                return m.TraitName;

            return null;
        }
        #endregion
    }
}