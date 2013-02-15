using System;
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
	}
}