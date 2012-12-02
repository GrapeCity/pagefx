using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DataDynamics.PageFX.CodeModel.TypeSystem
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
                var asm = stack.Pop();
                if (!(excludeRoot && ReferenceEquals(asm, root)))
                    yield return asm;
                foreach (var ar in asm.MainModule.References)
                {
                    if (!marked.Contains(ar))
                    {
                        marked[ar] = ar;
                        stack.Push(ar);
                    }
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
    }
}