using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using DataDynamics.PageFX.Common.Collections;
using DataDynamics.PageFX.Common.Extensions;
using DataDynamics.PageFX.FlashLand.Swf;

namespace DataDynamics.PageFX.FlashLand.Abc
{
    public sealed class AbcConstPool<T> : IReadOnlyList<AbcConst<T>>, ISwfAtom, ISupportXmlDump, IAbcConstPool
    {
    	private readonly List<AbcConst<T>> _list = new List<AbcConst<T>>();
		private readonly Dictionary<T, int> _index = new Dictionary<T, int>();

    	public AbcConstPool()
        {
            _list.Add(new AbcConst<T>(default(T)) { Index = 0 });
        }

	    public int Count
        {
            get { return _list.Count; }
        }

        public AbcConst<T> this[int index]
        {
            get { return _list[index]; }
        }

        public int IndexOf(T value)
        {
            if (AbcConst<T>.IsString)
            {
                string s = value as string;
                if (s == null) return 0;
            }
            int i;
            if (_index.TryGetValue(value, out i))
                return i;
            return -1;
        }

        public AbcConst<T> Add(T value)
        {
            int index = _list.Count;
            var c = new AbcConst<T>(value) {Index = index};
            if (index != 0)
            {
                _index.Add(value, index);    
            }
            _list.Add(c);
            return c;
        }

        public AbcConst<T> Define(T value)
        {
            int i = IndexOf(value);
            if (i >= 0) return _list[i];
            return Add(value);
        }

	    public void Read(SwfReader reader)
        {
            int n = (int)reader.ReadUIntEncoded();
            for (int i = 1; i < n; ++i)
            {
                Add(reader.ReadAbcConst<T>(AbcConst<T>.SharedKind));
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
                    var value = this[i].Value;
                    writer.WriteAbcConst(value);
                }
            }
        }

	    private static string TypeName
        {
            get
            {
                var type = typeof(T);
                switch (Type.GetTypeCode(type))
                {
                    case TypeCode.Boolean:
                        return "bool";
                    case TypeCode.Char:
                        return "char";
                    case TypeCode.SByte:
                        return "int8";
                    case TypeCode.Byte:
                        return "uint8";
                    case TypeCode.Int16:
                        return "int16";
                    case TypeCode.UInt16:
                        return "uin16";
                    case TypeCode.Int32:
                        return "int";
                    case TypeCode.UInt32:
                        return "uint";
                    case TypeCode.Int64:
                        return "int64";
                    case TypeCode.UInt64:
                        return "uint64";
                    case TypeCode.Single:
                        return "float";
                    case TypeCode.Double:
                        return "double";
                    case TypeCode.Decimal:
                        return "decimal";
                    case TypeCode.String:
                        return "string";
                }
                return type.FullName;
            }
        }

        public string Name
        {
            get { return TypeName; }
        }

        public void DumpXml(XmlWriter writer)
        {
            writer.WriteStartElement(Name);
            int n = Count;
            writer.WriteAttributeString("count", n.ToString());
            for (int i = 0; i < n; ++i)
            {
                writer.WriteStartElement("item");
                writer.WriteAttributeString("index", i.ToString());
                writer.WriteAttributeString("value", this[i].ToString().ToXmlString());
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }

	    public IEnumerator<AbcConst<T>> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

	    IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

	    public override string ToString()
        {
            return string.Format("{0}[{1}]", Name, Count);
        }

	    IAbcConst IAbcConstPool.this[int index]
        {
            get { return this[index]; }
        }

        /// <summary>
        /// Determines whether given constant is defined in this pool.
        /// </summary>
        /// <param name="c">constant to check.</param>
        /// <returns>true if defined; otherwise, false</returns>
        public bool IsDefined(IAbcConst c)
        {
            if (c == null) return false;
            int i = c.Index;
            if (i < 0 || i >= Count) return false;
            return ReferenceEquals(c, this[i]);
        }

        /// <summary>
        /// Imports given constant.
        /// </summary>
        /// <param name="c">constant to import.</param>
        /// <returns>imported constant.</returns>
        public AbcConst<T> Import(AbcConst<T> c)
        {
            if (IsDefined(c)) return c;
            return Define(c.Value);
        }

        /// <summary>
        /// Imports given constant.
        /// </summary>
        /// <param name="c">constant to import.</param>
        /// <returns>imported constant.</returns>
        public IAbcConst Import(IAbcConst c)
        {
            return Import((AbcConst<T>)c);
        }
    }
}