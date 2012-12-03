using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.Syntax;

namespace DataDynamics.PageFX.Common.TypeSystem
{
	/// <summary>
	/// List of <see cref="Parameter"/>s.
	/// </summary>
	public sealed class ParameterCollection : List<IParameter>, IParameterCollection
	{
		public IParameter this[string name]
		{
			get { return Find(p => p.Name == name); }
		}

		public CodeNodeType NodeType
		{
			get { return CodeNodeType.Parameters; }
		}

		public IEnumerable<ICodeNode> ChildNodes
		{
			get { return this.Cast<ICodeNode>(); }
		}

		/// <summary>
		/// Gets or sets user defined data assotiated with this object.
		/// </summary>
		public object Tag { get; set; }

		public string ToString(string format, IFormatProvider formatProvider)
		{
			return SyntaxFormatter.Format(this, format, formatProvider);
		}

		public override string ToString()
		{
			return ToString(null, null);
		}

		public static readonly IParameterCollection Empty = new EmptyImpl();

		#region class EmptyImpl

		private sealed class EmptyImpl : IParameterCollection
		{
			public IEnumerator<IParameter> GetEnumerator()
			{
				return Enumerable.Empty<IParameter>().GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}

			public int Count
			{
				get { return 0; }
			}

			public IParameter this[int index]
			{
				get { throw new ArgumentOutOfRangeException("index"); }
			}

			public string ToString(string format, IFormatProvider formatProvider)
			{
				return "";
			}

			public CodeNodeType NodeType
			{
				get { return CodeNodeType.Parameters; }
			}

			public IEnumerable<ICodeNode> ChildNodes
			{
				get { return this.Cast<ICodeNode>(); }
			}

			public object Tag { get; set; }

			public IParameter this[string name]
			{
				get { return null; }
			}

			public void Add(IParameter parameter)
			{
				throw new NotSupportedException();
			}
		}

		#endregion
	}
}