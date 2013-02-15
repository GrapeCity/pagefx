using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.CodeModel;

namespace DataDynamics.PageFX.Common.TypeSystem
{
    public sealed class PropertyCollection : MultiMemberCollection<IProperty>, IPropertyCollection
    {
		public static readonly IPropertyCollection Empty = new EmptyImpl();

        private sealed class EmptyImpl : IPropertyCollection
		{
			public IEnumerable<IProperty> Find(string name)
			{
				return Enumerable.Empty<IProperty>();
			}

			public void Add(IProperty property)
			{
				throw new NotSupportedException();
			}

			public IEnumerator<IProperty> GetEnumerator()
			{
				return Enumerable.Empty<IProperty>().GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}

			public int Count
			{
				get { return 0; }
			}

			public IProperty this[int index]
			{
				get { throw new ArgumentOutOfRangeException("index"); }
			}

			public IEnumerable<ICodeNode> ChildNodes
			{
				get { return Enumerable.Empty<ICodeNode>(); }
			}

			public object Data { get; set; }

			public string ToString(string format, IFormatProvider formatProvider)
			{
				return "";
			}
		}
    }
}