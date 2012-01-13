using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.FLI.ABC;
using DataDynamics.PageFX.FLI.IL;

namespace DataDynamics.PageFX.FLI
{
    //Contains generation of MX preloading frame where flex environment is initialized.
    internal partial class SwfCompiler
    {
        AbcInstance ImportType(AbcFile abc, string fullname)
        {
            return AvmHelper.ImportType(abc, _assembly, fullname);
        }

        #region BuildMxSystemManager
        bool HasCrossDomainRSLs
        {
            get
            {
                return Algorithms.Contains(_options.RSLList, rsl => rsl.IsCrossDomain);
            }
        }

        AbcInstance _mxSystemManager;

        AbcMultiname DefineMxSystemManagerName(AbcFile abc)
        {
            string ns = RootNamespace;
            return abc.DefineQName(ns, NameMxSysManager);
        }

        void BuildMxSystemManager(AbcFile abc)
        {
            var superType = ImportType(abc, "mx.managers.SystemManager");
            var IFlexModuleFactory = ImportType(abc, "mx.core.IFlexModuleFactory");

            if (HasCrossDomainRSLs)
            {
                // Cause the CrossDomainRSLItem class to be linked into this application.
                var crossDomainRSLItem = ImportType(abc, "mx.core.CrossDomainRSLItem");
                Debug.Assert(crossDomainRSLItem != null);
            }
            
            var instance = new AbcInstance(true)
                               {
                                   Name = DefineMxSystemManagerName(abc),
                                   Flags = AbcClassFlags.FinalSealed,
                                   //ProtectedNamespace = abc.DefineProtectedNamespace(NameMxSysManager),
                                   Type = _typeMxApp,
                                   SuperName = superType.Name,
                                   SuperType = superType
                               };
            instance.Interfaces.Add(IFlexModuleFactory.Name);

            instance.Initializer = abc.DefineEmptyConstructor();
            instance.Class.Initializer = abc.DefineEmptyMethod();

            abc.AddInstance(instance);

            _mxSystemManager = instance;

            BuildSystemManagerCreate(abc, instance);
            BuildSystemManagerInfo(abc, instance);

            abc.DefineScript(instance);
            abc.DefineEmptyScript();
        }
        #endregion

        #region BuildSystemManagerCreate
        /* Base Method Code
            public function create(... params):Object
	        {
	            var mainClassName:String = info()["mainClassName"];

		        if (mainClassName == null)
	            {
                    var url:String = loaderInfo.loaderURL;
                    var dot:int = url.lastIndexOf(".");
                    var slash:int = url.lastIndexOf("/");
                    mainClassName = url.substring(slash + 1, dot);
	            }

		        var mainClass:Class = Class(getDefinitionByName(mainClassName));
        		
		        return mainClass ? new mainClass() : null;
	        }
         */

        /* Flex Auto Generated Code
            public override function create(... params):Object
            {
                if (params.length > 0 && !(params[0] is String))
                    return super.create.apply(this, params);

                var mainClassName:String = params.length == 0 ? "AppName" : String(params[0]);
                var mainClass:Class = Class(getDefinitionByName(mainClassName));
                if (!mainClass)
                    return null;

                var instance:Object = new mainClass();
                if (instance is IFlexModule)
                    (IFlexModule(instance)).moduleFactory = this;
                return instance;
            }
         */
        void BuildSystemManagerCreate(AbcFile abc, AbcInstance instance)
        {
            var method = instance.DefineVirtualOverrideMethod(
                "create", AvmTypeCode.Object,
                delegate(AbcCode code)
                    {
                        var typeString = abc.DefineGlobalQName("String");
                        var typeClass = abc.DefineGlobalQName("Class");
                        //NOTE: getDefinitionByName is method of mx.core.SystemManager
                        var getDefByName = abc.DefineGlobalQName("getDefinitionByName");

                        const int argParams = 1;
                        const int varClassName = 2;
                        const int varClass = 3;
                        const int varInstance = 4;

                        code.PushThisScope();
                        code.GetLocal(argParams);
                        code.GetArrayLengthInt();
                        code.PushInt(0);
                        var gotoCheckString = code.If(BranchOperator.GreaterThan);

                        code.PushString(MainClassName);
                        code.SetLocal(varClassName);
                        var gotoCreate1 = code.Goto();

                        //check string block
                        var checkString = code.Label();
                        gotoCheckString.BranchTarget = checkString;

                        //params[0]
                        code.GetNativeArrayItem(argParams, 0);
                        code.Getlex(typeString);
                        code.Add(InstructionCode.Istypelate);
                        //if not str goto calling of super.create
                        var ifNotStr = code.IfFalse();

                        code.FindPropertyStrict(typeString);
                        code.GetNativeArrayItem(argParams, 0);
                        code.Call(typeString, 1);
                        code.CoerceString();
                        code.SetLocal(varClassName);
                        var gotoCreate2 = code.Goto();

                        //super.create.apply(this, params) block
                        var superCreate = code.Label();
                        ifNotStr.BranchTarget = superCreate;

                        code.LoadThis();
                        code.GetSuper("create");
                        code.LoadThis();
                        code.GetLocal(argParams);
                        code.ApplyFunction(2);
                        code.ReturnValue();

                        //instance creation block
                        var create = code.Label();
                        gotoCreate1.BranchTarget = create;
                        gotoCreate2.BranchTarget = create;

                        code.FindPropertyStrict(typeClass);

                        code.LoadThis();

                        code.GetLocal(varClassName);
                        code.Call(getDefByName, 1);
                        code.Call(typeClass, 1);
                        code.Coerce(typeClass);
                        code.SetLocal(varClass);

                        code.GetLocal(varClass);
                        var ifClassNotNull = code.IfTrue();
                        code.ReturnNull();

                        ifClassNotNull.BranchTarget = code.Label();

                        code.GetLocal(varClass);
                        code.Add(InstructionCode.Construct, 0);
                        code.Add(InstructionCode.Coerce_o);
                        code.SetLocal(varInstance);

                        //if (instance is mx.core.IFlexModule)
                        code.GetLocal(varInstance);
                        var flexModule = ImportType(abc, "mx.core.IFlexModule");
                        code.Getlex(flexModule);
                        code.Add(InstructionCode.Istypelate);
                        var gotoReturn = code.IfFalse();

                        //((IFlexModule)instance).moduleFactory = this
                        code.FindPropertyStrict(flexModule.Name);
                        code.GetLocal(varInstance);
                        code.Call(flexModule.Name, 1);
                        code.LoadThis();
                        code.SetPublicProperty("mx.core:IFlexModule", "moduleFactory");

                        gotoReturn.BranchTarget = code.Label();
                        code.GetLocal(varInstance);
                        code.ReturnValue();
                    });
            method.Flags |= AbcMethodFlags.NeedRest;
        }
        #endregion

        #region BuildSystemManagerInfo
        static bool cacheInfoObject;

        void BuildSystemManagerInfo(AbcFile abc, AbcInstance instance)
        {
            var objType = abc.BuiltinTypes.Object;
            if (cacheInfoObject)
            {
                var method = instance.DefineInstanceMethod("$info$", objType, null, null);
                AddLateMethod(method, BuildSystemManagerInfo);

                var __info = instance.DefineSlot("__info", AvmTypeCode.Object);

                instance.DefineVirtualOverrideMethod(
                    "info", objType,
                    delegate(AbcCode code)
                        {
                            code.LoadThis();
                            code.GetProperty(__info);

                            var br = code.IfNotNull();
                            code.LoadThis();
                            code.LoadThis();
                            code.Call(method);
                            code.SetProperty(__info);

                            br.BranchTarget = code.Label();
                            code.LoadThis();
                            code.GetProperty(__info);
                            code.ReturnValue();
                        });
            }
            else
            {
                var method = instance.DefineVirtualOverrideMethod("info", objType, null, null);
                AddLateMethod(method, BuildSystemManagerInfo);
            }
        }

        List<string> GetResourceBundleNames()
        {
            var list = new List<string>();
            var hash = new Hashtable();
            foreach (var abc in AbcFrames)
            {
                foreach (var instance in abc.Instances)
                {
                    string name = instance.ResourceBundleName;
                    if (instance.IsResourceBundle && !hash.Contains(name))
                    {
                        hash[name] = instance;
                        list.Add(name);
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Name of TopLevel Window Class (MX App)
        /// </summary>
        public string MainClassName
        {
            get
            {
                if (_typeMxApp == null)
                    return null;

                string ns = NameUtil.GetTypeNamespace(RootNamespace, _typeMxApp);
                if (string.IsNullOrEmpty(ns))
                    return _typeMxApp.Name;

                return ns + "::" + _typeMxApp.Name;
            }
        }

        void BuildSystemManagerInfo(AbcCode code)
        {
            int pn = 0; //number of object properties

            code.PushThisScope();

            List<RSLItem> cdRsls;
            List<RSLItem> rsls;
            Algorithms.Split(
                _options.RSLList,
                out cdRsls,
                out rsls,
                rsl => rsl.IsCrossDomain);

            if (cdRsls != null && cdRsls.Count > 0)
            {
                code.PushString("cdRsls");
                CreateCdRslArray(code, cdRsls);
                ++pn;
            }

            if (rsls != null && rsls.Count > 0)
            {
                code.PushString("rsls");
                CreateSimpleRslArray(code, rsls);
                ++pn;
            }

            code.PushString("compiledLocales");
            code.PushStringArray(Locales);
            ++pn;

            var list = GetResourceBundleNames();
            if (list != null && list.Count > 0)
            {
                code.PushString("compiledResourceBundleNames");
                code.PushStringArray(list);
                ++pn;
            }

            code.PushString("currentDomain");
            code.Getlex("flash.system", "ApplicationDomain");
            code.GetProperty("currentDomain");
            ++pn;

            //code.PushString("initialize");
            //++n;

            code.PushString("layout");
            code.PushString("absolute");
            ++pn;

            code.PushString("mainClassName");
            code.PushString(MainClassName);
            ++pn;

            list = _mixinNames;
            if (list != null && list.Count > 0)
            {
                code.PushString("mixins");
                code.PushStringArray(list);
                ++pn;
            }

            code.NewObject(pn);
            code.ReturnValue();
        }

        static void CreateCdRslArray(AbcCode code, IList<RSLItem> rsls)
        {
            int n = rsls.Count;
            for (int i = 0; i < n; ++i)
            {
                var rsl = rsls[i];
                code.PushString("rsls");
                code.PushStringArray(new[] { rsl.URI });

                code.PushString("policyFiles");
                code.PushStringArray(rsl.PolicyFiles);

                code.PushString("digests");
                string digest = rsl.SWC.GetLibraryDigest(null, rsl.HashType, rsl.IsSigned);
                if (string.IsNullOrEmpty(digest))
                    throw Errors.RSL.UnableToResolveLibraryDigest.CreateException(rsl.LocalPath);
                code.PushStringArray(new[] { digest });

                code.PushString("types");
                code.PushStringArray(new[] { rsl.HashType });

                code.PushString("isSigned");
                code.PushNativeBool(rsl.IsSigned);
                code.Add(InstructionCode.Newarray, 1);

                code.NewObject(5);
            }
            code.Add(InstructionCode.Newarray, n);
        }

        static void CreateSimpleRslArray(AbcCode code, IList<RSLItem> rsls)
        {
            int n = rsls.Count;
            for (int i = 0; i < n; ++i)
            {
                var rsl = rsls[i];
                code.PushString("url");
                code.PushString(rsl.URI);

                code.PushString("size");
                code.PushInt(-1);

                code.NewObject(2);
            }
            code.Add(InstructionCode.Newarray, n);
        }
        #endregion

        #region BuildMxFrame
        const string MxMgrNameSuffix = "_PFX_SYSMGR$";
        string MxAppPrefix;
        string NameMxSysManager;

        bool BuildMxFrame()
        {
            if (!IsMxApplication) return false;

            MxAppPrefix = _typeMxApp.FullName.Replace('.', '_');
            NameMxSysManager = "$" + MxAppPrefix + MxMgrNameSuffix;

            AbcFile.AllowExternalLinking = false;

            FrameMX = new AbcFile
                          {
                              Name = "MX Frame",
                              SwfCompiler = this
                          };

            BuildMxSystemManager(FrameMX);

            FrameMX.Finish();

            AbcFile.AllowExternalLinking = true;

            return true;
        }
        #endregion
    }
}