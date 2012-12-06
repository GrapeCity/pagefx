using System;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Syntax;

namespace DataDynamics.PageFX.Common.TypeSystem
{
	/// <summary>
	/// List of <see cref="Argument"/>s.
	/// </summary>
	public sealed class ArgumentCollection : List<IArgument>, IArgumentCollection
	{
		public ArgumentCollection()
		{
		}

		public ArgumentCollection(IEnumerable<IArgument> args)
			: base(args)
		{
		}

		public IArgument this[string name]
		{
			get { return Find(a => a.Name == name); }
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
	}
}