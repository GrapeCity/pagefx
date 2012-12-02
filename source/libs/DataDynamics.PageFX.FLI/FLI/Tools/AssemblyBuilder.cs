using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.CodeModel.TypeSystem;
using DataDynamics.PageFX.FLI.ABC;
using DataDynamics.PageFX.FLI.SWC;
using DataDynamics.PageFX.FLI.SWF;
using Ionic.Zip;

namespace DataDynamics.PageFX.FLI
{
    /// <summary>
    /// Builds assembly from abc or swc file.
    /// </summary>
    internal class AssemblyBuilder : IDisposable, ISwcLinker, ITypeResolver
    {
        #region Shared Members
        public static IAssembly Build(string path, CommandLine cl)
        {
            using (var builder = new AssemblyBuilder(cl))
                return builder.FromFile(path);
        }

        public static IAssembly Build(Stream input, CommandLine cl)
        {
            using (var builder = new AssemblyBuilder(cl))
                return builder.FromStream(input);
        }
        #endregion

        #region Fields
        List<string> _references = new List<string>();
        readonly List<IAssembly> _refs = new List<IAssembly>();
        List<AbcFile> _abcFiles = new List<AbcFile>();
        readonly IAssembly _assembly;
        XmlDocument _doc;
        bool _xdoc;
        CommandLine _cl;
        bool _useFPAttrs;
        float _fpVersion;
        #endregion

        #region ctor
        public AssemblyBuilder(CommandLine cl)
        {
            if (cl != null)
                Setup(cl);

            _assembly = new AssemblyImpl
                            {
                                Version = new Version(1, 0, 0, 0)
                            };
        }

        private static Stream Unzip(string path)
        {
            var zip = new ZipFile(path);
            return zip.First().OpenReader().ToMemoryStream();
        }

        private static Stream Unzip(Stream stream)
        {
            var zip = ZipFile.Read(stream);
            return zip.First().OpenReader().ToMemoryStream();
        }

        void Setup(CommandLine cl)
        {
            _cl = cl;
            _references = GlobalSettings.GetRefs(cl);

            SetupDoc(cl);
            SetPlayerVersion(cl);
        }

        void SetupDoc(CommandLine cl)
        {
            string docpath = cl.GetPath(null, "xdoc");
            if (LoadDocFile(docpath))
            {
                _xdoc = true;
            }
            else
            {
                docpath = cl.GetPath(null, "doc");
                if (!LoadDocFile(docpath))
                    LoadStdDoc();
            }
        }

        void SetPlayerVersion(CommandLine cl)
        {
            string fp = cl.GetOption(null, "FP");
            if (!string.IsNullOrEmpty(fp))
            {
                if (!float.TryParse(fp, NumberStyles.Currency, CultureInfo.InvariantCulture, out _fpVersion))
                    throw new InvalidOperationException("Invalid /FP option");
                _useFPAttrs = true;
                LoadFP9();
            }
        }
        #endregion

        #region LoadDocFile
        private bool LoadDocFile(string docpath)
        {
            if (string.IsNullOrEmpty(docpath))
                return false;

            if (!Path.IsPathRooted(docpath))
                docpath = Path.Combine(Environment.CurrentDirectory, docpath);

            if (!File.Exists(docpath))
                return false;

            try
            {
                _doc = new XmlDocument();
                if (docpath.EndsWith("zip", StringComparison.InvariantCultureIgnoreCase))
                {
                    var stream = Unzip(docpath);
                    _doc.Load(stream);
                }
                else
                {
                    _doc.Load(docpath);
                }
                return true;
            }
            catch
            {
                _doc = null;
                //Console.WriteLine("Unable to load doc file");
                return false;
            }
        }

        void LoadStdDoc()
        {
            var rs = typeof(AssemblyBuilder).GetResourceStream("apidoc.zip");
            if (rs == null) return;

            try
            {
                _doc = new XmlDocument();
                var stream = Unzip(rs);
                _doc.Load(stream);
            }
            catch
            {
                _doc = null;
                //Console.WriteLine("Unable to load doc file");
            }
        }
        #endregion

        #region IDisposable Members
        //Implement IDisposable.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Free other state (managed objects).
                _abcFiles = null;
            }
            // Free your own state (unmanaged objects).
            // Set large fields to null.
        }

        // Use C# destructor syntax for finalization code.
        ~AssemblyBuilder()
        {
            // Simply call Dispose(false).
            Dispose(false);
        }
        #endregion

        #region ISwcLinker Members
        public IAssembly Assembly
        {
            get { return _assembly; }
        }

        public void LinkType(AbcInstance instance)
        {
        }

        public object ResolveExternalReference(string id)
        {
        	return _refs.Select(assembly => AssemblyIndex.ResolveRef(assembly, id)).FirstOrDefault(r => r != null);
        }

    	#endregion

        #region Naming Utils
        static readonly Hashtable namemap = new Hashtable();

        #region C# keywords
        static readonly string[] CSharpKeywords =
            {
                "abstract",
                "event",
                "new",
                "struct",
                "as",
                "explicit",
                "null",
                "switch",
                "base",
                "extern",
                "object",
                "this",
                "bool",
                "false",
                "operator",
                "throw",
                "break",
                "finally",
                "out",
                "true",
                "byte",
                "fixed",
                "override",
                "try",
                "case",
                "float",
                "params",
                "typeof",
                "catch",
                "for",
                "private",
                "uint",
                "char",
                "foreach",
                "protected",
                "ulong",
                "checked",
                "goto",
                "public",
                "unchecked",
                "class",
                "if",
                "readonly",
                "unsafe",
                "const",
                "implicit",
                "ref",
                "ushort",
                "continue",
                "in",
                "return",
                "using",
                "decimal",
                "int",
                "sbyte",
                "virtual",
                "default",
                "interface",
                "sealed",
                "volatile",
                "delegate",
                "internal",
                "short",
                "void",
                "do",
                "is",
                "sizeof",
                "while",
                "double",
                "lock",
                "stackalloc",
                "else",
                "long",
                "static",
                "enum",
                "namespace",
                "string"
            };
        #endregion

        static AssemblyBuilder()
        {
            //namemap["CASEINSENSITIVE"] = "CaseInsensitive";
            //namemap["UNIQUESORT"] = "UniqueSort";
            //namemap["RETURNINDEXEDARRAY"] = "ReturnIndexedArray";
            //namemap["XMLUI"] = "XmlUI";
            namemap["uint"] = "UInt";
            namemap["int"] = "Int";
        }

        static readonly char[] BadNameChars = { '$' };

        static bool IsBadNameChar(char c)
        {
            return BadNameChars.Contains(c);
        }

        private static string ReplaceBadChars(IEnumerable<char> name)
        {
        	return name.Aggregate("", (current, c) => current + (IsBadNameChar(c) ? '_' : c));
        }

    	static string Rename(string name, bool clistyle)
        {
            string map = namemap[name] as string;
            if (map != null) return map;
            name = ReplaceBadChars(name);
            return clistyle ? name.ToPascalCase() : name;
        }

        static string RenameParam(string name)
        {
            int i = Array.IndexOf(CSharpKeywords, name);
            if (i >= 0) return "@" + name;
            return name;
        }
        #endregion

        #region ResolveRefs
        static string ResolveRef(string path, string name)
        {
            if (Path.IsPathRooted(name))
                return name;

            if (string.IsNullOrEmpty(path))
                return Path.Combine(Environment.CurrentDirectory, name);

            return Path.Combine(Path.GetDirectoryName(path), name);
        }

        bool HasCorlibRef
        {
            get 
            {
                return GlobalSettings.HasCorlibRef(_references);
            }
        }

        void ResolveRefs(string path)
        {
            if (_references != null && _references.Count > 0)
            {
                foreach (var name in _references)
                {
                    string refPath = ResolveRef(path, name);
                    LoadReference(refPath);
                }
            }
            //auto detection of corlib dependency
            if (!IsCoreAPI && !HasCorlibRef)
            {
                string corlib = GlobalSettings.GetCorlibPath(true);
                //string dir = Path.GetDirectoryName(path);
                //string local = Path.Combine(dir, Path.GetFileName(corlib));
                //File.Copy(corlib, local, true);
                LoadReference(corlib);
                if (_cl != null)
                    _cl.AddOption("ref", corlib);
            }
        }

        void LoadReference(string refPath)
        {
            var asm = LanguageInfrastructure.CLI.Deserialize(refPath, null);
            if (!Linker.Start(asm))
                throw new InvalidOperationException();
            _refs.Add(asm);
        }
        #endregion

        #region Entry Points
        public IAssembly FromFile(string path)
        {
            var abc = new AbcFile(path);
            _abcFiles.Add(abc);
            ResolveRefs(path);
            _assembly.Location = path;
            _assembly.Name = Path.GetFileName(path);
            BuildCore();
            return _assembly;
        }

        public IAssembly FromStream(Stream input)
        {
            var abc = new AbcFile(input);
            _abcFiles.Add(abc);
            ResolveRefs(null);
            BuildCore();
            return _assembly;
        }

        SwcFile _swc;

        public IAssembly FromSwc(string path)
        {
            _assembly.Location = path;
            _assembly.Name = Path.GetFileName(path);

        	_swc = new SwcFile(path) {AddNsRefs = true};

        	foreach (var abc in _swc.GetAbcFiles())
				_abcFiles.Add(abc);

            ResolveRefs(path);

            _swc.ResolveDependencies(this, null);

            BuildCore();

            //var abl = AblFile.FromSwc(swc);
            //abl.Save(Path.ChangeExtension(path, ".abl"));

            BuildSwcDeps(path);

            return _assembly;
        }

        void BuildCore()
        {
            foreach (var abc in _abcFiles)
            {
                foreach (var instance in abc.Instances)
                    BuildType(instance);

                foreach (var script in abc.Scripts)
                    BuildScript(script);
            }
        }
        #endregion

        #region BuildSwcDeps
        void BuildSwcDeps(string path)
        {
            var f = new SwcDepFile();
            f.Build(_swc, this);

            path = Path.ChangeExtension(path, ".swcdep");
            f.Save(path);
        }
        #endregion

        #region BuildScript, BuildGlobalType
        IType BuildGlobalType(AbcScript script, string ns)
        {
            if (string.IsNullOrEmpty(ns))
                ns = SetNamespacePrefix(ns, NsPrefix);

            const string name = "Global";
            string fullname = ns.MakeFullName(name);
            var res = _assembly.FindType(fullname);
            if (res != null) return res;
        	var type = new UserDefinedType
        	           	{
        	           		Namespace = ns,
        	           		Name = name,
        	           		Visibility = Visibility.Public
        	           	};
        	type.Tag = new GlobalType(type);
            RegisterType(type);

            DefineSwcAbcFileAttribute(type, script.ABC);
            //DefineIndexAttribute(type, Attrs.AbcScript, script.Index);
            DefineAttribute(type, Attrs.GlobalFunctions);

            return type;
        }

        void BuildScript(AbcScript script)
        {
            foreach (var trait in script.Traits)
            {
                if (IsVisible(trait.Visibility))
                {
                    if (trait.Kind == AbcTraitKind.Method)
                    {
                        string ns = trait.Name.NamespaceString;
                        var type = BuildGlobalType(script, ns);
                        BuildMethod(type, trait);
                    }
                }
            }
        }
        #endregion

        #region BuildTypeByName, BuildMemberType
        IType ITypeResolver.Resolve(string fullname)
        {
            return BuildTypeByName(fullname);
        }

        static bool IsVoid(AbcMultiname name)
        {
            if (name == null) return false;
            if (name.Namespace == null) return false;
            return name.Namespace.Kind == AbcConstKind.PackageNamespace
                   && name.NamespaceString == ""
                   && name.NameString == "void";
        }

        static bool IsVector(AbcMultiname name)
        {
            if (name == null) return false;
            return name.FullName == "__AS3__.vec.Vector";
        }

        static bool IsGlobalType(AbcMultiname name, string type)
        {
            if (name == null) return false;
            return name.IsGlobalName(type);
        }

        static bool IsNumber(AbcMultiname name)
        {
            return IsGlobalType(name, "Number");
        }

        static bool IsInt(AbcMultiname name)
        {
            return IsGlobalType(name, "int");
        }

        static bool IsUInt(AbcMultiname name)
        {
            return IsGlobalType(name, "uint");
        }

        static bool IsObject(AbcMultiname name)
        {
            return IsGlobalType(name, "Object");
        }

        static string GetVectorParamType(AbcMultiname param)
        {
            if (IsNumber(param)) return "double";
            if (IsInt(param)) return "int";
            if (IsUInt(param)) return "uint";
            if (IsObject(param)) return "object";
            return null;
        }

        private static AbcInstance FindInstance(AbcFile abc, AbcMultiname name)
        {
            return abc.FindInstance(instance => Equals(instance.Name, name));
        }

    	private AbcInstance FindInstance(AbcMultiname name)
    	{
    		return _abcFiles.Select(abc => FindInstance(abc, name)).FirstOrDefault(instance => instance != null);
    	}

    	AbcInstance FindVector(string type)
        {
            string vtype = "Vector$" + type;
    		return _abcFiles.SelectMany(abc => abc.Instances)
				.FirstOrDefault(instance => instance.NamespaceString == AS3.Vector.Namespace && instance.NameString == vtype);
        }

        IType BuildPredefinedVector(AbcMultiname param)
        {
            string type = GetVectorParamType(param);
            if (string.IsNullOrEmpty(type)) return null;
            var v = FindVector(type);
            if (v == null)
                throw new InvalidOperationException("No vector$" + type);
            return BuildType(v);
        }

        IType BuildTypeByName(AbcMultiname name)
        {
            if (IsAnyType(name))
                return SystemTypes.Object;

            if (IsVoid(name))
                return SystemTypes.Void;

            if (name.IsParameterizedType)
            {
                if (IsVector(name.Type))
                {
                    //IType type = BuildPredefinedVector(name.TypeParameter);
                    //if (type != null) return type;
                    return BuildVector(name);
                }
                return BuildTypeByName(name.Type);
            }

            var instance = FindInstance(name);
            if (instance != null)
                return BuildType(instance);

            if (_refs != null)
            {
                foreach (var asm in _refs)
                {
                    var abc = AssemblyTag.Instance(asm).ABC;
                    if (abc != null)
                    {
                        instance = FindInstance(abc, name);
                        if (instance != null)
                            return BuildType(instance);
                    }

                    var type = AssemblyIndex.FindType(asm, name);
                    if (type != null)
                        return type;
                }
            }

            throw new InvalidOperationException(string.Format("Unable to find type {0}", name.FullName));
        }

        IType BuildTypeByName(string fullname)
        {
            if (_swc != null)
            {
                var instance = _swc.AbcCache.FindInstance(fullname);
                if (instance != null)
                    return BuildType(instance);
            }
            else
            {
                foreach (var abc in _abcFiles)
                {
                    foreach (var instance in abc.Instances)
                    {
                        if (instance.FullName == fullname)
                            return BuildType(instance);
                    }
                }
            }

            bool isObj = fullname == "Object";

            foreach (var asm in _refs)
            {
                Linker.Start(asm);
                foreach (var type in asm.Types)
                {
                    if (isObj && type.FullName == "Avm.Object")
                        return type;
                    if (type.FullName == fullname)
                        return type;
                }
            }

            return null;
        }

        private IType BuildMemberType(AbcMultiname name)
        {
			if (name.IsObject)
			{
				return SystemTypes.Object;
			}

        	if (string.IsNullOrEmpty(name.NamespaceString))
        	{
        		string s = name.NameString;
        		switch (s)
        		{
        			case "Number":
        				return SystemTypes.Double;
        			case "int":
        				return SystemTypes.Int32;
        			case "uint":
        				return SystemTypes.UInt32;
        			case "Boolean":
        				return SystemTypes.Boolean;
        		}
        	}

        	return BuildTypeByName(name);
        }
        #endregion

        #region BuildVector
        IType BuildVector(AbcMultiname vname)
        {
            Debug.Assert(vname.IsParameterizedType);
            var param = vname.TypeParameter;
            var paramType = BuildMemberType(param);
            if (paramType == null)
                throw new InvalidOperationException("Unable to build vector type parameter: " + param);

            var vector = GetGenericVector();
            return TypeFactory.MakeGenericType(vector, paramType);
        }

        IGenericType GetGenericVector()
        {
            if (_genericVector != null) return _genericVector;

            string src = GetType().GetTextResource("Resources.Vector.cs");

            var type = new GenericType
                           {
                               Namespace = "Avm",
                               Name = "Vector",
                               SourceCode = src
                           };

            var T = new GenericParameter
                        {
                            Name = "T",
                            Position = 0,
                            DeclaringType = type
                        };
            type.GenericParameters.Add(T);

            _genericVector = type;

			if (IsCoreAPI)
			{
				RegisterType(type);
			}

        	return _genericVector;
        }

        IGenericType _genericVector;
        #endregion

        #region BuildType
		//static bool IsExcluded(AbcInstance instance)
		//{
		//    AbcClass klass = instance.Class;
		//    var t = klass.Trait;
		//    if (t == null) return false;
		//    if (!t.HasMetadata) return false;
		//    bool excludeClass = false;
		//    bool embed = false;
		//    foreach (var entry in t.Metadata)
		//    {
		//        string name = entry.NameString;
		//        if (!excludeClass && name == "ExcludeClass")
		//            excludeClass = true;
		//        if (!embed && name == "Embed")
		//            embed = true;
		//        if (excludeClass && embed)
		//            return true;
		//    }
		//    return false;
		//}

        static bool IsVisible(AbcInstance instance)
        {
            //if (IsExcluded(instance))
            //{
            //    Console.WriteLine("ExcludedClass {0}", instance.FullName);
            //    return false;
            //}

            var vis = instance.Visibility;
            switch (vis)
            {
                case Visibility.PrivateScope:
                case Visibility.Private:
                case Visibility.NestedPrivate:
                case Visibility.NestedInternal:
                    return false;

                case Visibility.Internal:
                    return instance.FullName.StartsWith(AS3.Vector.FullName);
            }

            return true;
        }

        IType BuildType(AbcInstance instance)
        {
            if (instance.Type != null)
                return instance.Type;

            if (!IsVisible(instance))
            {
                //Console.WriteLine("internal type: {0}", instance.FullName);
                return null;
            }

            var type = new UserDefinedType();
            bool isInterface = instance.IsInterface;
            type.TypeKind = isInterface ? TypeKind.Interface : TypeKind.Class;

            string ns = instance.NamespaceString;
            if (string.IsNullOrEmpty(ns))
                ns = SetNamespacePrefix(ns, NsPrefix);

            type.Namespace = ns;
            type.Visibility = Visibility.Public;

            string name = instance.NameString;
            name = Rename(name, false);

            type.Name = name;
            type.Tag = instance;
            instance.Type = type;

            type.Documentation = FindTypeSummary(instance);

            DefinePfxAttributes(null, type, instance.Name);

            if (instance.SuperName != null && instance.SuperName.Index != 0)
            {
                if (instance.FullName == "Error")
                {
                    type.BaseType = SystemTypes.Exception;
                }
                else
                {
                    var baseType = BuildTypeByName(instance.SuperName);
                    type.BaseType = baseType;
                }
            }

            if (instance.Interfaces != null)
            {
                foreach (var ifaceName in instance.Interfaces)
                {
                    var ifaceType = BuildTypeByName(ifaceName);
                    if (ifaceType != null)
                        type.Interfaces.Add(ifaceType);
                }
            }

            bool isAbstract = isInterface;
            //Note: abstract instance is instance with at least one abstract method (that has no method body)

            if (!isInterface)
            {
                BuildCtors(type, instance.Initializer, false);
            }

            BuildFields(type, instance.Traits); //instance fiedls
            if (!isInterface)
                BuildFields(type, instance.Class.Traits); //static fields

            BuildMethods(type, instance.Traits); //instance methods
            if (!isInterface)
                BuildMethods(type, instance.Class.Traits); //static methods

            type.IsAbstract = isAbstract;

            BuildEvents(type, instance);

            DefineCustomMembers(type, instance);

            RegisterType(type);
            
            return type;
        }

        string FindTypeSummary(AbcInstance instance)
        {
			if (_doc == null || _doc.DocumentElement == null) return null;
            if (instance == null) return null;
            XmlElement elem;
            if (_xdoc)
            {
                string name = GetTypeName(instance);
                string xpath = string.Format("members/member[@name='T:{0}']/summary", name);
                elem = _doc.DocumentElement.SelectSingleNode(xpath) as XmlElement;
            }
            else
            {
                var name = instance.Name;
                string fullname = name.FullName;
                string xpath = string.Format("type[@name='{0}']/summary", fullname);
                elem = _doc.DocumentElement.SelectSingleNode(xpath) as XmlElement;
            }
            return elem == null ? null : elem.InnerText;
        }

        void RegisterType(IType type)
        {
            var mod = _assembly.MainModule;
            type.Module = mod;
            _assembly.Types.Add(type);
        }
        #endregion

        #region BuildEvents
        const string FlashEvent = "flash.events.Event";

        void BuildEvents(IType type, AbcInstance instance)
        {
            var klass = instance.Class;
            var trait = klass.Trait;
            if (trait != null && trait.HasMetadata)
            {
                foreach (var e in trait.Metadata)
                {
                    if (e.NameString == "Event")
                    {
                        BuildEvent(e, type);
                    }
                }
            }
            ImplementEvents(type);
        }

        static void ImplementEvents(IType type)
        {
        	if (type.Interfaces == null) return;

        	foreach (var iface in type.Interfaces)
        	{
        		foreach (var e in iface.Events)
        		{
        			var typeEvent = FindMember(type, e.Name);
        			if (typeEvent == null)
        			{
        				typeEvent = CopyEvent(e);
        				type.Members.Add(typeEvent);
        			}
					typeEvent.Visibility = Visibility.Public;
        		}
        	}
        }

    	static Event CopyEvent(IEvent e)
        {
            var copy = new Event
                         {
                             Name = e.Name,
                             IsFlash = e.IsFlash,
                             Visibility = e.Visibility,
                             IsStatic = e.IsStatic,
                             Type = e.Type
                         };
            CopyAttrs(e, copy);
            return copy;
        }

        static void CopyAttrs(ICustomAttributeProvider from, ICustomAttributeProvider to)
        {
            foreach (var attr in from.CustomAttributes)
            {
                var attr2 = (ICustomAttribute)attr.Clone();
                attr2.Owner = to;
                to.CustomAttributes.Add(attr2);
            }
        }

		private static ITypeMember FindMember(IType type, string name)
		{
			return type.Members.FirstOrDefault(m => m.Name == name);
		}

    	private static bool HasMember(IType type, string name)
    	{
    		return FindMember(type, name) != null;
    	}

        void BuildEvent(AbcMetaEntry me, IType type)
        {
            string name = me["name"];
            string eventType = me["type"];
            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("WARN: Event has no name");
                return;
            }

            string eventName = name;
            if (HasMember(type, name))
            {
                eventName = "On" + name.ToPascalCase();
            }

			var be = FindBaseEvent(type, eventName);
			if (be != null)
			{
				IEvent e = CopyEvent(be);
				type.Members.Add(e);
				return;
			}

            if (eventType == "Object")
                eventType = FlashEvent;
            else if (string.IsNullOrEmpty(eventType))
                eventType = FlashEvent;

            var handlerType = BuildEventHandler(eventType) ?? BuildEventHandler(FlashEvent);

        	if (handlerType != null)
            {
                var e = new Event
                        	{
                        		IsFlash = true,
                        		Name = eventName,
                        		IsStatic = false,
                        		Visibility = Visibility.Public,
                        		Type = handlerType
                        	};
            	DefineAttribute(e, Attrs.Event, "name", name);
                type.Members.Add(e);
            }
        }

        static IEvent FindBaseEvent(IType type, string name)
        {
            if (type.Interfaces != null)
            {
                foreach (var iface in type.Interfaces)
                {
                    var e = iface.Events[name];
                    if (e != null)
                        return e;
                }
            }

            var baseType = type.BaseType;
            if (baseType != null)
            {
                var e = baseType.Events[name];
                if (e != null)
                    return e;
            }

            return null;
        }

        IType BuildEventHandler(string fullname)
        {
            var eventType = BuildTypeByName(fullname);
            if (eventType == null)
            {
                Console.WriteLine("WARN: Unable to find event type {0}", fullname);
                //eventType = BuildTypeByName(fullname);
                //throw new InvalidOperationException();
                return null;
            }

            string handlerName = eventType.Namespace.MakeFullName(eventType.Name + "Handler");
            var handlerType = BuildTypeByName(handlerName);
            if (handlerType != null)
                return handlerType;

            var type = new UserDefinedType
                           {
                               TypeKind = TypeKind.Delegate,
                               Visibility = Visibility.Public,
                               Namespace = eventType.Namespace,
                               Name = (eventType.Name + "Handler")
                           };

            var m = new Method
                        {
                            Name = "Invoke",
                            IsStatic = false,
                            Visibility = Visibility.Public,
                            Type = SystemTypes.Void
                        };
            m.Parameters.Add(new Parameter(eventType, "e", 1));
            type.Members.Add(m);

            RegisterType(type);
            return type;
        }
        #endregion

        #region CustomMembers
        static Hashtable _customMembers;

        static void LoadCustomMembers()
        {
            if (_customMembers != null) return;
            _customMembers = new Hashtable();

            var asm = typeof(AssemblyBuilder).Assembly;
            foreach (var resName in asm.GetManifestResourceNames())
            {
                int i = resName.IndexOf("API", 0, StringComparison.InvariantCultureIgnoreCase);
                if (i >= 0)
                {
                    var rs = asm.GetManifestResourceStream(resName);
                    string name = resName.Substring(i + 4).Trim().TrimFileExtension();
                    string text = rs.ReadText();
                    _customMembers[name] = text;
                }
            }
        }

        static void DefineCustomMembers(IType type, AbcInstance instance)
        {
            LoadCustomMembers();

            string fname = instance.FullName;
            string text = (string)_customMembers[fname];
            if (text != null)
            {
                type.CustomMembers = text;
            }
        }
        #endregion

        #region BuildMembers
        void BuildFields(IType type, IEnumerable<AbcTrait> traits)
        {
            foreach (var trait in traits)
            {
                if (IsVisible(trait) && trait.IsField)
                {
                    BuildField(type, trait);
                }
            }
        }

        void BuildMethods(IType type, IEnumerable<AbcTrait> traits)
        {
            //build methods and properties
            foreach (var trait in traits)
            {
                if (IsVisible(trait) && trait.IsMethod)
                {
                    BuildMethod(type, trait);
                }
            }
        }

        static void AddMethod(IType type, IMethod method)
        {
            if (method == null) return;

            type.Members.Add(method);

            if (method.IsAbstract)
                type.IsAbstract = true;

            var assoc = method.Association;
            if (assoc != null && assoc.DeclaringType == null)
                type.Members.Add(assoc);
        }

        static void AddMethod2(IType type, IMethod method)
        {
            if (method.DeclaringType == null)
                AddMethod(type, method);
        }
        #endregion

        #region BuildCtor
        void BuildCtors(IType declType, AbcMethod abcMethod, bool isStatic)
        {
            if (DoNotBuildCtor(abcMethod.Instance)) return;

            var method = new Method
                             {
                                 IsInternalCall = true,
                                 IsSpecialName = true
                             };

            if (isStatic)
            {
                method.Name = CLRNames.StaticConstructor;
                method.Visibility = Visibility.Internal;
            }
            else
            {
                method.Visibility = Visibility.Public;
                method.Name = CLRNames.Constructor;
            }

            method.IsStatic = isStatic;
            method.Type = SystemTypes.Void;

            var docElem = FindCtorDocElem(abcMethod);
            method.Documentation = GetSummary(docElem);
            
            BuildParams(abcMethod, method.Parameters, docElem);

            //DefineIndexAttribute(method, abcMethod.Index);

            BuildOverloads(declType, method, abcMethod);
        }

        XmlElement FindCtorDocElem(AbcMethod abcMethod)
        {
            if (!_xdoc) return null;

            string typename = GetTypeName(abcMethod.Instance);
            if (string.IsNullOrEmpty(typename)) return null;

            string memberName = "M:" + typename + ".#ctor";
            return FindMember(memberName);
        }
        #endregion

        #region Fixes
        static Hashtable _paramFix;
        static Hashtable _returnFix;

        static void LoadFixes()
        {
            if (_paramFix != null) return;
            _paramFix = new Hashtable();
            _returnFix = new Hashtable();

            _paramFix["String.indexOf"] = new[] {"string val", "int startIndex"};
            _paramFix["String.lastIndexOf"] = new[] {"string val", "int startIndex"};
            _paramFix["String.substring"] = new[] {"int startIndex", "int endIndex"};
            _paramFix["String.slice"] = new[] {"int startIndex", "int endIndex"};
            _paramFix["String.split"] = new[] {"object delimiter", "int limit"};
            _paramFix["String.search"] = new[] {"object pattern"};
            _paramFix["String.charAt"] = new[] {"int index"};

            _paramFix["String.charCodeAt"] = new[] {"int index"};
            _returnFix["String.charCodeAt"] = "uint";

            _paramFix["String.substr"] = new[] {"int startIndex", "int len"};
            _paramFix["String.replace"] = new[] {"object pattern", "object repl"};

            var errorCtor = new[] { "string message", "int id" };

            _paramFix["Error..ctor"] = errorCtor;
            _paramFix["ArgumentError..ctor"] = errorCtor;
            _paramFix["DefinitionError..ctor"] = errorCtor;
            _paramFix["EvalError..ctor"] = errorCtor;
            _paramFix["RangeError..ctor"] = errorCtor;
            _paramFix["ReferenceError..ctor"] = errorCtor;
            _paramFix["SecurityError..ctor"] = errorCtor;
            _paramFix["SyntaxError..ctor"] = errorCtor;
            _paramFix["TypeError..ctor"] = errorCtor;
            _paramFix["UninitializedError..ctor"] = errorCtor;
            _paramFix["URIError..ctor"] = errorCtor;
            _paramFix["VerifyError..ctor"] = errorCtor;
        }
        #endregion

        #region BuildParams, BuildParam
        class ParamFix
        {
            public int index;
            public string name;
            public string type;

            public override string ToString()
            {
                return string.Format("[{0}] {1} {2}", index, type, name);
            }
        }

        static ParamFix[] GetParamFix(AbcMethod m)
        {
            LoadFixes();
            string name = m.FullName;
            if (string.IsNullOrEmpty(name)) return null;
            var fix = (string[])_paramFix[name];
            if (fix == null) return null;
            int n = fix.Length;
            if (n <= 0) return null;
            var arr = new ParamFix[n];
            for (int i = 0; i < n; ++i)
            {
                var ps = fix[i].Split(' ');
            	arr[i] = new ParamFix
            	         	{
            	         		index = i,
            	         		type = ps[0],
            	         		name = ps[1]
            	         	};
            }
            return arr;
        }

        IType GetSystemType(string name)
        {
            switch (name)
            {
                case "int":
                    return SystemTypes.Int32;
                case "uint":
                    return SystemTypes.UInt32;
                case "object":
                    return SystemTypes.Object;
                case "double":
                    return SystemTypes.Double;
                case "string":
                    return BuildTypeByName("String");
                case "Function":
                    return BuildTypeByName("Function");
                default:
                    throw new ArgumentOutOfRangeException("name");
            }
        }

        //static bool IsInstance(AbcMethod method, string name)
        //{
        //    if (method == null) return false;
        //    AbcInstance instance = method.Instance;
        //    if (instance == null) return false;
        //    return instance.FullName == name;
        //}

        void BuildParams(AbcMethod method, IParameterCollection parameters, XmlNode methodElem)
        {
            if (method.IsSetter)
            {
                if (IsDate(method.Instance))
                {
                    var p = new Parameter {Type = SystemTypes.Int32, Name = "value"};
                    parameters.Add(p);
                    return;
                }
            }

            var fix = GetParamFix(method);

            int n = method.Parameters.Count;
            for (int i = 0; i < n; ++i)
            {
                var p = BuildParam(method, i, fix, methodElem);
                parameters.Add(p);
            }

            //if (method.NeedRest)
            //{
            //    Parameter p = new Parameter();
            //    p.Name = "rest";
            //    p.Type = TypeFactory.MakeArray(SystemTypes.Object);
            //    p.HasParams = true;
            //    parameters.Add(p);
            //}
        }

        static bool IsFunctionCall(AbcMethod method)
        {
            return method.FullName == "Function.call";
        }

        static bool IsFunctionApply(AbcMethod method)
        {
            return method.FullName == "Function.apply";
        }

        static bool HasThisObjectArgument(AbcMethod method)
        {
            return IsFunctionCall(method) || IsFunctionApply(method);
        }

        static bool IsExternalInterfaceCall(AbcMethod method)
        {
            return method.FullName == "flash.external.ExternalInterface.call";
        }

        Parameter BuildParam(AbcMethod method, int i, IEnumerable<ParamFix> fix, XmlNode methodElem)
        {
            var pe = GetParamElement(methodElem, i);

            var p = BuildVectorParam(method, i, pe);
            if (p != null)  return p;

            p = new Parameter();
            var ap = method.Parameters[i];

            if (pe != null)
                p.Documentation = pe.InnerText;

            ParamFix pfix = null;
            if (fix != null)
				pfix = fix.FirstOrDefault(item => item.index == i);

            string name = null;
            IType type = null;
            if (i == 0 && HasThisObjectArgument(method))
            {
                name = "thisObject";
                type = SystemTypes.Object;
            }
            else if (i == 0 && IsExternalInterfaceCall(method))
            {
                name = "name";
            }
            else if (pfix != null)
            {
                name = pfix.name;
                type = GetSystemType(pfix.type);
            }
            else
            {
                if (pe != null)
                    name = pe.GetAttribute("name");
                else if (method.HasParamNames)
                    name = ap.Name.Value;
                if (string.IsNullOrEmpty(name))
                    name = string.Format("arg{0}", i);
                type = BuildMemberType(ap.Type);
            }

            if (type == null)
                type = BuildMemberType(ap.Type);

            p.Name = RenameParam(name);
            p.Type = type;

            return p;
        }

        static XmlElement GetParamElement(XmlNode elem, int i)
        {
            if (elem == null) return null;
            int k = 0;
            foreach (XmlNode node in elem.ChildNodes)
            {
                var e = node as XmlElement;
                if (e != null)
                {
                    if (e.Name == "param")
                    {
                        if (k == i)
                            return e;
                        ++k;
                    }
                }
            }
            return null;
        }

		/// <summary>
		/// Creates parameter for AS3.Vector type method.
		/// </summary>
		/// <param name="method"></param>
		/// <param name="i"></param>
		/// <param name="pe"></param>
		/// <returns></returns>
        private Parameter BuildVectorParam(AbcMethod method, int i, XmlElement pe)
        {
            string vparam = GetVectorTypeParam(method);
            if (vparam == null) return null;

            if (method.IsInitializer) //ctor
            {
                if (i == 0)
                    return CreateParam(SystemTypes.Int32, "length", pe);
                if (i == 1)
                    return CreateParam(SystemTypes.Boolean, "@fixed", pe);
                throw new InvalidOperationException();
            }

            var t = method.Trait;
            if (t == null) return null;
            switch (t.NameString)
            {
                case "join":
                    {
                        if (i == 0)
                            return CreateParam(AvmString, "sep", pe);
                        throw new InvalidOperationException();
                    }

                case "indexOf":
                case "lastIndexOf":
                    {
                        if (i == 0)
                        {
                            var type = GetSystemType(vparam);
                            return CreateParam(type, "searchElement", pe);
                        }
                        if (i == 1)
                            return CreateParam(SystemTypes.Int32, "fromIndex", pe);
                        throw new InvalidOperationException();
                    }

                case "slice":
                    {
                        if (i == 0)
                            return CreateParam(SystemTypes.Int32, "startIndex", pe);
                        if (i == 1)
                            return CreateParam(SystemTypes.Int32, "endIndex", pe);
                        throw new InvalidOperationException();
                    }

                case "splice":
                    {
                        if (i == 0)
                            return CreateParam(SystemTypes.Int32, "startIndex", pe);
                        if (i == 1)
                            return CreateParam(SystemTypes.UInt32, "deleteCount", pe);
                        throw new InvalidOperationException();
                    }

                case "some":
                case "every":
                case "map":
                case "forEach":
                case "filter":
                    {
                        if (i == 0)
                            return CreateParam(AvmFunction, "callback", pe);
                        if (i == 1)
                            return CreateParam(SystemTypes.Object, "thisObject", pe);
                        throw new InvalidOperationException();
                    }

                case "sort":
                    {
                        if (i == 0)
                            return CreateParam(AvmFunction, "compareFunction", pe);
                        throw new InvalidOperationException();
                    }
            }

            return null;
        }

        IType BuildVectorReturnType(AbcMethod m)
        {
            string vparam = GetVectorTypeParam(m);
            if (vparam == null) return null;

            var t = m.Trait;
            if (t == null) return null;
            switch (t.NameString)
            {
                case "pop":
                case "shift":
                    return GetSystemType(vparam);

                case "concat":
                case "filter":
                case "reverse":
                case "map":
                case "sort":
                case "slice":
                case "splice":
                    return m.Instance.Type;

                case "push":
                case "unshift":
                    return SystemTypes.UInt32;

                case "indexOf":
                case "lastIndexOf":
                    return SystemTypes.Int32;

                case "some":
                    return SystemTypes.Boolean;
            }

            return null;
        }

        static Parameter CreateParam(IType type, string name, XmlElement pe)
        {
            var p = new Parameter(type, name);
            if (pe != null)
                p.Documentation = pe.InnerText;
            return p;
        }

        static string GetVectorTypeParam(AbcMethod method)
        {
            if (method == null) return null;
            var instance = method.Instance;
            if (instance == null) return null;

            string name = instance.FullName;
            if (name.StartsWith(AS3.Vector.FullName))
            {
                if (name.Length > AS3.Vector.FullName.Length)
                    return name.Substring(AS3.Vector.FullName.Length + 1);
                return "";
            }

            return null;
        }

        static bool IsVectorMethod(AbcMethod m)
        {
            string p = GetVectorTypeParam(m);
            return p != null;
        }

        private IType AvmString
        {
            get { return _avmString ?? (_avmString = GetSystemType("string")); }
        }
		private IType _avmString;

		private IType AvmFunction
        {
            get { return _avmFunction ?? (_avmFunction = GetSystemType("Function")); }
        }
		private IType _avmFunction;
        #endregion

        #region BuildMethod
		private static void RenameMethod(IType declType, IMethod method)
		{
			string newname = ReplaceBadChars(method.Name);
			if (newname != method.Name && newname != declType.Name)
			{
				method.Name = newname;
			}
		}

        void BuildMethod(IType declType, AbcTrait trait)
        {
            var abcMethod = trait.Method;

            var method = new Method();
            InitTypeMember(method, trait);

        	RenameMethod(declType, method);
			
            method.IsVirtual = trait.IsVirtual;
            method.IsNewSlot = !trait.IsOverride;
            method.Type = BuildReturnType(abcMethod);

            if (!abcMethod.IsNative)
                method.IsAbstract = abcMethod.IsAbstract;

            //error CS0180: method can not be both extern and abstract
            if (!method.IsAbstract)
                method.IsInternalCall = true;

            DefinePfxAttributes(declType, method, trait);

            var docElem = FindDocElement(trait);

            string summary = GetSummary(docElem);
            string retdoc = GetInnerTextOf(docElem, "returns");
            
            BuildParams(abcMethod, method.Parameters, docElem);

            if (trait.IsAccessor)
            {
                BuildProperty(method, trait, summary);
                AddMethod(declType, method);
            }
            else
            {
                method.Documentation = summary;
                method.ReturnDocumentation = retdoc;

                BuildOverloads(declType, method, abcMethod);
            }
        }

        #region BuildOverloads
        void BuildOverloads(IType declType, IMethod method, AbcMethod abcMethod)
        {
            if (declType.IsInterface)
            {
                if (abcMethod.NeedRest)
                {
                    //TODO:
                    AddMethod2(declType, method);
                }
                else
                {
                    AddMethod2(declType, method);
                }
                return;
            }

            //TODO: Think about how to correctly wrap method with rest argument to allow pfx developer to override such methods
            //FB: case 112212
            int paramNum = method.Parameters.Count;
            if (abcMethod.NeedRest)
            {
                AddMethod2(declType, method);
                
                int n = MaxRestCount;
                if (IsVectorMethod(abcMethod))
                    n = 20;

                for (int k = 1; k <= n; ++k)
                {
                    var m2 = CreateOverload(method, -1);
                    for (int i = 0; i < k; ++i)
                    {
                        var p = CreateRestParam(abcMethod, paramNum, i);
                        m2.Parameters.Add(p);
                    }
                    AddMethod(declType, m2);
                }
            }
            else
            {
                AddMethod2(declType, method);
            }

            if (!abcMethod.IsOverride && abcMethod.HasOptionalParams)
            {
                for (int i = paramNum - 1; i >= 0; --i)
                {
                    var p = abcMethod.Parameters[i];
                    if (p.IsOptional)
                    {
                        var m2 = CreateOverload(method, i);
                        //TODO: Extend doc with default values for optional params.
                        AddMethod(declType, m2);
                    }
                }
            }
        }
        #endregion

        #region BuildProperty
        static string GetAccessorPrefix(bool getter)
        {
            return getter ? "get_" : "set_";
        }

        static void BuildProperty(IMethod method, AbcTrait trait, string summary)
        {
            string name = method.Name;
            method.Name = GetAccessorPrefix(trait.IsGetter) + name;
            var prop = FindProperty(trait.Owner.Traits, trait.Name.NameString);
            if (prop == null)
            {
                prop = new Property
                           {
                               Documentation = summary,
                               Name = name
                           };
                trait.Property = prop;
            }
            if (trait.IsGetter) prop.Getter = method;
            else prop.Setter = method;
            prop.ResolveTypeAndParameters();
        }
        #endregion

        Parameter CreateRestParam(AbcMethod m, int paramNum, int i)
        {
            string vparam = GetVectorTypeParam(m);
            if (vparam != null)
            {
                var type = GetSystemType(vparam);
                string name = "item" + (i + 1);
                return new Parameter(type, name, paramNum + i + 1);
            }
            else
            {
                string name = GetRestPrefix(m) + i;
                return new Parameter(SystemTypes.Object, name, paramNum + i + 1);
            }
        }

    	private const int MaxRestCount = 10;

    	static bool UseArg(AbcMethod m)
        {
            if (IsFunctionCall(m)) return true;
            if (IsFunctionApply(m)) return true;
            if (IsExternalInterfaceCall(m)) return true;
            return false;
        }

        static string GetRestPrefix(AbcMethod m)
        {
            if (UseArg(m)) return "arg";
            return "rest";
        }

        static Method CopyMethod(IMethod m, int paramNum)
        {
            var method = new Method
                             {
                                 IsSpecialName = m.IsSpecialName,
                                 IsStatic = m.IsStatic,
                                 IsAbstract = m.IsAbstract,
                                 Visibility = m.Visibility,
                                 Name = m.Name,
                                 IsInternalCall = m.IsInternalCall,
                                 IsVirtual = m.IsVirtual,
                                 IsNewSlot = m.IsNewSlot,
                                 Type = m.Type,
                                 Association = m.Association
                             };

            CopyAttrs(m, method);
            
            //copy params
            int n = m.Parameters.Count;
            if (paramNum >= 0)
                n = paramNum;
            for (int i = 0; i < n; ++i)
            {
                var p = (IParameter)m.Parameters[i].Clone();
                //NOTE: Overloads will have not doc.
                p.Documentation = null;
                method.Parameters.Add(p);
            }

            //NOTE: Overloads will have no doc.
            //method.Documentation = m.Documentation;
            //method.ReturnDocumentation = m.ReturnDocumentation;
            return method;
        }

        static Method CreateOverload(IMethod m, int paramNum)
        {
            var m2 = CopyMethod(m, paramNum);
            m2.IsVirtual = false;
            m2.IsNewSlot = false;
            return m2;
        }

        IType BuildReturnType(AbcMethod m)
        {
            LoadFixes();

            if (m.IsGetter && IsDate(m.Instance))
            {
                //TODO: check number
                return SystemTypes.Int32;
            }

            var type = BuildVectorReturnType(m);
            if (type != null)
                return type;

            string name = m.FullName;
            if (!string.IsNullOrEmpty(name))
            {
                name = (string)_returnFix[name];
                if (name != null)
                    return GetSystemType(name);
            }

            return BuildMemberType(m.ReturnType);
        }

        static IProperty FindProperty(IEnumerable<AbcTrait> traits, string name)
        {
            if (traits == null) return null;
            //can be two traits (getter and setter) with the same property
            int n = 0;
            foreach (var trait in traits)
            {
                if (trait.IsAccessor && trait.Name.NameString == name)
                {
                    var prop = trait.Property;
                    if (prop != null) return prop;
                    if (n == 1) return null;
                    ++n;
                }
            }
            return null;
        }
        #endregion

        #region BuildField
        void BuildField(IType declType, AbcTrait trait)
        {
            bool isConst = trait.Kind == AbcTraitKind.Const;
            string name = trait.Name.NameString;
            if (isConst && name == "length") return;

            if (trait.SlotType == null)
                return;

            //IType type = null;
            //object value;
            //if (isConst)
            //{
            //    IAbcConst c = trait.SlotValue as IAbcConst;
            //    if (c != null)
            //    {
            //        value = c.Value;
            //    }
            //    else
            //    {
            //        return;
            //    }
            //    if (value != null)
            //    {
            //        TypeCode tc = Type.GetTypeCode(value.GetType());
            //        type = SystemTypes.GetType(tc);
            //    }
            //}

            //if (type == null)
                //type = BuildMemberType(trait.SlotType);

            var type = BuildMemberType(trait.SlotType);

            var field = new Field();
            InitTypeMember(field, trait);

            //TODO: avoid warning about using "new" keyword
            //FIX: This is fix for field and accessor name conflict.
            var instance = trait.Instance;
            if (instance.GetAllTraits().Any(t => t != trait && t.Name.NameString == trait.Name.NameString))
            {
                if (field.IsStatic)
                    field.Name = "st_" + field.Name;
                else
                    field.Name = "m_" + field.Name;
            }

            //field.IsConstant = isConst;
            if (isConst)
                field.IsStatic = true;
            field.Type = type;

            //if (value != null)
            //{
            //    if (value is string)
            //    {
            //        field.Value = value;
            //    }
            //    else
            //    {
            //        //Use zero to avoid compiler errors
            //        field.Value = Zero.GetValue(value.GetType());
            //    }
            //}

            DefinePfxAttributes(declType, field, trait);

            var elem = FindDocElement(trait);
            field.Documentation = GetSummary(elem);
            
            declType.Members.Add(field);
        }
        #endregion

        #region Custom Attributes
        static void DefineAttribute(ICustomAttributeProvider owner, string typeName, params object[] args)
        {
            var attr = new CustomAttribute(typeName) {Owner = owner};

            if (args != null)
            {
                int n = args.Length;
                for (int i = 0; i < n; i += 2)
                {
                    string name = args[i] as string;
                    if (string.IsNullOrEmpty(name))
                        throw new InvalidOperationException();
                    var value = args[i + 1];
                    attr.Arguments.Add(new Argument(name, value));
                }
            }

            owner.CustomAttributes.Add(attr);
        }

        static void DefineIndexAttribute(ICustomAttributeProvider owner, string typeName, int index)
        {
            DefineAttribute(owner, typeName, "index", index);
        }

        static void DefineABCAttribute(ICustomAttributeProvider owner)
        {
            DefineAttribute(owner, Attrs.ABC);
        }

        static void DefineQNameAttribute(ITypeMember owner, AbcMultiname name)
        {
            var ns = name.Namespace;

            var type = owner as IType;
            if (type != null)
            {
                if (type.Namespace == ns.NameString && ns.Kind == AbcConstKind.PackageNamespace)
                    return;
            }

            if (ns.IsGlobalPackage)
            {
                if (type == null && owner.Name == name.NameString)
                    return;

                DefineAttribute(owner, Attrs.QName,
                                "name", name.NameString);
            }
            else
            {
                DefineAttribute(owner, Attrs.QName,
                                "name", name.NameString,
                                "ns", ns.NameString,
                                "nskind", AbcNamespace.GetShortNsKind(ns.Kind));
            }
        }

        void DefineFPAttribute(IType declType, ITypeMember owner, AbcMultiname name)
        {
            if (!_useFPAttrs) return;

            AbcInstance instance;
            var type = owner as IType;
            if (type != null)
            {
                instance = type.Tag as AbcInstance;
                if (instance != null)
                {
                    float version = GetTypeFPVersion(name);
                    DefineFPAttribute(type, version);
                    instance.FlashVersion = version;
                }
                return;
            }

            instance = declType.Tag as AbcInstance;
            if (instance != null)
            {
                if (instance.FlashVersion == 9)
                {
                    float version = GetTraitFPVersion(instance.Name, name);
                    DefineFPAttribute(owner, version);
                }
                else if (instance.FlashVersion > 0)
                {
                    DefineFPAttribute(owner, instance.FlashVersion);
                }
            }
            else if (declType.Tag is GlobalType)
            {
                float version = GetGlobalTraitFPVersion(name);
                DefineFPAttribute(owner, version);
            }
        }

        static void DefineFPAttribute(ICustomAttributeProvider owner, float version)
        {
        	if (version == 9)
        	{
        		DefineAttribute(owner, Attrs.FP9);
				return;
        	}
        	if (version == 10)
        	{
        		DefineAttribute(owner, Attrs.FP10);
				return;
        	}
        	DefineAttribute(owner, Attrs.FP, "version", version.ToString(CultureInfo.InvariantCulture));
        }

    	void DefinePfxAttributes(IType declType, ITypeMember owner, AbcMultiname name)
        {
            var type = owner as IType;
            if (type != null)
            {
                var instance = type.Tag as AbcInstance;
                if (instance != null)
                {
                    DefineSwcAbcFileAttribute(owner, instance.Abc);
                    DefineIndexAttribute(owner, Attrs.AbcInstance, instance.Index);
                }
            }

            DefineABCAttribute(owner);
            DefineQNameAttribute(owner, name);
            DefineFPAttribute(declType, owner, name);
        }

        void DefinePfxAttributes(IType declType, ITypeMember owner, AbcTrait trait)
        {
            DefineTraitIndexAttribute(owner, trait);
            DefinePfxAttributes(declType, owner, trait.Name);
        }

        void DefineTraitIndexAttribute(ICustomAttributeProvider owner, AbcTrait trait)
        {
            if (trait.Owner is AbcInstance)
            {
                DefineIndexAttribute(owner, Attrs.AbcInstanceTrait, trait.Index);
                return;
            }

            if (trait.Owner is AbcClass)
            {
                DefineIndexAttribute(owner, Attrs.AbcClassTrait, trait.Index);
                return;
            }

            var script = trait.Owner as AbcScript;
            if (script != null)
            {
                DefineSwcAbcFileAttribute(owner, script.ABC);
                DefineIndexAttribute(owner, Attrs.AbcScript, script.Index);
                DefineIndexAttribute(owner, Attrs.AbcScriptTrait, trait.Index);
                return;
            }
        }

        void DefineSwcAbcFileAttribute(ICustomAttributeProvider owner, AbcFile abc)
        {
            if (_swc == null) return;
            if (abc.SWF == null) return;
            int lib = abc.SWF.Index;
            if (lib == 0)
                DefineAttribute(owner, Attrs.SwcAbcFile, "file", abc.Index);
            else
                DefineAttribute(owner, Attrs.SwcAbcFile, "lib", lib, "file", abc.Index);
        }
        #endregion

        #region API Compatibility Checking
        AbcFile _fp9;

        void LoadFP9()
        {
            if (_fp9 != null) return;
            var rs = GetType().GetResourceStream("fp9.abc");
            _fp9 = new AbcFile(rs);
        }

        bool IsFP9Type(AbcMultiname name)
        {
            if (_fpVersion == 9) return true;
            LoadFP9();
            return _fp9.Instances.Contains(name);
        }

        bool IsFP9Trait(AbcMultiname type, AbcMultiname name)
        {
            if (_fpVersion == 9) return true;
            LoadFP9();
            var instance = _fp9.Instances[type];
            if (instance == null) return false;
            return instance.GetAllTraits().Any(t => Equals(t.Name, name));
        }

        private float GetTypeFPVersion(AbcMultiname type)
        {
            return IsFP9Type(type) ? 9 : _fpVersion;
        }

        private float GetTraitFPVersion(AbcMultiname type, AbcMultiname name)
        {
            return IsFP9Trait(type, name) ? 9 : _fpVersion;
        }

        private float GetGlobalTraitFPVersion(AbcMultiname name)
        {
            if (_fpVersion == 9) return 9;
            LoadFP9();
            if (_fp9.Scripts.SelectMany(script => script.Traits).Any(trait => trait.Kind == AbcTraitKind.Method
                                                                              && Equals(trait.Name, name)))
            {
            	return 9;
            }
            return _fpVersion;
        }
        #endregion

        #region Utils
        static string GetInnerTextOf(XmlNode elem, string childElem)
        {
            if (elem != null)
            {
                var e = elem[childElem];
                if (e != null)
                    return e.InnerText;
            }
            return null;
        }

        static string GetSummary(XmlNode elem)
        {
            return GetInnerTextOf(elem, "summary");
        }

        //Finds xml element with doc for given trait
        private XmlElement FindDocElement(AbcTrait trait)
        {
            string typename = GetTypeName(trait);
            if (string.IsNullOrEmpty(typename))
                return null;

			if (_xdoc)
			{
				return FindMember(trait, typename);
			}

			if (_doc == null || _doc.DocumentElement == null) return null;

        	string xpath = string.Format("type[@name='{0}']/trait[@name='{1}']",
                                         typename, trait.Name.NameString);
            var elem = _doc.DocumentElement.SelectSingleNode(xpath) as XmlElement;
            return elem;
        }

        private XmlElement FindMember(AbcTrait trait, string typename)
        {
            string memberName = GetMemberName(trait, typename);
            return FindMember(memberName);
        }

        private XmlElement FindMember(string memberName)
        {
            if (_doc == null || _doc.DocumentElement == null) return null;
            if (string.IsNullOrEmpty(memberName)) return null;

            var members = _doc.DocumentElement["members"];
            if (members == null) return null;

        	return (from elem in members.ChildNodes.OfType<XmlElement>()
					where elem.LocalName == "member"
					let name = elem.GetAttribute("name")
					where name.StartsWith(memberName)
					select elem).FirstOrDefault();
        }

        private static string GetMemberName(AbcTrait trait, string typename)
        {
            switch (trait.Kind)
            {
                case AbcTraitKind.Const:
                case AbcTraitKind.Slot:
                    return "F:" + typename + "." + trait.NameString;

                case AbcTraitKind.Getter:
                case AbcTraitKind.Setter:
                    return "P:" + typename + "." + trait.NameString;

                case AbcTraitKind.Function:
                case AbcTraitKind.Method:
                    return "M:" + typename + "." + trait.NameString;

                case AbcTraitKind.Class:
                    return "T:" + trait.Name.FullName;
            }
            return null;
        }

        static string GetTypeName(AbcInstance instance)
        {
            if (instance == null) return null;
            string name = instance.FullName;
            return instance.Name.Namespace.IsGlobalPackage ? "Avm." + name : name;
        }

        string GetTypeName(AbcTrait trait)
        {
            if (_xdoc)
                return GetTypeName(trait.Instance);

            string typename = trait.OwnerFullName;
            if (!string.IsNullOrEmpty(typename))
                return typename;

            typename = "";
            string ns = trait.Name.NamespaceString;
            if (!string.IsNullOrEmpty(ns))
            {
                typename += ns;
                typename += ".";
            }
            typename += "Global";
            return typename;
        }

        static bool IsAnyType(ISwfIndexedAtom name)
        {
            if (name == null) return true;
            if (name.Index == 0) return true;
            return false;
        }

        static bool IsVisible(AbcTrait t)
        {
            string ns = t.Name.NamespaceString;
            //mx_internal = http://www.adobe.com/2006/flex/mx/internal
            if (ns.EndsWith("mx/internal"))
                return false;
            return IsVisible(t.Visibility);
        }

        static bool IsVisible(Visibility v)
        {
            switch (v)
            {
                case Visibility.NestedProtected:
                case Visibility.NestedProtectedInternal:
                case Visibility.NestedPublic:
                case Visibility.Protected:
                case Visibility.ProtectedInternal:
                case Visibility.Public:
                    return true;
            }
            return false;
        }

        static void InitTypeMember(ITypeMember m, AbcTrait trait)
        {
            m.IsStatic = trait.IsStatic;
            m.Visibility = trait.Visibility;
            m.Name = trait.Name.NameString;
        }

        static bool Is(AbcInstance instance, string fullname)
        {
            if (instance == null) return false;
            return instance.FullName == fullname;
        }

        static bool IsDate(AbcInstance instance)
        {
            return Is(instance, "Date");
        }

        static bool DoNotBuildCtor(AbcInstance instance)
        {
            if (instance == null) return true;
            if (IsDate(instance)) return true;
            //if (Is(instance, "Error")) return true;
            return false;
        }

        private bool IsCoreAPI
        {
            get { return _abcFiles.Any(abc => abc.IsCoreAPI); }
        }

        private string NsPrefix
        {
            get { return IsCoreAPI ? "Avm" : ""; }
        }

        static string SetNamespacePrefix(string ns, string prefix)
        {
            if (string.IsNullOrEmpty(prefix))
                return ns;
            if (string.IsNullOrEmpty(ns))
                return prefix;
            return prefix + "." + ns;
        }
        #endregion
    }
}
