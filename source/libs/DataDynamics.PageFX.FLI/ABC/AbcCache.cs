using System.Collections;
using System.Collections.Generic;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.FLI.ABC
{
    internal class AbcCache
    {
        public List<AbcInstance> Mixins
        {
            get { return _mixins; }
        }
        readonly List<AbcInstance> _mixins = new List<AbcInstance>();

        readonly Hashtable _instanceCache = new Hashtable();
        static readonly Hashtable _nsCache = new Hashtable();
        readonly Hashtable _globalMethods = new Hashtable();

        public AbcCache(bool checkCoreAPI)
        {
            _checkCoreAPI = checkCoreAPI;
        }

        public bool IsCoreAPI
        {
            get { return _isCoreAPI; }
        }
        bool _isCoreAPI;
        readonly bool _checkCoreAPI;

        public AbcInstance FindInstance(IType type)
        {
            var qn = new AbcQName(type);
            return FindInstance(qn.FullName);
        }

        public AbcInstance FindInstance(string fullname)
        {
            return _instanceCache[fullname] as AbcInstance;
        }

        public AbcFile FindNamespace(string name)
        {
            return _nsCache[name] as AbcFile;
        }

        public AbcMethod FindGlobalMethod(IMethod method)
        {
            var qn = new AbcQName(method);
            return _globalMethods[qn.Key] as AbcMethod;
        }

        public void Add(AbcFile abc)
        {
            foreach (var instance in abc.Instances)
            {
                string fn = instance.FullName;
                _instanceCache[fn] = instance;
                if (_checkCoreAPI && !_isCoreAPI && fn == "Object")
                    abc.IsCoreAPI = _isCoreAPI = true;
            }

            foreach (var script in abc.Scripts)
            {
                foreach (var t in script.Traits)
                {
                    switch (t.Kind)
                    {
                        case AbcTraitKind.Const:
                            {
                                var ns = t.SlotValue as AbcNamespace;
                                if (ns != null)
                                    _nsCache[t.Name.FullName] = abc;
                            }
                            break;

                        case AbcTraitKind.Method:
                            {
                                string key = AbcQName.GetKey(t);
                                _globalMethods[key] = t.Method;
                            }
                            break;
                    }
                }
            }
        }

        public void Add(IEnumerable<AbcFile> set)
        {
            foreach (var abc in set)
                Add(abc);
        }
    }
}