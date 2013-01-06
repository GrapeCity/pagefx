using System.Collections;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.Abc;
using DataDynamics.PageFX.FlashLand.Core.CodeGeneration.Corlib;

namespace DataDynamics.PageFX.FlashLand.Core.CodeGeneration
{
    //contains casting methods
    partial class AbcGenerator
    {
        #region DefineCastToMethod
        AbcMultiname GetAsMethodName(IType type, bool me)
        {
            string name = "as_";
            if (me) name += "me";
            else name += type.GetSigName();
            return Abc.DefinePfxName(name);
        }

        AbcMultiname GetCastMethodName(IType type, bool me)
        {
            string name = "cast_to_";
            if (me) name += "me";
            else name += type.GetSigName();
            return Abc.DefinePfxName(name);
        }

        public AbcMethod DefineCastToMethod(IType type, bool cast)
        {
            if (type == null) return null;

            if (type.IsArray)
                return DefineCastToArray(type, cast);

            if (type.IsNullableInstance())
                return DefineCastToNullable(type, cast);

            if (type.IsValueType())
            {
                if (AbcGenConfig.UseCastToValueType)
                    return DefineCastToValueType(type, cast);
                return null;
            }

            if (type.IsStringInterface())
                return DefineCastToStringInterface(type, cast);

            if (type.IsGenericArrayInterface())
                return DefineCastToGenericArrayInterface(type, cast);

            return DefineCastToDefault(type, cast);
        }
        #endregion

        #region DefineCastToDefault
        AbcMethod DefineCastToDefault(IType type, bool cast)
        {
            const bool me = true;

            if (cast)
            {
                var AS = DefineCastToDefault(type, false);
                return DefineCastMethod(type, AS, GetCastMethodName(type, me));
            }

            var instance = DefineAbcInstance(type);
            var typeName = DefineMemberType(type);
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

        #region DefineCastToArray
        public AbcMethod DefineCastToArray(IType type, bool cast)
        {
            if (type == null) return null;
            if (!type.IsArray) return null;

            const bool me = false;

            if (cast)
            {
                var AS = DefineCastToArray(type, false);
                return DefineCastMethod(type, AS, GetCastMethodName(type, me));
            }

            var instance = GetArrayInstance();
            var typeName = DefineMemberType(type);
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

        #region DefineCastToArrayInterface
        public AbcMethod DefineCastToGenericArrayInterface(IType type, bool cast)
        {
            if (type == null) return null;
            if (!type.IsGenericArrayInterface())
                return null;

            const bool me = false;

            if (cast)
            {
                var AS = DefineCastToGenericArrayInterface(type, false);
                return DefineCastMethod(type, AS, GetCastMethodName(type, me));
            }

            var instance = GetArrayInstance();
            var typeName = DefineMemberType(type);
            var name = GetAsMethodName(type, me);

	        return instance.DefineMethod(
		        Sig.@static(name, typeName, AvmTypeCode.Object, "value"),
		        code =>
			        {
				        var gi = (IGenericInstance)type;
				        var elemType = gi.GenericArguments[0];

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

        #region DefineCastToStringInterface
        public AbcMethod DefineCastToStringInterface(IType type, bool cast)
        {
            if (type == null) return null;

            const bool me = false;

            if (cast)
            {
                var AS = DefineCastToStringInterface(type, false);
                return DefineCastMethod(type, AS, GetCastMethodName(type, me));
            }

            var instance = DefineAbcInstance(SystemTypes.String);
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

        #region DefineCastToNullable
        AbcMethod DefineCastToNullable(IType type, bool cast)
        {
            if (!type.IsNullableInstance())
                return null;

            const bool me = true;

            if (cast)
            {
                var AS = DefineCastToNullable(type, false);
                return DefineCastMethod(type, AS, GetCastMethodName(type, me));
            }

            var instance = DefineAbcInstance(type);
            var name = GetAsMethodName(type, me);

            var typeName = DefineMemberType(type);
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

        #region DefineCastToValueType
        AbcMethod DefineCastToValueType(IType type, bool cast)
        {
            if (!type.IsValueType())
                return null;
            if (type.IsNullableInstance())
                return DefineCastToNullable(type, cast);

            const bool me = true;

            if (cast)
            {
                var AS = DefineCastToValueType(type, false);
                return DefineCastMethod(type, AS, GetCastMethodName(type, me));
            }

            var typeName = DefineAbcInstance(type);
            var instance = typeName;
            var name = GetAsMethodName(type, me);

	        return instance.DefineMethod(
		        Sig.@static(name, typeName, AvmTypeCode.Object, "value"),
		        code =>
			        {
				        const int value = 1;
				        code.BeginAsMethod();

				        code.Try();

				        var MyNullable = DefineAbcInstance(MakeNullable(type));

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
        AbcMethod DefineCastMethod(IType type, AbcMethod AS, AbcMultiname name)
        {
            var instance = AS.Instance;

            var typeName = DefineMemberType(type);

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

        #region DefineCastToString
        public AbcMethod DefineCastToString()
        {
            var instance = DefineAbcInstance(SystemTypes.String);
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

        #region Cache of Casting Operators
        private static string GetCastOperatorKey(IType source, IType target)
        {
            var s = source.SystemType();
            if (s == null) return null;
            var t = target.SystemType();
            if (t == null) return null;
            return ((int)s.Code).ToString() + ((int)t.Code);
        }

        public AbcMethod GetCastOperator(IType source, IType target)
        {
            string key = GetCastOperatorKey(source, target);
            if (key == null) return null;
            return _cacheCastOps[key] as AbcMethod;
        }

        public void CacheCastOperator(IType source, IType target, AbcMethod op)
        {
            string key = GetCastOperatorKey(source, target);
            _cacheCastOps[key] = op;
        }

        readonly Hashtable _cacheCastOps = new Hashtable();
        #endregion
    }
}