using System;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.Execution
{
	internal sealed class FieldPtr : IPointer
	{
		private readonly IFieldStorage _storage;
		private readonly IField _field;

		public FieldPtr(IFieldStorage storage, IField field)
		{
			_storage = storage;
			_field = field;
		}

		public object Value
		{
			get { return _storage.Fields[_field.Slot].Value; }
			set { _storage.Fields[_field.Slot].Value = value; }
		}
	}

	internal sealed class ArrayElementPtr : IPointer
	{
		private readonly Array _array;
		private readonly long _index;

		public ArrayElementPtr(Array array, long index)
		{
			_array = array;
			_index = index;
		}

		public object Value
		{
			get { return _array.GetValue(_index); }
			set { _array.SetValue(value, _index); }
		}
	}

	internal sealed class MdArrayElementPtr : IPointer
	{
		private readonly Array _array;
		private readonly int[] _index;

		public MdArrayElementPtr(Array array, int[] index)
		{
			_array = array;
			_index = index;
		}

		public object Value
		{
			get { return _array.GetValue(_index); }
			set { _array.SetValue(value, _index); }
		}
	}

	internal sealed class MethodPtr
	{
		private readonly object _instance;
		private readonly IMethod _method;

		public MethodPtr(object instance, IMethod method)
		{
			_instance = instance;
			_method = method;
		}

		public object Instance
		{
			get { return _instance; }
		}

		public IMethod Method
		{
			get { return _method; }
		}
	}
}