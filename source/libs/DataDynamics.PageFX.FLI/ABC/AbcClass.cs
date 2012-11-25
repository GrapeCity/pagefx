using System;
using System.Collections.Generic;
using System.Xml;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.FLI.SWF;

namespace DataDynamics.PageFX.FLI.ABC
{
    /// <summary>
    /// Contains traits for static members of user defined type
    /// </summary>
    public sealed class AbcClass : ISupportXmlDump, ISwfIndexedAtom, IAbcTraitProvider
    {
        #region Constructors
        public AbcClass()
        {
            _traits = new AbcTraitCollection(this);
        }

        public AbcClass(SwfReader reader) : this()
        {
            Read(reader);
        }

        public AbcClass(AbcInstance instance) : this()
        {
            Instance = instance;
        }
        #endregion

        #region Properties
        public int Index { get; set; }

        public AbcMethod Initializer
        {
            get { return _initializer; }
            set
            {
                _initializer = value;
                if (value != null)
                    value.IsInitializer = true;
            }
        }
        AbcMethod _initializer;

        public AbcInstance Instance
        {
            get { return _instance; }
            set
            {
                if (value != _instance)
                {
                    if (_instance != null)
                        _instance.Class = null;
                    _instance = value;
                    if (_instance != null)
                    {
                        _instance.Class = this;
                        if (_initializer != null)
                            _initializer.Instance = _instance;
                    }
                }
            }
        }
        AbcInstance _instance;

        public IType Type
        {
            get
            {
                return Instance.Type;
            }
        }

        public AbcFile ABC
        {
            get
            {
                if (_instance != null)
                    return _instance.Abc;
                return null;
            }
        }

        public string Name
        {
            get
            {
                string name = "Class" + Index;
                if (_instance != null && _instance.Name != null)
                    name += " = " + _instance.Name;
                return name;
            }
        }

        public AbcTraitCollection Traits
        {
            get { return _traits; }
        }
        readonly AbcTraitCollection _traits;

        public bool Initialized { get; set; }

        /// <summary>
        /// Gets or sets script trait assotiated with the class.
        /// </summary>
        public AbcTrait Trait
        {
            get { return _trait; }
            set
            {
                if (value != _trait)
                {
                    _trait = value;
                    if (value != null)
                        value.Class = this;
                }
            }
        }
        AbcTrait _trait;
        #endregion

        #region IAbcAtom Members
        public void Read(SwfReader reader)
        {
            Initializer = reader.ReadAbcMethod();
            _traits.Read(reader);
        }

        public void Write(SwfWriter writer)
        {
            writer.WriteUIntEncoded((uint)_initializer.Index);
            _traits.Write(writer);
        }
        #endregion

        #region Dump
        public void DumpXml(XmlWriter writer)
        {
            writer.WriteStartElement("class");
            writer.WriteAttributeString("index", Index.ToString());

            var instance = Instance;
            if (instance != null && instance.Name != null)
                writer.WriteAttributeString("name", instance.Name.FullName);

            writer.WriteElementString("name", Name);
            if (_initializer != null)
            {
                if (AbcDumpService.DumpInitializerCode)
                {
                    bool old = AbcDumpService.DumpCode;
                    AbcDumpService.DumpCode = true;
                    _initializer.DumpXml(writer, "cinit");
                    AbcDumpService.DumpCode = old;
                }
                else
                {
                    writer.WriteElementString("cinit", _initializer.ToString());
                }
            }
            _traits.DumpXml(writer);
            writer.WriteEndElement();
        }
        #endregion

        #region Object Override Members
        public override string ToString()
        {
            return Name;
        }
        #endregion
    }

    public sealed class AbcClassCollection : List<AbcClass>, ISupportXmlDump
    {
        #region Public Members
        public new void Add(AbcClass klass)
        {
#if DEBUG
            if (IsDefined(klass))
                throw new InvalidOperationException();
#endif
            klass.Index = Count;
            base.Add(klass);
        }

        internal void AddInternal(AbcClass klass)
        {
            base.Add(klass);
        }

        public bool IsDefined(AbcClass klass)
        {
            if (klass == null) return false;
            int index = klass.Index;
            if (index < 0 || index >= Count)
                return false;
            if (this[index] != klass)
                return false;
            return true;
        }
        #endregion

        #region IO

        public void Read(int n, SwfReader reader)
        {
            for (int i = 0; i < n; ++i)
            {
                Add(new AbcClass(reader));
            }
        }

        public void Write(SwfWriter writer)
        {
            int n = Count;
            for (int i = 0; i < n; ++i)
                this[i].Write(writer);
        }

    	#endregion

        #region Dump
        public void DumpXml(XmlWriter writer)
        {
            if (!AbcDumpService.DumpInstances) return;
            writer.WriteStartElement("classes");
            writer.WriteAttributeString("count", Count.ToString());
            foreach (var c in this)
                c.DumpXml(writer);
            writer.WriteEndElement();
        }
        #endregion
    }
}