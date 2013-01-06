using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.Abc;
using DataDynamics.PageFX.FlashLand.Core.CodeGeneration.Corlib;
using DataDynamics.PageFX.FlashLand.IL;

namespace DataDynamics.PageFX.FlashLand.Core.CodeGeneration
{
    internal partial class AbcGenerator
    {
        #region DefineBoxMethod
        public AbcMethod DefineBoxMethod(IType type)
        {
            if (type.IsNullableInstance())
                return DefineBoxNullable(type);

            return DefineBoxPrimitive(type);
        }
        #endregion

        #region DefineBoxPrimitive
        AbcMethod DefineBoxPrimitive(IType type)
        {
            if (!type.IsBoxableType())
                return null;

            var instance = DefineAbcInstance(type);
            var name = DefinePfxName(Const.Boxing.MethodBox);

	        return instance.DefineMethod(
		        Sig.@static(name, instance.Name, type, "value"),
		        code =>
			        {
				        code.BoxVariable(instance, 1);
				        code.ReturnValue();
			        });
        }
        #endregion

        #region DefineBoxNullable
        private AbcMethod DefineBoxNullable(IType type)
        {
            if (!type.IsNullableInstance())
                return null;

            var instance = DefineAbcInstance(type);
            var name = DefinePfxName(Const.Boxing.MethodBox);

            var arg = type.GetTypeArgument(0);
            var argInstance = DefineAbcInstance(arg);

	        return instance.DefineMethod(
		        Sig.@static(name, argInstance.Name, type, "value"),
		        code =>
			        {
				        const int value = 1;
				        code.If(
					        //hasValue?
					        () =>
						        {
							        code.GetLocal(value);
							        code.Nullable_HasValue(true);
							        return code.IfTrue();
						        },
					        () =>
						        {
							        code.Box(arg, () => code.GetBoxedValue(value));
							        code.ReturnValue();
						        },
					        () => code.ReturnNull()
					        );
			        });
        }
        #endregion

        #region SelectUnboxMethod
        public AbcMethod SelectUnboxMethod(IType type, bool strict)
        {
            if (type.IsNullableInstance())
                return DefineUnboxNullable(type);
            var m = DefineUnboxMethod(type, strict);
            if (m != null) return m;
            return DefineUnboxStruct(type);
        }
        #endregion

        #region DefineUnboxMethod
        private AbcMultiname GetUnboxMethodName()
        {
            return DefinePfxName(Const.Boxing.MethodUnbox);
        }

        public AbcMethod DefineUnboxMethod(IType type, bool strict)
        {
            if (!type.IsBoxableOrInt64Based())
                return null;

            var instance = DefineAbcInstance(type);

            var vtype = type;
            if (type.IsEnum)
                vtype = type.ValueType;

            bool isInt64 = vtype.IsInt64();
            if (type.IsEnum && !strict)
                type = vtype;

            var name = DefinePfxName(Const.Boxing.MethodUnbox + (strict ? "strict" : ""));
            var retType = DefineMemberType(type);
	        return instance.DefineMethod(
		        Sig.@static(name, retType, AvmTypeCode.Object, "value"),
		        code =>
			        {
				        const int varValue = 1;
				        const int varType = 2;
				        const int varKind = 3;

				        if (isInt64)
					        EnsureInt64Members();

				        //NOTE: Check null to throw cast exception
				        code.GetLocal(varValue);
				        code.ThrowNullReferenceException();

				        code.GetLocal(varValue);
				        code.CallGetType();
				        code.SetLocal(varType);

				        code.If(
					        delegate
						        {
							        code.GetLocal(varType);
							        return code.IfNull();
						        },
					        () => TryUnboxNumber(code, type),
					        delegate
						        {
							        if (strict)
							        {
								        code.GetLocal(varType);
								        code.TypeOf(type);
								        var ifType = code.Add(InstructionCode.Ifstricteq);
								        code.ThrowInvalidCastException("not " + type.FullName);
								        ifType.BranchTarget = code.Label();
							        }
							        else
							        {
								        code.GetLocal(varType);
								        code.Call(TypeMethodId.ValueTypeKind);
								        code.SetLocal(varKind);

								        code.GetLocal(varKind);
								        code.PushInt(type.GetCorlibKind());
								        var ifKind = code.IfEquals();
								        code.ThrowInvalidCastException("invalid value type kind (not " + type.FullName +
								                                       ")");

								        ifKind.BranchTarget = code.Label();
							        }

							        code.GetBoxedValue(varValue);
							        code.TryCastToSystemType(null, vtype.SystemType());
							        code.ReturnValue();
						        }
					        );
			        });
        }

        private void EnsureInt64Members()
        {
            DefineAbcMethod(SystemTypes.Int64, "get_m_value", 0);
            DefineAbcMethod(SystemTypes.UInt64, "get_m_value", 0);
        }

        private static void TryUnboxNumber(AbcCode code, IType type)
        {
            const int varValue = 1;
            code.GetLocal(varValue);
            var ifNotNumber = code.IfType(AvmTypeCode.Number);
            code.ThrowInvalidCastException();
            ifNotNumber.BranchTarget = code.Label();
            code.GetLocal(varValue);
            if (!code.TryCastToSystemType(null, type.SystemType()))
                code.Coerce(type, true);
            code.ReturnValue();
        }
        #endregion

        #region DefineUnboxNullable
        public AbcMethod DefineUnboxNullable(IType type)
        {
            if (!type.IsNullableInstance())
                return null;

            var instance = DefineAbcInstance(type);

            var name = GetUnboxMethodName();

            //Rules for operation (T?)obj
            //1. null => new T?();
            //2. if obj is T? => coerce (T?)
            //3. try unbox T

	        return instance.DefineMethod(
		        Sig.@static(name, instance, AvmTypeCode.Object, "value"),
		        code =>
			        {
				        const int value = 1;

				        code.If(
					        () =>
						        {
							        code.GetLocal(value);
							        return code.IfNullable();
						        },
					        () =>
						        {
							        code.CreateInstance(instance);
							        code.ReturnValue();
						        },
					        () => code.If(
						        () =>
							        {
								        code.GetLocal(value);
								        code.As(instance.Name);
								        return code.IfNotNull();
							        },
						        () =>
							        {
								        code.GetLocal(value);
								        code.Coerce(instance.Name);
								        code.ReturnValue();
							        },
						        () =>
							        {
								        code.NewNullable(type, value);
								        code.ReturnValue();
							        })
					        );
			        });
        }
        #endregion

        #region DefineUnboxStruct
        public AbcMethod DefineUnboxStruct(IType type)
        {
            if (type == null) return null;
            if (type.TypeKind != TypeKind.Struct) return null;

            var instance = DefineAbcInstance(type);

            var name = GetUnboxMethodName();

	        return instance.DefineMethod(
		        Sig.@static(name, instance, AvmTypeCode.Object, "value"),
		        code =>
			        {
				        const int value = 1;

				        code.GetLocal(value);
				        code.ThrowNullReferenceException();

				        var MyNullable = DefineAbcInstance(MakeNullable(type));

				        code.If(
					        () =>
						        {
							        code.GetLocal(value);
							        code.As(MyNullable.Name);
							        return code.IfNull();
						        },
					        () =>
						        {
							        code.GetLocal(value);
							        code.Coerce(instance.Name);
							        code.ReturnValue();
						        },
					        () =>
						        {
							        code.GetBoxedValue(value);
							        code.Coerce(instance.Name);
							        code.ReturnValue();
						        }
					        );
			        });
        }
        #endregion
    }
}