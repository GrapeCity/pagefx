using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Collections;
using DataDynamics.PageFX.Common.Syntax;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Ecma335.Metadata;

namespace DataDynamics.PageFX.Ecma335.LoaderInternals.Collections
{
	internal sealed class InterfaceImpl : ITypeCollection
	{
		private readonly AssemblyLoader _loader;
		private readonly IType _owner;
		private IReadOnlyList<IType> _list;

		public InterfaceImpl(AssemblyLoader loader, IType owner)
		{
			_loader = loader;
			_owner = owner;
		}

		private int OwnerIndex
		{
			get { return _owner.RowIndex(); }
		}

		public int Count
		{
			get { return List.Count; }
		}

		public IType this[int index]
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

		public IType FindType(string fullname)
		{
			return this.FirstOrDefault(t => t.FullName == fullname);
		}

		public void Add(IType type)
		{
			throw new NotSupportedException();
		}

		public bool Contains(IType type)
		{
			return type != null && this.Any(x => ReferenceEquals(x, type));
		}

		public IEnumerator<IType> GetEnumerator()
		{
			return List.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		private IReadOnlyList<IType> List
		{
			get { return _list ?? (_list = Populate().Memoize()); }
		}

		private IEnumerable<IType> Populate()
		{
			var rows = _loader.Metadata.LookupRows(TableId.InterfaceImpl, Schema.InterfaceImpl.Class, OwnerIndex, true);
			return rows.Select(row =>
				{
					SimpleIndex ifaceIndex = row[Schema.InterfaceImpl.Interface].Value;
					var iface = _loader.GetTypeDefOrRef(ifaceIndex, new Context(_owner));
					if (iface == null)
						throw new BadMetadataException();
					return iface;
				});
		}
	}
}
