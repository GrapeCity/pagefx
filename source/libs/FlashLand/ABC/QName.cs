using System;

namespace DataDynamics.PageFX.FlashLand.Abc
{
    internal enum KnownNamespace : byte
    {
        Global,
        Internal,
        BodyTrait,
        AS3,
        MxInternal,
        PfxPackage,
        PfxPublic,
    }

	internal enum NamespaceKind : byte
	{
		Package,
		Public,
		Protected,
		Private,
	}

	internal sealed class Namespace
	{
		public Namespace(string name, NamespaceKind kind)
		{
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");

			Name = name;
			Kind = kind;
		}

		public string Name { get; private set; }

		public NamespaceKind Kind { get; private set; }

		public AbcNamespace Define(AbcFile abc)
		{
			return abc.DefineNamespace(ToAbc(Kind), Name);
		}

		private static AbcConstKind ToAbc(NamespaceKind value)
		{
			switch (value)
			{
				case NamespaceKind.Package:
					return AbcConstKind.PackageNamespace;
				case NamespaceKind.Public:
					return AbcConstKind.PublicNamespace;
				case NamespaceKind.Protected:
					return AbcConstKind.ProtectedNamespace;
				case NamespaceKind.Private:
					return AbcConstKind.PrivateNamespace;
				default:
					throw new ArgumentOutOfRangeException("value");
			}
		}
	}

    internal class QName
    {
	    private readonly KnownNamespace _knownNamespace;
	    private readonly Namespace _namespace;

		public static QName PtrValue = new QName("value", new Namespace("$ptr", NamespaceKind.Package));

		public QName(string name)
		{
			if (name == null) throw new ArgumentNullException("name");

			Name = name;
		}

        public QName(string name, KnownNamespace ns) : this(name)
        {
	        _knownNamespace = ns;
        }

		public QName(string name, string ns) : this(name)
		{
			_namespace = new Namespace(ns, NamespaceKind.Public);
		}

	    public QName(string name, Namespace ns) : this(name)
	    {
		    if (ns == null) throw new ArgumentNullException("ns");

		    _namespace = ns;
	    }

	    public string Name { get; private set; }

	    public AbcMultiname Define(AbcFile abc)
        {
			var ns = _namespace != null ? _namespace.Define(abc) : abc.DefineNamespace(_knownNamespace);
            return abc.DefineQName(ns, Name);
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