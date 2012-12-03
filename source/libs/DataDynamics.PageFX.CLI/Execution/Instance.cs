using System;
using System.Linq;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Ecma335.Execution
{
	/// <summary>
	/// HACK: inherited from Exception to allow throw of such objects
	/// </summary>
	internal sealed class Instance : Exception, IFieldStorage
	{
		private readonly VirtualMachine _engine;
		private readonly FieldSlot[] _fields;

		public Instance(VirtualMachine engine, Class klass)
		{
			if (engine == null) throw new ArgumentNullException("engine");
			if (klass == null) throw new ArgumentNullException("klass");

			_engine = engine;

			Class = klass;

			if (Type.TypeKind == TypeKind.Delegate)
			{
				_fields = new FieldSlot[3];
				InitFields(_fields.Length);
			}
			else
			{
				_fields = Class.InitFields(klass.Type, false);

				_engine.InitFields(this);
			}
		}

		private Instance(VirtualMachine engine, Class klass, FieldSlot[] fields)
		{
			_engine = engine;
			Class = klass;
			_fields = fields;
		}

		private void InitFields(int count)
		{
			for (int i = 0; i < count; i++)
			{
				_fields[i] = new FieldSlot(null);
			}
		}

		public Class Class { get; private set; }

		public IType Type
		{
			get { return Class.Type; }
		}

		public bool IsValueType
		{
			get { return Type.TypeKind == TypeKind.Struct; }
		}

		public FieldSlot[] Fields
		{
			get { return _fields; }
		}

		public override int GetHashCode()
		{
			if (Class.GetHashCodeMethod != null)
			{
				return Convert.ToInt32(_engine.Call(Class.GetHashCodeMethod, new object[] { this }));
			}

			return base.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (Class.EqualsMethod != null)
			{
				return Convert.ToBoolean(_engine.Call(Class.EqualsMethod, new[] { this, obj }));
			}

			//TODO: base implementation for value types
			if (IsValueType)
			{
				var other = obj as Instance;
				if (other == null) return false;
				if (other.Class != Class) return false;

				for (int i = 0; i < Fields.Length; i++)
				{
					if (!Equals(Fields[i].Value, other.Fields[i].Value))
					{
						return false;
					}
				}

				return true;
			}

			return obj == this;
		}

		public override string ToString()
		{
			if (Class.ToStringMethod != null)
			{
				return _engine.Call(Class.ToStringMethod, new object[] { this }) as string;
			}
			return Type.ToString();
		}

		public bool IsInstanceOf(IType type)
		{
			if (type.IsInterface)
			{
				return Type.Implements(type);
			}
			return Type.IsSubclassOf(type);
		}

		public Instance Copy()
		{
			var fields = _fields.Select(x => x.Copy()).ToArray();

			return new Instance(_engine, Class, fields);
		}
	}
}