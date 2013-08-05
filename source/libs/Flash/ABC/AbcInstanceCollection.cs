using System;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Flash.Swf;

namespace DataDynamics.PageFX.Flash.Abc
{
	public sealed class AbcInstanceCollection : AbcAtomList<AbcInstance>
	{
		private readonly AbcFile _abc;
		private readonly Dictionary<string,AbcInstance> _cache = new Dictionary<string, AbcInstance>();

		public AbcInstanceCollection(AbcFile abc)
		{
			if (abc == null)
				throw new ArgumentNullException("abc");

			_abc = abc;
		}

		protected override void OnAdded(AbcInstance item)
		{
			item.Abc = _abc;
			_cache.Add(item.FullName, item);
		}

		public override void Clear()
		{
			base.Clear();

			_cache.Clear();
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

		public AbcInstance FindStrict(AbcMultiname name)
		{
			return this.FirstOrDefault(i => ReferenceEquals(i.Name, name));
		}

		public AbcInstance this[AbcMultiname name]
		{
			get { return Find(name); }
		}

		public AbcInstance this[string fullname]
		{
			get { return Find(fullname); }
		}

		public bool Contains(AbcMultiname name)
		{
			return Find(name) != null;
		}

		protected override bool DumpDisabled
		{
			get { return !AbcDumpService.DumpInstances; }
		}

		protected override string DumpElementName
		{
			get { return "instances"; }
		}

		public void DumpDirectory(string dir)
		{
			foreach (var i in this)
				i.DumpDirectory(dir);
		}

		protected override void WriteCount(SwfWriter writer)
		{
		}
	}
}