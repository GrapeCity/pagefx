using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Syntax;

namespace DataDynamics.PageFX.Common.TypeSystem
{
    public sealed class EventCollection : IEventCollection
    {
		public static readonly IEventCollection Empty = new EmptyImpl();

	    private readonly IList<IEvent> _list = new List<IEvent>();

	    public void Add(IEvent item)
	    {
		    if (item == null)
				throw new ArgumentNullException("item");

			_list.Add(item);
	    }

	    public IEvent this[string name]
        {
            get { return this.FirstOrDefault(e => e.Name == name); }
        }

	    public IEnumerator<IEvent> GetEnumerator()
	    {
		    return _list.GetEnumerator();
	    }

	    IEnumerator IEnumerable.GetEnumerator()
	    {
		    return GetEnumerator();
	    }

	    public int Count
	    {
			get { return _list.Count; }
	    }

	    public IEvent this[int index]
	    {
			get { return _list[index]; }
	    }

	    public string ToString(string format, IFormatProvider formatProvider)
	    {
			return SyntaxFormatter.Format(this, format, formatProvider);
	    }

	    public IEnumerable<ICodeNode> ChildNodes
	    {
			get { return this.Cast<ICodeNode>(); }
	    }

	    public object Data { get; set; }

		private sealed class EmptyImpl : IEventCollection
		{
			public IEnumerator<IEvent> GetEnumerator()
			{
				return Enumerable.Empty<IEvent>().GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}

			public int Count
			{
				get { return 0; }
			}

			public IEvent this[int index]
			{
				get { throw new ArgumentOutOfRangeException("index"); }
			}

			public IEnumerable<ICodeNode> ChildNodes
			{
				get { return Enumerable.Empty<ICodeNode>(); }
			}

			public object Data { get; set; }

			public IEvent this[string name]
			{
				get { return null; }
			}

			public void Add(IEvent item)
			{
				throw new NotSupportedException();
			}

			public string ToString(string format, IFormatProvider formatProvider)
			{
				return "";
			}
		}
    }
}