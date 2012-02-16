using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.CodeModel.Syntax;
using DataDynamics.PageFX.FLI.IL;
using DataDynamics.PageFX.FLI.SWF;

namespace DataDynamics.PageFX.FLI.ABC
{
    /// <summary>
    /// Describes method signature.
    /// </summary>
    public class AbcMethod : ISupportXmlDump, ISwfIndexedAtom
    {
        #region Constructors
        public AbcMethod()
        {
        }

        public AbcMethod(SwfReader reader)
        {
            Read(reader);
        }

        public AbcMethod(IMethod method)
        {
            if (method == null)
                throw new ArgumentNullException();
            SourceMethod = method;
            method.Tag = this;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the index of this method signature within method array in ABC file.
        /// </summary>
        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }
        int _index = -1;

        public AbcFile ABC { get; set; }
        
        /// <summary>
        /// Gets or sets method name.
        /// </summary>
        public AbcConst<string> Name
        {
            get
            {
                if (_trait != null)
                    return _trait.Name.Name;
                return _name;
            }
            set { _name = value; }
        }
        AbcConst<string> _name;

        public string NameString
        {
            get
            {
                if (_trait != null)
                    return _trait.Name.ToString();
                if (_name != null)
                    return _name.Value;
                return string.Empty;
            }
        }

        public string ShortName
        {
            get
            {
                if (_trait != null)
                    return _trait.Name.FullName;
                if (_name != null)
                    return _name.Value;
                return string.Empty;
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
        public AbcMultiname ReturnType
        {
            get { return _returnType; }
            set { _returnType = value; }
        }
        AbcMultiname _returnType;

        public bool IsVoid
        {
            get
            {
                if (SourceMethod != null)
                {
                    return MethodHelper.IsVoid(SourceMethod);
                }
                if (_returnType == null) //any
                    return false;
                return _returnType.FullName == "void";
            }
        }

        /// <summary>
        /// Gets the method param list.
        /// </summary>
        public AbcParameterList Parameters
        {
            get { return _params; }
        }
        readonly AbcParameterList _params = new AbcParameterList();

        public int ParamCount
        {
            get { return _params.Count; }
        }

        public int ActualParamCount
        {
            get
            {
                int n = ParamCount;
                if (NeedRest)
                    return n + 1;
                return n;
            }
        }

        public void AddParam(AbcParameter p)
        {
            _params.Add(p);
        }

        public void AddParam(AbcMultiname type, AbcConst<string> name)
        {
            _params.Add(new AbcParameter(type, name));
        }

        #region Flags
        public AbcMethodFlags Flags
        {
            get { return _flags; }
            set { _flags = value; }
        }
        AbcMethodFlags _flags;

        public bool IsNative
        {
            get
            {
                var instance = Instance;
                if (instance != null && instance.IsNative)
                    return true;
                return (_flags & AbcMethodFlags.Native) != 0;
            }
            set
            {
                if (value) _flags |= AbcMethodFlags.Native;
                else _flags &= ~AbcMethodFlags.Native;
            }
        }

        public bool HasParamNames
        {
            get { return (_flags & AbcMethodFlags.HasParamNames) != 0; }
            set
            {
                if (value) _flags |= AbcMethodFlags.HasParamNames;
                else _flags &= ~AbcMethodFlags.HasParamNames;
            }
        }

        public bool HasOptionalParams
        {
            get { return (_flags & AbcMethodFlags.HasOptional) != 0; }
            set
            {
                if (value) _flags |= AbcMethodFlags.HasOptional;
                else _flags &= ~AbcMethodFlags.HasOptional;
            }
        }

        public bool NeedRest
        {
            get { return (_flags & AbcMethodFlags.NeedRest) != 0; }
            set
            {
                if (value) _flags |= AbcMethodFlags.NeedRest;
                else _flags &= ~AbcMethodFlags.NeedRest;
            }
        }

    	public bool IsInitializer { get; set; }

    	public bool IsEntryPoint
        {
            get
            {
                if (SourceMethod != null)
                    return SourceMethod.IsEntryPoint;
                return false;
            }
        }
        #endregion

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
        AbcTrait _trait;

        public AbcMultiname TraitName
        {
            get
            {
                if (_trait != null)
                    return _trait.Name;
                return null;
            }
        }

        public bool IsAbstract
        {
            get { return _body == null; }
        }

        public bool IsOverride
        {
            get
            {
                if (_trait != null)
                    return _trait.IsOverride;
                return false;
            }
        }

        /// <summary>
        /// Determines whether the method is getter.
        /// </summary>
        public bool IsGetter
        {
            get
            {
                if (_trait != null)
                    return _trait.IsGetter;
                return false;
            }
        }

        /// <summary>
        /// Determines whether the method is setter.
        /// </summary>
        public bool IsSetter
        {
            get
            {
                if (_trait != null)
                    return _trait.IsSetter;
                return false;
            }
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
            get
            {
                if (_trait != null)
                    return _trait.Owner;
                return null;
            }
        }

        public AbcInstance Instance
        {
            get
            {
                if (_trait != null)
                    return _trait.Instance;
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        AbcInstance _instance;

        public AbcClass Class
        {
            get
            {
                var i = Instance;
                if (i != null)
                    return i.Class;
                return Owner as AbcClass;
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
        AbcMethodBody _body;

        public Visibility Visibility
        {
            get
            {
                if (_trait != null)
                    return _trait.Visibility;
                return Visibility.Private;
            }
        }

        public IMethod SourceMethod { get; set; }

        internal AbcMethod ImportedMethod { get; set; }

        internal AbcMethod OriginalMethod { get; set; }

        internal bool IsImported;
        #endregion

        #region IO
        public void Read(SwfReader reader)
        {
            _begin = (int)reader.Position;

            int param_count = (int)reader.ReadUIntEncoded();

            _returnType = AbcIO.ReadMultiname(reader); //ret_type

            _beginParamTypes = (int)reader.Position;

            //U30 param_types[param_count]
            for (int i = 0; i < param_count; ++i)
            {
				var type = AbcIO.ReadMultiname(reader);
            	_params.Add(new AbcParameter {Type = type});
            }

            _beginName = (int)reader.Position;
            _name = AbcIO.ReadString(reader); //name_index
            _flags = (AbcMethodFlags)reader.ReadUInt8();

            _beginParamValues = (int)reader.Position;
            if ((_flags & AbcMethodFlags.HasOptional) != 0)
            {
                int optionalCount = (int)reader.ReadUIntEncoded();
                int firstOptionalParam = param_count - optionalCount;
                for (int i = 0; i < optionalCount; ++i)
                {
                    int valueIndex = (int)reader.ReadUIntEncoded();
                    var valueKind = (AbcConstKind)reader.ReadUInt8();
                    var p = _params[firstOptionalParam + i];
                    p.Value = reader.ABC.GetConstant(valueKind, valueIndex);
                    p.IsOptional = true;
                }
            }
            
            _beginParamNames = (int)reader.Position;
            if ((_flags & AbcMethodFlags.HasParamNames) != 0)
            {
                for (int i = 0; i < param_count; ++i)
                {
                    _params[i].Name = AbcIO.ReadString(reader);
                }
            }
            _end = (int)reader.Position;
        }

        public void Write(SwfWriter writer)
        {
            var abc = writer.ABC;

            int param_count = _params.Count;
            writer.WriteUIntEncoded((uint)param_count);

            if (_returnType == null) writer.WriteUInt8(0);
            else writer.WriteUIntEncoded((uint)_returnType.Index);

            //U30 param_types[param_count]
            for (int i = 0; i < param_count; ++i)
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
            writer.WriteUInt8((byte)_flags);

            //param values
            if (HasOptionalParams)
            {
                int optionalCount = _params.Count(p => p.IsOptional);

                writer.WriteUIntEncoded((uint)optionalCount);
                for (int i = 0; i < param_count; ++i)
                {
                    var p = _params[i];
                    if (p.IsOptional)
                    {
                        AbcIO.WriteConstIndex(writer, p.Value);
                    }
                }
            }

            //param names
            if (HasParamNames)
            {
                for (int i = 0; i < param_count; ++i)
                {
                    var p = _params[i];
                    if (p.Name == null) writer.WriteUInt8(0);
                    else writer.WriteUIntEncoded((uint)p.Name.Index);
                }
            }
        }

        int _begin;
        int _end;
        int _beginParamTypes;
        int _beginName;
        int _beginParamValues;
        int _beginParamNames;
        
        public string FormatOffset(int offset, int index)
        {
            if (offset >= _begin && offset < _end)
            {
                if (offset < _beginParamTypes)
                    return string.Format("Method {0} before param types", index);
                if (offset < _beginName)
                    return string.Format("Method {0} in param types", index);
                if (offset < _beginParamValues)
                    return string.Format("Method {0} in method name", index);
                if (offset < _beginParamNames)
                    return string.Format("Method {0} in param values", index);
                return string.Format("Method {0} in param names", index);
            }
            return null;
        }
        #endregion

        #region Xml Dump
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
            writer.WriteElementString("flags", _flags.ToString());
            _params.DumpXml(writer);

            if (AbcDumpService.DumpCode && _body != null)
                _body.DumpXml(writer);

            writer.WriteEndElement();
        }
        #endregion

        #region Text Dump
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
        #endregion

        #region Object Override Members
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
        #endregion

        #region Internal Members
        internal void Finish(AbcCode code)
        {
            if (_body == null)
                throw new InvalidOperationException("Method has no body");
            _body.Finish(code);
        }
        #endregion

        internal bool IsTypeUsed(AbcMultiname typename)
        {
            if (ReturnType == typename) return true;
        	return Parameters.Any(p => p.Type == typename);
        }

        internal int MethodInfoIndex { get; set; }
    }

    public class AbcMethodCollection : List<AbcMethod>, ISwfAtom, ISupportXmlDump
    {
        readonly AbcFile _abc;

        public AbcMethodCollection(AbcFile abc)
        {
            _abc = abc;
        }

        #region Public Members
        public new void Add(AbcMethod method)
        {
            if (method.ABC != null)
                throw new InvalidOperationException();
            method.ABC = _abc;
            method.Index = Count;
            base.Add(method);
        }
        #endregion

        #region IAbcAtom Members
        //int _begin;
        //int _end;

        public void Read(SwfReader reader)
        {
            //_begin = (int)reader.Position;
            int n = (int)reader.ReadUIntEncoded();
            for (int i = 0; i < n; ++i)
            {
                Add(new AbcMethod(reader));
            }
            //_end = (int)reader.Position;
        }

        public void Write(SwfWriter writer)
        {
            int n = Count;
            writer.WriteUIntEncoded((uint)n);
            for (int i = 0; i < n; ++i)
                this[i].Write(writer);
        }

        public string FormatOffset(int offset)
        {
            int n = Count;
            for (int i = 0; i < n; ++i)
            {
                var m = this[i];
                string s = m.FormatOffset(offset, i);
                if (!string.IsNullOrEmpty(s))
                    return s;
            }
            return null;
            //return AbcUtils.FormatOffset(offset, this, _begin, _end, "Methods", false, true);
        }
        #endregion

        #region Dump
        public void DumpXml(XmlWriter writer)
        {
            if (!AbcDumpService.DumpFunctions) return;
            writer.WriteStartElement("methods");
            writer.WriteAttributeString("count", Count.ToString());
            foreach (var m in this)
            {
                if (m.Trait != null) continue;
                if (m.IsInitializer) continue;
                m.DumpXml(writer);
            }
            writer.WriteEndElement();
        }

        public void Dump(TextWriter writer, string tab, bool isStatic)
        {
            int n = Count;
            for (int i = 0; i < n; ++i)
            {
                if (i > 0) writer.WriteLine();
                this[i].Dump(writer, tab, isStatic);
            }
        }
        #endregion
    }

    public delegate void AbcMethodHandler(AbcMethod method);
}