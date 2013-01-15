using System;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Core.Execution
{
	internal sealed class FieldSlot
	{
		public FieldSlot(IField field)
		{
			// NOTE: field could be null for native calls
			Field = field;
		}

		public IField Field { get; private set; }

		public object Value { get; set; }

		public bool IsValueType
		{
			get { return Field != null && Field.Type.TypeKind == TypeKind.Struct; }
		}

		public FieldSlot Copy()
		{
			return new FieldSlot(Field)
			       	{
			       		Value = Value.Copy()
			       	};
		}
	}

	internal interface IFieldStorage
	{
		FieldSlot[] Fields { get; }
	}

	internal sealed class Class : IFieldStorage
	{
		private struct LazyMethod
		{
			private IMethod _value;

			public bool Initialized { get; private set; }

			public IMethod Value
			{
				get { return _value; }
				set
				{
					_value = value;
					Initialized = true;
				}
			}
		}

		private readonly FieldSlot[] _fields;
		private LazyMethod _toString;
		private LazyMethod _equalsMethod;
		private LazyMethod _getHashCodeMethod;
		private Type _systemType;

		public FieldSlot[] Fields
		{
			get { return _fields; }
		}

		public Class(IType type)
		{
			Type = type;

			_fields = InitFields(type, true);
		}

		public IType Type { get; private set; }

		public Class BaseClass { get; set; }

		public Type SystemType
		{
			get { return _systemType ?? (_systemType = new RuntimeType(Type)); }
		}

		public IMethod ToStringMethod
		{
			get
			{
				if (!_toString.Initialized)
				{
					_toString.Value = Type.FindMethodHierarchically(
						"ToString",
						x => x.Parameters.Count == 0,
						x => x.Is(SystemTypeCode.ValueType) || x.Is(SystemTypeCode.Object));
				}
				return _toString.Value;
			}
		}

		public IMethod EqualsMethod
		{
			get
			{
				if (!_equalsMethod.Initialized)
				{
					_equalsMethod.Value = Type.FindMethodHierarchically(
						"Equals",
						x => x.Parameters.Count == 1 && x.Parameters[0].Type.Is(SystemTypeCode.Object),
						x => x.Is(SystemTypeCode.ValueType) || x.Is(SystemTypeCode.Object));
				}
				return _equalsMethod.Value;
			}
		}

		public IMethod GetHashCodeMethod
		{
			get
			{
				if (!_getHashCodeMethod.Initialized)
				{
					_getHashCodeMethod.Value = Type.FindMethodHierarchically(
						"GetHashCode",
						x => x.Parameters.Count == 0,
						x => x.Is(SystemTypeCode.ValueType) || x.Is(SystemTypeCode.Object));
				}
				return _getHashCodeMethod.Value;
			}
		}

		public static FieldSlot[] InitFields(IType type, bool isStatic)
		{
			var fields = GetFields(type, isStatic).Select(x => new FieldSlot(x)).ToArray();

			//TODO: layout fields
			for (int i = 0; i < fields.Length; i++)
			{
				var field = fields[i].Field;
				field.Slot = i;
				fields[i].Value = field.Value ?? field.Type.GetDefaultValue();
			}

			return fields;
		}

		private static IEnumerable<IField> GetFields(IType type, bool isStatic)
		{
			if (isStatic)
			{
				return type.Fields.Where(x => x.IsStatic && !x.IsConstant && !x.IsArrayInitializer());
			}
			return GetHierarchy(type).SelectMany(x => x.Fields.Where(f => !f.IsStatic && !f.IsConstant && !f.IsArrayInitializer()));
		}

		private static IEnumerable<IType> GetHierarchy(IType type)
		{
			var list = new List<IType>();

			while (type != null && !type.Is(SystemTypeCode.Object) && !type.Is(SystemTypeCode.ValueType))
			{
				list.Add(type);

				type = type.BaseType;
			}

			list.Reverse();

			return list;
		}
	}
}