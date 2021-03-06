﻿using System;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.NUnit;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Common.Utilities;
using DataDynamics.PageFX.Flash.Abc;
using DataDynamics.PageFX.Flash.Avm;
using DataDynamics.PageFX.Flash.IL;

namespace DataDynamics.PageFX.Flash.Core.CodeGeneration.Builders
{
    internal sealed class NUnitRunner
    {
	    private readonly AbcGenerator _generator;
	    private readonly List<IType> _fixtures = new List<IType>();

		public NUnitRunner(AbcGenerator generator)
		{
			_generator = generator;
		}

	    public int FixtureCount
	    {
			get { return _fixtures.Count; }
	    }

		public void AddFixture(IType type)
		{
			_fixtures.Add(type);
		}

	    public void Main(AbcCode code)
        {
		    if (_fixtures.Count <= 0) return;

		    foreach (var fixture in _fixtures)
			    RunTestFixture(code, fixture);

		    code.CallStatic(GetMethod(NUnitMethodId.TestRunner_Run), null);
        }

        private void RunTestFixture(AbcCode code, IType type)
        {
            foreach (var test in type.GetUnitTests(false))
                RunTest(code, test);
        }

		private void RunTest(AbcCode code, IMethod test)
        {
            var testFixture = test.DeclaringType;

            var testType = GetType(NUnitTypeId.Test);
            var testInstance = GetInstance(NUnitTypeId.Test);
            const int varTest = 1;
            code.CreateInstance(testInstance);
            code.SetLocal(varTest);

            code.GetLocal(varTest);
            code.PushString(test.FullName);
            code.SetProperty(testType, "Name");

            code.GetLocal(varTest);
            code.PushString(testFixture.FullName);
            code.SetProperty(testType, "SuiteName");

            string desc = test.GetTestDescription();
            if (!string.IsNullOrEmpty(desc))
            {
                code.GetLocal(varTest);
                code.PushString(desc);
                code.SetProperty(testType, "Description");
            }

            desc = testFixture.GetTestDescription();
            if (!string.IsNullOrEmpty(desc))
            {
                code.GetLocal(varTest);
                code.PushString(desc);
                code.SetProperty(testType, "SuiteDescription");
            }

            var func = DefineTestRunner(test);
            code.GetLocal(varTest);
            code.GetStaticFunction(func);
            //code.CallSetter(testType, "Func");
            code.SetField(testType, "Func");

            //register test in FlashTestRunner
            code.CallStatic(GetMethod(NUnitMethodId.TestRunner_Register), () => code.GetLocal(varTest));
        }

	    private AbcMethod DefineTestRunner(IMethod test)
        {
			var method = _generator.MethodBuilder.BuildAbcMethod(test);

            var instance = method.Instance;
            string name = "run_test_" + test.GetMonoTestCaseName();
            name = name.Replace('.', '_');

            return instance.DefineMethod(
                Sig.@static(name, AvmTypeCode.Void, GetInstance(NUnitTypeId.Test)),
                code =>
                    {
                        var testFixture = test.DeclaringType;
                        const int varTF = 2;
                        const int varErr = 3;
                        
                        //TODO: Redirect Console, Debug output

                        var ee = test.GetExpectedExceptionType();
                        var setup = testFixture.GetUnitTestSetup();
                        AbcMethod setupAM = null;
                        if (setup != null)
							setupAM = _generator.MethodBuilder.BuildAbcMethod(setup);

                        Test_Success(code, true);
                        Test_Executed(code, true);

                        code.ConsoleOpenSW();

                        code.Try();

                        #region setup & call
                        if (test.IsStatic)
                        {
                            if (setup != null && setup.IsStatic)
                            {
                                code.Getlex(setupAM);
                                code.Call(setupAM);
                            }

                            code.Getlex(method);
                            code.Call(method);
                        }
                        else
                        {
                            code.CreateInstance(testFixture, true);
                            code.CoerceAnyType();
                            code.SetLocal(varTF);

                            if (setup != null)
                            {
                                code.GetLocal(varTF);
                                code.Call(setupAM);
                            }

                            code.GetLocal(varTF);
                            code.Call(method);
                        }
                        #endregion

                        if (ee != null)
                        {
                            code.ConsoleCloseSW(true);
                            Test_Success(code, false);
                            Test_Output(code, "No expected exception: {0}", ee.FullName);
                        }
                        else
                        {
                            Test_Output(code, () => code.ConsoleCloseSW(false));
                        }

                        code.ReturnVoid();

                        if (ee != null)
                        {
							code.BeginCatch(_generator.TypeBuilder.BuildInstance(ee), false);
                            code.Pop();
                            Test_Success(code, true);
                            Test_Output(code, () => code.ConsoleCloseSW(false));
                            code.ReturnVoid();
                            code.EndCatch(false);
                        }

                        code.BeginCatch();
                        code.CoerceAnyType();
                        code.SetLocal(varErr);
                        code.ConsoleCloseSW(true);
                        Test_Success(code, false);
                        Test_Output(
                            code,
                            () =>
                                {
                                    code.PushString("Unexpected exception: ");
                                    code.GetLocal(varErr);
                                    code.GetErrorMessage();
                                    code.Add(InstructionCode.Add);
                                });
                        Test_StackTrace(
                            code,
                            () =>
                            {
                                code.GetLocal(varErr);
                                code.GetErrorStackTrace();
                            });
                        code.EndCatch(true);

                        code.ReturnVoid();
                    });
        }

        private IType TestType
        {
            get { return GetType(NUnitTypeId.Test); }
        }

        private void Test_SetBool(AbcCode code, string prop, bool value)
        {
            code.SetPropertyBool(1, TestType, prop, value);
        }

		private void Test_Success(AbcCode code, bool value)
        {
            Test_SetBool(code, "Success", value);
        }

		private void Test_Executed(AbcCode code, bool value)
        {
            Test_SetBool(code, "Executed", value);
        }

		private void Test_Output(AbcCode code, Action value)
        {
            code.SetField(1, TestType, "Output", value);
        }

		private void Test_StackTrace(AbcCode code, Action value)
        {
            code.SetField(1, TestType, "StackTrace", value);
        }

		private void Test_Output(AbcCode code, string format, params object[] args)
        {
            Test_Output(code, () => code.PushString(string.Format(format, args)));
        }

	    private IType FindNUnitType(string fullname)
        {
            return InternalTypeExtensions.FindType(NUnitFrameworkAssembly, fullname);
        }

		private IAssembly NUnitFrameworkAssembly
        {
            get { return _asmNUnitFramework ?? (_asmNUnitFramework = FindNUnitFramework()); }
        }
        private IAssembly _asmNUnitFramework;

	    private IAssembly FindNUnitFramework()
	    {
		    return _generator.AppAssembly
		                     .GetReferences(true)
		                     .FirstOrDefault(asm => string.Equals(asm.Name, "NUnit.Framework", StringComparison.OrdinalIgnoreCase));
	    }

	    private IType GetType(NUnitTypeId id)
        {
            return NUnitTypes[(int)id].Value;
        }

		private AbcInstance GetInstance(NUnitTypeId id)
        {
			return _generator.TypeBuilder.BuildInstance(GetType(id));
        }

        private enum NUnitTypeId
        {
            FlashTestRunner,
            Test,
        }

        const string NS_PFX_NUNIT = "DataDynamics.PageFX.NUnit.";

		private LazyValue<IType>[] NUnitTypes
        {
            get
            {
            	return _nunitTypes ?? (_nunitTypes =
            	                       new[]
            	                       	{
            	                       		new LazyValue<IType>(() => FindNUnitType(NS_PFX_NUNIT + "FlashTestRunner")),
            	                       		new LazyValue<IType>(() => FindNUnitType(NS_PFX_NUNIT + "Test"))
            	                       	});
            }
        }
        private LazyValue<IType>[] _nunitTypes;

		private enum NUnitMethodId
        {
            TestRunner_Register,
            TestRunner_Run
        }

		private AbcMethod GetMethod(NUnitMethodId id)
        {
            return NUnitMethods[(int)id].Value;
        }

        private LazyValue<AbcMethod>[] NUnitMethods
        {
            get
            {
            	return _methodsNUnit ?? (_methodsNUnit =
            	                         new[]
            	                         	{
            	                         		_generator.MethodBuilder.LazyMethod(GetType(NUnitTypeId.FlashTestRunner), "Register", 1),
            	                         		_generator.MethodBuilder.LazyMethod(GetType(NUnitTypeId.FlashTestRunner), "Run", 0)
            	                         	});
            }
        }
        private LazyValue<AbcMethod>[] _methodsNUnit;
    }
}