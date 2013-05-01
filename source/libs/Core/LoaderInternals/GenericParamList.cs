using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Syntax;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Core.LoaderInternals
{
	internal sealed class GenericParamList : ITypeCollection
	{
		private readonly IList<IType> _list;

		public GenericParamList(AssemblyLoader loader, IMetadataElement owner)
		{
			_list = loader.GenericParameters.Find(owner.MetadataToken);
		}

		public IEnumerator<IType> GetEnumerator()
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

		public IType this[int index]
		{
			get { return _list[index]; }
		}

		public IEnumerable<ICodeNode> ChildNodes
		{
			get { return this.Cast<ICodeNode>(); }
		}

		public object Data { get; set; }

		public IType FindType(string fullname)
		{
			return _list.FirstOrDefault(x => x.Name == fullname);
		}

		public void Add(IType parameter)
		{
			throw new NotSupportedException("Cannot modify readonly collection!");
		}

		public bool Contains(IType type)
		{
			return _list.Contains(type);
		}

		public string ToString(string format, IFormatProvider formatProvider)
		{
			return SyntaxFormatter.Format(this, format, formatProvider);
		}

		public override string ToString()
		{
			return ToString(null, null);
		}
	}
}