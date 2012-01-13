using System;
using DataDynamics.PageFX.CodeModel.Syntax;

namespace DataDynamics.PageFX.CodeModel
{
    [XmlElementName("Properties")]
    public sealed class PropertyCollection : ParameterizedMemberCollection<IProperty>, IPropertyCollection
    {
        public PropertyCollection(IType owner) : base(owner)
        {
        }

        #region IPropertyCollection Members
        public IProperty this[string name, IType arg1]
        {
            get
            {
                return base[name,
                    m =>
                    {
                        var p = m.Parameters;
                        if (p.Count != 1) return false;
                        return Signature.TypeEquals(p[0].Type, arg1);
                    }];
            }
        }

        public IProperty this[string name, IType arg1, IType arg2]
        {
            get
            {
                return base[name,
                    m =>
                    {
                        var p = m.Parameters;
                        if (p.Count != 2) return false;
                        if (!Signature.TypeEquals(p[0].Type, arg1)) return false;
                        return Signature.TypeEquals(p[1].Type, arg2);
                    }];
            }
        }

        public IProperty this[string name, IType arg1, IType arg2, IType arg3]
        {
            get
            {
                return base[name,
                    m =>
                    {
                        var p = m.Parameters;
                        if (p.Count != 3) return false;
                        if (!Signature.TypeEquals(p[0].Type, arg1)) return false;
                        if (!Signature.TypeEquals(p[1].Type, arg2)) return false;
                        return Signature.TypeEquals(p[2].Type, arg3);
                    }];
            }
        }

        public IProperty this[string name, params IType[] types]
        {
            get
            {
                return base[name, m => Signature.Equals(m.Parameters, types)];
            }
        }
        #endregion

        #region ICodeNode Members
        public CodeNodeType NodeType
        {
            get { return CodeNodeType.Properties; }
        }

        public object Tag { get; set; }
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
}