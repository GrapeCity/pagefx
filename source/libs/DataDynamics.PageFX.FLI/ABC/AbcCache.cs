using System.Collections;
using System.Collections.Generic;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.FLI.ABC
{
    internal sealed class AbcCache
    {
		private readonly List<AbcInstance> _mixins = new List<AbcInstance>();
		private readonly Hashtable _instanceCache = new Hashtable();
		private static readonly Hashtable NamespaceCache = new Hashtable();
		private readonly Hashtable _globalMethods = new Hashtable();
		private readonly bool _checkCoreAPI;

		public AbcCache(bool checkCoreAPI)
		{
			_checkCoreAPI = checkCoreAPI;
		}

        public IList<AbcInstance> Mixins
        {
            get { return _mixins; }
        }

    	public bool IsCoreAPI { get; private set; }

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
            return NamespaceCache[name] as AbcFile;
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
                if (_checkCoreAPI && !IsCoreAPI && fn == "Object")
                    abc.IsCoreAPI = IsCoreAPI = true;
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
                                    NamespaceCache[t.Name.FullName] = abc;
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