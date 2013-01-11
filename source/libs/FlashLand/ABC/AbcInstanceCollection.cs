using System;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.Extensions;
using DataDynamics.PageFX.FlashLand.Swf;

namespace DataDynamics.PageFX.FlashLand.Abc
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

		private AbcInstance Find(string fullname)
		{
			AbcInstance instance;
			return _cache.TryGetValue(fullname, out instance) ? instance : null;
		}

		public AbcInstance Find(AbcMultiname mname)
		{
			if (mname == null)
				throw new ArgumentNullException("mname");
			if (mname.IsRuntime)
				return null;

			string name = mname.NameString;
			if (string.IsNullOrEmpty(name))
				return null;

			if (mname.NamespaceSet != null)
			{
				return mname.NamespaceSet
				            .Select(ns => ns.NameString.MakeFullName(name))
				            .Select(fullname => Find(fullname))
				            .FirstOrDefault(instance => instance != null);
			}

			return Find(mname.FullName);
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