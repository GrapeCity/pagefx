using System;

namespace DataDynamics.PageFX.FlashLand.Abc
{
	public sealed class AbcNssetPool : AbcConstList<AbcNamespaceSet>
	{
		private readonly AbcFile _abc;

		public AbcNssetPool(AbcFile abc)
		{
			if (abc == null)
				throw new ArgumentNullException("abc");

			_abc = abc;

			Add(new AbcNamespaceSet {Key = "*"});
		}

		protected override string DumpElementName
		{
			get { return "ns-set-pool"; }
		}
		
		/// <summary>
		/// Imports given namespace set.
		/// </summary>
		/// <param name="nss">namespace set to import.</param>
		/// <returns>imported namespace set.</returns>
		public override AbcNamespaceSet Import(AbcNamespaceSet nss)
		{
			if (nss == null) return null;
			if (IsDefined(nss)) return nss;

			string key = nss.Key;
			var set = this[key];
			if (set != null) return set;

			int n = nss.Count;
			var namespaces = new AbcNamespace[n];
			for (int i = 0; i < n; ++i)
				namespaces[i] = _abc.ImportConst(nss[i]);

			set = new AbcNamespaceSet(namespaces, key);
			Add(set);

			return set;
		}
	}
}