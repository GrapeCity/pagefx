using System;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.Execution
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
		private readonly FieldSlot[] _fields;
		private IMethod _toString;
		private IMethod _equalsMethod;
		private IMethod _getHashCodeMethod;
		private Type _systemType;

		public FieldSlot[] Fields
		{
			get { return _fields; }
		}

		public Class(IType type, Class baseClass)
		{
			Type = type;
			BaseClass = baseClass;

			_fields = InitFields(type, true);
		}

		public IType Type { get; private set; }

		public Class BaseClass { get; private set; }

		public Type SystemType
		{
			get { return _systemType ?? (_systemType = new RuntimeType(Type)); }
		}

		public IMethod ToStringMethod
		{
			get { return _toString ?? (_toString = Type.FindMethodHierarchically("ToString", 0)); }
		}

		public IMethod EqualsMethod
		{
			get { return _equalsMethod ?? (_equalsMethod = Type.FindMethodHierarchically("Equals", SystemTypes.Object)); }
		}

		public IMethod GetHashCodeMethod
		{
			get { return _getHashCodeMethod ?? (_getHashCodeMethod = Type.FindMethodHierarchically("GetHashCode", 0)); }
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
				return type.Fields.Where(x => x.IsStatic && !x.IsConstant && !x.Type.IsArrayInitializer());
			}
			return GetHierarchy(type).SelectMany(x => x.Fields.Where(f => !f.IsStatic && !f.IsConstant && !x.Type.IsArrayInitializer()));
		}

		private static IEnumerable<IType> GetHierarchy(IType type)
		{
			var list = new List<IType>();

			while (type != null)
			{
				list.Add(type);

				type = type.BaseType;
			}

			list.Reverse();

			return list;
		}
	}
}