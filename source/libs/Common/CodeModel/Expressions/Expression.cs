using System;
using System.Collections.Generic;
using DataDynamics.PageFX.Common.Syntax;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Common.CodeModel.Expressions
{
    public abstract class Expression : IExpression
    {
    	public abstract IType ResultType
        {
            get;
        }

    	public CodeNodeType NodeType
        {
            get { return CodeNodeType.Expression; }
        }

        public virtual IEnumerable<ICodeNode> ChildNodes
        {
            get { return null; }
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
    }
}