using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Syntax;

namespace DataDynamics.PageFX.Common.TypeSystem
{
	public sealed class GenericParameterCollection : ITypeCollection
	{
		private readonly List<IType> _list = new List<IType>();

		public IEnumerable<ICodeNode> ChildNodes
		{
			get { return this.Cast<ICodeNode>(); }
		}

		/// <summary>
		/// Gets or sets user defined data assotiated with this object.
		/// </summary>
		public object Data { get; set; }

		public string ToString(string format, IFormatProvider formatProvider)
		{
			return SyntaxFormatter.Format(this, format, formatProvider);
		}

		public override string ToString()
		{
			return ToString(null, null);
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

		public IType FindType(string fullname)
		{
			return _list.Find(x => x.Name == fullname);
		}

		public void Add(IType type)
		{
			if (type == null)
				throw new ArgumentNullException("type");
			if (type.TypeKind != TypeKind.GenericParameter)
				throw new ArgumentException("The generic parameter expected");
			_list.Add(type);
		}

		public bool Contains(IType type)
		{
			return _list.Contains(type);
		}
	}
}