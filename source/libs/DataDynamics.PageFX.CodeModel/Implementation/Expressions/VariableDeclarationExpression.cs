using System.Collections.Generic;

namespace DataDynamics.PageFX.CodeModel
{
    public class VariableDeclarationExpression : Expression, IVariableDeclarationExpression, ITypeReferenceProvider
    {
        #region IVariableDeclarationExpression Members
        public IVariable Variable
        {
            get { return _var; }
            set { _var = value; }
        }
        private IVariable _var;
        #endregion

        #region ICodeNode Members
        public override IEnumerable<ICodeNode> ChildNodes
        {
            get { return CMHelper.Enumerate(_var); }
        }
        #endregion

        #region IExpression Members
        public override IType ResultType
        {
            get { return _var.Type; }
        }
        #endregion

        #region Object Override Members
        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var e = obj as IVariableDeclarationExpression;
            if (e == null) return false;
            if (e.Variable != _var) return false;
            return true;
        }

        private static readonly int _hs = typeof(IVariableDeclarationExpression).GetHashCode();

        public override int GetHashCode()
        {
            if (_var != null)
                return _var.GetHashCode() ^ _hs;
            return base.GetHashCode();
        }
        #endregion

        #region ITypeReferenceProvider Members
        public IEnumerable<IType> GetTypeReferences()
        {
            if (_var != null)
                return new[] { _var.Type };
            return null;
        }
        #endregion
    }
}