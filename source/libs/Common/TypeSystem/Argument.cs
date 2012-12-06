using System;
using System.Collections.Generic;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Syntax;

namespace DataDynamics.PageFX.Common.TypeSystem
{
    public class Argument : CustomAttributeProvider, IArgument
    {
        #region Constructors
        public Argument()
        {
        }

        public Argument(object value)
        {
            Value = value;
        }

        public Argument(string name, object value)
        {
            Name = name;
            Value = value;
        }
        #endregion

        #region IArgument Members
        public IType Type { get; set; }

        public ArgumentKind Kind { get; set; }

        /// <summary>
        /// Gets or sets param name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///  Gets or sets param value
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Returns true if the argument is fixed.
        /// </summary>
        public bool IsFixed
        {
            get { return Kind == ArgumentKind.Fixed; }
        }

        /// <summary>
        /// Returns true if the argument is named (not fixed).
        /// </summary>
        public bool IsNamed
        {
            get { return Kind != ArgumentKind.Fixed; }
        }

        /// <summary>
        /// Gets or sets the member to initialize (actual only for named arguments)
        /// </summary>
        public ITypeMember Member { get; set; }
        #endregion

        #region ICodeNode Members

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

        #region ICloneable Members
        public object Clone()
        {
        	return new Argument(Name, Value) {Kind = Kind, Type = Type};
        }
        #endregion

        #region Object Override Methods
        public override string ToString()
        {
            return ToString(null, null);
        }
        #endregion
    }
}