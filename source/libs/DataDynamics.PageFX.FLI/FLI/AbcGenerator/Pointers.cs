using System;
using System.Collections;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FLI.ABC;

namespace DataDynamics.PageFX.FLI
{
    //contains generation of pointer classes
    partial class AbcGenerator
    {
        #region DefineFuncPtr
        public AbcInstance DefineFuncPtr()
        {
            if (_ptrFunc != null) return _ptrFunc;

            var instance = _abc.DefineEmptyInstance(DefinePfxName("func_ptr", false), false);
            _ptrFunc = instance;
            
            var getter = instance.CreatePrivateSlot("_getter", AvmTypeCode.Function);
            var setter = instance.CreatePrivateSlot("_setter", AvmTypeCode.Function);

            instance.Initializer = _abc.DefineTraitsInitializer(getter, setter);

            instance.DefinePtrGetter(
                code =>
                {
                    code.LoadThis();
                    code.GetProperty(getter);
                    code.PushNull();
                    code.CallFunction(1); //this
                    code.ReturnValue();
                });

            instance.DefinePtrSetter(
                code =>
                {
                    code.LoadThis();
                    code.GetProperty(setter);
                    code.PushNull();
                    code.GetLocal(1); //value
                    code.CallFunction(2); //this + value
                    code.Pop();
                    code.ReturnVoid();
                });

            return instance;
        }

        AbcInstance _ptrFunc;
        #endregion

        #region DefineStaticFieldPtr
        public AbcInstance DefineStaticFieldPtr(IField field)
        {
            if (field == null)
                throw new ArgumentNullException("field");
            if (!field.IsStatic)
                throw new ArgumentException("field is not static");

            var type = field.DeclaringType;
            string typeName = type.FullName.MakeFullName(field.Name);
            var name = _abc.DefinePfxName("sfld_ptr$" + typeName);

            var instance = _abc.Instances[name];
            if (instance != null)
                return instance;

            var fieldInstance = DefineAbcInstance(type);

            instance = _abc.DefineEmptyInstance(name, true);
            
            instance.DefinePtrGetter(
                code =>
                {
                    code.Getlex(fieldInstance);
                    code.GetField(field);
                    code.ReturnValue();
                });

            instance.DefinePtrSetter(
                code =>
                {
                    code.Getlex(fieldInstance);
                    code.GetLocal(1); //value
                    code.SetField(field);
                    code.ReturnVoid();
                });

            var ti = instance.CreatePrivateStaticSlot("_instance", instance);

            instance.DefineStaticGetter(
                Const.Instance, instance.Name,
                code =>
                {
                    code.LoadThis();
                    code.GetProperty(ti);
                    var br = code.IfNotNull(true);

                    code.LoadThis();
                    code.LoadThis();
                    code.Construct(0);
                    code.SetProperty(ti);

                    br.BranchTarget = code.Label();
                    code.LoadThis();
                    code.GetProperty(ti);
                    code.ReturnValue();
                });

            return instance;
        }
        #endregion

        #region DefinePropertyPtr
        AbcInstance _ptrProperty;

        public AbcInstance DefinePropertyPtr()
        {
            if (_ptrProperty != null) return _ptrProperty;

            var instance = _abc.DefineEmptyInstance(DefinePfxName("prop_ptr", false), false);
            _ptrProperty = instance;

            var obj = instance.CreatePrivateSlot("_obj", AvmTypeCode.Object);
            var ns = instance.CreatePrivateSlot("_ns", AvmTypeCode.Namespace);
            var name = instance.CreatePrivateSlot("_name", AvmTypeCode.String);

            instance.Initializer = _abc.DefineTraitsInitializer(obj, ns, name);

            instance.DefinePtrGetter(
                code =>
                {
                    code.LoadThis();
                    code.GetProperty(obj);
                    code.LoadThis();
                    code.GetProperty(ns);
                    code.LoadThis();
                    code.GetProperty(name);
                    code.GetRuntimeProperty();
                    code.ReturnValue();
                });

            instance.DefinePtrSetter(
                code =>
                {
                    code.LoadThis();
                    code.GetProperty(obj);
                    code.LoadThis();
                    code.GetProperty(ns);
                    code.LoadThis();
                    code.GetProperty(name);
                    code.GetLocal(1); //value
                    code.SetRuntimeProperty();
                    code.ReturnVoid();
                });

            return instance;
        }
        #endregion

        #region DefineGlobalPropertyPtr
        AbcInstance _ptrGlobalProperty;

        public AbcInstance DefineGlobalPropertyPtr()
        {
            if (_ptrGlobalProperty != null) return _ptrGlobalProperty;

            var instance  = _abc.DefineEmptyInstance(DefinePfxName("gprop_ptr", false), false);
            _ptrGlobalProperty = instance;

            var obj = instance.CreatePrivateSlot("_obj", AvmTypeCode.Object);
            var name = instance.CreatePrivateSlot("_name", AvmTypeCode.String);

            instance.Initializer = _abc.DefineTraitsInitializer(obj, name);

            instance.DefinePtrGetter(
                code =>
                {
                    code.LoadThis();
                    code.GetProperty(obj);
                    code.PushGlobalPackage();
                    code.LoadThis();
                    code.GetProperty(name);
                    code.GetRuntimeProperty();
                    code.ReturnValue();
                });

            instance.DefinePtrSetter(
                code =>
                {
                    code.LoadThis();
                    code.GetProperty(obj);
                    code.PushGlobalPackage();
                    code.LoadThis();
                    code.GetProperty(name);
                    code.GetLocal(1); //value
                    code.SetRuntimeProperty();
                    code.ReturnVoid();
                });

            return instance;
        }
        #endregion

        #region DefineFieldPtr(IField field)
        public AbcInstance DefineFieldPtr(IField field)
        {
            if (field.IsStatic)
                return DefineStaticFieldPtr(field);
            var name = GetFieldName(field);
            return DefinePropertyPtr(name);
        }

        public AbcInstance DefinePropertyPtr(AbcMultiname name)
        {
            if (name.Namespace.IsGlobalPackage)
                return DefineGlobalPropertyPtr();
            return DefinePropertyPtr();
        }
        #endregion

        #region DefineElemPtr
        AbcInstance _ptrElem;

        public AbcInstance DefineElemPtr()
        {
            if (_ptrElem != null) return _ptrElem;

            var instance = _abc.DefineEmptyInstance(DefinePfxName("elem_ptr", false), false);
            _ptrElem = instance;
            
            var arrType = GetArrayInstance();
            var arr = instance.CreatePrivateSlot("_arr", arrType);
            var index = instance.CreatePrivateSlot("_index", AvmTypeCode.Int32);

            instance.Initializer = _abc.DefineTraitsInitializer(arr, index);

            instance.DefinePtrGetter(
                code =>
                {
                    code.LoadThis();
                    code.GetProperty(arr);
                    code.LoadThis();
                    code.GetProperty(index);
                    code.GetArrayElem(false);
                    code.ReturnValue();
                });

            instance.DefinePtrSetter(
                code =>
                {
                    code.LoadThis();
                    code.GetProperty(arr);
                    code.LoadThis();
                    code.GetProperty(index);
                    code.GetLocal(1); //value
                    code.SetArrayElem(false);
                    code.ReturnVoid();
                });

            return instance;
        }
        

        public AbcMethod DefineGetElemPtr()
        {
            var instance = GetArrayInstance();

            var name = DefinePfxName("GetElemPtr");
            return instance.DefineInstanceMethod(
                name, AvmTypeCode.Object,
                code =>
                    {
                        var ptr = DefineElemPtr();
                        code.Getlex(ptr);
                        code.LoadThis();
                        code.GetLocal(1); //index
                        code.Construct(2);
                        code.ReturnValue();
                    },
                AvmTypeCode.Int32, "index");
        }
        #endregion

        #region DefineSlotPtr
        readonly Hashtable _slotPtrs = new Hashtable();

        public AbcInstance DefineSlotPtr(AbcTrait slot)
        {
            if (slot == null)
                throw new ArgumentNullException("slot");

            //NOTE: VerifyError: Error #1026: Slot 1 exceeds slotCount=0 of Object
            //therefore we can not use Get/Set slots by slot_id.

            string name = "slot_ptr$" + slot.NameString;

            var instance = _slotPtrs[name] as AbcInstance;
            if (instance != null) return instance;

            instance = _abc.DefineEmptyInstance(DefinePfxName(name, false), false);
            _slotPtrs[name] = instance;
            
            var obj = instance.CreatePrivateSlot("_obj", AvmTypeCode.Object);
            
            instance.Initializer = _abc.DefineTraitsInitializer(obj);

            instance.DefinePtrGetter(
                code =>
                {
                    code.LoadThis();
                    code.GetProperty(obj);
                    code.GetProperty(slot);
                    code.ReturnValue();
                });

            instance.DefinePtrSetter(
                code =>
                {
                    code.LoadThis();
                    code.GetProperty(obj);
                    code.GetLocal(1); //value
                    code.SetProperty(slot);
                    code.ReturnVoid();
                });

            return instance;
        }
        #endregion

        #region DefineFunction_GetProperty, DefineFunction_SetProperty
        //readonly Hashtable _cacheGPFuncs = new Hashtable();
        //readonly Hashtable _cacheSPFuncs = new Hashtable();

        public AbcMethod DefineFunction_GetProperty(AbcMultiname prop)
        {
            if (prop == null)
                throw new ArgumentNullException("prop");

            //var m = _cacheGPFuncs[prop] as AbcMethod;
            //if (m != null) return m;

            var m = _abc.DefineMethod(
                AvmTypeCode.Object,
                code =>
                    {
                        code.Getlex(prop);
                        code.ReturnValue();
                    });

#if DEBUG
            m.Name = _abc.DefineString("get_" + prop.NameString);
#endif

            //_cacheGPFuncs[prop] = m;

            return m;
        }

        public AbcMethod DefineFunction_SetProperty(AbcMultiname prop)
        {
            if (prop == null)
                throw new ArgumentNullException("prop");

            //var m = _cacheSPFuncs[prop] as AbcMethod;
            //if (m != null) return m;

            var m = _abc.DefineMethod(
                AvmTypeCode.Void,
                code =>
                    {
                        code.FindProperty(prop);
                        code.GetLocal(1); //value
                        code.SetProperty(prop);
                        code.ReturnVoid();
                    },
                AvmTypeCode.Object, "value");

#if DEBUG
            m.Name = _abc.DefineString("set_" + prop.NameString);
#endif

            //_cacheSPFuncs[prop] = m;

            return m;
        }
        #endregion
    }
}