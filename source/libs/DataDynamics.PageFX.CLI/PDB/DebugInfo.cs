using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.PDB
{
    class MethodLocal
    {
        public MethodLocal(int index, string name)
        {
            Index = index;
            Name = name;
        }

        public int Index;
        public string Name;
    }

    class MethodDebugInfo
    {
        public List<SequencePoint> SequencePoints;
        public MethodLocal[] Locals;
    }

    class DebugInfo
    {
        readonly Dictionary<int, MethodDebugInfo> _cache = new Dictionary<int, MethodDebugInfo>();

        public DebugInfo(ISymbolReader reader, IMethod[] methods)
        {
            foreach (var method in methods)
            {
                var symMethod = SymbolUtil.GetSymbolMethod(reader, method);
                if (symMethod == null) continue;

                var info = new MethodDebugInfo
                               {
                                   SequencePoints = SymbolUtil.ReadSequencePoints(symMethod),
                                   Locals = ReadLocals(symMethod.RootScope)
                               };
                _cache.Add(method.MetadataToken, info);
            }
        }

        public MethodDebugInfo GetInfo(IMethod method)
        {
            MethodDebugInfo info;
            if (_cache.TryGetValue(method.MetadataToken, out info))
                return info;
            return null;
        }

        

        static MethodLocal[] ReadLocals(ISymbolScope scope)
        {
            var locals = new List<MethodLocal>();
            ReadLocals(scope, locals);
            return locals.ToArray();
        }

        static void ReadLocals(ISymbolScope scope, List<MethodLocal> locals)
        {
            try
            {
                foreach (var l in scope.GetLocals())
                {
                    if (l.AddressKind == SymAddressKind.ILOffset)
                    {
                        int slot = l.AddressField1;
                        locals.Add(new MethodLocal(slot, l.Name));
                    }
                }

                foreach (var child in scope.GetChildren())
                    ReadLocals(child, locals);
            }
            catch (Exception e)
            {
            }
        }
    }
}