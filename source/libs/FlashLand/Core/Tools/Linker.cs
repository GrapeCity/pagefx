using System;
using System.Diagnostics;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.Abc;
using DataDynamics.PageFX.FlashLand.Core.SpecialTypes;
using DataDynamics.PageFX.FlashLand.IL;
using DataDynamics.PageFX.FlashLand.Swc;

namespace DataDynamics.PageFX.FlashLand.Core.Tools
{
    /// <summary>
    /// Links abc/swc resources inside assembly.
    /// </summary>
    internal sealed class Linker : ISwcLinker
    {
	    private readonly IAssembly _assembly;
		private AbcCache _cache;
		private AbcFile _abc;
		private SwcFile _swc;

        public Linker(IAssembly assembly)
        {
	        if (assembly == null)
				throw new ArgumentNullException("assembly");

			var data = assembly.CustomData();
			if (data.Linker != null)
				throw new InvalidOperationException();

			data.Linker = this;

	        _assembly = assembly;
        }

	    public IAssembly Assembly
		{
			get { return _assembly; }
		}

		public object ResolveExternalReference(string id)
		{
			return AssemblyIndex.ResolveRef(_assembly, id);
		}

	    public event EventHandler<TypeEventArgs> TypeLinked;

	    public static bool Run(IAssembly assembly)
        {
            var linker = new Linker(assembly);
            return linker.Run();
        }

        public bool Run()
        {
            //To avoid multiple calls of this routine
	        var data = _assembly.CustomData();
            if ((data.Flags & InternalAssembyFlags.PassedLinker) != 0)
            {
				return (data.Flags & InternalAssembyFlags.HasAbcImports) != 0;
            }

			data.Flags |= InternalAssembyFlags.PassedLinker;

            bool result = false;
            foreach (var res in _assembly.MainModule.Resources)
            {
#if DEBUG
                DebugService.DoCancel();
#endif
	            if (res.Name.EndsWith(".abc", StringComparison.InvariantCultureIgnoreCase))
                {
                    LinkAbc(res.Data);
                    result = true;
                }
                else if (res.Name.EndsWith(".swc", StringComparison.InvariantCultureIgnoreCase))
                {
                    string swcName = GetResFileName(res.Name);

                    var deps = LoadSwcDep(swcName);

                    LinkSwc(swcName, res.Data, deps);

                    result = true;
                }
            }

	        if (result)
	        {
				data.Flags |= InternalAssembyFlags.HasAbcImports;
	        }

	        return result;
        }

        private static string GetResFileName(string resName)
        {
            int i = resName.LastIndexOf('.');
            string name = resName.Substring(0, i).Trim();
            i = name.LastIndexOf('.', i - 1);
            if (i >= 0)
                name = name.Substring(i + 1).Trim();
            return name;
        }

        private SwcDepFile LoadSwcDep(string swcName)
        {
            foreach (var res in _assembly.MainModule.Resources)
            {
                string resName = res.Name;
                if (resName.EndsWith(".swcdep", StringComparison.InvariantCultureIgnoreCase))
                {
                    string fname = GetResFileName(resName);
					if (string.Equals(fname, swcName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        var f = new SwcDepFile();
                        f.Load(res.Data);
                        return f;
                    }
                }
            }
            return null;
        }

	    private void LinkAbc(byte[] data)
        {
#if PERF
            int start = Environment.TickCount;
#endif
            _abc = new AbcFile(data) {Assembly = _assembly};
            _assembly.CustomData().ABC = _abc;
            _cache = new AbcCache(true);
            _cache.Add(_abc);

            LinkTypes();
#if PERF
            Console.WriteLine("LinkAbc: {0}", Environment.TickCount - start);
#endif
        }

        private void LinkSwc(string name, byte[] data, SwcDepFile deps)
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
            _assembly.CustomData().SWC = _swc;
            _swc.ResolveDependencies(this, deps);

            LinkTypes();
#if PERF
            Console.WriteLine("LinkSwc: {0}", Environment.TickCount - start);
#endif
        }

	    private void LinkTypes()
        {
			if (_assembly.Loader != null)
			{
				_assembly.Loader.TypeLoaded += OnTypeLoaded;
				return;
			}

            foreach (var type in _assembly.Types)
            {
#if DEBUG
                DebugService.DoCancel();
#endif
                LinkType(type);
            }
        }

	    //private int _counter;

	    private void OnTypeLoaded(object sender, TypeEventArgs e)
	    {
		    LinkType(e.Type);
			//Debug.WriteLine((++_counter) + ":" + e.Type.FullName);
	    }

		private void FireTypeLinked(IType type)
		{
			if (TypeLinked != null)
			{
				TypeLinked(this, new TypeEventArgs(type));
			}
		}

	    public IType FindType(string fullname)
        {
            return AssemblyIndex.FindType(_assembly, fullname);
        }

        private AbcInstance FindInstance(IType type)
        {
            return _cache.FindInstance(type);
        }

	    private static bool IsLinked(IType type)
        {
            var instance = type.Tag as AbcInstance;
            return instance != null;
        }

        private bool IsCorLib
        {
            get { return _assembly.IsCorlib; }
        }

        private void LinkType(IType type)
        {
            if (LinkInternalType(type)) return;

            if (IsLinked(type)) return;

            foreach (var attr in type.CustomAttributes)
            {
                switch (attr.TypeName)
                {
	                case Attrs.AbcInstance:
		                int index = GetIndex(attr);
		                var abc = GetAbcFile(type);
		                if (abc != null)
		                {
			                LinkType(type, abc.Instances[index]);
			                return;
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

			FireTypeLinked(type);
        }

        private void LinkType(IType type, AbcInstance instance)
        {
            type.Tag = instance;
            instance.Type = type;

            if (IsCorLib)
            {
                if (instance.IsGlobal)
                {
                    switch (instance.NameString)
                    {
                        case Const.AvmGlobalTypes.Object:
                            _assembly.CustomData().ObjectInstance = instance;
                            break;

                        case Const.AvmGlobalTypes.Error:
                            _assembly.CustomData().ErrorInstance = instance;
                            break;
                    }
                }
                instance.IsNative = true;
            }

            LinkMethods(type, instance, false);
            LinkFields(type, instance);
        }

	    private void LinkMethods(IType type, IAbcTraitProvider owner, bool isGlobal)
        {
            foreach (var method in type.Methods)
                LinkMethod(method, owner, isGlobal);
        }

        private static AbcMethod FindMethod(AbcInstance instance, IMethod method)
        {
            var t = instance.FindTrait(method);
            if (t == null)
            {
                throw new InvalidOperationException();
            }
            return t.Method;
        }

        private AbcMethod FindGlobalMethod(IMethod method)
        {
            return _cache.FindGlobalMethod(method);
        }

        private void LinkMethod(IMethod method, IAbcTraitProvider owner, bool isGlobal)
        {
#if DEBUG
            DebugService.DoCancel();
#endif
            var instance = owner as AbcInstance;

            if (instance != null && method.IsConstructor && !isGlobal)
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

        private static void LinkMethod(IMethod method, AbcMethod abcMethod)
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

	    private bool LinkEvent(IMethod method, AbcInstance instance)
        {
            var e = method.Association as IEvent;
            if (e == null) return false;
            
            var attr = e.FindAttribute(Attrs.Event);
            if (attr == null) return false;

            string eventName = attr.Arguments[0].Value as string;
            if (string.IsNullOrEmpty(eventName))
                throw new InvalidOperationException();

            if (e.Adder == method)
            {
                //stack transition: dispatcher, delegate -> ...
                var code = new AbcCode(instance.Abc);
                code.Swap();
                code.PushString(eventName);
                code.CallVoid(GetDelegateMethodName(true), 2);
                method.Tag = code;
                return true;
            }

            if (e.Remover == method)
            {
                //stack transition: delegate -> ...
                var code = new AbcCode(instance.Abc);
                code.Swap();
                code.PushString(eventName);
                code.CallVoid(GetDelegateMethodName(false), 2);
                method.Tag = code;
                return true;
            }
            throw new NotImplementedException();
        }

	    private string GetDelegateMethodName(bool add)
	    {
		    var type = _assembly.SystemTypes.Delegate;
		    return type.GetMethodName(add ? Const.Delegate.AddEventListeners : Const.Delegate.RemoveEventListeners, 2);
	    }

	    private void LinkFields(IType type, AbcInstance instance)
        {
            foreach (var field in type.Fields)
                LinkField(field, instance);
        }

        private void LinkField(IField field, AbcInstance instance)
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

        private static void LinkField(IField field, AbcTrait trait)
        {
            if (trait == null)
                throw new InvalidOperationException("Unable to link field " + field);
            field.Tag = trait;
        }

	    private bool LinkInternalType(IType type)
        {
            if (type.IsInternalType())
            {
                var tag = new InternalType(type);

	            type.Tag = tag;

				FireTypeLinked(type);

                return true;
            }
            return false;
        }

	    private void LinkGlobalType(IType type)
        {
            var tag = new GlobalType(type);

		    type.Tag = tag;

            LinkMethods(type, null, true);
        }

	    private void LinkVector(IType type, ICustomAttribute attr)
        {
            var vectorType = type.Tag as VectorType;
            if (vectorType != null) return;

            string param = VectorType.GetVectorParam(attr);
            if (param == null) return;

            var paramType = FindType(param);
            if (paramType == null)
                throw new InvalidOperationException(string.Format("Unable to find Vector param type: {0}", param));

            vectorType = new VectorType(type, paramType);

		    type.Tag = vectorType;
        }

	    private AbcTrait GetTrait(ICustomAttributeProvider cp, IAbcTraitProvider owner)
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

                            var scriptAttr = cp.FindAttribute(Attrs.AbcScript);
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

            var attr = cp.FindAttribute(Attrs.SwcAbcFile);
            if (attr == null) return null;

            int lib = 0;
            int file;
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

        private static int GetIndex(ICustomAttribute attr)
        {
            return GetInt(attr, 0);
        }

        private static int GetInt(ICustomAttribute attr, int index)
        {
            return ((IConvertible)attr.Arguments[index].Value).ToInt32(null);
        }

        private static bool ShouldLink(ICustomAttributeProvider cp)
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
    }
}