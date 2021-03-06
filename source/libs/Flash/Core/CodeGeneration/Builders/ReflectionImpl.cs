using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Flash.Abc;
using DataDynamics.PageFX.Flash.Avm;
using DataDynamics.PageFX.Flash.Core.CodeGeneration.Corlib;
using DataDynamics.PageFX.Flash.Core.SwfGeneration;
using DataDynamics.PageFX.Flash.Core.Tools;
using DataDynamics.PageFX.Flash.IL;

namespace DataDynamics.PageFX.Flash.Core.CodeGeneration.Builders
{
    internal sealed partial class ReflectionImpl
    {
	    private readonly AbcGenerator _generator;

		private Dictionary<IType, int> _typeIndex;
		private Dictionary<int, IType> _id2type;
		//Used to implement Assembly.InitTypes method
		private List<IType> _initTypes;

	    private bool _emitReflection;

		public ReflectionImpl(AbcGenerator generator)
		{
			_generator = generator;
		}

	    private AbcFile Abc
	    {
			get { return _generator.Abc; }
	    }

	    private bool IsSwf
	    {
			get { return _generator.IsSwf; }
	    }

	    private SwfCompiler SwfCompiler
	    {
			get { return _generator.SwfCompiler; }
	    }

	    private SystemTypes SystemTypes
	    {
			get { return _generator.SystemTypes; }
	    }

	    #region GetTypeId

        public int GetTypeId(IType type)
        {
            if (type == null)
                return -1;

            if (type.IsModuleType())
                return -1;

            if (type.HasGenericParams())
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

			if (type.BaseType != null)
				GetTypeId(type.BaseType);

            if (type.Interfaces != null)
            {
                foreach (var ifaceType in type.Interfaces)
                    GetTypeId(ifaceType);
            }

	        if (type.ElementType != null)
		        GetTypeId(type.ElementType);

	        index = _typeIndex.Count;
            _typeIndex[type] = index;
            _id2type[index] = type;
            _initTypes.Add(type);

            return index;
        }

        private IType FindTypeById(int id)
        {
            IType type;
            if (_id2type.TryGetValue(id, out type))
                return type;
            return null;
        }
        #endregion

        #region DefineGetTypeIdMethod
        //Called when GetTypeId method is used.
        public AbcMethod DefineGetTypeIdMethod(IType type, AbcInstance instance)
        {
            if (type == null) return null;
            if (instance == null) return null;
            if (instance.IsNative) return null;
            if (instance.IsInterface) return null;
            if (instance.IsForeign) return null;

            var abc = instance.Abc;
            if (IsSwf)
            {
                if (!((IEnumerable<AbcFile>)SwfCompiler.AbcFrames).Contains(abc))
                    return null;
            }
            else
            {
                if (abc != Abc)
                    return null;
            }

	        string name1 = Const.GetTypeId;
	        var name = abc.DefineName(QName.Global(name1));
            var trait = instance.Traits.FindMethod(name);
            if (trait != null) return trait.Method;

	        var method = instance.DefineMethod(
		        Sig.@virtual(name, AvmTypeCode.Int32),
		        code => code.ReturnTypeId(type));

            method.Trait.IsOverride = IsOverrideGetTypeId(type, instance);

            //File.AppendAllText("c:\\GetTypeId.txt", type.FullName + "\n");

            if (type.Is(SystemTypeCode.Exception))
            {
                //DefinePrototype_GetType(instance, type);

				var getTypeId = _generator.Corlib.GetMethod(ObjectMethodId.GetTypeId);
                instance.DefineMethod(Sig.@from(getTypeId).@override(false), code => code.ReturnTypeId(type));

				var prototype = _generator.Corlib.GetMethod(ObjectMethodId.GetType);
	            instance.DefineMethod(Sig.@from(prototype).@override(false), code =>
	                {
		                code.CallAssemblyGetType(
			                () =>
				                {
					                code.LoadThis();
					                code.Call(getTypeId);
				                }
			                );
		                code.ReturnValue();
	                });
            }

            return method;
        }

        private bool IsOverrideGetTypeId(IType type, AbcInstance instance)
        {
            if (type.Is(SystemTypeCode.Exception))
                return false;

            var bt = type.BaseType;
            var st = instance.BaseInstance;
            while (bt != null && st != null)
            {
                if (st.IsObject) return false;
                if (st.IsError) return false;
                var m = DefineGetTypeIdMethod(bt, st);
                if (m != null) return true;
                bt = bt.BaseType;
                st = st.BaseInstance;
            }
            return false;
        }
        #endregion

        #region DefineGetTypeIdMethods
        private void DefineGetTypeIdMethods()
        {
            var list = new List<AbcFile>();
            if (IsSwf)
            {
                list.AddRange(SwfCompiler.AbcFrames);
            }
            else
            {
                list.Add(Abc);
            }
            foreach (var abc in list)
            {
                abc.Generator = _generator;
                for (int i = 0; i < abc.Instances.Count; ++i)
                {
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
						_generator.MethodBuilder.BuildAbcMethod(m);
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

	    public AbcMethod Define_Assembly_GetTypeNum(IMethod method, AbcInstance instance)
	    {
		    _emitReflection = true;
			var m = instance.DefineMethod(_generator.MethodBuilder.SigOf(method), null);
		    _generator.AddLateMethod(
			    m,
			    code =>
				    {
#if DEBUG
					    DebugService.LogInfo("FinishInitTypes started");
#endif

					    DefineInitTypeMethods();
					    DefineGetTypeIdMethods();

					    code.PushInt(_initTypes.Count);
					    code.ReturnValue();

#if DEBUG
					    DebugService.LogInfo("FinishInitTypes succeeded");
#endif
				    }
			    );
		    return m;
	    }

	    #endregion

        #region Define_Assembly_InitType - InternalCall

	    public AbcMethod Define_Assembly_InitType(IMethod method, AbcInstance instance)
	    {
		    _emitReflection = true;
			var m = instance.DefineMethod(_generator.MethodBuilder.SigOf(method), null);
		    _generator.AddLateMethod(
			    m,
			    code =>
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
				    });
		    return m;
	    }

	    #endregion

        #region DefineInitTypeMethods
        void DefineInitTypeMethod(IType type, int typeId)
        {
            Debug.Assert(typeId >= 0);

			var instance = _generator.Corlib.Assembly.Instance;
	        instance.DefineMethod(
		        Sig.@this(QName.Global(Const.InitTypePrefix + typeId),
		                  AvmTypeCode.Void,
		                  SystemTypes.Type, "type"),
		        code =>
			        {
				        InitTypeInfo(code, type, typeId);
				        code.ReturnVoid();
			        });
        }

        private void DefineInitTypeMethods()
        {
            for (int i = 0; i < _initTypes.Count; ++i)
            {
                var type = _initTypes[i];
				if (!(_generator.TypeBuilder.Build(type) is AbcInstance))
                    continue;
                DefineInitTypeMethod(type, i);
            }
        }
        #endregion
    }
}