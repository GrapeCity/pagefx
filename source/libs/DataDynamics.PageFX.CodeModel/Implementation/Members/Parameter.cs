using System;
using System.Collections.Generic;
using DataDynamics.PageFX.CodeModel.Syntax;

namespace DataDynamics.PageFX.CodeModel
{
    public class Parameter : CustomAttributeProvider, IParameter
    {
        #region Constructors
        public Parameter()
        {
        }

        public Parameter(IType type, string name)
        {
            _type = type;
            _name = name;
        }

        public Parameter(IType type, string name, int index)
        {
            _type = type;
            _name = name;
            _index = index;
        }
        #endregion

        /// <summary>
        /// Gets or sets param attributes.
        /// </summary>
        public ParamAttributes Flags { get; set; }

        #region IParameter Members
        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }
        private int _index;

        /// <summary>
        /// Gets or sets param type.
        /// </summary>
        public IType Type
        {
            get { return _type; }
            set { _type = value; }
        }
        private IType _type;

        /// <summary>
        /// Gets or sets param name
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private string _name;

        /// <summary>
        ///  Gets or sets param value
        /// </summary>
        public object Value
        {
            get { return _value; }
            set { _value = value; }
        }
        private object _value;

        /// <summary>
        /// Gets the flag indicating whether parameter is passed by reference.
        /// </summary>
        public bool IsByRef
        {
            get
            {
                if (_type != null)
                {
                    return _type.TypeKind == TypeKind.Reference;
                }
                return false;
            }
        }

        public bool IsIn
        {
            get { return (Flags & ParamAttributes.In) != 0; }
        }

        public bool IsOut
        {
            get { return (Flags & ParamAttributes.Out) != 0; }
        }

        /// <summary>
        /// Gets or sets flags indicating whether the method parameter that takes an argument where the number of arguments is variable.
        /// </summary>
        public bool HasParams { get; set; }

        /// <summary>
        /// Gets or sets flag indicating whether address of this parameter used onto the evaluation stack.
        /// </summary>
        public bool IsAddressed { get; set; }

        public IInstruction Instruction { get; set; }

        /// <summary>
        /// Returns true if type of this parameter was changed during resolving.
        /// </summary>
        public bool HasResolvedType { get; set;  }
        #endregion

        #region ICodeNode Members
        public CodeNodeType NodeType
        {
            get { return CodeNodeType.Parameter; }
        }

        public IEnumerable<ICodeNode> ChildNodes
        {
            get { return null; }
        }

        /// <summary>
        /// Gets or sets user defined data assotiated with this object.
        /// </summary>
        public object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }
        private object _tag;
        #endregion

        #region IFormattable Members
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return SyntaxFormatter.Format(this, format, formatProvider);
        }
        #endregion

        #region IDocumentationProvider Members
        /// <summary>
        /// Gets or sets documentation of this member
        /// </summary>
        public string Documentation
        {
            get { return _doc; }
            set { _doc = value; }
        }
        private string _doc;
        #endregion

        #region ICloneable Members
        public object Clone()
        {
            var p = new Parameter(_type, _name, _index)
                        {
                            _doc = _doc,
                            Flags = Flags,
                            HasParams = HasParams,
                            _value = CloneObject(_value),
                            IsAddressed =  IsAddressed,
                            HasResolvedType = HasResolvedType,
                        };
            return p;
        }
        #endregion

        private static object CloneObject(object obj)
        {
            var c = obj as ICloneable;
            if (c != null)
                return c.Clone();
            return obj;
        }

        public override string ToString()
        {
            return ToString(null, null);
        }
    }

    /// <summary>
    /// List of <see cref="Parameter"/>s.
    /// </summary>
    public class ParameterCollection : List<IParameter>, IParameterCollection
    {
        #region IParamaterCollection Members
        public IParameter this[string name]
        {
            get
            {
                return Find(delegate(IParameter p) { return p.Name == name; });
            }
        }
        #endregion

        #region ICodeNode Members
        public CodeNodeType NodeType
        {
            get { return CodeNodeType.Parameters; }
        }

        public IEnumerable<ICodeNode> ChildNodes
        {
            get { return CMHelper.Convert(this); }
        }

        /// <summary>
        /// Gets or sets user defined data assotiated with this object.
        /// </summary>
        public object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }
        private object _tag;
        #endregion

        #region IFormattable Members
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return SyntaxFormatter.Format(this, format, formatProvider);
        }
        #endregion

        #region Object Override Methods
        public override string ToString()
        {
            return ToString(null, null);
        }

        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var c = obj as IParameterCollection;
            if (c != null)
            {
                int n = Count;
                if (c.Count != n) return false;
                for (int i = 0; i < n; ++i)
                {
                    if (this[i].Type != c[i].Type)
                        return false;
                }
                return true;
            }
            var arr = obj as IType[];
            if (arr != null)
            {
                int n = Count;
                if (arr.Length != n) return false;
                for (int i = 0; i < n; ++i)
                {
                    if (this[i].Type != arr[i])
                        return false;
                }
                return true;
            }
            var c2 = obj as ICollection<IType>;
            if (c2 != null)
            {
                int n = Count;
                if (c2.Count != n) return false;
                int i = 0;
                foreach (var type in c2)
                {
                    if (this[i].Type != type)
                        return false;
                    ++i;
                }
                return true;
            }
            var e = obj as IEnumerable<IType>;
            if (e != null)
            {
                int n = Count;
                int i = 0;
                foreach (var type in e)
                {
                    if (this[i].Type != type)
                        return false;
                    ++i;
                }
                if (i != n) return false;
                return true;
            }
            return false;
        }
        #endregion
    }
}