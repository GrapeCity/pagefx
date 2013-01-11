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
                    if (RTQNameLA != null) return RTQNameLA;
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

			//if (item.Index != 0)
			//    item.Check();
		}

	    public override void Read(SwfReader reader)
        {
            int n = (int)reader.ReadUIntEncoded();
            reader.MultinameCount = n;
            List<AbcMultiname> ptypes = null;
            for (int i = 1; i < n; ++i)
            {
                var mn = new AbcMultiname();
				mn.Read(reader);
                Add(mn);
                if (mn.IsParameterizedType)
                {
                    if (ptypes == null)
                        ptypes = new List<AbcMultiname>();
                    ptypes.Add(mn);
                }
            }
            if (ptypes != null)
            {
                foreach (var mn in ptypes)
                {
                    int i = mn.TypeParameter.Index;
                    if (i == 0 || i >= n)
                        throw new BadImageFormatException("Bad index in multiname cpool");
                    mn.TypeParameter = this[i];
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
        /// <param name="mname">name to import.</param>
        /// <returns>imported name.</returns>
        public override AbcMultiname Import(AbcMultiname mname)
        {
            if (mname == null) return null;
            if (IsDefined(mname)) return mname;
            if (mname.IsAny) return this[0];

            mname = ImportCore(mname);

            return mname;
        }

        private AbcMultiname ImportQName(bool attr, AbcNamespace ns, AbcConst<string> name)
        {
            var kind = attr ? AbcConstKind.QNameA : AbcConstKind.QName;
            string key = AbcMultiname.KeyOf(kind, ns, name);

            var mn = this[key];
            if (mn != null) return mn;

            name = _abc.ImportConst(name);
            ns = _abc.ImportConst(ns);

            mn = new AbcMultiname(kind, ns, name) {Key = key};

            Add(mn);
            return mn;
        }

        private AbcMultiname ImportCore(AbcMultiname mname)
        {
            var kind = mname.Kind;
            switch (kind)
            {
                    //U30 ns_index
                    //U30 name_index
                case AbcConstKind.QName:
                case AbcConstKind.QNameA:
                    {
                        string key = mname.Key;
                        var mn = this[key];
                        if (mn != null) return mn;

                        var name = _abc.ImportConst(mname.Name);
                        var ns = _abc.ImportConst(mname.Namespace);
                        mname = new AbcMultiname(kind, ns, name) {Key = key};
                        Add(mname);
                        return mname;
                    }
                    
                    //U30 name_index
                case AbcConstKind.RTQName:
                case AbcConstKind.RTQNameA:
                    {
                        string key = mname.Key;
                        var mn = this[key];
                        if (mn != null) return mn;

                        var name = _abc.ImportConst(mname.Name);
                        mname = new AbcMultiname(kind, name) {Key = key};
                        Add(mname);
                        return mname;
                    }

                    //no data
                case AbcConstKind.RTQNameL:
                    {
                        if (RTQNameL != null)
                            return RTQNameL;
                        mname = new AbcMultiname(kind);
                        Add(mname);
                        return mname;
                    }

                case AbcConstKind.RTQNameLA:
                    {
                        if (RTQNameLA != null)
                            return RTQNameLA;
                        mname = new AbcMultiname(kind);
                        Add(mname);
                        return mname;
                    }

                    //U30 name_index
                    //U30 ns_set_index
                case AbcConstKind.Multiname:
                case AbcConstKind.MultinameA:
                    {
                        if (mname.NamespaceSet.Count == 1)
                        {
                            return ImportQName(kind == AbcConstKind.MultinameA,
                                               mname.NamespaceSet[0], mname.Name);
                        }

                        string key = mname.Key;
                        var mn = this[key];
                        if (mn != null) return mn;

                        var name = _abc.ImportConst(mname.Name);
                        var nss = _abc.ImportConst(mname.NamespaceSet);
                        mname = new AbcMultiname(kind, nss, name) {Key = key};
                        Add(mname);
                        return mname;
                    }

                    //U30 ns_set_index
                case AbcConstKind.MultinameL:
                case AbcConstKind.MultinameLA:
                    {
                        string key = mname.Key;
                        var mn = this[key];
                        if (mn != null) return mn;
                        var nss = _abc.ImportConst(mname.NamespaceSet);
                        mname = new AbcMultiname(kind, nss) {Key = key};
                        Add(mname);
                        return mname;
                    }

                case AbcConstKind.TypeName:
                    {
                        string key = mname.Key;
                        var mn = this[key];
                        if (mn != null) return mn;
                        var type = Import(mname.Type);
                        var param = Import(mname.TypeParameter);
                        mname = new AbcMultiname(type, param) {Key = key};
                        Add(mname);
                        return mname;
                    }

                default:
                    return mname;
            }
        }
    }
}