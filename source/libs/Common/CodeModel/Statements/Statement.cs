using System;
using System.Collections.Generic;
using DataDynamics.PageFX.Common.Syntax;

namespace DataDynamics.PageFX.Common.CodeModel.Statements
{
    public abstract class Statement : IStatement
    {
    	public IStatement ParentStatement { get; set; }

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