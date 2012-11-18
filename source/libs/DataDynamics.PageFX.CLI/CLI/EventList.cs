using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.CodeModel.Syntax;

namespace DataDynamics.PageFX.CLI
{
	internal sealed class EventList : IEventCollection
	{
		private readonly IType _owner;
		private List<IEvent> _list;
		private Dictionary<string, IEvent> _lookup;

		public EventList(IType owner)
		{
			_owner = owner;
		}

		public int Count
		{
			get
			{
				Load();
				return _list.Count;
			}
		}

		public IEvent this[int index]
		{
			get
			{
				Load();
				return _list[index];
			}
		}

		public string ToString(string format, IFormatProvider formatProvider)
		{
			return SyntaxFormatter.Format(this, format, formatProvider);
		}

		public CodeNodeType NodeType
		{
			get { return CodeNodeType.Events; }
		}

		public IEnumerable<ICodeNode> ChildNodes
		{
			get { return this.Cast<ICodeNode>(); }
		}

		public object Tag { get; set; }

		public void Add(IEvent item)
		{
			throw new NotSupportedException();
		}

		public IEvent this[string name]
		{
			get
			{
				Load();
				IEvent item;
				return _lookup.TryGetValue(name, out item) ? item : null;
			}
		}

		public IEnumerator<IEvent> GetEnumerator()
		{
			for (int i = 0; i < Count; i++)
			{
				yield return this[i];
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		private void Load()
		{
			if (_list != null) return;

			_list = _owner.Methods.Select(x => x.Association as IEvent).Where(x => x != null).ToList();
			_lookup = _list.ToDictionary(x => x.Name, x => x);

			foreach (var e in _list)
			{
				e.DeclaringType = _owner;
			}
		}
	}
}
