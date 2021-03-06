using System;
using System.Globalization;
using System.Reflection;
using DataDynamics.PageFX.Common.TypeSystem;
using Module = System.Reflection.Module;

namespace DataDynamics.PageFX.Core.Execution
{
	internal sealed class RuntimeType : Type
	{
		private readonly IType _type;
		private Type _baseType;

		public RuntimeType(IType type)
		{
			_type = type;
		}

		#region Overrides of MemberInfo

		public override object[] GetCustomAttributes(bool inherit)
		{
			throw new NotImplementedException();
		}

		public override bool IsDefined(Type attributeType, bool inherit)
		{
			throw new NotImplementedException();
		}

		public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr)
		{
			throw new NotImplementedException();
		}

		public override Type GetInterface(string name, bool ignoreCase)
		{
			throw new NotImplementedException();
		}

		public override Type[] GetInterfaces()
		{
			throw new NotImplementedException();
		}

		public override EventInfo GetEvent(string name, BindingFlags bindingAttr)
		{
			throw new NotImplementedException();
		}

		public override EventInfo[] GetEvents(BindingFlags bindingAttr)
		{
			throw new NotImplementedException();
		}

		public override Type[] GetNestedTypes(BindingFlags bindingAttr)
		{
			throw new NotImplementedException();
		}

		public override Type GetNestedType(string name, BindingFlags bindingAttr)
		{
			throw new NotImplementedException();
		}

		public override Type GetElementType()
		{
			throw new NotImplementedException();
		}

		protected override bool HasElementTypeImpl()
		{
			throw new NotImplementedException();
		}

		protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
		{
			throw new NotImplementedException();
		}

		public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
		{
			throw new NotImplementedException();
		}

		protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			throw new NotImplementedException();
		}

		public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
		{
			throw new NotImplementedException();
		}

		public override FieldInfo GetField(string name, BindingFlags bindingAttr)
		{
			throw new NotImplementedException();
		}

		public override FieldInfo[] GetFields(BindingFlags bindingAttr)
		{
			throw new NotImplementedException();
		}

		public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
		{
			throw new NotImplementedException();
		}

		protected override TypeAttributes GetAttributeFlagsImpl()
		{
			throw new NotImplementedException();
		}

		protected override bool IsArrayImpl()
		{
			throw new NotImplementedException();
		}

		protected override bool IsByRefImpl()
		{
			throw new NotImplementedException();
		}

		protected override bool IsPointerImpl()
		{
			throw new NotImplementedException();
		}

		protected override bool IsPrimitiveImpl()
		{
			throw new NotImplementedException();
		}

		protected override bool IsCOMObjectImpl()
		{
			throw new NotImplementedException();
		}

		public override object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
		{
			throw new NotImplementedException();
		}

		public override Type UnderlyingSystemType
		{
			get { throw new NotImplementedException(); }
		}

		protected override ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			throw new NotImplementedException();
		}

		public override string Name
		{
			get { return _type.Name; }
		}

		public override Guid GUID
		{
			get { return Guid.Empty; }
		}

		public override Module Module
		{
			get { throw new NotImplementedException(); }
		}

		public override Assembly Assembly
		{
			get { throw new NotImplementedException(); }
		}

		public override string FullName
		{
			get { return _type.FullName; }
		}

		public override string Namespace
		{
			get { return _type.Namespace; }
		}

		public override string AssemblyQualifiedName
		{
			get
			{
				//TODO:
				return FullName;
			}
		}

		public override Type BaseType
		{
			get
			{
				if (_baseType == null && _type.BaseType != null)
				{
					_baseType = new RuntimeType(_type.BaseType);
				}
				return _baseType;
			}
		}

		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			throw new NotImplementedException();
		}

		#endregion

		public override string ToString()
		{
			return FullName;
		}
	}
}