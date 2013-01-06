using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using DataDynamics.PageFX.Common.Collections;
using DataDynamics.PageFX.Common.Extensions;
using DataDynamics.PageFX.FlashLand.Swf;

namespace DataDynamics.PageFX.FlashLand.Abc
{
	public sealed class AbcInstanceCollection : IReadOnlyList<AbcInstance>, ISupportXmlDump
	{
		private readonly AbcFile _abc;
		private readonly Hashtable _cache = new Hashtable();
		private readonly List<AbcInstance> _list = new List<AbcInstance>();

		public AbcInstanceCollection(AbcFile abc)
		{
			if (abc == null)
				throw new ArgumentNullException("abc");

			_abc = abc;
		}

		public int Count
		{
			get { return _list.Count; }
		}

		public AbcInstance this[int index]
		{
			get { return _list[index]; }
		}

		public void Add(AbcInstance instance)
		{
			if (instance == null)
				throw new ArgumentNullException("instance");

			instance.Index = Count;
			instance.Abc = _abc;

			_cache[instance.FullName] = instance;
			_list.Add(instance);
		}

		public void Clear()
		{
			_list.Clear();
			_cache.Clear();
		}

		public bool IsDefined(AbcInstance instance)
		{
			if (instance == null) return false;
			int index = instance.Index;
			if (index < 0 || index >= Count)
				return false;
			return this[index] == instance;
		}

		private AbcInstance Find(string fullname)
		{
			return _cache[fullname] as AbcInstance;
		}

		public AbcInstance Find(AbcMultiname mname)
		{
			if (mname == null)
				throw new ArgumentNullException("mname");
			if (mname.IsRuntime) return null;
			string name = mname.NameString;
			if (string.IsNullOrEmpty(name))
				return null;
			if (mname.NamespaceSet != null)
			{
				return mname.NamespaceSet
				            .Select(ns => NameExtensions.MakeFullName(ns.NameString, name))
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

		public void Read(int n, SwfReader reader)
		{
			for (int i = 0; i < n; ++i)
			{
				var instance = new AbcInstance();
				instance.Read(reader);
				Add(instance);
			}
		}

		public void Write(SwfWriter writer)
		{
			int n = Count;
			for (int i = 0; i < n; ++i)
				this[i].Write(writer);
		}

		public void DumpXml(XmlWriter writer)
		{
			if (!AbcDumpService.DumpInstances) return;
			writer.WriteStartElement("instances");
			writer.WriteAttributeString("count", XmlConvert.ToString(Count));
			foreach (var i in this)
				i.DumpXml(writer);
			writer.WriteEndElement();
		}

		public void DumpDirectory(string dir)
		{
			foreach (var i in this)
				i.DumpDirectory(dir);
		}

		public IEnumerator<AbcInstance> GetEnumerator()
		{
			return _list.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}