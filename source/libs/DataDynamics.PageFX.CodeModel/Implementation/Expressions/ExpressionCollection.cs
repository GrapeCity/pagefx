using System;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.CodeModel.Syntax;

namespace DataDynamics.PageFX.CodeModel
{
    public sealed class ExpressionCollection : List<IExpression>, IExpressionCollection
    {
        #region ICodeNode Members
        public CodeNodeType NodeType
        {
            get { return CodeNodeType.Expression; }
        }

        public IEnumerable<ICodeNode> ChildNodes
        {
            get { return this.Cast<ICodeNode>(); }
        }

    	/// <summary>
    	/// Gets or sets user defined data assotiated with this object.
    	/// </summary>
    	public object Tag { get; set; }

    	#endregion

        #region IFormattable Members
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return SyntaxFormatter.Format(this, format, formatProvider);
        }
        #endregion

        #region IExpression Members
        public IType ResultType
        {
            get
            {
                int i = Count - 1;
                if (i >= 0) return this[i].ResultType;
                return null;
            }
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
            var e = obj as IExpressionCollection;
            if (e == null) return false;
            int n = Count;
            if (e.Count != n) return false;
            for (int i = 0; i < n; ++i)
            {
                if (!Equals(e[i], this[i]))
                    return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            return this.EvalHashCode();
        }
        #endregion
    }
}