using System.Xml;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.Swf;

namespace DataDynamics.PageFX.FlashLand.Abc
{
    /// <summary>
    /// Contains traits for static members of user defined type
    /// </summary>
    public sealed class AbcClass : ISupportXmlDump, ISwfIndexedAtom, IAbcTraitProvider
    {
	    public AbcClass()
        {
            _traits = new AbcTraitCollection(this);
        }

	    public AbcClass(AbcInstance instance) : this()
        {
            Instance = instance;
        }

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
        private AbcMethod _initializer;

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
        private AbcInstance _instance;

        public IType Type
        {
            get { return Instance.Type; }
        }

        public AbcFile Abc
        {
            get { return _instance != null ? _instance.Abc : null; }
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
        private readonly AbcTraitCollection _traits;

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
        private AbcTrait _trait;

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

	    public override string ToString()
        {
            return Name;
        }
    }
}