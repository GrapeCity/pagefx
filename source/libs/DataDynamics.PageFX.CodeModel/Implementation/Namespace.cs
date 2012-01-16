using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using DataDynamics.PageFX.CodeModel.Syntax;

namespace DataDynamics.PageFX.CodeModel
{
    public sealed class Namespace : INamespace, IXmlSerializationFeedback
    {
        #region Constructors
        public Namespace()
        {
        }

        public Namespace(string name)
        {
            _name = name;
        }
        #endregion

        #region INamespace Members
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private string _name;

        /// <summary>
        /// Get all declared types in this container.
        /// </summary>
        public ITypeCollection Types
        {
            get { return _types; }
        }
        private readonly TypeCollection _types = new TypeCollection();
        #endregion

        #region ICodeNode Members

        public CodeNodeType NodeType
        {
            get { return CodeNodeType.Namespace; }
        }

        public IEnumerable<ICodeNode> ChildNodes
        {
            get { return CMHelper.Enumerate(_types); }
        }

    	/// <summary>
    	/// Gets or sets user defined data assotiated with this object.
    	/// </summary>
    	public object Tag { get; set; }

    	#endregion

        #region IFormattable Members
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return SyntaxFormatter.Format(this, format, formatProvider);
        }
        #endregion

        #region IXmlSerializationFeedback Members
        string IXmlSerializationFeedback.XmlElementName
        {
            get { return null; }
        }

        void IXmlSerializationFeedback.WriteProperties(XmlWriter writer)
        {
            writer.WriteElementString("Name", _name);
        }
        #endregion

        public override string ToString()
        {
            return ToString(null, null);
        }
    }

    [XmlElementName("Namespaces")]
    public sealed class NamespaceCollection : INamespaceCollection
    {
        private readonly List<INamespace> _list = new List<INamespace>();
        private readonly Dictionary<string, INamespace> _cache = new Dictionary<string, INamespace>();

        #region INamespaceCollection Members
        public int Count
        {
            get { return _list.Count; }
        }

        public INamespace this[int index]
        {
            get { return _list[index]; }
        }

        public INamespace this[string name]
        {
            get
            {
                if (string.IsNullOrEmpty(name))
                {
                    if (_global == null)
                    {
                        _global = new Namespace("");
                        _list.Add(_global);
                    }
                    return _global;
                }
                else
                {
                    INamespace ns;
                    if (_cache.TryGetValue(name, out ns))
                        return ns;
                    ns = new Namespace(name);
                    _list.Add(ns);
                    _cache.Add(name, ns);
                    return ns;
                }
            }
        }

        private INamespace _global;

        public void Sort()
        {
            _list.Sort((x, y) => x.Name.CompareTo(y.Name));
            foreach (var ns in _list)
            {
                ns.Types.Sort();
            }
        }
        #endregion

        #region IEnumerable<Namespace> Members
        public IEnumerator<INamespace> GetEnumerator()
        {
            return _list.GetEnumerator();
        }
        #endregion

        #region IEnumerable Members
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        #region ICodeNode Members

        public CodeNodeType NodeType
        {
            get { return CodeNodeType.Namespaces; }
        }

        public IEnumerable<ICodeNode> ChildNodes
        {
            get { return CMHelper.Convert(_list); }
        }

    	/// <summary>
    	/// Gets or sets user defined data assotiated with this object.
    	/// </summary>
    	public object Tag { get; set; }

    	#endregion

        #region IFormattable Members
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return SyntaxFormatter.Format(this, format, formatProvider);
        }
        #endregion

        #region Object Override Members
        public override string ToString()
        {
            return ToString(null, null);
        }
        #endregion
    }
}