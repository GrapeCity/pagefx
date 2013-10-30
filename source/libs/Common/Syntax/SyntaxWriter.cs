using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.CodeModel.Expressions;
using DataDynamics.PageFX.Common.CodeModel.Statements;
using DataDynamics.PageFX.Common.Collections;
using DataDynamics.PageFX.Common.Extensions;
using DataDynamics.PageFX.Common.Services;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Common.Utilities;

namespace DataDynamics.PageFX.Common.Syntax
{
    internal class SyntaxWriter
    {
	    private string _tab = "";
		private readonly TextWriter _writer;

	    #region Constructors
        public SyntaxWriter(TextWriter writer)
        {
            _writer = writer;
            Add<IAssembly>(WriteAssembly);
            Add<IModule>(WriteModule);
            Add<IModuleCollection>(WriteModuleCollection);
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

        private static readonly Hashtable Keywords = new Hashtable();

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

		private static void RegisterKeywords(params string[] keywords)
        {
            foreach (var k in keywords)
            {
                Keywords[k] = 1;
            }
        }
        #endregion

        #region Public Properties

	    public string Language { get; set; }

	    public FormatMode Mode { get; set; }

	    bool _useRegions;
        //bool _sortMembers;
        #endregion

        #region Common API
        private readonly Stack<ICodeNode> _scopeStack = new Stack<ICodeNode>();

		private void PushScope(ICodeNode node)
        {
            _scopeStack.Push(node);
        }

		private void PopScope()
        {
            _scopeStack.Pop();
        }

        private ICodeNode Scope
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
        private string _indentChars = "    ";

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

		private int _length;

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

        public string FormatEnum<T>(T value) where T:struct 
        {
            return value.EnumString(Language);
        }

        private string GetKeyword<T>(T value) where T:struct 
        {
            return FormatEnum(value);
        }

        public void WriteEnum<T>(T value) where T:struct 
        {
            string s = FormatEnum(value);
            Debug.Assert(s != null);
            Write(s);
        }

        public bool WriteKeyword<T>(T value, bool space) where T:struct 
        {
            string s = FormatEnum(value);
	        return WriteKeyword(s, space);
        }

		public bool WriteKeyword(string keyword, bool space)
		{
			if (!string.IsNullOrEmpty(keyword))
			{
				Write(keyword);
				if (space) Write(" ");
				return true;
			}
			return false; 
		}

        public bool WriteKeyword<T>(T value) where T:struct 
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
		private void Add<T>(Action<T> method)
            where T : class, ICodeNode
        {
            _writers.Add(method);
        }

		private void AddStatement<T>(Action<T> method)
            where T : class, IStatement
        {
            _statements.Add(method);
        }

		private void AddExpression<T>(Action<T> method)
            where T : class, IExpression
        {
            _expressions.Add(method);
        }

		private readonly ActionSelector _writers = new ActionSelector();
		private readonly ActionSelector _statements = new ActionSelector();
		private readonly ActionSelector _expressions = new ActionSelector();

        public void Init(string format)
        {
            if (format == null)
                format = "lang = c#";

            var dic = format.ParseKeyValuePairs("=", ",;");

            string value;
            if (dic.TryGetValue("lang", out value)
                || dic.TryGetValue("language", out value))
                Language = value;
            else
                Language = "c#";

            if (dic.TryGetValue("mode", out value))
            {
                if (string.Equals(value, "short", StringComparison.OrdinalIgnoreCase))
                    Mode = FormatMode.ShortDeclaration;
				else if (string.Equals(value, "decl", StringComparison.OrdinalIgnoreCase))
                    Mode = FormatMode.FullDeclaration;
                else
                    Mode = FormatMode.Full;
            }

            if (dic.TryGetValue("region", out value))
            {
                if (value == "+" || value == "1" || string.Equals(value, "true", StringComparison.OrdinalIgnoreCase))
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

		private void WriteKids(ICodeNode node)
        {
            var kids = node.ChildNodes;
            if (kids != null)
            {
                WriteNodes(kids, null);
            }
        }

		private void WriteVariable(IVariable v)
        {
            BeginStatement();
            WriteReferenceName(v.Type);
            Write(" ");
            Write(v.Name);
            EndStatement();
        }

		private void WriteVariableCollection(IVariableCollection collection)
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

		private string GetActiveNamespace()
        {
            if (_scopeStack.Count > 0)
            {
                var node = _scopeStack.Peek();
                var type = node as IType;
                if (type != null) return type.Namespace;
            }
            return null;
        }

		private string GetNamespace(IType type)
        {
            string ns = GetActiveNamespace();
            string tns = type.Namespace;
            if (string.IsNullOrEmpty(ns)) return tns;
            if (type.Namespace == ns) return "";
            return tns;
        }

		private static string GetFullName(IType type)
        {
            return type.DisplayName;
        }

        public string GetReferenceName(IType type)
        {
            var kind = type.TypeKind;
            switch (kind)
            {
	            case TypeKind.Array:
		            return GetReferenceName(type.ElementType) + type.ArrayDimensions;

	            case TypeKind.Pointer:
		            return GetReferenceName(type.ElementType) + "*";

	            case TypeKind.Reference:
		            return "ref " + GetReferenceName(type.ElementType);

	            default:
		            {
			            string k = type.GetKeyword(Language);
			            if (!string.IsNullOrEmpty(k))
				            return k;
			            return GetFullName(type);
		            }
            }
        }

		private void WriteReferenceName(IType type)
        {
            if (type == null)
            {
                Write("object");
                return;
            }
            Write(GetReferenceName(type));
        }

		private void WriteFullName(IType type)
        {
            Write(GetFullName(type));
        }

		private void WriteTypeName(IType type)
        {
            WriteID(GetName(type));
        }

		private void WriteID(string name)
        {
            if (Keywords.Contains(name))
                Write("@");
            Write(name);
        }
        #endregion

        #region Type Members
        #region IAssembly
		private readonly Hashtable _usings = new Hashtable();
		private const string System_Runtime_CompilerServices = "System.Runtime.CompilerServices";

		private void WriteUsings(IAssembly asm)
        {
            WriteUsingNamespace("System");
            WriteUsingNamespace(System_Runtime_CompilerServices);
            WriteLine();
        }

		private void WriteUsingNamespace(string ns)
        {
            if (!_usings.Contains(ns))
            {
                _usings[ns] = 1;
                WriteLine("using {0};", ns);
            }
        }

		private IAssembly _assembly;

		private void WriteAssembly(IAssembly asm)
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
		private void WriteModule(IModule mod)
        {
            WriteKids(mod);
        }
        #endregion

        #region IModuleCollection
		private void WriteModuleCollection(IModuleCollection collection)
        {
            WriteKids(collection);
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

		private static bool HasCustomAttributes(ICustomAttributeProvider p)
        {
            if (p == null) return false;
            if (HasImplAttribute(p as IMethod)) return true;
            if (p.CustomAttributes == null) return false;
            return p.CustomAttributes.Count > 0;
        }

		private bool IsFull
        {
            get { return Mode == FormatMode.Full || Mode == FormatMode.FullDeclaration; }
        }

		private void WriteAttributes(ICustomAttributeProvider p)
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

		private static bool HasBase(IType type)
        {
            if (type.TypeKind == TypeKind.Interface) return false;
            if (type.TypeKind == TypeKind.Struct) return false;
            var baseType = type.BaseType;
			return baseType != null && !baseType.Is(SystemTypeCode.Object);
        }

		private static IMethod IsDelegate(IType type)
        {
            if (type.TypeKind != TypeKind.Delegate) return null;
            var list = type.Methods.Find("Invoke");
            if (list == null) return null;
			return list.FirstOrDefault();
        }

		private void WriteType(IType type)
        {
            if (!ExportService.CanWrite(type))
                return;

            bool full = Mode == FormatMode.Full;

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

            bool fulldecl = full || Mode == FormatMode.FullDeclaration;

            //declaration
            WriteTab();
            WriteKeyword(type.Visibility);

			if (type.IsPartial)
			{
				WriteKeyword("partial", true);
			}

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
                    EndType(type);
                }
            }

            PopScope();

            if (writeNs)
                EndNamespace(type.Namespace);
        }

		private void WriteCustomCode(string code)
        {
            if (string.IsNullOrEmpty(code)) return;
            WriteLine();
            WriteLine();
            code = code.IndentLines(_tab);
            Write(code);
            WriteLine();
            WriteLine();
        }

		private void WriteBaseTypes(IType type)
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
		private void WriteTypeCollection(ITypeCollection collection)
        {
            foreach (var type in collection)
            {
                WriteType(type);
            }
        }
        #endregion

        #region IEnumerable<ICodeNode>
		private bool _doubleEOL;

		private void WriteNodes<T>(IEnumerable<T> nodes, string region) where T : ICodeNode
        {
            bool reg = !string.IsNullOrEmpty(region);
        	var list = nodes.ToList();
            if (list.Count == 0)
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
		private class MemberSet
        {
            public Visibility Visibility;
            public readonly List<ITypeMember> List = new List<ITypeMember>();
        }

		private void WriteMembers<T>(IEnumerable<T> collection, string region) where T : ITypeMember
        {
            if (_useRegions)
            {
            	var members = collection.ToList();
                if (members.Count == 0) return;

                var sets = new List<MemberSet>();
                foreach (var m in members)
                {
                    var v = m.Visibility;
					int i = sets.FindIndex(x => x.Visibility == v);
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
                    WriteNodes(set.List.Cast<ICodeNode>(), set.Visibility + " " + region);
                    eol = _length > len;
                }
            }
            else
            {
                //Type type = typeof(T);
                bool old = _doubleEOL;
                _doubleEOL = true;
                WriteNodes(collection, null);
                _doubleEOL = old;
            }
        }
        #endregion

        #region Value
		private void WriteNull()
        {
            WriteKeyword(Keyword.Null, false);
        }

		private void WriteString(string s)
        {
            Write(Escaper.Escape(s));
        }

		private static string ToHex(object v)
        {
            return "0x" + ((IFormattable)v).ToString("x", null);
        }

		private void WriteValue(object value)
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
		private void WriteEnumValue(IType type, object value)
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

		private void WriteArgument(IArgument arg)
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
		private void WriteArgumentCollection(IArgumentCollection collection)
        {
            WriteKids(collection);
        }
        #endregion

        #region ICustomAttribute
		private const string SuffixAttribute = "Attribute";

		private void WriteCustomAttribute(ICustomAttribute attr)
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
		private void WriteCustomAttributeCollection(ICustomAttributeCollection attrs)
        {
            bool old = _doubleEOL;
            _doubleEOL = false;
            WriteNodes(attrs, null);
            _doubleEOL = old;
        }
        #endregion

        #region IFieldCollection
		private void WriteFieldCollection(IFieldCollection list)
        {
            WriteMembers(list, "Fields");
        }
        #endregion

        #region IField
		private void WriteField(IField field)
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
		private void WriteMethodCollection(IMethodCollection list)
        {
            WriteMembers(list, "Methods");
        }
        #endregion

        #region IMethod
		private void WriteParameterCollection(IParameterCollection list)
        {
            bool sep = false;
            foreach (var p in list)
            {
                if (sep) WritePunctuator(Punctuator.ParameterSeparator);
                WriteParameter(p);
                sep = true;
            }
        }

		private void WriteParameter(IParameter p)
        {
            WriteCustomAttributeCollection(p.CustomAttributes);

			var hasParams = p.HasParams();
			if (hasParams)
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
            	Write(hasParams ? "object[] " : "object ");
            }
            Write(p.Name);
        }

		private void BeginBlock()
        {
            WriteLine();
            WriteTab();
            WritePunctuator(Punctuator.BeginBlock);
            WriteLine();
            Indent();
        }

		private void EndBlock()
        {
            WriteLine();
            Unindent();
            WriteTab();
            WritePunctuator(Punctuator.EndBlock);
        }

		private static bool HasBody(IMethod method)
        {
            if (method.IsAbstract) return false;
            if (method.Body == null) return false;
            return true;
        }

		private static bool IsExtern(IMethod method)
        {
            if (method == null) return false;
            if (method.IsInternalCall) return true;
            if (method.IsForwardRef) return true;
            if (method.PInvoke) return true;
            return false;
        }

		private static bool IsExtern(IPolymorphicMember m)
        {
            var method = m as IMethod;
            if (method != null)
                return IsExtern(method);
            var prop = m as IProperty;
            if (prop != null)
                return IsExtern(prop.Getter) || IsExtern(prop.Setter);
            return false;
        }

		private static bool HasImplAttribute(IMethod method)
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

		private void WriteFullDecl(string ns, string decl)
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

		private void WriteImplOpt(string name, bool sep)
        {
            if (sep) Write(" | ");
            WriteFullDecl(System_Runtime_CompilerServices, "MethodImplOptions." + name);
        }

		private static MethodImplOptions GetImplOptions(IMethod m)
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

		private void WriteImplOptions(MethodImplOptions v, ref bool sep)
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

		private void BeginMethodImplAttribute()
        {
            WriteTab();
            Write("[");
            WriteFullDecl(System_Runtime_CompilerServices, "MethodImpl(");
        }

		private void WriteMethodImplAttribute(MethodImplOptions opts)
        {
            BeginMethodImplAttribute();
            bool sep = false;
            WriteImplOptions(opts, ref sep);
            Write(")]");
        }

		private bool WriteMethodAttributes(IMethod method)
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

		private IMethod _curMethod;

		private static string GetCtorName(IType type)
        {
            return type.Name;
        }

		private static string GetAssociationName(IMethod method)
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

		private void WriteXmlComment(string text, string tag, params string[] attrs)
        {
            _writer.WriteXmlComments(text, _tab, tag, attrs);
        }

		private void WriteSummary(string text)
        {
            if (string.IsNullOrEmpty(text)) return;
            WriteXmlComment(text, "summary");
        }

		private void WriteMethodDocumentation(IMethod method)
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

		//TODO: use existing method extension
        private static bool IsNew(IMethod method)
        {
            if (method == null || method.IsStatic || method.IsConstructor)
				return false;

            var type = method.DeclaringType;
            var baseType = type.BaseType;
            while (baseType != null)
            {
                foreach (var baseMethod in baseType.Methods)
                {
                    if (Signature.Equals(baseMethod, method, false))
                    {
                        return !ReferenceEquals(baseMethod.Type, method.Type);
                    }
                }
                baseType = baseType.BaseType;
            }

            return false;
        }

        private static bool IsNew(IPolymorphicMember m)
        {
            var method = m as IMethod;
            if (method != null)
                return IsNew(method);
            var prop = m as IProperty;
            if (prop != null)
                return IsNew(prop.Getter);
            return false;
        }

		private void WriteKeywords(IPolymorphicMember m)
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

		private static bool IsPrivate(Visibility v)
        {
            switch(v)
            {
                case Visibility.Private:
                case Visibility.PrivateScope:
                    return true;
            }
            return false;
        }

		private void WriteMethod(IMethod method)
        {
            var assoc = method.Association;
            if (assoc != null && _skipAssociatedMethods && Scope != null)
                return;

            bool full = Mode == FormatMode.Full;

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

		private void WriteGenericParams(IEnumerable<IType> parameters)
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

		private void WriteGenericArgs(IEnumerable<IType> args)
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

		private static IExpression ToCtorCall(IStatement st)
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

		private void WriteCtorBody(IList<IStatement> body)
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
            bool full = Mode == FormatMode.Full;

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

            bool full = Mode == FormatMode.Full;

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

            if (e.IsFlashEvent() && !isIface)
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

		private void WriteStatement(IStatement st)
        {
            _cst = st;
            //_statementStack.Push(st);
            if (!_statements.Run(st))
            {
                throw new NotImplementedException();
            }
            //_statementStack.Pop();
        }

		private void WriteStatementCollection(IStatementCollection collection)
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

		private bool AsOneStatement(IStatement st)
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

		private void WriteBlock(IList<IStatement> body, int startIndex, int bf)
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

		private void WriteBlock(string name, IList<IStatement> body, int bf)
        {
            WriteTab();
            Write(name);
            WriteBlock(body, 0, bf);
        }

		private void BeginStatement()
        {
            WriteTab();
        }

		private void EndStatement()
        {
            Write(";");
        }

		private void WriteKeywordStatement(Keyword k)
        {
            BeginStatement();
            WriteKeyword(k, false);
            EndStatement();
        }

		private void WriteContinueStatement(IContinueStatement s)
        {
            WriteKeywordStatement(Keyword.Continue);
        }

		private void WriteBreakStatement(IBreakStatement s)
        {
            WriteKeywordStatement(Keyword.Break);
        }

		private void WriteExpressionStatement(IExpressionStatement s)
        {
            BeginStatement();
            WriteExpression(s.Expression);
            EndStatement();
        }

		private void WriteReturnStatement(IReturnStatement s)
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

		private void WriteVariableDeclarationStatement(IVariableDeclarationStatement s)
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

		private void WriteLabeledStatement(ILabeledStatement s)
        {
            if (s.Statement == null)
                throw new InvalidOperationException();

            BeginStatement();
            Write(s.Name);
            Write(":");
            WriteLine();

            WriteStatement(s.Statement);
        }

		private void WriteGotoStatement(IGotoStatement s)
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

		private void WriteCondition(IExpression e)
        {
            if (e != null)
                WriteExpression(e);
            else
                Write("(true)");
        }

		private bool _elseif;

		private void WriteIfStatement(IIfStatement s)
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

		private void WriteLoopStatement(ILoopStatement s)
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

		private void WriteSwitchStatement(ISwitchStatement s)
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

		private void WriteSwitchCase(ISwitchCase sc)
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

		private void WriteTryCatchStatement(ITryCatchStatement s)
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

		private void WriteCatchClause(ICatchClause c)
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

		private void WriteThrowExceptionStatement(IThrowExceptionStatement s)
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

		private void WriteMemoryInitializeStatement(IMemoryInitializeStatement s)
        {
            BeginStatement();
            EndStatement();
        }

		private void WriteMemoryCopyStatement(IMemoryCopyStatement s)
        {
            BeginStatement();
            EndStatement();
        }
        #endregion

        #region IExpression
		private void WriteExpression(IExpression e)
        {
            if (!_expressions.Run(e))
                throw new NotImplementedException();
        }

		private void WriteConstExpression(IConstantExpression e)
        {
            WriteValue(e.Value);
        }

		private void WriteThisReferenceExpression(IThisReferenceExpression e)
        {
            Write(GetKeyword(Keyword.This));
        }

		private void WriteBaseReferenceExpression(IBaseReferenceExpression e)
        {
            Write(GetKeyword(Keyword.Base));
        }

		private void WriteTypeReferenceExpression(ITypeReferenceExpression e)
        {
            WriteReferenceName(e.Type);
        }

		private void WriteVariableReferenceExpression(IVariableReferenceExpression e)
        {
            Write(e.Variable.Name);
        }

		private void WriteArgumentReferenceExpression(IArgumentReferenceExpression e)
        {
            Write(e.Argument.Name);
        }

		private void WriteMethodReferenceExpression(IMethodReferenceExpression e)
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

        private bool CanSkipTarget(IMemberReferenceExpression e)
        {
            if (_curMethod == null) return false;
            var tr = e.Target as ITypeReferenceExpression;
            return tr != null && ReferenceEquals(tr.Type, _curMethod.DeclaringType);
        }

        private void WritePropertyReferenceExpression(IPropertyReferenceExpression e)
        {
            WriteMemberReferenceExpression(e);
        }

		private void WriteFieldReferenceExpression(IFieldReferenceExpression e)
        {
            WriteMemberReferenceExpression(e);
        }

		private void WriteMemberReferenceExpression(IMemberReferenceExpression e)
        {
            if (!CanSkipTarget(e))
            {
                WriteExpression(e.Target);
                Write(".");
            }
            Write(e.Member.Name);
        }

		private void WriteExpressions(IEnumerable<IExpression> list, string separator)
        {
            bool sep = false;
            foreach (var e in list)
            {
                if (sep) Write(separator);
                WriteExpression(e);
                sep = true;
            }
        }

		private void WriteArguments(IReadOnlyList<IParameter> parameters, IList<IExpression> args, string prefix, string suffix)
        {
            Write(prefix);
            for (int i = 0; i < args.Count; ++i)
            {
                if (i > 0) Write(", ");
                var arg = args[i];
                var p = parameters[i];
                if (p.IsByRef())
                    Write("ref ");
                WriteExpression(arg);
            }
            Write(suffix);
        }

		private void WriteMethodArguments(IMethod method, IList<IExpression> args)
        {
            const string prefix = "(";
            const string suffx = ")";
            if (method == null)
            {
                Write(prefix);
                Write(suffx);
                return;
            }
            WriteArguments(method.Parameters, args, prefix, suffx);
        }

		private void WriteCallExpression(ICallExpression e)
        {
            WriteExpression(e.Method);
            WriteMethodArguments(e.Method.Method, e.Arguments);
        }

		private void WriteFencedExpression(IExpression e)
        {
            bool cb = ExpressionService.CanBrace(e);
            if (cb) Write("(");
            WriteExpression(e);
            if (cb) Write(")");
        }

		private void WriteCastExpression(ICastExpression e)
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

		private void WriteBinaryExpression(IBinaryExpression e)
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

		private void WriteUnaryExpression(IUnaryExpression e)
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

		private void WriteBoxExpression(IBoxExpression e)
        {
            WriteExpression(e.Expression);
        }

		private void WriteUnboxExpression(IUnboxExpression e)
        {
            Write("(");
            Write("(");
            WriteReferenceName(e.TargetType);
            Write(")");
            WriteExpression(e.Expression);
            Write(")");
        }

		private void WriteConditionExpression(IConditionExpression e)
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

		private void WriteExpressionCollection(IExpressionCollection e)
        {
            WriteExpressions(e, ", ");
        }

		private void WriteNewObjectExpression(INewObjectExpression e)
        {
            Write("new ");
            WriteReferenceName(e.ObjectType);
            WriteMethodArguments(e.Constructor, e.Arguments);
        }

		private void WriteNewArrayExpression(INewArrayExpression e)
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

		private void WriteIndexerExpression(IIndexerExpression e)
        {
            WriteExpression(e.Property.Target);
            Write("[");
            WriteExpressionCollection(e.Index);
            Write("]");
        }

		private void WriteArrayIndexerExpression(IArrayIndexerExpression e)
        {
            WriteExpression(e.Array);
            Write("[");
            WriteExpressionCollection(e.Index);
            Write("]");
        }

		private void WriteArrayLengthExpression(IArrayLengthExpression e)
        {
            WriteExpression(e.Array);
            Write(".Length");
        }

		private void WriteAddressOfExpression(IAddressOfExpression e)
        {
            Write("&");
            bool b = ExpressionService.CanBrace(e.Expression);
            if (b) Write("(");
            WriteExpression(e.Expression);
            if (b) Write(")");
        }

		private void WriteAddressDereferenceExpression(IAddressDereferenceExpression e)
        {
            Write("*");
            bool b = ExpressionService.CanBrace(e.Expression);
            if (b) Write("(");
            WriteExpression(e.Expression);
            if (b) Write(")");
        }

		private void WriteAddressOutExpression(IAddressOutExpression e)
        {
            Write("out ");
            bool b = ExpressionService.CanBrace(e.Expression);
            if (b) Write("(");
            WriteExpression(e.Expression);
            if (b) Write(")");
        }

		private void WriteAddressRefExpression(IAddressRefExpression e)
        {
            Write("ref ");
            bool b = ExpressionService.CanBrace(e.Expression);
            if (b) Write("(");
            WriteExpression(e.Expression);
            if (b) Write(")");
        }

		private void WriteNewDelegateExpression(INewDelegateExpression e)
        {
            Write("new ");
            WriteFullName(e.DelegateType);
            Write("(");
            WriteExpression(e.Method);
            Write(")");
        }

		private void WriteDelegateInvokeExpression(IDelegateInvokeExpression e)
        {
            Write(e.Target);
            WriteMethodArguments(e.Method, e.Arguments);
        }

		private void WriteTypeOfExpression(ITypeOfExpression e)
        {
            Write("typeof(");
            WriteReferenceName(e.Type);
            Write(")");
        }

		private void WriteSizeOfExpression(ISizeOfExpression e)
        {
            Write("sizeof(");
            WriteReferenceName(e.Type);
            Write(")");
        }
        #endregion

	    public override string ToString()
        {
            return _writer.ToString();
        }
    }
}