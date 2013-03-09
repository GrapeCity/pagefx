using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Syntax;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Core.LoaderInternals
{
	internal sealed class GenericParamList : IGenericParameterCollection
	{
		private readonly IList<IGenericParameter> _list;

		public GenericParamList(AssemblyLoader loader, IMetadataElement owner)
		{
			_list = loader.GenericParameters.Find(owner.MetadataToken).ToList();
		}

		public IEnumerator<IGenericParameter> GetEnumerator()
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

		public IGenericParameter this[int index]
		{
			get { return _list[index]; }
		}

		public IEnumerable<ICodeNode> ChildNodes
		{
			get { return this.Cast<ICodeNode>(); }
		}

		public object Data { get; set; }

		public IGenericParameter Find(string name)
		{
			return _list.FirstOrDefault(x => x.Name == name);
		}

		public void Add(IGenericParameter parameter)
		{
			throw new NotSupportedException("Cannot modify readonly collection!");
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