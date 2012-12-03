using System;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Ecma335.Execution
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

	internal sealed class MethodPtr : IPointer
	{
		private IMethod _method;

		public MethodPtr(IMethod method)
		{
			_method = method;
		}

		public IMethod Method
		{
			get { return _method; }
		}

		public object Value
		{
			get { return _method; }
			set { _method = (IMethod)value; }
		}
	}
}