using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.CodeModel.Syntax;

namespace DataDynamics.PageFX.CodeModel
{
    [XmlElementName("Types")]
    public sealed class TypeCollection : ITypeCollection
    {
        private readonly Dictionary<string, IType> _cache = new Dictionary<string, IType>();
        private readonly List<IType> _list = new List<IType>();
        private readonly IType _owner;

        #region Constructors
        public TypeCollection()
        {
        }

        internal TypeCollection(IType owner)
        {
            _owner = owner;
        }
        #endregion

        #region ITypeCollection Members
        public int Count
        {
            get { return _list.Count; }
        }

        public IType this[int index]
        {
            get { return _list[index]; }
        }

        public IType this[string fullname]
        {
            get
            {
                IType res;
                if (_cache.TryGetValue(fullname, out res))
                    return res;
                return null;
            }
        }

	    public bool Contains(IType type)
        {
            return this[type.FullName] != null;
        }

        public void Add(IType type)
        {
            if (!Contains(type))
            {
                _list.Add(type);
                AddToCache(type);
            }
        }

        public IEnumerator<IType> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        private void AddToCache(IType type)
        {
            if (_owner != null)
            {
                type.DeclaringType = _owner;
				// this is needed to resolve subclass refs
				AddToCache(type.Name, type);
            }
            AddToCache(type.FullName, type);
        }

        private void AddToCache(string key, IType type)
        {
            IType typeInCache;
            if (_cache.TryGetValue(key, out typeInCache))
                return;
            _cache.Add(key, type);
        }

        public void AddRange(IEnumerable<IType> list)
        {
            foreach (var type in list)
                Add(type);
        }

        #region ICodeNode Members

        public CodeNodeType NodeType
        {
            get { return CodeNodeType.Types; }
        }

        public IEnumerable<ICodeNode> ChildNodes
        {
            get { return _list.Cast<ICodeNode>(); }
        }

    	/// <summary>
    	/// Gets or sets user defined data assotiated with this object.
    	/// </summary>
    	public object Tag { get; set; }

    	#endregion

	    public string ToString(string format, IFormatProvider formatProvider)
        {
            return SyntaxFormatter.Format(this, format, formatProvider);
        }

	    public static readonly ITypeCollection Empty = new EmptyImpl();

		private sealed class EmptyImpl : ITypeCollection
		{
			#region Impl
			public int Count
			{
				get { return 0; }
			}

			public IType this[int index]
			{
				get { return null; }
			}

			public IType this[string fullname]
			{
				get { return null; }
			}

			public void Add(IType type)
			{
			}

			public bool Contains(IType type)
			{
				return false;
			}

			public IEnumerator<IType> GetEnumerator()
			{
				return Enumerable.Empty<IType>().GetEnumerator();
			}
			
			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}

			public CodeNodeType NodeType
			{
				get { return CodeNodeType.Types; }
			}

			public IEnumerable<ICodeNode> ChildNodes
			{
				get { return this.Cast<ICodeNode>(); }
			}

			public object Tag { get; set; }

			public string ToString(string format, IFormatProvider formatProvider)
			{
				return "";
			}
			#endregion
		}
    }

    internal sealed class SimpleTypeCollection : List<IType>, ITypeCollection
    {
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return SyntaxFormatter.Format(this, format, formatProvider);
        }

        public override string ToString()
        {
            return ToString(null, null);
        }

        public CodeNodeType NodeType
        {
            get { return CodeNodeType.Types; }
        }

        public IEnumerable<ICodeNode> ChildNodes
        {
            get { return this.Cast<ICodeNode>(); }
        }

        public object Tag { get; set; }

        public IType this[string fullname]
        {
            get { return this.FirstOrDefault(t => t.FullName == fullname); }
        }
    }
}