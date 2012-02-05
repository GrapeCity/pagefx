using System;
using System.Diagnostics;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.FLI.ABC;
using DataDynamics.PageFX.FLI.IL;

namespace DataDynamics.PageFX.FLI
{
    using AbcString = AbcConst<string>;

    //Contains generation of AbcMethod
    //Key method (entry point): DefineMethod
    partial class AbcGenerator
    {
        #region GetMethodName
        AbcMultiname GetMethodName(IMethod method)
        {
            string name = NameUtil.GetMethodName(method);
            return _abc.DefineQName(method, name);
        }

        static string GetFixedName(IMethod method)
        {
            //NOTE: flex compiler lookups inside instance to resolve type refs
            var declType = method.DeclaringType;
            if (declType == SystemTypes.DateTime)
            {
                //NOTE: Date property can hide global Date type therefore we must rename it.
                if (MethodHelper.IsAccessor(method)
                    && method.Association.Name == "Date")
                    return "DATE";
            }
            else if (declType == SystemTypes.Type)
            {
                //NOTE: Namespace property can hide global Namespace type therefore we must rename it.
                if (MethodHelper.IsAccessor(method)
                    && method.Association.Name == "Namespace")
                    return "NAMESPACE";
            }
            return null;
        }

        AbcMultiname GetMethodName(IMethod method, out bool isOverride)
        {
            isOverride = MethodHelper.IsOverride(method);

            var bm = method.BaseMethod;
            if (isOverride && bm != null)
            {
                var mn = GetDefinedMethodName(bm);
                if (mn != null) return mn;
            }

            var impls = method.ImplementedMethods;
            if (impls != null && impls.Length == 1)
            {
                var impl = impls[0];
                if (impl != null)
                    return GetDefinedMethodName(impl);
            }

            string name = GetFixedName(method);
            if (name == null)
                name = NameUtil.GetMethodName(method);
            return _abc.DefineQName(method, name);
        }

        AbcMultiname GetDefinedMethodName(IMethod method)
        {
            var tag = DefineMethod(method);

            var m = tag as AbcMethod;
            if (m != null)
                return m.TraitName;

            var name = tag as AbcMultiname;
            if (name == null)
                throw new InvalidOperationException();

            return name;
        }
        #endregion

        #region DefineMethodTrait
        AbcTrait DefineMethodTrait(AbcMethod abcMethod, IMethod method)
        {
#if DEBUG
            DebugService.DoCancel();
#endif
            bool isOverride;
            var name = GetMethodName(method, out isOverride);

            var trait = AbcTrait.CreateMethod(abcMethod, name);

            if (MethodHelper.IsGetter(method))
                trait.Kind = AbcTraitKind.Getter;
            else if (MethodHelper.IsSetter(method))
                trait.Kind = AbcTraitKind.Setter;

            if (MethodHelper.IsStatic(method))
            {
                trait.Attributes |= AbcTraitAttributes.Final;
            }
            else
            {
                if (method.DeclaringType == SystemTypes.Exception)
                {
                    if (MethodHelper.IsObjectOverrideMethod(method))
                        isOverride = false;
                }
                trait.IsOverride = isOverride;
            }

            return trait;
        }
        #endregion

        #region ImportMethod
        object ImportAbcMethod(AbcMethod method)
        {
            if (method.IsNative)
                return method;

            var abc = method.ABC;
            if (abc.IsCoreAPI)
                return method;

            if (method.ImportedMethod != null)
                return method.ImportedMethod;

            var instance = method.Instance;
            if (instance != null)
            {

                var i2 = _abc.ImportInstance(instance, ref method);
                if (i2 == null)
                    throw new InvalidOperationException();
            }
            else
            {
                if (!(method.Owner is AbcScript))
                    throw new InvalidOperationException();

                _abc.Import(abc);

                //NOTE: ABC can be linked externally
                if (method.ImportedMethod != null)
                    return method.ImportedMethod;
            }

            return method;
        }

        object ImportMethod(IMethod method)
        {
            var tag = method.Tag;
            if (tag == null) return null;
            var m = tag as AbcMethod;
            if (m != null)
            {
                tag = ImportAbcMethod(m);
                DefineOverrideMethods(method);
                return tag;
            }
            throw new InvalidOperationException();
        }
        #endregion

        #region DefineMethod
        object IsDefined(IMethod method)
        {
            if (_abc.IsDefined(method))
            {
                return method.Tag;
            }

            var m = method.Tag as AbcMethod;
            if (m != null)
            {
                if (m.ImportedMethod != null)
                    return m.ImportedMethod;

                //external linking
                if (m.IsImported)
                    return m;
            }

            return null;
        }

        /// <summary>
        /// Defines given method.
        /// </summary>
        /// <param name="method">method to define.</param>
        /// <returns></returns>
        public object DefineMethod(IMethod method)
        {
            if (method == null) return null;

            if (method.IsGeneric)
                throw new InvalidOperationException();

            var tag = IsDefined(method);
            if (tag != null) return tag;

            var type = method.DeclaringType;
            DefineType(type);

            tag = IsDefined(method);
            if (tag != null) return tag;

            CheckApiCompatibility(method);

            tag = ImportMethod(method);
            if (tag != null) return tag;

            tag = ResolveCall(method);
            if (tag != null) return tag;

            DefineBaseMethods(method);

            //Define method signature types.
            DefineType(method.Type);
            foreach (var p in method.Parameters)
                DefineType(p.Type);

            tag = IsDefined(method);
            if (tag != null) return tag;

#if DEBUG
            DebugService.LogSeparator();
            DebugService.LogInfo("ABC DefineMethod started for method: {0}", method);
#endif

            var abcMethod = DefineMethodCore(method);

#if DEBUG
            DebugService.LogInfo("ABC DefineMethod succeded for method: {0}", method);
#endif
            return abcMethod;
        }
        #endregion

        #region DefineMethodCore
        /// <summary>
        /// Defines ABC method from managed method with normal body.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        AbcMethod DefineMethodCore(IMethod method)
        {
            var declType = method.DeclaringType;
            var instance = declType.Tag as AbcInstance;
            if (instance == null)
                throw new InvalidOperationException();

            var abcMethod = new AbcMethod(method);
            _abc.Methods.Add(abcMethod);

            bool isMxAppCtor = false;
            if (AbcGenConfig.FlexAppCtorAsHandler)
                isMxAppCtor = IsMxAppCtor(method);

            //NOTE:
            //1. ctor can be used as class or instance initializer
            //2. Static constructor will be compiled as normal method
            //3. Only default parameterless ctor will be compiled as initializer,
            //   all other ctors will be compiled as normal methods

            if (!GlobalSettings.ReflectionSupport)
            {
                if (!isMxAppCtor && instance.Initializer == null
                    && MethodHelper.IsInstanceInitializer(method))
                {
                    instance.Initializer = abcMethod;
                    //abcMethod.Name = _abc.DefineString(method.DeclaringType.Name);
                }
            }

            //for non initializer method we must define trait and return type
            if (!abcMethod.IsInitializer)
            {
                var trait = DefineMethodTrait(abcMethod, method);
                instance.AddTrait(trait, MethodHelper.IsStatic(method));
                abcMethod.ReturnType = DefineReturnType(abcMethod, method);
            }

            //if (IsSwc)
            //{
            //    abcMethod.Name = DefineMethodName(abcMethod);
            //}

            //HACK: Define mx.core.FlexEvent argument for MX app ctor
            if (isMxAppCtor)
            {
                var typeFlexEvent = ImportFlexEventType();
                abcMethod.AddParam(typeFlexEvent.Name, _abc.DefineString("e"));
            }
            else
            {
                DefineParameters(abcMethod, method);
            }

            DefineMethodBody(abcMethod);
            DefineImplementedMethods(method, instance, abcMethod);
            DefineOverrideMethods(method);

            ImplementProtoMethods(method, abcMethod);

            return abcMethod;
        }

        AbcString DefineMethodName(AbcMethod method)
        {
            string name = "";
            var instance = method.Instance;
            if (instance != null)
            {
                string ns = instance.NamespaceString;
                if (!string.IsNullOrEmpty(ns))
                {
                    name += ns;
                    name += ":";
                }
                name += instance.NameString;
                name += "/";
            }
            if (method.IsInitializer)
            {
                if (instance != null)
                    name += instance.NameString;
                else
                    name += "iinit";
            }
            else
            {
                var t = method.Trait;
                if (t == null) return null;
                name += t.NameString;
            }
            return _abc.DefineString(name);
        }
        #endregion

        void ImplementProtoMethods(IMethod method, AbcMethod abcMethod)
        {
            ImplementStringMethod(method);
            ImplementObjectMethod(method, abcMethod);
        }

        #region DefineImplementedMethods
        void DefineImplementedMethods(IMethod method, AbcInstance instance, AbcMethod abcMethod)
        {
            var impls = method.ImplementedMethods;
            if (impls == null) return;
            int n = impls.Length;
            if (n <= 0) return;

            //NOTE: To avoid conflict with name of explicit implementation method has the same name as iface method
            if (method.IsExplicitImplementation)
            {
                DefineMethod(impls[0]);
                return;
            }

            if (n == 1 && !abcMethod.IsOverride)
            {
                DefineMethod(impls[0]);
                return;
            }

            for (int i = 0; i < n; ++i)
            {
                DefineExplicitImplementation(instance, abcMethod, method, impls[i]);
            }
        }
        #endregion

        #region DefineReturnType
        public AbcMultiname DefineReturnType(AbcMethod abcMethod, IMethod method)
        {
            if (method.IsConstructor && MethodHelper.AsStaticCall(method))
                return DefineMemberType(method.DeclaringType);

            var bm = GetBaseMethod(abcMethod, method);
            if (bm != null)
                return bm.ReturnType;

            return DefineReturnType(method.Type);
        }

        public AbcMultiname DefineReturnType(IType type)
        {
            if (type == null)
                return _abc.BuiltinTypes.Void;
            var name = DefineMemberType(type);
            if (name == null)
                throw new InvalidOperationException("Unable to define return type for method");
            return name;
        }
        #endregion

        #region DefineParameters, CopyParameters
        /// <summary>
        /// Defines parameters for given <see cref="AbcMethod"/>.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="source">source method of given <see cref="AbcMethod"/></param>
        public void DefineParameters(AbcMethod target, IMethod source)
        {
            if (source == _entryPoint)
            {
                if (AbcGenConfig.ParameterlessEntryPoint)
                    return;
            }

            if (MethodHelper.HasPseudoThis(source))
            {
                var typeName = DefineMemberType(source.DeclaringType);
                target.AddParam(CreateParam(typeName, "this"));
            }

            var abm = GetBaseMethod(target, source);
            if (abm != null)
            {
                CopyParams(target, abm);
            }
            else
            {
                int n = source.Parameters.Count;
                for (int i = 0; i < n; ++i)
                {
#if DEBUG
                    DebugService.DoCancel();
#endif
                    var p = source.Parameters[i];
                    var ap = CreateParam(p.Type, p.Name);
                    target.AddParam(ap);
                }
            }
        }

        AbcMethod GetBaseMethod(AbcMethod am, IMethod m)
        {
            var impls = m.ImplementedMethods;
            if (impls != null && impls.Length == 1)
                return DefineAbcMethod(impls[0]);

            var bm = m.BaseMethod;
            if (am.IsOverride && bm != null)
                return bm.Tag as AbcMethod;

            return null;
        }

        void CopyParams(AbcMethod to, AbcMethod from)
        {
            int n = from.ParamCount;
            for (int i = 0; i < n; ++i)
            {
#if DEBUG
                DebugService.DoCancel();
#endif
                var bp = from.Parameters[i];
                var ap = new AbcParameter
                             {
                                 Type = _abc.ImportConst(bp.Type),
                                 Name = _abc.ImportConst(bp.Name)
                             };
                if (bp.IsOptional)
                {
                    ap.IsOptional = true;
                    ap.Value = _abc.ImportValue(bp.Value);
                    to.HasOptionalParams = true;
                }
                to.AddParam(ap);
            }
            if (from.NeedRest)
                to.NeedRest = true;
        }
        #endregion

        #region DefineBaseMethods
        void DefineBaseMethods(IMethod method)
        {
            //            var declType = method.DeclaringType;
            //            if (declType.IsInterface) return;

            //            var baseType = declType.BaseType;
            //            while (baseType != null)
            //            {
            //#if DEBUG
            //                DebugService.DoCancel();
            //#endif
            //                var m = Method.FindMethod(baseType, method, false);
            //                DefineMethod(m);
            //                baseType = baseType.BaseType;
            //            }

            var bm = method.BaseMethod;
            while (bm != null)
            {
#if DEBUG
                DebugService.DoCancel();
#endif
                DefineMethod(bm);
                bm = bm.BaseMethod;
            }

            var impls = method.ImplementedMethods;
            if (impls != null)
            {
                foreach (var impl in impls)
                    DefineMethod(impl);
            }
        }
        #endregion

        #region DefineOverrideMethods
        void DefineOverrideMethods(IMethod method)
        {
            if (method.IsAbstract || method.IsVirtual)
            {
#if DEBUG
                DebugService.DoCancel();
#endif
                var type = method.DeclaringType;
                var instance = type.Tag as AbcInstance;
                if (instance == null)
                    throw new InvalidOperationException();
                if (instance.IsInterface)
                {
                    for (int i = 0; i < instance.Implementations.Count; ++i)
                    {
#if DEBUG
                        DebugService.DoCancel();
#endif
                        var impl = instance.Implementations[i];
                        DefineImplementation(impl.Type, method);
                    }
                }
                else
                {
                    DefineSubclassOverrideMethods(instance, method);
                }
            }
        }
        #endregion

        #region DefineSubclassOverrideMethods
        //Defines override methods in subclasses
        void DefineSubclassOverrideMethods(AbcInstance instance, IMethod method)
        {
            var type = instance.Type;
            if (type == SystemTypes.Enum) return;
            for (int i = 0; i < instance.Subclasses.Count; ++i)
            {
#if DEBUG
                DebugService.DoCancel();
#endif
                var subclass = instance.Subclasses[i];
                DefineOverrideMethod(subclass.Type, method);

#if DEBUG
                DebugService.DoCancel();
#endif
                DefineSubclassOverrideMethods(subclass, method);
            }
        }
        #endregion

        #region DefineOverrideMethod
        static IMethod FindOverrideMethod(IType implType, IMethod method)
        {
            if (method.IsGenericInstance)
            {
                var gm = method.InstanceOf;
                var m = Method.FindMethod(implType, gm, false);
                if (m == null) return null;
                m = GenericType.CreateMethodInstance(implType, m, method.GenericArguments);
                if (m == null)
                    throw new InvalidOperationException();
                return m;
            }
            return Method.FindMethod(implType, method, false);
        }

        void DefineOverrideMethod(IType implType, IMethod method)
        {
            if (implType == null) return;
            var m = FindOverrideMethod(implType, method);
            if (m != null)
                DefineMethod(m);
        }
        #endregion

        #region DefineImplementation
        void DefineImplementation(IType implType, IMethod method)
        {
            if (implType == null)
                throw new ArgumentNullException();

            if (implType.IsInterface)
                return;

            var impl = MethodHelper.FindImplementation(implType, method);

            if (impl == null)
            {
                //impl = AvmHelper.FindImplementation(implType, method);

                throw new InvalidOperationException(
                    string.Format("Unable to find implementation for method {0} in type {1}",
                                  method.FullName, implType.FullName));
            }

            if (impl.IsGeneric)
            {
                if (!method.IsGenericInstance)
                    throw new InvalidOperationException("invalid context");
                //IMethod gmi = new GenericMethodInstance(implType, m, method.GenericArguments);
                //gmi = GenericType.ResolveMethodInstance(implType, null, gmi);
                //impl = gmi;
                impl = GenericType.CreateMethodInstance(implType, impl, method.GenericArguments);
            }

            DefineMethod(impl);
        }
        #endregion

        #region DefineExplicitImplementation
        void DefineExplicitImplementation(AbcInstance instance, AbcMethod abcMethod, IMethod method, IMethod ifaceMethod)
        {
            var ifaceAbcMethod = DefineAbcMethod(ifaceMethod);

            var m = instance.DefineMethod(
                ifaceAbcMethod,
                delegate(AbcCode code)
                {
                    code.LoadThis();
                    code.LoadArguments(ifaceMethod);
                    code.Call(abcMethod);
                    if (ifaceAbcMethod.IsVoid) code.ReturnVoid();
                    else code.ReturnValue();
                });

            //m.SourceMethod = method;
            m.Trait.IsOverride = MethodHelper.FindImplementation(method.DeclaringType.BaseType, ifaceMethod, true) != null;
        }
        #endregion

        #region DefineMethodBody
        public bool UseCCS;

        void DefineMethodBody(AbcMethod abcMethod)
        {
            if (abcMethod == null)
                throw new ArgumentNullException();

            var method = abcMethod.SourceMethod;
            if (method == null) return;
            if (method.Body == null) return;

#if DEBUG
            DebugService.DoCancel();
#endif
            if (!IsCcsRunning && UseCCS)
            {
                if (LoadBodyFromCache(abcMethod))
                    return;
            }

            BuildBodyCore(abcMethod, method);
        }

        bool LoadBodyFromCache(AbcMethod abcMethod)
        {
            var refs = new BodyReferences();
            var body = _ccs.Import(abcMethod, refs);
            if (body == null) return false;
            _abc.MethodBodies.Add(body);
            foreach (var member in refs.Members)
                DefineMember(member);
            foreach (var field in refs.FieldPointers)
                DefineFieldPtr(field);
            return true;
        }

        void BuildBodyCore(AbcMethod target, IMethod source)
        {
            var targetBody = new AbcMethodBody(target);
            _abc.MethodBodies.Add(targetBody);

#if DEBUG
            DebugService.DoCancel();
#endif
            var sourceBody = source.Body;

            var codeProvider = new AvmCodeProvider(this, target);

            var translator = sourceBody.CreateTranslator();
            if (translator == null)
                throw new InvalidOperationException("No IL translator");
            var il = translator.Translate(source, sourceBody, codeProvider);

            codeProvider.Finish();

#if DEBUG
            DebugService.DoCancel();
#endif

            targetBody.IL.Add(il);
            targetBody.Finish(_abc);
        }
        #endregion

        #region DefineAbcMethod
        public AbcMethod DefineAbcMethod(IMethod method)
        {
            if (method == null)
                throw new ArgumentNullException("method");
            var m = DefineMethod(method) as AbcMethod;
            if (m == null)
                throw new InvalidOperationException(string.Format("Unable to define method: {0}", method));
            return m;
        }

        public AbcMethod DefineAbcMethod(IType type, Predicate<IMethod> p)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (p == null)
                throw new ArgumentNullException("p");
            var m = Algorithms.Find(type.Methods, p);
            if (m == null)
                throw new InvalidOperationException("Unable to find method by given predicate");
            return DefineAbcMethod(m);
        }

        public AbcMethod DefineAbcMethod(IType type, string name, int argc)
        {
            var source = type.Methods[name, argc];
            return DefineAbcMethod(source);
        }

        public AbcMethod DefineAbcMethod(IType type, string name, Predicate<IType> arg1)
        {
            var source = type.Methods[name, arg1];
            return DefineAbcMethod(source);
        }

        public AbcMethod DefineAbcMethod(IType type, string name, Predicate<IType> arg1, Predicate<IType> arg2)
        {
            var source = type.Methods[name, arg1, arg2];
            return DefineAbcMethod(source);
        }

        public AbcMethod DefineAbcMethod(IType type, string name, Predicate<IType> arg1, Predicate<IType> arg2, Predicate<IType> arg3)
        {
            var source = type.Methods[name, arg1, arg2, arg3];
            return DefineAbcMethod(source);
        }

        public AbcMethod DefineAbcMethod(IType type, string name, IType arg1)
        {
            var source = type.Methods[name, arg1];
            return DefineAbcMethod(source);
        }

        public AbcMethod DefineAbcMethod(IType type, string name, IType arg1, IType arg2)
        {
            var source = type.Methods[name, arg1, arg2];
            return DefineAbcMethod(source);
        }

        public AbcMethod DefineAbcMethod(IType type, string name, IType arg1, IType arg2, IType arg3)
        {
            var source = type.Methods[name, arg1, arg2, arg3];
            return DefineAbcMethod(source);
        }

        public AbcMethod DefineAbcMethod(IType type, string name)
        {
            return DefineAbcMethod(type, name, 0);
        }

        LazyValue<AbcMethod> LazyMethod(IType type, Predicate<IMethod> p)
        {
            return new LazyValue<AbcMethod>(() => DefineAbcMethod(type, p));
        }

        LazyValue<AbcMethod> LazyMethod(IType type, string name, int argc)
        {
            return new LazyValue<AbcMethod>(() => DefineAbcMethod(type, name, argc));
        }

        LazyValue<AbcMethod> LazyMethod(IType type, string name)
        {
            return new LazyValue<AbcMethod>(() => DefineAbcMethod(type, name));
        }

        LazyValue<AbcMethod> LazyMethod(IType type, string name, Predicate<IType> arg1)
        {
            return new LazyValue<AbcMethod>(() => DefineAbcMethod(type, name, arg1));
        }

        LazyValue<AbcMethod> LazyMethod(IType type, string name, Predicate<IType> arg1, Predicate<IType> arg2)
        {
            return new LazyValue<AbcMethod>(() => DefineAbcMethod(type, name, arg1, arg2));
        }

        LazyValue<AbcMethod> LazyMethod(IType type, string name, Predicate<IType> arg1, Predicate<IType> arg2, Predicate<IType> arg3)
        {
            return new LazyValue<AbcMethod>(() => DefineAbcMethod(type, name, arg1, arg2, arg3));
        }

        LazyValue<AbcMethod> LazyMethod(IType type, string name, IType arg1)
        {
            return new LazyValue<AbcMethod>(() => DefineAbcMethod(type, name, arg1));
        }

        LazyValue<AbcMethod> LazyMethod(IType type, string name, IType arg1, IType arg2)
        {
            return new LazyValue<AbcMethod>(() => DefineAbcMethod(type, name, arg1, arg2));
        }

        LazyValue<AbcMethod> LazyMethod(IType type, string name, IType arg1, IType arg2, IType arg3)
        {
            return new LazyValue<AbcMethod>(() => DefineAbcMethod(type, name, arg1, arg2, arg3));
        }
        #endregion

        #region Utils
        bool IsMxAppCtor(IMethod method)
        {
            if (method == null) return false;
            if (method.IsStatic) return false;
            if (!IsSwf) return false;
            if (!method.IsConstructor) return false;
            if (method.Parameters.Count != 0) return false;
            return method.DeclaringType == sfc.TypeFlexApp;
        }
        #endregion
    }
}