using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Syntax;

namespace DataDynamics.PageFX.Common.TypeSystem
{
	public sealed class GenericParameterCollection : List<IGenericParameter>, IGenericParameterCollection
	{
		public IGenericParameter Find(string name)
		{
			return Find(p => p.Name == name);
		}

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

		public static readonly IGenericParameterCollection Empty = new EmptyImpl();

		private sealed class EmptyImpl : IGenericParameterCollection
		{
			public IEnumerator<IGenericParameter> GetEnumerator()
			{
				yield break;
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}

			public int Count
			{
				get { return 0; }
			}

			public IGenericParameter this[int index]
			{
				get { return null; }
			}

			public string ToString(string format, IFormatProvider formatProvider)
			{
				return "";
			}

			public IEnumerable<ICodeNode> ChildNodes
			{
				get { return Enumerable.Empty<ICodeNode>(); }
			}

			public object Data { get; set; }

			public IGenericParameter Find(string name)
			{
				return null;
			}

			public void Add(IGenericParameter parameter)
			{
				throw new NotSupportedException();
			}
		}
	}
}