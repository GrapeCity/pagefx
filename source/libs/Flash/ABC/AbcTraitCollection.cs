using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using DataDynamics.PageFX.Common.Collections;
using DataDynamics.PageFX.Flash.Swf;

namespace DataDynamics.PageFX.Flash.Abc
{
    /// <summary>
    /// Represents collection of <see cref="AbcTrait"/>s.
    /// </summary>
    public sealed class AbcTraitCollection : IReadOnlyList<AbcTrait>, ISwfAtom, ISupportXmlDump
    {
        private readonly List<AbcTrait> _list = new List<AbcTrait>();
		private readonly Hashtable _cache = new Hashtable();
		private readonly IAbcTraitProvider _owner;

        public AbcTraitCollection(IAbcTraitProvider owner)
        {
	        if (owner == null)
				throw new ArgumentNullException("owner");

	        _owner = owner;
        }

	    public int Count
        {
            get { return _list.Count; }   
        }

        public AbcTrait this[int index]
        {
            get { return _list[index]; }
        }

	    public AbcTrait Find(AbcMultiname name, AbcTraitKind kind)
        {
            string key = AbcTrait.MakeKey(kind, name);
            return _cache[key] as AbcTrait;
        }

        public AbcTrait FindMethod(AbcMultiname name)
        {
            return Find(name, AbcTraitKind.Method);
        }

        public AbcTrait FindSlot(AbcMultiname name)
        {
            return Find(name, AbcTraitKind.Slot);
        }

        public AbcTrait Find(string name, AbcTraitKind kind)
        {
			return _list.FirstOrDefault(t => t.Kind == kind && t.NameString == name);
        }

        public AbcTrait FindMethod(string name)
        {
            return Find(name, AbcTraitKind.Method);
        }

        public AbcTrait Find(string name)
        {
			return _list.FirstOrDefault(t => t.NameString == name);
        }

        public bool Contains(AbcMultiname name, AbcTraitKind kind)
        {
            return Find(name, kind) != null;
        }

	    public void Add(AbcTrait trait)
        {
#if DEBUG
            //var t = Find(trait.Name, trait.Kind);
            var t = _cache[trait.Key] as AbcTrait;
            if (t != null)
            {
                throw new InvalidOperationException(string.Format("Trait with name {0} is already in collection", trait.Name));
            }
            trait.Verify();
#endif
            AddInternal(trait);
        }

        internal void AddInternal(AbcTrait trait)
        {
            int index = Count;
            trait.Index = index;
            trait.Owner = _owner;
            _list.Add(trait);
            _cache[trait.Key] = trait;
        }

        public void AddClass(AbcClass klass)
        {
            Add(AbcTrait.CreateClass(klass));
        }

	    public void Read(SwfReader reader)
        {
            int n = (int)reader.ReadUIntEncoded();
            for (int i = 0; i < n; ++i)
            {
	            var trait = new AbcTrait();
				trait.Read(reader);
	            AddInternal(trait);
            }
        }

        public void Write(SwfWriter writer)
        {
            int n = Count;
            writer.WriteUIntEncoded((uint)n);
            for (int i = 0; i < n; ++i)
                this[i].Write(writer);
        }

	    public void DumpXml(XmlWriter writer)
        {
            if (!AbcDumpService.DumpTraits && !(_owner is AbcScript))
                return;

            int n = Count;
            if (n > 0)
            {
                writer.WriteStartElement("traits");
                writer.WriteAttributeString("count", n.ToString());
                foreach (var t in this)
                    t.DumpXml(writer);
                writer.WriteEndElement();
            }
        }

	    public AbcTrait[] GetRange(AbcTraitKind kind)
        {
        	return this.Where(trait => trait.Kind == kind).ToArray();
        }

        public AbcTrait[] GetFields()
        {
            var list = this.Where(trait => trait.Kind == AbcTraitKind.Slot || trait.Kind == AbcTraitKind.Const).ToList();
        	list.Sort((x, y) => (int)x.Visibility - (int)y.Visibility);
            return list.ToArray();
        }

        public AbcMethod[] GetMethods()
        {
            var list = (from trait in this
						where trait.Kind == AbcTraitKind.Method
						select trait.Method).ToList();
        	list.Sort((x, y) => (int)x.Visibility - (int)y.Visibility);
            return list.ToArray();
        }

        public AbcPropertyCollection GetProperties()
        {
            var list = new AbcPropertyCollection();
            foreach (var trait in this)
            {
                bool isGetter = trait.Kind == AbcTraitKind.Getter;
                if (isGetter || trait.Kind == AbcTraitKind.Setter)
                {
                    var p = list[trait.Name];
                    if (p == null)
                    {
                        p = new AbcProperty();
                        list.Add(p);
                    }
                    if (isGetter)
                        p.Getter = trait.Method;
                    else
                        p.Setter = trait.Method;
                }
            }
            return list;
        }

	    public IEnumerator<AbcTrait> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public AbcTrait AddSlot(AbcMultiname type, AbcMultiname name)
        {
            var t = AbcTrait.CreateSlot(type, name);
            Add(t);
            return t;
        }

        public AbcTrait AddConst(AbcMultiname type, AbcMultiname name, object value)
        {
            var t = AbcTrait.CreateConst(type, name, value);
            Add(t);
            return t;
        }
    }
}