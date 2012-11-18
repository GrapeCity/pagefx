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
		private IReadOnlyList<IField> _list;

		public FieldList(AssemblyLoader loader, IType owner, int from, int to)
		{
			_loader = loader;
			_owner = owner;
			_from = from;
			_to = to;
		}

		public int Count
		{
			get { return List.Count; }
		}

		public IField this[int index]
		{
			get { return List[index]; }
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
			return List.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		private IReadOnlyList<IField> List
		{
			get { return _list ?? (_list = Populate().Memoize()); }
		}

		private IEnumerable<IField> Populate()
		{
			int n = _loader.Fields.Count;
			for (int i = _from; i < n && i < _to; ++i)
			{
				var field = _loader.Fields[i];

				field.DeclaringType = _owner;

				yield return field;
			}
		}
	}
}
