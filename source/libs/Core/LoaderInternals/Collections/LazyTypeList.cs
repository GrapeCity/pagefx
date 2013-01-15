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
	internal abstract class LazyTypeList : ITypeCollection
	{
		private IReadOnlyList<IType> _list;

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

		public virtual IType FindType(string fullname)
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

		protected abstract IEnumerable<IType> Populate();
	}
}
