using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using DataDynamics.PageFX.Common.CompilerServices;
using DataDynamics.PageFX.Common.Services;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.Abc;
using DataDynamics.PageFX.FlashLand.Avm;
using DataDynamics.PageFX.FlashLand.IL;

namespace DataDynamics.PageFX.FlashLand.Core.CodeGeneration.Builders
{
	internal sealed partial class ReflectionImpl
	{
		private const int varType = 1;
		private const int varTempArray = 2;

		private int NameDummyCounter;

		/// <summary>
		/// Returns instance that stores reflection data for API builtin in target player (AVM, Flash, AIR).
		/// </summary>
		/// <returns></returns>
		private AbcInstance DefineReflectionInstance()
		{
			if (_instanceReflection != null) return _instanceReflection;
			var name = Abc.DefineName(QName.PfxPackage("PlayerReflectionData"));
			_instanceReflection = Abc.DefineEmptyInstance(name, true);
			Abc.DefineScript(_instanceReflection);
			return _instanceReflection;
		}

		private AbcInstance _instanceReflection;

		private AbcInstance FixInstance(AbcInstance instance)
		{
			if (instance.IsNative)
				return DefineReflectionInstance();
			if (instance.IsInterface)
				return _generator.RuntimeImpl.Instance;
			return instance;
		}

		private string GetMethodSuffix(IType type)
		{
			var suffix = type.GetSigName();
			if (suffix == null)
			{
				suffix = NameDummyCounter.ToString();
				++NameDummyCounter;
			}
			return suffix;
		}

		#region FieldInfo

		private void NewFieldInfo(AbcCode code, AbcInstance instance, IField field, int varField)
		{
			var trait = field.Data as AbcTrait;
			if (trait == null)
			{
				code.PushNull();
				return;
			}

			var name = trait.Name;

			var fieldInfo = _generator.Corlib.GetInstance(CorlibTypeId.FieldInfo);
			code.CreateInstance(fieldInfo);
			code.SetLocal(varField);

			var ns = name.Namespace;
			code.GetLocal(varField);
			code.PushNamespace(ns);
			code.SetField(FieldId.FieldInfo_Namespace);

			code.GetLocal(varField);
			code.PushString(field.Name);
			code.SetField(FieldId.FieldInfo_Name);

			int typeIndex = GetTypeId(field.Type);
			code.GetLocal(varField);
			code.PushInt(typeIndex);
			code.SetField(FieldId.FieldInfo_Type);

			typeIndex = GetTypeId(field.DeclaringType);
			code.GetLocal(varField);
			code.PushInt(typeIndex);
			code.SetField(FieldId.FieldInfo_DeclType);

			code.GetLocal(varField);
			code.PushBool(field.IsStatic);
			code.SetField(FieldId.FieldInfo_IsStatic);

			if (GlobalSettings.ReflectionSupport)
			{
				InitCustomAttributes(code, instance, field, varField);
			}

			code.GetLocal(varField);
		}

		private AbcMethod DefineMyFieldsInitializer(AbcInstance instance, IType type)
		{
			var myfields = new List<IField>(type.Fields.Where(f => !f.IsStatic));
			if (myfields.Count == 0) return null;

			instance = FixInstance(instance);

			var name = _generator.Abc.DefineName(QName.PfxPublic("init_myfields_" + GetMethodSuffix(type)));

			return instance.DefineMethod(
				Sig.@static(name, _generator.Corlib.Array.Instance),
				code =>
					{
						var typeFieldInfo = _generator.Corlib.GetType(CorlibTypeId.FieldInfo);
						const int arr = 1;
						code.NewArray(arr, typeFieldInfo, myfields,
						              f => NewFieldInfo(code, instance, f, 2));
						code.ReturnValue();
					});
		}

		private bool IsMemberwiseCloneCompiled
		{
			get
			{
				if (_isMemberwiseCloneCompiled.HasValue)
					return _isMemberwiseCloneCompiled.Value;
				var m = SystemTypes.Object.Methods.Find("MemberwiseClone", 0);
				if (m == null)
					throw new InvalidOperationException("Invalid corlib");
				_isMemberwiseCloneCompiled = m.Data is AbcMethod;
				return _isMemberwiseCloneCompiled.Value;
			}
		}

		private bool? _isMemberwiseCloneCompiled;

		private bool MustInitFields
		{
			get
			{
				if (GlobalSettings.ReflectionSupport) return true;
				if (IsMemberwiseCloneCompiled) return true;
				return false;
			}
		}

		private void InitFields(AbcCode code, AbcInstance instance, IType type)
		{
			if (!MustInitFields) return;
			if (type.IsInterface) return;
			var init = DefineMyFieldsInitializer(instance, type);
			if (init != null)
			{
				code.GetLocal(varType);
				code.GetStaticFunction(init);
				code.SetProperty(Const.Type.MyFieldsInit);
			}
		}

		#endregion

		#region CustomAttributes

		private AbcMethod DefineCustomAttributesInitializer(AbcInstance instance, ICustomAttributeProvider provider)
		{
			instance = FixInstance(instance);

			var provname = provider.GetQName();
			if (provname == null)
			{
				provname = NameDummyCounter.ToString();
				++NameDummyCounter;
			}
			var name = _generator.Abc.DefineName(QName.PfxPublic("init_custom_attrs_" + provname));

			return instance.DefineMethod(
				Sig.@static(name, _generator.Corlib.Array.Instance),
				code =>
					{
						const int arr = 1;
						const int varAttr = 2;

						code.NewArray(arr, SystemTypes.Object, provider.CustomAttributes,
						              attr => NewAttribute(code, attr, varAttr));

						code.ReturnValue();
					});
		}

		private void NewAttribute(AbcCode code, ICustomAttribute attr, int varAttr)
		{
			code.NewObject(attr.Constructor,
			               () =>
				               {
					               foreach (var arg in attr.FixedArguments())
						               code.PushValue(code, arg.Value);
				               });
			code.SetLocal(varAttr);
			//TODO: Set fields and properties
			foreach (var arg in attr.NamedArguments())
			{
				code.GetLocal(varAttr);
				code.PushValue(code, arg.Value);
				if (arg.Kind == ArgumentKind.Field)
				{
					var field = arg.Member as IField;
					if (field == null)
						throw new InvalidOperationException();
					code.SetField(field);
				}
				else
				{
					var prop = arg.Member as IProperty;
					if (prop == null)
						throw new InvalidOperationException();
					var s = _generator.MethodBuilder.BuildAbcMethod(prop.Setter);
					code.Call(s);
				}

			}
			code.GetLocal(varAttr);
		}

		private void InitCustomAttributes(AbcCode code, AbcInstance instance, ICustomAttributeProvider provider, int var)
		{
			var init = DefineCustomAttributesInitializer(instance, provider);
			code.GetLocal(var);
			code.GetStaticFunction(init);
			code.SetProperty(Const.MemberInfo.CustomAttrsInit);
		}

		#endregion

		#region Method Wrappers

		private int MethodWrappperCounter;

		private static bool ShouldWrap(IMethod m)
		{
			var abcMethod = m.AbcMethod();
			if (abcMethod == null) return false;
			if (abcMethod.IsAccessor) return true;
			if (!m.IsStatic) return true;
			return false;
		}

		private AbcMethod DefineMetodWrapper(IMethod method, bool init)
		{
			var abcMethod = method.AbcMethod();
			if (abcMethod == null) return null;
			if (!ShouldWrap(method)) return abcMethod;

			var provname = ++MethodWrappperCounter;
			const string prefix = "wrap_";
			var name = _generator.Abc.DefineName(QName.PfxPublic(prefix + provname));
			var instance = abcMethod.Instance;
			bool addParam = false;

			instance = FixInstance(instance);

			var wrapper = instance.DefineMethod(
				Sig.@static(name, AvmTypeCode.Object),
				code =>
					{
						int n = abcMethod.Parameters.Count;
						bool hasReturn = false;
						if (method.IsConstructor)
						{
							if (init)
							{
								//addParam = true;
								if (abcMethod.IsInitializer)
								{
									//code.ThrowNotSupportedException();
									//return;
									//addParam = true;
									//code.LoadArguments(n + 1);
									//code.Add(InstructionCode.Callstatic, am, n);
								}
								else
								{
									addParam = true;
									code.LoadArguments(n + 1);
									code.Call(abcMethod);
								}
							}
							else
							{
								hasReturn = true;
								code.Getlex(abcMethod);
								if (abcMethod.IsInitializer)
								{
									code.LoadArguments(n);
									code.Construct(n);
								}
								else
								{
									code.Construct(0);
									code.Dup();
									code.LoadArguments(n);
									code.Call(abcMethod);
								}
							}
						}
						else
						{
							hasReturn = !abcMethod.IsVoid;
							if (method.IsStatic)
							{
								code.Getlex(abcMethod);
							}
							else
							{
								addParam = true;
								++n;
							}

							code.LoadArguments(n);
							code.Call(abcMethod);
						}

						if (!hasReturn)
							code.PushNull();
						code.ReturnValue();
					});

			if (addParam)
			{
				wrapper.Parameters.Add(Abc.CreateParameter(AvmTypeCode.Object, "obj"));
			}
			_generator.MethodBuilder.CopyParameters(wrapper, abcMethod);

			return wrapper;
		}

		#endregion

		#region Methods & Constructors

		private void NewParameterInfo(AbcCode code, AbcInstance instance, IParameter param, int varMethod, int varParam)
		{
			var pitype = _generator.Corlib.GetType(CorlibTypeId.ParameterInfo);
			if (param.Type == null)
				throw new InvalidOperationException("Parametr type is null");

			var ctor = pitype.FindConstructor(0);
			if (ctor == null)
				throw new InvalidOperationException(".ctor not found");
			code.NewObject(ctor, () => { });
			code.SetLocal(varParam);

			code.GetLocal(varParam);
			code.PushInt(GetTypeId(param.Type));
			code.SetField(FieldId.ParameterInfo_ClassImpl);

			code.GetLocal(varParam);
			code.PushString(param.Name);
			code.SetField(FieldId.ParameterInfo_NameImpl);

			code.GetLocal(varParam);
			code.GetLocal(varMethod);
			code.SetField(FieldId.ParameterInfo_MemberImpl);

			InitCustomAttributes(code, instance, param, varParam);

			code.GetLocal(varParam);
		}

		private static MethodAttributes GetMethodAttributes(IMethod method)
		{
			MethodAttributes ma = 0;

			if (method.IsAbstract)
				ma |= MethodAttributes.Abstract;
			if (method.IsStatic)
				ma |= MethodAttributes.Static;
			if (method.IsVirtual)
				ma |= MethodAttributes.Virtual;

			switch (method.Visibility)
			{
				case Visibility.Public:
				case Visibility.NestedPublic:
					ma |= MethodAttributes.Public;
					break;

				case Visibility.Internal:
				case Visibility.NestedInternal:
					ma |= MethodAttributes.Assembly;
					break;

				case Visibility.Protected:
				case Visibility.NestedProtected:
					ma |= MethodAttributes.Family;
					break;

				case Visibility.ProtectedInternal:
				case Visibility.NestedProtectedInternal:
					ma |= MethodAttributes.FamORAssem;
					break;

				case Visibility.NestedPrivate:
				case Visibility.Private:
					ma |= MethodAttributes.Private;
					break;

				case Visibility.PrivateScope:
					ma |= MethodAttributes.PrivateScope;
					break;
			}
			return ma;
		}

		#region NewMethodInfo

		private void NewMethodInfo(AbcCode code, AbcInstance instance, IMethod method,
		                           int varMethod, int varParams, int varParam,
		                           IType mtype, int index)
		{
			var ctor = mtype.FindConstructor(0);
			if (ctor == null)
				throw new InvalidOperationException(".ctor not found");

			var abcMethod = method.AbcMethod();
			if (abcMethod == null)
				throw new InvalidOperationException();
			abcMethod.MethodInfoIndex = index;

			code.NewObject(ctor, () => { });
			code.SetLocal(varMethod);

			code.GetLocal(varMethod);
			code.PushString(method.Name);
			code.SetField(FieldId.MethodBase_Name);

			code.GetLocal(varMethod);
			var wrapper = DefineMetodWrapper(method, true);
			code.GetStaticFunction(wrapper);
			code.SetField(FieldId.MethodBase_Function);

			if (method.IsConstructor)
			{
				code.GetLocal(varMethod);
				wrapper = DefineMetodWrapper(method, false);
				code.GetStaticFunction(wrapper);
				code.SetField(FieldId.ConstructorInfo_CreateFunction);
			}

			var mattrs = (int)GetMethodAttributes(method);
			code.GetLocal(varMethod);
			code.PushInt(mattrs);
			code.SetField(FieldId.MethodBase_Attributes);

			code.GetLocal(varMethod);
			code.NewArray(varParams, mtype, method.Parameters,
			              param => NewParameterInfo(code, instance, param, varMethod, varParam));
			code.SetField(FieldId.MethodBase_Parameters);

			InitCustomAttributes(code, instance, method, varMethod);

			code.GetLocal(varMethod);
		}

		#endregion

		// Pointer and Reference type parameters currently not supported
		private static bool IsUnsupportedMethod(IMethod method)
		{
			var abcMethod = method.AbcMethod();
			if (abcMethod == null)
				return true;
			if (abcMethod.IsInitializer) return true;
			if (method.IsGeneric)
				return true;
			return
				method.Parameters.Any(
					p => p.IsByRef() || p.Type.TypeKind == TypeKind.Pointer || p.Type.HasGenericParams());
		}

		private AbcMethod DefineMethodsInitializer(AbcInstance instance, IType type, bool ctor)
		{
			var provname = type.GetSigName();
			string prefix = "init_methods_";
			if (ctor)
				prefix = "init_ctors_";

			var name = _generator.Abc.DefineName(QName.PfxPublic(prefix + provname));

			var methods =
				new List<IMethod>(type.Methods.Where(m => m.IsConstructor == ctor && !IsUnsupportedMethod(m)));

			var mtype = ctor
							? _generator.Corlib.GetType(CorlibTypeId.ConstructorInfo)
							: _generator.Corlib.GetType(CorlibTypeId.MethodInfo);

			instance = FixInstance(instance);

			return instance.DefineMethod(
				Sig.@static(name, _generator.Corlib.Array.Instance),
				code =>
					{
						const int arr = 1;
						const int varObj = 2;
						const int varParams = 3;
						const int varParam = 4;

						code.NewArray(arr, mtype, methods,
						              (method, index) => NewMethodInfo(code, instance, method,
						                                               varObj, varParams, varParam,
						                                               mtype, index));
						code.ReturnValue();
					});
		}

		private void InitMethods(AbcCode code, AbcInstance instance, IType type, int var)
		{
			var init = DefineMethodsInitializer(instance, type, false);
			code.GetLocal(var);
			code.GetStaticFunction(init);
			code.SetProperty(Const.Type.MethodsInit);
		}

		private void InitConstructors(AbcCode code, AbcInstance instance, IType type, int var)
		{
			// TODO: problem is here
			var init = DefineMethodsInitializer(instance, type, true);
			code.GetLocal(var);
			code.GetStaticFunction(init);
			code.SetProperty(Const.Type.ConstructorsInit);
		}

		#endregion

		#region Properties

		/// <summary>
		/// Create, initialize and put on stack PropertyInfo object
		/// </summary>
		/// <param name="code"></param>
		/// <param name="prop"></param>
		private void NewPropertyInfo(AbcCode code, AbcInstance instance, IProperty prop, int varProp)
		{
			var type = _generator.Corlib.GetType(CorlibTypeId.PropertyInfo);
			var ctor = type.FindConstructor(0);
			if (ctor == null)
				throw new InvalidOperationException(".ctor not found");
			code.NewObject(ctor, () => { });
			code.SetLocal(varProp);

			code.GetLocal(varProp);
			code.PushString(prop.Name);
			code.SetField(FieldId.PropertyInfo_Name);

			code.GetLocal(varProp);
			code.PushTypeId(prop.Type);
			code.SetField(FieldId.PropertyInfo_Type);

			SetAccessor(code, prop.Getter, FieldId.PropertyInfo_Getter, varProp);
			SetAccessor(code, prop.Setter, FieldId.PropertyInfo_Setter, varProp);

			InitCustomAttributes(code, instance, prop, varProp);

			code.GetLocal(varProp);
		}

		private static void SetAccessor(AbcCode code, IMethod accessor, FieldId fieldId, int varProp)
		{
			if (accessor == null) return;
			var abcMethod = accessor.AbcMethod();
			if (abcMethod == null) return;
			int index = abcMethod.MethodInfoIndex;
			code.GetLocal(varProp);
			code.PushInt(index);
			code.SetField(fieldId);
		}

		/// <summary>
		/// m_properties initializer
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		private AbcMethod DefinePropertiesInitializer(AbcInstance instance, IType type)
		{
			var provname = type.GetSigName();
			if (provname == null)
			{
				provname = NameDummyCounter.ToString();
				++NameDummyCounter;
			}
			var name = _generator.Abc.DefineName(QName.PfxPublic("init_properties_" + provname));

			instance = FixInstance(instance);

			return instance.DefineMethod(
				Sig.@static(name, _generator.Corlib.Array.Instance),
				code =>
					{
						var mtype = _generator.Corlib.GetType(CorlibTypeId.PropertyInfo);
						const int arr = 1;
						code.NewArray(arr, mtype, type.Properties,
						              property => NewPropertyInfo(code, instance, property, 2));
						code.ReturnValue();
					});
		}

		private void InitProperties(AbcCode code, AbcInstance instance, IType type, int varObj)
		{
			var propertiesInitializer = DefinePropertiesInitializer(instance, type);
			code.GetLocal(varObj);
			code.GetStaticFunction(propertiesInitializer);
			code.SetProperty(Const.Type.PropertiesInit);
		}

		#endregion

		#region EnumInfo

		private AbcMethod DefineEnumInfoInitializer(IType type)
		{
			var enumInstance = type.AbcInstance();
			if (enumInstance == null)
				throw new InvalidOperationException();

			var EnumInfo = _generator.Corlib.GetInstance(CorlibTypeId.EnumInfo);

			var name = _generator.Abc.DefineName(QName.PfxPublic("init_enum_info_" + type.GetSigName()));
			return enumInstance.DefineMethod(
				Sig.@static(name, EnumInfo),
				code =>
					{
						const int varEnumInfo = 1;
						const int varArr = 2;
						code.CreateInstance(EnumInfo);
						code.SetLocal(varEnumInfo);

						if (type.HasAttribute("System.FlagsAttribute"))
						{
							code.GetLocal(varEnumInfo);
							code.PushBool(true);
							code.SetProperty(Const.EnumInfo.Flags);
						}

						var fields = type.GetEnumFields();
						var utype = type.ValueType;

						code.GetLocal(varEnumInfo);

						code.NewArray(varArr, SystemTypes.Object, fields,
						              f =>
							              {
								              var val = f.Value;
								              if (val == null)
									              throw new InvalidOperationException();
								              val = ToIntegralType(utype, val);
								              code.Box(type, () => code.LoadConstant(val));
							              });

						code.SetProperty(Const.EnumInfo.Values);

						code.GetLocal(varEnumInfo);
						code.NewArray(varArr, SystemTypes.String, fields,
						              field => code.PushString(field.Name));
						code.SetProperty(Const.EnumInfo.Names);

						code.GetLocal(varEnumInfo);
						code.ReturnValue();
					});
		}

		private static object ToIntegralType(IType type, object value)
		{
			var st = type.SystemType();
			if (st == null)
				throw new ArgumentException("Type is not system");
			switch (st.Code)
			{
				case SystemTypeCode.Int8:
					return Convert.ToSByte(value, CultureInfo.InvariantCulture);
				case SystemTypeCode.UInt8:
					return Convert.ToByte(value, CultureInfo.InvariantCulture);
				case SystemTypeCode.Int16:
					return Convert.ToInt16(value, CultureInfo.InvariantCulture);
				case SystemTypeCode.UInt16:
					return Convert.ToUInt16(value, CultureInfo.InvariantCulture);
				case SystemTypeCode.Int32:
					return Convert.ToInt32(value, CultureInfo.InvariantCulture);
				case SystemTypeCode.UInt32:
					return Convert.ToUInt32(value, CultureInfo.InvariantCulture);
				case SystemTypeCode.Int64:
					return Convert.ToInt64(value, CultureInfo.InvariantCulture);
				case SystemTypeCode.UInt64:
					return Convert.ToUInt64(value, CultureInfo.InvariantCulture);
				default:
					throw new ArgumentException("Invalid type");
			}
		}

		#endregion

		#region TypeInfo

		private void InitTypeInfo(AbcCode code, IType type, int typeId)
		{
			var instance = _generator.TypeBuilder.BuildInstance(type);

			//NOTE: typeId is assigned in corlib in Assembly.GetType
			//code.GetLocal(varType);
			//code.PushInt(typeId);
			//code.SetProperty("index");

			//WARNING: Sync field names with Type in corlib.
			code.GetLocal(varType);
			code.PushString(type.Namespace);
			code.SetProperty(Const.Type.Namespace);

			code.GetLocal(varType);
			code.PushNamespace(instance.Name.Namespace);
			code.SetProperty(Const.Type.NamespaceObject);

			code.GetLocal(varType);
			code.PushString(type.NestedName);
			code.SetProperty(Const.Type.Name);

			int baseIndex = GetTypeId(type.BaseType);
			code.GetLocal(varType);
			code.PushInt(baseIndex);
			code.SetProperty(Const.Type.BaseType);

			int typeKind = type.GetCorlibKind();
			if (typeKind != 0)
			{
				code.GetLocal(varType);
				code.PushInt(typeKind);
				code.SetProperty(Const.Type.Kind);
			}

			if (type.IsArray)
			{
				code.GetLocal(varType);
				code.PushInt(type.ArrayDimensions.Count + 1);
				code.SetProperty(Const.Type.Rank);
			}

			if (type.ElementType != null)
			{
				var elemType = type.ElementType;
				int elemTypeIndex = GetTypeId(elemType);
				if (elemTypeIndex < 0)
					throw new InvalidOperationException();
				code.GetLocal(varType);
				code.PushInt(elemTypeIndex);
				code.SetProperty(Const.Type.ElementType);
			}

			if (type.IsEnum)
			{
				var utype = type.ValueType;
				SetUnderlyingType(code, utype);

				var init = DefineEnumInfoInitializer(type);
				code.GetLocal(varType);
				code.GetStaticFunction(init);
				code.SetProperty(Const.Type.EnumInfoInit);
			}
			else if (type.IsNullableInstance())
			{
				var gi = (IGenericInstance)type;
				var utype = gi.GenericArguments[0];
				if (utype.IsEnum)
					utype = utype.ValueType;
				SetUnderlyingType(code, utype);
			}

			InitTypeFuncs(code, type, instance);

			if (GlobalSettings.ReflectionSupport)
			{
				InitCustomAttributes(code, instance, type, varType);
				InitMethods(code, instance, type, varType);
				InitConstructors(code, instance, type, varType);
				InitProperties(code, instance, type, varType);
			}

			InitFields(code, instance, type);

			var ifaces = GetInterfaces(type);
			if (ifaces != null)
			{
				code.GetLocal(varType);

				code.NewNativeArray(
					varTempArray, ifaces,
					iface => code.PushTypeId(iface));

				code.SetProperty(Const.Type.Interfaces);
			}
		}

		private void SetUnderlyingType(AbcCode code, IType utype)
		{
			code.GetLocal(varType);
			code.PushInt(GetTypeId(utype));
			code.SetProperty(Const.Type.UnderlyingType);
		}

		private void InitTypeFuncs(AbcCode code, IType type, AbcInstance instance)
		{
			var f = _generator.Boxing.Box(type);
			if (f != null)
			{
				code.GetLocal(varType);
				code.GetStaticFunction(f);
				code.SetProperty(Const.Type.BoxFunction);
			}

			f = _generator.Boxing.Unbox(type, false);
			if (f != null)
			{
				code.GetLocal(varType);
				code.GetStaticFunction(f);
				code.SetProperty(Const.Type.UnboxFunction);
			}

			f = CopyImpl.StaticCopy(instance);
			if (f != null)
			{
				code.GetLocal(varType);
				code.GetStaticFunction(f);
				code.SetProperty(Const.Type.CopyFunction);
			}

			var ctor = type.FindParameterlessConstructor();
			if (ctor != null)
			{
				f = _generator.MethodBuilder.Build(ctor) as AbcMethod;
				if (f != null && !f.IsInitializer)
				{
					f = _generator.TypeBuilder.DefineCtorStaticCall(ctor);
					Debug.Assert(f != null);
					code.GetLocal(varType);
					code.GetStaticFunction(f);
					code.SetProperty(Const.Type.CreateFunction);
				}
			}
		}

		private static List<IType> GetInterfaces(IType type)
		{
			var list = new List<IType>();
			var hash = new Hashtable();
			GetInterfaces(type, list, hash);
			return list;
		}

		private static void GetInterfaces(IType type, List<IType> list, Hashtable hash)
		{
			if (type.BaseType != null)
				GetInterfaces(type.BaseType, list, hash);
			if (type.Interfaces != null)
			{
				foreach (var iface in type.Interfaces)
				{
					if (!hash.Contains(iface))
					{
						hash[iface] = iface;
						list.Add(iface);
					}
				}
			}
		}

		#endregion
	}
}