using System;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.FlashLand.Abc
{
    internal enum NamespaceKind : byte
	{
		Package,
		Public,
		Protected,
		Private,
		Internal,
		Explicit,
		StaticProtected
	}

	internal sealed class Namespace
	{
		public Namespace(string name, NamespaceKind kind)
		{
			Name = name ?? "";
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
				case NamespaceKind.Internal:
					return AbcConstKind.InternalNamespace;
				case NamespaceKind.Explicit:
					return AbcConstKind.ExplicitNamespace;
				case NamespaceKind.StaticProtected:
					return AbcConstKind.StaticProtectedNamespace;
				default:
					throw new ArgumentOutOfRangeException("value");
			}
		}
	}

    internal class QName
    {
		public static QName Global(string name)
		{
			return new QName(name, KnownNamespace.Global);
		}

		public static QName AS3(string name)
		{
			return new QName(name, KnownNamespace.AS3);
		}

		public static QName MxInternal(string name)
		{
			return new QName(name, KnownNamespace.MxInternal);
		}

	    public static QName PfxPackage(string name)
	    {
		    return new QName(name, KnownNamespace.PfxPackage);
	    }

		public static QName PfxPublic(string name)
		{
			return new QName(name, KnownNamespace.PfxPublic);
		}

		public static QName BodyTrait(string name)
		{
			return new QName(name, KnownNamespace.BodyTrait);
		}

		public static QName Package(string ns, string name)
		{
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");
			if (string.IsNullOrEmpty(ns))
				return Global(name);
			return new QName(name, new Namespace(ns, NamespaceKind.Package));
		}

		public static QName Public(string ns, string name)
		{
			if (ns == null)
				throw new ArgumentNullException("ns");
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");
			return new QName(name, new Namespace(ns, NamespaceKind.Public));
		}

		public static QName FromAttribute(ICustomAttribute attribute)
		{
			int n = attribute.Arguments.Count;
			if (n == 0)
				throw new InvalidOperationException("Invalid qname attribute");

			var name = (string)attribute.Arguments[0].Value;
			string ns = "";
			var nskind = NamespaceKind.Package;
			if (n > 1)
			{
				ns = (string)attribute.Arguments[1].Value;
				string kind = (string)attribute.Arguments[2].Value;
				nskind = ParseNsKind(kind);
			}

			return new QName(name, new Namespace(ns, nskind));
		}

		private static NamespaceKind ParseNsKind(string kind)
		{
			switch (kind)
			{
				case "private":
					return NamespaceKind.Private;
				case "public":
					return NamespaceKind.Public;
				case "package":
					return NamespaceKind.Package;
				case "internal":
					return NamespaceKind.Internal;
				case "protected":
					return NamespaceKind.Protected;
				case "explicit":
					return NamespaceKind.Explicit;
				case "static protected":
					return NamespaceKind.StaticProtected;
				default:
					throw new ArgumentOutOfRangeException("kind");
			}
		}

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

	    public string FullName
	    {
		    get
		    {
			    var ns = Ns;
			    return string.IsNullOrEmpty(ns) ? Name : ns + "." + Name;
		    }
	    }

	    public AbcMultiname Define(AbcFile abc)
        {
			var ns = _namespace != null ? _namespace.Define(abc) : abc.KnownNamespaces.Get(_knownNamespace);
            return abc.DefineQName(ns, Name);
        }

	    private string Ns
	    {
		    get { return _namespace != null ? _namespace.Name : KnownNamespaces.GetName(_knownNamespace); }
	    }
    }

	internal sealed class QNameAttribute : Attribute
	{
		public QNameAttribute(string name) : this(KnownNamespace.Global, name)
		{
		}

		public QNameAttribute(KnownNamespace ns, string name)
		{
			Value = new QName(name, ns);
		}

		public QNameAttribute(string ns, string name)
		{
			Value = QName.Package(ns, name);
		}

		public QNameAttribute(string ns, string name, NamespaceKind kind)
		{
			Value = new QName(name, new Namespace(ns, kind));
		}

		public QName Value { get; private set; }
	}
}