using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Flash.Abc;
using DataDynamics.PageFX.Flash.Avm;
using DataDynamics.PageFX.Flash.Core.CodeGeneration.Corlib;

namespace DataDynamics.PageFX.Flash.Core.CodeGeneration.Builders
{
    //contains casting methods
    internal sealed class CastImpl
    {
	    private readonly AbcGenerator _generator;

		public static CastImpl With(AbcGenerator generator)
		{
			return new CastImpl(generator);
		}

	    private CastImpl(AbcGenerator generator)
		{
			_generator = generator;
		}

	    private AbcFile Abc
	    {
			get { return _generator.Abc; }
	    }

	    private SystemTypes SystemTypes
	    {
			get { return _generator.SystemTypes; }
	    }

		#region CastToImpl
		private AbcMultiname GetAsMethodName(IType type, bool me)
        {
            string name = "as_";
            if (me) name += "me";
            else name += type.GetSigName();
            return Abc.DefineName(QName.PfxPublic(name));
        }

		private AbcMultiname GetCastMethodName(IType type, bool me)
        {
            string name = "cast_to_";
            if (me) name += "me";
            else name += type.GetSigName();
            return Abc.DefineName(QName.PfxPublic(name));
        }

        public AbcMethod CastToImpl(IType type, bool cast)
        {
            if (type == null) return null;

            if (type.IsArray)
                return ToArrayImpl(type, cast);

            if (type.IsNullableInstance())
                return ToNullableImpl(type, cast);

            if (type.IsValueType())
            {
                if (AbcGenConfig.UseCastToValueType)
                    return ToValueTypeImpl(type, cast);
                return null;
            }

            if (type.IsStringInterface())
                return ToStringInterfaceImpl(type, cast);

            if (type.IsGenericArrayInterface())
                return ToGenericArrayInterfaceImpl(type, cast);

            return CastToDefaultImpl(type, cast);
        }
        #endregion

		#region CastToDefaultImpl
		private AbcMethod CastToDefaultImpl(IType type, bool cast)
        {
            const bool me = true;

            if (cast)
            {
                var AS = CastToDefaultImpl(type, false);
                return Impl(type, AS, GetCastMethodName(type, me));
            }

			var instance = _generator.TypeBuilder.BuildInstance(type);
			var typeName = _generator.TypeBuilder.BuildMemberType(type);
            var name = GetAsMethodName(type, me);

	        return instance.DefineMethod(
		        Sig.@static(name, typeName, AvmTypeCode.Object, "value"),
		        code =>
			        {
				        const int value = 1;
				        code.BeginAsMethod();

				        code.Try();
				        code.GetLocal(value);
				        code.Coerce(typeName);
				        code.ReturnValue();

				        code.CatchReturnNull();
			        });
        }
        #endregion

		#region ToArrayImpl
		public AbcMethod ToArrayImpl(IType type, bool cast)
        {
            if (type == null) return null;
            if (!type.IsArray) return null;

            const bool me = false;

            if (cast)
            {
                var AS = ToArrayImpl(type, false);
                return Impl(type, AS, GetCastMethodName(type, me));
            }

			var instance = _generator.Corlib.Array.Instance;
			var typeName = _generator.TypeBuilder.BuildMemberType(type);
            var name = GetAsMethodName(type, me);

	        return instance.DefineMethod(
		        Sig.@static(name, typeName, AvmTypeCode.Object, "value"),
		        code =>
			        {
				        const int value = 1;

				        code.BeginAsMethod();

				        code.Try();

				        code.GetLocal(value);
				        code.Coerce(typeName);

				        code.PushTypeId(type);
				        code.Call(ArrayMethodId.CastTo);

				        code.ReturnValue();

				        code.CatchReturnNull();
			        });
        }
        #endregion

		#region ToGenericArrayInterfaceImpl
		public AbcMethod ToGenericArrayInterfaceImpl(IType type, bool cast)
        {
            if (type == null) return null;
            if (!type.IsGenericArrayInterface())
                return null;

            const bool me = false;

            if (cast)
            {
                var AS = ToGenericArrayInterfaceImpl(type, false);
                return Impl(type, AS, GetCastMethodName(type, me));
            }

			var instance = _generator.Corlib.Array.Instance;
			var typeName = _generator.TypeBuilder.BuildMemberType(type);
            var name = GetAsMethodName(type, me);

	        return instance.DefineMethod(
		        Sig.@static(name, typeName, AvmTypeCode.Object, "value"),
		        code =>
			        {
				        var elemType = type.GenericArguments[0];

				        const int value = 1;

				        code.BeginAsMethod();

				        code.Try();

				        code.If(
					        () =>
						        {
							        code.GetLocal(value);
							        return code.IfNotArray();
						        },
					        () =>
						        {
							        code.GetLocal(value);
							        code.Coerce(type, false);
							        code.ReturnValue();
						        },
					        //array case
					        () => code.If(
						        () =>
							        {
								        code.HasElemType(elemType, value);
								        return code.IfTrue();
							        },
						        () =>
							        {
								        //can cast
								        code.GetLocal(value);
								        code.ReturnValue();
							        },
						        () => code.ReturnNull()
						              )
					        );

				        code.CatchReturnNull();
			        });
        }
        #endregion

		#region ToStringInterfaceImpl
		public AbcMethod ToStringInterfaceImpl(IType type, bool cast)
        {
            if (type == null) return null;

            const bool me = false;

            if (cast)
            {
                var AS = ToStringInterfaceImpl(type, false);
                return Impl(type, AS, GetCastMethodName(type, me));
            }

			var instance = _generator.TypeBuilder.BuildInstance(SystemTypes.String);
            var name = GetAsMethodName(type, me);

	        return instance.DefineMethod(
		        Sig.@static(name, AvmTypeCode.Object, AvmTypeCode.Object, "value"),
		        code =>
			        {
				        const int value = 1;

				        code.BeginAsMethod();

				        code.Try();

				        code.If(
					        () =>
						        {
							        code.GetLocal(value);
							        //code.PushString("String");
							        //code.PushQName(code[AvmTypeCode.String]);
							        //code.Add(InstructionCode.Istypelate);
							        code.Is(AvmTypeCode.String);
							        return code.IfTrue();
						        },
					        () =>
						        {
							        code.GetLocal(value);
							        code.ReturnValue();
						        },
					        () =>
						        {
							        if (type.IsIEnumerableInstance())
							        {
								        var elemType = type.GetTypeArgument(0);

								        //check array
								        code.GetLocal(value);
								        var notArray = code.IfNotArray();

								        code.HasElemType(elemType, value);

								        var notArgArray = code.IfFalse();
								        code.GetLocal(value);
								        code.ReturnValue();

								        var label = code.Label();
								        notArgArray.BranchTarget = label;
								        notArray.BranchTarget = label;
							        }

							        code.GetLocal(value);
							        code.As(Abc.GetTypeName(type, false));
							        code.ReturnValue();
						        }
					        );

				        code.CatchReturnNull();
			        });
        }
        #endregion

		#region ToNullableImpl
		private AbcMethod ToNullableImpl(IType type, bool cast)
        {
            if (!type.IsNullableInstance())
                return null;

            const bool me = true;

            if (cast)
            {
                var AS = ToNullableImpl(type, false);
                return Impl(type, AS, GetCastMethodName(type, me));
            }

			var instance = _generator.TypeBuilder.BuildInstance(type);
            var name = GetAsMethodName(type, me);

			var typeName = _generator.TypeBuilder.BuildMemberType(type);
	        return instance.DefineMethod(
		        Sig.@static(name, typeName, AvmTypeCode.Object, "value"),
		        code =>
			        {
				        const int value = 1;
				        code.BeginAsMethod();

				        code.Try();

				        code.If(
					        //not me?
					        () =>
						        {
							        code.GetLocal(value);
							        code.As(typeName);
							        return code.IfNull();
						        },
					        //not me
					        () =>
						        {
							        code.NewNullable(type, value);
							        code.ReturnValue();
						        },
					        //me
					        () =>
						        {
							        code.GetLocal(value);
							        code.Coerce(typeName);
							        code.ReturnValue();
						        }
					        );

				        code.CatchReturnNull();
			        });
        }
        #endregion

		#region ToValueTypeImpl
		private AbcMethod ToValueTypeImpl(IType type, bool cast)
        {
            if (!type.IsValueType())
                return null;
            if (type.IsNullableInstance())
                return ToNullableImpl(type, cast);

            const bool me = true;

            if (cast)
            {
                var AS = ToValueTypeImpl(type, false);
                return Impl(type, AS, GetCastMethodName(type, me));
            }

			var typeName = _generator.TypeBuilder.BuildInstance(type);
            var instance = typeName;
            var name = GetAsMethodName(type, me);

	        return instance.DefineMethod(
		        Sig.@static(name, typeName, AvmTypeCode.Object, "value"),
		        code =>
			        {
				        const int value = 1;
				        code.BeginAsMethod();

				        code.Try();

						var MyNullable = _generator.TypeBuilder.BuildInstance(_generator.Corlib.MakeNullable(type));

				        code.If(
					        () =>
						        {
							        code.GetLocal(value);
							        code.As(MyNullable);
							        return code.IfNull();
						        },
					        () =>
						        {
							        code.GetLocal(value);
							        code.Coerce(typeName);
							        code.ReturnValue();
						        },
					        () =>
						        {
							        if (type.IsBoxableType())
							        {
								        code.Box(typeName, () => code.GetBoxedValue(value));
							        }
							        else
							        {
								        code.GetBoxedValue(value);
							        }
							        code.ReturnValue();
						        });

				        code.CatchReturnNull();
			        });
        }
        #endregion

        #region DefineCastMethod
        /// <summary>
        /// Creates cast_to_type method via given AS method
        /// </summary>
        /// <param name="type"></param>
        /// <param name="AS"></param>
        /// <returns></returns>
        private AbcMethod Impl(IType type, AbcMethod AS, AbcMultiname name)
        {
            var instance = AS.Instance;

			var typeName = _generator.TypeBuilder.BuildMemberType(type);

	        return instance.DefineMethod(
		        Sig.@static(name, typeName, AvmTypeCode.Object, "value"),
		        code =>
			        {
				        const int value = 1;

				        code.GetLocal(value);
				        code.IfNullReturnNull(1);

				        code.Getlex(instance);
				        code.GetLocal(value);
				        code.Call(AS);
				        code.SetLocal(value);

				        code.GetLocal(value);
				        var notNull = code.IfNotNull(false);
				        code.ThrowInvalidCastException(type);

				        notNull.BranchTarget = code.Label();
				        code.GetLocal(value);
				        code.ReturnValue();
			        });
        }
        #endregion

		#region ToStringImpl
		public AbcMethod ToStringImpl()
        {
			var instance = _generator.TypeBuilder.BuildInstance(SystemTypes.String);
	        return instance.DefineMethod(
		        Sig.@static("cast_to_me", AvmTypeCode.String, AvmTypeCode.Object, "value"),
		        code =>
			        {
				        const int value = 1;

				        code.IfNullReturnNull(value);

				        code.If(
					        () =>
						        {
							        code.GetLocal(value);
							        code.Is(AvmTypeCode.String);
							        return code.IfTrue();
						        },
					        () =>
						        {
							        code.GetLocal(value);
							        code.CoerceString();
							        code.ReturnValue();
						        },
					        () => code.ThrowInvalidCastException()
					        );
			        });
        }
        #endregion
    }
}