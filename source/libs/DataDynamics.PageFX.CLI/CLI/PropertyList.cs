using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.CodeModel.Syntax;

namespace DataDynamics.PageFX.CLI
{
	internal sealed class PropertyList : IPropertyCollection
	{
		private readonly IType _owner;
		private Dictionary<string, IProperty[]> _lookup;
		private IReadOnlyList<IProperty> _list;

		public PropertyList(IType owner)
		{
			_owner = owner;
		}

		public IEnumerable<IProperty> Find(string name)
		{
			if (_lookup == null)
			{
				_lookup = this.GroupBy(x => x.Name).ToDictionary(x => x.Key, x => x.ToArray());
			}
			IProperty[] properties;
			return _lookup.TryGetValue(name, out properties) ? properties : Enumerable.Empty<IProperty>();
		}

		public void Add(IProperty property)
		{
			throw new NotSupportedException();
		}

		private IReadOnlyList<IProperty> List
		{
			get { return _list ?? (_list = _owner.Methods.Select(x => x.Association as IProperty).Where(x => x != null).Memoize()); }
		}

		public int Count
		{
			get { return List.Count; }
		}

		public IProperty this[int index]
		{
			get { return List[index]; }
		}

		public string ToString(string format, IFormatProvider formatProvider)
		{
			return SyntaxFormatter.Format(this, format, formatProvider);
		}

		public CodeNodeType NodeType
		{
			get { return CodeNodeType.Properties; }
		}

		public IEnumerable<ICodeNode> ChildNodes
		{
			get { return this.Cast<ICodeNode>(); }
		}

		public object Tag { get; set; }

		public IEnumerator<IProperty> GetEnumerator()
		{
			return List.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
