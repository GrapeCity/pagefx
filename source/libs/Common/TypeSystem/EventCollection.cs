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
	    private readonly IType _owner;
	    private readonly IList<IEvent> _list = new List<IEvent>();

	    public EventCollection(IType owner)
        {
	        _owner = owner;
        }

	    public void Add(IEvent item)
	    {
		    if (item == null)
				throw new ArgumentNullException("item");

		    item.DeclaringType = _owner;
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

	    public object Tag { get; set; }
    }
}