using System;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.FLI.ABC;
using DataDynamics.PageFX.FLI.IL;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.FLI
{
    internal partial class AbcGenerator
    {
        private readonly List<IType> _testFixtures = new List<IType>();

        /// <summary>
        /// Returns true if application assembly has nunit tests and does not define custom root sprite.
        /// </summary>
        public bool IsNUnit
        {
            get
            {
                if (IsSwf)
                {
                    if (IsMxApplication) return false;
                    if (!_rootSpriteGenerated)
                        return false;
                }
                return _testFixtures.Count > 0;
            }
        }

        #region NUnitMain
        void NUnitMain(AbcCode code)
        {
            if (_testFixtures.Count > 0)
            {
                foreach (var tf in _testFixtures)
                    RunTestFixture(code, tf);
                code.CallStatic(GetMethod(NUnitMethodId.TestRunner_Run), null);
            }
        }

        void RunTestFixture(AbcCode code, IType type)
        {
            foreach (var test in type.GetUnitTests(false))
                RunTest(code, test);
        }

        void RunTest(AbcCode code, IMethod test)
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
        #endregion

        #region DefineTestRunner
        AbcMethod DefineTestRunner(IMethod test)
        {
            var method = DefineAbcMethod(test);

            var instance = method.Instance;
            string name = "run_test_" + test.GetMonoTestCaseName();
            name = name.Replace('.', '_');

            return instance.DefineStaticMethod(
                name, AvmTypeCode.Void,
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
                            setupAM = DefineAbcMethod(setup);

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
                            code.BeginCatch(DefineAbcInstance(ee), false);
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
                    },
                GetInstance(NUnitTypeId.Test));
        }

        IType TypeTest
        {
            get { return GetType(NUnitTypeId.Test); }
        }

        void Test_SetBool(AbcCode code, string prop, bool value)
        {
            code.SetPropertyBool(1, TypeTest, prop, value);
        }

        void Test_Success(AbcCode code, bool value)
        {
            Test_SetBool(code, "Success", value);
        }

        void Test_Executed(AbcCode code, bool value)
        {
            Test_SetBool(code, "Executed", value);
        }

        void Test_Output(AbcCode code, Action value)
        {
            code.SetField(1, TypeTest, "Output", value);
        }

        void Test_StackTrace(AbcCode code, Action value)
        {
            code.SetField(1, TypeTest, "StackTrace", value);
        }

        void Test_Output(AbcCode code, string format, params object[] args)
        {
            Test_Output(code, () => code.PushString(string.Format(format, args)));
        }
        #endregion

        #region NUnitFrameworkAssembly

        private IType FindNUnitType(string fullname)
        {
            return InternalTypeExtensions.FindType(NUnitFrameworkAssembly, fullname);
        }

		private IAssembly NUnitFrameworkAssembly
        {
            get { return _asmNUnitFramework ?? (_asmNUnitFramework = FindNUnitFramework()); }
        }
        private IAssembly _asmNUnitFramework;

		private static bool IsNUnitFramework(IAssembly asm)
        {
            return string.Compare(asm.Name, "NUnit.Framework", true) == 0;
        }

		private IAssembly FindNUnitFramework()
        {
			return _assembly.GetReferences(true).FirstOrDefault(IsNUnitFramework);
        }
        
        #region NUnitTypes & Methods

		private IType GetType(NUnitTypeId id)
        {
            return NUnitTypes[(int)id].Value;
        }

		private AbcInstance GetInstance(NUnitTypeId id)
        {
            return DefineAbcInstance(GetType(id));
        }

        enum NUnitTypeId
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

        enum NUnitMethodId
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
            	                         		LazyMethod(GetType(NUnitTypeId.FlashTestRunner), "Register", 1),
            	                         		LazyMethod(GetType(NUnitTypeId.FlashTestRunner), "Run", 0)
            	                         	});
            }
        }
        private LazyValue<AbcMethod>[] _methodsNUnit;

        #endregion
        #endregion
    }
}