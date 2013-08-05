using System;
using System.Collections.Generic;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Flash.Abc;
using DataDynamics.PageFX.Flash.Core.Tools;
using DataDynamics.PageFX.Flash.IL;

namespace DataDynamics.PageFX.Flash.Core.CodeGeneration.Builders
{
    internal sealed class ScriptBuilder
    {
	    private readonly AbcGenerator _generator;
		private AbcCode _mainScriptCode;
		private AbcMethodBody _mainScriptBody;
		private int _insertIndexOfNewApi;

	    public ScriptBuilder(AbcGenerator generator)
		{
			_generator = generator;
		}

	    private AbcFile Abc
	    {
			get { return _generator.Abc; }
	    }

	    private IMethod EntryPoint
	    {
		    get { return _generator.EntryPoint; }
	    }

	    private bool IsSwf
	    {
			get { return _generator.IsSwf; }
	    }

		private bool IsSwc
		{
			get { return _generator.IsSwc; }
		}

	    public void BuildMainScript()
        {
            var instance = _generator.MainInstance;
            if (instance == null) return;

            instance.IsApp = true;

            var script = new AbcScript {IsMain = true};
            Abc.Scripts.Add(script);

            script.DefineClassTraits(instance);
            script.Initializer = DefineMainScriptInit(script, instance);
        }

	    //Generates method for script initializer
        private AbcMethod DefineMainScriptInit(AbcScript script, AbcInstance instance)
        {
#if DEBUG
	        DebugService.LogInfo("DefineScriptInit started");
#endif

            var method = new AbcMethod();

            bool notSwf = !IsSwf;

            //Note: entry point also can contains arguments
            //Note: but in swf entry point can not have arguments
            if (EntryPoint != null && notSwf)
            {
                method.ReturnType = _generator.TypeBuilder.BuildReturnType(EntryPoint.Type);
				_generator.MethodBuilder.BuildParameters(method, EntryPoint);
            }

            var body = new AbcMethodBody(method);
	        _generator.Abc.AddMethod(method);

	        _mainScriptBody = body;

            var code = new AbcCode(Abc);

            _mainScriptCode = code;
            code.PushThisScope();
            code.InitClassProperties(script);

            //code.Trace("Initialization of " + instance.FullName);

            _insertIndexOfNewApi = code.Count;
            //code.AddRange(_newAPI);
            _generator.StaticCtors.Call(code, instance);

            if (notSwf) //abc?
            {
                _generator.NUnit.Main(code);
                CallEntryPoint(code);
            }
            else
            {
                code.ReturnVoid();
            }

            //body.Finish(code);

#if DEBUG
            DebugService.LogInfo("DefineScriptInit succeeded");
#endif
            return method;
        }

        public void FinishMainScript()
        {
            var body = _mainScriptBody;
            if (body != null)
            {
                _mainScriptCode.InsertRange(_insertIndexOfNewApi, _generator.NewApi);
                body.Finish(_mainScriptCode);
            }
        }

	    public void BuildScripts()
        {
#if DEBUG
		    DebugService.LogInfo("BuildScripts started");
#endif
#if PERF
            int start = Environment.TickCount;
#endif

            for (int i = 0; i < Abc.Instances.Count; ++i)
            {
                var instance = Abc.Instances[i];
                if (instance.Script != null) continue;
                DefineScript(instance);
            }

#if PERF
            Console.WriteLine("AbcGen.BuildScripts: {0}", Environment.TickCount - start);
#endif

#if DEBUG
            DebugService.LogInfo("BuildScripts succeeded");
#endif
        }

	    private void DefineScript(AbcInstance instance)
        {
            var script = new AbcScript();
            Abc.Scripts.Add(script);

            script.DefineClassTraits(instance);

            script.Initializer = Abc.DefineMethod(
                Sig.@void(),
                code =>
                    {
                        code.PushThisScope();

                        if (IsSwc && instance.Type.Is(SystemTypeCode.Object))
                        {
                            code.AddRange(_generator.NewApi);
                        }

                        var list = GetBaseTypesWithCctors(instance);

                        const int arr = 1;
                        code.Add(InstructionCode.Newarray, 0);
                        code.SetLocal(arr);

                        _generator.StaticCtors.DelayCalls(code, list, arr);
                        code.InitClassProperties(script);
						_generator.StaticCtors.UndelayCalls(code, list, arr);

                        //code.Trace("Initialization of " + instance.FullName);

						_generator.StaticCtors.Call(code, instance);
						_generator.StaticCtors.CallRange(code, list);

                        code.ReturnVoid();
                    }
                );
        }

        private List<AbcInstance> GetBaseTypesWithCctors(AbcInstance instance)
        {
            var list = new List<AbcInstance>();
            var super = instance.BaseInstance;
            while (super != null)
            {
                if (super.IsObject) break;
                if (super.IsError) break;
				var ctor = _generator.StaticCtors.DefineStaticCtor(super);
                if (ctor != null)
                    list.Add(super);
                super = super.BaseInstance;
            }
            list.Reverse();
            return list;
        }


        /// <summary>
        /// Calls entry point (Main method).
        /// //NOTE: This method always adds return instruction
        /// </summary>
        /// <param name="code"></param>
        internal void CallEntryPoint(AbcCode code)
        {
            //Note: in swf entry point will be called in ctor of root sprite
            if (EntryPoint == null)
            {
                code.ReturnVoid();
                return;
            }

            var main = EntryPoint.AbcMethod();
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
                //TODO: pass process args
                int n = EntryPoint.Parameters.Count;
                for (int i = 0; i < n; ++i)
                {
                    //code.GetLocal(i + 1);
                    code.PushNull();
                }

                bool isVoid = EntryPoint.IsVoid();
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
    }
}