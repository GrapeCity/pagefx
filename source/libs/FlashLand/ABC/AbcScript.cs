using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;
using DataDynamics.PageFX.FlashLand.Swf;

namespace DataDynamics.PageFX.FlashLand.Abc
{
    public sealed class AbcScript : ISupportXmlDump, ISwfIndexedAtom, IAbcTraitProvider
    {
	    public AbcScript()
        {
            _traits = new AbcTraitCollection(this);
        }

	    public int Index { get; set; }

        public AbcFile Abc { get; internal set; }

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

        public AbcTraitCollection Traits
        {
            get { return _traits; }
        }
        readonly AbcTraitCollection _traits;

        internal bool IsMain { get; set; }

        internal AbcInstance SingleInstance
        {
            get
            {
                int n = _traits.Count;
                if (n != 1) return null;
                var t = _traits[0];
                var klass = t.Class;
                if (klass == null) return null;
                return klass.Instance;
            }
        }

	    public void DefineClassTraits(IEnumerable<AbcInstance> set, Predicate<AbcInstance> filter)
        {
            foreach (var instance in set)
            {
                //NOTE: Ignore instances initilized by other scripts
                if (instance.Script != null && instance.Script != this)
                    continue;

                var klass = instance.Class;

                if (filter != null && filter(instance))
                    continue;

                Traits.AddClass(klass);
            }
        }
        public void DefineClassTraits(params AbcInstance[] instances)
        {
            DefineClassTraits(instances, null);
        }

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

	    private bool ShouldDump()
        {
            if (!AbcDumpService.DumpScripts) return false;
            if (!AbcDumpService.DumpInstances) return true;
            foreach (var t in _traits)
            {
                switch (t.Kind)
                {
                    case AbcTraitKind.Class:
                        {
                            var klass = t.Class;
                            if (klass == null) return true;
                            var instance = klass.Instance;
                            if (instance == null) return true;
                            if (instance.IsDumped) return true;
                        }
                        break;

                    default:
                        return true;
                }
            }
            return false;
        }

        public void DumpXml(XmlWriter writer)
        {
            if (!ShouldDump()) return;

            writer.WriteStartElement("script");
            writer.WriteAttributeString("index", Index.ToString());
            if (_initializer != null)
            {
                if (AbcDumpService.DumpInitializerCode)
                {
                    bool old = AbcDumpService.DumpCode;
                    AbcDumpService.DumpCode = true;
                    _initializer.DumpXml(writer, "init");
                    AbcDumpService.DumpCode = old;
                }
                else
                {
                    writer.WriteElementString("init", _initializer.ToString());
                }
            }
            _traits.DumpXml(writer);
            writer.WriteEndElement();
        }

        public void Dump(TextWriter writer)
        {
            writer.WriteLine("#region Script{0}", Index);

            bool eol = false;
            if (_initializer != null)
            {
                _initializer.Dump(writer, "", true);
                eol = true;
            }

            foreach (var trait in _traits)
            {
                if (eol) writer.WriteLine();
                if (trait.IsMethod)
                {
                    var method = trait.Method;
                    Debug.Assert(method != null);
                    method.Dump(writer, "", true);
                }
                eol = true;
            }

            writer.WriteLine("#endregion");
        }

	    public override string ToString()
        {
            var def = SingleInstance;
            if (def != null)
                return def.FullName;
            return "Script" + Index;
        }
    }
}