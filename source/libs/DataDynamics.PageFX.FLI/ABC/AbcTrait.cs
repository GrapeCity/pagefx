using System;
using System.Diagnostics;
using System.IO;
using System.Xml;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.CodeModel.Syntax;
using DataDynamics.PageFX.FLI.SWF;

namespace DataDynamics.PageFX.FLI.ABC
{
    #region Enums
    /*
     enum TraitKind {
			TRAIT_Slot			= 0x00,
			TRAIT_Method		= 0x01,
			TRAIT_Getter		= 0x02,
			TRAIT_Setter		= 0x03,
			TRAIT_Class			= 0x04,
			TRAIT_Const			= 0x06,
			TRAIT_mask			= 15
		};
     */
    public enum AbcTraitKind : byte
    {
        Slot = 0x00,
        Method = 0x01,
        Getter = 0x02,
        Setter = 0x03,
        Class = 0x04,
        Function = 0x05,
        Const = 0x6,
    }

    [Flags]
    public enum AbcTraitAttributes
    {
        None,

        /// <summary>
        /// Is used with Method, Getter and Setter. It marks a
        /// method that cannot be overridden by a sub-class
        /// </summary>
        Final = 0x1, //1=final, 0=virtual

        /// <summary>
        /// Is used with Method, Getter and Setter. It marks a
        /// method that has been overridden in this class
        /// </summary>
        Override = 0x2, //1=override, 0=new

        /// <summary>
        /// Is used to signal that the fields metadata_count and metadata follow the
        /// data field in the traits_info entry
        /// </summary>
        HasMetadata = 0x4,
    }
    #endregion

    #region class AbcTrait
    /// <summary>
    /// Represents ABC trait (type member)
    /// </summary>
    public class AbcTrait : ISwfIndexedAtom, ISupportXmlDump
    {
        #region InnerTypes
        interface ITraitData : ISwfAtom, ISupportXmlDump
        {
            int SlotID { get; set; }
        }

        interface IMethodLink
        {
            AbcMethod Method { get; set; }
        }

        #region class SlotData
        class SlotData : ITraitData
        {
            #region Properties
            /// <summary>
            /// The slot_id field is an integer from 0 to N and is used to identify a position in which this trait resides. A
            /// value of 0 requests the AVM2 to assign a position.
            /// </summary>
            public int SlotID
            {
                get { return _slotID; }
                set { _slotID = value; }
            }
            int _slotID;

            public AbcMultiname Type
            {
                get { return _type; }
                set { _type = value; }
            }
            AbcMultiname _type;

            public bool HasValue
            {
                get { return _hasValue; }
                set { _hasValue = value; }
            }
            bool _hasValue;

            public object Value
            {
                get { return _value; }
                set
                {
                    _value = value;
                    _hasValue = true;
                }
            }
            object _value;
            #endregion

            #region IAbcAtom Members
            public void Read(SwfReader reader)
            {
                _slotID = (int)reader.ReadUIntEncoded(); //slod_id
                _type = reader.ReadMultiname();

                _hasValue = false;
                int index = (int)reader.ReadUIntEncoded(); //vindex
                if (index != 0)
                {
                    _hasValue = true;
                    var kind = (AbcConstKind)reader.ReadUInt8(); //vkind
                    _value = reader.ABC.GetConstant(kind, index);
                }
            }

            public void Write(SwfWriter writer)
            {
                writer.WriteUIntEncoded((uint)_slotID);
                writer.WriteUIntEncoded((uint)_type.Index);
                if (_hasValue)
                {
                    writer.WriteConstIndex(_value);
                }
                else
                {
                    writer.WriteByte(0);
                }
            }
            #endregion

            #region ISupportXmlDump Members
            public void DumpXml(XmlWriter writer)
            {
                writer.WriteElementString("slot-id", _slotID.ToString());
                writer.WriteElementString("type", _type.ToString());
                if (_hasValue)
                    writer.WriteElementString("value", _value != null ? _value.ToString() : "null");
            }
            #endregion
        }
        #endregion

        #region class ClassData
        class ClassData : ITraitData
        {
            #region Properties
            /// <summary>
            /// The slot_id field is an integer from 0 to N and is used to identify a position in which this trait resides. A
            /// value of 0 requests the AVM2 to assign a position.
            /// </summary>
            public int SlotID { get; set; }

            public AbcClass Class { get; set; }
            #endregion

            #region IAbcAtom Members
            public void Read(SwfReader reader)
            {
                SlotID = (int)reader.ReadUIntEncoded(); //slot_id
                int index = (int)reader.ReadUIntEncoded(); //classi
                Class = reader.ABC.Classes[index];
            }

            public void Write(SwfWriter writer)
            {
                writer.WriteUIntEncoded((uint)SlotID);
                writer.WriteUIntEncoded((uint)Class.Index);
            }
            #endregion

            #region ISupportXmlDump Members
            public void DumpXml(XmlWriter writer)
            {
                writer.WriteElementString("slot-id", SlotID.ToString());
                if (Class != null)
                    writer.WriteElementString("class", Class.ToString());
            }
            #endregion
        }
        #endregion

        #region class FunctionData
        class FunctionData : ITraitData, IMethodLink
        {
            #region Properties
            public int SlotID
            {
                get { return _slotID; }
                set { _slotID = value; }
            }
            int _slotID;

            public AbcMethod Method
            {
                get { return _method; }
                set { _method = value; }
            }
            AbcMethod _method;
            #endregion

            #region IAbcAtom Members
            public void Read(SwfReader reader)
            {
                _slotID = (int)reader.ReadUIntEncoded(); //slot_id
                _method = reader.ReadAbcMethod(); //function
            }

            public void Write(SwfWriter writer)
            {
                writer.WriteUIntEncoded((uint)_slotID);
                writer.WriteUIntEncoded((uint)_method.Index);
            }
            #endregion

            #region ISupportXmlDump Members
            public void DumpXml(XmlWriter writer)
            {
                writer.WriteElementString("slot-id", _slotID.ToString());
                if (_method != null && AbcDumpService.DumpMethods)
                {
                    writer.WriteElementString("function", _method.ToString());
                }
            }
            #endregion
        }
        #endregion

        #region class MethodData
        class MethodData : ITraitData, IMethodLink
        {
            #region Properties
            public int SlotID
            {
                get { return _dispID; }
                set { _dispID = value; }
            }
            int _dispID;

            public AbcMethod Method
            {
                get { return _method; }
                set { _method = value; }
            }
            AbcMethod _method;
            #endregion

            #region IAbcAtom Members
            public void Read(SwfReader reader)
            {
                _dispID = (int)reader.ReadUIntEncoded(); //disp_id
                _method = reader.ReadAbcMethod(); //method
            }

            public void Write(SwfWriter writer)
            {
                if (_method == null)
                    throw new InvalidOperationException();
                writer.WriteUIntEncoded((uint)_dispID);
                writer.WriteUIntEncoded((uint)_method.Index);
            }
            #endregion

            #region ISupportXmlDump Members
            public void DumpXml(XmlWriter writer)
            {
                writer.WriteElementString("disp-id", _dispID.ToString());
                if (_method != null && AbcDumpService.DumpMethods)
                {
                    _method.DumpXml(writer);
                }
            }
            #endregion
        }
        #endregion
        #endregion

        #region Constructors
        public AbcTrait()
        {
            Kind = AbcTraitKind.Slot;
        }

        public AbcTrait(AbcTraitKind kind)
        {
            Kind = kind;
        }

        public AbcTrait(AbcTraitKind kind, AbcMethod method)
        {
            Kind = kind;
            Method = method;
        }

        public AbcTrait(SwfReader reader)
        {
            Read(reader);
        }

        public static AbcTrait CreateSlot(AbcMultiname type, AbcMultiname name)
        {
            var t = new AbcTrait(AbcTraitKind.Slot)
                        {
                            Name = name,
                            SlotType = type
                        };
            return t;
        }

        public static AbcTrait CreateConst(AbcMultiname type, AbcMultiname name, object value)
        {
            var t = new AbcTrait(AbcTraitKind.Const)
                        {
                            Name = name,
                            SlotType = type,
                            SlotValue = value
                        };
            return t;
        }

        public static AbcTrait CreateMethod(AbcMethod method)
        {
            return new AbcTrait(AbcTraitKind.Method, method);
        }

        public static AbcTrait CreateMethod(AbcMethod method, AbcMultiname name)
        {
            var t = CreateMethod(method);
            t.Name = name;
            return t;
        }

        public static AbcTrait CreateClass(AbcClass klass)
        {
            var trait = new AbcTrait(AbcTraitKind.Class)
                            {
                                Name = klass.Instance.Name,
                                Class = klass
                            };
            return trait;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets trait owner.
        /// </summary>
        public IAbcTraitProvider Owner { get; set; }

        public AbcInstance Instance
        {
            get
            {
                var i = Owner as AbcInstance;
                if (i != null) return i;
                var c = Owner as AbcClass;
                if (c != null) return c.Instance;
                return null;
            }
        }

        public string OwnerFullName
        {
            get
            {
                var i = Owner as AbcInstance;
                if (i != null) return i.FullName;
                var c = Owner as AbcClass;
                if (c != null)
                {
                    i = c.Instance;
                    if (i != null) return i.FullName;
                }
                return null;
            }
        }

        public int Index { get; set; }

        /// <summary>
        /// Gets or sets trait name
        /// </summary>
        public AbcMultiname Name
        {
            get { return _name; }
            set
            {
                _name = value;
                _key = null;
            }
        }
        AbcMultiname _name;

        public string NameString
        {
            get
            {
                if (_name != null)
                    return _name.NameString;
                return "";
            }
        }

        public string FullName
        {
            get
            {
                if (_name != null)
                    return _name.FullName;
                return "";
            }
        }

        /// <summary>
        /// Used for fields.
        /// </summary>
        public IType Type { get; set; }

        public Visibility Visibility
        {
            get
            {
                if (_name != null)
                    return _name.Visibility;
                return Visibility.Private;
            }
        }

        #region Kind
        /// <summary>
        /// Gets or sets trait kind
        /// </summary>
        public AbcTraitKind Kind
        {
            get { return _kind; }
            set
            {
                if (value != _kind || _data == null)
                {
                    _kind = value;
                    _key = null;
                    switch (value)
                    {
                        case AbcTraitKind.Slot:
                        case AbcTraitKind.Const:
                            if (!(_data is SlotData))
                                _data = new SlotData();
                            break;

                        case AbcTraitKind.Method:
                        case AbcTraitKind.Getter:
                        case AbcTraitKind.Setter:
                            if (!(_data is MethodData))
                                _data = new MethodData();
                            break;

                        case AbcTraitKind.Class:
                            if (!(_data is ClassData))
                                _data = new ClassData();
                            break;

                        case AbcTraitKind.Function:
                            if (!(_data is FunctionData))
                                _data = new FunctionData();
                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }
        AbcTraitKind _kind;

        public bool IsSlot
        {
            get
            {
                return _kind == AbcTraitKind.Slot;
            }
        }

        public bool IsField
        {
            get { return _kind == AbcTraitKind.Const || _kind == AbcTraitKind.Slot; }
        }

        public bool IsConst
        {
            get { return _kind == AbcTraitKind.Const; }
            set { _kind = value ? AbcTraitKind.Const : AbcTraitKind.Slot; }
        }

        /// <summary>
        /// Determines whether this trait is method (method, function, getter or setter).
        /// </summary>
        public bool IsMethod
        {
            get
            {
                return _kind == AbcTraitKind.Method || _kind == AbcTraitKind.Function
                       || _kind == AbcTraitKind.Getter || _kind == AbcTraitKind.Setter;
            }
        }

        /// <summary>
        /// Returns true if the trait is getter.
        /// </summary>
        public bool IsGetter
        {
            get { return _kind == AbcTraitKind.Getter; }
        }

        /// <summary>
        /// Returns true if the trait is getter.
        /// </summary>
        public bool IsSetter
        {
            get { return _kind == AbcTraitKind.Setter; }
        }

        /// <summary>
        /// Gets whether the trait is getter or setter.
        /// </summary>
        public bool IsAccessor
        {
            get { return _kind == AbcTraitKind.Getter || _kind == AbcTraitKind.Setter; }
        }
        #endregion

        public IProperty Property { get; set; }

        public IField Field { get; set; }

        #region Attributes
        /// <summary>
        /// Gets or sets trait attributes
        /// </summary>
        public AbcTraitAttributes Attributes
        {
            get { return _attrs; }
            set { _attrs = value; }
        }
        AbcTraitAttributes _attrs;

        public bool IsStatic
        {
            get
            {
                if (Owner != null)
                {
                    return (Owner is AbcClass) || (Owner is AbcScript);
                }
                return false;
            }
        }

        public bool IsFinal
        {
            get { return (_attrs & AbcTraitAttributes.Final) != 0; }
            set
            {
                if (value) _attrs |= AbcTraitAttributes.Final;
                else _attrs &= ~AbcTraitAttributes.Final;
            }
        }

        public bool IsVirtual
        {
            get { return (_attrs & AbcTraitAttributes.Final) == 0; }
            set
            {
                if (value) _attrs &= ~AbcTraitAttributes.Final;
                else _attrs |= AbcTraitAttributes.Final;
            }
        }

        public bool IsOverride
        {
            get { return (_attrs & AbcTraitAttributes.Override) != 0; }
            set
            {
                if (value) _attrs |= AbcTraitAttributes.Override;
                else _attrs &= ~AbcTraitAttributes.Override;
            }
        }

        public bool IsNew
        {
            get { return (_attrs & AbcTraitAttributes.Override) == 0; }
            set
            {
                if (value) _attrs &= ~AbcTraitAttributes.Override;
                else _attrs |= AbcTraitAttributes.Override;
            }
        }

        public AbcMethodSemantics MethodSemantics
        {
            get
            {
                var r = AbcMethodSemantics.Default;
                if (IsStatic)
                {
                    r |= AbcMethodSemantics.Static;
                }
                else
                {
                    if (IsVirtual)
                        r |= AbcMethodSemantics.Virtual;
                    if (IsOverride)
                        r |= AbcMethodSemantics.Override;
                }
                return r;
            }
        }
        #endregion

        #region Data
        ITraitData _data;

        public int SlotID
        {
            get
            {
                if (_data != null)
                    return _data.SlotID;
                return 0;
            }
            set
            {
                if (_data != null)
                    _data.SlotID = value;
            }
        }

        public bool HasValue
        {
            get
            {
                var s = _data as SlotData;
                if (s != null)
                    return s.HasValue;
                return false;
            }
            set
            {
                var s = _data as SlotData;
                if (s == null)
                    throw new InvalidOperationException();
                s.HasValue = value;
            }
        }

        public object SlotValue
        {
            get
            {
                var s = _data as SlotData;
                if (s != null)
                    return s.Value;
                return null;
            }
            set
            {
                var s = _data as SlotData;
                if (s == null)
                    throw new InvalidOperationException();
                s.Value = value;
            }
        }

        public AbcMultiname SlotType
        {
            get
            {
                var st = _data as SlotData;
                if (st != null) return st.Type;
                return null;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                var st = _data as SlotData;
                if (st == null)
                    throw new InvalidOperationException();
                st.Type = value;
            }
        }

        //TODO: May be should divide property on two for Method and Function separately
        public AbcMethod Method
        {
            get
            {
                var ml = _data as IMethodLink;
                if (ml != null)
                    return ml.Method;
                return null;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                var ml = _data as IMethodLink;
                if (ml == null)
                    throw new InvalidOperationException();
                if (value != ml.Method)
                {
                    ml.Method = value;
                    value.Trait = this;
                }
            }
        }

        public AbcClass Class
        {
            get
            {
                var c = _data as ClassData;
                if (c == null) return null;
                return c.Class;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                var c = _data as ClassData;
                if (c == null)
                    throw new InvalidOperationException();
                if (value != c.Class)
                {
                    c.Class = value;
                    value.Trait = this;
                }
            }
        }
        #endregion

        /// <summary>
        /// Embedded asset info.
        /// </summary>
        internal Embed Embed { get; set; }

        internal AbcInstance AssetInstance { get; set; }

        #region Metadata
        public AbcMetadata Metadata
        {
            get
            {
                if (_metadata == null)
                {
                    _metadata = new AbcMetadata();
                    _attrs |= AbcTraitAttributes.HasMetadata;
                }
                return _metadata;
            }
        }
        AbcMetadata _metadata;

        public bool HasMetadata
        {
            get
            {
                if (_metadata != null && _metadata.Count > 0)
                {
                    _attrs |= AbcTraitAttributes.HasMetadata;
                    return true;
                }
                return false;
            }
            set
            {
                if (value)
                {
                    _attrs |= AbcTraitAttributes.HasMetadata;
                    if (_metadata == null)
                        _metadata = new AbcMetadata();
                }
                else
                {
                    _attrs &= ~AbcTraitAttributes.HasMetadata;
                    _metadata = null;
                }
            }
        }
        #endregion
        #endregion

        #region ISwfAtom Members
        public void Read(SwfReader reader)
        {
            Name = reader.ReadMultiname();

            byte kind = reader.ReadUInt8();
            Kind = (AbcTraitKind)(kind & 0x0F);
            _attrs = (AbcTraitAttributes)(kind >> 4);

            _data.Read(reader);

            switch (_kind)
            {
                case AbcTraitKind.Method:
                case AbcTraitKind.Getter:
                case AbcTraitKind.Setter:
                case AbcTraitKind.Function:
                    Method.Trait = this;
                    break;

                case AbcTraitKind.Class:
                    Class.Trait = this;
                    break;
            }

            if ((_attrs & AbcTraitAttributes.HasMetadata) != 0)
            {
                _metadata = new AbcMetadata();
                int n = (int)reader.ReadUIntEncoded();
                for (int i = 0; i < n; ++i)
                {
                    int index = (int)reader.ReadUIntEncoded();
                    _metadata.Add(reader.ABC.Metadata[index]);
                }
            }
        }

        public void Write(SwfWriter writer)
        {
            writer.WriteUIntEncoded((uint)_name.Index);

            if (_metadata == null)
                _attrs &= ~AbcTraitAttributes.HasMetadata;
            else
                _attrs |= AbcTraitAttributes.HasMetadata;

            int kind = ((int)_kind & 0x0F) | ((int)_attrs << 4);
            writer.WriteUInt8((byte)kind);

            _data.Write(writer);

            if (_metadata != null)
            {
                int n = _metadata.Count;
                writer.WriteUIntEncoded((uint)n);
                for (int i = 0; i < n; ++i)
                {
                    writer.WriteUIntEncoded((uint)_metadata[i].Index);
                }
            }
        }
        #endregion

        #region Dump
        #region Xml Dump
        public void DumpXml(XmlWriter writer)
        {
            writer.WriteStartElement("trait");
            if (_name != null)
            {
                //writer.WriteElementString("name", _name.ToString());
                _name.DumpXml(writer, "name");
            }
            writer.WriteElementString("kind", _kind.ToString());
            writer.WriteElementString("attrs", _attrs.ToString());
            ISupportXmlDump dump = _data;
            if (dump != null)
                dump.DumpXml(writer);
            if (_metadata != null && _metadata.Count > 0)
            {
                _metadata.DumpXml(writer);
            }
            writer.WriteEndElement();
        }
        #endregion

        #region Text Dump
        public void DumpField(TextWriter writer, string tab, bool isStatic)
        {
            writer.Write(tab);

            string vis = SyntaxFormatter.ToString(Visibility);
            if (!string.IsNullOrEmpty(vis))
            {
                writer.Write(vis);
                writer.Write(" ");
            }

            if (Kind == AbcTraitKind.Const)
                writer.Write("const ");
            else if (isStatic)
                writer.Write("static ");


            writer.Write(SlotType.ToString());
            writer.Write(" ");
            writer.Write(_name.ToString());

            if (HasValue)
            {
                writer.Write(" = ");
                var val = SlotValue;
                string s = val as string;
                if (s != null)
                {
                    writer.Write('\"');
                    writer.Write(s);
                    writer.Write('\"');
                }
                else
                {
                    writer.Write(val != null ? val.ToString() : "null");
                }
            }

            writer.Write(";");

            writer.WriteLine();
        }
        #endregion
        #endregion

        #region Object Overrides
        public override string ToString()
        {
            if (_name != null)
                return _name.ToString();
            return base.ToString();
        }
        #endregion

        #region Verify
        internal void Verify()
        {
            if (_data == null)
                throw new InvalidOperationException();
            switch (_kind)
            {
                case AbcTraitKind.Slot:
                case AbcTraitKind.Const:
                    if (SlotType == null)
                        throw new InvalidOperationException();
                    break;

                case AbcTraitKind.Class:
                    {
                        var k = Class;
                        if (k == null)
                            throw new InvalidOperationException();
                        Debug.Assert(k.Trait == this);
                    }
                    break;

                case AbcTraitKind.Function:
                case AbcTraitKind.Getter:
                case AbcTraitKind.Method:
                case AbcTraitKind.Setter:
                    {
                        var m = Method;
                        if (m == null)
                            throw new InvalidOperationException();
                        Debug.Assert(m.Trait == this);
                    }
                    break;
            }
        }
        #endregion

        internal string Key
        {
            get
            {
                if (_key == null && _name != null)
                    _key = MakeKey(_kind, _name);
                return _key;
            }
            set { _key = value; }
        }
        string _key;

        internal static string MakeKey(AbcTraitKind kind, AbcMultiname name)
        {
            return (int)kind + ":" + name.Key;
        }

        internal AbcTrait PtrSlot;
        internal PointerKind PtrKind;
    }
    #endregion

    enum PointerKind
    {
        None,
        FuncPtr,
        PropertyPtr,
        SlotPtr
    }
}