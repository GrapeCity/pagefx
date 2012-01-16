using System;
using System.Diagnostics;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.FLI.ABC;
using DataDynamics.PageFX.FLI.IL;
using DataDynamics.PageFX.FLI.SWC;

namespace DataDynamics.PageFX.FLI
{
    /// <summary>
    /// Links abc/swc resources inside assembly.
    /// </summary>
    internal class Linker : ISwcLinker
    {
        #region ctor
        private const int LinkFlag = 0x400;
        private const int LinkedOk = 0x800;

        const StringComparison strcmp = StringComparison.InvariantCultureIgnoreCase;
        readonly IAssembly _assembly;
        AbcCache _cache;

        public Linker(IAssembly asm)
        {
            _assembly = asm;
        }
        #endregion

        #region Start - EntryPoint
        public static bool Start(IAssembly asm)
        {
            var l = new Linker(asm);
            return l.Start();
        }

        public bool Start()
        {
            //To avoid multiple calls of this routine
            if ((_assembly.Marker & LinkFlag) != 0)
            {
                return (_assembly.Marker & LinkedOk) != 0;
            }

            _assembly.Marker |= LinkFlag;

            bool result = false;
            foreach (var res in _assembly.MainModule.Resources)
            {
#if DEBUG
                DebugService.DoCancel();
#endif
                string resName = res.Name;
                if (resName.EndsWith(".abc", strcmp))
                {
                    LinkAbc(res.Data);
                    result = true;
                }
                else if (resName.EndsWith(".swc", strcmp))
                {
                    string swcName = GetResFileName(resName);

                    var deps = LoadSwcDep(swcName);

                    LinkSwc(swcName, res.Data, deps);

                    result = true;
                }
            }

            if (result)
                _assembly.Marker |= LinkedOk;

            return result;
        }

        static string GetResFileName(string resName)
        {
            int i = resName.LastIndexOf('.');
            string name = resName.Substring(0, i).Trim();
            i = name.LastIndexOf('.', i - 1);
            if (i >= 0)
                name = name.Substring(i + 1).Trim();
            return name;
        }

        SwcDepFile LoadSwcDep(string swcName)
        {
            foreach (var res in _assembly.MainModule.Resources)
            {
                string resName = res.Name;
                if (resName.EndsWith(".swcdep", strcmp))
                {
                    string fname = GetResFileName(resName);
                    if (string.Compare(fname, swcName, true) == 0)
                    {
                        var f = new SwcDepFile();
                        f.Load(res.Data);
                        return f;
                    }
                }
            }
            return null;
        }
        #endregion

        #region ISwcLinker Members
        public IAssembly Assembly
        {
            get { return _assembly; }
        }

        public void LinkType(AbcInstance instance)
        {
            //var type = _assembly.Types[instance.FullName];
            //if (type != null)
            //    LinkType(type, instance);
        }

        public object ResolveExternalReference(string id)
        {
            return AssemblyIndex.ResolveRef(_assembly, id);
        }
        #endregion

        #region LinkAbc
        AbcFile _abc;

        void LinkAbc(byte[] data)
        {
#if PERF
            int start = Environment.TickCount;
#endif
            _abc = new AbcFile(data) {Assembly = _assembly};
            AssemblyTag.Instance(_assembly).ABC = _abc;
            _cache = new AbcCache(true);
            _cache.Add(_abc);
            LinkTypes();
#if PERF
            Console.WriteLine("LinkAbc: {0}", Environment.TickCount - start);
#endif
        }
        #endregion

        #region LinkSwc
        SwcFile _swc;

        void LinkSwc(string name, byte[] data, SwcDepFile deps)
        {
#if PERF
            int start = Environment.TickCount;
#endif
            _swc = new SwcFile(data)
                          {
                              Assembly = _assembly,
                              Name = name
                          };
            _cache = _swc.AbcCache;
            AssemblyTag.Instance(_assembly).SWC = _swc;
            _swc.ResolveDependencies(this, deps);
            LinkTypes();
#if PERF
            Console.WriteLine("LinkSwc: {0}", Environment.TickCount - start);
#endif
        }
        #endregion

        #region LinkTypes
        void LinkTypes()
        {
            foreach (var type in _assembly.Types)
            {
#if DEBUG
                DebugService.DoCancel();
#endif
                LinkType(type);
            }
        }

        public IType FindType(string fullname)
        {
            return AssemblyIndex.FindType(_assembly, fullname);
        }

        AbcInstance FindInstance(IType type)
        {
            return _cache.FindInstance(type);
        }

        bool IsCoreAPI
        {
            get 
            { 
                //return _cache.IsCoreAPI;
                return IsCorLib;
            }
        }

        static bool IsLinked(IType type)
        {
            var instance = type.Tag as AbcInstance;
            return instance != null;
        }

        bool IsCorLib
        {
            get { return _assembly.IsCorlib; }
        }

        void LinkType(IType type)
        {
            if (LinkInternalType(type)) return;
            if (IsLinked(type)) return;

            foreach (var attr in type.CustomAttributes)
            {
                switch (attr.TypeName)
                {
                    case Attrs.AbcInstance:
                        {
                            int index = GetIndex(attr);
                            var abc = GetAbcFile(type);
                            if (abc != null)
                            {
                                LinkType(type, abc.Instances[index]);
                                return;
                            }
                        }
                        break;

                    case Attrs.GlobalFunctions:
                        LinkGlobalType(type);
                        return;

                    case Attrs.Vector:
                        LinkVector(type, attr);
                        return;
                }
            }

            foreach (var attr in type.CustomAttributes)
            {
                switch (attr.TypeName)
                {
                    case Attrs.ABC:
                    case Attrs.QName:
                        {
                            var instance = FindInstance(type);
                            if (instance == null)
                                throw new InvalidOperationException("Unable to find instance for type");

                            LinkType(type, instance);
                        }
                        return;
                }
            }
        }

        void LinkType(IType type, AbcInstance instance)
        {
            type.Tag = instance;
            instance.Type = type;

            if (IsCoreAPI)
            {
                if (instance.IsGlobal)
                {
                    switch (instance.NameString)
                    {
                        case Const.AvmGlobalTypes.Object:
                            AssemblyTag.Instance(_assembly).InstanceObject = instance;
                            break;

                        case Const.AvmGlobalTypes.Error:
                            AssemblyTag.Instance(_assembly).InstanceError = instance;
                            break;
                    }
                }
                instance.IsNative = true;
            }

            LinkMethods(type, instance, false);
            LinkFields(type, instance);
        }
        #endregion

        #region LinkMethods
        void LinkMethods(IType type, IAbcTraitProvider owner, bool isGlobal)
        {
            foreach (var method in type.Methods)
                LinkMethod(method, owner, isGlobal);
        }

        static AbcMethod FindMethod(AbcInstance instance, IMethod method)
        {
            var t = instance.FindTrait(method);
            if (t == null)
            {
                throw new InvalidOperationException();
            }
            return t.Method;
        }

        AbcMethod FindGlobalMethod(IMethod method)
        {
            return _cache.FindGlobalMethod(method);
        }

        void LinkMethod(IMethod method, IAbcTraitProvider owner, bool isGlobal)
        {
#if DEBUG
            DebugService.DoCancel();
#endif
            var instance = owner as AbcInstance;

            if (method.IsConstructor && !isGlobal)
            {
                method.Tag = instance.Initializer;
                instance.Initializer.SourceMethod = method;
                return;
            }

            if (!isGlobal)
            {
                if (LinkEvent(method, instance))
                    return;
            }

            if (!ShouldLink(method))
                return;

            var t = GetTrait(method, owner);
            if (t != null)
            {
                LinkMethod(method, t.Method);
                return;
            }

            var abcMethod = isGlobal ? FindGlobalMethod(method) : FindMethod(instance, method);
            LinkMethod(method, abcMethod);
        }

        void LinkMethod(IMethod method, AbcMethod abcMethod)
        {
            if (abcMethod == null)
                throw new InvalidOperationException("Unable to find method " + method);

            method.Tag = abcMethod;

            //Prevent to link overload methods
            if (method.Parameters.Count == abcMethod.ActualParamCount)
                abcMethod.SourceMethod = method;

            //var instance = abcMethod.Instance;
            //if (instance != null && !instance.IsNative && abcMethod.IsNative)
            //    instance.IsNative = true;
        }
        #endregion

        #region LinkEvent
        private static bool LinkEvent(IMethod method, AbcInstance instance)
        {
            var e = method.Association as IEvent;
            if (e == null) return false;
            
            var attr = Attrs.Find(e, Attrs.Event);
            if (attr == null) return false;

            string eventName = attr.Arguments[0].Value as string;
            if (string.IsNullOrEmpty(eventName))
                throw new InvalidOperationException();

            if (e.Adder == method)
            {
                //stack transition: dispatcher, delegate -> ...
                var code = new AbcCode(instance.ABC);
                code.Swap();
                code.PushString(eventName);
                code.CallVoid(GetDelegateMethodName(true), 2);
                method.Tag = code;
                return true;
            }

            if (e.Remover == method)
            {
                //stack transition: delegate -> ...
                var code = new AbcCode(instance.ABC);
                code.Swap();
                code.PushString(eventName);
                code.CallVoid(GetDelegateMethodName(false), 2);
                method.Tag = code;
                return true;
            }
            throw new NotImplementedException();
        }

        private static string GetDelegateMethodName(bool add)
        {
            return NameUtil.GetMethodName(SystemTypes.Delegate,
                                             add
                                                 ? Const.Delegate.AddEventListeners
                                                 : Const.Delegate.RemoveEventListeners, 2);
        }
        #endregion

        #region LinkFields
        void LinkFields(IType type, AbcInstance instance)
        {
            foreach (var field in type.Fields)
                LinkField(field, instance);
        }

        void LinkField(IField field, AbcInstance instance)
        {
#if DEBUG
            DebugService.DoCancel();
#endif
            var t = GetTrait(field, instance);
            if (t != null)
            {
                LinkField(field, t);
                return;
            }

            if (!ShouldLink(field)) return;

            t = instance.FindTrait(field);
            LinkField(field, t);
        }

        static void LinkField(IField field, AbcTrait trait)
        {
            if (trait == null)
                throw new InvalidOperationException("Unable to link field " + field);
            field.Tag = trait;
        }
        #endregion

        #region LinkInternalType
        static bool LinkInternalType(IType type)
        {
            if (TypeHelper.IsInternalType(type))
            {
                var tag = new InternalType(type);
                Debug.Assert(type.Tag == tag);
                return true;
            }
            return false;
        }
        #endregion

        #region LinkGlobalType
        void LinkGlobalType(IType type)
        {
            var tag = new GlobalType(type);
            Debug.Assert(type.Tag == tag);
            LinkMethods(type, null, true);
        }
        #endregion

        #region LinkVector
        void LinkVector(IType type, ICustomAttribute attr)
        {
            var v = type.Tag as VectorType;
            if (v != null) return;

            string param = VectorType.GetVectorParam(attr);
            if (param == null) return;

            var paramType = FindType(param);
            if (paramType == null)
                throw new InvalidOperationException(string.Format("Unable to find Vector param type: {0}", param));

            v = new VectorType(type, paramType);
            Debug.Assert(type.Tag == v);
        }
        #endregion

        #region Utils
        AbcTrait GetTrait(ICustomAttributeProvider cp, IAbcTraitProvider owner)
        {
            foreach (var attr in cp.CustomAttributes)
            {
                switch (attr.TypeName)
                {
                    case Attrs.AbcInstanceTrait:
                        {
                            int index = GetIndex(attr);
                            return owner.Traits[index];
                        }

                    case Attrs.AbcClassTrait:
                        {
                            int index = GetIndex(attr);
                            return ((AbcInstance)owner).Class.Traits[index];
                        }

                    case Attrs.AbcScriptTrait:
                        {
                            var abc = GetAbcFileCore(cp);
                            if (abc == null) return null;

                            var scriptAttr = Attrs.Find(cp, Attrs.AbcScript);
                            if (scriptAttr == null) return null;

                            int scriptIndex = GetIndex(scriptAttr);
                            int index = GetIndex(attr);

                            return abc.Scripts[scriptIndex].Traits[index];
                        }
                }
            }
            return null;
        }

        private AbcFile GetAbcFile(ITypeMember member)
        {
            var type = member as IType ?? member.DeclaringType;

        	return GetAbcFileCore(type);
        }

        private AbcFile GetAbcFileCore(ICustomAttributeProvider cp)
        {
            if (_abc != null) return _abc;
            if (_swc == null) return null;

            var attr = Attrs.Find(cp, Attrs.SwcAbcFile);
            if (attr == null) return null;

            int lib = 0;
            int file = 0;
            if (attr.Arguments.Count == 1)
            {
                file = GetInt(attr, 0);
            }
            else
            {
                lib = GetInt(attr, 0);
                file = GetInt(attr, 1);
            }
            return _swc.GetLibrary(lib).GetAbcFiles()[file];
        }

        static int GetIndex(ICustomAttribute attr)
        {
            return GetInt(attr, 0);
        }

        static int GetInt(ICustomAttribute attr, int index)
        {
            return ((IConvertible)attr.Arguments[index].Value).ToInt32(null);
        }

        static bool ShouldLink(ICustomAttributeProvider cp)
        {
            if (cp is IType)
            {
                foreach (var attr in cp.CustomAttributes)
                {
                    switch (attr.TypeName)
                    {
                        case Attrs.AbcInstance:
                        case Attrs.AbcScript:
                        case Attrs.ABC:
                        case Attrs.QName:
                        case Attrs.GlobalFunctions:
                        case Attrs.Vector:
                            return true;
                    }
                }
            }
            else
            {
                foreach (var attr in cp.CustomAttributes)
                {
                    switch (attr.TypeName)
                    {
                        case Attrs.AbcInstanceTrait:
                        case Attrs.AbcClassTrait:
                        case Attrs.AbcScriptTrait:
                        case Attrs.ABC:
                        case Attrs.QName:
                            return true;
                    }
                }
            }

            return false;
        }
        #endregion
    }
}