using System;
using System.Diagnostics;
using System.Linq;
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
	    private readonly AbcCache _cache;
		private readonly AbcFile _abc;
		private readonly SwcFile _swc;
		private readonly SwcDepFile _swcDeps;
	    
	    public Linker(IAssembly assembly)
        {
	        if (assembly == null)
				throw new ArgumentNullException("assembly");

			var data = assembly.CustomData();
			if (data.Linker != null)
				throw new InvalidOperationException();

			data.Linker = this;

	        Assembly = assembly;

			// we should always listen TypeLoaded event to fill AssemblyLinked
			if (Assembly.Loader != null)
			{
				Assembly.Loader.TypeLoaded += OnTypeLoaded;
			}

	        var resource = Assembly.MainModule.Resources.FirstOrDefault(
		        x => x.Name.EndsWith(".abc", StringComparison.OrdinalIgnoreCase)
		             || x.Name.EndsWith(".swc", StringComparison.OrdinalIgnoreCase));

			if (resource != null)
			{
				data.Flags |= InternalAssembyFlags.HasAbcImports;

				if (resource.Name.EndsWith(".abc", StringComparison.OrdinalIgnoreCase))
				{
					_abc = new AbcFile(resource.Data) { Assembly = Assembly };
					data.Abc = _abc;
					_cache = new AbcCache();
					_cache.Add(_abc);
				}
				else if (resource.Name.EndsWith(".swc", StringComparison.OrdinalIgnoreCase))
				{
					string swcName = GetResFileName(resource.Name);

					_swcDeps = LoadSwcDep(swcName);
					_swc = new SwcFile(resource.Data)
						{
							Assembly = Assembly,
							Name = swcName
						};
					_cache = _swc.AbcCache;

					data.SWC = _swc;
				}
			}

			// create all linkers to subscribe on AssemblyLoader.TypeLoaded event before we start process
			assembly.ProcessReferences(true,
				asm =>
					{
						if (asm.CustomData().Linker == null)
							asm.CustomData().Linker = new Linker(asm);
					});

		    if (_swc != null)
		    {
			    _swc.ResolveDependencies(this, _swcDeps);
		    }
        }

	    public IAssembly Assembly { get; private set; }

	    public object ResolveExternalReference(string id)
		{
			return AssemblyIndex.ResolveRef(Assembly, id);
		}

	    public event EventHandler<TypeEventArgs> TypeLinked;

	    public static bool Run(IAssembly assembly)
        {
			var data = assembly.CustomData();
			if ((data.Flags & InternalAssembyFlags.PassedLinker) != 0)
			{
				return (data.Flags & InternalAssembyFlags.HasAbcImports) != 0;
			}

		    var linker = data.Linker ?? new Linker(assembly);
            return linker.Run();
        }

        public bool Run()
        {
            //To avoid multiple calls of this routine
	        var data = Assembly.CustomData();
            if ((data.Flags & InternalAssembyFlags.PassedLinker) != 0)
            {
				return (data.Flags & InternalAssembyFlags.HasAbcImports) != 0;
            }

			data.Flags |= InternalAssembyFlags.PassedLinker;

#if PERF
            int start = Environment.TickCount;
#endif
			LinkTypes();

#if PERF
            Console.WriteLine("LinkSwc: {0}", Environment.TickCount - start);
#endif

			return (data.Flags & InternalAssembyFlags.HasAbcImports) != 0;
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
		    foreach (var res in Assembly.MainModule.Resources)
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

	    private void LinkTypes()
        {
			if (Assembly.Loader != null)
			{
				// types will be linked on Assembly.Loader.TypeLoaded event
				return;
			}

            foreach (var type in Assembly.Types)
            {
                LinkType(type);
            }
        }

	    //private int _counter;

	    private void OnTypeLoaded(object sender, TypeEventArgs e)
	    {
		    var type = e.Type;
			if (IsLinked(type))
			{
				FireTypeLinked(type);
			}
			else
			{
				LinkType(type);
			}

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
            return AssemblyIndex.FindType(Assembly, fullname);
        }

        private AbcInstance FindInstance(IType type)
        {
            return _cache.Instances.Find(type);
        }

	    private static bool IsLinked(IType type)
	    {
		    return type.Data is NativeType
		           || type.AbcInstance() != null;
	    }

	    private void LinkType(IType type)
		{
			if (_cache != null)
			{
				LinkTypeCore(type);
			}
			else
			{
				LinkNativeType(type);
			}

			FireTypeLinked(type);
		}

	    private void LinkTypeCore(IType type)
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
        }

		private void LinkNativeType(IType type)
		{
			if (!Assembly.IsCorlib) return;

			if (type.HasAttribute(Attrs.Native))
			{
				var qnameAttr = type.FindAttribute(Attrs.QName);
				if (qnameAttr == null)
					throw new InvalidOperationException();

				var qname = QName.FromAttribute(qnameAttr);
				type.Data = new NativeType(type, qname);
				return;
			}

			if (type.HasAttribute(Attrs.GlobalFunctions))
			{
				type.Data = new GlobalFunctionsContainer(type);
			}
		}

        private void LinkType(IType type, AbcInstance instance)
        {
            type.Data = instance;
            instance.Type = type;

            if (IsFlashApi)
            {
                instance.IsNative = true;
            }

            LinkMethods(type, instance, false);
            LinkFields(type, instance);
        }

	    private bool IsFlashApi
	    {
		    get
		    {
				//TODO: determine by abc/swc resource
			    return Assembly.Name.StartsWith("flash.v", StringComparison.OrdinalIgnoreCase);
		    }
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
            return _cache.Functions.Find(method);
        }

        private void LinkMethod(IMethod method, IAbcTraitProvider owner, bool isGlobal)
        {
            var instance = owner as AbcInstance;

            if (instance != null && method.IsConstructor && !isGlobal)
            {
	            LinkCtor(method, instance);
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

	    private static void LinkCtor(IMethod method, AbcInstance instance)
	    {
			if (method.HasAttribute(Attrs.InlineFunction)) return;

		    method.Data = instance.Initializer;
		    instance.Initializer.Method = method;
	    }

	    private static void LinkMethod(IMethod method, AbcMethod abcMethod)
        {
            if (abcMethod == null)
                throw new InvalidOperationException("Unable to find method " + method);

            method.Data = abcMethod;

            //Prevent to link overload methods
            if (method.Parameters.Count == abcMethod.ActualParamCount)
                abcMethod.Method = method;

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

            if (e.Adder == method || e.Remover == method)
            {
                //stack transition: dispatcher, delegate -> ...
                var code = new AbcCode(instance.Abc);
                code.Swap();
                code.PushString(eventName);
                code.CallVoid(GetDelegateMethodName(e.Adder == method), 2);
                method.Data = new InlineCall(method, null, null, code);
                return true;
            }

            throw new NotImplementedException();
        }

	    private string GetDelegateMethodName(bool add)
	    {
		    var type = Assembly.SystemTypes.Delegate;
		    return type.GetMethodName(add ? Const.Delegate.AddEventListeners : Const.Delegate.RemoveEventListeners, 2);
	    }

	    private void LinkFields(IType type, AbcInstance instance)
        {
            foreach (var field in type.Fields)
                LinkField(field, instance);
        }

        private void LinkField(IField field, AbcInstance instance)
        {
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
            field.Data = trait;
        }

	    private static bool LinkInternalType(IType type)
        {
            if (type.IsInternalType())
            {
	            type.Data = new InternalType(type);
                return true;
            }
            return false;
        }

	    private void LinkGlobalType(IType type)
        {
		    type.Data = new GlobalFunctionsContainer(type);

            LinkMethods(type, null, true);
        }

	    private void LinkVector(IType type, ICustomAttribute attr)
        {
            var vectorType = type.Data as VectorType;
            if (vectorType != null) return;

            string param = VectorType.GetVectorParam(attr);
            if (param == null) return;

            var paramType = FindType(param);
            if (paramType == null)
                throw new InvalidOperationException(string.Format("Unable to find Vector param type: {0}", param));

            vectorType = new VectorType(type, paramType);

		    type.Data = vectorType;
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