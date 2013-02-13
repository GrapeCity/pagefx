using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Syntax;

namespace DataDynamics.PageFX.Common.TypeSystem
{
	public sealed class PropertyParameterCollection : IParameterCollection
	{
		private readonly IProperty _property;

		public PropertyParameterCollection(IProperty property)
		{
			_property = property;
		}

		public IEnumerator<IParameter> GetEnumerator()
		{
			for (int i = 0; i < Count; i++)
				yield return this[i];
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public int Count
		{
			get
			{
				if (_property.Getter != null)
				{
					return _property.Getter.Parameters.Count;
				}
				if (_property.Setter != null)
				{
					return _property.Setter.Parameters.Count - 1;
				}
				return 0;
			}
		}

		public IParameter this[int index]
		{
			get
			{
				if (_property.Getter != null)
				{
					return _property.Getter.Parameters[index];
				}
				if (_property.Setter != null)
				{
					return _property.Setter.Parameters[index];
				}
				throw new ArgumentOutOfRangeException("index");
			}
		}

		public IParameter this[string name]
		{
			get { return this.FirstOrDefault(x => x.Name == name); }
		}

		public IEnumerable<ICodeNode> ChildNodes
		{
			get { return this.Cast<ICodeNode>(); }
		}

		public object Data { get; set; }

		public void Add(IParameter parameter)
		{
			throw new NotSupportedException();
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