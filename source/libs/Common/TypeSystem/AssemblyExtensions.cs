using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DataDynamics.PageFX.Common.TypeSystem
{
    public static class AssemblyExtensions
    {
        public static IEnumerable<IAssembly> GetReferences(this IAssembly root, bool excludeRoot)
        {
            if (root == null)
                throw new ArgumentNullException("root");
            var marked = new Hashtable();
            var stack = new Stack<IAssembly>();
            stack.Push(root);
            while (stack.Count > 0)
            {
                var assembly = stack.Pop();
                if (!(excludeRoot && ReferenceEquals(assembly, root)))
                    yield return assembly;
                foreach (var item in assembly.MainModule.References.Where(x => !marked.Contains(x)))
                {
	                marked[item] = item;
	                stack.Push(item);
                }
            }
        }

        public static void ProcessReferences(this IAssembly root, bool excludeRoot, Action<IAssembly> handler)
        {
            if (root == null)
                throw new ArgumentNullException("root");
            if (handler == null)
                throw new ArgumentNullException("handler");
            foreach (var assembly in root.GetReferences(excludeRoot))
            {
                handler(assembly);
            }
        }

    	public static IManifestResource FindResource(this IAssembly asm, string subname)
    	{
    		subname = subname.ToLower();
    		return (from resource in asm.MainModule.Resources
    		        let name = resource.Name.ToLower()
    		        where name.Contains(subname)
    		        select resource).FirstOrDefault();
    	}

		public static IAssembly Corlib(this IAssembly assembly)
		{
			if (assembly.IsCorlib)
				return assembly;
			return assembly.GetReferences(true).FirstOrDefault(x => x.IsCorlib);
		}

	    public static IType ResolveType(this IAssembly assembly, Type type)
	    {
		    if (type == null)
			    throw new ArgumentNullException("type");
		    var typeCode = Type.GetTypeCode(type);
		    switch (typeCode)
		    {
			    case TypeCode.Empty:
			    case TypeCode.DBNull:
				    return null;
			    case TypeCode.Object:
				    return assembly.GetReferences(false).Select(x => x.FindType(type.FullName)).FirstOrDefault(x => x != null);
			    default:
				    return assembly.SystemTypes[typeCode];
		    }
	    }
    }
}