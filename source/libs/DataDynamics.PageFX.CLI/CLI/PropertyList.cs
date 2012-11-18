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
		private List<IProperty> _list;

		public PropertyList(IType owner)
		{
			_owner = owner;
		}

		public IEnumerable<IProperty> Find(string name)
		{
			Load();
			IProperty[] properties;
			return _lookup.TryGetValue(name, out properties) ? properties : Enumerable.Empty<IProperty>();
		}

		public void Add(IProperty property)
		{
			throw new NotSupportedException();
		}

		public int Count
		{
			get
			{
				Load();
				return _list.Count;
			}
		}

		public IProperty this[int index]
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
			get { return CodeNodeType.Properties; }
		}

		public IEnumerable<ICodeNode> ChildNodes
		{
			get { return this.Cast<ICodeNode>(); }
		}

		public object Tag { get; set; }

		public IEnumerator<IProperty> GetEnumerator()
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

			_list = _owner.Methods.Select(x => x.Association as IProperty).Where(x => x != null).ToList();
			_lookup = _list.GroupBy(x => x.Name).ToDictionary(x => x.Key, x => x.ToArray());

			foreach (var property in _list)
			{
				property.DeclaringType = _owner;
			}
		}
	}
}
