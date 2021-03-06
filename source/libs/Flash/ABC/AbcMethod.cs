using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using DataDynamics.PageFX.Common;
using DataDynamics.PageFX.Common.Syntax;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Flash.Core;
using DataDynamics.PageFX.Flash.IL;
using DataDynamics.PageFX.Flash.Swf;

namespace DataDynamics.PageFX.Flash.Abc
{
    /// <summary>
    /// Describes method signature.
    /// </summary>
    public sealed class AbcMethod : ISupportXmlDump, ISwfIndexedAtom, IMethodCall
    {
	    public AbcMethod()
        {
        }

	    public AbcMethod(IMethod method)
        {
            if (method == null)
                throw new ArgumentNullException();
            Method = method;
        }

	    /// <summary>
        /// Gets or sets the index of this method signature within method array in ABC file.
        /// </summary>
        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }
        int _index = -1;

        public AbcFile Abc { get; set; }
        
        /// <summary>
        /// Gets or sets method name.
        /// </summary>
        public AbcConst<string> Name
        {
            get { return _trait != null ? _trait.Name.Name : _name; }
	        set { _name = value; }
        }
        private AbcConst<string> _name;

        public string NameString
        {
            get
            {
                if (_trait != null)
                    return _trait.Name.ToString();
                return _name != null ? _name.Value : string.Empty;
            }
        }

        public string ShortName
        {
            get
            {
                if (_trait != null)
                    return _trait.Name.FullName;
                return _name != null ? _name.Value : string.Empty;
            }
        }

        public string FullName
        {
            get
            {
                var i = Instance;
                if (i == null) return null;

                string name = i.FullName;
                name += ".";
                if (_trait != null)
                {
                    name += _trait.Name.NameString;
                }
                else
                {
                    if (IsInitializer)
                    {
                        if (i.Initializer == this)
                            name += CLRNames.Constructor;
                        else
                            name += CLRNames.StaticConstructor;
                    }
                }
                return name;
            }
        }

	    /// <summary>
	    /// Gets or sets method return type.
	    /// </summary>
	    public AbcMultiname ReturnType { get; set; }

	    public bool IsVoid
        {
            get
            {
                if (Method != null)
                {
                    return Method.ReturnsVoid();
                }
                if (ReturnType == null) //any
                    return false;
                return ReturnType.FullName == "void";
            }
        }

        /// <summary>
        /// Gets the method param list.
        /// </summary>
        public AbcParameterList Parameters
        {
            get { return _params; }
        }
        private readonly AbcParameterList _params = new AbcParameterList();

	    public int ActualParamCount
        {
            get
            {
                int n = Parameters.Count;
                if (NeedRest)
                    return n + 1;
                return n;
            }
        }

	    public void AddParam(AbcMultiname type, AbcConst<string> name)
        {
	        Parameters.Add(new AbcParameter(type, name));
        }

	    public AbcMethodFlags Flags { get; set; }

	    public bool IsNative
        {
            get
            {
                var instance = Instance;
                if (instance != null && instance.IsNative)
                    return true;
                return (Flags & AbcMethodFlags.Native) != 0;
            }
            set
            {
                if (value) Flags |= AbcMethodFlags.Native;
                else Flags &= ~AbcMethodFlags.Native;
            }
        }

        public bool HasParamNames
        {
            get { return (Flags & AbcMethodFlags.HasParamNames) != 0; }
            set
            {
                if (value) Flags |= AbcMethodFlags.HasParamNames;
                else Flags &= ~AbcMethodFlags.HasParamNames;
            }
        }

        public bool HasOptionalParams
        {
            get { return (Flags & AbcMethodFlags.HasOptional) != 0; }
            set
            {
                if (value) Flags |= AbcMethodFlags.HasOptional;
                else Flags &= ~AbcMethodFlags.HasOptional;
            }
        }

        public bool NeedRest
        {
            get { return (Flags & AbcMethodFlags.NeedRest) != 0; }
            set
            {
                if (value) Flags |= AbcMethodFlags.NeedRest;
                else Flags &= ~AbcMethodFlags.NeedRest;
            }
        }

    	public bool IsInitializer { get; set; }

    	public bool IsEntryPoint
        {
            get
            {
                if (Method != null)
                    return Method.IsEntryPoint;
                return false;
            }
        }

	    /// <summary>
        /// Gets or sets assotiated method <see cref="AbcTrait"/>
        /// </summary>
        public AbcTrait Trait
        {
            get { return _trait; }
            set
            {
                if (value != _trait)
                {
                    if (value != null && !value.IsMethod)
                        throw new ArgumentException("Invalid trait kind");
                    _trait = value;
                    if (_trait != null)
                        _trait.Method = this;
                }
            }
        }
        private AbcTrait _trait;

        public AbcMultiname TraitName
        {
            get
            {
                if (_trait != null)
                    return _trait.Name;
                return null;
            }
        }

		AbcMultiname IMethodCall.Name 
		{
			get { return TraitName; }
		}

        public bool IsAbstract
        {
            get { return _body == null; }
        }

        public bool IsOverride
        {
            get { return _trait != null && _trait.IsOverride; }
        }

        /// <summary>
        /// Determines whether the method is getter.
        /// </summary>
        public bool IsGetter
        {
            get { return _trait != null && _trait.IsGetter; }
        }

        /// <summary>
        /// Determines whether the method is setter.
        /// </summary>
        public bool IsSetter
        {
            get { return _trait != null && _trait.IsSetter; }
        }

        /// <summary>
        /// Determines whether the method is getter or setter.
        /// </summary>
        public bool IsAccessor
        {
            get
            {
                if (_trait != null)
                    return _trait.IsAccessor;
                return false;
            }
        }

        public IAbcTraitProvider Owner
        {
            get { return _trait != null ? _trait.Owner : null; }
        }

        public AbcInstance Instance
        {
            get { return _trait != null ? _trait.Instance : _instance; }
	        set { _instance = value; }
        }
        private AbcInstance _instance;

        public AbcClass Class
        {
            get
            {
	            var instance = Instance;
	            return instance != null ? instance.Class : Owner as AbcClass;
            }
        }

        public AbcMethodBody Body
        {
            get { return _body; }
            set
            {
                if (value != _body)
                {
                    _body = value;
                    _body.Method = this;
                }
            }
        }
        private AbcMethodBody _body;

        public Visibility Visibility
        {
            get
            {
                if (_trait != null)
                    return _trait.Visibility;
                return Visibility.Private;
            }
        }

		/// <summary>
		/// Gets source method.
		/// </summary>
        public IMethod Method { get; set; }

        internal AbcMethod ImportedMethod { get; set; }

        internal AbcMethod OriginalMethod { get; set; }

        internal bool IsImported;

	    internal bool IsGenerated
	    {
		    get
		    {
				if (Method != null)
					return false;
				if (Instance != null)
				{
					if (Instance.IsForeign)
						return false;

					if (Instance.IsInterface && IsInitializer)
						return false;
				}
			    return true;
		    }
	    }

	    public void Read(SwfReader reader)
        {
            int paramCount = (int)reader.ReadUIntEncoded();

            ReturnType = reader.ReadMultiname(); //ret_type

            //U30 param_types[param_count]
            for (int i = 0; i < paramCount; ++i)
            {
				var type = reader.ReadMultiname();
            	_params.Add(new AbcParameter {Type = type});
            }

            _name = reader.ReadAbcString(); //name_index
            Flags = (AbcMethodFlags)reader.ReadUInt8();

            if ((Flags & AbcMethodFlags.HasOptional) != 0)
            {
                int optionalCount = (int)reader.ReadUIntEncoded();
                int firstOptionalParam = paramCount - optionalCount;
                for (int i = 0; i < optionalCount; ++i)
                {
                    int valueIndex = (int)reader.ReadUIntEncoded();
                    var valueKind = (AbcConstKind)reader.ReadUInt8();
                    var p = _params[firstOptionalParam + i];
                    p.Value = reader.ABC.GetConstant(valueKind, valueIndex);
                    p.IsOptional = true;
                }
            }
            
            if ((Flags & AbcMethodFlags.HasParamNames) != 0)
            {
                for (int i = 0; i < paramCount; ++i)
                {
                    _params[i].Name = reader.ReadAbcString();
                }
            }
        }

        public void Write(SwfWriter writer)
        {
            var abc = writer.ABC;

            int paramCount = _params.Count;
            writer.WriteUIntEncoded((uint)paramCount);

            if (ReturnType == null) writer.WriteUInt8(0);
            else writer.WriteUIntEncoded((uint)ReturnType.Index);

            //U30 param_types[param_count]
            for (int i = 0; i < paramCount; ++i)
            {
                var p = _params[i];
                //For now even if one of parameter has no name then we will not write param names at all
                if (abc.AutoComplete)
                {
                    if (p.HasName)
                        HasParamNames = true;
                    if (p.IsOptional)
                        HasOptionalParams = true;
                }
                writer.WriteUIntEncoded((uint)p.Type.Index);
            }

            //name
            if (_name == null) writer.WriteUInt8(0);
            else writer.WriteUIntEncoded((uint)_name.Index);

            //flags
            writer.WriteUInt8((byte)Flags);

            //param values
            if (HasOptionalParams)
            {
                int optionalCount = _params.Count(p => p.IsOptional);

                writer.WriteUIntEncoded((uint)optionalCount);
                for (int i = 0; i < paramCount; ++i)
                {
                    var p = _params[i];
                    if (p.IsOptional)
                    {
                        writer.WriteConstIndex(p.Value);
                    }
                }
            }

            //param names
            if (HasParamNames)
            {
                for (int i = 0; i < paramCount; ++i)
                {
                    var p = _params[i];
                    if (p.Name == null) writer.WriteUInt8(0);
                    else writer.WriteUIntEncoded((uint)p.Name.Index);
                }
            }
        }

	    public void DumpXml(XmlWriter writer)
        {
            DumpXml(writer, "method");
        }

        public void DumpXml(XmlWriter writer, string elemName)
        {
            writer.WriteStartElement(elemName);
            writer.WriteAttributeString("index", _index.ToString());
            if (_name != null)
                writer.WriteElementString("name", _name.Value);

            if (_trait != null)
                writer.WriteElementString("trait", _trait.Name.ToString());

            writer.WriteElementString("returnType", ReturnType.ToString());
            //writer.WriteElementString("signature", ToString("s"));
            writer.WriteElementString("flags", Flags.ToString());
            _params.DumpXml(writer);

            if (AbcDumpService.DumpCode && _body != null)
                _body.DumpXml(writer);

            writer.WriteEndElement();
        }

	    public void Dump(TextWriter writer, string tab, bool isStatic)
        {
            writer.Write(tab);

            string vis = SyntaxFormatter.ToString(Visibility);
            if (!string.IsNullOrEmpty(vis))
            {
                writer.Write(vis);
                writer.Write(" ");
            }

            if (isStatic)
            {
                writer.Write("static ");
            }
            else
            {
                if (_trait != null)
                {
                    if ((_trait.Attributes & AbcTraitAttributes.Final) != 0)
                        writer.Write("final ");
                    if ((_trait.Attributes & AbcTraitAttributes.Override) != 0)
                        writer.Write("override ");
                }
            }

            //writer.Write(ReturnType.ToString());
            writer.Write(ReturnType.FullName);
            writer.Write(" ");

            //writer.Write(NameString);
            writer.Write(ShortName);
            writer.Write("(");
            int n = Parameters.Count;
            if (n > 0)
            {
                for (int i = 0; i < n; ++i)
                {
                    if (i > 0) writer.Write(", ");
                    var p = Parameters[i];
                    //writer.Write(p.Type.ToString());
                    writer.Write(p.Type.FullName);
                    if (p.Name == null || string.IsNullOrEmpty(p.Name.Value))
                    {
                        writer.Write(" ");
                        writer.Write(string.Format("arg{0}", i));
                    }
                    else
                    {
                        writer.Write(" ");
                        writer.Write(p.Name.Value);
                    }
                }
            }
            writer.Write(")");
            
            if (AbcDumpService.DumpCode)
            {
                writer.WriteLine();
                writer.WriteLine(tab + "{");
                if (Body != null)
                    Body.IL.Dump(writer, tab + "\t");
                writer.WriteLine(tab + "}");
            }
            else
            {
                writer.WriteLine(";");
            }
        }

	    public override string ToString()
        {
            return ToString("f");
        }

        public string ToString(string typeFormat)
        {
            var s = new StringBuilder();
            s.AppendFormat("{0}: ", _index);

            string vis = SyntaxFormatter.ToString(Visibility);
            if (!string.IsNullOrEmpty(vis))
            {
                s.Append(vis);
                s.Append(" ");
            }

            if (_trait != null)
            {
                switch (_trait.Kind)
                {
                    case AbcTraitKind.Method:
                        s.Append("method ");
                        break;

                    case AbcTraitKind.Getter:
                        s.Append("get method ");
                        break;

                    case AbcTraitKind.Setter:
                        s.Append("set method ");
                        break;

                    case AbcTraitKind.Function:
                        s.Append("function ");
                        break;
                }
            }

            s.Append(Name);
            s.Append("(");
            int pn = Parameters.Count;
            for (int i = 0; i < pn; ++i)
            {
                if (i > 0) s.Append(", ");
                s.Append(Parameters[i].ToString(typeFormat));
            }
            if (NeedRest)
            {
                if (pn > 0) s.Append(", ");
                s.Append("... rest");
            }
            s.Append(")");
            if (ReturnType != null)
            {
                s.Append(": ");
                s.Append(ReturnType.ToString(typeFormat));
            }
            return s.ToString();
        }

	    internal void Finish(AbcCode code)
        {
            if (_body == null)
                throw new InvalidOperationException("Method has no body");
            _body.Finish(code);
        }

	    internal bool IsTypeUsed(AbcMultiname typeName)
        {
            if (ReferenceEquals(ReturnType, typeName)) return true;
        	return Parameters.Any(p => ReferenceEquals(p.Type, typeName));
        }

        internal int MethodInfoIndex { get; set; }
    }

	[Flags]
	public enum AbcMethodFlags
	{
		None = 0,

		/// <summary>
		/// Suggests to the run-time that an “arguments” object (as specified by the
		/// ActionScript 3.0 Language Reference) be created. Must not be used
		/// together with NeedRest. See Chapter 3.
		/// </summary>
		NeedArguments = 0x01,

		/// <summary>
		/// Must be set if this method uses the newactivation opcode.
		/// </summary>
		NeedActivation = 0x02,

		/// <summary>
		/// This flag creates an ActionScript 3.0 rest arguments array. Must not be
		/// used with NeedArguments. See Chapter 3.
		/// </summary>
		NeedRest = 0x04,

		/// <summary>
		/// Must be set if this method has optional parameters and the options
		/// field is present in this method_info structure.
		/// </summary>
		HasOptional = 0x08,

		/// <summary>
		/// 
		/// </summary>
		IgnoreRest = 0x10,

		/// <summary>
		/// Specifies whether method is native (implementation provided by AVM+)
		/// </summary>
		Native = 0x20,

		/// <summary>
		/// Must be set if this method uses the dxns or dxnslate opcodes.
		/// </summary>
		SetDxns = 0x40,

		/// <summary>
		/// Must be set when the param_names field is present in this method_info structure.
		/// </summary>
		HasParamNames = 0x80,
	}
}