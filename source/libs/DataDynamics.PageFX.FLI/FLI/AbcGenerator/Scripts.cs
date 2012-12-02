using System;
using System.Collections.Generic;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.CodeModel.TypeSystem;
using DataDynamics.PageFX.FLI.ABC;
using DataDynamics.PageFX.FLI.IL;

namespace DataDynamics.PageFX.FLI
{
    partial class AbcGenerator
    {
        #region BuildMainScript
        void BuildMainScript()
        {
            var instance = MainInstance;
            if (instance == null) return;
            instance.IsApp = true;
            var script = new AbcScript {IsMain = true};
            _abc.Scripts.Add(script);
            script.DefineClassTraits(instance);
            script.Initializer = DefineMainScriptInit(script, instance);
        }
        #endregion

        #region DefineMainScriptInit
        //Generates method for script initializer
        AbcMethod DefineMainScriptInit(AbcScript script, AbcInstance instance)
        {
#if DEBUG
            DebugService.DoCancel();
            DebugService.LogInfo("DefineScriptInit started");
#endif

            var method = new AbcMethod();

            bool notSwf = !IsSwf;

            //Note: entry point also can contains arguments
            //Note: but in swf entry point can not have arguments
            if (_entryPoint != null && notSwf)
            {
                method.ReturnType = DefineReturnType(_entryPoint.Type);
                DefineParameters(method, _entryPoint);
            }

            var body = new AbcMethodBody(method);
            AddMethod(method);

            _mainScriptBody = body;

            var code = new AbcCode(_abc);

            _mainScriptCode = code;
            code.PushThisScope();
            code.InitClassProperties(script);

            //code.Trace("Initialization of " + instance.FullName);

            _insertIndexOfNewAPI = code.Count;
            //code.AddRange(_newAPI);
            CallStaticCtor(code, instance);

            if (notSwf) //abc?
            {
                NUnitMain(code);
                CallEntryPoint(code);
            }
            else
            {
                code.ReturnVoid();
            }

            //body.Finish(code);

#if DEBUG
            DebugService.LogInfo("DefineScriptInit succeeded");
            DebugService.DoCancel();
#endif
            return method;
        }

        AbcCode _mainScriptCode;
        AbcMethodBody _mainScriptBody;
        int _insertIndexOfNewAPI;

        void FinishMainScript()
        {
            var body = _mainScriptBody;
            if (body != null)
            {
                _mainScriptCode.InsertRange(_insertIndexOfNewAPI, _newAPI);
                body.Finish(_mainScriptCode);
            }
        }
        #endregion

        #region BuildScripts
        //bool IsMainInstance(AbcInstance instance)
        //{
        //    if (instance == null) return false;
        //    return instance.Type == MainType;
        //}

        void BuildScripts()
        {
#if DEBUG
            DebugService.DoCancel();
            DebugService.LogInfo("BuildScripts started");
#endif
#if PERF
            int start = Environment.TickCount;
#endif

            for (int i = 0; i < _abc.Instances.Count; ++i)
            {
                var instance = _abc.Instances[i];
                if (instance.Script != null) continue;
                DefineScript(instance);
            }

#if PERF
            Console.WriteLine("AbcGen.BuildScripts: {0}", Environment.TickCount - start);
#endif

#if DEBUG
            DebugService.LogInfo("BuildScripts succeeded");
            DebugService.DoCancel();
#endif
        }
        #endregion

        #region DefineScript
        void DefineScript(AbcInstance instance)
        {
            var script = new AbcScript();
            _abc.Scripts.Add(script);

            script.DefineClassTraits(instance);

            script.Initializer = _abc.DefineMethod(
                AvmTypeCode.Void,
                code =>
                    {
                        code.PushThisScope();

                        if (IsSwc && instance.Type.Is(SystemTypeCode.Object))
                        {
                            code.AddRange(_newAPI);
                        }

                        var list = GetBaseTypesWithCctors(instance);

                        const int arr = 1;
                        code.Add(InstructionCode.Newarray, 0);
                        code.SetLocal(arr);

                        DelayStaticCtors(code, list, arr);
                        code.InitClassProperties(script);
                        UndelayStaticCtors(code, list, arr);

                        //code.Trace("Initialization of " + instance.FullName);

                        CallStaticCtor(code, instance);
                        CallStaticCtors(code, list);

                        code.ReturnVoid();
                    }
                );
        }

        List<AbcInstance> GetBaseTypesWithCctors(AbcInstance instance)
        {
            var list = new List<AbcInstance>();
            var super = instance.SuperType;
            while (super != null)
            {
                if (super.IsObject) break;
                if (super.IsError) break;
                var ctor = DefineStaticCtor(super);
                if (ctor != null)
                    list.Add(super);
                super = super.SuperType;
            }
            list.Reverse();
            return list;
        }

        void DelayStaticCtors(AbcCode code, IList<AbcInstance> list, int arr)
        {
            int vf = arr + 1;
            code.GetLocal(arr);
            int n = list.Count;
            for (int i = 0; i < n; ++i)
            {
                var instance = list[i];

                code.PushNativeBool(false); //delayed
                code.SetLocal(vf);

                GetStaticCtorFlag(code, instance);
                var called = code.IfTrue();

                SetStaticCtorFlag(code, instance, true);

                code.PushNativeBool(true);
                code.SetLocal(vf);

                called.BranchTarget = code.Label();

                code.GetLocal(arr);
                code.GetLocal(vf);
                code.CallAS3("push", 1);
            }
        }

        void UndelayStaticCtors(AbcCode code, IList<AbcInstance> list, int arr)
        {
            int n = list.Count;
            for (int i = 0; i < n; ++i)
            {
                var instance = list[i];

                code.GetLocal(arr);
                code.PushInt(i);
                code.GetNativeArrayItem();
                var br = code.IfFalse();

                SetStaticCtorFlag(code, instance, false);
                
                br.BranchTarget = code.Label();
            }
        }
        #endregion

        #region CallEntryPoint
        public IType MainType
        {
            get
            {
                if (_entryPoint != null)
                    return _entryPoint.DeclaringType;
                if (IsSwf)
                {
                    if (IsMxApplication)
                        return sfc.TypeFlexApp;
                    var root = RootSprite;
                    if (root != null)
                        return root.Type;
                }
                return null;
            }
        }

        public AbcInstance MainInstance
        {
            get
            {
                if (_rootSpriteGenerated)
                    return RootSprite;
                var type = MainType;
                if (type != null)
                    return DefineAbcInstance(type);
                return null;
            }
        }

        /// <summary>
        /// Calls entry point (Main method).
        /// //NOTE: This method always adds return instruction
        /// </summary>
        /// <param name="code"></param>
        void CallEntryPoint(AbcCode code)
        {
            //Note: in swf entry point will be called in ctor of root sprite
            if (_entryPoint == null)
            {
                code.ReturnVoid();
                return;
            }

            var main = _entryPoint.Tag as AbcMethod;
            if (main == null)
                throw new InvalidOperationException("Invalid entry point");

            code.Getlex(main);

            if (AbcGenConfig.ParameterlessEntryPoint)
            {
                code.Add(InstructionCode.Callpropvoid, main.TraitName, 0);
                code.ReturnVoid();
            }
            else
            {
                //TODO:
                int n = _entryPoint.Parameters.Count;
                for (int i = 0; i < n; ++i)
                {
                    //code.GetLocal(i + 1);
                    code.PushNull();
                }

                bool isVoid = _entryPoint.IsVoid();
                if (isVoid || IsSwf)
                {
                    code.Add(InstructionCode.Callpropvoid, main.Trait.Name, n);
                    code.ReturnVoid();
                }
                else
                {
                    code.Add(InstructionCode.Callproperty, main.Trait.Name, n);
                    code.ReturnValue();
                }
            }
        }
        #endregion
    }
}