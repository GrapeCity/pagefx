using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using DataDynamics.PageFX.FLI.SWF;

namespace DataDynamics.PageFX.FLI.ABC
{
    /// <summary>
    /// Represents collection of <see cref="AbcMultiname"/> objects.
    /// </summary>
    public class AbcMultinamePool : ISwfAtom, ISupportXmlDump, IAbcConstPool, IEnumerable<AbcMultiname>
    {
        readonly AbcFile _abc;
        readonly AbcConstList<AbcMultiname> _list = new AbcConstList<AbcMultiname>();
        AbcMultiname RTQNameL;
        AbcMultiname RTQNameLA;

        #region ctor
        public AbcMultinamePool(AbcFile abc)
        {
            _abc = abc;
            var mn = new AbcMultiname {key = "*"};
            Add(mn);
        }
        #endregion

        #region Public Members
        public int Count
        {
            get { return _list.Count; }
        }

        public AbcMultiname this[int index]
        {
            get { return _list[index]; }
        }

        public AbcMultiname this[string key]
        {
            get { return _list[key]; }
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

        public void Add(AbcMultiname mn)
        {
            mn.ABC = _abc;
            _list.Add(mn);

            switch (mn.Kind)
            {
                case AbcConstKind.RTQNameL:
                    RTQNameL = mn;
                    break;

                case AbcConstKind.RTQNameLA:
                    RTQNameLA = mn;
                    break;
            }

            //if (mn.Index != 0)
            //    mn.Check();
        }
        #endregion

        #region IO

        public void Read(SwfReader reader)
        {
            int n = (int)reader.ReadUIntEncoded();
            reader.MultinameCount = n;
            List<AbcMultiname> ptypes = null;
            for (int i = 1; i < n; ++i)
            {
                var mn = new AbcMultiname(reader);
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
                        throw new BadFormatException("Bad index in multiname cpool");
                    mn.TypeParameter = this[i];
                }
            }
        }

        public void Write(SwfWriter writer)
        {
            int n = Count;
            if (n <= 1)
            {
                writer.WriteUInt8(0);
            }
            else
            {
                writer.WriteUIntEncoded((uint)n);
                for (int i = 1; i < n; ++i)
                {
                    var mn = this[i];
                    mn.Write(writer);
                }
            }
        }

    	#endregion

        #region Dump
        public void DumpXml(XmlWriter writer)
        {
            writer.WriteStartElement("multinames");
            writer.WriteAttributeString("count", Count.ToString());
            for (int i = 0; i < Count; ++i)
                this[i].DumpXml(writer, "item");
            writer.WriteEndElement();
        }
        #endregion

        #region IAbcConstPool Members
        IAbcConst IAbcConstPool.this[int index]
        {
            get { return _list[index]; }
        }

        /// <summary>
        /// Determines whether given constant is defined in this pool.
        /// </summary>
        /// <param name="c">constant to check.</param>
        /// <returns>true if defined; otherwise, false</returns>
        public bool IsDefined(IAbcConst c)
        {
            return _list.IsDefined((AbcMultiname)c);
        }

#if PERF
        public static int CallCount;
        public static int Time;
#endif

        /// <summary>
        /// Imports given name.
        /// </summary>
        /// <param name="mname">name to import.</param>
        /// <returns>imported name.</returns>
        public AbcMultiname Import(AbcMultiname mname)
        {
            if (mname == null) return null;
            if (IsDefined(mname)) return mname;
            if (mname.IsAny) return this[0];

#if PERF
            CallCount++;
            int start = Environment.TickCount;
#endif

            mname = ImportCore(mname);

#if PERF
            Time += Environment.TickCount - start;
#endif

            return mname;
        }

        AbcMultiname ImportQName(bool attr, AbcNamespace ns, AbcConst<string> name)
        {
            var kind = attr ? AbcConstKind.QNameA : AbcConstKind.QName;
            string key = AbcMultiname.KeyOf(kind, ns, name);

            var mn = _list[key];
            if (mn != null) return mn;

            name = _abc.ImportConst(name);
            ns = _abc.ImportConst(ns);

            mn = new AbcMultiname(kind, ns, name) {key = key};

            Add(mn);
            return mn;
        }

        AbcMultiname ImportCore(AbcMultiname mname)
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
                        var mn = _list[key];
                        if (mn != null) return mn;

                        var name = _abc.ImportConst(mname.Name);
                        var ns = _abc.ImportConst(mname.Namespace);
                        mname = new AbcMultiname(kind, ns, name) {key = key};
                        Add(mname);
                        return mname;
                    }
                    
                    //U30 name_index
                case AbcConstKind.RTQName:
                case AbcConstKind.RTQNameA:
                    {
                        string key = mname.Key;
                        var mn = _list[key];
                        if (mn != null) return mn;

                        var name = _abc.ImportConst(mname.Name);
                        mname = new AbcMultiname(kind, name) {key = key};
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
                        var mn = _list[key];
                        if (mn != null) return mn;

                        var name = _abc.ImportConst(mname.Name);
                        var nss = _abc.ImportConst(mname.NamespaceSet);
                        mname = new AbcMultiname(kind, nss, name) {key = key};
                        Add(mname);
                        return mname;
                    }

                    //U30 ns_set_index
                case AbcConstKind.MultinameL:
                case AbcConstKind.MultinameLA:
                    {
                        string key = mname.Key;
                        var mn = _list[key];
                        if (mn != null) return mn;
                        var nss = _abc.ImportConst(mname.NamespaceSet);
                        mname = new AbcMultiname(kind, nss) {key = key};
                        Add(mname);
                        return mname;
                    }

                case AbcConstKind.TypeName:
                    {
                        string key = mname.Key;
                        var mn = _list[key];
                        if (mn != null) return mn;
                        var type = Import(mname.Type);
                        var param = Import(mname.TypeParameter);
                        mname = new AbcMultiname(type, param) {key = key};
                        Add(mname);
                        return mname;
                    }

                default:
                    return mname;
            }
        }

        /// <summary>
        /// Imports given constant.
        /// </summary>
        /// <param name="c">constant to import.</param>
        /// <returns>imported constant.</returns>
        public IAbcConst Import(IAbcConst c)
        {
            return Import((AbcMultiname)c);
        }
        #endregion

        #region IEnumerable Members

        public IEnumerator<AbcMultiname> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator<IAbcConst> IEnumerable<IAbcConst>.GetEnumerator()
        {
        	return _list.Cast<IAbcConst>().GetEnumerator();
        }

    	#endregion
    }
}