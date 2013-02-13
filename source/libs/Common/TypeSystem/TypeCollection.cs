using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Syntax;

namespace DataDynamics.PageFX.Common.TypeSystem
{
    public sealed class TypeCollection : ITypeCollection
    {
        private readonly Dictionary<string, IType> _cache = new Dictionary<string, IType>();
        private readonly List<IType> _list = new List<IType>();

	    public int Count
        {
            get { return _list.Count; }
        }

        public IType this[int index]
        {
            get { return _list[index]; }
        }

	    public IType FindType(string fullname)
	    {
		    IType res;
		    if (_cache.TryGetValue(fullname, out res))
			    return res;
		    return null;
	    }

	    public bool Contains(IType type)
        {
            return type != null && FindType(type.FullName) != null;
        }

        public void Add(IType type)
        {
	        if (type == null)
				throw new ArgumentNullException("type");

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

	    private void AddToCache(IType type)
        {
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

	    public IEnumerable<ICodeNode> ChildNodes
        {
            get { return _list.Cast<ICodeNode>(); }
        }

    	/// <summary>
    	/// Gets or sets user defined data assotiated with this object.
    	/// </summary>
    	public object Data { get; set; }

	    public string ToString(string format, IFormatProvider formatProvider)
        {
            return SyntaxFormatter.Format(this, format, formatProvider);
        }

	    public static readonly ITypeCollection Empty = new EmptyImpl();

		private sealed class EmptyImpl : ITypeCollection
		{
			public int Count
			{
				get { return 0; }
			}

			public IType this[int index]
			{
				get { return null; }
			}

			public IType FindType(string fullname)
			{
				return null;
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

			public IEnumerable<ICodeNode> ChildNodes
			{
				get { return this.Cast<ICodeNode>(); }
			}

			public object Data { get; set; }

			public string ToString(string format, IFormatProvider formatProvider)
			{
				return "";
			}
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

	    public IEnumerable<ICodeNode> ChildNodes
        {
            get { return this.Cast<ICodeNode>(); }
        }

        public object Data { get; set; }

	    public IType FindType(string fullname)
	    {
		    return this.FirstOrDefault(t => t.FullName == fullname);
	    }
    }
}