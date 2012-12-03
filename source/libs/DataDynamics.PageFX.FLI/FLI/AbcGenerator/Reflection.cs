using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DataDynamics.PageFX.Common;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FLI.ABC;
using DataDynamics.PageFX.FLI.IL;

namespace DataDynamics.PageFX.FLI
{
    internal partial class AbcGenerator
    {
        bool _emitReflection;

        #region GetTypeId
        Dictionary<IType, int> _typeIndex;
        Dictionary<int, IType> _id2type;
        //Used to implement Assembly.InitTypes method
        List<IType> _initTypes;

        public int GetTypeId(IType type)
        {
            if (type == null)
                return -1;

            if (type.IsModuleType())
                return -1;

            if (GenericType.HasGenericParams(type))
                throw new NotSupportedException();

            if (type.TypeKind == TypeKind.Pointer)
                throw new NotSupportedException();
            if (type.TypeKind == TypeKind.Reference)
                throw new NotSupportedException();

            if (_typeIndex == null)
            {
                _typeIndex = new Dictionary<IType, int>();
                _id2type = new Dictionary<int, IType>();
                _initTypes = new List<IType>();
            }

            int index;
            if (_typeIndex.TryGetValue(type, out index))
                return index;

            GetTypeId(type.BaseType);

            if (type.Interfaces != null)
            {
                foreach (var ifaceType in type.Interfaces)
                    GetTypeId(ifaceType);
            }

            var ct = type as ICompoundType;
            if (ct != null)
            {
                GetTypeId(ct.ElementType);
            }

            index = _typeIndex.Count;
            _typeIndex[type] = index;
            _id2type[index] = type;
            _initTypes.Add(type);

            return index;
        }

        IType FindTypeById(int id)
        {
            IType type;
            if (_id2type.TryGetValue(id, out type))
                return type;
            return null;
        }
        #endregion

        #region DefineGetTypeIdMethod
        //Called when GetTypeId method is used.
        private AbcMethod DefineGetTypeIdMethod(IType type, AbcInstance instance)
        {
            if (type == null) return null;
            if (instance == null) return null;
            if (instance.IsNative) return null;
            if (instance.IsInterface) return null;
            if (instance.IsForeign) return null;

            var abc = instance.Abc;
            if (IsSwf)
            {
                if (!((IEnumerable<AbcFile>)sfc.AbcFrames).Contains(abc))
                    return null;
            }
            else
            {
                if (abc != _abc)
                    return null;
            }

            var name = abc.DefineGlobalQName(Const.GetTypeId);
            var trait = instance.Traits.FindMethod(name);
            if (trait != null) return trait.Method;

            var method = instance.DefineVirtualMethod(
                name, AvmTypeCode.Int32,
                code => code.ReturnTypeId(type));

            method.Trait.IsOverride = IsOverrideGetTypeId(type, instance);

            //File.AppendAllText("c:\\GetTypeId.txt", type.FullName + "\n");

            if (type.Is(SystemTypeCode.Exception))
            {
                //DefinePrototype_GetType(instance, type);

                var getTypeId = GetMethod(ObjectMethodId.GetTypeId);
                instance.DefineMethod(
                    getTypeId,
                    code => code.ReturnTypeId(type), true);

                instance.DefineMethod(
                    GetMethod(ObjectMethodId.GetType),
                    code =>
	                    {
		                    code.CallAssemblyGetType(
			                    () =>
				                    {
					                    code.LoadThis();
					                    code.Call(getTypeId);
				                    }
			                    );
		                    code.ReturnValue();
	                    },
                    true);
            }

            return method;
        }

        private bool IsOverrideGetTypeId(IType type, AbcInstance instance)
        {
            if (type.Is(SystemTypeCode.Exception))
                return false;

            var bt = type.BaseType;
            var st = instance.SuperType;
            while (bt != null && st != null)
            {
                if (st.IsObject) return false;
                if (st.IsError) return false;
                var m = DefineGetTypeIdMethod(bt, st);
                if (m != null) return true;
                bt = bt.BaseType;
                st = st.SuperType;
            }
            return false;
        }
        #endregion

        #region DefineGetTypeIdMethods
        void DefineGetTypeIdMethods()
        {
            var list = new List<AbcFile>();
            if (IsSwf)
            {
                list.AddRange(sfc.AbcFrames);
            }
            else
            {
                list.Add(_abc);
            }
            foreach (var abc in list)
            {
                abc.generator = this;
                for (int i = 0; i < abc.Instances.Count; ++i)
                {
#if DEBUG
                    DebugService.DoCancel();
#endif
                    var instance = abc.Instances[i];
                    if (instance.Abc != abc)
                    {
                        throw new InvalidOperationException();
                    }

                    var type = instance.Type;
                    if (type == null) continue;

                    //NOTE: System.Array defines GetType explicitly
                    if (type.Is(SystemTypeCode.Array))
                    {
                        var m = type.Methods.Find("GetType", 0);
                        if (m == null)
                            throw new InvalidOperationException("Unable to find System.Array.GetType method. Invalid corlib.");
                        DefineAbcMethod(m);
                    }
                    else
                    {
                        DefineGetTypeIdMethod(type, instance);
                    }
                }
            }
        }
        #endregion

        #region Define_Assembly_GetTypeNum - InternalCall
        //TODO: Add options to control level of reflection support
        
        AbcMethod Define_Assembly_GetTypeNum(IMethod method, AbcInstance instance)
        {
            _emitReflection = true;
            var m = BeginMethod(method, instance);
            AddLateMethod(m, Finish_Assembly_GetTypeNum);            
            return m;
        }

        void Finish_Assembly_GetTypeNum(AbcCode code)
        {
#if DEBUG
            DebugService.DoCancel();
            DebugService.LogInfo("FinishInitTypes started");
#endif

            DefineInitTypeMethods();
            DefineGetTypeIdMethods();

            code.PushInt(_initTypes.Count);
            code.ReturnValue();
            
#if DEBUG
            DebugService.LogInfo("FinishInitTypes succeeded");
            DebugService.DoCancel();
#endif
        }
        #endregion

        #region Define_Assembly_InitType - InternalCall
        AbcMethod Define_Assembly_InitType(IMethod method, AbcInstance instance)
        {
            _emitReflection = true;
            var m = BeginMethod(method, instance);
            AddLateMethod(m, Finish_Assembly_InitType);
            return m;
        }

        static void Finish_Assembly_InitType(AbcCode code)
        {
            //args: this, type, typeId

            const int argType = 1;
            const int argId = 2;
            
            code.LoadThis();
            code.PushGlobalPackage();
            code.PushString(Const.InitTypePrefix);
            code.GetLocal(argId);
            code.Add(InstructionCode.Add);
            code.GetRuntimeProperty();
            code.CoerceFunction();

            code.LoadThis();
            code.GetLocal(argType);
            code.Add(InstructionCode.Call, 1);
            code.Pop();
            code.ReturnVoid();
        }
        #endregion

        #region DefineInitTypeMethods
        void DefineInitTypeMethod(IType type, int typeId)
        {
            Debug.Assert(typeId >= 0);

            var instance = DefineAbcInstance(CorlibTypes[CorlibTypeId.Assembly]);

            var name = _abc.DefineGlobalQName(Const.InitTypePrefix + typeId);
            var method = new AbcMethod();
            var trait = AbcTrait.CreateMethod(method, name);
            instance.AddTrait(trait, false);
            method.ReturnType = _abc.BuiltinTypes.Void;
            method.AddParam(CreateParam(SystemTypes.Type, "type"));

            var body = new AbcMethodBody(method);
            AddMethod(method);

            var code = new AbcCode(_abc);
            InitTypeInfo(code, type, typeId);
            code.ReturnVoid();

            body.Finish(code);
        }

        void DefineInitTypeMethods()
        {
            for (int i = 0; i < _initTypes.Count; ++i)
            {
#if DEBUG
                DebugService.DoCancel();
#endif
                var type = _initTypes[i];
                if (!(DefineType(type) is AbcInstance))
                    continue;
                DefineInitTypeMethod(type, i);
            }
        }
        #endregion
    }
}