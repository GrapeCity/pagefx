using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.CodeModel.Syntax;

namespace DataDynamics.PageFX.CLI
{
	internal sealed class FieldList : IFieldCollection
	{
		private readonly AssemblyLoader _loader;
		private readonly IType _owner;
		private readonly int _from;
		private readonly int _to;
		private IDictionary<string,IField> _lookup;
		private IList<IField> _list;

		public FieldList(AssemblyLoader loader, IType owner, int from, int to)
		{
			_loader = loader;
			_owner = owner;
			_from = from;
			_to = to;
		}

		public int Count
		{
			get
			{
				Load();
				return _list.Count;
			}
		}

		public IField this[int index]
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
			get { return CodeNodeType.Fields; }
		}

		public IEnumerable<ICodeNode> ChildNodes
		{
			get { return this.Cast<ICodeNode>(); }
		}

		public object Tag { get; set; }

		public void Add(IField field)
		{
			throw new NotSupportedException();
		}

		public IField this[string name]
		{
			get
			{
				if (_lookup == null)
				{
					_lookup = this.ToDictionary(x => x.Name, x => x);
				}
				IField field;
				return _lookup.TryGetValue(name, out field) ? field : null;
			}
		}

		public IEnumerator<IField> GetEnumerator()
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

			_list = new List<IField>();

			int n = _loader.Fields.Count;
			for (int i = _from; i < n && i < _to; ++i)
			{
				var field = _loader.Fields.Get(_owner, i);
				field.DeclaringType = _owner;
				_list.Add(field);
			}
		}
	}
}
