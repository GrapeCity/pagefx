using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.FLI.ABC;

namespace DataDynamics.PageFX.FLI.SWC
{
    class SwcDepBuilder
    {
        #region Private Fields
        class Dep
        {
            public string ID;
            public string Type;
        }

        readonly List<Dep> _deps = new List<Dep>();
        readonly Hashtable _cache = new Hashtable();
        AbcFile _app;
        AbcInstance _def;
        string _defID;
        #endregion

        #region Build
        public List<string[]> Build(AbcFile app, AbcInstance def, string defID)
        {
            _app = app;
            _def = def;
            _defID = defID;

            AddInheritanceRef(def.SuperName);

            foreach (var ifaceName in def.Interfaces)
                AddInheritanceRef(ifaceName);

            AddNamespaceRef("AS3");

            GetSigRefs();
            GetExpressionRefs(def.ABC);

			return _deps.Select(d => new[] { d.ID, d.Type }).ToList();
        }
        #endregion

        #region GetExpressionRefs
        void GetExpressionRefs(AbcFile abc)
        {
            //TODO: namespace refs
            int n = abc.Multinames.Count;
            for (int i = 1; i < n; ++i)
            {
                var mn = abc.Multinames[i];
                if (mn.IsRuntime) continue;

                foreach (var fullName in mn.GetFullNames())
                {
                    if (string.IsNullOrEmpty(fullName)) continue;
                    var type = _app.FindInstance(fullName);
                    if (type != null)
                    {
                        if (type.IsNative) continue;
                        Add(type.Name, SwcCatalog.DepTypes.Expression);
                    }
                }
            }
        }
        #endregion

        #region GetSigRefs
        void GetSigRefs()
        {
            foreach (var trait in _def.GetAllTraits())
                GetSigRefs(trait);
        }

        void GetSigRefs(AbcTrait trait)
        {
            switch (trait.Kind)
            {
                case AbcTraitKind.Class:
                    throw new InvalidOperationException();

                case AbcTraitKind.Const:
                case AbcTraitKind.Slot:
                    AddSigRef(trait.SlotType);
                    break;

                case AbcTraitKind.Function:
                case AbcTraitKind.Getter:
                case AbcTraitKind.Method:
                case AbcTraitKind.Setter:
                    {
                        var m = trait.Method;
                        AddSigRef(m.ReturnType);
                        foreach (var p in m.Parameters)
                            AddSigRef(p.Type);
                    }
                    break;
            }
        }

        

        void AddSigRef(AbcMultiname name)
        {
            if (name == null) return;
            if (name.IsAny) return;
            //if (name.IsGlobalType) return;
            //var type = _app.FindInstance(name.FullName);
            //if (type != null && type.IsNative) return;
            Add(name, SwcCatalog.DepTypes.Signature);
        }
        #endregion

        #region Add
        void AddInheritanceRef(AbcMultiname name)
        {
            Add(name, SwcCatalog.DepTypes.Inheritance);
        }

        void AddNamespaceRef(string id)
        {
            Add(id, SwcCatalog.DepTypes.Namespace);
        }

        void Add(AbcMultiname name, string type)
        {
            if (name == null) return;
            Add(name.ToSwcId(), type);
        }

        void Add(string id, string type)
        {
            if (id == _defID) return;
            var dep = _cache[id] as Dep;
            if (dep != null) return;

            dep = new Dep { ID = id, Type = type };
            _deps.Add(dep);
            _cache[id] = dep;
        }
        #endregion
    }
}