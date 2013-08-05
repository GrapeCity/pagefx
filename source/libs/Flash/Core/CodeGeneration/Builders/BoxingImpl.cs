using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Flash.Abc;
using DataDynamics.PageFX.Flash.Avm;
using DataDynamics.PageFX.Flash.Core.CodeGeneration.Corlib;
using DataDynamics.PageFX.Flash.IL;

namespace DataDynamics.PageFX.Flash.Core.CodeGeneration.Builders
{
	//TODO: make static

    internal sealed class BoxingImpl
    {
	    private readonly AbcGenerator _generator;

	    public BoxingImpl(AbcGenerator generator)
		{
			_generator = generator;
		}

	    private SystemTypes SystemTypes
	    {
			get { return _generator.SystemTypes; }
	    }

		#region Box Impl

		public AbcMethod Box(IType type)
        {
            if (type.IsNullableInstance())
                return BoxNullable(type);

            return BoxPrimitive(type);
        }

	    private AbcMultiname BoxName
	    {
			get { return _generator.Abc.DefineName(QName.PfxPublic(Const.Boxing.MethodBox)); }
	    }

	    private AbcMethod BoxPrimitive(IType type)
        {
            if (!type.IsBoxableType())
                return null;

			var instance = _generator.TypeBuilder.BuildInstance(type);

	        return instance.DefineMethod(
		        Sig.@static(BoxName, instance.Name, type, "value"),
		        code =>
			        {
				        code.BoxVariable(instance, 1);
				        code.ReturnValue();
			        });
        }

	    private AbcMethod BoxNullable(IType type)
        {
            if (!type.IsNullableInstance())
                return null;

			var instance = _generator.TypeBuilder.BuildInstance(type);

            var arg = type.GetTypeArgument(0);
			var argInstance = _generator.TypeBuilder.BuildInstance(arg);

	        return instance.DefineMethod(
		        Sig.@static(BoxName, argInstance.Name, type, "value"),
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

		public AbcMethod Unbox(IType type, bool strict)
        {
            if (type.IsNullableInstance())
                return UnboxNullable(type);
            var m = UnboxImpl(type, strict);
            if (m != null) return m;
            return UnboxStruct(type);
        }

	    private AbcMultiname UnboxName
	    {
		    get { return _generator.Abc.DefineName(QName.PfxPublic(Const.Boxing.MethodUnbox)); }
	    }

	    private AbcMethod UnboxImpl(IType type, bool strict)
        {
            if (!type.IsBoxableOrInt64Based())
                return null;

			var instance = _generator.TypeBuilder.BuildInstance(type);

            var vtype = type;
            if (type.IsEnum)
                vtype = type.ValueType;

            bool isInt64 = vtype.IsInt64();
            if (type.IsEnum && !strict)
                type = vtype;

			var name = _generator.Abc.DefineName(QName.PfxPublic(Const.Boxing.MethodUnbox + (strict ? "strict" : "")));
			var retType = _generator.TypeBuilder.BuildMemberType(type);
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
					        () =>
						        {
							        code.GetLocal(varType);
							        return code.IfNull();
						        },
					        () => TryUnboxNumber(code, type),
					        () =>
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
			_generator.MethodBuilder.BuildAbcMethod(SystemTypes.Int64, "get_m_value", 0);
			_generator.MethodBuilder.BuildAbcMethod(SystemTypes.UInt64, "get_m_value", 0);
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

	    private AbcMethod UnboxNullable(IType type)
        {
            if (!type.IsNullableInstance())
                return null;

			var instance = _generator.TypeBuilder.BuildInstance(type);

            var name = UnboxName;

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

	    private AbcMethod UnboxStruct(IType type)
        {
            if (type == null) return null;
            if (type.TypeKind != TypeKind.Struct) return null;

			var instance = _generator.TypeBuilder.BuildInstance(type);

            var name = UnboxName;

	        return instance.DefineMethod(
		        Sig.@static(name, instance, AvmTypeCode.Object, "value"),
		        code =>
			        {
				        const int value = 1;

				        code.GetLocal(value);
				        code.ThrowNullReferenceException();

						var MyNullable = _generator.TypeBuilder.BuildInstance(_generator.Corlib.MakeNullable(type));

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
    }
}