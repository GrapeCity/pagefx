using System;

namespace DataDynamics.PageFX.FLI.ABC
{
    internal enum KnownNamespace
    {
        Global,
        Internal,
        BodyTrait,
        AS3,
        MxInternal,
        PfxPackage,
        PfxPublic,
    }

    internal class QName
    {
	    private readonly KnownNamespace _knownNamespace;
	    private readonly string _name;
	    private readonly string _namespace;

		public QName(string name)
		{
			if (name == null) throw new ArgumentNullException("name");

			_name = name;
		}

        public QName(string name, KnownNamespace ns) : this(name)
        {
	        _knownNamespace = ns;
        }

		public QName(string name, string ns) : this(name)
		{
			_namespace = ns;
		}

	    public AbcMultiname Define(AbcFile abc)
        {
			var ns = string.IsNullOrEmpty(_namespace) ? abc.DefineNamespace(_knownNamespace) : abc.DefinePublicNamespace(_namespace);
            return abc.DefineQName(ns, _name);
        }
    }

    internal sealed class PfxQName : QName
    {
        public PfxQName(string name) : base(name, KnownNamespace.PfxPackage)
        {
        }
    }

    internal sealed class AS3QName : QName
    {
        public AS3QName(string name) : base(name, KnownNamespace.AS3)
        {
        }
    }
}