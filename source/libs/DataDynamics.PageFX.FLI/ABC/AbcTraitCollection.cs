using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using DataDynamics.PageFX.FLI.SWF;

namespace DataDynamics.PageFX.FLI.ABC
{
    /// <summary>
    /// Represents collection of <see cref="AbcTrait"/>s.
    /// </summary>
    public class AbcTraitCollection : IEnumerable<AbcTrait>, ISwfAtom, ISupportXmlDump
    {
        readonly List<AbcTrait> _list = new List<AbcTrait>();
        readonly Hashtable _cache = new Hashtable();
        readonly IAbcTraitProvider _owner;

        public AbcTraitCollection(IAbcTraitProvider owner)
        {
            _owner = owner;
        }

        #region Public Members
        public int Count
        {
            get { return _list.Count; }   
        }

        public AbcTrait this[int index]
        {
            get { return _list[index]; }
        }

        public int SlotCount
        {
            get { return this.Count(trait => trait.Kind == AbcTraitKind.Slot); }
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
            return _list.FirstOrDefault(trait => trait.Kind == kind && trait.NameString == name);
        }

        public AbcTrait FindMethod(string name)
        {
            return Find(name, AbcTraitKind.Method);
        }

        public AbcTrait Find(string name)
        {
            return _list.FirstOrDefault(trait => trait.NameString == name);
        }

        public bool Contains(AbcMultiname name, AbcTraitKind kind)
        {
            return Find(name, kind) != null;
        }

        public bool Contains(string name)
        {
            return Find(name) != null;
        }

        public void Add(AbcTrait trait)
        {
#if DEBUG
            //var t = Find(trait.Name, trait.Kind);
            var t = _cache[trait.Key] as AbcTrait;
            if (t != null)
            {
                throw new InvalidOperationException(
                    string.Format("Trait with name {0} is already in collection", trait.Name));
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
        #endregion

        #region ISwfAtom Members
        int _begin;
        int _end;

        public void Read(SwfReader reader)
        {
            _begin = (int)reader.Position;
            int n = (int)reader.ReadUIntEncoded();
            for (int i = 0; i < n; ++i)
            {
                AddInternal(new AbcTrait(reader));
            }
            _end = (int)reader.Position;
        }

        public void Write(SwfWriter writer)
        {
            int n = Count;
            writer.WriteUIntEncoded((uint)n);
            for (int i = 0; i < n; ++i)
                this[i].Write(writer);
        }

        public string FormatOffset(AbcFile file, int offset, string prefix)
        {
            return AbcHelper.FormatOffset(file, offset, new List<AbcTrait>(this),
                                          _begin, _end, prefix + " Traits", false, true);
        }
        #endregion

        #region Xml Dump
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
        #endregion

        #region Utils
        public AbcTrait[] GetRange(AbcTraitKind kind)
        {
            var list = new List<AbcTrait>();
            foreach (var trait in this)
            {
                if (trait.Kind == kind)
                    list.Add(trait);
            }
            return list.ToArray();
        }

        public AbcTrait[] GetFields()
        {
            var list = new List<AbcTrait>();
            foreach (var trait in this)
            {
                if (trait.Kind == AbcTraitKind.Slot || trait.Kind == AbcTraitKind.Const)
                    list.Add(trait);
            }
            list.Sort((x, y) => (int)x.Visibility - (int)y.Visibility);
            return list.ToArray();
        }

        public AbcMethod[] GetMethods()
        {
            var list = new List<AbcMethod>();
            foreach (var trait in this)
            {
                if (trait.Kind == AbcTraitKind.Method)
                    list.Add(trait.Method);
            }
            list.Sort((x, y) => (int)x.Visibility - (int)y.Visibility);
            return list.ToArray();
        }

        public PropertyCollection GetProperties()
        {
            var list = new PropertyCollection();
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
        #endregion

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

    public interface IAbcTraitProvider
    {
        AbcTraitCollection Traits { get; }
    }
}