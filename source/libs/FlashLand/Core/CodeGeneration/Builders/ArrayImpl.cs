//NOTE:
//1. For one-dimensional arrays that aren't zero-based and for multidimensional arrays,
//   the array class provides a Get method.
//2. For one-dimensional arrays that aren't zero-based and for multidimensional arrays,
//   the array class provides a StoreElement method.
//3. One-dimensional arrays that aren't zero-based and multidimensional arrays are created
//   using newobj rather than newarr. More commonly, they are created using the methods
//   of System.Array class in the Base Framework.

using System;
using DataDynamics.PageFX.Common;
using DataDynamics.PageFX.Common.Services;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.Abc;
using DataDynamics.PageFX.FlashLand.Core.CodeGeneration.Corlib;
using DataDynamics.PageFX.FlashLand.IL;

namespace DataDynamics.PageFX.FlashLand.Core.CodeGeneration.Builders
{
    internal sealed class ArrayImpl
    {
	    private readonly AbcGenerator _generator;

	    public ArrayImpl(AbcGenerator generator)
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

	    private AbcMethod BuildCtorImpl(IMethod method, AbcInstance instance)
        {
            if (!method.IsConstructor) return null;
            if (method.IsStatic) return null;
            var type = method.DeclaringType;
            if (!type.IsArray) return null;

        	var ctor = new AbcMethod
        	           	{
        	           		ReturnType = Abc[AvmTypeCode.Void]
        	           	};
			_generator.MethodBuilder.BuildParameters(ctor, method);

            var name = Abc.DefineGlobalQName("arrctor_" + type.GetSigName());
            var trait = AbcTrait.CreateMethod(ctor, name);
            instance.Traits.Add(trait);

            var body = new AbcMethodBody(ctor);

	        Abc.AddMethod(ctor);

	        var code = new AbcCode(Abc);

            code.PushThisScope();
            code.ConstructSuper();

            //check arguments
            int n = method.Parameters.Count;
            for (int i = 0; i < n; ++i)
            {
                code.GetLocal(i + 1);
                code.PushInt(0);
                var br = code.If(BranchOperator.GreaterThanOrEqual);
				var exceptionType = _generator.Corlib.GetType(CorlibTypeId.ArgumentOutOfRangeException);
                code.ThrowException(exceptionType);
                br.BranchTarget = code.Label();
            }

            //m_rank = n
            code.LoadThis();
            code.PushInt(n);
            code.SetProperty(Const.Array.Rank);

            int varSize = n + 1;
            for (int i = 0; i < n; ++i)
                code.GetLocal(i + 1);
            for (int i = 1; i < n; ++i)
                code.Add(InstructionCode.Multiply_i);
            code.SetLocal(varSize);

            //init m_value
            code.LoadThis();
            code.CreateArrayVarSize(varSize);
            code.SetProperty(Const.Array.Value);

            //init m_lengths
            code.LoadThis();
            for (int i = 0; i < n; ++i)
                code.GetLocal(i + 1);
            code.Add(InstructionCode.Newarray, n);
            code.SetProperty(Const.Array.Lengths);

            int varDimArr = varSize + 1;

            //init m_dims
            code.CreateArray(n - 1);
            code.SetLocal(varDimArr);

            //1, n, n * (n-1), ..., n * (n-1) * ... * n0
            for (int i = n - 2; i >= 0; --i)
            {
                int leni = i + 2;
                code.GetLocal(varDimArr);
                code.PushInt(i);

                if (i != n - 2)
                {
                    code.GetLocal(varDimArr);
                    code.PushInt(i + 1);
                    code.GetNativeArrayItem();
                    code.CoerceInt32(); //prev

                    code.GetLocal(leni);
                    code.Add(InstructionCode.Multiply_i); //prev * leni                    
                }
                else
                {
                    code.GetLocal(leni);
                }

                code.SetNativeArrayItem();
            }

            code.LoadThis();
            code.GetLocal(varDimArr);
            code.SetProperty(Const.Array.Dims);

            var elemType = type.GetElementType();
            InitFields(code, type, elemType, 0);

            if (InternalTypeExtensions.IsInitArray(elemType))
            {
                code.InitArray(elemType,
                               () =>
	                               {
		                               code.LoadThis();
		                               code.GetProperty(Const.Array.Value);
	                               }, varSize);
            }

            code.ReturnVoid();

            body.Finish(code);

            return ctor;
        }

		private AbcInstance Instance
		{
			get { return _generator.Corlib.Array.Instance; }
		}

	    #region NewArray / 1D Array
        private AbcMethod CreateSystemArraySZ()
        {
	        return Instance.DefineMethod(
				Sig.@static("__create_sz_array__", Instance.Name, AvmTypeCode.Int32, "size"),
                code =>
                    {
                        const int varSize = 1;
                        const int varArray = 2;

                        code.CreateInstance(Instance);
                        code.SetLocal(varArray);

                        code.GetLocal(varArray);
                        code.CreateArrayVarSize(varSize);
                        code.SetProperty(Const.Array.Value);

                        //NOTE: explicitly set rank because we did not call default ctor for System.Array
                        code.GetLocal(varArray);
                        code.PushInt(1);
                        code.SetProperty(Const.Array.Rank);

                        code.GetLocal(varArray);
                        code.ReturnValue();
                    });
        }

	    /// <summary>
        /// Creates single-dimensional array with given element type.
        /// </summary>
        /// <param name="elemType"></param>
        /// <returns></returns>
        public AbcMethod NewArray(IType elemType)
        {
			_generator.TypeBuilder.Build(elemType);

		    var name = Abc.DefineGlobalQName("newarr_" + elemType.GetSigName());

	        return Instance.DefineMethod(
		        Sig.@static(name, Instance.Name, AvmTypeCode.Int32, "size"),
		        code =>
			        {
				        const int varSize = 1; //size argument
				        const int varArray = 2;

				        var m = CreateSystemArraySZ();
				        code.LoadThis();
				        code.GetLocal(varSize);
				        code.Call(m);
				        code.SetLocal(varArray);

				        InitFields(code, elemType, varArray);

				        if (InternalTypeExtensions.IsInitArray(elemType))
				        {
					        code.InitArray(elemType,
					                       () =>
						                       {
							                       code.GetLocal(varArray);
							                       code.GetProperty(Const.Array.Value);
						                       }, varSize);
				        }

				        code.GetLocal(varArray);
				        code.ReturnValue();
			        });
        }
        #endregion

        #region Accessors
        private static void ToFlatIndex(AbcCode code, int n, bool getter)
        {
            code.LoadThis();
            if (getter)
            {
                code.LoadArguments(n);
                code.Add(InstructionCode.Newarray, n);
            }
            else
            {
                //Last argument is a value
                code.GetLocals(1, n - 1);
                code.Add(InstructionCode.Newarray, n - 1);
            }
            code.Call(ArrayMethodId.ToFlatIndex);
        }

	    private AbcMethod BuildGetterImpl(IMethod method, AbcInstance instance)
        {
            if (method.IsStatic) return null;
            var type = method.DeclaringType;
            if (!type.IsArray) return null;
            if (method.Name != CLRNames.Array.Getter) return null;

            //string name = "Get" + NameUtil.GetParamsString(method);
			var name = _generator.MethodBuilder.DefineQName(method);
	        return instance.DefineMethod(
		        Sig.@this(name, method.Type, method),
		        code =>
			        {
				        code.LoadThis();
				        ToFlatIndex(code, method.Parameters.Count, true);
				        code.GetArrayElem(method.Type, false);
				        code.ReturnValue();
			        });
        }

	    private AbcMethod BuildSetterImpl(IMethod method, AbcInstance instance)
        {
            if (method.IsStatic) return null;
            var type = method.DeclaringType;
            if (!type.IsArray) return null;
            if (method.Name != CLRNames.Array.Setter) return null;

			var name = _generator.MethodBuilder.DefineQName(method);
	        return instance.DefineMethod(
		        Sig.@this(name, method.Type, method),
		        code =>
			        {
				        int n = method.Parameters.Count;
				        code.LoadThis();
				        ToFlatIndex(code, n, false);
				        code.GetLocal(n); //value
				        code.SetArrayElem(false);
				        code.ReturnVoid();
			        });
        }

	    private AbcMethod BuildAddressImpl(IMethod method, AbcInstance instance)
        {
            if (method.IsStatic) return null;
            var type = method.DeclaringType;
            if (!type.IsArray) return null;
            if (method.Name != CLRNames.Array.Address) return null;

			var elemPtr = _generator.Pointers.ElemPtr.Instance;

            string name = "GetAddr_" + method.GetParametersSignature(Runtime.Avm);

	        return instance.DefineMethod(
		        Sig.@this(name, elemPtr.Name, method),
		        code =>
			        {
				        code.Getlex(elemPtr);
				        code.LoadThis(); //arr
				        ToFlatIndex(code, method.Parameters.Count, true);
				        code.Construct(2);
				        code.ReturnValue();
			        });
        }

	    #endregion

	    private AbcMethod GetBoxMethod(IType elemType)
        {
			var m = _generator.Boxing.Box(elemType);
            if (m != null) return m;
            return CopyImpl.With(_generator).StaticCopy(elemType);
        }

        private AbcMethod GetUnboxMethod(IType elemType)
        {
			var m = _generator.Boxing.Unbox(elemType, false);
            if (m != null) return m;
            return CopyImpl.With(_generator).StaticCopy(elemType);
        }

		private void InitFields(AbcCode code, IType type, IType elemType, Action getArr)
        {
            if (getArr == null)
                throw new ArgumentNullException("getArr");

			int typeIndex = _generator.Reflection.GetTypeId(type);

            getArr();
            
            code.PushInt(typeIndex);
            code.SetProperty("m_type");

            var box = GetBoxMethod(elemType);
            if (box != null)
            {
                getArr();
                code.GetStaticFunction(box);
                code.SetProperty("m_box");
            }

            var unbox = GetUnboxMethod(elemType);
            if (unbox != null)
            {
                getArr();
                code.GetStaticFunction(unbox);
                code.SetProperty("m_unbox");
            }
        }

		private void InitFields(AbcCode code, IType elemType, Action getArr)
        {
			var arrType = _generator.TypeFactory.MakeArray(elemType);
            InitFields(code, arrType, elemType, getArr);
        }

		private void InitFields(AbcCode code, IType elemType, int varArray)
        {
            InitFields(code, elemType, () => code.GetLocal(varArray));
        }

        private void InitFields(AbcCode code, IType type, IType elemType, int varArray)
        {
            InitFields(code, type, elemType, () => code.GetLocal(varArray));
        }

		private AbcMultiname BuildReturnType(IType type)
		{
			return _generator.TypeBuilder.BuildReturnType(type);
		}

	    public AbcMethod GetElemInt64(IType elemType, bool item)
        {
            if (elemType == null)
                throw new ArgumentNullException("elemType");

            if (!elemType.IsInt64Based())
                throw new ArgumentException("Invalid elem type");

            if (elemType.IsEnum)
                elemType = elemType.ValueType;

		    string name = (item ? "get_item_"  : "get_elem_") + elemType.GetSigName();
			var elemTypeName = BuildReturnType(elemType);
			var oppositeType = elemType.Is(SystemTypeCode.Int64) ? SystemTypes.UInt64 : SystemTypes.Int64;
			var oppositeTypeName = BuildReturnType(oppositeType);

	        return Instance.DefineMethod(
				Sig.@this(name, elemTypeName, AvmTypeCode.Int32, "index"),
		        code =>
			        {
				        const int index = 1;
				        const int value = 2;

				        code.LoadThis();
				        code.GetLocal(index);
				        code.Call(item ? ArrayMethodId.GetItem : ArrayMethodId.GetElem);
				        code.CoerceObject();
				        code.SetLocal(value);

				        //check elemType
				        code.GetLocal(value);
				        code.As(elemTypeName);
				        code.PushNull();
				        var ifElemType = code.IfNotEquals();

				        //check opposite type
				        code.GetLocal(value);
				        code.As(oppositeTypeName);
				        code.PushNull();
				        var ifOppositeType = code.IfNotEquals();

				        code.ThrowInvalidCastException("not int64");

				        //casting
				        var labelCast = code.Label();
				        ifOppositeType.BranchTarget = labelCast;
				        //gotoCast.BranchTarget = labelCast;
				        code.GetLocal(value);
				        code.Cast(oppositeType, elemType);
				        code.ReturnValue();

				        var normalExit = code.Label();
				        ifElemType.BranchTarget = normalExit;

				        code.GetLocal(value);
				        code.Coerce(elemTypeName);
				        code.ReturnValue();
			        });
        }

	    internal void ImplementInterface(IType type)
        {
            if (type == null) return;
            if (!type.IsInterface) return;
            //if (!TypeService.IsGenericArrayInterface(type)) return;
            var gi = type as IGenericInstance;
            if (gi == null) return;
            if (gi.GenericArguments.Count != 1) return;

            string fn = gi.Type.FullName;
            switch (fn)
            {
                case CLRNames.Types.IEnumerableT:
                    {
                        var elemType = type.GetTypeArgument(0);
                        elemType.HasIEnumerableInstance = true;
                        GetEnumeratorImpl(elemType);
                    }
                    break;

                case CLRNames.Types.ICollectionT:
                    ArrayICollectionImpl.Implement(Instance, type);
                    break;

                case CLRNames.Types.IListT:
					ArrayIListImpl.Implement(Instance, type);
                    break;
            }
        }

	    private void GetEnumeratorImpl(IType elemType)
        {
			var iface = _generator.Corlib.MakeIEnumerable(elemType);
			_generator.TypeBuilder.BuildInstance(iface);
            var ifaceMethod = iface.Methods[0];
			var ifaceAbcMethod = _generator.MethodBuilder.BuildAbcMethod(ifaceMethod);

		    Instance.DefineMethod(
		        Sig.@from(ifaceAbcMethod),
		        code =>
			        {
				        var implType = BuildArrayEnumerator(elemType);
				        var ctor = implType.FindConstructor(1);
				        code.NewObject(ctor, () => code.LoadThis());
				        code.ReturnValue();
			        });
        }

        private IType BuildArrayEnumerator(IType elemType)
        {
			var type = _generator.Corlib.MakeInstance(GenericTypeId.ArrayEnumeratorT, elemType);
			_generator.TypeBuilder.BuildInstance(type);
            foreach (var method in type.Methods)
				_generator.MethodBuilder.BuildAbcMethod(method);
            return type;
        }

	    #region InitArray Methods
        public AbcMethod InitImpl(IType elemType)
        {
            if (!InternalTypeExtensions.IsInitArray(elemType)) return null;

            if (elemType.IsEnum)
                elemType = elemType.ValueType;

	        string name = "init_" + elemType.GetSigName();
	        return Instance.DefineMethod(
		        Sig.@static(name, AvmTypeCode.Void,
		                         AvmTypeCode.Array, "arr",
		                         AvmTypeCode.Int32, "size"),
		        code =>
			        {
				        var init = InitImpl();
				        var f = DefineInitObjectMethod(elemType);

				        code.LoadThis();
				        code.GetLocal(1);
				        code.GetLocal(2);
				        code.Getlex(f.Instance);
				        code.GetProperty(f.TraitName);
				        code.Call(init);
				        code.ReturnVoid();
			        });
        }

        private AbcMethod InitImpl()
        {
	        return Instance.DefineMethod(
		        Sig.@static("init_core", AvmTypeCode.Void,
		                         AvmTypeCode.Array, "arr",
		                         AvmTypeCode.Int32, "size",
		                         AvmTypeCode.Function, "f"),
		        code =>
			        {
				        const int arr = 1;
				        const int size = 2;
				        const int func = 3;

				        code.While(
					        () =>
						        {
							        code.GetLocal(size);
							        code.Add(InstructionCode.Decrement_i);
							        code.SetLocal(size);
							        code.GetLocal(size);
							        code.Add(InstructionCode.Pushbyte, 0);
							        return code.If(BranchOperator.LessThan);
						        },
					        () =>
						        {
							        code.GetLocal(arr);
							        code.GetLocal(size);
							        code.GetLocal(func);
							        code.PushNull();
							        code.CallClosure(0);
							        code.SetNativeArrayItem();
						        }
					        );

				        code.ReturnVoid();
			        });
        }

        private AbcMethod DefineInitObjectMethod(IType type)
        {
			var instance = _generator.TypeBuilder.BuildInstance(type);
            AbcMultiname name;
            if (instance.IsNative)
            {
				instance = _generator.Corlib.Object.Instance;
	            name = _generator.DefinePfxName("initobj_" + type.GetSigName());
            }
            else
            {
				name = _generator.DefinePfxName("initobj");
            }
	        return instance.DefineMethod(
		        Sig.@static(name, AvmTypeCode.Object),
		        code =>
			        {
				        code.InitObject(type);
				        code.ReturnValue();
			        });
        }
        #endregion

		public AbcMethod BuildSpecMethod(IMethod method, AbcInstance instance)
		{
			return BuildCtorImpl(method, instance)
				   ?? BuildGetterImpl(method, instance)
				   ?? BuildSetterImpl(method, instance)
				   ?? BuildAddressImpl(method, instance);
		}
    }
}