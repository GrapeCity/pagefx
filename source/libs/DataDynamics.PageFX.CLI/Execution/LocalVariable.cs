using System;
using DataDynamics.PageFX.Common;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.CLI.Execution
{
	public interface ILocalVariable
	{
		IVariable Metadata { get; }

		/// <summary>
		/// Gets the current value.
		/// </summary>
		object Value { get; set; }
	}

	internal sealed class LocalVariable : ILocalVariable
	{
		private readonly IVariable _variable;
		private readonly Func<IType, object> _init;
		private object _value;

		public LocalVariable(IVariable variable, Func<IType,object> init)
		{
			_variable = variable;
			_init = init;
			_value = variable.Type.GetDefaultValue();
		}

		public IVariable Metadata
		{
			get { return _variable; }
		}

		public string Name
		{
			get { return _variable.Name; }
		}

		public object Value
		{
			get
			{
				if (_value == null && _variable.Type.TypeKind == TypeKind.Struct)
				{
					_value = _init(_variable.Type);
				}
				return _value;
			}
			set { _value = value; }
		}

		public override string ToString()
		{
			return string.Format("{{{0}: {1}}}", Name, Value);
		}
	}
}