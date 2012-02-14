using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.FLI.ABC;
using DataDynamics.PageFX.FLI.IL;

namespace DataDynamics.PageFX.FLI
{
    //Contains generation of MX preloading frame where flex environment is initialized.
    internal partial class SwfCompiler
	{
		#region ImportType Helpers

		private AbcInstance ImportType(AbcFile abc, string fullname)
		{
			return ImportType(abc, fullname, false);
		}

        private AbcInstance ImportType(AbcFile abc, string fullname, bool safe)
        {
        	try
        	{
				return AvmHelper.ImportType(abc, _assembly, fullname);
        	}
        	catch (Exception)
        	{
        		if (safe)
        		{
					CompilerReport.Add(Warnings.UnableImportType, fullname);
        			return null;
        		}
        		throw;
        	}
        }

		private AbcInstance ImportType(AbcFile abc, string fullname, ref AbcInstance field)
		{
			return ImportType(abc, fullname, ref field, false);
		}

		private AbcInstance ImportType(AbcFile abc, string fullname, ref AbcInstance field, bool safe)
		{
			return field ?? (field = ImportType(abc, fullname, safe));
		}

		#endregion

		#region BuildMxSystemManager
		private bool HasCrossDomainRsls
        {
            get
            {
                return _options.RslList.Any(rsl => rsl.IsCrossDomain);
            }
        }

        private AbcInstance _mxSystemManager;

        AbcMultiname DefineMxSystemManagerName(AbcFile abc)
        {
            string ns = RootNamespace;
            return abc.DefineQName(ns, NameMxSysManager);
        }

    	void BuildMxSystemManager(AbcFile abc)
        {
            var superType = ImportType(abc, "mx.managers.SystemManager");
        	var flexModuleFactoryInterface = GetFlexModuleFactoryInterface(abc);

            if (HasCrossDomainRsls)
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
                                   Type = TypeFlexApp,
                                   SuperName = superType.Name,
                                   SuperType = superType
                               };
            instance.Interfaces.Add(flexModuleFactoryInterface.Name);

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
                code =>
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
                        gotoCreate1.BranchTarget = 
                        gotoCreate2.BranchTarget = code.Label();

						// resolve class by name using SystemManager.getDefinitionByName
						//code.Trace("PFX_SYSMGR: try to resolve class using SystemManager.getDefinitionByName");
						code.FindPropertyStrict(typeClass);
						code.LoadThis();
						code.GetLocal(varClassName);
                        code.Call(getDefByName, 1);
                        code.Call(typeClass, 1);
                        code.Coerce(typeClass);
                        code.SetLocal(varClass);

                        code.GetLocal(varClass);
                        var ifClassNotNull1 = code.IfTrue();

						// try to resolve class using flash.system.ApplicationDomain.currentDomain
						//code.Trace("PFX_SYSMGR: try to resolve class using flash.system.ApplicationDomain.currentDomain");
						code.FindPropertyStrict(typeClass);
                    	GetCurrentAppDomain(code);
						code.GetLocal(varClassName);
						code.Call(abc.DefineGlobalQName("getDefinition"), 1);
						code.Call(typeClass, 1);
						code.Coerce(typeClass);
						code.SetLocal(varClass);

						code.GetLocal(varClass);
						var ifClassNotNull2 = code.IfTrue();
						code.Trace(string.Format("PFX_SYSMGR: unable to resolve class '{0}'", MainClassName));
                        code.ReturnNull();

						ifClassNotNull1.BranchTarget = 
						ifClassNotNull2.BranchTarget = code.Label();

						// create instance of class was resolved succesfully
                        code.GetLocal(varClass);
                        code.Add(InstructionCode.Construct, 0);
                        code.Add(InstructionCode.Coerce_o);
                        code.SetLocal(varInstance);

                        //if (instance is mx.core.IFlexModule)
                        code.GetLocal(varInstance);
                        var flexModule = ImportType(abc, MX.IFlexModule);
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
        private static bool _cacheInfoObject;

        void BuildSystemManagerInfo(AbcFile abc, AbcInstance instance)
        {
            var objType = abc.BuiltinTypes.Object;
            if (_cacheInfoObject)
            {
                var method = instance.DefineInstanceMethod("$info$", objType, null, null);
                AddLateMethod(method, BuildSystemManagerInfo);

                var infoField = instance.DefineSlot("__info", AvmTypeCode.Object);

                instance.DefineVirtualOverrideMethod(
                    "info", objType,
                    delegate(AbcCode code)
                        {
                            code.LoadThis();
                            code.GetProperty(infoField);

                            var br = code.IfNotNull();
                            code.LoadThis();
                            code.LoadThis();
                            code.Call(method);
                            code.SetProperty(infoField);

                            br.BranchTarget = code.Label();
                            code.LoadThis();
                            code.GetProperty(infoField);
                            code.ReturnValue();
                        });
            }
            else
            {
                var method = instance.DefineVirtualOverrideMethod("info", objType, null, null);
                AddLateMethod(method, BuildSystemManagerInfo);
            }
        }

        private List<string> GetResourceBundleNames()
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
				var type = TypeFlexApp;
				if (type == null) return null;

				string ns = NameUtil.GetTypeNamespace(RootNamespace, type);
				return string.IsNullOrEmpty(ns)
				       	? type.Name
				       	: ns + "::" + type.Name;
			}
        }

        private void BuildSystemManagerInfo(AbcCode code)
        {
            int propertyCount = 0; //number of object properties

            code.PushThisScope();

            List<RslItem> cdRsls;
            List<RslItem> rsls;
            Algorithms.Split(
                _options.RslList,
                out cdRsls,
                out rsls,
                rsl => rsl.IsCrossDomain);

            if (cdRsls != null && cdRsls.Count > 0)
            {
                code.PushString("cdRsls");
                CreateCdRslArray(code, cdRsls);
                ++propertyCount;
            }

            if (rsls != null && rsls.Count > 0)
            {
                code.PushString("rsls");
                CreateSimpleRslArray(code, rsls);
                ++propertyCount;
            }

            code.PushString("compiledLocales");
            code.PushStringArray(Locales);
            ++propertyCount;

            var list = GetResourceBundleNames();
            if (list != null && list.Count > 0)
            {
                code.PushString("compiledResourceBundleNames");
                code.PushStringArray(list);
                ++propertyCount;
            }

            code.PushString("currentDomain");
            GetCurrentAppDomain(code);
        	++propertyCount;

            //code.PushString("initialize");
            //++n;

            code.PushString("layout");
            code.PushString("absolute");
            ++propertyCount;

            code.PushString("mainClassName");
            code.PushString(MainClassName);
            ++propertyCount;

			code.PushString("usePreloader");
			code.PushBool(false);
			++propertyCount;

            list = _mixinNames;
            if (list != null && list.Count > 0)
            {
                code.PushString("mixins");
                code.PushStringArray(list);
                ++propertyCount;
            }

            code.NewObject(propertyCount);
            code.ReturnValue();
        }

    	private static void GetCurrentAppDomain(AbcCode code)
    	{
    		code.Getlex("flash.system", "ApplicationDomain");
    		code.GetProperty("currentDomain");
    	}

    	static void CreateCdRslArray(AbcCode code, IList<RslItem> rsls)
        {
            int n = rsls.Count;
            for (int i = 0; i < n; ++i)
            {
                var rsl = rsls[i];
                code.PushString("rsls");
                code.PushStringArray(new[] { rsl.Uri });

                code.PushString("policyFiles");
                code.PushStringArray(rsl.PolicyFiles);

                code.PushString("digests");
                string digest = rsl.Swc.GetLibraryDigest(null, rsl.HashType, rsl.IsSigned);
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

        static void CreateSimpleRslArray(AbcCode code, IList<RslItem> rsls)
        {
            int n = rsls.Count;
            for (int i = 0; i < n; ++i)
            {
                var rsl = rsls[i];
                code.PushString("url");
                code.PushString(rsl.Uri);

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
            if (!IsFlexApplication) return false;

            MxAppPrefix = _typeFlexApp.FullName.Replace('.', '_');
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