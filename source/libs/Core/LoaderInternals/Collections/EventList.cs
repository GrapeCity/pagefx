using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Collections;
using DataDynamics.PageFX.Common.Syntax;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Core.LoaderInternals.Collections
{
	internal sealed class EventList : IEventCollection
	{
		private readonly IType _owner;
		private IReadOnlyList<IEvent> _list;
		private Dictionary<string, IEvent> _lookup;

		public EventList(IType owner)
		{
			_owner = owner;
		}

		private IReadOnlyList<IEvent> List
		{
			get { return _list ?? (_list = Populate().Memoize()); }
		}

		private IEnumerable<IEvent> Populate()
		{
			return _owner
				.Methods
				.Select(x => x.Association as IEvent)
				.Where(x => x != null)
				.Distinct(new ReferenceEqualityComparer<IEvent>());
		}

		public int Count
		{
			get { return List.Count; }
		}

		public IEvent this[int index]
		{
			get { return List[index]; }
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

		public void Add(IEvent item)
		{
			throw new NotSupportedException();
		}

		public IEvent this[string name]
		{
			get
			{
				if (_lookup == null)
				{
					_lookup = this.ToDictionary(x => x.Name, x => x);
				}
				IEvent item;
				return _lookup.TryGetValue(name, out item) ? item : null;
			}
		}

		public IEnumerator<IEvent> GetEnumerator()
		{
			return List.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		private sealed class ReferenceEqualityComparer<T> : IEqualityComparer<T>
			where T:class 
		{
			public bool Equals(T x, T y)
			{
				return ReferenceEquals(x, y);
			}

			public int GetHashCode(T obj)
			{
				return obj.GetHashCode();
			}
		}
	}
}
