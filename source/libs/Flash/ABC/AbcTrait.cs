using System;
using System.Diagnostics;
using System.IO;
using System.Xml;
using DataDynamics.PageFX.Common.Syntax;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Flash.Core;
using DataDynamics.PageFX.Flash.Swf;

namespace DataDynamics.PageFX.Flash.Abc
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

	/// <summary>
    /// Represents ABC trait (type member)
    /// </summary>
    public sealed class AbcTrait : ISwfIndexedAtom, ISupportXmlDump
    {
        #region InnerTypes
        private interface ISlot : ISwfAtom, ISupportXmlDump
        {
            int SlotId { get; set; }
        }

		private interface IMethodSlot : ISlot
        {
            AbcMethod Method { get; set; }
        }

        #region class Slot
        private sealed class Slot : ISlot
        {
	        /// <summary>
	        /// The slot_id field is an integer from 0 to N and is used to identify a position in which this trait resides. A
	        /// value of 0 requests the AVM2 to assign a position.
	        /// </summary>
	        public int SlotId { get; set; }

	        public AbcMultiname Type { get; set; }

	        public bool HasValue { get; set; }

	        public object Value
            {
                get { return _value; }
                set
                {
                    _value = value;
                    HasValue = true;
                }
            }
            private object _value;

	        public void Read(SwfReader reader)
            {
                SlotId = (int)reader.ReadUIntEncoded(); //slod_id
                Type = reader.ReadMultiname();

                HasValue = false;
                int index = (int)reader.ReadUIntEncoded(); //vindex
                if (index != 0)
                {
                    HasValue = true;
                    var kind = (AbcConstKind)reader.ReadUInt8(); //vkind
                    _value = reader.ABC.GetConstant(kind, index);
                }
            }

            public void Write(SwfWriter writer)
            {
                writer.WriteUIntEncoded((uint)SlotId);
                writer.WriteUIntEncoded((uint)Type.Index);
                if (HasValue)
                {
                    writer.WriteConstIndex(_value);
                }
                else
                {
                    writer.WriteByte(0);
                }
            }

	        public void DumpXml(XmlWriter writer)
            {
                writer.WriteElementString("slot-id", SlotId.ToString());
                writer.WriteElementString("type", Type.ToString());
                if (HasValue)
                    writer.WriteElementString("value", _value != null ? _value.ToString() : "null");
            }
        }
        #endregion

		#region class ClassSlot
		private sealed class ClassSlot : ISlot
        {
	        /// <summary>
            /// The slot_id field is an integer from 0 to N and is used to identify a position in which this trait resides. A
            /// value of 0 requests the AVM2 to assign a position.
            /// </summary>
            public int SlotId { get; set; }

            public AbcClass Class { get; set; }

	        public void Read(SwfReader reader)
            {
                SlotId = (int)reader.ReadUIntEncoded(); //slot_id
                int index = (int)reader.ReadUIntEncoded(); //classi
                Class = reader.ABC.Classes[index];
            }

            public void Write(SwfWriter writer)
            {
                writer.WriteUIntEncoded((uint)SlotId);
                writer.WriteUIntEncoded((uint)Class.Index);
            }

	        public void DumpXml(XmlWriter writer)
            {
                writer.WriteElementString("slot-id", SlotId.ToString());
                if (Class != null)
                    writer.WriteElementString("class", Class.ToString());
            }
        }
        #endregion

		#region class FunctionSlot
		private sealed class FunctionSlot : IMethodSlot
        {
	        public int SlotId { get; set; }

	        public AbcMethod Method { get; set; }

	        public void Read(SwfReader reader)
            {
                SlotId = (int)reader.ReadUIntEncoded(); //slot_id
                Method = reader.ReadAbcMethod(); //function
            }

            public void Write(SwfWriter writer)
            {
                writer.WriteUIntEncoded((uint)SlotId);
                writer.WriteUIntEncoded((uint)Method.Index);
            }

	        public void DumpXml(XmlWriter writer)
            {
                writer.WriteElementString("slot-id", SlotId.ToString());
                if (Method != null && AbcDumpService.DumpMethods)
                {
                    writer.WriteElementString("function", Method.ToString());
                }
            }
        }
        #endregion

		#region class MethodSlot
		private sealed class MethodSlot : IMethodSlot
        {
	        public int SlotId { get; set; }

	        public AbcMethod Method { get; set; }

	        public void Read(SwfReader reader)
            {
                SlotId = (int)reader.ReadUIntEncoded(); //disp_id
                Method = reader.ReadAbcMethod(); //method
            }

            public void Write(SwfWriter writer)
            {
                if (Method == null)
                    throw new InvalidOperationException();
                writer.WriteUIntEncoded((uint)SlotId);
                writer.WriteUIntEncoded((uint)Method.Index);
            }

	        public void DumpXml(XmlWriter writer)
            {
                writer.WriteElementString("disp-id", SlotId.ToString());
                if (Method != null && AbcDumpService.DumpMethods)
                {
                    Method.DumpXml(writer);
                }
            }
        }
        #endregion
        #endregion

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

	    public static AbcTrait CreateSlot(AbcMultiname type, AbcMultiname name)
        {
		    return new AbcTrait(AbcTraitKind.Slot)
	            {
		            Name = name,
		            SlotType = type
	            };
        }

        public static AbcTrait CreateConst(AbcMultiname type, AbcMultiname name, object value)
        {
	        return new AbcTrait(AbcTraitKind.Const)
	            {
		            Name = name,
		            SlotType = type,
		            SlotValue = value
	            };
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
	        return new AbcTrait(AbcTraitKind.Class)
	            {
		            Name = klass.Instance.Name,
		            Class = klass
	            };
        }

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
                return c != null ? c.Instance : null;
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
        private AbcMultiname _name;

        public string NameString
        {
            get { return _name != null ? _name.NameString : ""; }
        }

        public string FullName
        {
            get { return _name != null ? _name.FullName : ""; }
        }

        /// <summary>
        /// Used for fields.
        /// </summary>
        public IType Type { get; set; }

        public Visibility Visibility
        {
            get { return _name != null ? _name.Visibility : Visibility.Private; }
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
                if (value != _kind || _slot == null)
                {
                    _kind = value;
                    _key = null;
                    switch (value)
                    {
                        case AbcTraitKind.Slot:
                        case AbcTraitKind.Const:
                            if (!(_slot is Slot))
                                _slot = new Slot();
                            break;

                        case AbcTraitKind.Method:
                        case AbcTraitKind.Getter:
                        case AbcTraitKind.Setter:
                            if (!(_slot is MethodSlot))
                                _slot = new MethodSlot();
                            break;

                        case AbcTraitKind.Class:
                            if (!(_slot is ClassSlot))
                                _slot = new ClassSlot();
                            break;

                        case AbcTraitKind.Function:
                            if (!(_slot is FunctionSlot))
                                _slot = new FunctionSlot();
                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }
        private AbcTraitKind _kind;

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

	    /// <summary>
	    /// Gets or sets trait attributes
	    /// </summary>
	    public AbcTraitAttributes Attributes { get; set; }

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
            get { return (Attributes & AbcTraitAttributes.Final) != 0; }
            set
            {
                if (value) Attributes |= AbcTraitAttributes.Final;
                else Attributes &= ~AbcTraitAttributes.Final;
            }
        }

        public bool IsVirtual
        {
            get { return (Attributes & AbcTraitAttributes.Final) == 0; }
            set
            {
                if (value) Attributes &= ~AbcTraitAttributes.Final;
                else Attributes |= AbcTraitAttributes.Final;
            }
        }

        public bool IsOverride
        {
            get { return (Attributes & AbcTraitAttributes.Override) != 0; }
            set
            {
                if (value) Attributes |= AbcTraitAttributes.Override;
                else Attributes &= ~AbcTraitAttributes.Override;
            }
        }

        public bool IsNew
        {
            get { return (Attributes & AbcTraitAttributes.Override) == 0; }
            set
            {
                if (value) Attributes &= ~AbcTraitAttributes.Override;
                else Attributes |= AbcTraitAttributes.Override;
            }
        }

        public MethodSemantics MethodSemantics
        {
            get
            {
                var r = MethodSemantics.Default;
                if (IsStatic)
                {
                    r |= Abc.MethodSemantics.Static;
                }
                else
                {
                    if (IsVirtual)
                        r |= Abc.MethodSemantics.Virtual;
                    if (IsOverride)
                        r |= Abc.MethodSemantics.Override;
                }
                return r;
            }
        }

	    #region Data
        private ISlot _slot;

        public int SlotId
        {
            get { return _slot != null ? _slot.SlotId : 0; }
	        set
            {
                if (_slot != null)
                    _slot.SlotId = value;
            }
        }

        public bool HasValue
        {
            get
            {
                var s = _slot as Slot;
                if (s != null)
                    return s.HasValue;
                return false;
            }
            set
            {
                var s = _slot as Slot;
                if (s == null)
                    throw new InvalidOperationException();
                s.HasValue = value;
            }
        }

        public object SlotValue
        {
            get
            {
                var s = _slot as Slot;
                if (s != null)
                    return s.Value;
                return null;
            }
            set
            {
                var s = _slot as Slot;
                if (s == null)
                    throw new InvalidOperationException();
                s.Value = value;
            }
        }

        public AbcMultiname SlotType
        {
            get
            {
                var st = _slot as Slot;
                if (st != null) return st.Type;
                return null;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                var st = _slot as Slot;
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
                var ml = _slot as IMethodSlot;
                if (ml != null)
                    return ml.Method;
                return null;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                var link = _slot as IMethodSlot;
                if (link == null)
                    throw new InvalidOperationException();
                if (value != link.Method)
                {
                    link.Method = value;
                    value.Trait = this;
                }
            }
        }

        public AbcClass Class
        {
            get
            {
                var c = _slot as ClassSlot;
                if (c == null) return null;
                return c.Class;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                var c = _slot as ClassSlot;
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
                    Attributes |= AbcTraitAttributes.HasMetadata;
                }
                return _metadata;
            }
        }
        private AbcMetadata _metadata;

        public bool HasMetadata
        {
            get
            {
                if (_metadata != null && _metadata.Count > 0)
                {
                    Attributes |= AbcTraitAttributes.HasMetadata;
                    return true;
                }
                return false;
            }
            set
            {
                if (value)
                {
                    Attributes |= AbcTraitAttributes.HasMetadata;
                    if (_metadata == null)
                        _metadata = new AbcMetadata();
                }
                else
                {
                    Attributes &= ~AbcTraitAttributes.HasMetadata;
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
            Attributes = (AbcTraitAttributes)(kind >> 4);

            _slot.Read(reader);

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

            if ((Attributes & AbcTraitAttributes.HasMetadata) != 0)
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
                Attributes &= ~AbcTraitAttributes.HasMetadata;
            else
                Attributes |= AbcTraitAttributes.HasMetadata;

            int kind = ((int)_kind & 0x0F) | ((int)Attributes << 4);
            writer.WriteUInt8((byte)kind);

            _slot.Write(writer);

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
            writer.WriteElementString("attrs", Attributes.ToString());
            ISupportXmlDump dump = _slot;
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
            if (_slot == null)
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
        private string _key;

        internal static string MakeKey(AbcTraitKind kind, AbcMultiname name)
        {
            return (int)kind + ":" + name.Key;
        }

        internal AbcTrait PtrSlot;
        internal PointerKind PtrKind;
    }

	internal enum PointerKind
    {
        None,
        FuncPtr,
        PropertyPtr,
        SlotPtr
    }
}