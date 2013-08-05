using System;
using System.Collections.Generic;
using DataDynamics.PageFX.FlashLand.Swf;

namespace DataDynamics.PageFX.FlashLand.Abc
{
    /// <summary>
    /// Represents collection of <see cref="AbcMultiname"/> objects.
    /// </summary>
    public sealed class AbcMultinamePool : AbcConstList<AbcMultiname>
    {
        private readonly AbcFile _abc;
        private AbcMultiname RTQNameL;
        private AbcMultiname RTQNameLA;

	    public AbcMultinamePool(AbcFile abc)
        {
            _abc = abc;
            var mn = new AbcMultiname {Key = "*"};
            Add(mn);
        }

	    public AbcMultiname Define(AbcConstKind kind)
        {
            switch (kind)
            {
                case AbcConstKind.RTQNameL:
                    if (RTQNameL != null)
                        return RTQNameL;
                    break;

                case AbcConstKind.RTQNameLA:
                    if (RTQNameLA != null)
						return RTQNameLA;
                    break;

                default:
                    throw new InvalidOperationException();
            }
            var mn = new AbcMultiname(kind);
            Add(mn);
            return mn;
        }

		protected override void OnAdded(AbcMultiname item)
		{
			item.ABC = _abc;
			
			switch (item.Kind)
			{
				case AbcConstKind.RTQNameL:
					RTQNameL = item;
					break;

				case AbcConstKind.RTQNameLA:
					RTQNameLA = item;
					break;
			}
		}

	    public override void Read(SwfReader reader)
        {
            int n = (int)reader.ReadUIntEncoded();
            reader.MultinameCount = n;

            List<AbcMultiname> ptypes = null;
            for (int i = 1; i < n; ++i)
            {
                var item = new AbcMultiname();
				item.Read(reader);
                Add(item);

                if (item.IsParameterizedType)
                {
                    if (ptypes == null)
                        ptypes = new List<AbcMultiname>();
                    ptypes.Add(item);
                }
            }

            if (ptypes != null)
            {
                foreach (var item in ptypes)
                {
                    int i = item.TypeParameter.Index;
                    if (i == 0 || i >= n)
                        throw new BadImageFormatException("Bad index in multiname cpool");

                    item.TypeParameter = this[i];

					// reset key to reevaluate it
	                item.Key = null;
					UpdateIndex(item);
                }
            }
        }

		protected override string DumpElementName
		{
			get { return "multinames"; }
		}

        /// <summary>
        /// Imports given name.
        /// </summary>
        /// <param name="item">The name to import.</param>
        /// <returns>The same or imported name.</returns>
        public override AbcMultiname Import(AbcMultiname item)
        {
            if (item == null) return null;
            if (IsDefined(item)) return item;
            if (item.IsAny) return this[0];

            item = ImportCore(item);

            return item;
        }

	    private AbcMultiname ImportCore(AbcMultiname item)
        {
			var kind = item.Kind;
		    switch (kind)
		    {
				    //no data
			    case AbcConstKind.RTQNameL:
			    case AbcConstKind.RTQNameLA:
				    return Define(kind);
		    }

		    string key = item.Key;
			var curItem = this[key];
		    if (curItem != null)
		    {
			    return curItem;
		    }

		    switch (kind)
		    {
				    //U30 ns_index
				    //U30 name_index
			    case AbcConstKind.QName:
			    case AbcConstKind.QNameA:
				    {
					    var name = _abc.ImportConst(item.Name);
					    var ns = _abc.ImportConst(item.Namespace);
					    var newItem = new AbcMultiname(kind, ns, name) {Key = key};
					    Add(newItem);
					    return newItem;
				    }

				    //U30 name_index
			    case AbcConstKind.RTQName:
			    case AbcConstKind.RTQNameA:
				    {
					    var name = _abc.ImportConst(item.Name);
					    var newItem = new AbcMultiname(kind, name) {Key = key};
					    Add(newItem);
					    return newItem;
				    }

				    //U30 name_index
				    //U30 ns_set_index
			    case AbcConstKind.Multiname:
			    case AbcConstKind.MultinameA:
				    {
					    if (item.NamespaceSet.Count == 1)
					    {
						    return ImportQName(item.NamespaceSet[0], item.Name, kind == AbcConstKind.MultinameA);
					    }

					    var name = _abc.ImportConst(item.Name);
					    var nss = _abc.ImportConst(item.NamespaceSet);
					    var newItem = new AbcMultiname(kind, nss, name) {Key = key};
					    Add(newItem);
					    return newItem;
				    }

				    //U30 ns_set_index
			    case AbcConstKind.MultinameL:
			    case AbcConstKind.MultinameLA:
				    {
					    var nss = _abc.ImportConst(item.NamespaceSet);
					    var newItem = new AbcMultiname(kind, nss) {Key = key};
					    Add(newItem);
					    return newItem;
				    }

			    case AbcConstKind.TypeName:
				    {
					    var type = Import(item.Type);
					    var param = Import(item.TypeParameter);

					    var newItem = new AbcMultiname(type, param) {Key = key};
					    Add(newItem);
					    return newItem;
				    }

			    default:
				    return item;
		    }
        }

	    private AbcMultiname ImportQName(AbcNamespace ns, AbcConst<string> name, bool isAttribute)
	    {
		    var kind = isAttribute ? AbcConstKind.QNameA : AbcConstKind.QName;
		    string key = AbcMultiname.KeyOf(kind, ns, name);

		    var mn = this[key];
		    if (mn != null) return mn;

		    name = _abc.ImportConst(name);
		    ns = _abc.ImportConst(ns);

		    mn = new AbcMultiname(kind, ns, name) {Key = key};

		    Add(mn);
		    return mn;
	    }
    }
}