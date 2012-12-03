using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using DataDynamics.PageFX.Common.Extensions;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FLI.ABC;

namespace DataDynamics.PageFX.FLI
{
	//TODO: lazy caching of types/instances

    internal sealed class AssemblyIndex
    {
        private readonly Hashtable _typeCache = new Hashtable();

        public static object ResolveRef(IAssembly asm, string id)
        {
            string name = id.Replace(':', '.');
            var instance = FindInstance(asm, name);
            if (instance != null)
            {
                if (instance.InSwc)
                    return instance.Abc;
            }
            return instance;
        }

        public static void Setup(IAssembly asm)
        {
            if (asm == null)
                throw new ArgumentNullException("asm");
            var tag = AssemblyTag.Instance(asm);
            Debug.Assert(tag != null);
            if (tag.Index is AssemblyIndex) return;
            tag.Index = new AssemblyIndex(asm);
        }

        public static IType FindType(IAssembly asm, string name)
        {
            Setup(asm);
            var idx = AssemblyTag.Instance(asm).Index as AssemblyIndex;
            if (idx != null)
                return idx.FindTypeCore(name);
            return null;
        }

        public static IType FindType(IAssembly asm, AbcMultiname name)
        {
        	return name.GetFullNames().Select(fullName => FindType(asm, fullName)).FirstOrDefault(type => type != null);
        }

    	public static AbcInstance FindInstance(IAssembly asm, string name)
        {
            Setup(asm);
            var idx = AssemblyTag.Instance(asm).Index as AssemblyIndex;
            if (idx != null)
                return idx.FindInstanceCore(name);
            return null;
        }

        public static AbcInstance FindInstance(IAssembly asm, AbcMultiname name)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (name.IsRuntime) 
                return null;
            if (name.NamespaceSet != null)
            {
            	return name.NamespaceSet
					.Select(ns => ns.NameString.MakeFullName(name.NameString))
					.Select(fullName => FindInstance(asm, fullName))
					.FirstOrDefault(instance => instance != null);
            }
        	return FindInstance(asm, name.FullName);
        }

        private AssemblyIndex(IAssembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException("assembly");
            Build(assembly);
        }

	    private void Build(IAssembly root)
	    {
		    root.ProcessReferences(
			    false,
			    asm =>
				    {
					    if (!ReferenceEquals(asm, root))
						    Linker.Start(asm);
					    CacheTypes(asm);
				    });
	    }

	    private void CacheTypes(IAssembly asm)
        {
            AssemblyTag.Instance(asm).Index = this;
            foreach (var type in asm.Types)
            {
                string name = type.FullName;
                _typeCache[name] = type;

                var instance = type.Tag as AbcInstance;
                if (instance != null)
                {
                    string name2 = instance.FullName;
                    if (name2 != name)
                        _typeCache[name2] = type;
                }
            }
        }

        private IType FindTypeCore(string name)
        {
            if (name == null) return null;
            return _typeCache[name] as IType;
        }

        private AbcInstance FindInstanceCore(string name)
        {
            var type = FindTypeCore(name);
            if (type != null)
                return type.Tag as AbcInstance;
            return null;
        }
    }
}