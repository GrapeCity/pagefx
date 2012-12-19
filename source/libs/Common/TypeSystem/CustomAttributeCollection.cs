using System;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Syntax;

namespace DataDynamics.PageFX.Common.TypeSystem
{
	public sealed class CustomAttributeCollection : List<ICustomAttribute>, ICustomAttributeCollection
	{
		public ICustomAttribute[] this[IType type]
		{
			get { return this.Where(x => ReferenceEquals(x.Type, type)).ToArray(); }
		}

		public ICustomAttribute[] this[string typeFullName]
		{
			get { return this.Where(x => x.Type.FullName == typeFullName).ToArray(); }
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
	}
}