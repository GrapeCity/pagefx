//NOTE:
//1. For one-dimensional arrays that aren't zero-based and for multidimensional arrays,
//   the array class provides a Get method.
//2. For one-dimensional arrays that aren't zero-based and for multidimensional arrays,
//   the array class provides a StoreElement method.
//3. One-dimensional arrays that aren't zero-based and multidimensional arrays are created
//   using newobj rather than newarr. More commonly, they are created using the methods
//   of System.Array class in the Base Framework.

using System;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.FLI.ABC;
using DataDynamics.PageFX.FLI.IL;

namespace DataDynamics.PageFX.FLI
{
    internal partial class AbcGenerator
    {
        #region DefineArrayCtor
        private AbcMethod DefineArrayCtor(IMethod method, AbcInstance instance)
        {
            if (!method.IsConstructor) return null;
            if (method.IsStatic) return null;
            var type = method.DeclaringType;
            if (!type.IsArray) return null;

        	var ctor = new AbcMethod
        	           	{
        	           		ReturnType = _abc[AvmTypeCode.Void]
        	           	};
        	DefineParameters(ctor, method);

            var name = _abc.DefineGlobalQName("arrctor_" + type.GetSigName());
            var trait = AbcTrait.CreateMethod(ctor, name);
            instance.Traits.Add(trait);

            var body = new AbcMethodBody(ctor);

            AddMethod(ctor);

            var code = new AbcCode(_abc);

            code.PushThisScope();
            code.ConstructSuper();

            //check arguments
            int n = method.Parameters.Count;
            for (int i = 0; i < n; ++i)
            {
                code.GetLocal(i + 1);
                code.PushInt(0);
                var br = code.If(BranchOperator.GreaterThanOrEqual);
                code.ThrowException(Corlib.Types.ArgumentOutOfRangeException);
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
            InitArrayFields(code, type, elemType, 0);

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
        #endregion

        #region NewArray / 1D Array
        private AbcMethod CreateSystemArraySZ()
        {
            var instance = GetArrayInstance();

            return instance.DefineStaticMethod(
                "__create_sz_array__", instance.Name,
                code =>
                    {
                        const int varSize = 1;
                        const int varArray = 2;

                        code.CreateInstance(instance);
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
                    },
                AvmTypeCode.Int32, "size");
        }

        /// <summary>
        /// Creates single-dimensional array with given element type.
        /// </summary>
        /// <param name="elemType"></param>
        /// <returns></returns>
        public AbcMethod NewArray(IType elemType)
        {
            DefineType(elemType);

            var instance = GetArrayInstance();

            var name = _abc.DefineGlobalQName("newarr_" + elemType.GetSigName());

            return instance.DefineStaticMethod(
                name, instance.Name,
                code =>
                    {
                        const int varSize = 1; //size argument
                        const int varArray = 2;

                        var m = CreateSystemArraySZ();
                        code.LoadThis();
                        code.GetLocal(varSize);
                        code.Call(m);
                        code.SetLocal(varArray);

                        InitArrayFields(code, elemType, varArray);

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
                    },
                AvmTypeCode.Int32, "size");
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

        #region DefineArrayGetter
        private AbcMethod DefineArrayGetter(IMethod method, AbcInstance instance)
        {
            if (method.IsStatic) return null;
            var type = method.DeclaringType;
            if (!type.IsArray) return null;
            if (method.Name != CLRNames.Array.Getter) return null;

            //string name = "Get" + NameUtil.GetParamsString(method);
            var name = GetMethodName(method);
            return instance.DefineInstanceMethod(
                name, method.Type,
                code =>
                    {
                        code.LoadThis();
                        ToFlatIndex(code, method.Parameters.Count, true);
                        code.GetArrayElem(method.Type, false);
                        code.ReturnValue();
                    },
                method);
        }
        #endregion

        #region DefineArraySetter
        private AbcMethod DefineArraySetter(IMethod method, AbcInstance instance)
        {
            if (method.IsStatic) return null;
            var type = method.DeclaringType;
            if (!type.IsArray) return null;
            if (method.Name != CLRNames.Array.Setter) return null;

            var name = GetMethodName(method);
            return instance.DefineInstanceMethod(
                name, method.Type,
                code =>
                    {
                        int n = method.Parameters.Count;
                        code.LoadThis();
                        ToFlatIndex(code, n, false);
                        code.GetLocal(n); //value
                        code.SetArrayElem(false);
                        code.ReturnVoid();
                    },
                method);
        }
        #endregion

        #region DefineArrayAddress
        private AbcMethod DefineArrayAddress(IMethod method, AbcInstance instance)
        {
            if (method.IsStatic) return null;
            var type = method.DeclaringType;
            if (!type.IsArray) return null;
            if (method.Name != CLRNames.Array.Address) return null;

            var elemPtr = DefineElemPtr();

            string name = "GetAddr_" + method.GetParametersSignature(Runtime.Avm);

            return instance.DefineInstanceMethod(
                name, elemPtr.Name,
                code =>
                    {
                        code.Getlex(elemPtr);
                        code.LoadThis(); //arr
                        ToFlatIndex(code, method.Parameters.Count, true);
                        code.Construct(2);
                        code.ReturnValue();
                    },
                method);
        }
        #endregion
        #endregion

        #region InitArrayFields
        AbcMethod GetArrayBoxMethod(IType elemType)
        {
            var m = DefineBoxMethod(elemType);
            if (m != null) return m;
            return DefineStaticCopyMethod(elemType);
        }

        AbcMethod GetArrayUnboxMethod(IType elemType)
        {
            var m = SelectUnboxMethod(elemType, false);
            if (m != null) return m;
            return DefineStaticCopyMethod(elemType);
        }

        public void InitArrayFields(AbcCode code, IType type, IType elemType, Action getArr)
        {
            if (getArr == null)
                throw new ArgumentNullException("getArr");

            int typeIndex = GetTypeId(type);

            getArr();
            
            code.PushInt(typeIndex);
            code.SetProperty("m_type");

            var box = GetArrayBoxMethod(elemType);
            if (box != null)
            {
                getArr();
                code.GetStaticFunction(box);
                code.SetProperty("m_box");
            }

            var unbox = GetArrayUnboxMethod(elemType);
            if (unbox != null)
            {
                getArr();
                code.GetStaticFunction(unbox);
                code.SetProperty("m_unbox");
            }
        }

        public void InitArrayFields(AbcCode code, IType elemType, Action getArr)
        {
            var arrType = TypeFactory.MakeArray(elemType);
            InitArrayFields(code, arrType, elemType, getArr);
        }

        public void InitArrayFields(AbcCode code, IType elemType, int varArray)
        {
            InitArrayFields(code, elemType,
                            () => code.GetLocal(varArray));
        }

        public void InitArrayFields(AbcCode code, IType type, IType elemType, int varArray)
        {
            InitArrayFields(code, type, elemType,
                            () => code.GetLocal(varArray));
        }
        #endregion

        #region DefineSystemArray_GetElemInt64
        public AbcMethod DefineSystemArray_GetElemInt64(IType elemType, bool item)
        {
            if (elemType == null)
                throw new ArgumentNullException("elemType");

            if (!elemType.IsInt64Based())
                throw new ArgumentException("Invalid elem type");

            if (elemType.IsEnum)
                elemType = elemType.ValueType;

            var instance = GetArrayInstance();

            string name = (item ? "get_item_"  : "get_elem_") + elemType.GetSigName();
            var elemTypeName = DefineReturnType(elemType);
            var oppositeType = elemType.Is(SystemTypeCode.Int64) ? SystemTypes.UInt64 : SystemTypes.Int64;
            var oppositeTypeName = DefineReturnType(oppositeType);

            return instance.DefineInstanceMethod(
                name, elemTypeName,
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
                    },
                    AvmTypeCode.Int32, "index");
        }
        #endregion

        #region ImplementArrayInterfaces, ImplementArrayInterface
        static void ImplementArrayInterface(IType iface, Action<IMethod, AbcMethod> methodImpl)
        {
            foreach (var im in iface.Methods)
            {
                var am = im.Tag as AbcMethod;
                if (am != null)
                    methodImpl(im, am);
            }
        }

        void ImplementArrayInterface(IType type)
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
                        DefineArrayGetEnumerator(elemType);
                    }
                    break;

                case CLRNames.Types.ICollectionT:
                    ImplementArrayInterface(type, ImplementArrayICollectionMethod);
                    break;

                case CLRNames.Types.IListT:
                    ImplementArrayInterface(type, ImplementArrayIListMethod);
                    break;
            }
        }
        #endregion

        #region DefineArrayGetEnumerator
        void DefineArrayGetEnumerator(IType elemType)
        {
            var IEnumerable = MakeIEnumerable(elemType);
            DefineAbcInstance(IEnumerable);
            var ifaceMethod = IEnumerable.Methods[0];
            var ifaceAbcMethod = DefineAbcMethod(ifaceMethod);
            var arrayInstance = GetArrayInstance();

        	arrayInstance.DefineMethod(
        		ifaceAbcMethod,
        		code =>
        			{
        				var ArrayEnumerator = DefineArrayEnumerator(elemType);
        				var ctor = ArrayEnumerator.FindConstructor(1);
        				code.NewObject(ctor, () => code.LoadThis());
        				code.ReturnValue();
        			});
        }

        IType DefineArrayEnumerator(IType elemType)
        {
            var ArrayEnumerator = TypeFactory.MakeGenericType(CorlibTypes[GenericTypeId.ArrayEnumeratorT], elemType);
            DefineAbcInstance(ArrayEnumerator);
            foreach (var method in ArrayEnumerator.Methods)
                DefineAbcMethod(method);
            return ArrayEnumerator;
        }
        #endregion

        #region ICollection<T> Implementation in Array
        void ImplementArrayICollectionMethod(IMethod im, AbcMethod am)
        {
            switch (im.Name)
            {
                case "get_Count":
                    GetArrayInstance().DefineMethod(
                        am,
                        code =>
                            {
                                code.LoadThis();
                                code.Call(ArrayMethodId.GetLength);
                                code.ReturnValue();
                            });
                    break;

                case "get_IsReadOnly":
                    GetArrayInstance().DefineMethod(
                        am,
                        code =>
                            {
                                code.PushBool(true);
                                code.ReturnValue();
                            });
                    break;

                case "Add":
                case "Remove":
                case "Clear":
                    GetArrayInstance().DefineMethod(
                        am,
                        code => code.ThrowException(CorlibTypeId.NotSupportedException));
                    break;

                case "CopyTo":
                    GetArrayInstance().DefineMethod(
                        am,
                        code =>
                            {
                                code.LoadThis();
                                code.GetLocal(1);
                                code.GetLocal(2);
                                code.Call(ArrayMethodId.CopyTo);
                                code.ReturnVoid();
                            });
                    break;

                case "Contains":
                    GetArrayInstance().DefineMethod(
                        am,
                        code =>
                            {
                                code.LoadThis();
                                var type = im.Parameters[0].Type;
                                code.BoxVariable(type, 1);
                                code.Call(ArrayMethodId.Contains);
                                code.ReturnValue();
                            });
                    break;
            }
        }
        #endregion

        #region IList<T> Implementation in Array
        private void ImplementArrayIListMethod(IMethod im, AbcMethod am)
        {
            switch (im.Name)
            {
                case "RemoveAt":
                case "Insert":
                    GetArrayInstance().DefineMethod(
                        am,
                        code => code.ThrowException(CorlibTypeId.NotSupportedException));
                    break;

                case "IndexOf":
                    GetArrayInstance().DefineMethod(
                        am,
                        code =>
                            {
                                code.LoadThis();
                                var type = im.Parameters[0].Type;
                                code.BoxVariable(type, 1);
                                code.Call(ArrayMethodId.IndexOf);
                                code.ReturnValue();
                            });
                    break;

                case "get_Item":
                    GetArrayInstance().DefineMethod(
                        am,
                        code =>
                            {
                                var type = im.DeclaringType.GetTypeArgument(0);
                                code.LoadThis();
                                code.GetLocal(1);
                                code.GetArrayElem(type, true);
                                code.ReturnValue();
                            });
                    break;

                case "set_Item":
                    GetArrayInstance().DefineMethod(
                        am,
                        code =>
                            {
                                code.LoadThis();
                                code.GetLocal(1);
                                code.GetLocal(2);
                                code.SetArrayElem(true);
                                code.ReturnVoid();
                            });
                    break;
            }
        }
        #endregion

        #region Utils
        public AbcNamespace PfxNamespace
        {
            get
            {
                if (_nspfx == null)
                {
                    string ns = RootNamespace.MakeFullName(Const.Namespaces.PFX);
                    _nspfx = _abc.DefinePackage(ns);
                }
                return _nspfx;
            }
        }
        AbcNamespace _nspfx;

        public AbcMultiname DefinePfxName(string name, bool member)
        {
            if (member) return _abc.DefinePfxName(name);
            return _abc.DefineQName(PfxNamespace, name);
        }

        public AbcMultiname DefinePfxName(string name)
        {
            return DefinePfxName(name, true);
        }

        public AbcMultiname GetObjectTypeName()
        {
            return DefineAbcInstance(SystemTypes.Object).Name;
        }
        #endregion

        #region InitArray Methods
        public AbcMethod DefineInitArrayMethod(IType elemType)
        {
            if (!InternalTypeExtensions.IsInitArray(elemType)) return null;

            if (elemType.IsEnum)
                elemType = elemType.ValueType;

            var instance = GetArrayInstance();

            string name = "init_" + elemType.GetSigName();
            return instance.DefineStaticMethod(
                name, AvmTypeCode.Void,
                code =>
                    {
                        var init = DefineInitArrayMethod();
                        var f = DefineInitObjectMethod(elemType);

                        code.LoadThis();
                        code.GetLocal(1);
                        code.GetLocal(2);
                        code.Getlex(f.Instance);
                        code.GetProperty(f.TraitName);
                        code.Call(init);
                        code.ReturnVoid();
                    },
                AvmTypeCode.Array, "arr",
                AvmTypeCode.Int32, "size");
        }

        AbcMethod DefineInitArrayMethod()
        {
            var instance = GetArrayInstance();

            return instance.DefineStaticMethod(
                "init_core", AvmTypeCode.Void,
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
                    },
                AvmTypeCode.Array, "arr",
                AvmTypeCode.Int32, "size",
                AvmTypeCode.Function, "f");
        }

        AbcMethod DefineInitObjectMethod(IType type)
        {
            var instance = DefineAbcInstance(type);
            AbcMultiname name;
            if (instance.IsNative)
            {
                instance = GetObjectInstance();
                name = DefinePfxName("initobj_" + type.GetSigName());
            }
            else
            {
                name = DefinePfxName("initobj");
            }
            return instance.DefineStaticMethod(
                name, AvmTypeCode.Object,
                code =>
                {
                    code.InitObject(type);
                    code.ReturnValue();
                });
        }
        #endregion

    }
}