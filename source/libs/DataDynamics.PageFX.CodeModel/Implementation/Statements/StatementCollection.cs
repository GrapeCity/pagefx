using System;
using System.Collections.Generic;
using DataDynamics.PageFX.CodeModel.Syntax;

namespace DataDynamics.PageFX.CodeModel
{
    public class StatementCollection : List<IStatement>, IStatementCollection
    {
        #region Constructors
        public StatementCollection()
        {
            _parent = null;
        }

        public StatementCollection(IStatement parent)
        {
            _parent = parent;
        }
        #endregion

        /// <summary>
        /// Gets or sets parent statement
        /// </summary>
        public IStatement ParentStatement
        {
            get { return _parent; }
            set { _parent = value; }
        }
        private IStatement _parent;

        public new void Add(IStatement st)
        {
            st.ParentStatement = this;
            base.Add(st);
        }

        public new void AddRange(IEnumerable<IStatement> collection)
        {
            foreach (var st in collection)
            {
                Add(st);
            }
        }

        #region ICodeNode Members
        public CodeNodeType NodeType
        {
            get { return CodeNodeType.Statement; }
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

        #region Object Override Members
        public override string ToString()
        {
            return ToString(null, null);
        }

        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var s = obj as IStatementCollection;
            if (s == null) return false;
            return Object2.Equals(this, s);
        }

        public override int GetHashCode()
        {
            return Object2.GetHashCode(this);
        }
        #endregion
    }

    public class StatementCollection<T> : List<T>, IStatementCollection<T> where T : IStatement
    {
        #region Constructors
        public StatementCollection()
        {
            _parent = null;
        }

        public StatementCollection(IStatement parent)
        {
            _parent = parent;
        }
        #endregion

        /// <summary>
        /// Gets or sets parent statement
        /// </summary>
        public IStatement ParentStatement
        {
            get { return _parent; }
            set { _parent = value; }
        }
        private IStatement _parent;

        public new void Add(T st)
        {
            st.ParentStatement = this;
            base.Add(st);
        }

        public new void AddRange(IEnumerable<T> collection)
        {
            foreach (var st in collection)
            {
                Add(st);
            }
        }

        #region ICodeNode Members
        public CodeNodeType NodeType
        {
            get { return CodeNodeType.Statement; }
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

        #region Object Override Members
        public override string ToString()
        {
            return ToString(null, null);
        }

        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var s = obj as IStatementCollection;
            if (s == null) return false;
            return Object2.Equals(this, s);
        }

        public override int GetHashCode()
        {
            return Object2.GetHashCode(this);
        }
        #endregion
    }
}