using System;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.FlashLand.Abc
{
	internal sealed class AbcCache
    {
		internal interface IInstanceCache
		{
			AbcInstance Find(IType type);
			AbcInstance Find(string fullname);
			AbcInstance Find(AbcMultiname multiname);
		}

		internal interface INamespaceCache
		{
			AbcFile Find(string fullname);
		}

		internal interface IFunctionCache
		{
			AbcMethod Find(IMethod method);
		}

		private sealed class InstanceCache : IInstanceCache
		{
			private readonly Dictionary<string, AbcInstance> _cache = new Dictionary<string, AbcInstance>();

			public void Add(AbcInstance instance)
			{
				string key = instance.FullName;
				_cache.Add(key, instance);
			}

			public AbcInstance Find(IType type)
			{
				return Find(MemberKey.FullName(type));
			}

			public AbcInstance Find(string fullname)
			{
				AbcInstance instance;
				return _cache.TryGetValue(fullname, out instance) ? instance : null;
			}

			public AbcInstance Find(AbcMultiname multiname)
			{
				if (multiname == null)
					throw new ArgumentNullException("multiname");

				if (multiname.IsAny || multiname.IsRuntime)
					return null;

				string name = multiname.NameString;
				if (string.IsNullOrEmpty(name))
					return null;

				return multiname.GetFullNames()
				                .Select(fullName => Find(fullName))
				                .FirstOrDefault(x => x != null);
			}
		}

		private sealed class NamespaceCache : INamespaceCache
		{
			private readonly Dictionary<string, AbcFile> _cache = new Dictionary<string, AbcFile>();

			public void Add(string name, AbcFile file)
			{
				_cache.Add(name, file);
			}

			public AbcFile Find(string fullname)
			{
				AbcFile value;
				return _cache.TryGetValue(fullname, out value) ? value : null;
			}
		}

		private sealed class FunctionCache : IFunctionCache
		{
			private readonly Dictionary<string, AbcMethod> _cache = new Dictionary<string, AbcMethod>();

			public void Add(AbcTrait trait)
			{
				string key = MemberKey.BuildKey(trait.Name);
				_cache.Add(key, trait.Method);
			}

			public AbcMethod Find(IMethod method)
			{
				var key = MemberKey.BuildKey(method);
				AbcMethod value;
				return _cache.TryGetValue(key,out value) ? value : null;
			}
		}

		private readonly List<AbcInstance> _mixins = new List<AbcInstance>();
		private readonly InstanceCache _instances = new InstanceCache();
		private readonly NamespaceCache _namespaces = new NamespaceCache();
		private readonly FunctionCache _functions = new FunctionCache();

		public IList<AbcInstance> Mixins
        {
            get { return _mixins; }
        }

    	public bool IsCoreApi { get; private set; }

		public IInstanceCache Instances
		{
			get { return _instances; }
		}

		public INamespaceCache Namespaces
		{
			get { return _namespaces; }
		}

		public IFunctionCache Functions
		{
			get { return _functions; }
		}

		public void Add(AbcFile abc)
        {
	        foreach (var instance in abc.Instances)
	        {
		        _instances.Add(instance);

		        if (!IsCoreApi)
		        {
			        string fullName = instance.FullName;
			        if (fullName == "Object")
				        abc.IsCore = IsCoreApi = true;
		        }
	        }

	        foreach (var trait in abc.Scripts.SelectMany(x => x.Traits))
	        {
		        switch (trait.Kind)
		        {
			        case AbcTraitKind.Const:
				        var ns = trait.SlotValue as AbcNamespace;
				        if (ns != null)
					        _namespaces.Add(trait.Name.FullName, abc);
				        break;

			        case AbcTraitKind.Method:
				        _functions.Add(trait);
				        break;
		        }
	        }
        }

	    public void AddRange(IEnumerable<AbcFile> files)
        {
            foreach (var abc in files)
                Add(abc);
        }
    }
}