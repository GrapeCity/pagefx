using System;
using System.Collections;
using System.Collections.Generic;

namespace DataDynamics.PageFX.CodeModel
{
    public static class AssemblyHelper
    {
        public static IEnumerable<IAssembly> GetReferences(IAssembly root, bool excludeRoot)
        {
            if (root == null)
                throw new ArgumentNullException("root");
            var marked = new Hashtable();
            var stack = new Stack<IAssembly>();
            stack.Push(root);
            while (stack.Count > 0)
            {
                var asm = stack.Pop();
                if (!(excludeRoot && asm == root))
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

        public static void ProcessReferences(IAssembly root, bool excludeRoot, Action<IAssembly> handler)
        {
            if (root == null)
                throw new ArgumentNullException("root");
            if (handler == null)
                throw new ArgumentNullException("handler");
            foreach (var assembly in GetReferences(root, excludeRoot))
            {
                handler(assembly);
            }
        }
    }
}