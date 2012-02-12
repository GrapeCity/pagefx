using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace DataDynamics.PageFX.CodeModel.Syntax
{
    internal class SyntaxWriter
    {
        #region Fields
        string _tab = "";
        readonly TextWriter _writer;
        #endregion

        #region Constructors
        public SyntaxWriter(TextWriter writer)
        {
            _writer = writer;
            Add<IAssembly>(WriteAssembly);
            Add<IModule>(WriteModule);
            Add<IModuleCollection>(WriteModuleCollection);
            Add<INamespaceCollection>(WriteNamespaceCollection);
            Add<INamespace>(WriteNamespace);
            Add<IType>(WriteType);
            Add<ITypeCollection>(WriteTypeCollection);
            Add<IFieldCollection>(WriteFieldCollection);
            Add<IField>(WriteField);
            Add<IPropertyCollection>(WritePropertyCollection);
            Add<IProperty>(WriteProperty);
            Add<IEventCollection>(WriteEventCollection);
            Add<IEvent>(WriteEvent);
            Add<IMethodCollection>(WriteMethodCollection);
            Add<IMethod>(WriteMethod);
            Add<IParameter>(WriteParameter);
            Add<IParameterCollection>(WriteParameterCollection);
            Add<ICustomAttribute>(WriteCustomAttribute);
            Add<ICustomAttributeCollection>(WriteCustomAttributeCollection);
            Add<IArgument>(WriteArgument);
            Add<IArgumentCollection>(WriteArgumentCollection);
            Add<IStatement>(WriteStatement);
            Add<IExpression>(WriteExpression);
            Add<IVariable>(WriteVariable);
            Add<IVariableCollection>(WriteVariableCollection);

            AddStatement<IStatementCollection>(WriteStatementCollection);
            AddStatement<IBreakStatement>(WriteBreakStatement);
            AddStatement<IContinueStatement>(WriteContinueStatement);
            AddStatement<IExpressionStatement>(WriteExpressionStatement);
            AddStatement<IReturnStatement>(WriteReturnStatement);
            AddStatement<IVariableDeclarationStatement>(WriteVariableDeclarationStatement);
            AddStatement<IIfStatement>(WriteIfStatement);
            AddStatement<ILoopStatement>(WriteLoopStatement);
            AddStatement<ISwitchStatement>(WriteSwitchStatement);
            AddStatement<ISwitchCase>(WriteSwitchCase);
            AddStatement<ILabeledStatement>(WriteLabeledStatement);
            AddStatement<IGotoStatement>(WriteGotoStatement);
            AddStatement<ITryCatchStatement>(WriteTryCatchStatement);
            AddStatement<ICatchClause>(WriteCatchClause);
            AddStatement<IThrowExceptionStatement>(WriteThrowExceptionStatement);
            AddStatement<IMemoryInitializeStatement>(WriteMemoryInitializeStatement);
            AddStatement<IMemoryCopyStatement>(WriteMemoryCopyStatement);
            
            AddExpression<IThisReferenceExpression>(WriteThisReferenceExpression);
            AddExpression<IBaseReferenceExpression>(WriteBaseReferenceExpression);
            AddExpression<IConstantExpression>(WriteConstExpression);
            AddExpression<ICallExpression>(WriteCallExpression);
            AddExpression<ITypeReferenceExpression>(WriteTypeReferenceExpression);
            AddExpression<IMethodReferenceExpression>(WriteMethodReferenceExpression);
            AddExpression<IArgumentReferenceExpression>(WriteArgumentReferenceExpression);
            AddExpression<IPropertyReferenceExpression>(WritePropertyReferenceExpression);
            AddExpression<IFieldReferenceExpression>(WriteFieldReferenceExpression);
            AddExpression<IMemberReferenceExpression>(WriteMemberReferenceExpression);
            AddExpression<ICastExpression>(WriteCastExpression);
            AddExpression<IVariableReferenceExpression>(WriteVariableReferenceExpression);
            AddExpression<IBinaryExpression>(WriteBinaryExpression);
            AddExpression<IUnaryExpression>(WriteUnaryExpression);
            AddExpression<IBoxExpression>(WriteBoxExpression);
            AddExpression<IUnboxExpression>(WriteUnboxExpression);
            AddExpression<IConditionExpression>(WriteConditionExpression);
            AddExpression<INewObjectExpression>(WriteNewObjectExpression);
            AddExpression<INewArrayExpression>(WriteNewArrayExpression);
            AddExpression<IExpressionCollection>(WriteExpressionCollection);
            AddExpression<IIndexerExpression>(WriteIndexerExpression);
            AddExpression<IArrayIndexerExpression>(WriteArrayIndexerExpression);
            AddExpression<IArrayLengthExpression>(WriteArrayLengthExpression);
            AddExpression<IAddressOfExpression>(WriteAddressOfExpression);
            AddExpression<IAddressDereferenceExpression>(WriteAddressDereferenceExpression);
            AddExpression<IAddressOutExpression>(WriteAddressOutExpression);
            AddExpression<IAddressRefExpression>(WriteAddressRefExpression);
            AddExpression<INewDelegateExpression>(WriteNewDelegateExpression);
            AddExpression<IDelegateInvokeExpression>(WriteDelegateInvokeExpression);
            AddExpression<ITypeOfExpression>(WriteTypeOfExpression);
            AddExpression<ISizeOfExpression>(WriteSizeOfExpression);
        }

        public SyntaxWriter()
            : this(new StringWriter())
        {
        }

        static readonly Hashtable _keywords = new Hashtable();

        static SyntaxWriter()
        {
            RegisterKeywords("abstract", "event", "new", "struct", "as", "explicit", "null", "switch", "base",
                             "extern", "object", "this", "bool", "false", "operator", "throw", "break", "finally",
                             "out", "true", "byte", "fixed", "override", "try", "case", "float", "params", "typeof",
                             "catch", "for", "private", "uint", "char", "foreach", "protected", "ulong", "checked",
                             "goto", "public", "unchecked", "class", "if", "readonly", "unsafe", "const", "implicit",
                             "ref", "ushort", "continue", "in", "return", "using", "decimal", "int", "sbyte", "virtual",
                             "default", "interface", "sealed", "volatile", "delegate", "internal", "short", "void", "do",
                             "is", "sizeof", "while", "double", "lock", "stackalloc", "else", "long", "static", "enum",
                             "namespace", "string");
        }

        static void RegisterKeywords(params string[] keywords)
        {
            foreach (var k in keywords)
            {
                _keywords[k] = 1;
            }
        }
        #endregion

        #region Public Properties
        public string Language
        {
            get { return _lang; }
            set { _lang = value; }
        }
        string _lang;

        public FormatMode Mode
        {
            get { return _mode; }
            set { _mode = value; }
        }
        FormatMode _mode;

        bool _useRegions;
        //bool _sortMembers;
        #endregion

        #region Common API
        readonly Stack<ICodeNode> _scopeStack = new Stack<ICodeNode>();

        void PushScope(ICodeNode node)
        {
            _scopeStack.Push(node);
        }

        void PopScope()
        {
            _scopeStack.Pop();
        }

        ICodeNode Scope
        {
            get
            {
                if (_scopeStack.Count > 0)
                    return _scopeStack.Peek();
                return null;
            }
        }

        public string IndentChars
        {
            get { return _indentChars; }
            set { _indentChars = value; }
        }
        string _indentChars = "    ";

        public void Indent()
        {
            _tab += _indentChars;
        }

        public void Unindent()
        {
            if (_tab.Length > 0)
            {
                _tab = _tab.Substring(0, _tab.Length - _indentChars.Length);
            }
        }

        int _length;

        public void Write(string s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                _writer.Write(s);
                _length += s.Length;
            }
        }

        public void Write(string format, params object[] args)
        {
            Write(string.Format(format, args));
        }

        public void WriteLine()
        {
            Write(_writer.NewLine);
        }

        public void WriteLine(string s)
        {
            Write(s);
            WriteLine();
        }

        public void WriteLine(string format, params object[] args)
        {
            WriteLine(string.Format(format, args));
        }

        public void WriteTab()
        {
            Write(_tab);
        }

        public string FormatEnum<T>(T value)
        {
            return SyntaxFormatter.FormatEnum(_lang, value);
        }

        string GetKeyword<T>(T value)
        {
            return FormatEnum(value);
        }

        public void WriteEnum<T>(T value)
        {
            string s = FormatEnum(value);
            Debug.Assert(s != null);
            Write(s);
        }

        public bool WriteKeyword<T>(T value, bool space)
        {
            string s = FormatEnum(value);
            if (!string.IsNullOrEmpty(s))
            {
                Write(s);
                if (space) Write(" ");
                return true;
            }
            return false;
        }

        public bool WriteKeyword<T>(T value)
        {
            return WriteKeyword(value, true);
        }

        public bool WritePunctuator(Punctuator p)
        {
            string s = FormatEnum(p);
            if (!string.IsNullOrEmpty(s))
            {
                Write(s);
                return true;
            }
            return false;
        }

        public void BeginNamespace(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                WriteTab();
                WriteKeyword(Keyword.Namespace);
                Write(name);
                WriteLine();

                WriteTab();
                WritePunctuator(Punctuator.BeginNamespace);
                WriteLine();
                Indent();
            }
        }

        public void EndNamespace(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                Unindent();
                WriteTab();
                WritePunctuator(Punctuator.EndNamespace);
                WriteLine();
            }
        }

        public void BeginType(IType type)
        {
            WriteLine();
            WriteTab();
            WritePunctuator(Punctuator.BeginType);
            WriteLine();
            Indent();
        }

        public void EndType(IType type)
        {
            if (!_useRegions)
                WriteLine();
            Unindent();
            WriteTab();
            WritePunctuator(Punctuator.EndType);
            WriteLine();
        }

        readonly Stack<string> _regions = new Stack<string>();

        public void BeginRegion(string name)
        {
            string s = FormatEnum(Punctuator.BeginRegion);
            if (!string.IsNullOrEmpty(s))
            {
                WriteTab();
                Write(s);
                Write(" ");
                Write(name);
                WriteLine();
                _regions.Push(name);
            }
        }

        public void EndRegion()
        {
            if (_regions.Count > 0)
            {
                //WriteLine();
                WriteTab();
                WritePunctuator(Punctuator.EndRegion);
                WriteLine();
                _regions.Pop();
            }
        }
        #endregion

        #region ICodeNode
        void Add<T>(Action<T> method)
            where T : class, ICodeNode
        {
            _writers.Add(method);
        }

        void AddStatement<T>(Action<T> method)
            where T : class, IStatement
        {
            _statements.Add(method);
        }

        void AddExpression<T>(Action<T> method)
            where T : class, IExpression
        {
            _expressions.Add(method);
        }

        readonly ActionSelector _writers = new ActionSelector();
        readonly ActionSelector _statements = new ActionSelector();
        readonly ActionSelector _expressions = new ActionSelector();

        public void Init(string format)
        {
            if (format == null)
                format = "lang = c#";

            var dic = ParseHelper.ParseKeyValuePairs(format, "=", ",;");

            string value;
            if (dic.TryGetValue("lang", out value)
                || dic.TryGetValue("language", out value))
                Language = value;
            else
                Language = "c#";

            if (dic.TryGetValue("mode", out value))
            {
                if (string.Compare(value, "short", true) == 0)
                    Mode = FormatMode.ShortDeclaration;
                else if (string.Compare(value, "decl", true) == 0)
                    Mode = FormatMode.FullDeclaration;
                else
                    Mode = FormatMode.Full;
            }

            if (dic.TryGetValue("region", out value))
            {
                if (value == "+" || value == "1" || string.Compare(value, "true", true) == 0)
                    _useRegions = true;
            }
        }

        public void Write(ICodeNode node, string format)
        {
            Init(format);
            Write(node);
        }

        public void Write(ICodeNode node)
        {
            if (!_writers.Run(node))
                throw new NotImplementedException();
        }

        void WriteKids(ICodeNode node)
        {
            var kids = node.ChildNodes;
            if (kids != null)
            {
                WriteNodes(kids, null);
            }
        }

        void WriteVariable(IVariable v)
        {
            BeginStatement();
            WriteReferenceName(v.Type);
            Write(" ");
            Write(v.Name);
            EndStatement();
        }

        void WriteVariableCollection(IVariableCollection collection)
        {
            bool sep = false;
            foreach (var v in collection)
            {
                if (sep) WriteTab();
                WriteVariable(v);
                sep = true;
            }
        }
        #endregion

        #region Names
        public string GetName(IType type)
        {
            //TODO:
            return type.Name;
        }

        string GetActiveNamespace()
        {
            if (_scopeStack.Count > 0)
            {
                var node = _scopeStack.Peek();
                var ns = node as INamespace;
                if (ns != null) return ns.Name;
                var type = node as IType;
                if (type != null) return type.Namespace;
            }
            return null;
        }

        string GetNamespace(IType type)
        {
            string ns = GetActiveNamespace();
            string tns = type.Namespace;
            if (string.IsNullOrEmpty(ns)) return tns;
            if (type.Namespace == ns) return "";
            return tns;
        }

        string GetFullName(IType type)
        {
            return type.DisplayName;
        }

        public string GetReferenceName(IType type)
        {
            var kind = type.TypeKind;
            switch (kind)
            {
                case TypeKind.Array:
                    {
                        var arr = (IArrayType)type;
                        return GetReferenceName(arr.ElementType) + arr.Dimensions;
                    }

                case TypeKind.Pointer:
                    {
                        var ctype = (ICompoundType)type;
                        return GetReferenceName(ctype.ElementType) + "*";
                    }

                case TypeKind.Reference:
                    {
                        var ctype = (ICompoundType)type;
                        return "ref " + GetReferenceName(ctype.ElementType);
                    }

                default:
                    {
                        string k = UserDefinedType.GetKeyword(_lang, type);
                        if (!string.IsNullOrEmpty(k))
                            return k;
                        return GetFullName(type);
                    }
            }
        }

        void WriteReferenceName(IType type)
        {
            if (type == null)
            {
                Write("object");
                return;
            }
            Write(GetReferenceName(type));
        }

        void WriteFullName(IType type)
        {
            Write(GetFullName(type));
        }

        void WriteTypeName(IType type)
        {
            WriteID(GetName(type));
        }

        void WriteID(string name)
        {
            if (_keywords.Contains(name))
                Write("@");
            Write(name);
        }
        #endregion

        #region Type Members
        #region IAssembly
        readonly Hashtable _usings = new Hashtable();
        const string System_Runtime_CompilerServices = "System.Runtime.CompilerServices";

        void WriteUsings(IAssembly asm)
        {
            WriteUsingNamespace("System");
            WriteUsingNamespace(System_Runtime_CompilerServices);
            WriteLine();
        }

        void WriteUsingNamespace(string ns)
        {
            if (!_usings.Contains(ns))
            {
                _usings[ns] = 1;
                WriteLine("using {0};", ns);
            }
        }

        IAssembly _assembly;

        void WriteAssembly(IAssembly asm)
        {
            _assembly = asm;

            WriteUsings(asm);

            BeginRegion("AssemblyInfo");
            WriteLine("[assembly: System.Reflection.AssemblyVersion(\"{0}\")]", asm.Version);
            if (asm.CustomAttributes.Count > 0)
            {
                WriteCustomAttributeCollection(asm.CustomAttributes);
                WriteLine();
            }
            EndRegion();

            WriteLine();

            WriteKids(asm);
        }
        #endregion

        #region IModule
        void WriteModule(IModule mod)
        {
            WriteKids(mod);
        }
        #endregion

        #region IModuleCollection
        void WriteModuleCollection(IModuleCollection collection)
        {
            WriteKids(collection);
        }
        #endregion

        #region INamespaceCollection
        public void WriteNamespaceCollection(INamespaceCollection list)
        {
            WriteKids(list);
        }
        #endregion

        #region INamespace
        public void WriteNamespace(INamespace ns)
        {
            PushScope(ns);
            BeginNamespace(ns.Name);
            WriteNodes(ns.Types, "");
            EndNamespace(ns.Name);
            PopScope();
        }
        #endregion

        #region IType
        public static ClassSemantics GetClassSemantics(IType type)
        {
            if (type.TypeKind == TypeKind.Class)
            {
                if (type.IsStatic) return ClassSemantics.Static;
                if (type.IsAbstract) return ClassSemantics.Abstract;
                if (type.IsSealed) return ClassSemantics.Sealed;
            }
            return ClassSemantics.None;
        }

        static bool HasCustomAttributes(ICustomAttributeProvider p)
        {
            if (p == null) return false;
            if (HasImplAttribute(p as IMethod)) return true;
            if (p.CustomAttributes == null) return false;
            return p.CustomAttributes.Count > 0;
        }

        bool IsFull
        {
            get { return _mode == FormatMode.Full || _mode == FormatMode.FullDeclaration; }
        }

        void WriteAttributes(ICustomAttributeProvider p)
        {
            if (p == null) return;
            if (IsFull)
            {
                if (p.CustomAttributes.Count > 0)
                {
                    WriteCustomAttributeCollection(p.CustomAttributes);
                    WriteLine();
                }
            }
        }

        static bool HasBase(IType type)
        {
            if (type.TypeKind == TypeKind.Interface) return false;
            if (type.TypeKind == TypeKind.Struct) return false;
            var baseType = type.BaseType;
            if (baseType == null) return false;
            if (baseType == SystemTypes.Object) return false;
            return true;
        }

        static IMethod IsDelegate(IType type)
        {
            if (type.TypeKind != TypeKind.Delegate) return null;
            var list = type.Methods["Invoke"];
            if (list == null) return null;
            return Algorithms.First(list);
        }

        void WriteType(IType type)
        {
            if (!ExportService.CanWrite(type))
                return;

            string src = type.SourceCode;
            if (!string.IsNullOrEmpty(src))
            {
                WriteCustomCode(src);
                return;
            }

            bool full = _mode == FormatMode.Full;

            if (full && _assembly == null)
                WriteUsings(type.Assembly);

            bool writeNs = full && Scope == null;
            if (writeNs)
                BeginNamespace(type.Namespace);

            PushScope(type);

            if (full)
            {
                WriteSummary(type.Documentation);
            }

            WriteAttributes(type);

            bool fulldecl = full || _mode == FormatMode.FullDeclaration;

            //declaration
            WriteTab();
            WriteKeyword(type.Visibility);

            var invoke = IsDelegate(type);
            if (invoke != null)
            {
                WriteKeyword(type.TypeKind);
                WriteReferenceName(invoke.Type);
                Write(" ");
                WriteTypeName(type);

                WritePunctuator(Punctuator.BeginParams);
                WriteParameterCollection(invoke.Parameters);
                WritePunctuator(Punctuator.EndParams);
                WriteLine(";");
            }
            else
            {
                WriteKeyword(GetClassSemantics(type));
                WriteKeyword(type.TypeKind);

                if (!full)
                {
                    if (!string.IsNullOrEmpty(type.Namespace))
                    {
                        Write(type.Namespace);
                        Write(".");
                    }
                }
                WriteTypeName(type);

                if (fulldecl)
                    WriteBaseTypes(type);

                if (full)
                {
                    BeginType(type);
                    bool old = _doubleEOL;
                    _doubleEOL = true;
                    WriteKids(type);
                    _doubleEOL = old;
                    WriteCustomCode(type.CustomMembers);
                    EndType(type);
                }
            }

            PopScope();

            if (writeNs)
                EndNamespace(type.Namespace);
        }

        void WriteCustomCode(string code)
        {
            if (string.IsNullOrEmpty(code)) return;
            WriteLine();
            WriteLine();
            code = Str.Tabulate(code, _tab);
            Write(code);
            WriteLine();
            WriteLine();
        }

        void WriteBaseTypes(IType type)
        {
            int ifaceNum = type.Interfaces.Count;
            if (HasBase(type))
            {
                WritePunctuator(Punctuator.BaseTypeSeparator);
                WriteFullName(type.BaseType);
                if (ifaceNum > 0)
                {
                    WritePunctuator(Punctuator.InterfacesSeparator);
                }
            }
            else if (ifaceNum > 0)
            {
                WritePunctuator(Punctuator.BeginInterfaces);
            }
            for (int i = 0; i < ifaceNum; ++i)
            {
                if (i > 0) WritePunctuator(Punctuator.InterfaceSeparator);
                WritePunctuator(Punctuator.InterfacePrefix);
                var iface = type.Interfaces[i];
                WriteFullName(iface);
            }
        }
        #endregion

        #region ITypeCollection
        void WriteTypeCollection(ITypeCollection collection)
        {
            foreach (var type in collection)
            {
                WriteType(type);
            }
        }
        #endregion

        #region IEnumerable<ICodeNode>
        bool _doubleEOL;

        void WriteNodes<T>(IEnumerable<T> list, string region) where T : ICodeNode
        {
            bool reg = !string.IsNullOrEmpty(region);
            if (Algorithms.IsEmpty(list))
            {
                if (reg)
                {
                    BeginRegion(region);
                    EndRegion();
                }
            }
            else
            {
                if (reg) BeginRegion(region);
                bool eol = false;
                int len = _length;
                foreach (var node in list)
                {
                    if (eol)
                    {
                        if (_doubleEOL) WriteLine();
                        WriteLine();
                    }
                    len = _length;
                    Write(node);
                    eol = _length > len;
                }
                if (reg)
                {
                    if (_length > len)
                        WriteLine();
                    EndRegion();
                }
            }
        }
        #endregion

        #region IEnumerable<ITypeMember>
        class MemberSet
        {
            public Visibility Visibility;
            public readonly List<ITypeMember> List = new List<ITypeMember>();
        }

        void WriteMembers<T>(IEnumerable<T> members, string region) where T : ITypeMember
        {
            if (_useRegions)
            {
                if (Algorithms.IsEmpty(members)) return;

                var sets = new List<MemberSet>();
                foreach (var m in members)
                {
                    var v = m.Visibility;
                    int i = Algorithms.IndexOf(sets, x => x.Visibility == v);
                    MemberSet set;
                    if (i < 0)
                    {
                    	set = new MemberSet {Visibility = v};
                    	sets.Add(set);
                    }
                    else
                    {
                        set = sets[i];
                    }
                    set.List.Add(m);
                }

                sets.Sort((x, y) => x.Visibility - y.Visibility);

                bool eol = false;
                foreach (var set in sets)
                {
                    if (eol) WriteLine();
                    int len = _length;
                    WriteNodes(CMHelper.Convert(set.List), set.Visibility + " " + region);
                    eol = _length > len;
                }
            }
            else
            {
                //Type type = typeof(T);
                bool old = _doubleEOL;
                _doubleEOL = true;
                WriteNodes(members, null);
                _doubleEOL = old;
            }
        }
        #endregion

        #region Value
        void WriteNull()
        {
            WriteKeyword(Keyword.Null, false);
        }

        void WriteString(string s)
        {
            Write(Escaper.Escape(s));
        }

        static string ToHex(object v)
        {
            return "0x" + ((IFormattable)v).ToString("x", null);
        }

        void WriteValue(object value)
        {
            if (value == null)
            {
                WriteNull();
                return;
            }
            
            var tc = Type.GetTypeCode(value.GetType());
            switch (tc)
            {
                case TypeCode.Char:
                    {
                        char v = (char)value;
                        Write(Escaper.Escape(v.ToString(), false));
                    }
                    break;

                case TypeCode.String:
                    WriteString((string)value);
                    break;

                case TypeCode.Boolean:
                    if ((bool)value) WriteEnum(Keyword.True);
                    else WriteEnum(Keyword.False);
                    break;

                case TypeCode.Int64:
                case TypeCode.UInt64:
                    Write(value + "L");
                    break;

                case TypeCode.Single:
                    {
                        float v = (float)value;
                        if (float.IsNaN(v))
                        {
                            Write("float.NaN");
                            return;
                        }
                        if (float.IsPositiveInfinity(v))
                        {
                            Write("float.PositiveInfinity");
                            return;
                        }
                        if (float.IsNegativeInfinity(v))
                        {
                            Write("float.NegativeInfinity");
                            return;
                        }
                        Write(value + "f");
                    }
                    break;

                case TypeCode.Double:
                    {
                        double v = (double)value;
                        if (double.IsNaN(v))
                        {
                            Write("double.NaN");
                            return;
                        }
                        if (double.IsPositiveInfinity(v))
                        {
                            Write("double.PositiveInfinity");
                            return;
                        }
                        if (double.IsNegativeInfinity(v))
                        {
                            Write("double.NegativeInfinity");
                            return;
                        }
                        Write(value.ToString());
                    }
                    break;

                case TypeCode.Decimal:
                    Write(value + "M");
                    break;

                default:
                    Write(value.ToString());
                    break;
            }
        }
        #endregion

        #region IArgument
        void WriteEnumValue(IType type, object value)
        {
            foreach (var field in type.Fields)
            {
                if (field.IsStatic)
                {
                    if (Equals(value, field.Value))
                    {
                        WriteReferenceName(type);
                        Write(".");
                        Write(field.Name);
                        return;
                    }
                }
            }
            Write("(");
            WriteReferenceName(type);
            Write(")");
            WriteValue(value);
        }

        void WriteArgument(IArgument arg)
        {
            var type = arg.Type;
            if (type != null && type.TypeKind == TypeKind.Enum)
            {
                if (arg.Kind == ArgumentKind.Fixed)
                {
                    WriteEnumValue(arg.Type, arg.Value);
                }
                else
                {
                    Write(arg.Name);
                    Write(" = ");
                    WriteEnumValue(arg.Type, arg.Value);
                }
            }
            else
            {
                if (arg.Kind == ArgumentKind.Fixed)
                {
                    WriteValue(arg.Value);
                }
                else
                {
                    Write(arg.Name);
                    Write(" = ");
                    WriteValue(arg.Value);
                }
            }
        }
        #endregion

        #region IArgumentCollection
        void WriteArgumentCollection(IArgumentCollection collection)
        {
            WriteKids(collection);
        }
        #endregion

        #region ICustomAttribute
        const string SuffixAttribute = "Attribute";

        void WriteCustomAttribute(ICustomAttribute attr)
        {
            if (attr.Type != null && attr.Type.FullName == "System.Reflection.DefaultMemberAttribute")
                return;

            WriteTab();
            Write("[");

            if (attr.Owner is IAssembly)
                Write("assembly: ");

        	string name = attr.Type == null ? attr.TypeName : GetFullName(attr.Type);
            if (name.EndsWith(SuffixAttribute))
                name = name.Substring(0, name.Length - SuffixAttribute.Length);
            Write(name);

            int n = attr.Arguments.Count;
            if (n > 0)
            {
                Write("(");
                for (int i = 0; i < n; ++i)
                {
                    if (i > 0) Write(", ");
                    WriteArgument(attr.Arguments[i]);
                }
                Write(")");
            }

            Write("]");
        }
        #endregion

        #region ICustomAttributeCollection
        void WriteCustomAttributeCollection(ICustomAttributeCollection attrs)
        {
            bool old = _doubleEOL;
            _doubleEOL = false;
            WriteNodes(attrs, null);
            _doubleEOL = old;
        }
        #endregion

        #region IFieldCollection
        void WriteFieldCollection(IFieldCollection list)
        {
            WriteMembers(list, "Fields");
        }
        #endregion

        #region IField
        void WriteField(IField field)
        {
            if (IsFull)
            {
                WriteSummary(field.Documentation);
                WriteAttributes(field);
            }

            WriteTab();
            WriteKeyword(field.Visibility);

            if (field.IsConstant)
            {
                WriteKeyword(Keyword.Const);
            }
            else
            {
                if (field.IsStatic)
                    WriteKeyword(Keyword.Static);
                if (field.IsReadOnly)
                    WriteKeyword(Keyword.ReadOnly);
            }

            WriteKeyword(Keyword.Field);

            WriteReferenceName(field.Type);
            Write(" ");
            WriteID(field.Name);

            if (field.Value != null)
            {
                Write(" = ");
                WriteValue(field.Value);
            }

            Write(";");
        }
        #endregion

        #region IMethodCollection
        void WriteMethodCollection(IMethodCollection list)
        {
            WriteMembers(list, "Methods");
        }
        #endregion

        #region IMethod
        void WriteParameterCollection(IParameterCollection list)
        {
            bool sep = false;
            foreach (var p in list)
            {
                if (sep) WritePunctuator(Punctuator.ParameterSeparator);
                WriteParameter(p);
                sep = true;
            }
        }

        void WriteParameter(IParameter p)
        {
            WriteCustomAttributeCollection(p.CustomAttributes);

            if (p.HasParams)
            {
                Write("params ");
            }

            if (p.Type != null)
            {
                WriteReferenceName(p.Type);
                Write(" ");
            }
            else
            {
            	Write(p.HasParams ? "object[] " : "object ");
            }
            Write(p.Name);
        }

        void BeginBlock()
        {
            WriteLine();
            WriteTab();
            WritePunctuator(Punctuator.BeginBlock);
            WriteLine();
            Indent();
        }

        void EndBlock()
        {
            WriteLine();
            Unindent();
            WriteTab();
            WritePunctuator(Punctuator.EndBlock);
        }

        static bool HasBody(IMethod method)
        {
            if (method.IsAbstract) return false;
            if (method.Body == null) return false;
            return true;
        }

        static bool IsExtern(IMethod method)
        {
            if (method == null) return false;
            if (method.IsInternalCall) return true;
            if (method.IsForwardRef) return true;
            if (method.PInvoke) return true;
            return false;
        }

        static bool IsExtern(IPolymorphicMember m)
        {
            var method = m as IMethod;
            if (method != null)
                return IsExtern(method);
            var prop = m as IProperty;
            if (prop != null)
                return IsExtern(prop.Getter) || IsExtern(prop.Setter);
            return false;
        }

        static bool HasImplAttribute(IMethod method)
        {
            if (method == null) return false;
            if (method.CodeType != MethodCodeType.IL) return true;
            if (method.IsInternalCall) return true;
            if (method.IsForwardRef) return true;
            if (method.IsPreserveSig) return true;
            if (method.IsSynchronized) return true;
            if (method.NoInlining) return true;
            return false;
        }

        void WriteFullDecl(string ns, string decl)
        {
            if (!string.IsNullOrEmpty(ns))
            {
                if (!_usings.Contains(ns))
                {
                    Write(ns);
                    Write(".");
                }
            }
            Write(decl);
        }

        void WriteImplOpt(string name, bool sep)
        {
            if (sep) Write(" | ");
            WriteFullDecl(System_Runtime_CompilerServices, "MethodImplOptions." + name);
        }

        static MethodImplOptions GetImplOptions(IMethod m)
        {
            MethodImplOptions res = 0;
            if (m.IsInternalCall)
                res |= MethodImplOptions.InternalCall;

            if (m.IsForwardRef)
                res |= MethodImplOptions.ForwardRef;

            if (m.IsPreserveSig)
                res |= MethodImplOptions.PreserveSig;

            if (m.IsSynchronized)
                res |= MethodImplOptions.Synchronized;

            if (m.NoInlining)
                res |= MethodImplOptions.NoInlining;

            return res;
        }

        void WriteImplOptions(MethodImplOptions v, ref bool sep)
        {
            if ((v & MethodImplOptions.InternalCall) != 0)
            {
                WriteImplOpt("InternalCall", sep);
                sep = true;
            }
            if ((v & MethodImplOptions.ForwardRef) != 0)
            {
                WriteImplOpt("ForwardRef", sep);
                sep = true;
            }
            if ((v & MethodImplOptions.PreserveSig) != 0)
            {
                WriteImplOpt("PreserveSig", sep);
                sep = true;
            }
            if ((v & MethodImplOptions.Synchronized) != 0)
            {
                WriteImplOpt("Synchronized", sep);
                sep = true;
            }
            if ((v & MethodImplOptions.NoInlining) != 0)
            {
                WriteImplOpt("NoInlining", sep);
                sep = true;
            }
        }

        void BeginMethodImplAttribute()
        {
            WriteTab();
            Write("[");
            WriteFullDecl(System_Runtime_CompilerServices, "MethodImpl(");
        }

        void WriteMethodImplAttribute(MethodImplOptions opts)
        {
            BeginMethodImplAttribute();
            bool sep = false;
            WriteImplOptions(opts, ref sep);
            Write(")]");
        }

        bool WriteMethodAttributes(IMethod method)
        {
            bool hasAttrs = method.CustomAttributes.Count > 0;
            if (hasAttrs)
                WriteCustomAttributeCollection(method.CustomAttributes);

            if (HasImplAttribute(method))
            {
                if (hasAttrs) WriteLine();

                hasAttrs = true;

                BeginMethodImplAttribute();

                var opts = GetImplOptions(method);
                bool sep = false;
                WriteImplOptions(opts, ref sep);
                
                if (method.CodeType != MethodCodeType.IL)
                {
                    if (sep) Write(", ");
                    WriteFullDecl(System_Runtime_CompilerServices, "MethodCodeType." + method.CodeType);
                }

                Write(")]");
            }
            return hasAttrs;
        }

        IMethod _curMethod;

        static string GetCtorName(IType type)
        {
            return type.Name;
        }

        static string GetAssociationName(IMethod method)
        {
            var assoc = method.Association;
            if (assoc == null) return null;

            var prop = assoc as IProperty;
            if (prop != null)
            {
                if (prop.Getter == method)
                    return "get";
                if (prop.Setter != method)
                    throw new InvalidOperationException();
                return "set";
            }

            var e = assoc as IEvent;
            if (e != null)
            {
                if (e.Adder == method)
                    return "add";
                if (e.Remover == method)
                    return "remove";
                if (e.Raiser != method)
                    throw new InvalidOperationException();
                return "raise";
            }

            throw new NotImplementedException();
        }

        bool _skipAssociatedMethods = true;

        void WriteXmlComment(string text, string tag, params string[] attrs)
        {
            XmlHelper.WriteComments(_writer, text, _tab, tag, attrs);
        }

        void WriteSummary(string text)
        {
            if (string.IsNullOrEmpty(text)) return;
            WriteXmlComment(text, "summary");
        }

        void WriteMethodDocumentation(IMethod method)
        {
            if (method.Association != null) return;

            WriteSummary(method.Documentation);

            int n = method.Parameters.Count;
            for (int i = 0; i < n; ++i)
            {
                var p = method.Parameters[i];
                string name = p.Name ?? "";
            	WriteXmlComment(p.Documentation, "param", "name", name);
            }

            WriteXmlComment(method.ReturnDocumentation, "returns");
        }

        static bool IsNew(IMethod method)
        {
            if (method == null) return false;
            var type = method.DeclaringType;
            var baseType = type.BaseType;
            while (baseType != null)
            {
                foreach (var baseMethod in baseType.Methods)
                {
                    if (Signature.Equals(baseMethod, method, false))
                    {
                        return baseMethod.Type != method.Type;
                    }
                }
                baseType = baseType.BaseType;
            }
            return false;
        }

        static bool IsNew(IPolymorphicMember m)
        {
            var method = m as IMethod;
            if (method != null)
                return IsNew(method);
            var prop = m as IProperty;
            if (prop != null)
                return IsNew(prop.Getter);
            return false;
        }

        void WriteKeywords(IPolymorphicMember m)
        {
            if (m.DeclaringType.TypeKind == TypeKind.Interface)
                return;

            bool writeVisibility = true;
            bool hasassoc = false;
            var method = m as IMethod;
            if (method != null)
            {
                if (method.IsConstructor && m.IsStatic)
                {
                    WriteKeyword(Keyword.Static);
                    return;
                }

                var assoc = method.Association;
                if (assoc != null)
                {
                    hasassoc = true;
                    if (assoc.Visibility == method.Visibility)
                        writeVisibility = false;
                }
            }

            if (writeVisibility)
                WriteKeyword(m.Visibility);

            if (!hasassoc)
            {
                if (IsExtern(m))
                    WriteKeyword(Keyword.Extern);

                if (m.IsStatic)
                {
                    WriteKeyword(Keyword.Static);
                    return;
                }

                if (m.IsAbstract)
                {
                    WriteKeyword(Keyword.Abstract);
                    return;
                }

                if (m.IsVirtual)
                {
                    if (!IsPrivate(m.Visibility))
                    {
                    	WriteKeyword(m.IsNewSlot ? Keyword.Virtual : Keyword.Override);
                    }
                    return;
                }

                if (IsNew(m))
                {
                    WriteKeyword(Keyword.New);
                    return;
                }
            }
        }

        static bool IsPrivate(Visibility v)
        {
            switch(v)
            {
                case Visibility.Private:
                case Visibility.PrivateScope:
                    return true;
            }
            return false;
        }

        void WriteMethod(IMethod method)
        {
            var assoc = method.Association;
            if (assoc != null && _skipAssociatedMethods && Scope != null)
                return;

            bool full = _mode == FormatMode.Full;

            if (full)
            {
                WriteMethodDocumentation(method);
                if (WriteMethodAttributes(method))
                    WriteLine();
            }

            WriteTab();

            bool isCtor = method.IsConstructor;

            WriteKeywords(method);

            if (assoc != null && Scope != null)
            {
                string name = GetAssociationName(method);
                Write(name);
            }
            else
            {
                if (isCtor)
                {
                    WriteID(GetCtorName(method.DeclaringType));
                }
                else
                {
                    WriteReferenceName(method.Type);
                    Write(" ");

                    //NOTE: Usefull to display method signatures in debug mode
                    //TODO: add format option for this
                    //if (!full)
                    //{
                    //    WriteFullName(method.DeclaringType);
                    //    Write(".");
                    //}

                    WriteID(method.Name);
                }

                if (method.IsGenericInstance)
                    WriteGenericArgs(method.GenericArguments);
                else if (method.IsGeneric)
                    WriteGenericParams(method.GenericParameters);

                WritePunctuator(Punctuator.BeginParams);
                WriteParameterCollection(method.Parameters);
                WritePunctuator(Punctuator.EndParams);
            }

            if (full && HasBody(method))
            {
                if (method.Body != null)
                {
                    var body = method.Body.Statements;
                    if (body != null)
                    {
                        _curMethod = method;
                        if (isCtor && !method.IsStatic)
                            WriteCtorBody(body);
                        else
                            WriteBlock(body, 0, 0);
                        _curMethod = null;
                    }
                    else
                    {
                        Write(";");
                    }
                }
            }
            else
            {
                Write(";");
            }
        }

        void WriteGenericParams(IEnumerable<IGenericParameter> parameters)
        {
            if (parameters == null) return;
            bool begin = false;
            foreach (var p in parameters)
            {
                if (begin)
                    Write(",");
                else
                {
                    Write("<");
                    begin = true;
                }
                Write(p.Name);
            }
            if (begin)
                Write(">");
        }

        void WriteGenericArgs(IEnumerable<IType> args)
        {
            if (args == null) return;
            bool begin = false;
            foreach (var arg in args)
            {
                if (begin)
                    Write(",");
                else
                {
                    Write("<");
                    begin = true;
                }
                WriteReferenceName(arg);
            }
            if (begin)
                Write(">");
        }

        static IExpression ToCtorCall(IStatement st)
        {
            var est = st as IExpressionStatement;
            if (est == null) return null;

            var call = est.Expression as ICallExpression;
            if (call == null) return null;

            var target = call.Method.Target;
            if (target is IThisReferenceExpression || target is IBaseReferenceExpression)
                return call;

            return null;
        }

        void WriteCtorBody(IList<IStatement> body)
        {
            bool sep = true;
            bool comma = false;
            int n = body.Count;
            var body2 = new List<IStatement>();
            for (int i = 0; i < n; ++i)
            {
                var st = body[i];
                var call = ToCtorCall(st);
                if (call != null)
                {
                    if (sep)
                    {
                        Write(" : ");
                        sep = false;
                    }
                    if (comma) Write(", ");
                    WriteExpression(call);
                    comma = true;
                }
                else
                {
                    body2.Add(st);
                }
            }
            WriteBlock(body2, 0, 0);
        }
        #endregion

        #region IPropertyCollection
        void WritePropertyCollection(IPropertyCollection list)
        {
            WriteMembers(list, "Properties");
        }
        #endregion

        #region IProperty
        void WriteProperty(IProperty prop)
        {
            bool full = _mode == FormatMode.Full;

            if (full)
            {
                WriteSummary(prop.Documentation);
                WriteAttributes(prop);
            }

            WriteTab();
            WriteKeywords(prop);

            WriteReferenceName(prop.Type);
            Write(" ");

            if (prop.Parameters.Count > 0)
            {
                Write("this[");
                WriteParameterCollection(prop.Parameters);
                Write("]");
            }
            else
            {
                WriteID(prop.Name);
            }

            if (full)
            {
                BeginBlock();

                _skipAssociatedMethods = false;
                if (prop.Getter != null)
                {
                    WriteMethod(prop.Getter);
                    if (prop.Setter != null)
                    {
                        if (HasBody(prop.Getter))
                            WriteLine();
                        if (HasCustomAttributes(prop.Setter))
                            WriteLine();
                    }
                }
                if (prop.Setter != null)
                    WriteMethod(prop.Setter);
                _skipAssociatedMethods = true;

                EndBlock();
            }
            else
            {
                Write("{");
                if (prop.Getter != null)
                    Write(" get;");
                if (prop.Setter != null)
                    Write(" set;");
                Write(" }");
            }
        }
        #endregion

        #region IEventCollection
        void WriteEventCollection(IEventCollection list)
        {
            WriteMembers(list, "Events");
        }
        #endregion

        #region IEvent
        void WriteEvent(IEvent e)
        {
            var declType = e.DeclaringType;
            bool isIface = declType.TypeKind == TypeKind.Interface;

            bool full = _mode == FormatMode.Full;

            if (full)
            {
                WriteSummary(e.Documentation);
                WriteAttributes(e);
            }

            WriteTab();
            if (!isIface)
                WriteKeyword(e.Visibility);
            if (e.IsStatic)
                WriteKeyword(Keyword.Static);
            //if (e.IsFlash)
            //    WriteKeyword(Keyword.Extern);
            Write("event ");

            WriteReferenceName(e.Type);
            Write(" ");

            WriteID(e.Name);

            if (e.IsFlash && !isIface)
            {
                BeginBlock();

                //WriteMethodImplAttribute(MethodImplOptions.InternalCall);
                //WriteLine();
                WriteTab();
                WriteLine("add { }");

                //WriteMethodImplAttribute(MethodImplOptions.InternalCall);
                //WriteLine();
                WriteTab();
                Write("remove { }");
                
                EndBlock();
            }
            else
            {
                Write(";");
            }
        }
        #endregion
        #endregion

        #region IStatement
        /// <summary>
        /// Current statement
        /// </summary>
        IStatement _cst;
        //readonly Stack<IStatement> _statementStack = new Stack<IStatement>();

        //IStatement ParentStatement
        //{
        //    get
        //    {
        //        if (_statementStack.Count > 1)
        //        {
        //            IStatement[] arr = _statementStack.ToArray();
        //            return arr[1];
        //        }
        //        return null;
        //    }
        //}

        void WriteStatement(IStatement st)
        {
            _cst = st;
            //_statementStack.Push(st);
            if (!_statements.Run(st))
            {
                throw new NotImplementedException();
            }
            //_statementStack.Pop();
        }

        void WriteStatementCollection(IStatementCollection collection)
        {
            bool sep = false;
            foreach (var st in collection)
            {
                if (sep) WriteTab();
                int len = _length;
                WriteStatement(st);
                sep = _length > len;
            }
        }

        const int bfOneStatement = 1;
        const int bfElse = 2;

        bool AsOneStatement(IStatement st)
        {
            if (_cst == null) return false;
            var f = _cst as IIfStatement;
            if (f != null)
            {
                if (st is ILoopStatement || st is ITryCatchStatement || st is ISwitchStatement)
                    return false;
                if (f.Else.Count > 1)
                    return false;
                f = st as IIfStatement;
                if (f != null)
                {
                    if (f.Then.Count > 1 || f.Else.Count > 0)
                        return false;
                }
            }
            if (_cst is ILoopStatement)
            {
                if (st is ILoopStatement || st is IIfStatement || st is ITryCatchStatement || st is ISwitchStatement)
                    return false;
            }
            return true;
        }

        void WriteBlock(IList<IStatement> body, int startIndex, int bf)
        {
            int n = body.Count;
            if (startIndex >= n)
            {
                WriteLine();
                WriteTab();
                WritePunctuator(Punctuator.BeginBlock);
                WriteLine();
                WriteTab();
                WritePunctuator(Punctuator.EndBlock);
                return;
            }

            //only one statement
            if (startIndex + 1 >= n && (bf == bfOneStatement || bf == bfElse))
            {
                var st = body[startIndex];
                if (AsOneStatement(st))
                {
                    bool indent = true;
                    if (bf == bfElse && st is IIfStatement)
                    {
                        _elseif = true;
                        indent = false;
                    }
                    else
                    {
                        WriteLine();
                        Indent();
                    }
                    WriteStatement(st);
                    if (indent)
                    {
                        //WriteLine();
                        Unindent();
                    }
                    return;
                }
            }

            BeginBlock();
            bool eol = false;
            for (int i = startIndex; i < n; ++i)
            {
                if (eol) WriteLine();
                int len = _length;
                WriteStatement(body[i]);
                eol = _length > len;
            }
            EndBlock();
        }

        void WriteBlock(string name, IList<IStatement> body, int bf)
        {
            WriteTab();
            Write(name);
            WriteBlock(body, 0, bf);
        }

        void BeginStatement()
        {
            WriteTab();
        }

        void EndStatement()
        {
            Write(";");
        }

        void WriteKeywordStatement(Keyword k)
        {
            BeginStatement();
            WriteKeyword(k, false);
            EndStatement();
        }

        void WriteContinueStatement(IContinueStatement s)
        {
            WriteKeywordStatement(Keyword.Continue);
        }

        void WriteBreakStatement(IBreakStatement s)
        {
            WriteKeywordStatement(Keyword.Break);
        }

        void WriteExpressionStatement(IExpressionStatement s)
        {
            BeginStatement();
            WriteExpression(s.Expression);
            EndStatement();
        }

        void WriteReturnStatement(IReturnStatement s)
        {
            BeginStatement();
            WriteKeyword(Keyword.Return, false);
            if (s.Expression != null)
            {
                Write(" ");
                WriteExpression(s.Expression);
            }
            EndStatement();
        }

        void WriteVariableDeclarationStatement(IVariableDeclarationStatement s)
        {
            BeginStatement();
            WriteReferenceName(s.Variable.Type);
            Write(" ");
            Write(s.Variable.Name);

            //Initialize variable with default value.
            Write(" = default(");
            WriteReferenceName(s.Variable.Type);
            Write(")");

            EndStatement();
        }

        void WriteLabeledStatement(ILabeledStatement s)
        {
            if (s.Statement == null)
                throw new InvalidOperationException();

            BeginStatement();
            Write(s.Name);
            Write(":");
            WriteLine();

            WriteStatement(s.Statement);
        }

        void WriteGotoStatement(IGotoStatement s)
        {
            BeginStatement();
            Write("goto ");
            //TODO: Throw exception for full mode
            var label = s.Label;
            if (label == null)
            {
                Write("NOLABEL");
            }
            else
            {
                var sc = LabelResolver.IsGotoCase(s);
                if (sc != null)
                {
                    Write("case {0}", sc.From);
                }
                else
                {
                    Write(label.Name);
                }
            }
            EndStatement();
        }

        void WriteCondition(IExpression e)
        {
            if (e != null)
                WriteExpression(e);
            else
                Write("(true)");
        }

        bool _elseif;

        void WriteIfStatement(IIfStatement s)
        {
            if (_elseif)
            {
                _elseif = false;
                Write(" ");
            }
            else BeginStatement();
            Write("if (");
            WriteCondition(s.Condition);
            Write(")");
            WriteBlock(s.Then, 0, bfOneStatement);
            if (s.Else.Count > 0)
            {
                WriteLine();
                WriteTab();
                Write("else");
                WriteBlock(s.Else, 0, bfElse);
            }
        }

        void WriteLoopStatement(ILoopStatement s)
        {
            BeginStatement();
            var type = s.LoopType;
            if (type == LoopType.PostTested)
            {
                Write("do");
                WriteBlock(s.Body, 0, 0);
                Write(" while (");
                WriteCondition(s.Condition);
                Write(")");
                EndStatement();
            }
            else
            {
                if (type == LoopType.Endless)
                {
                    Write("while (true)");
                }
                else
                {
                    Write("while (");
                    WriteCondition(s.Condition);
                    Write(")");
                }
                WriteBlock(s.Body, 0, bfOneStatement);
            }
        }

        void WriteSwitchStatement(ISwitchStatement s)
        {
            BeginStatement();
            Write("switch (");
            Write(s.Expression);
            Write(")");
            BeginBlock();
            bool sep = false;
            foreach (var sc in s.Cases)
            {
                if (sep) WriteLine();
                WriteSwitchCase(sc);
                sep = true;
            }
            EndBlock();
        }

        void WriteSwitchCase(ISwitchCase sc)
        {
            for (int i = sc.From; i <= sc.To;  ++i)
            {
                WriteTab();
                Write("case {0}:", i);
                if (i != sc.To)
                    WriteLine();
            }
            WriteBlock(sc.Body, 0, bfOneStatement);
        }

        void WriteTryCatchStatement(ITryCatchStatement s)
        {
            WriteBlock("try", s.Try, 0);
            WriteStatementCollection(s.CatchClauses);
            if (s.Fault.Count > 0)
            {
                WriteTab();
                WriteBlock("fault", s.Finally, 0);
            }
            if (s.Finally.Count > 0)
            {
                WriteTab();
                WriteBlock("finally", s.Finally, 0);
            }
        }

        void WriteCatchClause(ICatchClause c)
        {
            WriteTab();
            Write("catch");
            if (c.Condition != null)
            {
                Write(" (");
                WriteExpression(c.Condition);
                if (c.Variable != null)
                {
                    Write(" ");
                    Write(c.Variable.Name);
                }
                Write(")");
            }
            WriteBlock(c.Body, 0, 0);
        }

        void WriteThrowExceptionStatement(IThrowExceptionStatement s)
        {
            BeginStatement();
            Write("throw");
            if (s.Expression != null)
            {
                Write(" ");
                WriteExpression(s.Expression);
            }
            EndStatement();
        }

        void WriteMemoryInitializeStatement(IMemoryInitializeStatement s)
        {
            BeginStatement();
            EndStatement();
        }

        void WriteMemoryCopyStatement(IMemoryCopyStatement s)
        {
            BeginStatement();
            EndStatement();
        }
        #endregion

        #region IExpression
        void WriteExpression(IExpression e)
        {
            if (!_expressions.Run(e))
                throw new NotImplementedException();
        }

        void WriteConstExpression(IConstantExpression e)
        {
            WriteValue(e.Value);
        }

        void WriteThisReferenceExpression(IThisReferenceExpression e)
        {
            Write(GetKeyword(Keyword.This));
        }

        void WriteBaseReferenceExpression(IBaseReferenceExpression e)
        {
            Write(GetKeyword(Keyword.Base));
        }

        void WriteTypeReferenceExpression(ITypeReferenceExpression e)
        {
            WriteReferenceName(e.Type);
        }

        void WriteVariableReferenceExpression(IVariableReferenceExpression e)
        {
            Write(e.Variable.Name);
        }

        void WriteArgumentReferenceExpression(IArgumentReferenceExpression e)
        {
            Write(e.Argument.Name);
        }

        void WriteMethodReferenceExpression(IMethodReferenceExpression e)
        {
            if (e.Method.IsConstructor)
            {
                WriteExpression(e.Target);
            }
            else
            {
                if (!CanSkipTarget(e))
                {
                    WriteExpression(e.Target);
                    Write(".");
                }
                Write(e.Method.Name);
            }
        }

        bool CanSkipTarget(IMemberReferenceExpression e)
        {
            if (_curMethod == null) return false;
            var tr = e.Target as ITypeReferenceExpression;
            if (tr == null) return false;
            if (tr.Type == _curMethod.DeclaringType) return true;
            return false;
        }

        void WritePropertyReferenceExpression(IPropertyReferenceExpression e)
        {
            WriteMemberReferenceExpression(e);
        }

        void WriteFieldReferenceExpression(IFieldReferenceExpression e)
        {
            WriteMemberReferenceExpression(e);
        }

        void WriteMemberReferenceExpression(IMemberReferenceExpression e)
        {
            if (!CanSkipTarget(e))
            {
                WriteExpression(e.Target);
                Write(".");
            }
            Write(e.Member.Name);
        }

        void WriteExpressions(IEnumerable<IExpression> list, string separator)
        {
            bool sep = false;
            foreach (var e in list)
            {
                if (sep) Write(separator);
                WriteExpression(e);
                sep = true;
            }
        }

        void WriteArguments(IList<IParameter> parameters, IList<IExpression> args, string prefix, string suffix)
        {
            Write(prefix);
            for (int i = 0; i < args.Count; ++i)
            {
                if (i > 0) Write(", ");
                var arg = args[i];
                var p = parameters[i];
                if (p.IsByRef)
                    Write("ref ");
                WriteExpression(arg);
            }
            Write(suffix);
        }

        void WriteMethodArguments(IMethod method, IList<IExpression> args)
        {
            string prefix = "(";
            string suffx = ")";
            if (method == null)
            {
                Write(prefix);
                Write(suffx);
                return;
            }
            WriteArguments(method.Parameters, args, prefix, suffx);
        }

        void WriteCallExpression(ICallExpression e)
        {
            WriteExpression(e.Method);
            WriteMethodArguments(e.Method.Method, e.Arguments);
        }

        void WriteFencedExpression(IExpression e)
        {
            bool cb = ExpressionService.CanBrace(e);
            if (cb) Write("(");
            WriteExpression(e);
            if (cb) Write(")");
        }

        void WriteCastExpression(ICastExpression e)
        {
            var type = e.TargetType;
            var ee = e.Expression;
            var op = e.Operator;
            if (op == CastOperator.Cast)
            {
                Write("(");
                WriteReferenceName(type);
                Write(")");
                WriteFencedExpression(ee);
            }
            else if (op == CastOperator.As)
            {
                WriteFencedExpression(ee);
                Write(" as ");
                WriteReferenceName(type);
            }
            else if (op == CastOperator.Is)
            {
                WriteFencedExpression(ee);
                Write(" is ");
                WriteReferenceName(type);
            }
        }

        void WriteBinaryExpression(IBinaryExpression e)
        {
            bool isNotAssign = e.Operator != BinaryOperator.Assign;
            var left = e.Left;
            if (isNotAssign)
                left = ExpressionService.FixConstant(left, e.Right.ResultType);
            var right = ExpressionService.FixConstant(e.Right, left.ResultType);

            if (isNotAssign && ExpressionService.CanBrace(left))
            {
                Write("(");
                WriteExpression(left);
                Write(")");
            }
            else
            {
                WriteExpression(left);
            }

            Write(" ");
            WriteEnum(e.Operator);
            Write(" ");

            if (isNotAssign && ExpressionService.CanBrace(right))
            {
                Write("(");
                WriteExpression(right);
                Write(")");
            }
            else
            {
                WriteExpression(right);
            }
        }

        void WriteUnaryExpression(IUnaryExpression e)
        {
            if (e.Operator == UnaryOperator.PostIncrement || e.Operator == UnaryOperator.PostDecrement)
            {
                bool br = ExpressionService.CanBrace(e.Expression);
                if (br) Write("(");
                WriteExpression(e.Expression);
                if (br) Write(")");
                WriteEnum(e.Operator);
            }
            else
            {
                WriteEnum(e.Operator);
                bool br = ExpressionService.CanBrace(e.Expression);
                if (br) Write("(");
                WriteExpression(e.Expression);
                if (br) Write(")");
            }
        }

        void WriteBoxExpression(IBoxExpression e)
        {
            WriteExpression(e.Expression);
        }

        void WriteUnboxExpression(IUnboxExpression e)
        {
            Write("(");
            Write("(");
            WriteReferenceName(e.TargetType);
            Write(")");
            WriteExpression(e.Expression);
            Write(")");
        }

        void WriteConditionExpression(IConditionExpression e)
        {
            bool b = ExpressionService.CanBrace(e.Condition);

            if (b) Write("(");
            WriteExpression(e.Condition);
            if (b) Write(")");

            Write(" ? ");

            b = ExpressionService.CanBrace(e.TrueExpression);
            if (b) Write("(");
            WriteExpression(e.TrueExpression);
            if (b) Write(")");

            Write(" : ");

            b = ExpressionService.CanBrace(e.FalseExpression);
            if (b) Write("(");
            WriteExpression(e.FalseExpression);
            if (b) Write(")");
        }

        void WriteExpressionCollection(IExpressionCollection e)
        {
            WriteExpressions(e, ", ");
        }

        void WriteNewObjectExpression(INewObjectExpression e)
        {
            Write("new ");
            WriteReferenceName(e.ObjectType);
            WriteMethodArguments(e.Constructor, e.Arguments);
        }

        void WriteNewArrayExpression(INewArrayExpression e)
        {
            Write("new ");
            WriteReferenceName(e.ElementType);
            Write("[");
            WriteExpressionCollection(e.Dimensions);
            Write("]");
            if (e.Initializers != null && e.Initializers.Count > 0)
            {
                Write(" {");
                WriteExpressionCollection(e.Initializers);
                Write(" }");
            }
        }

        void WriteIndexerExpression(IIndexerExpression e)
        {
            WriteExpression(e.Property.Target);
            Write("[");
            WriteExpressionCollection(e.Index);
            Write("]");
        }

        void WriteArrayIndexerExpression(IArrayIndexerExpression e)
        {
            WriteExpression(e.Array);
            Write("[");
            WriteExpressionCollection(e.Index);
            Write("]");
        }

        void WriteArrayLengthExpression(IArrayLengthExpression e)
        {
            WriteExpression(e.Array);
            Write(".Length");
        }

        void WriteAddressOfExpression(IAddressOfExpression e)
        {
            Write("&");
            bool b = ExpressionService.CanBrace(e.Expression);
            if (b) Write("(");
            WriteExpression(e.Expression);
            if (b) Write(")");
        }

        void WriteAddressDereferenceExpression(IAddressDereferenceExpression e)
        {
            Write("*");
            bool b = ExpressionService.CanBrace(e.Expression);
            if (b) Write("(");
            WriteExpression(e.Expression);
            if (b) Write(")");
        }

        void WriteAddressOutExpression(IAddressOutExpression e)
        {
            Write("out ");
            bool b = ExpressionService.CanBrace(e.Expression);
            if (b) Write("(");
            WriteExpression(e.Expression);
            if (b) Write(")");
        }

        void WriteAddressRefExpression(IAddressRefExpression e)
        {
            Write("ref ");
            bool b = ExpressionService.CanBrace(e.Expression);
            if (b) Write("(");
            WriteExpression(e.Expression);
            if (b) Write(")");
        }

        void WriteNewDelegateExpression(INewDelegateExpression e)
        {
            Write("new ");
            WriteFullName(e.DelegateType);
            Write("(");
            WriteExpression(e.Method);
            Write(")");
        }

        void WriteDelegateInvokeExpression(IDelegateInvokeExpression e)
        {
            Write(e.Target);
            WriteMethodArguments(e.Method, e.Arguments);
        }

        void WriteTypeOfExpression(ITypeOfExpression e)
        {
            Write("typeof(");
            WriteReferenceName(e.Type);
            Write(")");
        }

        void WriteSizeOfExpression(ISizeOfExpression e)
        {
            Write("sizeof(");
            WriteReferenceName(e.Type);
            Write(")");
        }
        #endregion

        #region Object Override Members
        public override string ToString()
        {
            return _writer.ToString();
        }
        #endregion
    }
}