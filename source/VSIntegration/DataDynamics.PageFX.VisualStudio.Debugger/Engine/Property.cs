using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.Debugger.Interop;

namespace DataDynamics.PageFX.VisualStudio.Debugger
{
    #region enum PropFlags
    [Flags]
    enum PropFlags
    {
        None = 0x0000,
        This = 0x0001,
        Object = 0x0002,
        Boolean = 0x0004,
        String = 0x0008,
        Number = 0x0010,
        NativeArray = 0x0020,
        SystemArray = 0x0040,

        Variable = 0x0100,
        Argument = 0x0200,
        Getter = 0x0400,
        Setter = 0x0800,

        Public = 0x1000,
        Private = 0x2000,
        Protected = 0x4000,
        Static = 0x8000,

        ReadOnly = 0x10000,
        Const = 0x20000,
        Dynamic = 0x40000,

        Multiline = 0x100000,
        Evaluated = 0x200000,
        NotExpandable = 0x400000,
        Special = 0x800000,
    }
    #endregion

    // An implementation of IDebugProperty2
    // This interface represents a stack frame property, a program document property, or some other property. 
    // The property is usually the result of an expression evaluation. 
    class Property : IDebugProperty2, ICloneable
    {
        public string Name = "";
        public string Value = "";
        public string Type = "";
        public Property Parent;

        #region ctors
        public Property(string name)
        {
            Name = name;
        }

        public Property(string name, string value)
            : this(name)
        {
            Value = value;
        }

        public Property(string name, string value, string type) 
            : this(name, value)
        {
            Type = type;
        }
        #endregion

        #region Flags
        PropFlags _flags;

        bool IsFlag(PropFlags f)
        {
            return (_flags & f) != 0;
        }

        void SetFlag(PropFlags f, bool value)
        {
            if (value) _flags |= f;
            else _flags &= ~f;
        }

        public bool IsComplex
        {
            get { return IsObject || IsNativeArray || IsSystemArray; }
        }

        public bool IsNotExpandable
        {
            get { return IsFlag(PropFlags.NotExpandable); }
            set { SetFlag(PropFlags.NotExpandable, value); }
        }

        public bool IsSpecial
        {
            get { return IsFlag(PropFlags.Special); }
            set { SetFlag(PropFlags.Special, value); }
        }

        public bool IsObject
        {
            get { return IsFlag(PropFlags.Object); }
            set { SetFlag(PropFlags.Object, value); }
        }

        public bool IsThis
        {
            get { return IsFlag(PropFlags.This); }
            set { SetFlag(PropFlags.This, value); }
        }

        public bool IsBoolean
        {
            get { return IsFlag(PropFlags.Boolean); }
            set { SetFlag(PropFlags.Boolean, value); }
        }

        public bool IsString
        {
            get { return IsFlag(PropFlags.String); }
            set { SetFlag(PropFlags.String, value); }
        }

        public bool IsNumber
        {
            get { return IsFlag(PropFlags.Number); }
            set { SetFlag(PropFlags.Number, value); }
        }

        public bool IsNativeArray
        {
            get { return IsFlag(PropFlags.NativeArray); }
            set { SetFlag(PropFlags.NativeArray, value); }
        }

        public bool IsSystemArray
        {
            get { return IsFlag(PropFlags.SystemArray); }
            set { SetFlag(PropFlags.SystemArray, value); }
        }

        public bool IsMultiLine
        {
            get { return IsFlag(PropFlags.Multiline); }
            set { SetFlag(PropFlags.Multiline, value); }
        }

        public bool IsVariable
        {
            get { return IsFlag(PropFlags.Variable); }
            set { SetFlag(PropFlags.Variable, value); }
        }

        public bool IsArgument
        {
            get { return IsFlag(PropFlags.Argument); }
            set { SetFlag(PropFlags.Argument, value); }
        }

        public bool IsGetter
        {
            get { return IsFlag(PropFlags.Getter); }
            set { SetFlag(PropFlags.Getter, value); }
        }

        public bool IsSetter
        {
            get { return IsFlag(PropFlags.Setter); }
            set { SetFlag(PropFlags.Setter, value); }
        }

        public bool IsProperty
        {
            get { return IsGetter || IsSetter; }
        }

        public bool IsPublic
        {
            get { return IsFlag(PropFlags.Public); }
            set { SetFlag(PropFlags.Public, value); }
        }

        public bool IsPrivate
        {
            get { return IsFlag(PropFlags.Private); }
            set { SetFlag(PropFlags.Private, value); }
        }

        public bool IsProtected
        {
            get { return IsFlag(PropFlags.Protected); }
            set { SetFlag(PropFlags.Protected, value); }
        }

        public bool IsStatic
        {
            get { return IsFlag(PropFlags.Static); }
            set { SetFlag(PropFlags.Static, value); }
        }

        public bool IsReadOnly
        {
            get { return IsFlag(PropFlags.ReadOnly); }
            set { SetFlag(PropFlags.ReadOnly, value); }
        }

        public bool IsConst
        {
            get { return IsFlag(PropFlags.Const); }
            set { SetFlag(PropFlags.Const, value); }
        }

        public bool IsEvaluated
        {
            get { return IsFlag(PropFlags.Evaluated); }
            set { SetFlag(PropFlags.Evaluated, value); }
        }

        public bool IsIgnored
        {
            get
            {
                if (IsEvaluated) return false;
                return IsStatic || IsConst;
            }
        }
        #endregion

        #region IsTrue
        static NumberFormatInfo FloatFormat
        {
            get
            {
                if (_floatFormat == null)
                    _floatFormat = new NumberFormatInfo { NumberDecimalSeparator = "." };
                return _floatFormat;
            }
        }
        static NumberFormatInfo _floatFormat;

        public bool IsTrue
        {
            get
            {
                if (IsBoolean)
                    return string.Compare(Value, "true", true) == 0;

                if (IsNumber)
                {
                    double num;
                    if (double.TryParse(Value, NumberStyles.Float, FloatFormat, out num))
                        return num != 0;
                    return true;
                }

                return string.Compare(Value, "null", true) != 0;
            }
        }
        #endregion

        #region Frame
        public StackFrame Frame
        {
            get { return _frame; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                if (_frame != value)
                {
                    _frame = value;
                    _frame.Register(this);
                }
            }
        }
        StackFrame _frame;
        #endregion

        #region FullName
        public string FullName
        {
            get
            {
                if (m_fullName != null)
                    return m_fullName;
                if (Parent == null)
                    return Name;
                m_fullName = CalcFullName();
                return m_fullName;
            }
            set
            {
                m_fullName = value;
            }
        }

        string CalcFullName()
        {
            if (Parent != null)
                return Util.QName(Parent.FullName, Name);
            return Name;
        }

        string m_fullName;
        #endregion

        public Engine Engine
        {
            get { return Frame.Engine; }
        }

        #region Child Properties
        public Property this[string name]
        {
            get
            {
                return Util.Find(_kids, p => p.Name == name);
            }
        }

        public List<Property> Kids
        {
            get { return _kids; }
        }
        readonly List<Property> _kids = new List<Property>();

        public void AddChild(Property property)
        {
            if (property.IsIgnored) return;
            property.Parent = this;
            _kids.Add(property);
            property.Frame = Frame;
        }

        public void AddChildren(IEnumerable<Property> set)
        {
            if (set == null) return;
            foreach (var prop in set)
                AddChild(prop);
        }

        public void AddFrom1(IList<Property> list)
        {
            if (list == null) return;
            int n = list.Count;
            for (int i = 1; i < n; ++i)
                AddChild(list[i]);
        }
        #endregion

        #region Register, Unregister
        public void Register()
        {
            var frame = Frame;
            if (frame == null) return;
            frame.Register(this);
            foreach (var kid in _kids)
                kid.Register();
        }

        public void Unregister()
        {
            var frame = Frame;
            if (frame == null) return;
            frame.Unregister(this);
            foreach (var kid in _kids)
                kid.Unregister();
        }
        #endregion

        #region CreateDebugPropertyInfo
        // Construct a DEBUG_PROPERTY_INFO representing this local or parameter.
        public DEBUG_PROPERTY_INFO CreateDebugPropertyInfo(uint dwFields)
        {
            var pi = new DEBUG_PROPERTY_INFO();

            var flags = (enum_DEBUGPROP_INFO_FLAGS)dwFields;

            if ((flags & enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_FULLNAME) != 0)
            {
                pi.bstrFullName = FullName;
                pi.dwFields |= (uint)(DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_FULLNAME);
            }

            if ((flags & enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_NAME) != 0)
            {
                pi.bstrName = Name;
                pi.dwFields |= (uint)(DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_NAME);
            }

            if ((flags & enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_TYPE) != 0)
            {
                pi.bstrType = Type;
                pi.dwFields |= (uint)(DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_TYPE);
            }

            if ((flags & enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_VALUE) != 0)
            {
                pi.bstrValue = Value;
                pi.dwFields |= (uint)(DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_VALUE);
            }

            if ((flags & enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_ATTRIB) != 0)
            {
                // The sample does not support writing of values displayed in the debugger, so mark them all as read-only.
                pi.dwAttrib = DBG_ATTRIB_FLAGS.DBG_ATTRIB_VALUE_READONLY;
                
                if (!IsNotExpandable && (_kids.Count > 0 || NeedEval))
                {
                    pi.dwAttrib |= DBG_ATTRIB_FLAGS.DBG_ATTRIB_OBJ_IS_EXPANDABLE;
                }

                if (IsBoolean)
                    pi.dwAttrib |= DBG_ATTRIB_FLAGS.DBG_ATTRIB_VALUE_BOOLEAN;

                if (IsPublic)
                    pi.dwAttrib |= DBG_ATTRIB_FLAGS.DBG_ATTRIB_ACCESS_PUBLIC;
                else if (IsProtected)
                    pi.dwAttrib |= DBG_ATTRIB_FLAGS.DBG_ATTRIB_ACCESS_PROTECTED;
                else if (IsPrivate)
                    pi.dwAttrib |= DBG_ATTRIB_FLAGS.DBG_ATTRIB_ACCESS_PRIVATE;

                if (IsStatic)
                    pi.dwAttrib |= DBG_ATTRIB_FLAGS.DBG_ATTRIB_STORAGE_STATIC;
                if (IsConst)
                    pi.dwAttrib |= DBG_ATTRIB_FLAGS.DBG_ATTRIB_TYPE_CONSTANT;
                if (IsProperty)
                    pi.dwAttrib |= DBG_ATTRIB_FLAGS.DBG_ATTRIB_PROPERTY;
            }

            // If the debugger has asked for the property, or the property has children (meaning it is a pointer in the sample)
            // then set the pProperty field so the debugger can call back when the chilren are enumerated.
            if (((flags & enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_PROP) != 0))
            {
                pi.pProperty = this;
                pi.dwFields |= (uint)(DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_PROP);
            }

            return pi;
        }
        #endregion

        #region IDebugProperty2 Members
        bool evalChildren = true;

        public void ReevalChildren()
        {
            _kids.Clear();
            evalChildren = true;
        }

        public bool NeedEval
        {
            get
            {
                return evalChildren
                       && _kids.Count == 0
                       && IsObject
                       && Engine != null;
            }
        }

        public void Eval()
        {
            if (NeedEval)
            {
                evalChildren = false;
                Engine.EvalChildProperties(this);
            }
        }

        // Enumerates the children of a property. This provides support for dereferencing pointers, displaying members of an array, or fields of a class or struct.
        // The sample debugger only supports pointer dereferencing as children. This means there is only ever one child.
        public int EnumChildren(uint dwFields, uint dwRadix, ref Guid guidFilter,
            ulong dwAttribFilter, string pszNameFilter, uint dwTimeout,
            out IEnumDebugPropertyInfo2 ppEnum)
        {
            ppEnum = null;

            try
            {
                Eval();

                int n = _kids.Count;
                if (n > 0)
                {
                    var arr = new DEBUG_PROPERTY_INFO[n];
                    for (int i = 0; i < n; ++i)
                        arr[i] = _kids[i].CreateDebugPropertyInfo(dwFields);
                    ppEnum = new PropertyEnum(arr);
                }
                return Const.S_OK;
            }
            catch (Exception exc)
            {
            }

            return Const.S_FALSE;
        }

        // Returns the property that describes the most-derived property of a property
        // This is called to support object oriented languages. It allows the debug engine to return an IDebugProperty2 for the most-derived 
        // object in a hierarchy. This engine does not support this.
        public int GetDerivedMostProperty(out IDebugProperty2 ppDerivedMost)
        {
            throw new NotImplementedException();
        }

        // This method exists for the purpose of retrieving information that does not lend itself to being retrieved by calling the IDebugProperty2::GetPropertyInfo 
        // method. This includes information about custom viewers, managed type slots and other information.
        // The sample engine does not support this.
        public int GetExtendedInfo(ref Guid guidExtendedInfo, out object pExtendedInfo)
        {
            throw new NotImplementedException();
        }

        // Returns the memory bytes for a property value.
        public int GetMemoryBytes(out IDebugMemoryBytes2 ppMemoryBytes)
        {
            throw new NotImplementedException();
        }

        // Returns the memory context for a property value.
        public int GetMemoryContext(out IDebugMemoryContext2 ppMemory)
        {
            throw new NotImplementedException();
        }

        // Returns the parent of a property.
        // The sample engine does not support obtaining the parent of properties.
        public int GetParent(out IDebugProperty2 ppParent)
        {
            ppParent = Parent;
            return Const.S_OK;
        }

        // Fills in a DEBUG_PROPERTY_INFO structure that describes a property.
        public int GetPropertyInfo(uint dwFields, uint dwRadix, uint dwTimeout, IDebugReference2[] rgpArgs, uint dwArgCount, DEBUG_PROPERTY_INFO[] pPropertyInfo)
        {
            rgpArgs = null;
            pPropertyInfo[0] = CreateDebugPropertyInfo(dwFields);
            return Const.S_OK;
        }

        //  Return an IDebugReference2 for this property. An IDebugReference2 can be thought of as a type and an address.
        public int GetReference(out IDebugReference2 ppReference)
        {
            throw new NotImplementedException();
        }

        // Returns the size, in bytes, of the property value.
        public int GetSize(out uint pdwSize)
        {
            throw new NotImplementedException();
        }

        // The debugger will call this when the user tries to edit the property's values
        // the sample has set the read-only flag on its properties, so this should not be called.
        public int SetValueAsReference(IDebugReference2[] rgpArgs, uint dwArgCount, IDebugReference2 pValue, uint dwTimeout)
        {
            throw new NotImplementedException();
        }

        // The debugger will call this when the user tries to edit the property's values in one of the debugger windows.
        // the sample has set the read-only flag on its properties, so this should not be called.
        public int SetValueAsString(string pszValue, uint dwRadix, uint dwTimeout)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region ICloneable Impl
        public Property Clone()
        {
            var prop = new Property(Name, Value, Type)
                           {
                               _flags = _flags,
                               Frame = Frame,
                           };
            foreach (var cp in _kids)
                AddChild(cp.Clone());
            return prop;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }
        #endregion

        #region Parsing
        const string fdb_prefix = "(fdb) ";
        public static Property Parse(string s)
        {
            if (string.IsNullOrEmpty(s))
                return null;

            if (s.StartsWith(fdb_prefix))
                s = s.Substring(fdb_prefix.Length);

            var attrs = GrabAttrs(ref s);

            int eq = s.IndexOf('=');
            if (eq < 0) return null;

            var name = s.Substring(0, eq).Trim();
            string value;
            if (name[0] == '$')
            {
                int eq2 = s.IndexOf('=', eq + 1);
                if (eq2 < 0)
                {
                    value = s.Substring(eq + 1).Trim();
                }
                else
                {
                    name = s.Substring(eq + 1, eq2 - eq - 1).Trim();
                    value = s.Substring(eq2 + 1).Trim();
                }
            }
            else
            {
                value = s.Substring(eq + 1).Trim();
            }

            var prop = new Property(name) { _flags = attrs };

            if (name.StartsWith(DebugSpecProperties.Prefix))
                prop.IsSpecial = true;

            if (string.IsNullOrEmpty(value))
            {
                prop.Type = "Unknown";
                return prop;
            }

            ParseValue(prop, value);

            return prop;
        }

        static void ParseValue(Property prop, string value)
        {
            Match m;

            if (value[0] == '[')
            {
                if ((m = CRE.ValueObject.Match(value)).Success)
                {
                    prop.Value = m.Groups["addr"].Value;
                    prop.Type = m.Groups["type"].Value;
                    prop.Type = prop.Type.Replace("::", ".");
                    prop.IsObject = true;
                    switch (prop.Type)
                    {
                        case "Array":
                            prop.IsNativeArray = true;
                            break;

                        case "System.Array":
                            prop.IsSystemArray = true;
                            break;
                    }
                    return;
                }

                if ((m = CRE.ValueFunction.Match(value)).Success)
                {
                    prop.Value = m.Groups["addr"].Value;
                    prop.Type = "Function";
                    prop.IsObject = true;
                    return;
                }

                if ((m = CRE.ValueGetter.Match(value)).Success)
                {
                    prop.Value = m.Groups["addr"].Value;
                    prop.Type = "Function";
                    prop.IsObject = true;
                    return;
                }

                if ((m = CRE.ValueSetter.Match(value)).Success)
                {
                    prop.Value = m.Groups["addr"].Value;
                    prop.Type = "Function";
                    prop.IsObject = true;
                }
                else
                {
                    prop.Value = value;
                    prop.Type = "Unknown";
                }
                return;
            }

            if (value[0] == '\"')
            {
                prop.Value = value.Trim('\"');
                prop.Type = "String";
                prop.IsString = true;
                if (value[value.Length - 1] != '\"')
                {
                    prop.IsMultiLine = true;
                }
                return;
            }

            if ((m = CRE.ValueNumber.Match(value)).Success)
            {
                prop.Value = m.Groups["dec"].Value.Trim();
                prop.Type = "Number";
                prop.IsNumber = true;
                return;
            }

            if (string.Compare(value, "true", true) == 0
                     || string.Compare(value, "false", true) == 0)
            {
                prop.Value = value;
                prop.Type = "Boolean";
                prop.IsBoolean = true;
                return;
            }

            if (string.Compare(value, "null", true) == 0)
            {
                prop.Value = value;
            }
            else
            {
                prop.Value = value;
                prop.Type = "Unknown";
            }
        }
        #endregion

        #region GrabAttrs
        static PropFlags GrabAttrs(ref string value)
        {
            PropFlags f = 0;
            int n = value.Length;
            int i = n - 1;
            while (true)
            {
                if (f != 0)
                {
                    SkipWSR(value, ref i);
                    if (i < 0) break;
                    if (value[i] == ',')
                    {
                        --i;
                        SkipWSR(value, ref i);
                        if (i < 0) break;
                    }
                }

                string w = GrabReverseID(value, i);
                if (string.IsNullOrEmpty(w)) break;

                var attr = Util.Find(Attrs, a => a.Name == w);
                if (attr == null) break;

                f |= attr.Flag;
                i -= w.Length;
            }

            if (i != n - 1)
                value = value.Substring(0, i + 1);
            return f;
        }

        static bool IsWS(char c)
        {
            return c == ' ' || c == '\t' || c == '\n' || c == '\r';
        }

        //Skips whitespace chars in reverse order
        static void SkipWSR(string s, ref int i)
        {
            while (i >= 0)
            {
                if (!IsWS(s[i]))
                    break;
                --i;
            }
        }

        static string GrabReverseID(string value, int i)
        {
            string w = "";
            while (i >= 0)
            {
                char c = value[i];
                if (w.Length == 0)
                {
                    if (!char.IsLetter(c))
                        break;
                }
                else
                {
                    if (!(char.IsLetter(c) || c == '-' || c == '_'))
                        break;
                }
                w = c + w;
                --i;
            }
            return w;
        }

        class Attr
        {
            public readonly string Name;
            public readonly PropFlags Flag;

            public Attr(string name, PropFlags flag)
            {
                Name = name;
                Flag = flag;
            }

            public override string ToString()
            {
                return Name;
            }
        }

        static readonly Attr[] Attrs = new[]
                                           {
                                               new Attr("local", PropFlags.Variable), 
                                               new Attr("argument", PropFlags.Argument), 
                                               new Attr("getter", PropFlags.Getter), 
                                               new Attr("setter", PropFlags.Setter), 
                                               new Attr("public", PropFlags.Public), 
                                               new Attr("private", PropFlags.Private), 
                                               new Attr("protected", PropFlags.Protected), 
                                               new Attr("static", PropFlags.Static), 
                                               new Attr("const", PropFlags.Const), 
                                               new Attr("read-only", PropFlags.ReadOnly), 
                                               new Attr("dynamic", PropFlags.Dynamic), 
                                           };
        #endregion

        public override string ToString()
        {
            return string.Format("{0} = {1} ({2})", FullName, Value, Type);
        }
    }
}
