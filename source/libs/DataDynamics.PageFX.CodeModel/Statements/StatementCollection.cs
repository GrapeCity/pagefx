using System;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.CodeModel.Syntax;

namespace DataDynamics.PageFX.CodeModel.Statements
{
    public sealed class StatementCollection : List<IStatement>, IStatementCollection
    {
    	public StatementCollection()
        {
            ParentStatement = null;
        }

        public StatementCollection(IStatement parent)
        {
            ParentStatement = parent;
        }

    	/// <summary>
    	/// Gets or sets parent statement
    	/// </summary>
    	public IStatement ParentStatement { get; set; }

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

    	public CodeNodeType NodeType
        {
            get { return CodeNodeType.Statement; }
        }

        public IEnumerable<ICodeNode> ChildNodes
        {
            get { return this.Cast<ICodeNode>(); }
        }

    	/// <summary>
    	/// Gets or sets user defined data assotiated with this object.
    	/// </summary>
    	public object Tag { get; set; }

    	public string ToString(string format, IFormatProvider formatProvider)
        {
            return SyntaxFormatter.Format(this, format, formatProvider);
        }

    	public override string ToString()
        {
            return ToString(null, null);
        }

        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var s = obj as IStatementCollection;
            if (s == null) return false;
            return this.EqualsTo(s);
        }

        public override int GetHashCode()
        {
            return this.EvalHashCode();
        }
    }

    public class StatementCollection<T> : List<T>, IStatementCollection<T> where T : IStatement
    {
    	public StatementCollection()
        {
            ParentStatement = null;
        }

        public StatementCollection(IStatement parent)
        {
            ParentStatement = parent;
        }

    	/// <summary>
    	/// Gets or sets parent statement
    	/// </summary>
    	public IStatement ParentStatement { get; set; }

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

    	public CodeNodeType NodeType
        {
            get { return CodeNodeType.Statement; }
        }

        public IEnumerable<ICodeNode> ChildNodes
        {
            get { return this.Cast<ICodeNode>(); }
        }

    	/// <summary>
    	/// Gets or sets user defined data assotiated with this object.
    	/// </summary>
    	public object Tag { get; set; }

    	public string ToString(string format, IFormatProvider formatProvider)
        {
            return SyntaxFormatter.Format(this, format, formatProvider);
        }

    	public override string ToString()
        {
            return ToString(null, null);
        }

        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var s = obj as IStatementCollection;
            if (s == null) return false;
            return this.EqualsTo(s);
        }

        public override int GetHashCode()
        {
            return this.EvalHashCode();
        }
    }
}