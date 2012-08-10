using System;
using System.Collections;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.FLI.ABC;
using DataDynamics.PageFX.FLI.IL;

namespace DataDynamics.PageFX.FLI
{
    internal partial class AbcGenerator
    {
        #region DefineInlineCode
        AbcCode DefineInlineCode(IMethod method, AbcInstance instance)
        {
            if (instance != null && instance.IsNative)
            {
                var mn = instance.Name;
                string name = mn.FullName;
                if (name == "Object")
                {
                    var code = new AbcCode(_abc);
                    GetObjectInlineCode(method, code);
                    return code;
                }
                if (name == "Class")
                {
                    var code = new AbcCode(_abc);
                    GetClassInlineCode(method, code);
                    return code;
                }
                if (name == "Array")
                {
                    var code = new AbcCode(_abc);
                    GetArrayInlineCode(method, code);
                    return code;
                }
                if (name == "XMLList" || name == "XML")
                {
                    var code = new AbcCode(_abc);
                    GetXmlInlineCode(method, code);
                    return code;
                }
                if (name.StartsWith(AS3.Vector.FullName))
                {
                    return GetVectorInlineCode(method,
                                               name.Length != AS3.Vector.FullName.Length);
                }
            }

            var type = method.DeclaringType;
            if (type.Tag is IVectorType)
                return GetVectorInlineCode(method, true);

            string typeName = type.FullName;
            if (typeName == AvmTypeName)
            {
                var code = new AbcCode(_abc);
                GetAvmInlineCode(method, code);
                return code;
            }

            return GetInlineCode(typeName, method.Name);
        }
        #endregion

        #region Shared InlineCodeMap
        Hashtable _sharedICM; //shared inline code map
        const string AvmTypeName = "avm";

        void InitInlineCodeMap()
        {
            if (_sharedICM != null) return;
            _sharedICM = new Hashtable();

            AddInline("System.Diagnostics.Debugger", "Break",
                      code => code.DebuggerBreak());

            AddInline(
                "System.Object", "ReferenceEquals",
                code =>
                    {
                        code.Add(InstructionCode.Strictequals);
                        code.FixBool();
                    });

            AddInline("System.Exception", "avm_getStackTrace",
                      code => code.GetErrorStackTrace());

            AddInline("System.Exception", "avm_getMessage",
                      code => code.GetErrorMessage());

            AddInline("System.Exception", "avm_setMessage",
                      code => code.SetProperty("message"));

            InitStringInlines();
        }

        void AddInline(string type, string method, AbcCoder coder)
        {
            if (coder == null)
                coder = delegate { };

            var t = _sharedICM[type] as Hashtable;
            if (t == null)
            {
                t = new Hashtable();
                _sharedICM[type] = t;
            }
            t[method] = coder;
        }

        AbcCode GetInlineCode(string type, string method)
        {
            InitInlineCodeMap();
            var t = _sharedICM[type] as Hashtable;
            if (t == null) return null;
            var coder = t[method] as AbcCoder;
            if (coder != null)
            {
                var code = new AbcCode(_abc);
                coder(code);
                return code;
            }
            return null;
        }
        #endregion

        #region String Inlines
        void AddStringInline(string method, AbcCoder code)
        {
            AddInline("System.String", method, code);
        }

        void InitStringInlines()
        {
            AddStringInline(
                "GetChar",
                code => code.GetChar());

            AddStringInline(
                "get_Length",
                code => code.GetStringLength());

            AddStringInline(
                "substring",
                code => code.Substring(1));

            AddStringInline(
                "substring2",
                code => code.Substring(2));

            AddStringInline(
                "substr",
                code => code.Substr(2));

            AddStringInline(
                "Equals",
                code =>
                {
                    code.Add(InstructionCode.Equals);
                    code.FixBool();
                });

            AddStringInline(
                "op_Implicit",
                code => { }
                );

            AddStringInline(
                "op_Equality",
                code =>
                {
                    code.Add(InstructionCode.Equals);
                    code.FixBool();
                }
                );

            AddStringInline(
                "op_Inequality",
                code => 
                {
                    code.Add(InstructionCode.Equals);
                    code.Add(InstructionCode.Not);
                    code.FixBool();
                }
                );
        }
        #endregion

        #region class InlineCodeMap
        class InlineCodeMap
        {
            private readonly Hashtable _map = new Hashtable();
            private static readonly char[] Separator = { ';' };

        	private object this[string name]
            {
                get
                {
                    return _map[name];
                }
                set
                {
                    if (value == null)
                        value = (AbcCoder)delegate { };

                    var names = name.Split(Separator, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var key in names)
                    {
                        _map[key] = value;
                    }
                }
            }

            public void Register(params object[] args)
            {
                if (args == null)
                    throw new ArgumentNullException("args");
                int n = args.Length;
                for (int i = 0; i < n; i += 2)
                {
                    string name = args[i] as string;
                    if (name == null)
                        throw new InvalidOperationException();

                    //NOTE: coder can be null. it means empty code;
                    this[name] = args[i + 1];
                }
            }

            public void Resolve(IMethod method, AbcCode code)
            {
                var v = this[method.Name];

                var coder = v as AbcCoder;
                if (coder != null)
                {
                    coder(code);
                    return;
                }

                var coder2 = v as AbcCoder2;
                if (coder2 != null)
                {
                    coder2(method, code);
                    return;
                }

                throw UnknownCall(method);
            }
        }
        #endregion

        #region Avm.Object
        void GetObjectInlineCode(IMethod method, AbcCode code)
        {
            string name = method.Name;

            int n = method.Parameters.Count;

            if (name == "GetProperty")
            {
                if (n == 1) //name
                {
                    code.PushGlobalPackage();
                    code.Swap(); //namespace should be before name
                    code.GetRuntimeProperty();
                    return;
                }

                if (n == 2) //ns, name
                {
                    if (method.Parameters[0].Type == SystemTypes.String)
                    {
                        code.FixRuntimeQName();
                    }
                    code.GetRuntimeProperty();
                    return;
                }
            }
            else if (name == "SetProperty")
            {
                if (n == 2) //name, value
                {
                    var vh = DefineValueHolder();
                    var value = vh.GetStaticSlot("value");

                    code.Getlex(vh);
                    code.Swap(); //after: name, vh, value
                    code.SetSlot(value);

                    code.PushGlobalPackage(); //after: name, ns
                    code.Swap(); //ns, name

                    code.Getlex(vh);
                    code.GetSlot(value);

                    code.SetRuntimeProperty();
                    return;
                }

                if (n == 3) //ns, name, value
                {
                    if (method.Parameters[0].Type == SystemTypes.String)
                    {
                        var vh = DefineValueHolder();
                        var value = vh.GetStaticSlot("value");

                        code.Getlex(vh);
                        code.Swap();
                        code.SetSlot(value);

                        code.FixRuntimeQName();

                        code.Getlex(vh);
                        code.GetSlot(value);
                    }
                    code.SetRuntimeProperty();
                    return;
                }
            }

            throw UnknownCall(method);
        }
        #endregion

        #region Avm.Class
        static void GetClassInlineCode(IMethod method, AbcCode code)
        {
            string name = method.Name;

            if (name == "Find")
            {
                if (method.Parameters[0].Type == SystemTypes.String)
                {
                    code.FixRuntimeQName();
                }

                //NOTE: VerifyError: Error #1078: Illegal opcode/multiname combination: 96<[]::[]>.
                //code.Getlex(code.abc.RuntimeQName);

                var m = code.Generator.DefineFindClass();
                code.Call(m);

                return;
            }

            if (name == "CreateInstance")
            {
                code.Construct(method.Parameters.Count);
                return;
            }

            throw UnknownCall(method);
        }
        #endregion

        #region Avm.Array
        static InlineCodeMap _icmArray;

        static void InitArrayInlineCodeMap()
        {
            if (_icmArray != null) return;
            var map = new InlineCodeMap();
            _icmArray = map;

            map.Register(
                "get_Item",
                (AbcCoder)(code => code.GetNativeArrayItem()),

                "get_Length",
                (AbcCoder)(code => code.GetArrayLengthInt()),

                "set_Item;SetValue",
                (AbcCoder)(code => code.SetNativeArrayItem()),

                "GetBool",
                (AbcCoder)
                (code =>
                     {
                         code.GetNativeArrayItem();
                         code.CoerceBool();
                     }),

                "GetInt32",
                (AbcCoder)
                (code =>
                     {
                         code.GetNativeArrayItem();
                         code.CoerceInt32();
                     }),

                "GetUInt32",
                (AbcCoder)
                (code =>
                     {
                         code.GetNativeArrayItem();
                         code.CoerceUInt32();
                     }),

                "GetDouble",
                (AbcCoder)
                (code =>
                     {
                         code.GetNativeArrayItem();
                         code.CoerceDouble();
                     }),

                "GetString",
                (AbcCoder)
                (code =>
                     {
                         code.GetNativeArrayItem();
                         code.CoerceString();
                     }),

                "GetChar",
                (AbcCoder)
                (code =>
                     {
                         code.GetNativeArrayItem();
                         code.CoerceChar();
                     })
                );
        }

        private static void GetArrayInlineCode(IMethod method, AbcCode code)
        {
            InitArrayInlineCodeMap();
            _icmArray.Resolve(method, code);
        }
        #endregion

        #region Avm.XML, Avm.XMLList
        static void GetXmlInlineCode(IMethod method, AbcCode code)
        {
            string name = method.Name;
            if (name == "get_Item")
            {
                int n = method.Parameters.Count;
                if (n == 1)
                {
                    var p0 = method.Parameters[0].Type;
                    if (p0 == SystemTypes.Int32)
                    {
                        code.GetNativeArrayItem();
                        code.CoerceXML();
                    }
                    else //string
                    {
                        //TODO: It seams that any namespace can not be used with runtime qnames.
                        code.PushGlobalPackage();
                        code.Swap();
                        code.GetRuntimeProperty();
                        code.CoerceXMLList();
                    }
                }
                else
                {
                    if (n != 2)
                        throw new InvalidOperationException("Invalid arg count of XMLList call " + method.Name);

                    var p0 = method.Parameters[0].Type;
                    if (p0.Name == "Namespace")
                    {
                        code.GetRuntimeProperty();
                        code.CoerceXMLList();
                    }
                    else //namespace as Avm.String
                    {
                        code.Swap(); //stack [name, nsname]
                        var ns = code[AvmTypeCode.Namespace];
                        code.FindPropertyStrict(ns); //stack [name, nsname, global]
                        code.Swap(); //stack [name, global, nsname]
                        code.ConstructProperty(ns, 1); //stack [name, ns]
                        code.Coerce(ns); //stack [name, ns]
                        code.Swap(); //stack [ns, name]
                        code.GetRuntimeProperty();
                        code.CoerceXMLList();
                    }
                }

                return;
            }

            throw UnknownCall(method);
        }
        #endregion

        #region avm
        static InlineCodeMap _icmAVM;

        static void InitAvmInlineCodeMap()
        {
            if (_icmAVM != null) return;
            _icmAVM = new InlineCodeMap();
            _icmAVM.Register(

                "get_EmptyString",
                (AbcCoder)
                (code => code.PushString("")),

                "get_GlobalPackage",
                (AbcCoder)
                (code => code.PushGlobalPackage()),

                "get_IsFlashPlayer",
                (AbcCoder)
                (code =>
                     {
                         var m = code.Generator.DefineIsFlashPlayer();
                         code.Getlex(m);
                         code.Call(m);
                     }),

                "trace",
                (AbcCoder2)
                ((method, code) =>
                     {
                         //has global receiver
                         var mn = code.DefineGlobalQName("trace");
                         //code.Add(InstructionCode.Findpropstrict, mn);
                         code.CallVoid(mn, method.Parameters.Count);
                     }),

                "exit",
                (AbcCoder)
                (code =>
                     {
                         var m = code.Generator.DefineExitMethod();
                         code.Getlex(m);
                         code.Call(m);
                     }),

                "Console_Write",
                (AbcCoder)
                (code => code.AvmplusSystemWrite()),

                "AddEventListener",
                (AbcCoder)
                (code => code.AddEventListener()),

                "RemoveEventListener",
                (AbcCoder)
                (code => code.RemoveEventListener()),

                "CreateInstance",
                (AbcCoder2)
                ((method, code) => code.Construct(method.Parameters.Count - 1)),

                "GetProperty",
                (AbcCoder)
                (code => code.GetRuntimeProperty()),

                "SetProperty",
                (AbcCoder)
                (code => code.SetRuntimeProperty()),

                "Findpropstrict",
                (AbcCoder)
                (code => code.FindPropertyStrict(code.abc.RuntimeQName)),

                "Construct",
                (AbcCoder)
                (code => code.ConstructProperty(code.abc.RuntimeQName, 0)),

                "get_m_value",
                (AbcCoder)
                (code => code.GetBoxedValue()),

                "set_m_value",
                (AbcCoder)
                (code => code.SetBoxedValue()),

                "Concat",
                (AbcCoder2)
                ((method, code) =>
                     {
                         int n = method.Parameters.Count;
                         if (n <= 1)
                             throw new InvalidOperationException();
                         for (int i = 1; i < n; ++i)
                             code.Add(InstructionCode.Add);
                         code.CoerceString();
                     }),

                "NewObject",
                (AbcCoder2)
                ((method, code) => code.NewObject(method.Parameters.Count / 2)),

                "ReturnValue",
                (AbcCoder)
                (code => code.ReturnValue()),

                "ToString",
                (AbcCoder)
                (code => code.CallGlobal("toString", 0)),

                "NewArray",
                (AbcCoder2)
                ((method, code) =>
                     {
                         if (method.Parameters.Count == 1)
                         {
                             //size is onto the stack
                             code.Getlex(AvmTypeCode.Array);
                             code.Swap();
                             code.Construct(1);
                             code.CoerceArray();
                             return;
                         }
                         code.Add(InstructionCode.Newarray, 0);
                     }),

                "CopyArray",
                (AbcCoder)
                (code =>
                     {
                         var prop = code.abc.DefineGlobalQName("concat");
                         code.Call(prop, 0);
                         code.Coerce(AvmTypeCode.Array);
                     }),

                "IsNull",
                (AbcCoder)
                (code =>
                     {
                         code.PushNull();
                         code.Add(InstructionCode.Equals);
                         code.FixBool();
                     }),

                "IsUndefined",
                (AbcCoder)
                (code =>
                     {
                         code.PushUndefined();
                         code.Add(InstructionCode.Equals);
                         code.FixBool();
                     }),

                "GetArrayElem",
                (AbcCoder)
                (code => code.GetNativeArrayItem())
                );
        }

        static void GetAvmInlineCode(IMethod method, AbcCode code)
        {
            InitAvmInlineCodeMap();
            _icmAVM.Resolve(method, code);
        }
        #endregion

        #region Avm.Vector
        AbcCode GetVectorInlineCode(IMethod m, bool typed)
        {
            var code = new AbcCode(_abc);
            if (m.IsConstructor)
            {
                var vec = m.DeclaringType.Tag as IVectorType;
                if (vec == null)
                    throw new InvalidOperationException();
                code.Construct(m.Parameters.Count);
                code.Coerce(vec.Name);
                return code;
            }

            string name = m.Name;
            if (name.StartsWith("op_"))
                return code;

            if (m.IsGetter)
            {
                var prop = m.Association as IProperty;
                if (prop == null)
                    throw new InvalidOperationException();
                if (prop.Parameters.Count > 0) //indexer
                {
                    if (typed)
                    {
                        code.GetProperty(code.abc.NameArrayIndexer);
                        code.Coerce(m.Type, true);
                    }
                    else
                    {
                        code.GetNativeArrayItem();
                    }
                }
                else
                {
                    code.GetProperty(prop.Name);
                }
                return code;
            }
            if (m.IsSetter)
            {
                var prop = m.Association as IProperty;
                if (prop == null)
                    throw new InvalidOperationException();
                if (prop.Parameters.Count > 0) //indexer
                {
                    code.SetNativeArrayItem();
                }
                else
                {
                    code.SetProperty(prop.Name);
                }
                return code;
            }
            code.CallAS3(name, m.Parameters.Count);
            return code;
        }
        #endregion

        #region Utils
        static Exception UnknownCall(ITypeMember method)
        {
            return new InvalidOperationException(string.Format("Unknown internal call {0}.{1}",
                                                               method.DeclaringType.FullName, method.Name));
        }
        #endregion
    }
}