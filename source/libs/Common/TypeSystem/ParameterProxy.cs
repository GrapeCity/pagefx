using System;
using System.Collections.Generic;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Syntax;

namespace DataDynamics.PageFX.Common.TypeSystem
{
	public sealed class ParameterProxy : IParameterProxy
	{
		private readonly IParameter _parameter;
		private readonly IType _type;

		public ParameterProxy(IParameter parameter, IType type)
		{
			_parameter = parameter;
			_type = type;
		}

		public int MetadataToken
		{
			get { return _parameter.MetadataToken; }
			set { throw new NotSupportedException(); }
		}

		public ICustomAttributeCollection CustomAttributes
		{
			get { return _parameter.CustomAttributes; }
		}

		public object Value
		{
			get { return _parameter.Value; }
			set { throw new NotSupportedException(); }
		}

		public string ToString(string format, IFormatProvider formatProvider)
		{
			return SyntaxFormatter.Format(this, format, formatProvider);
		}

		public IEnumerable<ICodeNode> ChildNodes
		{
			get { return null; }
		}

		public object Data { get; set; }

		public string Documentation
		{
			get { return _parameter.Documentation; }
			set { throw new NotSupportedException(); }
		}

		public int Index
		{
			get { return _parameter.Index; }
			set { throw new NotSupportedException(); }
		}

		public string Name
		{
			get { return _parameter.Name; }
			set { throw new NotSupportedException(); }
		}

		public IType Type
		{
			get { return _type; }
			set { throw new NotSupportedException(); }
		}

		public bool IsIn
		{
			get { return _parameter.IsIn; }
		}

		public bool IsOut
		{
			get { return _parameter.IsOut; }
		}

		public bool IsAddressed { get; set; }

		public IInstruction Instruction { get; set; }

		public IParameter ProxyOf
		{
			get { return _parameter; }
		}
	}
}