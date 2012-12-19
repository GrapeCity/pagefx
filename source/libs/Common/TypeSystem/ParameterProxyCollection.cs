using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Collections;
using DataDynamics.PageFX.Common.Syntax;

namespace DataDynamics.PageFX.Common.TypeSystem
{
	internal sealed class ParameterProxyCollection : IParameterCollection
	{
		private readonly IParameterCollection _source;
		private readonly IType _contextType;
		private readonly IMethod _contextMethod;
		private IReadOnlyList<IParameter> _list;

		public ParameterProxyCollection(IParameterCollection source, IType contextType, IMethod contextMethod)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (contextType == null)
				throw new ArgumentNullException("contextType");
			if (contextMethod == null)
				throw new ArgumentNullException("contextMethod");

			_source = source;
			_contextType = contextType;
			_contextMethod = contextMethod;
		}

		public IEnumerator<IParameter> GetEnumerator()
		{
			return List.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public int Count
		{
			get { return List.Count; }
		}

		public IParameter this[int index]
		{
			get { return List[index]; }
		}

		public string ToString(string format, IFormatProvider formatProvider)
		{
			return SyntaxFormatter.Format(this, format, formatProvider);
		}

		public override string ToString()
		{
			return ToString(null, null);
		}

		public IEnumerable<ICodeNode> ChildNodes
		{
			get { return this.Cast<ICodeNode>(); }
		}

		public object Data { get; set; }

		public IParameter this[string name]
		{
			get { return this.FirstOrDefault(x => x.Name == name); }
		}

		public void Add(IParameter parameter)
		{
			throw new NotSupportedException();
		}

		private IReadOnlyList<IParameter> List
		{
			get { return _list ?? (_list = Populate().Memoize()); }
		}

		private IEnumerable<IParameter> Populate()
		{
			return _source.Select(p => (IParameter)new ParameterProxy(p, GenericType.Resolve(_contextType, _contextMethod, p.Type)));
		}
	}
}