using System;
using System.Collections.Generic;
using DataDynamics.PageFX.CodeModel.Syntax;

namespace DataDynamics.PageFX.CodeModel
{
    public abstract class Statement : IStatement
    {
        #region IStatement Members

    	public IStatement ParentStatement { get; set; }

    	#endregion

        #region ICodeNode Members

        public CodeNodeType NodeType
        {
            get { return CodeNodeType.Statement; }
        }

        public virtual IEnumerable<ICodeNode> ChildNodes
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
        public override string ToString()
        {
            return ToString(null, null);
        }
        #endregion
    }
}