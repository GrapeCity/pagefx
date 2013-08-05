using System;
using DataDynamics.PageFX.Common.Extensions;

namespace DataDynamics.PageFX.FlashLand.Abc
{
	public sealed class AbcNamespacePool : AbcConstList<AbcNamespace>
	{
		private readonly AbcFile _abc;
		
		public AbcNamespacePool(AbcFile abc)
		{
			if (abc == null)
				throw new ArgumentNullException("abc");

			_abc = abc;

			var ns = new AbcNamespace {Key = "*"};
			Add(ns);
		}

		protected override string DumpElementName
		{
			get { return "namespaces"; }
		}

		public AbcNamespace this[AbcConst<string> name, AbcConstKind kind]
		{
			get
			{
				string key = name.MakeKey(kind);
				return this[key];
			}
		}

		public override string ToString()
		{
			return this.Join<AbcNamespace>("\n");
		}

		/// <summary>
		/// Imports given namespace.
		/// </summary>
		/// <param name="ns">namespace to import.</param>
		/// <returns>imported namespace.</returns>
		public override AbcNamespace Import(AbcNamespace ns)
		{
			if (ns == null) return null;
			if (ns.IsAny) return this[0];
			if (IsDefined(ns)) return ns;

			var name = ns.Name;
			var kind = ns.Kind;
			string key = name.MakeKey(kind);
			var ns2 = this[key];
			if (ns2 != null) return ns2;

			name = _abc.ImportConst(name);
			ns = new AbcNamespace(name, kind, key);
			Add(ns);
			return ns;
		}
	}
}