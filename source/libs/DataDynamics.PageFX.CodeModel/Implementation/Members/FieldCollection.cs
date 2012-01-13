using System;
using System.Collections;
using System.Collections.Generic;
using DataDynamics.Collections;
using DataDynamics.PageFX.CodeModel.Syntax;

namespace DataDynamics.PageFX.CodeModel
{
    [XmlElementName("Fields")]
    public sealed class FieldCollection : IFieldCollection
    {
        readonly List<IField> _list = new List<IField>();
        readonly Hashtable _cache = new Hashtable();
        readonly IType _owner;

        public FieldCollection(IType owner)
        {
            _owner = owner;
        }

        public int Count
        {
            get { return _list.Count; }
        }

        public IField this[int index]
        {
            get { return _list[index]; }
        }

        #region IFieldCollection Members
        public void Add(IField field)
        {
            field.DeclaringType = _owner;
            _list.Add(field);
            _cache[field.Name] = field;
        }

        public IField this[string name]
        {
            get { return _cache[name] as IField; }
        }
        #endregion

        #region IEnumerable Members
        public IEnumerator<IField> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        #region ICodeNode Members
        public CodeNodeType NodeType
        {
            get { return CodeNodeType.Methods; }
        }

        public object Tag { get; set; }

        public IEnumerable<ICodeNode> ChildNodes
        {
            get { return CMHelper.Convert(_list); }
        }
        #endregion

        #region IFormattable Members
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return SyntaxFormatter.Format(this, format, formatProvider);
        }
        #endregion

        public override string ToString()
        {
            return ToString(null, null);
        }
    }

    class EmptyFieldCollection : IFieldCollection
    {
        public static readonly IFieldCollection Instance = new EmptyFieldCollection();

        public int Count
        {
            get { return 0; }
        }

        public IField this[int index]
        {
            get { return null; }
        }

        public void Add(IField field)
        {
        }

        #region IFieldCollection Members
        public IField this[string name]
        {
            get { return null; }
        }
        #endregion

        #region ICodeNode Members
        public CodeNodeType NodeType
        {
            get { return CodeNodeType.Fields; }
        }

        public IEnumerable<ICodeNode> ChildNodes
        {
            get { return Algorithms.Convert<IField, ICodeNode>(this); }
        }

        public object Tag
        {
            get { return null; }
            set { }
        }
        #endregion

        #region IFormattable Members
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return "";
        }
        #endregion

        #region IEnumerable Members
        public IEnumerator<IField> GetEnumerator()
        {
            return EmptyEnumerator<IField>.Instance;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }
}