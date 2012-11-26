using System;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.CodeModel.Syntax;

namespace DataDynamics.PageFX.CodeModel
{
    public class Variable : IVariable
    {
        #region Constructors
        public Variable()
        {
        }

        public Variable(IType type, int index)
        {
            Type = type;
            Index = index;
            Name = "v" + index;
        }
        #endregion

        #region IVariable Members
        public int Index { get; set; }

        public string Name { get; set; }

        public IType Type { get; set; }

        public bool IsPinned { get; set; }

        /// <summary>
        /// Gets or sets flag indicating whether address of this local variable is used onto the evaluation stack.
        /// </summary>
        public bool IsAddressed { get; set; }

        public IType GenericType { get; set; }
        #endregion

        #region ICodeNode Members
        public CodeNodeType NodeType
        {
            get { return CodeNodeType.Variable; }
        }

        public IEnumerable<ICodeNode> ChildNodes
        {
            get { return null; }
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

        #region Object Override Members
        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var v = obj as IVariable;
            if (v == null) return false;
            if (v.Index != Index) return false;
            return true;
        }

        public override int GetHashCode()
        {
            return Index;
        }

        public override string ToString()
        {
            return ToString(null, null);
        }
        #endregion
    }

    public class VariableCollection : List<IVariable>, IVariableCollection
    {
        #region IVariableCollection Members

        public IVariable this[string name]
        {
            get { return Find(v => v.Name == name); }
        }

        #endregion

        #region ICodeNode Members

        public CodeNodeType NodeType
        {
            get { return CodeNodeType.Variables; }
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

        #region Object Override Members
        public override string ToString()
        {
            return ToString(null, null);
        }
        #endregion
    }
}